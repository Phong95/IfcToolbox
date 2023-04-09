using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Common.Geometry;

namespace SplitIFC.Model
{
    [Serializable]
    public class Face
    {
        public List<int> Indices {  get; set; }=new List<int>();
        public Face() { }
        public Face(XbimFaceTriangulation xbimFace)
        {
            Indices = (List<int>)xbimFace.Indices;
        }
    }
}
