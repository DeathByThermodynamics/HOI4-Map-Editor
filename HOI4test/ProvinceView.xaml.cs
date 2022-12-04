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
using System.Windows.Shapes;

namespace HOI4test
{
    /// <summary>
    /// Interaction logic for ProvinceView.xaml
    /// </summary>
    public partial class ProvinceView : Window
    {
        public Dictionary<string, Object> state;
        int province;
        MainWindow main;
        Window1 resourcewindow;
        BuildingView buildingwindow;
        List<string> resources;
        List<string> buildings;
        List<string> provinceBuildings;
        public ProvinceView(MainWindow main)
        {
            InitializeComponent();
            state = new Dictionary<string, Object>();
            province = 0;
            this.main = main;
            resources = new List<string>();
            buildings = new List<string>();
           
            defineResources();
            


        }

        public void defineResources()
        {
            string text = System.IO.File.ReadAllText(@starter.hoi4folder + "/common/resources/00_resources.txt");
            var splittext = text.Split("{");
            var icchan2 = splittext[0].Trim(' ').Trim('=');
            //MessageBox.Show(splittext.Length.ToString());
            if (icchan2.Trim(' ') == "resources")
            {
                //MessageBox.Show("icchan");
                for (var i = 1; i < splittext.Length; i++)
                {
                    
                    var splittext1 = splittext[i].Split("\n");
                    //MessageBox.Show("icchan");
                    var resource = splittext1[splittext1.Length - 1].Trim(' ').Trim('=').Trim();
                    //MessageBox.Show("icc" + resource);
                    if (resource != "")
                    {
                        resources.Add(resource);
                    }
                        
                }

            }

        }

        public bool checkStateCoastal()
        {
            foreach (var stateid in main.stateDict.Keys)
            {
                if (main.stateDict[stateid].Contains(province.ToString()))
                {
                    foreach (var prov in main.stateDict[stateid])
                    {
                        string coastal;
                        if (MainWindow.definition.ContainsKey(prov.ToString()))
                        {
                            coastal = MainWindow.definition[prov.ToString()][4].ToString();
                            //MessageBox.Show(coastal);
                        }
                        else
                        {
                            coastal = "false";
                        }
                        if (!(coastal == "false")) {
                            //MessageBox.Show("found");
                            return true;
                        }
                    }
                    return false;
                }
            }
            return false;
        }
        public void defineBuildings()
        {
            buildings = new List<string>();
            provinceBuildings = new List<string>();
            string text = System.IO.File.ReadAllText(@starter.hoi4folder + "/common/buildings/00_buildings.txt");
            var splittext = text.Split("{");
            var icchan2 = splittext[0].Trim(' ').Trim('=');
            string coastal;
            if (MainWindow.definition.ContainsKey(province.ToString()))
            {
               coastal = MainWindow.definition[province.ToString()][4].ToString();
            } else
            {
                coastal = "false";
            }
                    
            //MessageBox.Show(coastal);
            if (icchan2.Trim(' ') == "buildings")
            {
                //MessageBox.Show("icchan");
                for (var i = 1; i < splittext.Length-1; i++)
                {
                    
                    var prov = false;
                    var cancelled = false;
                    var splittext1 = splittext[i+1].Split("\n");
                    var splittext0 = splittext[i].Split("\n");
                    for (var k = 0; k < splittext1.Length; k++)
                    {
                        
                        splittext1[k] = splittext1[k].Trim();
                        if (splittext1[k].StartsWith("provincial") && splittext1[k].EndsWith("yes"))
                        {
                            prov = true;
                            
                        }
                        // Believe it or not, this is an intentional 'typo'
                        if (splittext1[k].StartsWith("only_costal") && splittext1[k].EndsWith("yes") && coastal == "false")
                        {
                            if (prov)
                            {
                                cancelled = true;
                            } else if (!checkStateCoastal())
                            {
                                cancelled = true;
                            }
                            
                        }

                    }
                    
                    //MessageBox.Show("icchan");
                    var resource = splittext0[splittext0.Length - 1].Trim(' ').Trim('=').Trim();
                    //MessageBox.Show("icc" + resource);
                    //MessageBox.Show(resource.ToString());
                    if (!cancelled)
                    {
                        if (prov && resource != "")
                        {
                            provinceBuildings.Add(resource);
                        }
                        else if (resource != "")
                        {
                            buildings.Add(resource);
                        }
                    }
                    

                }

            }

        }

