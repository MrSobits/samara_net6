namespace Bars.Gkh.Regions.Nao
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Windsor;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Regions.Nao.Interceptors;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            //Ресурсы
            Container.RegisterTransient<IResourceManifest, ResourceManifest>();

            //Интерцепторы
            Container.RegisterDomainInterceptor<RealityObject, RealityObjectInterceptor>();

            //Ограничения
            Container.RegisterPermissionMap<PermissionMap>();

            Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();

            Container.RegisterTransient<INavigationProvider, NavigationProvider>();
        }
    }
}
