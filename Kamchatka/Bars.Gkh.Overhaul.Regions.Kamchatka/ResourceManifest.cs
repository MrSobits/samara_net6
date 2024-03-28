
namespace Bars.Gkh.Overhaul.Regions.Kamchatka
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            AddResource(container, "libs/B4/controller/OverhaulExtternalLinks.js");
            AddResource(container, "libs/B4/controller/import/KamchatkaRealtyObject.js");
            AddResource(container, "libs/B4/controller/realityobj/Edit.js");
            AddResource(container, "libs/B4/controller/report/ProgramCrByDpkrForm1.js");
            AddResource(container, "libs/B4/controller/report/ProgramCrByDpkrForm2.js");
            AddResource(container, "libs/B4/controller/report/RealityObjectDataReport.js");
            AddResource(container, "libs/B4/controller/report/TurnoverBalanceSheet.js");
            AddResource(container, "libs/B4/view/FramePanel.js");
            AddResource(container, "libs/B4/view/import/KamchatkaRealtyObjectPanel.js");
            AddResource(container, "libs/B4/view/realityobj/RealityObjToolbar.js");
            AddResource(container, "libs/B4/view/report/ProgramCrByDpkrForm1Panel.js");
            AddResource(container, "libs/B4/view/report/ProgramCrByDpkrForm2Panel.js");
            AddResource(container, "libs/B4/view/report/RealityObjectDataReportPanel.js");
            AddResource(container, "libs/B4/view/report/TurnoverBalanceSheetPanel.js");
            AddResource(container, "content/css/gkhOvrhlKamchatka.css");
            AddResource(container, "content/img/332.png");
            AddResource(container, "content/img/an.png");
            AddResource(container, "content/img/an_mon.png");
            AddResource(container, "content/img/mon.png");
            AddResource(container, "content/img/ucp.png");
        }

        private void AddResource(IResourceManifestContainer container, string path)
		{

            container.Add(path, string.Format("Bars.Gkh.Overhaul.Regions.Kamchatka.dll/Bars.Gkh.Overhaul.Regions.Kamchatka.{0}", path.Replace("/", ".")));
        }
    }
}
