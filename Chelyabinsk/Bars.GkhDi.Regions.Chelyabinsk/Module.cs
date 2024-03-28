namespace Bars.GkhDi.Regions.Chelyabinsk
{
    using System;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.B4.Application;
    using Bars.B4.Events;
    using Bars.B4.IoC;
    using Castle.MicroKernel.Registration;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            // Регистрируем манифест ресурсов 
            Container.RegisterResourceManifest<ResourceManifest>();
        }
    }
}
