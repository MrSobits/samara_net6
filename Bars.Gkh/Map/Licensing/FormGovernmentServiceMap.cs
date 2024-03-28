namespace Bars.Gkh.Map.Licensing
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Licensing;

    /// <summary>
    /// Маппинг <see cref="FormGovernmentService"/>
    /// </summary>
    public class FormGovernmentServiceMap : BaseImportableEntityMap<FormGovernmentService>
    {
        /// <inheritdoc />
        public FormGovernmentServiceMap()
            : base("Bars.Gkh.Entities.Licensing.FormGovernmentService", "GKH_FORM_GOV_SERVICE")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.GovernmentServiceType, "Государственная услуга").Column("SERVICE_TYPE").NotNull();
            this.Property(x => x.Year, "Год").Column("YEAR").NotNull();
            this.Property(x => x.Quarter, "Квартал").Column("QUARTER").NotNull();
        }
    }
}