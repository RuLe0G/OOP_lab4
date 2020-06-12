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
    class Route : MapObject
    {

        private List<PointLatLng> Points;

        public Route(string title, List<PointLatLng> points) : base(title)
        {
            Points = new List<PointLatLng>();
            foreach (PointLatLng p in points)
            {

                Points.Add(p);
            }
        }

        public List<PointLatLng> GetPoints()
        {
            return Points;
        }


        public override double GetDistance(PointLatLng ent_point)
        {
            double distance = Double.MaxValue;
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
            GMapMarker marker_route = new GMapRoute(Points)
            {
                Shape = new Path
                {
                    Stroke = Brushes.DarkBlue, // цвет обводки
                    Fill = Brushes.DarkBlue, // цвет заливки
                    StrokeThickness = 4 // толщина обводки
                }
            };
            return marker_route;
        }
    }
}
