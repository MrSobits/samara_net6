namespace Bars.GkhCalendar
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Windsor;
    using Bars.GkhCalendar.Controllers;
    using Bars.GkhCalendar.DomainService;
    using Bars.GkhCalendar.DomainService.Impl;
    using Bars.GkhCalendar.Entities;
    using Bars.GkhCalendar.Interceptors;
    using Bars.GkhCalendar.ViewModel;

    using Castle.MicroKernel.Registration;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Container.RegisterTransient<INavigationProvider, NavigationProvider>("GkhCalendar navigation");
            Container.RegisterTransient<IResourceManifest, ResourceManifest>("GkhCalendar resources");
            Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();
            Container.RegisterSingleton<IPermissionSource, GkhCalendarPermissionMap>();
            //Container.Register(Component.For<IPermissionSource>().ImplementedBy<GkhCalendarPermissionMap>());

            Container.RegisterTransient<IDayService, DayService>();
            Container.RegisterSingleton<IIndustrialCalendarService, IndustrialCalendarService>();
            // Container.Register(Component.For<IDayService>().ImplementedBy<DayService>().LifeStyle.Transient);
            // Container.Register(Component.For<IIndustrialCalendarService>().ImplementedBy<IndustrialCalendarService>());
            Container.RegisterController<DayController>();
            Container.RegisterAltDataController<AppointmentQueue>();
            Container.RegisterAltDataController<AppointmentTime>();
            Container.RegisterAltDataController<AppointmentDiffDay>();

            Container.RegisterViewModel<Day, DayViewModel>();
            Container.RegisterViewModel<AppointmentQueue, AppointmentQueueViewModel>();
            Container.RegisterViewModel<AppointmentTime, AppointmentTimeViewModel>();
            Container.RegisterViewModel<AppointmentDiffDay, AppointmentDiffDayViewModel>();

            Container.RegisterDomainInterceptor<AppointmentQueue, AppointmentQueueInterceptor>();

        }
    }
}