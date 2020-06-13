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
            if (Mouset1.IsChecked == true)
            {

                Point.Add(point);

                if (cb_add.SelectedIndex > -1)
                {
                    if (cb_add.SelectedIndex == 0)
                    {
                        Car = new Car(tb_name.Text, Point[0]);
                        MapObjects.Add(Car);
                        if (Human != null)
                        {
                            Car.Arrived += Human.CarArrived;
                            Human.chelvmashine += chelvmashine;

                        }
                    }
                    if (cb_add.SelectedIndex == 1)
                    {
                        Human = new Human(tb_name.Text, Point[0]);
                        MapObjects.Add(Human);

                        if (Car != null)
                        {
                            Car.Arrived += Human.CarArrived;
                            Human.chelvmashine += chelvmashine;
                        }

                    }
                    if (cb_add.SelectedIndex == 2)
                    {
                        if (Human != null)
                        {
                            Human.moveTo(point);

                            dist = new GMapMarker(point)
                            {
                                Shape = new Image
                                {
                                    Width = 20, // ширина маркера
                                    Height = 20, // высота маркера  
                                    ToolTip = "dest",
                                    Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Mark.png")), // картинка
                                    RenderTransform = new TranslateTransform { X = -10, Y = -10 } // картинка
                                }
                            };
                        }
                    }
                    Point = new List<PointLatLng>();
                }

                Map.Markers.Clear();
                lb_search.Items.Clear();

                foreach (MapObject cm in MapObjects)
                {
                    Map.Markers.Add(cm.GetMarker());
                    lb_search.Items.Add(cm.GetTitle());
                }

                Map.Markers.Add(dist);
            }
        }


        private void lb_search_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lb_search.SelectedIndex > -1)
            {
                PointLatLng p = MapObjects[lb_search.SelectedIndex].GetFocus();
                Map.Position = p;
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
                MessageBox.Show("Нет конечной точки маршрута");
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


        //.....................................................................................
        private void Mouset1_Checked(object sender, RoutedEventArgs e)
        {
            Map.CanDragMap = false;
            Mouset2.IsChecked = false;
        }

        private void Mouset2_Checked(object sender, RoutedEventArgs e)
        {
            Map.CanDragMap = true;
            Mouset1.IsChecked = false;
        }

        private void Secret_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (Secret.SelectedIndex)
            {
                case 0:
                    Map.MapProvider = OpenStreetMapProvider.Instance;
                    break;
                case 1:
                    Map.MapProvider = GMapProviders.GoogleMap;
                    break;
                case 2:
                    Map.MapProvider = GMapProviders.GoogleSatelliteMap;
                    break;
                case 3:
                    Map.MapProvider = GMapProviders.GoogleHybridMap;
                    break;
                case 4:
                    Map.MapProvider = GMapProviders.BingMap;
                    break;
                case 5:
                    Map.MapProvider = GMapProviders.YandexMap;
                    break;
                case 6:
                    Map.MapProvider = GMapProviders.WikiMapiaMap;
                    break;
                default:
                    break;
            }
        }

        private void tb_name_GotFocus(object sender, RoutedEventArgs e)
        {

            if (tb_name.Text == "no name ")
            {
                tb_name.Text = "";
                tb_name.Opacity = 1;
            }
        }

        private void tb_name_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tb_name.Text == "")
            {
                tb_name.Opacity = 0.5;
                tb_name.Text = "no name ";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Map.Markers.Clear();
            MapObjects.Clear();
            lb_search.Items.Clear();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            lb_search.UnselectAll();
            lb_search.Items.Clear();

            foreach (MapObject mapObject in MapObjects)
            {
                if (mapObject.GetTitle().Contains(tb_search.Text))
                {
                    lb_search.Items.Add(mapObject.GetTitle());
                }
            }
        }
    }
}
