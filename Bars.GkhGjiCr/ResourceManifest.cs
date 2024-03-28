namespace Bars.GkhGjiCr
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            AddResource(container, "libs/B4/controller/report/Form1StateHousingInspection.js");
            AddResource(container, "libs/B4/controller/report/InformationCrByResolution.js");
            AddResource(container, "libs/B4/controller/report/TestReport.js");
            AddResource(container, "libs/B4/view/report/Form1StateHousingInspectionPanel.js");
            AddResource(container, "libs/B4/view/report/InformationCrByResolutionPanel.js");
            AddResource(container, "libs/B4/view/report/TestReport.js");

            AddResource(container, "resources/Form1StateHousingInspection.xlsx");
            AddResource(container, "resources/InformationCrByResolution.xlsx");
            AddResource(container, "resources/TestReport.xlsx");
        }

        

        private void AddResource(IResourceManifestContainer container, string path)
		{

            container.Add(path, string.Format("Bars.GkhGjiCr.dll/Bars.GkhGjiCr.{0}", path.Replace("/", ".")));
        }
    }
}
