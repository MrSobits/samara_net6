namespace Bars.Gkh.Map.Licensing
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Licensing;

    /// <summary>
    /// Маппинг <see cref="GovernmenServiceDetail"/>
    /// </summary>
    public class GovernmenServiceDetailMap : BaseImportableEntityMap<GovernmenServiceDetail>
    {
        /// <inheritdoc />
        public GovernmenServiceDetailMap()
            : base("Bars.Gkh.Entities.Licensing.GovernmenServiceDetail", "GKH_FORM_GOV_SERVICE_DETAIL")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.Value, "Значение показателя").Column("VALUE");

            this.Reference(x => x.DetailGroup, "Тип показателя").Column("GROUP_ID").NotNull().Fetch();
            this.Reference(x => x.FormGovernmentService, "Форма 1-ГУ").Column("GOV_SERVICE_ID").NotNull();
        }
    }
}