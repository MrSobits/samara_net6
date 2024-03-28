namespace Bars.GisIntegration.Base.Map.Inspection
{
    using Bars.GisIntegration.Base.Entities.Inspection;
    using Bars.GisIntegration.Base.Map;

    /// <summary>
    /// Маппинг для "Bars.GisIntegration.Inspection.Entities.ExaminationPlace"
    /// </summary>
    public class ExaminationPlaceMap : BaseRisEntityMap<ExaminationPlace>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public ExaminationPlaceMap()
            : base("Bars.GisIntegration.Inspection.Entities.ExaminationPlace", "GI_INSPECTION_EXAM_PLACE")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.Examination, "Проверка").Column("EXAMINATION_ID").NotNull().Fetch();
            this.Property(x => x.OrderNumber, "Порядковый номер").Column("ORDER_NUM");
            this.Property(x => x.FiasHouseGuid, "Гуид дома в ФИАС").Column("FIAS_HOUSE_GUID").Length(200);
        }
    }
}
