using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SplitIFC.Model
{
    public class RenderMaterial
    {
        public string Name { get; set; } = "";
        public double Opacity { get; set; } = 1;
        public double Metalness { get; set; } = 0;
        public double Roughness { get; set; } = 1;
        public double Red { get; set; } = 0;
        public double Green { get; set; } = 0;
        public double Blue { get; set; } = 0;

        //public int Diffuse { get; set; } = Color.LightGray.ToArgb();
        //public int Emissive { get; set; } = Color.Black.ToArgb();

        //public Color DiffuseColor
        //{
        //    get => Color.FromArgb(Diffuse);
        //    set => Diffuse = value.ToArgb();
        //}

        //public Color EmissiveColor
        //{
        //    get => Color.FromArgb(Emissive);
        //    set => Emissive = value.ToArgb();
        //}
        public RenderMaterial()
        {

        }
        public RenderMaterial(double red, double green, double blue)
        {
            //    name = revitMaterial.Name,
            //opacity = 1 - (revitMaterial.Transparency / 100d),
            //Diffuse = System.Drawing.Color.FromArgb(red, green, blue).ToArgb();
            Red = red;
            Green = green;
            Blue = blue;
        }
        public RenderMaterial(double red, double green, double blue,double transparency)
        {
            Red = red;
            Green = green;
            Blue = blue;
            //    name = revitMaterial.Name,
            Opacity = 1 - transparency;
        }
    }
}
