namespace Bars.Gkh.Reforma
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
		
            RegisterResource(container, "libs/B4/controller/ReformaRestore.js");
            RegisterResource(container, "libs/B4/controller/SyncLog.js");
            RegisterResource(container, "libs/B4/controller/SyncParams.js");
            RegisterResource(container, "libs/B4/controller/dict/ReportingPeriod.js");
            RegisterResource(container, "libs/B4/controller/manorg/ReformaManOrg.js");
            RegisterResource(container, "libs/B4/controller/manualintegration/RealityObjectIntegration.js");
            RegisterResource(container, "libs/B4/controller/robject/ReformaRobject.js");
            RegisterResource(container, "libs/B4/model/SyncAction.js");
            RegisterResource(container, "libs/B4/model/SyncActionDetails.js");
            RegisterResource(container, "libs/B4/model/SyncSession.js");
            RegisterResource(container, "libs/B4/model/dict/ReportingPeriod.js");
            RegisterResource(container, "libs/B4/model/manualintegration/RefRealityObject.js");
            RegisterResource(container, "libs/B4/model/manualintegration/RefRealityObjectSelected.js");
            RegisterResource(container, "libs/B4/store/SyncAction.js");
            RegisterResource(container, "libs/B4/store/SyncActionDetails.js");
            RegisterResource(container, "libs/B4/store/SyncSession.js");
            RegisterResource(container, "libs/B4/store/dict/ReportingPeriod.js");
            RegisterResource(container, "libs/B4/store/manualintegration/RefRealityObject.js");
            RegisterResource(container, "libs/B4/store/manualintegration/RefRealityObjectSelected.js");
            RegisterResource(container, "libs/B4/view/dict/reportingperiod/Grid.js");
            RegisterResource(container, "libs/B4/view/ManOrg/ReformaPanel.js");
            RegisterResource(container, "libs/B4/view/manualintegration/RobjectEditPanel.js");
            RegisterResource(container, "libs/B4/view/reformarestore/Panel.js");
            RegisterResource(container, "libs/B4/view/robject/ReformaPanel.js");
            RegisterResource(container, "libs/B4/view/synclog/DetailsWindow.js");
            RegisterResource(container, "libs/B4/view/synclog/Panel.js");
            RegisterResource(container, "libs/B4/view/syncparams/Panel.js");
        }             
    }
}
