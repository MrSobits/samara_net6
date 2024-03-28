namespace Bars.Gkh.Regions.Msk
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            AddResource(container, "libs/B4/controller/import/CommonRealityObjectImport.js");
            AddResource(container, "libs/B4/controller/import/MskCeoPointImport.js");
            AddResource(container, "libs/B4/controller/import/MskCeoStateImport.js");
            AddResource(container, "libs/B4/controller/import/MskCeoStateServiceImport.js");
            AddResource(container, "libs/B4/controller/import/MskDpkrImport.js");
            AddResource(container, "libs/B4/controller/import/MskOverhaulImport.js");
            AddResource(container, "libs/B4/controller/report/TypeWorkReport.js");
            AddResource(container, "libs/B4/view/import/MskCeoPointPanel.js");
            AddResource(container, "libs/B4/view/import/MskCeoStatePanel.js");
            AddResource(container, "libs/B4/view/import/MskCeoStateServicePanel.js");
            AddResource(container, "libs/B4/view/import/MskDpkrPanel.js");
            AddResource(container, "libs/B4/view/import/MskOverhaulPanel.js");
            AddResource(container, "libs/B4/view/report/TypeWorkReportPanel.js");

        }

        private void AddResource(IResourceManifestContainer container, string path)
		{
            container.Add(path, string.Format("Bars.Gkh.Regions.Msk.dll/Bars.Gkh.Regions.Msk.{0}", path.Replace("/", ".")));
        }
    }
}
