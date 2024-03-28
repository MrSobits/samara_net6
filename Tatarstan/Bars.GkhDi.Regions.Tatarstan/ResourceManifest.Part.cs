namespace Bars.GkhDi.Regions.Tatarstan
{
    using Bars.B4;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            container.Add("WS/DiServiceTat.svc", "Bars.GkhDi.Regions.Tatarstan.dll/Bars.GkhDi.Regions.Tatarstan.Services.Service.svc");
            container.Add("WS/RestDiServiceTat.svc", "Bars.GkhDi.Regions.Tatarstan.dll/Bars.GkhDi.Regions.Tatarstan.Services.RestService.svc");
        }
    }
}
