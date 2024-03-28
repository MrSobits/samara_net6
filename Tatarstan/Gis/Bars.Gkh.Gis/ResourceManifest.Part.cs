namespace Bars.Gkh.Gis
{
    using B4;
    using B4.Modules.ExtJs;

    using Enum.IncrementalImport;
    using Enum.ManOrg;
    using Enum;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            container.RegisterExtJsEnum<CommonParamType>();
            container.RegisterExtJsEnum<TypeStatus>();
            container.RegisterExtJsEnum<GisTypeCondition>();
            container.RegisterExtJsEnum<GisTypeIndicator>();
            container.RegisterExtJsEnum<TypeAddressMatched>();
            container.RegisterExtJsEnum<TypeGroupIndicators>();
            container.RegisterExtJsEnum<TypeImportFormat>();
            container.RegisterExtJsEnum<TypeIncImportFormat>();
            container.RegisterExtJsEnum<TypeIncLoadStatus>();
            container.RegisterExtJsEnum<ImportResult>();
            container.RegisterExtJsEnum<TypeWaste>();
            container.RegisterExtJsEnum<TypeWasteCollectionPlace>();
            container.RegisterExtJsEnum<CommunalServiceName>();
            container.RegisterExtJsEnum<CommunalServiceResource>();
            container.RegisterExtJsEnum<ServiceWorkPurpose>();
            container.RegisterExtJsEnum<ServiceWorkType>();
            container.RegisterExtJsEnum<ConnectionType>();

            container.RegisterExtJsEnum<ConsumerByElectricEnergyType>();
            container.RegisterExtJsEnum<ConsumerType>();
            container.RegisterExtJsEnum<GisTariffKind>();
            container.RegisterExtJsEnum<SettelmentType>();
        }
    }
}