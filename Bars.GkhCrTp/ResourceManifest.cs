namespace Bars.GkhCrTp
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            AddResource(container, "libs/B4/controller/report/ExcerptFromTechPassportMkd.js");
            AddResource(container, "libs/B4/controller/report/ProgramCrReport.js");
            AddResource(container, "libs/B4/controller/report/SetTwoOutlineBoiler.js");
            AddResource(container, "libs/B4/view/report/ExcerptFromTechPassportMkdPanel.js");
            AddResource(container, "libs/B4/view/report/ProgramCrReportPanel.js");
            AddResource(container, "libs/B4/view/report/SetTwoOutlineBoilerPanel.js");

            AddResource(container, "resources/ExcerptFromTechPassportMkdReport.xlsx");
            AddResource(container, "resources/ProgramCrReport.xlsx");
            AddResource(container, "resources/SetTwoOutlineBoiler.xlsx");
        }

        

        private void AddResource(IResourceManifestContainer container, string path)
		{

            container.Add(path, string.Format("Bars.GkhCrTp.dll/Bars.GkhCrTp.{0}", path.Replace("/", ".")));
        }
    }
}
