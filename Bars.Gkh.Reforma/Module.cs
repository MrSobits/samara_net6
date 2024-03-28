namespace Bars.Gkh.Reforma
{
    using System;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.Events;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Quartz;
    using Bars.B4.Windsor;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.Reforma.Controllers;
    using Bars.Gkh.Reforma.Controllers.Dict;
    using Bars.Gkh.Reforma.Controllers.Log;
    using Bars.Gkh.Reforma.Domain;
    using Bars.Gkh.Reforma.Domain.Impl;
    using Bars.Gkh.Reforma.ExecutionAction;
    using Bars.Gkh.Reforma.GroupAction.Impl;
    using Bars.Gkh.Reforma.Impl;
    using Bars.Gkh.Reforma.Impl.DataCollectors;
    using Bars.Gkh.Reforma.Interceptors;
    using Bars.Gkh.Reforma.Interface;
    using Bars.Gkh.Reforma.Interface.DataCollectors;
    using Bars.Gkh.Reforma.Interface.Performer;
    using Bars.Gkh.Reforma.Navigation;
    using Bars.Gkh.Reforma.PerformerActions.GetCompanyProfile;
    using Bars.Gkh.Reforma.PerformerActions.GetHouseInfo;
    using Bars.Gkh.Reforma.PerformerActions.GetHouseList;
    using Bars.Gkh.Reforma.PerformerActions.GetReportingPeriodList;
    using Bars.Gkh.Reforma.PerformerActions.GetRequestList;
    using Bars.Gkh.Reforma.PerformerActions.SetCompanyProfile;
    using Bars.Gkh.Reforma.PerformerActions.SetHouseLinkToOrganization;
    using Bars.Gkh.Reforma.PerformerActions.SetHouseProfile;
    using Bars.Gkh.Reforma.PerformerActions.SetHouseUnlinkFromOrganization;
    using Bars.Gkh.Reforma.PerformerActions.SetNewCompany;
    using Bars.Gkh.Reforma.PerformerActions.SetNewHouse;
    using Bars.Gkh.Reforma.PerformerActions.SetRequestForSubmit;
    using Bars.Gkh.Reforma.Tasks;
    using Bars.Gkh.Reforma.Utils.Validation;
    using Bars.Gkh.Utils;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.GroupAction;

    using Castle.MicroKernel.Registration;
    using Castle.Windsor;

    using global::Quartz;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            // Групповые операции по интеграции с реформой
            this.Container.RegisterSingleton<IDiGroupAction, ReformaManorgManualIntegration>();
            this.Container.RegisterSingleton<IDiGroupAction, ReformaRealityObjManualIntegration>();

            this.Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();

            this.Container.RegisterTransient<INavigationProvider, NavigationProvider>();
            this.Container.RegisterTransient<INavigationProvider, ManOrgMenuProvider>();
            this.Container.RegisterTransient<INavigationProvider, RealityObjMenuProvider>();

            this.Container.RegisterPermissionMap<ReformaPermissionMap>();

            this.RegisterSyncAction();
            this.RegisterInterceptors();
            this.RegisterServices();
            this.RegisterControllers();
            this.RegisterExecutionActions();

            // Регистрируем манифест ресурсов 
            this.Container.RegisterResourceManifest<ResourceManifest>();

            this.RegistrationQuartz();

            ReformaValidator.Init();
        }

        private void RegistrationQuartz()
        {
            // Регистрация только для веб-приложений
            if (ApplicationContext.Current.GetContextType() == ApplicationContextType.WebApplication)
            {
                this.Container.RegisterTransient<ITask, SetCompanyProfile988Task>();
                this.Container.RegisterTransient<ITask, SetHouseProfile988Task>();

                this.Container.Resolve<IEventAggregator>().GetEvent<AppStartEvent>().Subscribe<InitScheduler>();

                this.RegisterTaskScheduler();
            }
        }

        private void RegisterTaskScheduler()
        {
            Component.For<ISchedulerFactory>()
                .LifestyleSingleton()
                .UsingFactoryMethod(x => SchedulerFactoryInitializer.CreateSchedulerFactory())
                .Named("ReformaTaskSchedulerFactory")
                .RegisterIn(this.Container);

            Component.For<IScheduler>()
                .Named("ReformaTaskScheduler")
                .UsingFactoryMethod((k, cc) =>
                {
                    var scheduler = k.Resolve<ISchedulerFactory>("ReformaTaskSchedulerFactory").GetScheduler().GetResultWithoutContext();

                    if (!scheduler.IsStarted)
                    {
                        scheduler.Start();
                    }

                    return scheduler;
                })
                .LifestyleSingleton()
                .RegisterIn(this.Container);
        }

        private void RegisterExecutionActions()
        {
            this.Container.RegisterExecutionAction<SetRefFileNullReportingPeriod>();
        }

        private void RegisterInterceptors()
        {
            this.Container.RegisterDomainInterceptor<PeriodDi, PeriodDiInterceptor>();

            this.Container.RegisterDomainInterceptor<ContragentContact, GenericEntityChangeInterceptor<ContragentContact>>();
            this.Container.RegisterDomainInterceptor<ManagingOrganization, GenericEntityChangeInterceptor<ManagingOrganization>>();
            this.Container.RegisterDomainInterceptor<ManOrgContractRealityObject, GenericEntityChangeInterceptor<ManOrgContractRealityObject>>();
            this.Container.RegisterDomainInterceptor<ManagingOrgWorkMode, GenericEntityChangeInterceptor<ManagingOrgWorkMode>>();
            this.Container.RegisterDomainInterceptor<ManagingOrgMembership, GenericEntityChangeInterceptor<ManagingOrgMembership>>();
            this.Container.RegisterDomainInterceptor<FinActivity, GenericEntityChangeInterceptor<FinActivity>>();
            this.Container.RegisterDomainInterceptor<FinActivityCommunalService, GenericEntityChangeInterceptor<FinActivityCommunalService>>();
            this.Container.RegisterDomainInterceptor<FinActivityManagCategory, GenericEntityChangeInterceptor<FinActivityManagCategory>>();
            this.Container.RegisterDomainInterceptor<AdminResp, GenericEntityChangeInterceptor<AdminResp>>();
            this.Container.RegisterDomainInterceptor<DisclosureInfo, GenericEntityChangeInterceptor<DisclosureInfo>>();
            this.Container.RegisterDomainInterceptor<FinActivityRepairCategory, GenericEntityChangeInterceptor<FinActivityRepairCategory>>();
            this.Container.RegisterDomainInterceptor<FinActivityRepairSource, GenericEntityChangeInterceptor<FinActivityRepairSource>>();
            this.Container.RegisterDomainInterceptor<Contragent, GenericEntityChangeInterceptor<Contragent>>();
            this.Container.RegisterDomainInterceptor<RealityObject, GenericEntityChangeInterceptor<RealityObject>>();
            this.Container.RegisterDomainInterceptor<DisclosureInfoRealityObj, GenericEntityChangeInterceptor<DisclosureInfoRealityObj>>();
            this.Container.RegisterDomainInterceptor<CommunalService, GenericEntityChangeInterceptor<CommunalService>>();
            this.Container.RegisterDomainInterceptor<FinActivityRealityObjCommunalService, GenericEntityChangeInterceptor<FinActivityRealityObjCommunalService>>();
            this.Container.RegisterDomainInterceptor<FileInfo, GenericEntityChangeInterceptor<FileInfo>>();
            this.Container.RegisterDomainInterceptor<FinActivityManagRealityObj, GenericEntityChangeInterceptor<FinActivityManagRealityObj>>();
            this.Container.RegisterDomainInterceptor<TehPassportValue, GenericEntityChangeInterceptor<TehPassportValue>>();
            this.Container.RegisterDomainInterceptor<RealityObject, RealityObjectInterceptor>();
        }

        private void RegisterServices()
        {
            Container.RegisterSingleton<IClassMerger, ClassMerger>();

            this.Container.RegisterTransient<ISyncLogService, SyncLogService>();

            //this.Container.RegisterPerSession<IManOrgService, ManOrgService>();
            this.Container.RegisterPerSession<ManOrgService, ManOrgService>();
            this.Container.Register(Component.For<IManOrgService>().UsingFactoryMethod(() => ManOrgService.Instance, true).LifeStyle.Transient);

            this.Container.RegisterPerSession<IRobjectService, RobjectService>();

            this.Container.RegisterTransient<IEntityChangeTracker, EntityChangeTracker>();

            this.Container.RegisterTransient<IManOrgDataCollector, IManOrg988DataCollector, ManOrgDataCollector>();
            this.Container.RegisterTransient<IRobjectDataCollector, IRobject988DataCollector, RobjectDataCollector>();
            this.Container.RegisterTransient<IManualIntegrationService, ManualIntegrationService>();
        }

        private void RegisterControllers()
        {
            this.Container.RegisterController<ReformaController>();
            this.Container.RegisterController<SyncLogController>();

            this.Container.RegisterController<ReportingPeriodDictController>();

            this.Container.RegisterController<DataCollectorTestController>();
            this.Container.RegisterController<ManualIntegrationController>();
        }

        private void RegisterSyncAction()
        {
            this.Container.RegisterTransient<ISyncService, SyncService>();
            this.Container.RegisterTransient<ISyncProvider, SyncProvider>();

            this.Container.RegisterAction<GetRequestListAction>(GetRequestListAction.ActionId);
            this.Container.RegisterAction<SetRequestForSubmitAction>(SetRequestForSubmitAction.ActionId);
            this.Container.RegisterAction<SetNewCompanyAction>(SetNewCompanyAction.ActionId);
            this.Container.RegisterAction<GetReportingPeriodListAction>(GetReportingPeriodListAction.ActionId);
            this.Container.RegisterAction<GetCompanyProfileAction>(GetCompanyProfileAction.ActionId);
            this.Container.RegisterAction<GetCompanyProfile988Action>(GetCompanyProfile988Action.ActionId);
            this.Container.RegisterAction<SetCompanyProfileAction>(SetCompanyProfileAction.ActionId);
            this.Container.RegisterAction<SetCompanyProfile988Action>(SetCompanyProfile988Action.ActionId);
            this.Container.RegisterAction<GetHouseInfoAction>(GetHouseInfoAction.ActionId);
            this.Container.RegisterAction<GetHouseListAction>(GetHouseListAction.ActionId);
            this.Container.RegisterAction<SetNewHouseAction>(SetNewHouseAction.ActionId);
            this.Container.RegisterAction<SetHouseLinkToOrganizationAction>(SetHouseLinkToOrganizationAction.ActionId);
            this.Container.RegisterAction<SetUnlinkFromOrganizationAction>(SetUnlinkFromOrganizationAction.ActionId);
            this.Container.RegisterAction<SetHouseProfileAction>(SetHouseProfileAction.ActionId);
            this.Container.RegisterAction<SetHouseProfile988Action>(SetHouseProfile988Action.ActionId);

        }
    }

    internal static class WindsorExtensions
    {
        public static void RegisterAction<TAction>(this IWindsorContainer container, string id = null) where TAction : ISyncAction
        {
            container.Register(Component.For<ISyncAction, TAction>().ImplementedBy<TAction>().Named(id).LifestyleTransient());
        }

        public static void RegisterPerSession<TService, TImpl>(this IWindsorContainer container, Func<ComponentRegistration<TService>, ComponentRegistration<TService>> config = null)
            where TImpl : TService where TService : class
        {
            if (config == null)
            {
                config = registration => registration;
            }

            container.Register(config(Component.For<TService>().ImplementedBy<TImpl>()).LifeStyle.Scoped());
        }
    }
}