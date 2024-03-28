namespace Bars.Gkh.Overhaul.Tat
{
    using B4;
    using B4.Modules.ExtJs;

    using Enum;

    public partial class ResourceManifest : ResourceManifestBase
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            container.Add("libs/B4/enums/VersionActualizeType.js", new ExtJsEnumResource<VersionActualizeType>("B4.enums.VersionActualizeType"));
            container.Add("libs/B4/enums/AccountOperationType.js", new ExtJsEnumResource<AccountOperationType>("B4.enums.AccountOperationType"));
            container.Add("libs/B4/enums/TypeOrganization.js", new ExtJsEnumResource<TypeOrganization>("B4.enums.TypeOrganization"));
            container.Add("libs/B4/enums/PropertyOwnerDecisionType.js", new ExtJsEnumResource<PropertyOwnerDecisionType>("B4.enums.PropertyOwnerDecisionType"));
            container.Add("libs/B4/enums/MoOrganizationForm.js", new ExtJsEnumResource<MoOrganizationForm>("B4.enums.MoOrganizationForm"));
            container.Add("libs/B4/enums/PropertyOwnerProtocolType.js", new ExtJsEnumResource<PropertyOwnerProtocolType>("B4.enums.PropertyOwnerProtocolType"));
            container.Add("libs/B4/enums/PropertyOwnerDecisionType.js", new ExtJsEnumResource<PropertyOwnerDecisionType>("B4.enums.PropertyOwnerDecisionType"));
            container.Add("libs/B4/enums/OwnerAccountDecisionType.js", new ExtJsEnumResource<OwnerAccountDecisionType>("B4.enums.OwnerAccountDecisionType"));
            container.Add("libs/B4/enums/TypeCollectRealObj.js", new ExtJsEnumResource<TypeCollectRealObj>("B4.enums.TypeCollectRealObj"));
        }
    }
}