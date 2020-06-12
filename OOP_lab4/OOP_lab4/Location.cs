using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET;
using GMap.NET.WindowsPresentation;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Device.Location;

namespace OOP_lab4
{
    class Location : MapObject
    {

        public PointLatLng Point;

        public Location(string title, PointLatLng point) : base(title)
        {
            Point = point;
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
            GMapMarker marker_chelik = new GMapMarker(Point)
            {
                Shape = new Image
                {
                    Width = 32, // ширина маркера
                    Height = 32, // высота маркера                   
                    Source = new BitmapImage(new Uri("pack://application:,,,/Resources/marker.png")) // картинка
                }
            };
            marker_chelik.Position = Point;
            return marker_chelik;
        }
    }
}
