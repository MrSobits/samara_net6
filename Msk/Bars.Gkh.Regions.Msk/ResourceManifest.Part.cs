namespace Bars.Gkh.Regions.Msk
{
    using Bars.B4;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            container.Add("WS/MskService.svc", "Bars.Gkh.Regions.Msk.dll/Bars.Gkh.Regions.Msk.Services.Service.svc");
        }
    }
}