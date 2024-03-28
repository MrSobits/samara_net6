namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Расчет категории риска"</summary>
    public class ROMCategoryMap : BaseEntityMap<ROMCategory>
    {
        
        public ROMCategoryMap() : 
                base("Расчет категории риска", "GJI_CH_ROM_CATEGORY")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.CalcDate, "Дата расчета").Column("CALC_DATE");
            Property(x => x.KindKND, "Тип КНД").Column("TYPE_KND");
            Property(x => x.MkdAreaTotal, "Общая площадь МКД").Column("MKD_AREA_TOTAL");
            Property(x => x.MonthCount, "Количество месяцев в управлении").Column("MONTH_COUNT");
            Property(x => x.Result, "Результат расчета").Column("RESULT");
            Property(x => x.RiskCategory, "Категория риска").Column("CATEGORY");
            Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
            Property(x => x.Vn, "Коэффициент Vн").Column("VN_AMMOUNT");
            Property(x => x.Vp, "Коэффициент Vп").Column("VP_AMMOUNT");
            Property(x => x.Vpr, "Коэффициент Vпр").Column("VPR_AMMOUNT");
            Property(x => x.YearEnums, "Год расчета").Column("YEAR");
            Reference(x => x.Inspector, "Инспектор").Column("INSPECTOR_ID").NotNull().Fetch();
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").NotNull().Fetch();
            

        }
    }
}
