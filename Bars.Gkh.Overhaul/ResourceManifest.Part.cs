namespace Bars.Gkh.Overhaul
{
    using Bars.B4;
    using Bars.B4.Modules.ExtJs;
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.Gkh.ConfigSections.Overhaul.Enums;
    using Bars.Gkh.Overhaul.Entities.Dict;
    using Bars.Gkh.Utils;

    using Enum;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            container.Add("libs/B4/enums/TypeFinSource.js", new ExtJsEnumResource<TypeFinSource>("B4.enums.TypeFinSource"));
            container.Add("libs/B4/enums/TypeIndicator.js", new ExtJsEnumResource<TypeIndicator>("B4.enums.TypeIndicator"));
            container.Add("libs/B4/enums/AccountType.js", new ExtJsEnumResource<AccountType>("B4.enums.AccountType"));
            container.Add("libs/B4/enums/ActionKind.js", new ExtJsEnumResource<ActionKind>("B4.enums.ActionKind"));
            container.Add("libs/B4/enums/CommonParamType.js", new ExtJsEnumResource<CommonParamType>("B4.enums.CommonParamType"));
            container.Add("libs/B4/enums/TypeUsage.js", new ExtJsEnumResource<TypeUsage>("B4.enums.TypeUsage"));

            container.RegisterExtJsEnum<EngineeringSystemType>();
            container.RegisterBaseDictController<BasisOverhaulDocKind>("Вид документа основания ДПКР", "GkhCr.Dict.BasisOverhaulDocKind");

            container.Add("WS/OverhaulService.svc", "Bars.Gkh.Overhaul.dll/Bars.Gkh.Overhaul.Services.Service.svc");
            container.Add("WS/RestOverhaulService.svc", "Bars.Gkh.Overhaul.dll/Bars.Gkh.Overhaul.Services.RestService.svc");
        }
    }
}