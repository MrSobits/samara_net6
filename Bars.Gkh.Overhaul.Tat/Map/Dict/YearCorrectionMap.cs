namespace Bars.Gkh.Overhaul.Tat.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;

    /// <summary>
    /// Маппинг <see cref="YearCorrection"/>
    /// </summary>
    public class YearCorrectionMap : BaseEntityMap<YearCorrection>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public YearCorrectionMap()
            : base("Bars.Gkh.Overhaul.Tat.Entities.YearCorrection", "OVRHL_DICT_YEAR_CORR")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.Year, "Год").Column("YEAR").NotNull();
        }
    }
}