using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Diagnostics;
using System.IO;
using HOI4test;

/*
 * States is contained in a list / dictionary of dictionaries.
 * Each individual state dictionary should contain:
 * - State ID (for backup purposes) (probably not)
 * - Name               (not yet)
 * - Provinces          (provinces)
 * - Owner              (owner)
 * - Buildings          (buildings)
 * - Victory Points     (vp)
 * - Manpower           (manpower)
 * - State Category     (state_category)
 * - Cores              (cores)
 * - Claims?
 * - Resources          (resources)
 */
public class States
{
    public Dictionary<int, Dictionary<string, Object>> states;
    public Dictionary<string, List<int>> countryColour;

    public void getCountryColours(string directory)
    {
        string text = System.IO.File.ReadAllText(@directory);
        var newtext = text.Split(":");
        var countrysplit = newtext[1].Split("?");
        countryColour = new Dictionary<string, List<int>>();
        for (var i = 0; i < int.Parse(newtext[0]); i++)
        {
            var tagsplit = countrysplit[i].Split(";");
            var coloursplit = tagsplit[1].Split(",");

            countryColour.Add(tagsplit[0], new List<int> { int.Parse(coloursplit[0]), int.Parse(coloursplit[1]), int.Parse(coloursplit[2]) });
        }
    }
    
    public void changeState(Dictionary<string, Object> stateinfo, string old_owner, MainWindow main)
    {

        var owner = states[int.Parse(stateinfo["id"].ToString())]["owner"];
        states[int.Parse(stateinfo["id"].ToString())] = stateinfo;
        var new_owner = states[int.Parse(stateinfo["id"].ToString())]["owner"];
        var colour = countryColour[(string) new_owner];
        var colourstring = colour[0].ToString() + "," + colour[1].ToString() + "," + colour[2].ToString();
        if (!old_owner.Equals(new_owner))
        {
           // MessageBox.Show("changing state");
            //MessageBox.Show("lmao");
            var thiw = (string[]) states[int.Parse(stateinfo["id"].ToString())]["provinces"];
            var newlist = thiw.ToList<string>();
            RepaintProvinces(newlist, colourstring, main);
        }
    }

    public void addState(Dictionary<string, Object> stateinfo)
    {
        var provinceid = ((string[])stateinfo["provinces"])[0];
        int oldstate = 0;
        foreach (var state in states.Values)
        {
            if (((string[])state["provinces"]).Contains(provinceid))
            {
                oldstate = int.Parse(state["id"].ToString());
            }
        }
        var oldprovs = ((string[])states[oldstate]["provinces"]).ToList();
        oldprovs.Remove(provinceid.ToString());
        states[oldstate]["provinces"] = oldprovs.ToArray();
        states.Add(int.Parse(stateinfo["id"].ToString()), stateinfo);
        var selectedstate = int.Parse(stateinfo["id"].ToString());

        if (states[oldstate].ContainsKey("buildings"))
        {
            var oldbuildings = (Dictionary<string, Object>)states[oldstate]["buildings"];
            if (oldbuildings.ContainsKey(provinceid.ToString()))
            {
                var provinceinfo = oldbuildings[provinceid.ToString()];
                if (!states[selectedstate].ContainsKey("buildings"))
                {
                    states[selectedstate].Add("buildings", new Dictionary<string, Object>());
                }
                var newbuildings = (Dictionary<string, Object>)states[selectedstate]["buildings"];
                newbuildings.Add(provinceid.ToString(), provinceinfo);
            }
        }

        if (states[oldstate].ContainsKey("vp"))
        {
            var oldvps = (Dictionary<string, string>)states[oldstate]["vp"];
            if (oldvps.ContainsKey(provinceid.ToString()))
            {
                var provinceinfo = oldvps[provinceid.ToString()];
                if (!states[selectedstate].ContainsKey("vp"))
                {
                    states[selectedstate].Add("vp", new Dictionary<string, string>());
                }
                var newvps = (Dictionary<string, string>)states[selectedstate]["vp"];
                newvps.Add(provinceid.ToString(), provinceinfo);
            }
        }
    }

