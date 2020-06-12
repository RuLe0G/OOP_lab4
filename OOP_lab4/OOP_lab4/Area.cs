using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using GMap.NET;
using GMap.NET.WindowsPresentation;

namespace OOP_lab4
{
    class Area : MapObject
    {
        public List<PointLatLng> Points;

        public Area(string title, List<PointLatLng> points) : base(title)
        {
            Points = points;
        }

        public override double GetDistance(PointLatLng ent_point)
        {
            var distance = Double.MaxValue;
            foreach (PointLatLng point in Points)
            {
                GeoCoordinate c1 = new GeoCoordinate(point.Lat, point.Lng);
                GeoCoordinate c2 = new GeoCoordinate(ent_point.Lat, ent_point.Lng);
                if (distance > c1.GetDistanceTo(c2))
                {
                    distance = c1.GetDistanceTo(c2);
                }
            }
            return distance;
        }

        public override PointLatLng GetFocus()
        {
            return Points[0];
        }

        public override GMapMarker GetMarker()
        {
            GMapMarker polygon_route = new GMapPolygon(Points)
            {
                Shape = new Path
                {
                    Stroke = Brushes.Black, // стиль обводки
                    Fill = Brushes.Violet, // стиль заливки
                    Opacity = 0.7 // прозрачность
                }
            };
            return polygon_route;
        }
    }
}

