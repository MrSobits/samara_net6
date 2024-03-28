using Bars.B4;
using Bars.B4.Modules.ExtJs;
// TODO: Расскоментировать после перевода FIAS
//using Bars.B4.Modules.FIAS.Enums;

namespace Bars.Gkh.Regions.Tyumen
{
    public partial class ResourceManifest
    {
        private void AdditionalInit(IResourceManifestContainer container)
        {
            container.Add("WS/Suggestion.svc", "Bars.Gkh.Regions.Tyumen.dll/Bars.Gkh.Regions.Tyumen.Services.Suggestion.svc");
            //container.Add("libs/B4/enums/FiasEstimateStatusEnum.js", new ExtJsEnumResource<FiasEstimateStatusEnum>("B4.enums.FiasEstimateStatusEnum"));
        }
    }
}