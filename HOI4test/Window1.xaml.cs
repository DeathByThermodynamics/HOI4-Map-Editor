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
using System.IO;

namespace HOI4test
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Dictionary<string, Label> resourcenames;
        public Dictionary<string, TextBox> resourceboxes;
        public Window1(List<string> resources)
        {
            InitializeComponent();
            resourcenames = new Dictionary<string, Label>();
            resourceboxes = new Dictionary<string, TextBox>();
            for (var i = 0; i < resources.Count; i++)
            {
                AddResourceFunction(resources[i], "0");
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void AddResource(object sender, RoutedEventArgs e)
        {

            AddResourceFunction("Jack", "1");
        }

        public void AddResourceFunction(string name, string value)
        {
            var sorus = new TextBox();
            sorus.Height = 25;
            sorus.Width = 75;
            mainGrid.Children.Add(sorus);
            resourceboxes.Add(name, sorus);
            sorus.Margin = new Thickness(110, resourceboxes.Count * 75 - 260, 0, 0);
            sorus.Text = value;
            
            var label = new Label();
            
            label.Height = 25;
            label.Width = 75;
            label.Content = name;
            mainGrid.Children.Add(label);
            resourcenames.Add(name, label);
            label.Margin = new Thickness(-50, resourcenames.Count * 75 - 260, 0, 0);
            
        }

        public Dictionary<string, object> getResources()
        {
            var returndict = new Dictionary<string, object>();
            foreach (var i in resourceboxes.Keys)
            {
                //MessageBox.Show(i.ToString());
                returndict.Add(i, resourceboxes[i].Text);
            }
            //MessageBox.Show("uifhewi2");
            return returndict;
        }
    }
}
