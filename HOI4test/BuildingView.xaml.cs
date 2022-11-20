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
    /// Interaction logic for BuildingView.xaml
    /// </summary>
    public partial class BuildingView : Window
    {
        public Dictionary<string, Label> buildingnames;
        public Dictionary<string, TextBox> buildingboxes;
        public Dictionary<string, Label> provincenames;
        public Dictionary<string, TextBox> provinceboxes;
        string province;
        public BuildingView(List<string> buildings, List<string> provinceBuildings)
        {
            InitializeComponent();
            buildingnames = new Dictionary<string, Label>();
            buildingboxes = new Dictionary<string, TextBox>();
            provincenames = new Dictionary<string, Label>();
            provinceboxes = new Dictionary<string, TextBox>();
            for (var i = 0; i < buildings.Count; i++)
            {
                AddBuildingFunction(buildings[i], "0", -260, buildingboxes, buildingnames);
            }

            for (var i = 0; i < provinceBuildings.Count; i++)
            {
                AddBuildingFunction(provinceBuildings[i], "0", 100, provinceboxes, provincenames);
            }
        }

        public void AddBuildingFunction(string name, string value, int leftmargin, Dictionary<string, TextBox> boxes, Dictionary<string, Label> names)
        {
            var sorus = new TextBox();
            sorus.Height = 25;
            sorus.Width = 35;
            mainGrid.Children.Add(sorus);
            boxes.Add(name, sorus);
            sorus.Margin = new Thickness(leftmargin + 180, boxes.Count * 75 - 460, 0, 0);
            sorus.Text = value;
            //MessageBox.Show(name);

            var label = new Label();

            label.Height = 25;
            label.Width = 120;
            label.Content = name;
            mainGrid.Children.Add(label);
            names.Add(name, label);
            label.Margin = new Thickness(leftmargin, names.Count * 75 - 460, 0, 0);

        }

        public Dictionary<string, object> getBuildings(string province)
        {
            var returndict = new Dictionary<string, object>();
            foreach (var i in buildingboxes.Keys)
            {
                //MessageBox.Show(i.ToString());
                returndict.Add(i, buildingboxes[i].Text);
            }

            var provdict = new Dictionary<string, int>();
            foreach (var i in provinceboxes.Keys)
            {
                provdict.Add(i, int.Parse(provinceboxes[i].Text.Trim()));
            }
            returndict.Add(province, provdict);
            //MessageBox.Show("uifhewi2");
            return returndict;
        }
    }
}
