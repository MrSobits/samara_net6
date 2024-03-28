namespace Bars.Gkh.FillPassport1468
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Windsor;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Utils;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Container.RegisterResourceManifest<ResourceManifest>();
            Container.RegisterExecutionAction<CreatePassports>();
            Container.RegisterExecutionAction<FillPassports>();
            Container.RegisterController<Fill1468Controller>();
        }
    }
}
