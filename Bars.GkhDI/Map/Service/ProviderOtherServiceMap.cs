namespace Bars.GkhDi.Map.Service
{
    using Bars.Gkh.Map;
    using Bars.GkhDi.Entities.Service;

    public class ProviderOtherServiceMap : BaseImportableEntityMap<ProviderOtherService>
    {
        public ProviderOtherServiceMap() : base("DI_SERVICE_PROVIDER_OTHER_SERVICE")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.ProviderName, "ProviderName").Column("PROVIDER_NAME");
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.DateStartContract, "DateStartContract").Column("DATE_START_CONTRACT");
            this.Property(x => x.Description, "Description").Column("DESCRIPTION").Length(500);
            this.Property(x => x.IsActive, "IsActive").Column("IS_ACTIVE");
            this.Property(x => x.NumberContract, "NumberContract").Column("NUMBER_CONTRACT");
            this.Reference(x => x.OtherService, "OtherService").Column("OTHER_SERVICE_ID").NotNull().Fetch();
            this.Reference(x => x.Provider, "Provider").Column("PROVIDER_ID").Fetch();
        }
    }
}
