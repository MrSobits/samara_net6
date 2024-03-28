using System;
using Bars.B4.DataAccess;
using Bars.B4.IoC;
using Bars.B4.Modules.NHibernateChangeLog;
using Bars.B4.Modules.Security;
using Bars.B4.Windsor;
using Bars.Gkh.UserActionRetention.Controllers;
using Bars.Gkh.UserActionRetention.DomainService;
using Bars.Gkh.UserActionRetention.DomainService.Impl;
using Bars.Gkh.UserActionRetention.ViewModel;
using Castle.MicroKernel.Registration;

namespace Bars.Gkh.UserActionRetention
{
    using B4;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Container.Register(
                Component.For<IResourceManifest>().Named("GkhUserActionRetention resources").ImplementedBy<ResourceManifest>()
                .LifeStyle.Transient);

            Container.Register(Component.For<IPermissionSource>().ImplementedBy<PermissionMap>());
            Container.RegisterTransient<INavigationProvider, NavigationProvider>("GkhUserActionRetention navigation");

            Container.Register(Component.For<IClientRouteMapRegistrar>().ImplementedBy<ClientRouteMapRegistrar>().LifestyleTransient());

            Container.RegisterAltDataController<LogEntity>();
            Container.RegisterViewModel<LogEntity, LogEntityViewModel>();
            Container.RegisterAltDataController<LogEntityProperty>();
            Container.RegisterViewModel<LogEntityProperty, LogEntityPropertyViewModel>();
            Container.RegisterAltDataController<User>();
            Container.RegisterViewModel<User, UserLoginViewModel>();

            Container.RegisterController<AuditLogMapController>();
            Container.RegisterController<UserLoginController>();
            Container.RegisterController<UserActionRetentionController>();

            Container.RegisterTransient<IAuditLogMapService, AuditLogMapService>();
            Container.RegisterTransient<IUserLoginService, UserLoginService>();
            Container.RegisterTransient<IUserActionRetentionService, UserActionRetentionService>();
        }
    }
}