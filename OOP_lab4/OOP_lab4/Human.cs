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
    class Human : MapObject
    {

        public PointLatLng Point;
        private PointLatLng destination;
        public GMapMarker marker_chelik;

        public event EventHandler chelvmashine;

        public Human(string title, PointLatLng startPoint) : base(title)
        {
            Point = startPoint;
        }

        public void setPosition(PointLatLng point)
        {
            Point = point;
        }

        public PointLatLng getDestination()
        {
            return destination;
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

        public override GMapMarker GetMarker()
        {
                        
            marker_chelik = new GMapMarker(Point)
            {
                Shape = new Image
                {
                    Width = 32, // ширина маркера
                    Height = 32, // высота маркера                   
                    Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Chelik.png")), // картинка
                    RenderTransform = new TranslateTransform { X = -20, Y = -20 } // картинка
                }
            };
            marker_chelik.Position = Point;
            return marker_chelik;

        }
        public void moveTo(PointLatLng LastP)
        {
            destination = LastP;
        }

        public void CarArrived(object sender, EventArgs e)
        {
            chelvmashine?.Invoke(this, EventArgs.Empty);
            MessageBox.Show("Человек в машине");

        }

    }
}
