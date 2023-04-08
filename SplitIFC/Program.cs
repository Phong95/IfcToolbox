// See https://aka.ms/new-console-template for more information
using IfcToolbox.Core.Editors;
using IfcToolbox.Core.Utilities;
using IfcToolbox.Tools.Configurations;
using IfcToolbox.Tools.Processors;
using SplitIFC.Extensions;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;

//Console.WriteLine("Hello, World!");
string filePath = "concept2.ifc";
//IConfigSplit config = ConfigFactory.CreateConfigSplit();
//config.LogDetail = true;
//config.SplitStrategy = SplitStrategy.ByBuildingStorey;
//config.SelectedItems = new List<string> { "137", "131" };
//SplitterProcessor.Process(filePath, config, true);
using (var model = IfcStore.Open(filePath))
{
    var requiredProducts = model.Instances.OfType<IIfcProduct>().Where(x => !(x is IIfcSpatialStructureElement)).ToList();
    var listListProducts  = requiredProducts.Divide(20);
    for (int i = 0; i < listListProducts.Count(); i++)
    {
        var items = listListProducts[i];
        string path = Path.Combine("D:\\Github\\IfcToolbox\\SplitIFC\\bin\\Debug\\net6.0\\output", $"concept2{i}.ifc");
        InsertCopy.CopyProducts(model, path, listListProducts[i], true);
    }


}