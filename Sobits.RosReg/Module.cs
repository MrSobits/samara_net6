namespace Sobits.RosReg
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Windsor;
    using Bars.Gkh.Utils;

    using Sobits.RosReg.Controllers;
    using Sobits.RosReg.Entities;
    using Sobits.RosReg.ExecutionAction;
    using Sobits.RosReg.ExtractTypes;
    using Sobits.RosReg.Imports;
    using Sobits.RosReg.Interceptors;
    using Sobits.RosReg.Permissions;
    using Sobits.RosReg.Tasks.ExtractParse;
    using Sobits.RosReg.ViewModel;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            this.Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();
            this.Container.RegisterTransient<INavigationProvider, NavigationProvider>("RosReg menu");
            this.Container.RegisterResources<ResourceManifest>();
            Container.RegisterTransient<IPermissionSource, RosRegPermissionMap>();
            this.Container.RegisterImport<ExtractImport>();

            this.RegisterExtractTypes();
            this.RegisterControllers();
            this.RegisterViewModels();
            this.RegisterExecutorTasks();
            this.RegisterInterceptors();
            this.RegicterExecutionActions();
        }

        private void RegicterExecutionActions()
        {
            this.Container.RegisterExecutionAction<ParseOldExtractPaspExecutionAction>();
        }

        private void RegisterControllers()
        {
            this.Container.RegisterController<ExtractImportController>();
            this.Container.RegisterController<ExtractParseController>();
            this.Container.RegisterController<ExtractActionsController>();

            this.Container.RegisterAltDataController<Extract>();
            this.Container.RegisterAltDataController<ExtractEgrn>();
            this.Container.RegisterAltDataController<ExtractEgrnRight>();
            this.Container.RegisterAltDataController<ExtractEgrnRightInd>();
        }

        private void RegisterViewModels()
        {
            this.Container.RegisterViewModel<Extract, ExtractViewModel>();
            this.Container.RegisterViewModel<ExtractEgrn, ExtractEgrnViewModel>();
            this.Container.RegisterViewModel<ExtractEgrnRight, ExtractEgrnRightViewModel>();
            this.Container.RegisterViewModel<ExtractEgrnRightInd, ExtractEgrnRightIndViewModel>();
        }

        private void RegisterExtractTypes()
        {
            this.Container.RegisterTransient<IExtractType, EgrpReestrExtractObject06>();
            this.Container.RegisterTransient<IExtractType, EgrpReestrExtractSubject06>();
            this.Container.RegisterTransient<IExtractType, EgrpReestrExtractBig>();
            this.Container.RegisterTransient<IExtractType, EgrpReestrExtractAboutPropertyRoom>();
            this.Container.RegisterTransient<IExtractType, EgrnReestrExtractList07>();
            this.Container.RegisterTransient<IExtractType, EgrnReestrExtractBaseParamsRoom>();
            this.Container.RegisterTransient<IExtractType, EgrnReestrExtractTransferRights>();
            this.Container.RegisterTransient<IExtractType, EgrnReestrExtractCarParkingSpace>();
            this.Container.RegisterTransient<IExtractType, EgrnReestrExtractBaseParamsCarParkingSpace>();
        }

        private void RegisterExecutorTasks()
        {
            this.Container.RegisterTaskExecutor<ExtractParseTaskExecutor>(ExtractParseTaskExecutor.Id);
            this.Container.RegisterTaskExecutor<ParseOldExtractsPaspTaskExecutor>(ParseOldExtractsPaspTaskExecutor.Id);
        }
        private void RegisterInterceptors()
        {
            this.Container.RegisterDomainInterceptor<ExtractEgrn, ExtractEgrnInterceptor>();
        }
    }
}