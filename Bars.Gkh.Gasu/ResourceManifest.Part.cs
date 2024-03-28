namespace Bars.Gkh.Gasu
{
    using Bars.B4;
    using Bars.B4.Modules.ExtJs;
    using Bars.Gkh.Gasu.Entities;
    using Bars.Gkh.Gasu.Enums;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            container.RegisterExtJsEnum<Periodicity>();
            container.RegisterExtJsEnum<EbirModule>();

            container.RegisterExtJsModel<GasuIndicator>().Controller("GasuIndicator");
        }
    }
}