    public void RepaintProvinces(List<string> provinces, string colour, MainWindow main)
    {
        for (var i = 0; i < provinces.Count; i++)
        {
            if (!main.changedprovinces.Contains(provinces[i]))
            {
                main.changedprovinces.Add(provinces[i]);
                main.changedcolours.Add(colour);
            } else
            {
                int index = main.changedprovinces.IndexOf(provinces[i]);
                main.changedcolours[index] = colour;
            }
        }
        
        ProcessStartInfo start = new ProcessStartInfo();
        string provincestring = "";
        foreach (var provincenum in provinces) {
            if (provincenum != "")
            {
                provincestring += provincenum + ",";
            }
            
        }
        /*
         * Exe replacements - done 11/20
         * 
         */
        start.FileName = starter.programfolder + "/dist/maprepainter/maprepainter.exe";
        //MessageBox.Show(string.Format("C:/Users/alexh/electron/backend/hoi4/maprepainter.py {0} {1}", provincestring, colour));
        var directory = starter.hoi4folder;
        start.Arguments = string.Format(" {0} {1} {2} temp", provincestring, colour, directory);
        start.UseShellExecute = false;
        start.CreateNoWindow = true;
        start.RedirectStandardOutput = true;
        //MessageBox.Show("bruh");
        try {
            using (Process process = Process.Start(start))
            {

                using (StreamReader reader = process.StandardOutput)
                {
                    while (!process.StandardOutput.EndOfStream)
                    {
                        string line = process.StandardOutput.ReadLine();
                        //MessageBox.Show(line);
                    }
                }

            }
        }
        catch (Exception e)
        {
            MessageBox.Show(e.ToString());
        }

        main.AddtoMap(provinces);

    }

    public void transferProvince(int selectedstate, int provinceid, int oldstate)
    {
        var oldprovs = ((string[])states[oldstate]["provinces"]).ToList();
        var newprovs = ((string[])states[selectedstate]["provinces"]).ToList();

        oldprovs.Remove(provinceid.ToString());
        states[oldstate]["provinces"] = oldprovs.ToArray();
        newprovs.Add(provinceid.ToString());
        states[selectedstate]["provinces"] = newprovs.ToArray();

        if (states[oldstate].ContainsKey("buildings"))
        {
            var oldbuildings = (Dictionary<string, Object>)states[oldstate]["buildings"];
            if (oldbuildings.ContainsKey(provinceid.ToString()))
            {
                var provinceinfo = oldbuildings[provinceid.ToString()];
                if (!states[selectedstate].ContainsKey("buildings"))
                {
                    states[selectedstate].Add("buildings", new Dictionary<string, Object>());
                }
                var newbuildings = (Dictionary<string, Object>)states[selectedstate]["buildings"];
                newbuildings.Add(provinceid.ToString(), provinceinfo);
                oldbuildings.Remove(provinceid.ToString());
            }
        }

        if (states[oldstate].ContainsKey("vp"))
        {
            var oldvps = (Dictionary<string, string>)states[oldstate]["vp"];
            if (oldvps.ContainsKey(provinceid.ToString()))
            {
                var provinceinfo = oldvps[provinceid.ToString()];
                if (!states[selectedstate].ContainsKey("vp"))
                {
                    states[selectedstate].Add("vp", new Dictionary<string, string>());
                }
                var newvps = (Dictionary<string, string>)states[selectedstate]["vp"];
                newvps.Add(provinceid.ToString(), provinceinfo);
                oldvps.Remove(provinceid.ToString());
            }
        }

    }

