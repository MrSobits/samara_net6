/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Hmao.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Hmao.Entities;
/// 
///     public class MissingByMargCostDpkrRecMap : BaseImportableEntityMap<MissingByMargCostDpkrRec>
///     {
///         public MissingByMargCostDpkrRecMap()
///             : base("OVRHL_MISS_DPKR_REC")
///         {
///             References(x => x.RealityObject, "RO_ID", ReferenceMapConfig.NotNullAndFetch);
///             Map(x => x.Year, "YEAR", true, 0);
///             Map(x => x.Sum, "SUM", true, 0);
///             Map(x => x.CommonEstateObjects, "CEO_STRING", true);
///             Map(x => x.RealEstateTypeName, "REAL_EST_TYPE_NAME", true);
///             Map(x => x.MargSum, "MARG_REPAIR_COST");
///             Map(x => x.Area, "AREA_MKD");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    
    
    /// <summary>Маппинг для "Отсутствующий по предельной записи"</summary>
    public class MissingByMargCostDpkrRecMap : BaseImportableEntityMap<MissingByMargCostDpkrRec>
    {
        
        public MissingByMargCostDpkrRecMap() : 
                base("Отсутствующий по предельной записи", "OVRHL_MISS_DPKR_REC")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealityObject, "Жилой дом").Column("RO_ID").NotNull().Fetch();
            Property(x => x.Year, "Плановый Год").Column("YEAR").NotNull();
            Property(x => x.CommonEstateObjects, "Строка объектов общего имущества").Column("CEO_STRING").Length(250).NotNull();
            Property(x => x.Sum, "Плановая сумма").Column("SUM").NotNull();
            Property(x => x.Area, "Площадь дома, на момент расчета (в зависимости от параметра в настройках может бы" +
                    "ть: площадь МКД, жилая площадь, жилая и нежилая площадь)").Column("AREA_MKD");
            Property(x => x.MargSum, "Предельная сумма на кв.м, на момент расчета").Column("MARG_REPAIR_COST");
            Property(x => x.RealEstateTypeName, "Тип дома, на момент расчета").Column("REAL_EST_TYPE_NAME").Length(250).NotNull();
        }
    }
}
