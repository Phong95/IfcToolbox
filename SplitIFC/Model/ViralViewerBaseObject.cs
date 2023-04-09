using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitIFC.Model
{
    public class ViralViewerBaseObject
    {
        public string Name { get; set; } = "";
        public List<Mesh> DisplayValue { get; set; } = new List<Mesh>();
        public List<ViralViewerBaseObject> Child = new List<ViralViewerBaseObject>();
    }
}
