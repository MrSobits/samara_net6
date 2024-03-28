
namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;


    /// <summary>Маппинг для "Виды рисков"</summary>
    public class CSCalculationRowMap : BaseEntityMap<CSCalculationRow>
    {

        public CSCalculationRowMap() :
                base("Виды рисков", "GKH_CS_CALCULATION_ROW")
        {
        }

        protected override void Map()
        {
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            Property(x => x.Code, "Код").Column("CODE").Length(300);
            Property(x => x.Value, "Значение").Column("VALUE").Length(300);
            Property(x => x.DisplayValue, "Наименование поля").Column("DISPLAY_TEXT").Length(300);
            Reference(x => x.CSCalculation, "Расчет").Column("CALC_ID");
        }
    }
}
