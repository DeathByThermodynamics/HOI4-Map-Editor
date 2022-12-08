using HOI4test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace hoi4test3.InfoWindows
{
    internal class BuildingsView
    {
        MainWindow main;
        public BuildingsView(MainWindow main)
        {
            this.main = main;
        }
        
        public Dictionary<string, object> getWindowData(List<string> stateBuildings, List<string> provinceBuildings, int province)
        {
            Dictionary<string, object> buildingsdict = new Dictionary<string, object>();
            foreach (UIElement element in main.BuildingGrid.Children)
            {
                if (element is TextBox)
                {
                    var textbox = (TextBox)element;
                    if (int.Parse(textbox.Text) != 0)
                    {
                        if (stateBuildings.Contains(textbox.Name))
                        {
                            buildingsdict.Add(textbox.Name, textbox.Text);
                            //MessageBox.Show(textbox.Name);
                        }
                        else if (provinceBuildings.Contains(textbox.Name))
                        {
                            if (!buildingsdict.ContainsKey(province.ToString()))
                            {
                                buildingsdict.Add(province.ToString(), new Dictionary<string, int>());
                            }
                            ((Dictionary<string, int>)buildingsdict[province.ToString()]).Add(textbox.Name, int.Parse(textbox.Text));
                            //MessageBox.Show(textbox.Name);
                            //MessageBox.Show(textbox.Text);
                        }
                        
                    }
                }
            }
            
            return buildingsdict;
        }

        public void updateWindow(Dictionary<string, Object> stateinfo, int province) {
            Dictionary<string, object> buildingslist;
            if (stateinfo.ContainsKey("buildings"))
            {
                buildingslist = (Dictionary<string, object>)stateinfo["buildings"];
            }
            else
            {
                buildingslist = new Dictionary<string, object>();
            }

            Dictionary<string, int> provincebuildings = new Dictionary<string, int>();
            if (buildingslist.ContainsKey(province.ToString()))
            {
                provincebuildings = (Dictionary<string, int>)buildingslist[province.ToString()];
            }
            
            foreach (UIElement element in main.BuildingGrid.Children)
            {

                if (element is TextBox)
                {
                    var textbox = (TextBox)element;
                    main.updateUIContent(element, "0");
                    if (buildingslist.Keys.Contains(textbox.Name))
                    {
                        main.updateUIContent(element, buildingslist[textbox.Name].ToString());

                    }
                    else if (provincebuildings.Keys.Contains(textbox.Name))
                    {
                        main.updateUIContent(element, provincebuildings[textbox.Name].ToString());
                    }
                }

            }
        }

        public void drawBuildings(List<string> stateBuildings, List<string> provinceBuildings)
        {
            main.BuildingGrid.Children.Clear();
            int row = 1;
            int col = 1;
            foreach (string building in stateBuildings)
            {
                Label label = new Label();
                label.Content = building;
                TextBox textbox = new TextBox();
                Viewbox viewbox = new Viewbox();
                textbox.Name = building;
                viewbox.Child = label;
                textbox.PreviewTextInput += main.TextBox_CheckNumbersOnly;
                main.alignOnGrid(main.BuildingGrid, viewbox, row, col);
                main.alignOnGrid(main.BuildingGrid, textbox, row, col + 2);

                row += 2;
                if (row > 15)
                {
                    row = 1;
                    col += 4;
                }

            }
            row = 1;
            col = 9;
            foreach (string building in provinceBuildings)
            {
                Label label = new Label();
                label.Content = building;
                TextBox textbox = new TextBox();
                Viewbox viewbox = new Viewbox();
                textbox.Name = building;
                viewbox.Child = label;
                textbox.PreviewTextInput += main.TextBox_CheckNumbersOnly;
                main.alignOnGrid(main.BuildingGrid, viewbox, row, col);
                main.alignOnGrid(main.BuildingGrid, textbox, row, col + 2);

                row += 2;
                if (row > 15)
                {
                    row = 1;
                    col += 4;
                }

            }

        }
        private bool checkStateCoastal(int province)
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
                        if (!(coastal == "false"))
                        {
                            //MessageBox.Show("found");
                            return true;
                        }
                    }
                    return false;
                }
            }
            return false;
        }

        public Tuple<List<string>, List<string>> defineBuildings(int province)
        {
            List<string> buildings = new List<string>();
            List<string> provinceBuildings = new List<string>();
            string text = System.IO.File.ReadAllText(@starter.hoi4folder + "/common/buildings/00_buildings.txt");
            var splittext = text.Split("{");
            var icchan2 = splittext[0].Trim(' ').Trim('=');
            string coastal;
            if (MainWindow.definition.ContainsKey(province.ToString()))
            {
                coastal = MainWindow.definition[province.ToString()][4].ToString();
            }
            else
            {
                coastal = "false";
            }

            //MessageBox.Show(coastal);
            if (icchan2.Trim(' ') == "buildings")
            {
                //MessageBox.Show("icchan");
                for (var i = 1; i < splittext.Length - 1; i++)
                {

                    var prov = false;
                    var cancelled = false;
                    var splittext1 = splittext[i + 1].Split("\n");
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
                            }
                            else if (!checkStateCoastal(province))
                            {
                                cancelled = true;
                            }

                        }

                    }

                    //MessageBox.Show("icchan");
                    var building = splittext0[splittext0.Length - 1].Trim(' ').Trim('=').Trim();
                    //MessageBox.Show("icc" + resource);
                    //MessageBox.Show(resource.ToString());
                    if (!cancelled)
                    {
                        if (prov && building != "")
                        {
                            provinceBuildings.Add(building);
                        }
                        else if (building != "")
                        {
                            buildings.Add(building);
                        }
                    }


                }

            }
            if (MainWindow.definition.ContainsKey(province.ToString()))
            {
                if (MainWindow.definition[province.ToString()][3].ToString() == "lake")
                {
                    provinceBuildings = new List<string>();
                }
                
            }
            return Tuple.Create(buildings, provinceBuildings);

        }
    }
}
