namespace Bars.Gkh.DeloIntegration
{
    using Bars.B4;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            container.Add("WS/DeloService.svc", "Bars.Gkh.DeloIntegration.dll/Bars.Gkh.DeloIntegration.Wcf.DeloService.svc");
        }
    }
}
