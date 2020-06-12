using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET;
using GMap.NET.WindowsPresentation;

namespace OOP_lab4
{
    abstract class MapObject
    {
        public string Title;
        public DateTime CreationDate;
        public MapObject(string title)
        {
            this.Title = title;
            this.CreationDate = DateTime.Now;
        }

        public DateTime GetCreationData()
        {
            return CreationDate;
        }

        abstract public double GetDistance(PointLatLng point);

        abstract public PointLatLng GetFocus();
        abstract public GMapMarker GetMarker();
        public string GetTitle()
        {
            return Title;
        }


    }
}
