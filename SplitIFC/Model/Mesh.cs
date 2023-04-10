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
        public RenderMaterial Material { get; set; } = new RenderMaterial();
        //public Transform Transform { get; set; } = new Transform();
        public string Matrix4 { get; set; } = "";
    }
}
