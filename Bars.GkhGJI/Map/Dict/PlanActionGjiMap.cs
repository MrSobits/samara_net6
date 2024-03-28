/// <mapping-converter-backup>
/// using Bars.B4.DataAccess;
/// 
/// namespace Bars.GkhGji.Map
/// {
///     using Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "План инспекционных проверок"
///     /// </summary>
///     public class PlanActionGjiMap : BaseEntityMap<PlanActionGji>
///     {
///         public PlanActionGjiMap()
///             : base("GJI_DICT_PLANACTION")
///         {
///             Map(x => x.Name, "NAME").Length(300).Not.Nullable();
///             Map(x => x.DateStart, "DATE_START");
///             Map(x => x.DateEnd, "DATE_END");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "План мероприятий"</summary>
    public class PlanActionGjiMap : BaseEntityMap<PlanActionGji>
    {
        
        public PlanActionGjiMap() : 
                base("План мероприятий", "GJI_DICT_PLANACTION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            Property(x => x.DateStart, "Дата начала").Column("DATE_START");
            Property(x => x.DateEnd, "Дата окончания").Column("DATE_END");
        }
    }
}
