using HOI4test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace hoi4test3.InfoWindows
{
    internal class ResourcesView
    {
        MainWindow main;
        List<string> resources;

        public ResourcesView(MainWindow main)
        {
            this.main = main;
            defineResources();
            drawResources();
        }

        public Dictionary<string, object> getWindowData()
        {
            Dictionary<string, object> resourcesdict = new Dictionary<string, object>();
            foreach (UIElement element in main.ResourcesGrid.Children)
            {
                if (element is TextBox)
                {
                    var textbox = (TextBox)element;
                    if (int.Parse(textbox.Text) != 0)
                    {
                        resourcesdict.Add(textbox.Name, textbox.Text);
                    }
                }
            }
            return resourcesdict;
        }

        public void updateWindow(Dictionary<string, Object> stateinfo)
        {
            Dictionary<string, object> resourcelist; 
            if (stateinfo.ContainsKey("resources"))
            {
                resourcelist = (Dictionary<string, object>)stateinfo["resources"];
            }
            else
            {
                resourcelist = new Dictionary<string, object>();
            }
            foreach (var resource in resources)
            {
                foreach (UIElement element in main.ResourcesGrid.Children)
                {
                   
                    if (element is TextBox)
                    {
                        var textbox = (TextBox)element;
                        if (textbox.Name == resource)
                        {
                            if (resourcelist.Keys.Contains(resource))
                            {
                                main.updateUIContent(element, resourcelist[resource].ToString());
                            }
                            else
                            {
                                main.updateUIContent(element, "0");
                            }

                        }
                    }   
                    
                }
            }
        }
            
        public void defineResources()
        {
            string text = System.IO.File.ReadAllText(@starter.hoi4folder + "/common/resources/00_resources.txt");
            string[] splittext = text.Split("{");
            string icchan2 = splittext[0].Trim(' ').Trim('=');
            resources = new List<string>();
            //MessageBox.Show(splittext.Length.ToString());
            if (icchan2.Trim(' ') == "resources")
            {
                //MessageBox.Show("icchan");
                for (var i = 1; i < splittext.Length; i++)
                {

                    string[] splittext1 = splittext[i].Split("\n");
                    //MessageBox.Show("icchan");
                    string resource = splittext1[splittext1.Length - 1].Trim(' ').Trim('=').Trim();
                    //MessageBox.Show("icc" + resource);
                    if (resource != "")
                    {
                        resources.Add(resource);
                    }

                }

            }
        }

        public void drawResources()
        {
            //main.ResourcesGrid.Children.Clear();
            int row = 1;
            int col = 1;
            foreach (string resource in resources)
            {
                Label label = new Label();
                label.Content = resource;
                TextBox textbox = new TextBox();
                textbox.Name = resource;
                textbox.PreviewTextInput += main.TextBox_CheckNumbersOnly;
                main.alignOnGrid(main.ResourcesGrid, label, row, col);
                main.alignOnGrid(main.ResourcesGrid, textbox, row, col + 2);

                row += 2;
                if (row > 11)
                {
                    row = 1;
                    col += 4;
                }

            }

        }

        
    }
}
