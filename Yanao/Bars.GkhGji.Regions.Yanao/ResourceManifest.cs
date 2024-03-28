namespace Bars.GkhGji.Regions.Yanao
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            AddResource(container, "libs/B4/DisposalTextValues.js");
            AddResource(container, "libs/B4/controller/report/Form1StateHousingInspection.js");
            AddResource(container, "libs/B4/mixins/ActCheck.js");
            AddResource(container, "libs/B4/view/appealcits/BaseStatementAddWindow.js");
            AddResource(container, "libs/B4/view/documentsgjiregister/DisposalGrid.js");
            AddResource(container, "libs/B4/view/report/Form1StateHousingInspectionPanel.js");

        }

        private void AddResource(IResourceManifestContainer container, string path)
		{
            container.Add(path, string.Format("Bars.GkhGji.Regions.Yanao.dll/Bars.GkhGji.Regions.Yanao.{0}", path.Replace("/", ".")));
        }
    }
}