        public void SaveThings(object sender, RoutedEventArgs e)
        {
            SaveThingsCalled();
        }

        public void SaveThingsCalled()
        {
            state["category"] = category.SelectedItem.GetType().GetProperty("Name").GetValue(category.SelectedItem, null).ToString();
            //MessageBox.Show(category.SelectedItem.GetType().GetProperty("Name").GetValue(category.SelectedItem, null).ToString());
            string old_owner;
            if (!state.ContainsKey("owner"))
            {
                old_owner = "NONE";
            }
            else
            {
                old_owner = (string)state["owner"];
            }

            state["owner"] = ProvOwner.Text;
            state["manpower"] = Manpower.Text;
            var corelist = Cores.Text.Trim(' ').Split(',');
            for (var i = 0; i < corelist.Length; i++)
            {
                corelist[i].Trim(' ');
                //MessageBox.Show(corelist[i]);
            }
            state["cores"] = corelist;
            if (state.ContainsKey("vp"))
            {
                var vpdict = (Dictionary<string, string>)state["vp"];

                if (vpdict.ContainsKey(province.ToString()))
                {
                    vpdict[province.ToString()] = VP.Text;
                }
                else
                {
                    vpdict.Add(province.ToString(), VP.Text);
                }

            }
            else
            {
                var vpdict = new Dictionary<string, string>();
                vpdict.Add(province.ToString(), VP.Text);
                state.Add("vp", vpdict);
            }
            if (resourcewindow != null)
            {
                if (!state.ContainsKey("resources"))
                {
                    var temp = resourcewindow.getResources();
                    //MessageBox.Show(temp.GetType().ToString());
                    state.Add("resources", temp);
                }
                else
                {
                    state["resources"] = resourcewindow.getResources();
                }
            }

            if (buildingwindow != null)
            {

                if (!state.ContainsKey("buildings"))
                {
                    var temp = buildingwindow.getBuildings(province.ToString());
                    //MessageBox.Show(temp.GetType().ToString());
                    state.Add("buildings", temp);
                }
                else
                {
                    var tempstatebuildings = buildingwindow.getBuildings(province.ToString());
                    var tempstate = (Dictionary<string, object>)state["buildings"];
                    for (var i = 0; i < tempstate.Keys.Count; i++)
                    {
                        var key = tempstate.Keys.ElementAt(i);
                        if (!tempstatebuildings.ContainsKey(key))
                        {
                            tempstatebuildings.Add(key, tempstate[key]);
                        }
                    }
                    state["buildings"] = tempstatebuildings;

                }
                //MessageBox.Show(state["buildings"].GetType().ToString());
            }

            sentToMain(old_owner);
        }

        public void sentToMain(string old_owner)
        {
            int newstateprov = 0;
            if (main.states.states.ContainsKey(int.Parse(state["id"].ToString()))) {
                main.states.changeState(state, old_owner, main);
            } else
            {
                main.states.addState(state);
                newstateprov = province;
            }
                
            
            var bruh = (string[]) state["provinces"];
            main.UpdateProvinces(bruh.ToList(), newstateprov);
            
            main.states.saveStates();
        }
        
