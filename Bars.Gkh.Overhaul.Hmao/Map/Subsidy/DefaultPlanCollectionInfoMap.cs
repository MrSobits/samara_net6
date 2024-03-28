/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Hmao.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Hmao.Entities;
/// 
///     public class DefaultPlanCollectionInfoMap : BaseImportableEntityMap<DefaultPlanCollectionInfo>
///     {
///         public DefaultPlanCollectionInfoMap()
///             : base("OVRHL_PLAN_COLL_INF")
///         {
///             Map(x => x.Year, "YEAR", true, 0);
///             Map(x => x.PlanOwnerPercent, "PLAN_OWN_PRC", true, 0);
///             Map(x => x.NotReduceSizePercent, "NOT_REDUCE_SIZE_PRC", true, 0);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    
    
    /// <summary>Маппинг для "Плановые показатели собираемости (по умолчанию)"</summary>
    public class DefaultPlanCollectionInfoMap : BaseImportableEntityMap<DefaultPlanCollectionInfo>
    {
        
        public DefaultPlanCollectionInfoMap() : 
                base("Плановые показатели собираемости (по умолчанию)", "OVRHL_PLAN_COLL_INF")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Year, "Год").Column("YEAR").NotNull();
            Property(x => x.PlanOwnerPercent, "(% Забивается в ручную) Процент собираемости (То есть есть вероятность того что н" +
                    "есоберут нужную сумма) и поэтому заводят процент который покажет Нормальную сумм" +
                    "у средств собственников").Column("PLAN_OWN_PRC").NotNull();
            Property(x => x.NotReduceSizePercent, "(% Забивается в ручную) Не снижаемый размер регионального фонда").Column("NOT_REDUCE_SIZE_PRC").NotNull();
        }
    }
}
