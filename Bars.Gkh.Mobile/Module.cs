namespace Bars.Gkh.Mobile
{
    using Bars.B4;
    using Bars.B4.Windsor;
    using Bars.Gkh.Mobile.Controllers;
    using Bars.Gkh.Overhaul.Hmao.Controllers;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Container.RegisterController<MobileCrController>();
        }
    }
}
