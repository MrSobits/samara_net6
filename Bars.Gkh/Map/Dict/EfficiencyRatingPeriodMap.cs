namespace Bars.Gkh.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Маппинг <see cref="EfficiencyRatingPeriod"/>
    /// </summary>
    public class EfficiencyRatingPeriodMap : BaseImportableEntityMap<EfficiencyRatingPeriod>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public EfficiencyRatingPeriodMap()
            : base("Bars.Gkh.Entities.Dicts.EfficiencyRatingPeriod", "GKH_DICT_EF_PERIOD")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.Name, "Наименование").Column("NAME").NotNull();
            this.Property(x => x.DateStart, "Дата начала").Column("DATE_START").NotNull();
            this.Property(x => x.DateEnd, "Дата окончания").Column("DATE_END").NotNull();

            this.Reference(x => x.Group, "Группа конструктора").Column("GROUP_ID").NotNull();
        }
    }
}