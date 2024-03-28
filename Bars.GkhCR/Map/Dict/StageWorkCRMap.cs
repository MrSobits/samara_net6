/// <mapping-converter-backup>
/// namespace Bars.GkhCr.Map
/// {
///     using Bars.Gkh.Map;;
///     using Bars.GkhCr.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Этап работы капитального ремонт"
///     /// </summary>
///     public class StageWorkCrMap : BaseGkhEntityMap<StageWorkCr>
///     {
///         public StageWorkCrMap() : base("CR_DICT_STAGE_WORK")
///         {
///             Map(x => x.Code, "CODE").Length(50);
///             Map(x => x.Name, "NAME").Length(300);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Этапы работ капитального ремонта"</summary>
    public class StageWorkCrMap : BaseImportableEntityMap<StageWorkCr>
    {
        
        public StageWorkCrMap() : 
                base("Этапы работ капитального ремонта", "CR_DICT_STAGE_WORK")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Code, "Код").Column("CODE").Length(50);
            Property(x => x.Name, "Наименование этапа").Column("NAME").Length(300);
        }
    }
}
