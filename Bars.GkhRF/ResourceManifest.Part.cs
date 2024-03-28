namespace Bars.GkhRf
{    
    using Bars.B4;
    using Bars.B4.Modules.ExtJs;
    using Bars.GkhRf.Enums;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            container.Add("libs/B4/enums/TypePayment.js", new ExtJsEnumResource<TypePayment>("B4.enums.TypePayment"));
            container.Add("libs/B4/enums/TypeCondition.js", new ExtJsEnumResource<TypePayment>("B4.enums.TypeCondition"));
            container.Add("libs/B4/enums/TypeProgramRequest.js", new ExtJsEnumResource<TypeProgramRequest>("B4.enums.TypeProgramRequest"));
            container.Add("libs/B4/enums/TypePaymentRfCtr.js", new ExtJsEnumResource<TypePaymentRfCtr>("B4.enums.TypePaymentRfCtr"));

            container.Add("WS/RfService.svc", "Bars.GkhRf.dll/Bars.GkhRf.Services.Service.svc");
            container.Add("WS/RestRfService.svc", "Bars.GkhRf.dll/Bars.GkhRf.Services.RestService.svc");
        }
    }
}
