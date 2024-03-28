namespace Bars.Gkh.Map.Licensing
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Licensing;

    /// <summary>
    /// Маппинг <see cref="GovernmenServiceDetailGroup"/>
    /// </summary>
    public class GovernmenServiceDetailGroupMap : BaseImportableEntityMap<GovernmenServiceDetailGroup>
    {
        /// <inheritdoc />
        public GovernmenServiceDetailGroupMap()
            : base("Bars.Gkh.Entities.Licensing.GovernmenServiceDetailGroup", "GKH_SERVICE_DETAIL_GROUP")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.Name, "Наименование").Column("NAME");
            this.Property(x => x.GroupName, "Название группы (для отображения в fieldset)").Column("GROUP_NAME");
            this.Property(x => x.RowNumber, "Номер строки").Column("ROW_NUMBER").NotNull();
            this.Property(x => x.ServiceDetailSectionType, "Раздел").Column("SECTION").NotNull();
        }
    }
}