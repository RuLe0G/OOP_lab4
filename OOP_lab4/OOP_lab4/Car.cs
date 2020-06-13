using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Numerics;
using System.Windows.Media;

namespace OOP_lab4
{
    class Car : MapObject
    {
        public PointLatLng Point;
        private Route route;
        private Human chel;

        GMapMarker marker_car;


        public event EventHandler Arrived;
        public event EventHandler Follow;

        
        Thread newThread;


        public Car(string title, PointLatLng point) : base(title)
        {
            this.Point = point;
            chel = null;

        }

        public void setPosition(PointLatLng point)
        {
            this.Point = point;
        }

        public override double GetDistance(PointLatLng point)
        {
            GeoCoordinate c1 = new GeoCoordinate(point.Lat, point.Lng);
            GeoCoordinate c2 = new GeoCoordinate(Point.Lat, Point.Lng);

            return c1.GetDistanceTo(c2);
        }

        public override PointLatLng GetFocus()
        {
            return Point;
        }

        public void setPass(Human chel)
        {
            this.chel = chel;
        }

        public override GMapMarker GetMarker()
        {
            marker_car = new GMapMarker(Point)
            {
                Shape = new Image
                {
                    Width = 40, // ширина маркера
                    Height = 40, // высота маркера
                    ToolTip = Title, // всплывающая подсказка
                    Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Car.png")), // картинка
                    RenderTransform = new TranslateTransform { X = -20, Y = -20 } // картинка
                }
            };
            marker_car.Position = Point;
            return marker_car;
        }

        public GMapMarker moveTo(PointLatLng LastP)
        {            
            RoutingProvider routingProvider = GMapProviders.OpenStreetMap;            
            MapRoute route = routingProvider.GetRoute(Point, LastP,false,false,(int)15); // (начало, конец, дорога, пешеход)

            List<PointLatLng> routePoints = route.Points;

            this.route = new Route("", routePoints);

            newThread = new Thread(new ThreadStart(Movement));
            newThread.Start();


            return this.route.GetMarker();
        }

        private void Movement()
        {
            foreach (var point in route.GetPoints())
            {                
                Application.Current.Dispatcher.Invoke(delegate {
                    marker_car.Position = point;
                    this.Point = point;
                    if (chel != null)
                    {
                        chel.setPosition(point);
                        chel.marker_chelik.Position = point;
                        Follow?.Invoke(this, null);
                    }
                });
                Thread.Sleep(1000);
            }
            if (chel == null)
            {
                Arrived?.Invoke(this, null);
            }
            else
            {
                MessageBox.Show("Приехали");
                newThread.Abort();
                chel = null;

            }


        }

    }




}

