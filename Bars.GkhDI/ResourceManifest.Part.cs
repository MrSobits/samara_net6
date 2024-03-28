namespace Bars.GkhDi
{    
    using B4;
    using B4.Modules.ExtJs;

    using Bars.GkhDi.GroupAction;

    using Enums;

    public partial class ResourceManifest
    {

        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            container.Add("libs/B4/enums/TypePersonAdminResp.js", new ExtJsEnumResource<TypePersonAdminResp>("B4.enums.TypePersonAdminResp"));
            container.Add("libs/B4/enums/TypeSourceFundsDi.js", new ExtJsEnumResource<TypeSourceFundsDi>("B4.enums.TypeSourceFundsDi"));
            container.Add("libs/B4/enums/TypeCategoryHouseDi.js", new ExtJsEnumResource<TypeCategoryHouseDi>("B4.enums.TypeCategoryHouseDi"));
            container.Add("libs/B4/enums/TypeServiceDi.js", new ExtJsEnumResource<TypeServiceDi>("B4.enums.TypeServiceDi"));
            container.Add("libs/B4/enums/TypeAuditStateDi.js", new ExtJsEnumResource<TypeAuditStateDi>("B4.enums.TypeAuditStateDi"));
            container.Add("libs/B4/enums/TypeDocByYearDi.js", new ExtJsEnumResource<TypeDocByYearDi>("B4.enums.TypeDocByYearDi"));
            container.Add("libs/B4/enums/TypeContragentDi.js", new ExtJsEnumResource<TypeContragentDi>("B4.enums.TypeContragentDi"));
            container.Add("libs/B4/enums/TypeContractDi.js", new ExtJsEnumResource<TypeContractDi>("B4.enums.TypeContractDi"));
            container.Add("libs/B4/enums/TypeGroupServiceDi.js", new ExtJsEnumResource<TypeGroupServiceDi>("B4.enums.TypeGroupServiceDi"));
            container.Add("libs/B4/enums/KindServiceDi.js", new ExtJsEnumResource<KindServiceDi>("B4.enums.KindServiceDi"));
            container.Add("libs/B4/enums/TypeOfProvisionServiceDi.js", new ExtJsEnumResource<TypeOfProvisionServiceDi>("B4.enums.TypeOfProvisionServiceDi"));
            container.Add("libs/B4/enums/KindElectricitySupplyDi.js", new ExtJsEnumResource<KindElectricitySupplyDi>("B4.enums.KindElectricitySupplyDi"));
            container.Add("libs/B4/enums/RegionalFundDi.js", new ExtJsEnumResource<RegionalFundDi>("B4.enums.RegionalFundDi"));
            container.Add("libs/B4/enums/TariffIsSetForDi.js", new ExtJsEnumResource<TariffIsSetForDi>("B4.enums.TariffIsSetForDi"));
            container.Add("libs/B4/enums/EquipmentDi.js", new ExtJsEnumResource<EquipmentDi>("B4.enums.EquipmentDi"));
            container.Add("libs/B4/enums/TypeOrganSetTariffDi.js", new ExtJsEnumResource<TypeOrganSetTariffDi>("B4.enums.TypeOrganSetTariffDi"));

            container.Add("WS/DiService.svc", "Bars.GkhDi.dll/Bars.GkhDi.Services.Service.svc");
            container.Add("WS/RestDiService.svc", "Bars.GkhDi.dll/Bars.GkhDi.Services.RestService.svc");

            this.RegisterEnums(container);
        }

        private void RegisterEnums(IResourceManifestContainer container)
        {
            container.RegisterExtJsEnum<LesseeTypeDi>();
            container.RegisterExtJsEnum<TypeDiTargetAction>();
            container.RegisterExtJsEnum<DiFieldPathType>();
        }
    }
}
