namespace Bars.GkhGji.Regions.Tomsk
{
    using B4;
    using B4.Modules.ExtJs;
    using Enums;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            container.Add("libs/B4/enums/TypeGender.js", new ExtJsEnumResource<TypeGender>("B4.enums.TypeGender"));
            container.Add("libs/B4/enums/TypeInitiativeOrgGji.js", new ExtJsEnumResource<TypeInitiativeOrgGji>("B4.enums.TypeInitiativeOrgGji"));
            container.Add("libs/B4/enums/TypeDocumentGji.js", new ExtJsEnumResource<TypeDocumentGji>("B4.enums.TypeDocumentGji"));
            container.Add("libs/B4/enums/TypeStage.js", new ExtJsEnumResource<TypeStage>("B4.enums.TypeStage"));
            container.Add("libs/B4/enums/TypeResolProsDefinition.js", new ExtJsEnumResource<TypeResolProsDefinition>("B4.enums.TypeResolProsDefinition"));
            container.Add("libs/B4/enums/TypeRequirement.js", new ExtJsEnumResource<TypeRequirement>("B4.enums.TypeRequirement"));
            container.Add("libs/B4/enums/TypeAdminCaseBase.js", new ExtJsEnumResource<TypeAdminCaseBase>("B4.enums.TypeAdminCaseBase"));
            container.Add("libs/B4/enums/TypeAdminCaseDoc.js", new ExtJsEnumResource<TypeAdminCaseDoc>("B4.enums.TypeAdminCaseDoc"));
            container.Add("libs/B4/enums/TypeResolutionPetitions.js", new ExtJsEnumResource<TypeResolutionPetitions>("B4.enums.TypeResolutionPetitions"));
        }
    }
}
