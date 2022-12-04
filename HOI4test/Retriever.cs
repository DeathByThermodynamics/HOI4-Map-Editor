using HOI4test;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace hoi4test3
{
    class Retriever
    {
        public static Dictionary<int, List<string>> stateData;
        public static Dictionary<string, List<string>> definitionData;
        public static Dictionary<int, Dictionary<string, Object>> fullStateData;
        public static Dictionary<string, List<int>> countryColourData;

        public Retriever()
        {
            definitionData = GetProvinceData();
            stateData = GetStatetoProvinceData();
            fullStateData = GetFullStateData();
            countryColourData = GetColourData();
        }

        public Dictionary<string, List<string>> GetProvinceData()
        {
            /*
             * PROVINCE DEFINITION STRUCTURE
            0: provinceid
            1, 2, 3: rgb colours
            4: land / sea
            5: is_coastal
            6: terrain
            7: continent
             */

            Dictionary<string, List<string>> definition = new Dictionary<string, List<string>>();
            List<string> deftext = System.IO.File.ReadLines(starter.hoi4folder + "/map/definition.csv").ToList();
            foreach (var provincedef in deftext)
            {
                var defsplit = provincedef.Split(";");
                var deflist = defsplit.ToList();
                deflist.RemoveAt(0);
                definition.Add(defsplit[0], deflist);
            }
            return definition;
        }

        public Dictionary<int, List<string>> GetStatetoProvinceData()
        {
            string text = System.IO.File.ReadAllText(@starter.hoi4folder + "/mapEditor/statedata.txt");
            string[] newtext = text.Split(":");
            string[] provincesplit = newtext[1].Split("?");
            Dictionary<int, List<string>> stateDict = new Dictionary<int, List<string>>();
            for (var i = 0; i < int.Parse(newtext[0]); i++)
            {
                var idsplit = provincesplit[i].Split(";");
                var provinces = idsplit[1].Split(",");
                stateDict.Add(int.Parse(idsplit[0]), provinces.ToList());
            }
            return stateDict;
        }

        public Dictionary<string, List<int>> GetColourData()
        {
            Dictionary<string, List<int>> countryColour = new Dictionary<string, List<int>>();
            string text = System.IO.File.ReadAllText(@starter.hoi4folder + "/mapEditor/countrycolours.txt");
            var newtext = text.Split(":");
            var countrysplit = newtext[1].Split("?");
            countryColour = new Dictionary<string, List<int>>();
            for (var i = 0; i < int.Parse(newtext[0]); i++)
            {
                var tagsplit = countrysplit[i].Split(";");
                var coloursplit = tagsplit[1].Split(",");

                countryColour.Add(tagsplit[0], new List<int> { int.Parse(coloursplit[0]), int.Parse(coloursplit[1]), int.Parse(coloursplit[2]) });
            }
            return countryColour;
        }

        public Dictionary<int, Dictionary<string, Object>> GetFullStateData()
        {
            //countryColour = new Dictionary<string, List<int>>();
            string text = System.IO.File.ReadAllText(@starter.hoi4folder + "/mapEditor/statedatafull.txt");
            var newtext = text.Split(":");
            var statesplit = newtext[1].Split("?");
            Dictionary<int, Dictionary<string, Object>>  states = new Dictionary<int, Dictionary<string, Object>>();
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
                                    //MessageBox.Show(building_split[1]);
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
                                if (!resources_list.ContainsKey(resource_split[0]))
                                {
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
            return states;
        }
    }
}
