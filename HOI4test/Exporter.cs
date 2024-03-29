﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
namespace HOI4test
{
    internal class Exporter
    {
        /*
        private void GetStateData()
        {
            string text = System.IO.File.ReadAllText(@"C:\Users\alexh\electron\backend\hoi4\statedata.txt");
            var newtext = text.Split(":");
            var provincesplit = newtext[1].Split("?");

            for (var i = 0; i < int.Parse(newtext[0]); i++)
            {
                var idsplit = provincesplit[i].Split(";");
                var provinces = idsplit[1].Split(",");
                stateDict.Add(int.Parse(idsplit[0]), provinces.ToList());
            }
            states = new States("C:/Users/alexh/electron/backend/hoi4/statedatafull.txt");
            states.getCountryColours("C:/Users/alexh/electron/backend/hoi4/countrycolours.txt");
        } */
        public void ExportStratRegions(Dictionary<string, List<string>> provinces, Dictionary<string, List<string>> data)
        {
            Directory.CreateDirectory(starter.outputfolder + "/strategicregions");
            int remove_line = 0;
            foreach(var i in data.Keys)
            {
                for (var k = 0; k < data[i].Count; k++)
                {
                    if (data[i][k].Trim().StartsWith("provinces"))
                    {
                        data[i].Insert(k + 1, "      " + JoinedProvinces(provinces[i]));
                        //data[i].Insert(k + 1, "      " + string.Join(" ", provinces[i]));
                        //data[i][k + 2] = "";
                        remove_line = k + 1;
                    }
                }

                saveFile("strategicregions/" + i + ".txt", data[i]);
                if (remove_line != 0)
                {
                    data[i].RemoveAt(remove_line);
                }
                
            }
        }

        private string JoinedProvinces(List<string> provinces)
        {
            string return_string = "";
            var templist = new List<string>();
            for (var i = 0; i < provinces.Count; i++)
            {
                return_string += " ";
                if (!templist.Contains(provinces[i]))
                {
                    return_string += provinces[i];
                    templist.Add(provinces[i]);
                }
                
            }
            return return_string;
        }
        
