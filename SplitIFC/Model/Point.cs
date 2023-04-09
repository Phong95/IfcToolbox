using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Common.Geometry;

namespace SplitIFC.Model
{
    [Serializable]
    public class Point
    {
        public double X { get; set; } =0;
        public double Y { get; set; } =0;
        public double Z { get; set; } =0;
        public Point()
        {

        }
        public Point (XbimPoint3D xbimPoint)
        {
            X = xbimPoint.X;
            Y = xbimPoint.Y;
            Z = xbimPoint.Z;
        }
    }
}
