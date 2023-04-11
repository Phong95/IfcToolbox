using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitIFC.Model
{
    [Serializable]
    public class ViralViewerBaseProject
    {
        public List<ViralViewerBaseObject> Objects { get; set; }= new List<ViralViewerBaseObject>();
        public List<RenderMaterial> Materials { get; set; } = new List<RenderMaterial>();
    }
}
