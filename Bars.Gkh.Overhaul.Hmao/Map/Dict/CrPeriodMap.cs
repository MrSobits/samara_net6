namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities;


    /// <summary>
    /// Маппинг для "Период программ КР"
    /// </summary>
    public class CrPeriodMap : BaseEntityMap<CrPeriod>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public CrPeriodMap() :
                base("Периоды программ капитального ремонта", "OVRHL_DICT_CR_PERIOD")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.YearStart, "Начальный год").Column("YEAR_START").NotNull();
            this.Property(x => x.YearEnd, "Конечный год").Column("YEAR_END").NotNull();
        }
    }
}
