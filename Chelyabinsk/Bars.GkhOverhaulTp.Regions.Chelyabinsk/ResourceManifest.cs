namespace Bars.GkhOverhaulTp.Regions.Chelyabinsk
{     
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {
		protected override void BaseInit(IResourceManifestContainer container)
        {  
            AddResource(container, "libs/B4/controller/report/AreaDataSheetReport.js");
            AddResource(container, "libs/B4/view/report/AreaDataSheetReportPanel.js");
        }

        private void AddResource(IResourceManifestContainer container, string path)
		{
            container.Add(path, string.Format("Bars.GkhOverhaulTp.Regions.Chelyabinsk.dll/Bars.GkhOverhaulTp.Regions.Chelyabinsk.{0}", path.Replace("/", ".")));
        }
    }
}
