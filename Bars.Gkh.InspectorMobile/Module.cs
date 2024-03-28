using Bars.B4;
using Bars.B4.IoC;
using Bars.B4.Windsor;
using Bars.Gkh.InspectorMobile.Api.Version1.Services;
using Bars.Gkh.InspectorMobile.Api.Version1.Services.Impl;
using Bars.Gkh.InspectorMobile.ConfigSections;
using Bars.Gkh.InspectorMobile.Controllers;
using Bars.Gkh.InspectorMobile.Entities;
using Bars.Gkh.InspectorMobile.Services;
using Bars.Gkh.InspectorMobile.Services.Impl;
using Bars.Gkh.InspectorMobile.ViewModels;
using Bars.Gkh.InspectorMobile.Api.Login.Services;
using Bars.Gkh.InspectorMobile.Api.Login.Services.Impl;
using Bars.Gkh.Utils;

namespace Bars.Gkh.InspectorMobile
{
    /// <inheritdoc />
    public class Module : AssemblyDefinedModule
    {
        /// <inheritdoc />
        public override void Install()
        {
            this.RegisterCommonComponents();
            this.RegisterConfigs();
            this.RegisterControllers();
            this.RegisterViewModels();
            this.RegisterServices();
            this.RegisterApiServices();
        }

        private void RegisterCommonComponents()
        {
            this.Container.RegisterTransient<IStartup, Startup>();

            this.Container.RegisterTransient<IResourceManifest, ResourceManifest>("Gkh.InspectorMobile resources");
        }

        private void RegisterConfigs()
        {
            this.Container.RegisterGkhConfig<InspectorMobileConfig>();
        }

        private void RegisterControllers()
        {
            this.Container.RegisterController<MpRoleController>();
        }

        private void RegisterViewModels()
        {
            this.Container.RegisterViewModel<MpRole, MpRoleViewModel>();
        }

        private void RegisterServices()
        {
            this.Container.RegisterTransient<IMpRoleService, MpRoleService>();
        }

        private void RegisterApiServices()
        {
            this.Container.RegisterTransient<IDocumentActCheckService, DocumentActCheckService>();
            this.Container.RegisterTransient<IDocumentActActionIsolatedService, DocumentActActionIsolatedService>();
            this.Container.RegisterTransient<IDocumentPrescriptionService, DocumentPrescriptionService>();
            this.Container.RegisterTransient<IDocumentDecisionService, DocumentDecisionService>();
            this.Container.RegisterTransient<IDocumentProtocolService, DocumentProtocolService>();
            this.Container.RegisterTransient<IDocumentVisitSheetService, DocumentVisitSheetService>();
            this.Container.RegisterTransient<IDocMotivatedPresentationService, DocMotivatedPresentationService>();
            this.Container.RegisterTransient<IDocumentTaskActionIsolatedService, DocumentTaskActionIsolatedService>();
            this.Container.RegisterTransient<IDocumentTaskPreventiveActionService, DocumentTaskPreventiveActionService>();
            this.Container.RegisterTransient<IDocumentActCheckActionService, DocumentActCheckActionService>();

            this.Container.RegisterTransient<IInspectionControlActionService, InspectionControlActionService>();
            this.Container.RegisterTransient<IInspectionPreventiveActionService, InspectionPreventiveActionService>();
            this.Container.RegisterTransient<IHeatingSeasonService, HeatingSeasonService>();
            this.Container.RegisterTransient<IRealityObjectService, RealityObjectService>();
            this.Container.RegisterTransient<IFilesService, FilesService>();
            this.Container.RegisterTransient<IDictService, DictService>();
            this.Container.RegisterTransient<IMobileLoginService, MobileLoginService>();
            this.Container.RegisterTransient<IObjectCrService, ObjectCrService>();
            this.Container.RegisterTransient<IUserInfoService, UserInfoService>();
            this.Container.RegisterTransient<IContragentService, ContragentService>();
        }
    }
}