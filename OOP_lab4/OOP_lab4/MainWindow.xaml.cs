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
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using System.Device.Location;

namespace OOP_lab4
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<MapObject> MapObjects = new List<MapObject>();
        private List<PointLatLng> Point = new List<PointLatLng>();

        Route Route = null;
        Human Human = null;
        Car Car = null;
        public GMapMarker marker_chelik = null;
        GMapMarker dist;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MapLoaded(object sender, RoutedEventArgs e)
        {
            // настройка доступа к данным
            GMaps.Instance.Mode = AccessMode.ServerAndCache;
            // установка провайдера карт
            Map.MapProvider = OpenStreetMapProvider.Instance;
            // установка зума карты
            Map.MinZoom = 2;
            Map.MaxZoom = 20;
            Map.Zoom = 15;
            // установка фокуса карты
            Map.Position = new PointLatLng(55.012823, 82.950359);
            // настройка взаимодействия с картой

            Map.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter;
            Map.CanDragMap = true;
            Map.DragButton = MouseButton.Left;
        }

        private void Map_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PointLatLng point = Map.FromLocalToLatLng((int)e.GetPosition(Map).X, (int)e.GetPosition(Map).Y);
            if (CHm.IsChecked == true)
            {

                Point.Add(point);

                if (objType.SelectedIndex > -1)
                {
                    if (objType.SelectedIndex == 0)
                    {
                        Car = new Car(objTitle.Text, Point[0]);
                        MapObjects.Add(Car);
                        if (Human != null)
                        {
                            Car.Arrived += Human.CarArrived;
                            Human.chelvmashine += chelvmashine;

                        }
                    }
                    if (objType.SelectedIndex == 1)
                    {
                        Human = new Human(objTitle.Text, Point[0]);
                        MapObjects.Add(Human);

                        if (Car != null)
                        {
                            Car.Arrived += Human.CarArrived;
                            Human.chelvmashine += chelvmashine;
                        }

                    }
                    if (objType.SelectedIndex == 2)
                    {
                        if (Human != null)
                        {
                            Human.moveTo(point);

                            dist = new GMapMarker(point)
                            {
                                Shape = new Image
                                {
                                    Width = 40, // ширина маркера
                                    Height = 40, // высота маркера  
                                    ToolTip = "dest",
                                    Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Kit.jpg")), // картинка
                                    RenderTransform = new TranslateTransform { X = -20, Y = -20 } // картинка
                                }
                            };
                        }
                    }
                    Point = new List<PointLatLng>();
                }

                Map.Markers.Clear();
                objectList.Items.Clear();

                foreach (MapObject cm in MapObjects)
                {
                    Map.Markers.Add(cm.GetMarker());
                    objectList.Items.Add(cm.GetTitle());
                }

                Map.Markers.Add(dist);
            }
        }

        private void CHm_Checked(object sender, RoutedEventArgs e)
        {
            if (CHm.IsChecked == true)
            {
                NCHm.IsChecked = false;
                objTitle.IsEnabled = true;
                objType.IsEnabled = true;
                //AddM.IsEnabled = true;
            }

        }


        private void NCHm_Checked(object sender, RoutedEventArgs e)
        {
            if (NCHm.IsChecked == true)
            {
                CHm.IsChecked = false;
                objTitle.IsEnabled = false;
                objType.IsEnabled = false;
                // AddM.IsEnabled = false;
            }
        }

        private void ObjectList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (objectList.SelectedIndex > -1)
            {
                PointLatLng p = MapObjects[objectList.SelectedIndex].GetFocus();
                Map.Position = p;
            }
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            objectList.UnselectAll();
            objectList.Items.Clear();

            foreach (MapObject mapObject in MapObjects)
            {
                if (mapObject.GetTitle().Contains(poisk.Text))
                {
                    objectList.Items.Add(mapObject.GetTitle());
                }
            }
        }

        private void RoutMap_Click(object sender, RoutedEventArgs e)
        {
            if (dist != null)
            {
                Map.Markers.Add(Car.moveTo(Human.GetFocus()));
                Car.Follow += Car_Follow;
            }
            else
            {
                MessageBox.Show("Определите пункт назначения.");
            }
        }

        private void Car_Follow(object sender, EventArgs e)
        {
            Car Car = (Car)sender;
            Car.Arrived += Human.CarArrived;


            Map.Position = Car.GetFocus();


        }

        public void chelvmashine(object sender, EventArgs e)
        {
            var pass = (Human)sender;
            Car.setPass(pass);

            Application.Current.Dispatcher.Invoke(delegate {
                Map.Markers.Add(Car.moveTo(pass.getDestination()));
            });
        }
    }
}
