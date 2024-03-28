namespace Bars.GkhDi.Map.Service
{
    using Bars.Gkh.Map;
    using Bars.GkhDi.Entities.Service;

    public class TariffForConsumersOtherServiceMap : BaseImportableEntityMap<TariffForConsumersOtherService>
    {
        /// <inheritdoc />
        public TariffForConsumersOtherServiceMap()
            : base("DI_TARIFF_FCONSUMERS_OTHER_SERVICE")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.DateStart, "DateStart").Column("DATE_START");
            this.Property(x => x.DateEnd, "DateEnd").Column("DATE_END");
            this.Property(x => x.TariffIsSetFor, "TariffIsSetFor").Column("TARIFF_IS_SET_FOR").NotNull();
            this.Property(x => x.OrganizationSetTariff, "OrganizationSetTariff").Column("ORGANIZATION_SET_TARIFF");
            this.Property(x => x.TypeOrganSetTariffDi, "TypeOrganSetTariffDi").Column("TYPE_ORGAN_SET_TARIFF").NotNull();
            this.Property(x => x.Cost, "Cost").Column("COST");
            this.Property(x => x.CostNight, "CostNight").Column("COST_NIGHT");
            this.Reference(x => x.OtherService, "OtherService").Column("OTHER_SERVICE_ID").NotNull().Fetch();
        }
    }
}
