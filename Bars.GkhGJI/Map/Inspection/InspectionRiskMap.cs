namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <inheritdoc />
    public class InspectionRiskMap : BaseEntityMap<InspectionRisk>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public InspectionRiskMap() : 
                base("Риск проверки ГЖИ", "GJI_INSPECTION_RISK")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.Inspection, "Проверка ГЖИ").Column("INSPECTION_ID").NotNull();
            this.Reference(x => x.RiskCategory, "Категория риска").Column("RISK_CATEGORY_ID").NotNull();
            this.Property(x => x.StartDate, "Дата начала").Column("START_DATE").NotNull();
            this.Property(x => x.EndDate, "Дата окончания").Column("END_DATE");
        }
    }
}
