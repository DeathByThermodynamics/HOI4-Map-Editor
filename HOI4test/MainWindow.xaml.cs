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

namespace HOI4test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Point displacementFromMouse;
        double scale = 1;
        double scaleMap = 1;
        double x_lim = -4455;
        double y_lim = -1547;
        double x_change1;
        double y_change1;
        double x_change2;
        double y_change2;
        ProvinceView provincewindow;
        public States states;
        Dictionary<int,List<string>> stateDict = new Dictionary<int, List<string>>();
        Dictionary<int, UIElement> provinceDict;
        public static Dictionary<string, List<string>> definition;
        int selectedState = 0;
        bool opened_map = false;
        public MainWindow()
        {
            InitializeComponent();
            provincewindow = new ProvinceView(this);
            x_change1 = +((x_lim - x_lim * scale) / 2);
            y_change1 = +((y_lim - y_lim * scale) / 2);
            x_change2 = x_change1;
            y_change2 = y_change1;
            definition = new Dictionary<string, List<string>>();
        }

        public void Exit(object sender, RoutedEventArgs e)
        {
            if (opened_map)
            {
                states.saveStates();
            }
            
            System.Windows.Application.Current.Shutdown();
        }

        private void GetStateData()
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
            string text = System.IO.File.ReadAllText(starter.programfolder + "/statedata.txt");
            var newtext = text.Split(":");
            var provincesplit = newtext[1].Split("?");
            
            for (var i = 0; i < int.Parse(newtext[0]); i++)
            {
                var idsplit = provincesplit[i].Split(";");
                var provinces = idsplit[1].Split(",");
                stateDict.Add(int.Parse(idsplit[0]), provinces.ToList());
            }

            List<string> deftext = System.IO.File.ReadLines(starter.hoi4folder + "/map/definition.csv").ToList();
            foreach (var provincedef in deftext)
            {
                var defsplit = provincedef.Split(";");
                var deflist = defsplit.ToList();
                deflist.RemoveAt(0);
                definition.Add(defsplit[0], deflist);
            }
            states = new States(starter.programfolder + "/statedatafull.txt");
            stateDict = new Dictionary<int, List<string>>();
            foreach (var state in states.states)
            {
                var tempProvinces = (string[]) state.Value["provinces"];
                stateDict.Add(state.Key, tempProvinces.ToList());
            }
            states.getCountryColours(starter.programfolder + "/countrycolours.txt");
        }
        
        public void UpdateProvinces(List<string> provinces)
        {
            foreach (var province in provinces)
            {
                if (province != "")
                {
                    var newImage = (Image)provinceDict[int.Parse(province.ToString())];
                    newImage.Source = LoadBitmapImage(starter.hoi4folder + "/provinces/" + province + ".png");
                    //MessageBox.Show(province.ToString());
                }

            }
            
        }

        public BitmapImage LoadBitmapImage(string fileName)
        {
            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                return bitmapImage;
            }
        }
        private void GenerateMap(object sender, RoutedEventArgs e)
        {
            provincewindow.Show();
            provinceDict = new Dictionary<int, UIElement>();
            GetStateData();
            Console.WriteLine("lmao");
            // Create image.
            txtName.Clear();
            Image newImage = new Image();
            newImage.Source = LoadBitmapImage(starter.hoi4folder + "/provinces/borders.png");
            // Create Point for upper-left corner of image.
            grid1.Width = newImage.Width;
            grid1.Height = newImage.Height;
            System.Console.Write(newImage.Height);
            System.Console.Write(newImage.Width);
            //map.Width = newImage.Width;
            //map.Height = newImage.Height;
            

            // Draw image to screen.

            string text = System.IO.File.ReadAllText(@starter.programfolder + "/provincepos.txt");
            //MessageBox.Show(starter.programfolder + "/provincepos.txt");
            var newtext = text.Split(";");
            var provincesplit = newtext[1].Split("?");
            /*
            for (var i = 0; i < int.Parse(newtext[0]); i++)
            {
                var provincedata = provincesplit[i].Split(":");
                var provinceid = provincedata[0];
                var xcord = provincedata[1].Split(",")[0];
                var ycord = provincedata[1].Split(",")[1];

                Image tempImage = new Image();
                tempImage.Source = new BitmapImage(new Uri("file:///C:/users/alexh/hoi4example/provinces/" + provinceid + ".png"));
                

                map.Children.Add(tempImage);
                Canvas.SetLeft(tempImage, int.Parse(ycord));
                Canvas.SetTop(tempImage, int.Parse(xcord));
            } */
            
            map.Children.Add(newImage);

            for (var i = 0; i < int.Parse(newtext[0]); i++)
            {
                var provincedata = provincesplit[i].Split(":");
                var provinceid = provincedata[0];
                var xcord = provincedata[1].Split(",")[0];
                var ycord = provincedata[1].Split(",")[1];
                
                Image tempImage = new Image();
                tempImage.Source = LoadBitmapImage(starter.hoi4folder + "/provinces/" + provinceid + ".png");
                tempImage.Style = (Style)FindResource("ProvinceStyle");
                tempImage.Opacity = 0.7;
                tempImage.MouseLeftButtonDown += new MouseButtonEventHandler(HandleClickProvince);
                //tempImage.Name = provinceid;
                map.Children.Add(tempImage);
                provinceDict.Add(int.Parse(provinceid), tempImage);
                Canvas.SetLeft(tempImage, int.Parse(ycord));
                Canvas.SetTop(tempImage, int.Parse(xcord));
            }


        }
        
        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var matTrans = backGrid2.RenderTransform as MatrixTransform;
            var pos1 = e.GetPosition(backGrid1);

            var scale1 = e.Delta > 0 ? 1.1 : 1 / 1.1;

            var mat = matTrans.Matrix;
            //mat.ScaleAt(scale1, scale1, pos1.X, pos1.Y);'
            var new_scale = scale1 * scaleMap;
            if (new_scale < 0.5)
            {
                scale1 = 0.5 / scaleMap;
                scaleMap = 0.5;
            } else
            {
                scaleMap = scale1 * scaleMap;
            }
            mat.ScaleAt(scale1, scale1, pos1.X, pos1.Y);
            matTrans.Matrix = mat;
            e.Handled = true;
            
            
            //x_change2 = -1 * ((x_lim - x_lim * scale)) * (pos1.X / x_lim);
            //x_change1 = -1 * ((x_lim - x_lim * scale)) * (1 - pos1.X / x_lim);
            //y_change2 = -1 * ((y_lim - y_lim * scale)) * (pos1.Y / y_lim);
            //y_change1 = -1 * ((y_lim - y_lim * scale)) * (1 - pos1.Y / y_lim);

            /*
            if (Canvas.GetLeft(MapPanel) < x_lim + x_change)
            {
                Canvas.SetLeft(MapPanel, x_lim + x_change);
            }
            else if (Canvas.GetLeft(MapPanel) > 0)
            {
                Canvas.SetLeft(MapPanel, 0 + x_change);
            }


            if (Canvas.GetTop(MapPanel) < y_lim + y_change)
            {
                Canvas.SetTop(MapPanel, y_lim + y_change);
            }
            else if (Canvas.GetTop(MapPanel) > 0 + y_change)
            {
                Canvas.SetTop(MapPanel, 0 + y_change);
            } */

            displacementFromMouse = e.GetPosition(MapPanel);

        }

        private void HandleDragDrop(object sender, MouseEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(MapPanel, MapPanel, DragDropEffects.Move);
            }
            
        }

        private void Canvas_Drop(object sender, DragEventArgs e)
        {
            Point dropPosition = e.GetPosition(BackCanvas);
            displacementFromMouse = new Point(displacementFromMouse.X / scale, displacementFromMouse.Y / scale);
            
            if (dropPosition.X - displacementFromMouse.X < x_lim + x_change1)
            {
                Canvas.SetLeft(MapPanel, x_lim + x_change1);
            } else if (dropPosition.X - displacementFromMouse.X > 0 + x_change2)
            {
                Canvas.SetLeft(MapPanel, 0 + x_change2);
            } else
            {
                Canvas.SetLeft(MapPanel, dropPosition.X - displacementFromMouse.X);
            }


            if (dropPosition.Y - displacementFromMouse.Y < y_lim + y_change1)
            {
                Canvas.SetTop(MapPanel, y_lim + y_change1);
            }
            else if (dropPosition.Y - displacementFromMouse.Y > 0 + y_change2)
            {
                Canvas.SetTop(MapPanel, 0 + y_change2);
            }
            else
            {
                Canvas.SetTop(MapPanel, dropPosition.Y - displacementFromMouse.Y);
            }

            
        }

        private void HandleMouseDown(object sender, MouseButtonEventArgs e)
        {
            displacementFromMouse = e.GetPosition(MapPanel);
        }

        private void HandleClickProvince(object sender, MouseButtonEventArgs e)
        {
            int nextState = 0;
            var element = (UIElement)sender;
            var provinceid = provinceDict.FirstOrDefault(x => x.Value.Equals(element)).Key;
            //MessageBox.Show(provinceid.ToString());
            var notocean = false;

            if (selectedState != 0 && Keyboard.IsKeyDown(Key.LeftShift))
            {
                
                foreach (var stateid in stateDict.Keys)
                {

                    if (stateDict[stateid].Contains(provinceid.ToString()))
                    {
                        if (stateid == nextState)
                        {
                            return;
                        }
                        states.transferProvince(selectedState, provinceid, stateid);
                        //provincewindow.addProvince(provinceid);
                        // THIS BREAKS THE CODE ^^^^^^^^^^^^^^^^^^^^
                        // UNCOMMENT FIXES IT BUT MAKES A GIANT STATE AMALGAMATION
                    }
                }
                stateDict = new Dictionary<int, List<string>>();
                foreach (var state in states.states)
                {
                    var tempProvinces = (string[])state.Value["provinces"];
                    stateDict.Add(state.Key, tempProvinces.ToList());
                }
            }
            foreach (var stateid in stateDict.Keys)
            {

                if (stateDict[stateid].Contains(provinceid.ToString()))
                {
                    notocean = true;
                    for (var j = 0; j < stateDict[stateid].Count; j++)
                    {
                        if (stateDict[stateid][j] == provinceid.ToString())
                        {
                            var id1 = int.Parse(stateDict[stateid][j].Trim());
                            var tempImage1 = (Image)provinceDict[id1];
                            tempImage1.Style = (Style)FindResource("SelectedProvince");
                        }
                        else if (stateDict[stateid][j] != "")
                        {
                            //MessageBox.Show(stateDict[stateid][j]);
                            var id = int.Parse(stateDict[stateid][j].Trim());
                            var tempImage = (Image)provinceDict[id];
                            tempImage.Style = (Style)FindResource("StateStyle");
                        }

                    }
                    nextState = stateid;

                }

                else if (stateid == selectedState)
                {
                    for (var j = 0; j < stateDict[stateid].Count; j++)
                    {
                        if (stateDict[stateid][j] != "")
                        {
                            var id = int.Parse(stateDict[stateid][j].Trim());
                            var tempImage = (Image)provinceDict[id];
                            tempImage.Style = (Style)FindResource("ProvinceStyle");
                        }
                    }
                }

            }

            selectedState = nextState;
            if (notocean)
            {
                provincewindow.updateWindow(states.getStates()[selectedState], provinceid);
            }
            
            
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("lmao");
            //states.saveStates();

            //var next = Saver.saveData(states.getStates());
            //MessageBox.Show(next.ToString() + "states saved");
            //string[] returnstring = next.ToArray();
            var whatever = new Exporter();
            whatever.ExportStateData(states.getStates());


        }
    }
}