    public async void saveStates()
    {
        
        var next = Saver.saveData(states);
        string[] returnstring = next.ToArray();
        File.Delete(@starter.hoi4folder + "/mapEditor/statedatafull.txt");
        await File.WriteAllLinesAsync(@starter.hoi4folder + "/mapEditor/statedatafull.txt", returnstring);
    }
    public States(string directory)
    {
        countryColour = new Dictionary<string, List<int>>();
        string text = System.IO.File.ReadAllText(@directory);
        var newtext = text.Split(":");
        var statesplit = newtext[1].Split("?");
        states = new Dictionary<int, Dictionary<string, Object>>();
        for (var i = 0; i < int.Parse(newtext[0]); i++)
        {
            //MessageBox.Show(statesplit[i]);
            var idsplit = statesplit[i].TrimEnd('\n').Split(";");
            var infosplit = idsplit[1].Split("$");
            states.Add(int.Parse(idsplit[0]), new Dictionary<string, Object>());
            int placer = int.Parse(idsplit[0]);
            states[placer].Add("id", int.Parse(idsplit[0]));
            foreach (var entry in infosplit)
            {
                if (entry != "")
                {
                    

                    var name = entry.Split(">")[0];

                    var entries = entry.Split(">")[1];


                    if (name == "manpower" || name == "owner" || name == "category")
                    {
                        states[placer].Add(name, entries);
                    }

                    else if (name == "provinces")
                    {
                        var provinces = entries.Split("!");
                        states[placer].Add(name, provinces);
                    }

                    else if (name == "buildings")
                    {
                        var buildings = entries.Split("!");
                        var building_list = new Dictionary<string, Object>();
                        for (var k = 0; k < buildings.Length; k++)
                        {
                            if (buildings[k] == "")
                            {
                                continue;
                            }
                            var building_split = buildings[k].Split("#");
                            if (building_split[0].StartsWith("@"))
                            {
                                var province_split = building_split[0].Split("@")[1];
                                var provincebuildings = building_split[1].Split("+");
                                var provincebuildingDict = new Dictionary<string, int>();
                                for (var l = 0; l < provincebuildings.Length; l++)
                                {
                                    if (provincebuildings[l] != "")
                                    {
                                        var building = provincebuildings[l].Split("<");
                                        provincebuildingDict.Add(building[0], int.Parse(building[1]));

                                    }
                                }
                                building_list.Add(province_split, provincebuildingDict);
                            }
                            else
                            {
                                building_list.Add(building_split[0], int.Parse(building_split[1]));
                            }
                        }
                        states[placer].Add(name, building_list);
                    }

                    else if (name == "vp")
                    {
                        var vp = entries.Split("!");
                        var vp_list = new Dictionary<string, string>();

                        for (var k = 0; k < vp.Length; k++)
                        {
                            if (vp[k] == "" || !vp[k].Contains("#"))
                           {
                               continue;
                           }
                           var vp_split = vp[k].Split("#");
                           vp_list.Add(vp_split[0], vp_split[1]);
                       }
                       states[placer].Add(name, vp_list);
                   }
                   else if (name == "cores")
                   {
                       var cores = entries.Split("!");
                       states[placer].Add(name, cores);
                   }

                   else if (name == "resources")
                   {
                       var resources = entries.Split("!");
                       var resources_list = new Dictionary<string, object>();
                       for (var k = 0; k < resources.Length; k++)
                       {
                            if (resources[k] == "" || !resources[k].Contains("#"))
                            {
                                continue;
                            }
                            var resource_split = resources[k].Split("#");
                            var uwu = resource_split[0];
                            if (!resources_list.ContainsKey(resource_split[0])) {
                               resources_list.Add(resource_split[0], resource_split[1]);
                            }
                            
                       }
                       states[placer].Add(name, resources_list);
                   }
                   else
                   {
                       Debug.WriteLine("Error: " + name);
                   }

                }



            }

        }
    }

    public Dictionary<int, Dictionary<string, Object>> getStates()
    {
        //MessageBox.Show(states.Keys.Count.ToString());
        return states;
    }
}
