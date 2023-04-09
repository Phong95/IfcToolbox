// See https://aka.ms/new-console-template for more information
using IfcToolbox.Core.Editors;
using IfcToolbox.Core.Utilities;
using IfcToolbox.Tools.Configurations;
using IfcToolbox.Tools.Processors;
using SplitIFC.Extensions;
using SplitIFC.Model;
using Xbim.Common.Geometry;
using Xbim.Common.XbimExtensions;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;
using Xbim.ModelGeometry.Scene;

//Console.WriteLine("Hello, World!");
string filePath = "Project2.ifc";
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
    ViralViewerBaseObject viralViewerBaseProject = new ViralViewerBaseObject();

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
                if(surfaceStyle is not null)
                {
                    var styles = ((Xbim.Ifc2x3.PresentationAppearanceResource.IfcSurfaceStyle)surfaceStyle!).Styles;
                    if (styles.Count > 0)
                    {
                        var firstMaterial = styles.First();
                        var materialColour = ((Xbim.Ifc2x3.PresentationAppearanceResource.IfcSurfaceStyleShading)firstMaterial).SurfaceColour;
                        var transparent = ((Xbim.Ifc2x3.PresentationAppearanceResource.IfcSurfaceStyleRendering)firstMaterial).Transparency;
                        viralViewerBaseObjectMesh.Material = new RenderMaterial((double)materialColour.Red.Value, (double)materialColour.Green.Value, (double)materialColour.Blue.Value, transparent!.Value);
                        viralViewerBaseObjectMesh.Material.Name = ((Xbim.Ifc2x3.PresentationAppearanceResource.IfcPresentationStyle)surfaceStyle).Name!.Value;
                    }
                }

            }
            var transfor = instance.Transformation; //Transformation matrix (location point inside)

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
                    viralViewerBaseObjectMesh.Faces = faces.Select(x=> new Face(x)).ToList();
                    viralViewerBaseObject.DisplayValue.Add(viralViewerBaseObjectMesh);
                }
            }
        }
        viralViewerBaseProject.Child.Add(viralViewerBaseObject);
    }
    //var listListProducts  = requiredProducts.Divide(20);
    //for (int i = 0; i < listListProducts.Count(); i++)
    //{
    //    var items = listListProducts[i];
    //    string path = Path.Combine("D:\\Github\\IfcToolbox\\SplitIFC\\bin\\Debug\\net6.0\\output", $"Canteen{i}.ifc");
    //    InsertCopy.CopyProducts(model, path, listListProducts[i], true);
    //}


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