        public void ExportStateData(Dictionary<int, Dictionary<string, Object>> stateDict)
        {
            Directory.CreateDirectory(starter.outputfolder + "/states");
            var tab = "    ";
            List<string> nums = new List<string>() { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            foreach (var entry in stateDict.Keys)
            {
                
                var returnlist = new List<string>();
                returnlist.Add("state = {");
                if (stateDict[entry].ContainsKey("id"))
                {
                    returnlist.Add(tab + "id = " + stateDict[entry]["id"].ToString());
                    returnlist.Add(tab + "name = \"STATE_" + stateDict[entry]["id"].ToString() + "\"");
                } else
                {
                    continue;
                }
                //MessageBox.Show("1");
                if (stateDict[entry].ContainsKey("manpower"))
                {
                    returnlist.Add(tab + "manpower = " + stateDict[entry]["manpower"].ToString());
                } else
                {
                    returnlist.Add(tab + "manpower = 0");
                }
                //MessageBox.Show("2");
                if (stateDict[entry].ContainsKey("resources"))
                {
                    returnlist.Add(tab + "resources = {");
                    var resourcedict = (Dictionary<string, object>)stateDict[entry]["resources"];
                    for (var i = 0; i < resourcedict.Count; i++)
                    {
                        if (!(resourcedict.Values.ElementAt(i).ToString() == "") && !(resourcedict.Values.ElementAt(i).ToString() == "0"))
                        {
                            returnlist.Add(tab + tab + resourcedict.Keys.ElementAt(i).Trim() + " = " + resourcedict.Values.ElementAt(i).ToString());
                        }
                    }
                    returnlist.Add(tab + "}");
                }
                //MessageBox.Show("3");
                if (stateDict[entry].ContainsKey("category"))
                {
                    returnlist.Add(tab + "state_category = " + stateDict[entry]["category"].ToString());
                }
                //MessageBox.Show("4");
                returnlist.Add(tab + "history = {");
                if (stateDict[entry].ContainsKey("owner"))
                {
                    returnlist.Add(tab + tab + "owner = " + stateDict[entry]["owner"].ToString());
                }
                //MessageBox.Show("5");
                if (stateDict[entry].ContainsKey("vp"))
                {
                    var vpdict = (Dictionary<string, string>)stateDict[entry]["vp"];
                    for (var i = 0; i < vpdict.Count; i++)
                    {
                        if (vpdict.Values.ElementAt(i) != "0")
                        {
                            returnlist.Add(tab + tab + "victory_points = { " + vpdict.Keys.ElementAt(i) + " " + vpdict.Values.ElementAt(i) + " }");

                        }
                    }
                }
                //MessageBox.Show("6");
                if (stateDict[entry].ContainsKey("buildings"))
                {
                    returnlist.Add(tab + tab + "buildings = {");
                    var buildingdict = (Dictionary<string, object>)stateDict[entry]["buildings"];
                    //MessageBox.Show("6a");
                    for (var i = 0; i < buildingdict.Count; i++)
                    {
                        if (nums.Contains(buildingdict.Keys.ElementAt(i)[0].ToString()))
                        {
                            //MessageBox.Show("6b");
                            var provincedict = (Dictionary<string, int>)buildingdict[buildingdict.Keys.ElementAt(i)];
                            //MessageBox.Show("6c");
                            returnlist.Add(tab + tab + tab + buildingdict.Keys.ElementAt(i) + " = {");
                            for (var j = 0; j < provincedict.Count; j++)
                            {
                                if (provincedict.Values.ElementAt(j).ToString() != "0")
                                {
                                    returnlist.Add(tab + tab + tab + tab + provincedict.Keys.ElementAt(j) + " = " + provincedict.Values.ElementAt(j).ToString());
                                }
                                
                            }
                            returnlist.Add(tab + tab + tab + "}");
                        }
                        else
                        {
                            if (buildingdict.Values.ElementAt(i).ToString() != "0")
                            {
                                returnlist.Add(tab + tab + tab + buildingdict.Keys.ElementAt(i) + " = " + buildingdict.Values.ElementAt(i).ToString());
                            }
                                
                        }
                    }
                    returnlist.Add(tab + tab + "}");
                }
               // MessageBox.Show("7");
                if (stateDict[entry].ContainsKey("cores"))
                {
                    var corelist1 = (string[])stateDict[entry]["cores"];
                    var corelist = corelist1.ToList<string>();
                    for (var i = 0; i < corelist.Count; i++)
                    {
                        if (corelist[i] != "")
                        {
                            returnlist.Add(tab + tab + "add_core_of = " + corelist[i]);
                        }
                        
                    }
                }

                if (stateDict[entry].ContainsKey("claims"))
                {
                    var claimlist1 = (string[])stateDict[entry]["claims"];
                    var claimlist = claimlist1.ToList<string>();
                    for (var i = 0; i < claimlist.Count; i++)
                    {
                        if (claimlist[i] != "")
                        {
                            returnlist.Add(tab + tab + "add_claim_by = " + claimlist[i]);
                        }

                    }
                }

                //MessageBox.Show("8");
                returnlist.Add(tab + "}");
                if (stateDict[entry].ContainsKey("impassable"))
                {
                    if (stateDict[entry]["impassable"] == "yes")
                    {
                        returnlist.Add(tab + "impassable = yes");
                    }
                }
                if (stateDict[entry].ContainsKey("dmz"))
                {
                    if (stateDict[entry]["dmz"] == "yes")
                    {
                        returnlist.Add(tab + "set_demilitarized_zone = yes");
                    }
                }
                if (stateDict[entry].ContainsKey("provinces"))
                {
                    returnlist.Add(tab + "provinces = {");
                    //MessageBox.Show(stateDict[entry]["provinces"].GetType().ToString());
                    var provincelist1 = (string[])stateDict[entry]["provinces"];
                    var provincelist = provincelist1.ToList<string>();
                    var identity = "";
                    for (var i = 0; i < provincelist.Count; i++)
                    {
                        identity += " " + provincelist[i];
                    }
                    returnlist.Add(tab + tab + identity);
                    returnlist.Add(tab + "}");
                } else
                {
                    continue;
                }
                //MessageBox.Show("9");
                returnlist.Add("local_supplies = 0.0");
                returnlist.Add("}");
                saveFile("states/" + stateDict[entry]["id"].ToString() + ".txt", returnlist);
                //MessageBox.Show("0");
            }
        }

        public void saveFile(string directory, List<string> text)
        {
            if (File.Exists(@starter.outputfolder + "/" + directory))
            {
                File.Delete(@starter.outputfolder + "/" + directory);
            }

            File.WriteAllLines(@starter.outputfolder + "/" + directory, text);
        }
    }

    

    
        

}
