namespace Bars.GkhEdoInteg
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            AddResource(container, "libs/B4/aspects/DocumentListForEmsed.js");
            AddResource(container, "libs/B4/controller/LogEdo.js");
            AddResource(container, "libs/B4/controller/LogEdoRequests.js");
            AddResource(container, "libs/B4/controller/SendEmsed.js");
            AddResource(container, "libs/B4/controller/dict/InspectorEdoInteg.js");
            AddResource(container, "libs/B4/controller/dict/KindStatementEdoInteg.js");
            AddResource(container, "libs/B4/controller/dict/RevenueFormEdoInteg.js");
            AddResource(container, "libs/B4/controller/dict/RevenueSourceEdoInteg.js");
            AddResource(container, "libs/B4/model/AppealCits.js");
            AddResource(container, "libs/B4/model/DocsForSendInEmsed.js");
            AddResource(container, "libs/B4/model/dict/InspectorEdoInteg.js");
            AddResource(container, "libs/B4/model/dict/KindStatementEdoInteg.js");
            AddResource(container, "libs/B4/model/dict/RevenueFormEdoInteg.js");
            AddResource(container, "libs/B4/model/dict/RevenueSourceEdoInteg.js");
            AddResource(container, "libs/B4/model/edolog/AppealCits.js");
            AddResource(container, "libs/B4/model/edolog/Requests.js");
            AddResource(container, "libs/B4/model/edolog/RequestsAppealCits.js");
            AddResource(container, "libs/B4/store/AppealCitsEdoInteg.js");
            AddResource(container, "libs/B4/store/DocsForSendInEmsed.js");
            AddResource(container, "libs/B4/store/dict/InspectorEdoInteg.js");
            AddResource(container, "libs/B4/store/dict/KindStatementEdoInteg.js");
            AddResource(container, "libs/B4/store/dict/RevenueFormEdoInteg.js");
            AddResource(container, "libs/B4/store/dict/RevenueSourceEdoInteg.js");
            AddResource(container, "libs/B4/store/edolog/AppealCits.js");
            AddResource(container, "libs/B4/store/edolog/Requests.js");
            AddResource(container, "libs/B4/store/edolog/RequestsAppealCits.js");
            AddResource(container, "libs/B4/view/EmsedGrid.js");
            AddResource(container, "libs/B4/view/EmsedListPanel.js");
            AddResource(container, "libs/B4/view/appealcits/EdoPanel.js");
            AddResource(container, "libs/B4/view/appealcits/Grid.js");
            AddResource(container, "libs/B4/view/appealcits/SelectionGrid.js");
            AddResource(container, "libs/B4/view/dict/inspEdoIntegGrid.js");
            AddResource(container, "libs/B4/view/dict/kindStEdoIntegGrid.js");
            AddResource(container, "libs/B4/view/dict/revFormEdoIntegGrid.js");
            AddResource(container, "libs/B4/view/dict/revsourceedointeg/EditWindow.js");
            AddResource(container, "libs/B4/view/dict/revsourceedointeg/Grid.js");
            AddResource(container, "libs/B4/view/edolog/FilterPanel.js");
            AddResource(container, "libs/B4/view/edolog/Grid.js");
            AddResource(container, "libs/B4/view/edolog/Panel.js");
            AddResource(container, "libs/B4/view/edologrequests/AppealCitsGrid.js");
            AddResource(container, "libs/B4/view/edologrequests/EditWindow.js");
            AddResource(container, "libs/B4/view/edologrequests/FilterPanel.js");
            AddResource(container, "libs/B4/view/edologrequests/Grid.js");
            AddResource(container, "libs/B4/view/edologrequests/Panel.js");
        }

        private void AddResource(IResourceManifestContainer container, string path)
		{

            container.Add(path, string.Format("Bars.GkhEdoInteg.dll/Bars.GkhEdoInteg.{0}", path.Replace("/", ".")));
        }
    }
}
