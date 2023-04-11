// See https://aka.ms/new-console-template for more information
using IfcToolbox.Core.Editors;
using IfcToolbox.Core.Utilities;
using IfcToolbox.Tools.Configurations;
using IfcToolbox.Tools.Processors;
using Newtonsoft.Json;
using SplitIFC.Extensions;
using SplitIFC.Model;
using System.Text.Json;
using System.Xml.Linq;
using Xbim.Common.Geometry;
using Xbim.Common.XbimExtensions;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;
using Xbim.ModelGeometry.Scene;

//Console.WriteLine("Hello, World!");
string filePath = "Cofico_Office-FM-220829.ifc";
//IConfigSplit config = ConfigFactory.CreateConfigSplit();
//config.LogDetail = true;
//config.SplitStrategy = SplitStrategy.ByBuildingStorey;
//config.SelectedItems = new List<string> { "137", "131" };
//SplitterProcessor.Process(filePath, config, true);
using (var model = IfcStore.Open(filePath))
{
    Xbim3DModelContext context = new Xbim3DModelContext(model);
    context.CreateContext();
    var requiredProducts = model.Instances.OfType<IIfcProduct>().Where(x => !(x is IIfcSpatialStructureElement)).ToList();
    //styles=>surfacestyles
    ViralViewerBaseProject viralViewerBaseProject = new ViralViewerBaseProject();

    foreach (var item in requiredProducts)
    {
        var instances = context.ShapeInstancesOf(item);
        ViralViewerBaseObject viralViewerBaseObject = new ViralViewerBaseObject();
        viralViewerBaseObject.Name = item.Name!;
        IIfcMaterialSelect material = item.Material;
        foreach (var instance in instances)
        {
            Mesh viralViewerBaseObjectMesh = new Mesh();

            if (instance.HasStyle)
            {
                int materialIndex = instance.StyleLabel;
                var surfaceStyle = model.Instances.FirstOrDefault(x => x.EntityLabel == materialIndex);
                if (surfaceStyle is not null)
                {

                    var styles = ((Xbim.Ifc2x3.PresentationAppearanceResource.IfcSurfaceStyle)surfaceStyle!).Styles;
                    if (styles.Count > 0)
                    {
                        string materialName = ((Xbim.Ifc2x3.PresentationAppearanceResource.IfcPresentationStyle)surfaceStyle).Name!.Value;
                        int findedIndex = viralViewerBaseProject.Materials.FindIndex(x => x.Name == materialName);
                        if(findedIndex>=0)
                        {
                            viralViewerBaseObjectMesh.MaterialIndex = findedIndex;
                        }
                        else
                        {
                            var firstMaterial = styles.First();
                            var materialColour = ((Xbim.Ifc2x3.PresentationAppearanceResource.IfcSurfaceStyleShading)firstMaterial).SurfaceColour;
                            var transparent = ((Xbim.Ifc2x3.PresentationAppearanceResource.IfcSurfaceStyleRendering)firstMaterial).Transparency;
                            RenderMaterial newMaterial = new RenderMaterial((double)materialColour.Red.Value, (double)materialColour.Green.Value, (double)materialColour.Blue.Value, transparent!.Value);
                            newMaterial.Name = materialName;
                            viralViewerBaseProject.Materials.Add(newMaterial);
                            viralViewerBaseObjectMesh.MaterialIndex = viralViewerBaseProject.Materials.Count - 1;
                        }

                    }
                }

            }
            var transfor = instance.Transformation;
            //Transformation matrix (location point inside)
            //viralViewerBaseObjectMesh.Transform.OffsetX = transfor.OffsetX;
            //viralViewerBaseObjectMesh.Transform.OffsetY = transfor.OffsetY;
            //viralViewerBaseObjectMesh.Transform.OffsetZ = transfor.OffsetZ;
            var matrixItem = transfor.ToString().Split(' ');
            var matrix4 = JsonConvert.SerializeObject(matrixItem);
            viralViewerBaseObjectMesh.Matrix4 = matrix4;
            XbimShapeGeometry geometry = context.ShapeGeometry(instance);   //Instance's geometry
            XbimRect3D box = geometry.BoundingBox; //bounding box you need

            byte[] data = ((IXbimShapeGeometryData)geometry).ShapeData;

            //If you want to get all the faces and trinagulation use this
            using (var stream = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(stream))
                {
                    var mesh = reader.ReadShapeTriangulation();
                    List<XbimFaceTriangulation> faces = (mesh.Faces as List<XbimFaceTriangulation>)!;
                    List<XbimPoint3D> vertices = (mesh.Vertices as List<XbimPoint3D>)!;
                    viralViewerBaseObjectMesh.Vertices = vertices.Select(x => new Point(x)).ToList();
                    viralViewerBaseObjectMesh.Faces = faces.Select(x => new Face(x)).ToList();
                    viralViewerBaseObject.DisplayValue.Add(viralViewerBaseObjectMesh);
                }
            }
        }
        viralViewerBaseProject.Objects.Add(viralViewerBaseObject);
    }
    //var listListProducts  = requiredProducts.Divide(20);
    //for (int i = 0; i < listListProducts.Count(); i++)
    //{
    //    var items = listListProducts[i];
    //    string path = Path.Combine("D:\\Github\\IfcToolbox\\SplitIFC\\bin\\Debug\\net6.0\\output", $"Canteen{i}.ifc");
    //    InsertCopy.CopyProducts(model, path, listListProducts[i], true);
    //}
    //BinaryExtensions.WriteToBinaryFile(@"D:\Github\IfcToolbox\SplitIFC\bin\Debug\net6.0\output\Project2.json", viralViewerBaseProject);
    viralViewerBaseProject.Objects = viralViewerBaseProject.Objects.Where(x=>x.DisplayValue.Count!=0).ToList();
    string json = System.Text.Json.JsonSerializer.Serialize(viralViewerBaseProject);
    string final =  StringCompressionExtensions.Compress(json);
    File.WriteAllText(@"D:\Github\IfcToolbox\SplitIFC\bin\Debug\net6.0\output\Cofico_Office-FM-220829-compress.json", final);
}



//using (var model = IfcStore.Open(filePath))
//{
//    Xbim3DModelContext context = new Xbim3DModelContext(model);
//    context.CreateContext();

//    List<XbimShapeGeometry> geometrys = context.ShapeGeometries().ToList();
//    List<XbimShapeInstance> instances = context.ShapeInstances().ToList();

//    //Check all the instances
//    foreach (var instance in instances)
//    {
//        var transfor = instance.Transformation; //Transformation matrix (location point inside)

//        XbimShapeGeometry geometry = context.ShapeGeometry(instance);   //Instance's geometry
//        XbimRect3D box = geometry.BoundingBox; //bounding box you need

//        byte[] data = ((IXbimShapeGeometryData)geometry).ShapeData;

//        //If you want to get all the faces and trinagulation use this
//        using (var stream = new MemoryStream(data))
//        {
//            using (var reader = new BinaryReader(stream))
//            {
//                var mesh = reader.ReadShapeTriangulation();

//                List<XbimFaceTriangulation> faces = mesh.Faces as List<XbimFaceTriangulation>;
//                List<XbimPoint3D> vertices = mesh.Vertices as List<XbimPoint3D>;
//            }
//        }
//    }
//}