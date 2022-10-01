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
using System.Reflection;

namespace HOI4test
{
    /// <summary>
    /// Interaction logic for starter.xaml
    /// </summary>
    public partial class starter : Window
    {
        MainWindow mainWindow;
        public static string hoi4folder = "C:/Users/alexh/hoi4example";
        public static string programfolder = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/hoi4";
        public static string outputfolder = "C:/Users/alexh/hoi4example/statetest";
        public starter()
        {
            InitializeComponent();
            //MessageBox.Show(programfolder);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StartProgram(object sender, RoutedEventArgs e)
        {
            hoi4folder = Inputs.Text;
            outputfolder = Outputs.Text;
            mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
