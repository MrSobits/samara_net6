namespace Sobits.GisGkh
{
    using Bars.B4;

    /// <summary>
    /// Манифест ресурсов.
    /// Используется для регистрации ресурсов модуля в общем контейере ресурсов.
    /// </summary>
    public partial class ResourceManifest : ResourceManifestBase
    {
        protected override void BaseInit(IResourceManifestContainer container)
        {

            this.RegisterResource(container, "libs/B4/controller/GisGkhIntegration.js");
            this.RegisterResource(container, "libs/B4/cryptopro/jsxmlsigner.js");
            this.RegisterResource(container, "libs/B4/cryptopro/xadessigner.js");
            this.RegisterResource(container, "libs/B4/model/gisGkh/Certificate.js");
            this.RegisterResource(container, "libs/B4/model/gisGkh/ContragentForGisGkhExportModel.js");
            this.RegisterResource(container, "libs/B4/model/gisGkh/DictGridModel.js");
            this.RegisterResource(container, "libs/B4/model/gisGkh/DictItemModel.js");
            this.RegisterResource(container, "libs/B4/model/gisGkh/DisposalForGisGkhModel.js");
            this.RegisterResource(container, "libs/B4/model/gisGkh/DownloadGridModel.js");
            this.RegisterResource(container, "libs/B4/model/gisGkh/MOForGisGkhExportModel.js");
            this.RegisterResource(container, "libs/B4/model/gisGkh/PremisesGridModel.js");
            this.RegisterResource(container, "libs/B4/model/gisGkh/ProgramForGisGkhExportModel.js");
            this.RegisterResource(container, "libs/B4/model/gisGkh/ResolutionForGisGkhModel.js");
            this.RegisterResource(container, "libs/B4/model/gisGkh/ROForGisGkhExportModel.js");
            this.RegisterResource(container, "libs/B4/model/gisGkh/RoomGridModel.js");
            this.RegisterResource(container, "libs/B4/model/gisGkh/TaskGridModel.js");
            this.RegisterResource(container, "libs/B4/model/gisGkh/ProgramImportModel.js");
            this.RegisterResource(container, "libs/B4/model/gisGkh/ObjectCrImportModel.js");
            this.RegisterResource(container, "libs/B4/model/gisGkh/BuildContractImportModel.js");
            this.RegisterResource(container, "libs/B4/model/gisGkh/PerfWorkActImportModel.js");
            this.RegisterResource(container, "libs/B4/store/gisGkh/ContragentForGisGkhExportStore.js");
            this.RegisterResource(container, "libs/B4/store/gisGkh/DictGridStore.js");
            this.RegisterResource(container, "libs/B4/store/gisGkh/DictItemStore.js");
            this.RegisterResource(container, "libs/B4/store/gisGkh/DisposalForGisGkhStore.js");
            this.RegisterResource(container, "libs/B4/store/gisGkh/DownloadGridStore.js");
            this.RegisterResource(container, "libs/B4/store/gisGkh/MOForGisGkhExportStore.js");
            this.RegisterResource(container, "libs/B4/store/gisGkh/PremisesGridStore.js");
            this.RegisterResource(container, "libs/B4/store/gisGkh/ProgramForGisGkhExportStore.js");
            this.RegisterResource(container, "libs/B4/store/gisGkh/ResolutionForGisGkhStore.js");
            this.RegisterResource(container, "libs/B4/store/gisGkh/ROForGisGkhExportStore.js");
            this.RegisterResource(container, "libs/B4/store/gisGkh/RoomGridStore.js");
            this.RegisterResource(container, "libs/B4/store/gisGkh/TaskGridStore.js");
            this.RegisterResource(container, "libs/B4/store/gisGkh/TaskSignStore.js");
            this.RegisterResource(container, "libs/B4/store/gisGkh/ProgramImportStore.js");
            this.RegisterResource(container, "libs/B4/store/gisGkh/ObjectCrImportStore.js");
            this.RegisterResource(container, "libs/B4/store/gisGkh/BuildContractImportStore.js");
            this.RegisterResource(container, "libs/B4/store/gisGkh/BuildContractForActImportStore.js");
            this.RegisterResource(container, "libs/B4/store/gisGkh/PerfWorkActImportStore.js");
            this.RegisterResource(container, "libs/B4/view/gisGkh/DictGrid.js");
            this.RegisterResource(container, "libs/B4/view/gisGkh/DictItemGrid.js");
            this.RegisterResource(container, "libs/B4/view/gisGkh/DictWindow.js");
            this.RegisterResource(container, "libs/B4/view/gisGkh/DownloadGrid.js");
            this.RegisterResource(container, "libs/B4/view/gisGkh/EditWindow.js");
            this.RegisterResource(container, "libs/B4/view/gisGkh/MassSignWindow.js");
            this.RegisterResource(container, "libs/B4/view/gisGkh/Panel.js");
            this.RegisterResource(container, "libs/B4/view/gisGkh/PremisesGrid.js");
            this.RegisterResource(container, "libs/B4/view/gisGkh/ROGrid.js");
            this.RegisterResource(container, "libs/B4/view/gisGkh/RoomGrid.js");
            this.RegisterResource(container, "libs/B4/view/gisGkh/RoomMatchingPanel.js");
            this.RegisterResource(container, "libs/B4/view/gisGkh/SignWindow.js");
            this.RegisterResource(container, "libs/B4/view/gisGkh/TaskGrid.js");
            this.RegisterResource(container, "content/img/btnSend.png");
            this.RegisterResource(container, "content/img/btnSign.png");

        }
    }
}
