namespace Bars.Gkh.Regions.Chelyabinsk
{     
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {
		protected override void BaseInit(IResourceManifestContainer container)
        {  
            AddResource(container, "libs/B4/aspects/RosRegExtractAspect.js");
            AddResource(container, "libs/B4/controller/RosRegExtract.js");
            AddResource(container, "libs/B4/controller/RosRegExtractBig.js");
            AddResource(container, "libs/B4/controller/report/UnderstandingHomeReport.js");
            AddResource(container, "libs/B4/model/RosRegExtractBig.js");
            AddResource(container, "libs/B4/model/RosRegExtractBigOwner.js");
            AddResource(container, "libs/B4/store/RosRegExtractBig.js");
            AddResource(container, "libs/B4/store/RosRegExtractBigOwner.js");
            AddResource(container, "libs/B4/view/RosRegExtract.js");
            AddResource(container, "libs/B4/view/realityobj/Grid.js");
            AddResource(container, "libs/B4/view/report/UnderstandingHomeReportPanel.js");
            AddResource(container, "libs/B4/view/rosregextract/EditWindow.js");
            AddResource(container, "libs/B4/view/rosregextract/Grid.js");
            AddResource(container, "libs/B4/view/rosregextract/OwnerGrid.js");
        }

        private void AddResource(IResourceManifestContainer container, string path)
		{
            container.Add(path, string.Format("Bars.Gkh.Regions.Chelyabinsk.dll/Bars.Gkh.Regions.Chelyabinsk.{0}", path.Replace("/", ".")));
        }
    }
}
