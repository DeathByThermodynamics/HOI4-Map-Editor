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
using static System.Net.Mime.MediaTypeNames;
using Image = System.Windows.Controls.Image;
using Application = System.Windows.Application;
using Color = System.Windows.Media.Color;
using hoi4test3;

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
        //ProvinceView provincewindow;
        public States states;
        public StratRegion stratRegions;
        public Dictionary<int,List<string>> stateDict = new Dictionary<int, List<string>>();
        Dictionary<int, UIElement> provinceDict;
        Dictionary<int, UIElement> tempProvDict;
        public static Dictionary<string, List<string>> definition;
        int selectedState = 0;
        bool opened_map = false;
        Image map_background;
        public List<string> changedprovinces;
        public List<string> changedcolours;
        bool mapProcess = false;
        int num_provinces = 0;
        NewProvinceView provinceView;
        public MainWindow()
        {
            InitializeComponent();
            new hoi4test3.DataManager();
            //provincewindow = new ProvinceView(this);
            
            x_change1 = +((x_lim - x_lim * scale) / 2);
            y_change1 = +((y_lim - y_lim * scale) / 2);
            x_change2 = x_change1;
            y_change2 = y_change1;
            definition = new Dictionary<string, List<string>>();
            changedprovinces = new List<string>();
            changedcolours = new List<string>();
            tempProvDict = new Dictionary<int, UIElement>();
            provinceView = new NewProvinceView(this);
        }

        public void Exit(object sender, RoutedEventArgs e)
        {
            if (opened_map)
            {
                states.saveStates();
                // This is ok because exporting strategic regions is much more lightweight.
                // Eventually, would like states to also always be exported.
                var exporter = new Exporter();
                exporter.ExportStratRegions(stratRegions.stratProvinces, stratRegions.stratData);
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
            string text = System.IO.File.ReadAllText(@starter.hoi4folder + "/mapEditor/statedata.txt");
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
            states = new States(@starter.hoi4folder + "/mapEditor/statedatafull.txt");
            stateDict = new Dictionary<int, List<string>>();
            foreach (var state in states.states)
            {
                var tempProvinces = (string[]) state.Value["provinces"];
                stateDict.Add(state.Key, tempProvinces.ToList());
            }
            states.getCountryColours(@starter.hoi4folder + "/mapEditor/countrycolours.txt");
        }

        public void ReloadMap()
        {
            map_background.Source = LoadBitmapImage(starter.hoi4folder + "/mapEditor/provinces/borders.png");
        }
        
        public void UpdateProvinces(List<string> provinces, int newstateprov)
        {
            /*
            foreach (var province in provinces)
            {
                if (province != "")
                {
                    var newImage = (Image)provinceDict[int.Parse(province.ToString())];
                    newImage.Source = LoadBitmapImage(starter.hoi4folder + "/mapEditor/provinces/" + province + ".png");
                    //MessageBox.Show(province.ToString());
                }

            } */
            // ReloadMap();
            stateDict = new Dictionary<int, List<string>>();
            foreach (var state in states.states)
            {
                var tempProvinces = (string[])state.Value["provinces"];
                stateDict.Add(state.Key, tempProvinces.ToList());
            }

            if (newstateprov > 0)
            {
                var tempImage1 = (Image)provinceDict[newstateprov];
                tempImage1.Style = (Style)FindResource("SelectedProvince");
                
                for (var j = 0; j < stateDict[selectedState].Count; j++) 
                {
                        if (stateDict[selectedState][j] != "")
                        {
                            var id = int.Parse(stateDict[selectedState][j].Trim());
                            var tempImage = (Image)provinceDict[id];
                            tempImage.Style = (Style)FindResource("ProvinceStyle");
                        }
                 }
                selectedState = states.states.Count;
                //provincewindow.updateWindow(states.getStates()[selectedState], newstateprov);
                provinceView.updateWindow(states.getStates()[selectedState], newstateprov);
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

        public void AddtoMap(List<string> provinces)
        {
            string text = System.IO.File.ReadAllText(@starter.hoi4folder + "/mapEditor/provincepos.txt");
            var newtext = text.Split(";");
            var provincesplit = newtext[1].Split("?");

            for (var i = 0; i < int.Parse(newtext[0]); i++)
            {
                var provincedata = provincesplit[i].Split(":");
                var provinceid = provincedata[0];
                if (provinces.Contains(provinceid))
                {
                    var xcord = provincedata[1].Split(",")[0];
                    var ycord = provincedata[1].Split(",")[1];

                    Image tempImage = new Image();
                    tempImage.Source = LoadBitmapImage(starter.hoi4folder + "/mapEditor/temp/" + provinceid + ".png");
                    //tempImage.Style = (Style)FindResource("ProvinceStyle");


                    tempImage.MouseLeftButtonDown += new MouseButtonEventHandler(HandleClickProvince);
                    //RenderOptions.SetBitmapScalingMode(tempImage, BitmapScalingMode.LowQuality);
                    //tempImage.Name = provinceid;
                    map.Children.Add(tempImage);
                    if (tempProvDict.ContainsKey(int.Parse(provinceid)))
                    {
                        map.Children.Remove(tempProvDict[int.Parse(provinceid)]);
                        tempProvDict[int.Parse(provinceid)] = tempImage;
                    } else
                    {
                        tempProvDict.Add(int.Parse(provinceid), tempImage);
                    }
                    
                    Canvas.SetLeft(tempImage, int.Parse(ycord));
                    Canvas.SetTop(tempImage, int.Parse(xcord));
                    Canvas.SetZIndex(tempImage, -1);
                    //Canvas.SetZIndex(tempImage, i + 1);
                }
            }
        }
        
        private void GenerateMap(object sender, RoutedEventArgs e)
        {
            stratRegions = new StratRegion();
            stratRegions.getStratRegions(starter.hoi4folder);
            //provincewindow.Owner = this;
            //provincewindow.Show();
            provinceDict = new Dictionary<int, UIElement>();
            GetStateData();
            //MessageBox.Show("lmao");
            // Draw the Background
            Image newImage = new Image();
            newImage.Source = LoadBitmapImage(starter.hoi4folder + "/mapEditor/provinces/borders.png");
            newImage.Opacity = 1;
            map_background = newImage;
            // Create Point for upper-left corner of image.
            grid1.Width = newImage.Width;
            grid1.Height = newImage.Height;
            System.Console.Write(newImage.Height);
            System.Console.Write(newImage.Width);
            //map.Width = newImage.Width;
            //map.Height = newImage.Height;
            
            

            // Draw the Provinces

            string text = System.IO.File.ReadAllText(@starter.hoi4folder + "/mapEditor/provincepos.txt");
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
            RenderOptions.SetBitmapScalingMode(newImage, BitmapScalingMode.LowQuality);
            map.Children.Add(newImage);
            Canvas.SetZIndex(newImage, -2);
            num_provinces = int.Parse(newtext[0]);
            
            List<Image> sortedProvinces = new List<Image>();
            Dictionary<Image, List<string>> provinceData = new Dictionary<Image, List<string>>();
            for (var i = 0; i < int.Parse(newtext[0]); i++)
            {
                var provincedata = provincesplit[i].Split(":");
                var provinceid = provincedata[0];
                var xcord = provincedata[1].Split(",")[0];
                var ycord = provincedata[1].Split(",")[1];
                
                Image tempImage = new Image();
                tempImage.Source = LoadBitmapImage(starter.hoi4folder + "/mapEditor/provinces/" + provinceid + ".png");
                tempImage.Style = (Style)FindResource("ProvinceStyle");
                
                
                tempImage.MouseLeftButtonDown += new MouseButtonEventHandler(HandleClickProvince);
                RenderOptions.SetBitmapScalingMode(tempImage, BitmapScalingMode.LowQuality);
                //tempImage.Name = provinceid;
                
                provinceDict.Add(int.Parse(provinceid), tempImage);
                sortedProvinces.Add(tempImage);
                var templist = new List<string>();
                templist.Add(xcord);
                templist.Add(ycord);
                provinceData.Add(tempImage, templist);
                //map.Children.Add(tempImage);
                //Canvas.SetLeft(tempImage, int.Parse(ycord));
                //Canvas.SetTop(tempImage, int.Parse(xcord));
                //Canvas.SetZIndex(tempImage, 2);
                //Canvas.SetZIndex(tempImage, i + 1);
            }
            
            /*
            sortedProvinces.Sort(delegate (Image x, Image y)
            {
                var xarea = (x).ActualWidth * (x).ActualHeight;
                var yarea = (y).ActualWidth * (y).ActualHeight;
                return xarea > yarea ? 1
                        : xarea < yarea ? -1
                        : 0;
            }); */

            //sortedProvinces.Reverse();

            foreach (var province in sortedProvinces) {
                map.Children.Add(province);
                Canvas.SetLeft(province, int.Parse(provinceData[province][1]));
                Canvas.SetTop(province, int.Parse(provinceData[province][0]));
            }

            // Draw the Overlay Borders
            Image background = new Image();
            background.Source = LoadBitmapImage(starter.hoi4folder + "/mapEditor/provinces/borders2.png");
            background.IsHitTestVisible = false;
            background.Opacity = 1;
            RenderOptions.SetBitmapScalingMode(background, BitmapScalingMode.LowQuality);
            map.Children.Add(background);
            Canvas.SetZIndex(background, 3);

            //Canvas.SetZIndex(background, 0);

        }
        
        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var matTrans = backGrid2.RenderTransform as MatrixTransform;
            var pos1 = e.GetPosition(BackCanvas);

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
            
            /*
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
            */
            Canvas.SetLeft(MapPanel, dropPosition.X - displacementFromMouse.X);
            /*
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
            */
            Canvas.SetTop(MapPanel, dropPosition.Y - displacementFromMouse.Y);
        }

        private void HandleMouseDown(object sender, MouseButtonEventArgs e)
        {
            displacementFromMouse = e.GetPosition(MapPanel);
        }

        private void HandleClickProvince(object sender, MouseButtonEventArgs e)
        {
            int nextState = selectedState;
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
                        stratRegions.transferProvince(provinceid.ToString(), stateDict[stateid][0]);
                        var templist = new List<string>();
                        templist.Add(provinceid.ToString());
                        var colour = states.countryColour[(string)states.getStates()[selectedState]["owner"]];
                        var colourstring = colour[0].ToString() + "," + colour[1].ToString() + "," + colour[2].ToString();
                        
                        states.RepaintProvinces(templist, colourstring, this);
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
                //provincewindow.updateWindow(states.getStates()[selectedState], provinceid);
                provinceView.updateWindow(states.getStates()[selectedState], provinceid);

            }


        }

        private void Save(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("lmao");
            //states.saveStates();

            //var next = Saver.saveData(states.getStates());
            //MessageBox.Show(next.ToString() + "states saved");
            //string[] returnstring = next.ToArray();
            var exporter = new Exporter();
            exporter.ExportStateData(states.getStates());
            exporter.ExportStratRegions(stratRegions.stratProvinces, stratRegions.stratData);


        }

        public void RepaintMapBridge(object sender, RoutedEventArgs e)
        {
            RepaintMap(changedprovinces, changedcolours);
        }

        public void RepaintMap(List<string> provinces, List<string> colours)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            string provincestring = "";
            string colourstring = "";
            //MessageBox.Show(provinces.Count.ToString());
            //MessageBox.Show(colours.Count.ToString());
            for (var i = 0; i < provinces.Count; i++)
            {
                provincestring += provinces[i] + ",";
                colourstring += colours[i] + ";";
            }
            //MessageBox.Show(provincestring);
            //MessageBox.Show(colourstring);
            provincestring = provincestring.Remove(provincestring.Length - 1);
            colourstring = colourstring.Remove(colourstring.Length - 1);
            //MessageBox.Show(provincestring);
            //MessageBox.Show(colourstring);
            /*
             * Exe replacements - done 11/20
             * 
             */
            start.FileName = starter.programfolder + "/dist/maprepainter/maprepainter.exe";
            //MessageBox.Show(string.Format("C:/Users/alexh/electron/backend/hoi4/maprepainter.py {0} {1}", provincestring, colour));
            var directory = starter.hoi4folder;
            start.Arguments = string.Format(" {0} {1} {2} full", provincestring, colourstring, directory);
            start.UseShellExecute = false;
            start.CreateNoWindow = true;
            start.RedirectStandardOutput = true;

            try
            {
                using (Process process = Process.Start(start))
                {

                    using (StreamReader reader = process.StandardOutput)
                    {
                        while (!process.StandardOutput.EndOfStream)
                        {
                            string line = process.StandardOutput.ReadLine();
                            //MessageBox.Show(line);
                        }
                    }

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            foreach (var entry in tempProvDict.Keys)
            {
                map.Children.Remove(tempProvDict[entry]);
            }
            tempProvDict = new Dictionary<int, UIElement>();
            ReloadMap();
            changedprovinces.Clear();
            changedcolours.Clear();
               
            
        }
        public void alignOnGrid(Grid grid, UIElement element, int row, int column)
        {
            Grid.SetRow(element, row);
            Grid.SetColumn(element, column);
            grid.Children.Add(element);
        }
        public void updateUIContent(UIElement element, string content)
        {
            if (element is Label)
            {
                ((Label)element).Content = content;
            }
            else if (element is TextBox)
            {
                ((TextBox)element).Text = content;
            }
            else if (element is ComboBox)
            {
                ((ComboBox)element).Text = content;
            }
            else if (element is CheckBox)
            {
                ((CheckBox)element).IsChecked = bool.Parse(content);
            }
            else if (element is Slider)
            {
                ((Slider)element).Value = double.Parse(content);
            } else
            {
                MessageBox.Show("Error: " + element.ToString() + " is not a valid UI element");
            }
            /*
            else if (element is Image)
            {
                ((Image)element).Source = new BitmapImage(new Uri(content));
            } */
        }
        
        public void TextBox_CheckNumbersOnly(object sender, TextCompositionEventArgs e) =>
            e.Handled = !e.Text.Any(x => Char.IsDigit(x) || ':'.Equals(x));

        private void SaveState(object sender, RoutedEventArgs e)
        {
            provinceView.saveWindows();
        }

        private void CreateNewState(object sender, RoutedEventArgs e)
        {
            provinceView.CreateNewState();
        }
    }
}

