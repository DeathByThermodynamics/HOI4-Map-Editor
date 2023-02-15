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
using System.Diagnostics;

namespace HOI4test
{
    /// <summary>
    /// Interaction logic for starter.xaml
    /// </summary>
    public partial class starter : Window
    {
        MainWindow mainWindow;
        // In the new version, all the .txt save data will also be stored
        // in the player directory, under a folder named 'mapEditor'.
        // here, i.e. it would be C:/users/alexh/hoi4example/mapEditor.
        public static string hoi4folder = "C:/";
        public static string programfolder = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/hoi4";
        public static string outputfolder = "C:/";
        public starter()
        {
            InitializeComponent();
            StatusUpdater.Text = "On Standby";
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
        
        private void Updater(string data)
        {
            StatusUpdater.Text = data;

        }
        private void LoadFilesAsync()
        {
            hoi4folder = Inputs.Text;
            ProcessStartInfo start = new ProcessStartInfo();
            System.Console.WriteLine(starter.programfolder);
            start.FileName = starter.programfolder + "/dist/main/main.exe";
            start.Arguments = string.Format(" {0}", hoi4folder);
            start.UseShellExecute = false;
            start.CreateNoWindow = true;
            start.RedirectStandardOutput = true;

            Process process = new Process();
            /*
         * Exe Replacements - done 11/20
         * 
         */
            process.StartInfo = start;
            process.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
            {
                if (e.Data != null)
                {
                    Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() => Updater(e.Data)));
                    
                }
            });
            
            try
            {
                process.Start();
                process.BeginOutputReadLine();
            }
            catch (Exception et)
            {
                MessageBox.Show(et.ToString());
            }

        }
        private void LoadFiles(object sender, RoutedEventArgs e)
        {
            LoadFilesAsync();
        }
        
        

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
