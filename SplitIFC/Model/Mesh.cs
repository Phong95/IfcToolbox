using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitIFC.Model
{
    [Serializable]
    public class Mesh
    {
        public List<Point> Vertices { get; set; } = new List<Point>();
        public List<Face> Faces { get; set; } = new List<Face>();
        public int MaterialIndex { get; set; } = 0;
        public string Matrix4 { get; set; } = "";
    }
}
