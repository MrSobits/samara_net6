namespace Bars.Gkh.Regions.Tatarstan
{
    using Bars.B4;
    using Bars.B4.Modules.ExtJs;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Regions.Tatarstan.Enums;
    using Bars.Gkh.Regions.Tatarstan.Enums.Administration;
    using Bars.Gkh.Utils;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            this.RegisterEnums(container);
        }

        private void RegisterEnums(IResourceManifestContainer container)
        {
            container.RegisterExtJsEnum<ConstructionObjectDocumentType>();
            container.RegisterExtJsEnum<ConstructionObjectContractType>();
            container.RegisterExtJsEnum<ConstructionObjectParticipantType>();
            container.RegisterExtJsEnum<ConstructionObjectCustomerType>();
            container.RegisterExtJsEnum<ConstructionObjectPhotoGroup>();
            container.RegisterExtJsEnum<TerminationReasonType>();
            container.RegisterExtJsEnum<ExecutoryProcessDocumentType>();
            container.RegisterExtJsEnum<NormConsumptionType>();
            container.RegisterExtJsEnum<TypeHotWaterSystem>();
            container.RegisterExtJsEnum<TypeRisers>();
            container.RegisterExtJsEnum<EgsoTaskType>();
            container.RegisterExtJsEnum<EgsoTaskStateType>();
            container.RegisterExtJsEnum<ElementOutdoorGroup>();
            container.RegisterExtJsEnum<ConditionElementOutdoor>();
            container.RegisterExtJsEnum<KindWorkOutdoor>();
            container.RegisterGkhEnum("OutdoorImagesGroup", ImagesGroup.PictureHouse);
            container.RegisterExtJsEnum<IntercomUnitMeasure>();
            container.RegisterExtJsEnum<ChartersContractsRisExportType>();
            container.RegisterExtJsEnum<FsspFileState>();
            container.RegisterExtJsEnum<DebtRequestMailNotificationType>();
            container.RegisterExtJsEnum<TariffDataIntegrationMethod>();
            container.RegisterExtJsEnum<ExecutionStatus>();
        }
    }
}