/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Показатели о готовности ЖКС к зимнему периоду"
///     /// </summary>
///     public class WorkInWinterMarkMap : BaseEntityMap<WorkInWinterMark>
///     {
///         public WorkInWinterMarkMap()
///             : base("GJI_WORK_WINTER_MARK")
///         {
///             Map(x => x.Name, "WORKMARK_NAME").Length(200).Not.Nullable();
///             Map(x => x.RowNumber, "WORKMARK_ROW");
///             Map(x => x.Measure, "WORKMARK_MEASURE").Length(50);
///             Map(x => x.Okei, "WORKMARK_OKEI").Length(50);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Entities.WorkInWinterMark"</summary>
    public class WorkInWinterMarkMap : BaseEntityMap<WorkInWinterMark>
    {
        
        public WorkInWinterMarkMap() : 
                base("Bars.GkhGji.Entities.WorkInWinterMark", "GJI_WORK_WINTER_MARK")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Name").Column("WORKMARK_NAME").Length(200).NotNull();
            Property(x => x.RowNumber, "RowNumber").Column("WORKMARK_ROW");
            Property(x => x.Measure, "Measure").Column("WORKMARK_MEASURE").Length(50);
            Property(x => x.Okei, "Okei").Column("WORKMARK_OKEI").Length(50);
        }
    }
}
