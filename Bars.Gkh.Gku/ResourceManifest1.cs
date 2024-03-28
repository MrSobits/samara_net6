namespace Bars.Gkh.Gku
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

		AddResource(container, "libs/B4/controller/GkuInfo.js");
		AddResource(container, "libs/B4/controller/dict/GkuTarifGji.js");
		AddResource(container, "libs/B4/controller/gkuinfo/Edit.js");
		AddResource(container, "libs/B4/controller/gkuinfo/Navigation.js");
		AddResource(container, "libs/B4/controller/gkuinfo/housingcommunalservice/InfoOverview.js");
		AddResource(container, "libs/B4/controller/gkuinfo/housingcommunalservice/MeteringDeviceValue.js");
		AddResource(container, "libs/B4/controller/report/OperationalDataOfPayments.js");
		AddResource(container, "libs/B4/model/GkuInfo.js");
		AddResource(container, "libs/B4/model/dict/GkuTarifGji.js");
		AddResource(container, "libs/B4/store/GkuInfo.js");
		AddResource(container, "libs/B4/store/dict/GkuTarifGji.js");
		AddResource(container, "libs/B4/store/gkuinfo/NavigationMenu.js");
		AddResource(container, "libs/B4/view/dict/gkutarif/EditWindow.js");
		AddResource(container, "libs/B4/view/dict/gkutarif/Grid.js");
		AddResource(container, "libs/B4/view/gkuinfo/EditPanel.js");
		AddResource(container, "libs/B4/view/gkuinfo/Grid.js");
		AddResource(container, "libs/B4/view/gkuinfo/InfoOverviewEditPanel.js");
		AddResource(container, "libs/B4/view/gkuinfo/NavigationPanel.js");
		AddResource(container, "libs/B4/view/gkuinfo/TarifsGrid.js");
		AddResource(container, "libs/B4/view/report/OperationalDataOfPaymentsPanel.js");
        }

        private void AddResource(IResourceManifestContainer container, string path)
		{

            container.Add(path, string.Format("Bars.Gkh.Gku.dll/Bars.Gkh.Gku.{0}", path.Replace("/", ".")));
        }
    }
}