        public void addProvince(int provinceid)
        {
            var provincedict = (string[]) state["provinces"];
            Array.Resize(ref provincedict, provincedict.Length + 1);
            provincedict[provincedict.Length - 1] = provinceid.ToString();
            state["provinces"] = provincedict;
            SaveThingsCalled();
        }
        public void updateWindow(Dictionary<string, Object> stateinfo, int provinceid)
        {
            state = stateinfo;
            province = provinceid;
            ProvinceID.Content = provinceid.ToString();
            StateID.Content = stateinfo["id"].ToString();
            
            if (stateinfo.ContainsKey("owner"))
            {
                ProvOwner.Text = stateinfo["owner"].ToString();
            } else
            {
                ProvOwner.Text = "";
            }
            
            if (stateinfo.ContainsKey("manpower"))
            {
                Manpower.Text = stateinfo["manpower"].ToString();
            } else
            {
                Manpower.Text = "0";
            }
            
            if (stateinfo.ContainsKey("cores"))
            {
                var corelist = (string[])stateinfo["cores"];
                string core_display = corelist[0];
                for (var i = 1; i < corelist.Length; i++)
                {
                    core_display += "," + corelist[i];
                }
                Cores.Text = core_display;
            }
            
            if (stateinfo.ContainsKey("category"))
            {
                switch (stateinfo["category"].ToString())
                {
                    case "wasteland":
                        category.SelectedItem = wasteland;
                        break;
                    case "enclave":
                        category.SelectedItem = enclave;
                        break;
                    case "small_island":
                        category.SelectedItem = small_island;
                        break;
                    case "tiny_island":
                        category.SelectedItem = tiny_island;
                        break;
                    case "pastoral":
                        category.SelectedItem = pastoral;
                        break;
                    case "town":
                        category.SelectedItem = town;
                        break;
                    case "large_town":
                        category.SelectedItem = large_town;
                        break;
                    case "rural":
                        category.SelectedItem = rural;
                        break;
                    case "city":
                        category.SelectedItem = city;
                        break;
                    case "large_city":
                        category.SelectedItem = large_city;
                        break;
                    case "metropolis":
                        category.SelectedItem = metropolis;
                        break;
                    case "megalopolis":
                        category.SelectedItem = megalopolis;
                        break;
                    default:
                        category.SelectedItem = wasteland;
                        break;

                }
            } else
            {
                category.SelectedItem = wasteland;
            }
            
            if (stateinfo.ContainsKey("vp"))
            {
                try
                {
                    var next = (Dictionary<string, string>)stateinfo["vp"];
                    VP.Text = next[provinceid.ToString()];
                } 
                catch (Exception e)
                {
                    VP.Text = "0";
                }
            } else
            {
                VP.Text = "0";
            }

            if (resourcewindow != null)
                {
                    resourcewindow.Close();
                }
            if (buildingwindow != null)
            {
                buildingwindow.Close();
            }

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void GoToResources(object sender, RoutedEventArgs e)
        {
            resourcewindow = new Window1(resources);
            if (state.ContainsKey("resources"))
            {
                foreach (var i in resources)
                {
                    var castmaster = (Dictionary<string, object>)state["resources"];

                    if (castmaster.Keys.Contains(i))
                    {
                        resourcewindow.resourceboxes[i].Text = castmaster[i].ToString();
                    }

                }
            }

            else
            {
               foreach (var i in resources)
                    {
                        resourcewindow.resourceboxes[i].Text = "0";
                    }
            }

            resourcewindow.Owner = this;
            resourcewindow.Show();
            
        }

        private void GoToBuildings(object sender, RoutedEventArgs e)
        {
            defineBuildings();
            buildingwindow = new BuildingView(buildings, provinceBuildings);
            foreach (var i in buildings)
            {
                buildingwindow.buildingboxes[i].Text = "0";
            }
            foreach (var i in provinceBuildings)
            {
                buildingwindow.provinceboxes[i].Text = "0";
            }
            if (state.ContainsKey("buildings"))
            {
                var castmaster = (Dictionary<string, object>)state["buildings"];
                foreach (var i in buildings)
                {
                    

                    if (castmaster.Keys.Contains(i))
                    {
                        buildingwindow.buildingboxes[i].Text = castmaster[i].ToString();
                    }

                }
                if (castmaster.Keys.Contains(province.ToString()))
                {
                    //MessageBox.Show(castmaster[province.ToString()].GetType().ToString());
                    foreach (var provbuild in (Dictionary<string, int>)castmaster[province.ToString()])
                    {
                        buildingwindow.provinceboxes[provbuild.Key].Text = provbuild.Value.ToString();
                    }
                }
            }
            buildingwindow.Owner = this;
            buildingwindow.Show();
        }

        private void CreateNewState(object sender, RoutedEventArgs e)
        {
            var stateinfo = new Dictionary<string, object>();
            stateinfo.Add("id", main.states.getStates().Count + 1);
            stateinfo.Add("provinces", new string[] { province.ToString() });
            stateinfo.Add("owner", ProvOwner.Text);
            stateinfo.Add("manpower", "0");
            state = stateinfo;
            updateWindow(stateinfo, province);
            sentToMain("NONE");
        }
    }
}
