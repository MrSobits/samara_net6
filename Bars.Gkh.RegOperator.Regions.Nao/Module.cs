namespace Bars.Gkh.RegOperator.Regions.Nao
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.RegOperator.Regions.Nao.Imports;
    using Bars.Gkh.RegOperator.Regions.Nao.Permissions;
    using Bars.Gkh.Utils;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            //Ресурсы
            Container.RegisterTransient<INavigationProvider, Navigation.NavigationProvider>();

            //Интерцепторы
            //Container.RegisterDomainInterceptor<RealityObject, RealityObjectInterceptor>();

            //Ограничения
            Container.RegisterPermissionMap<NaoPermissionMap>();

            //Импорты
            Container.RegisterImport<QuarterImport>(QuarterImport.Id);
        }
    }
}
