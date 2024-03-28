/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Мероприятия по устранению нарушений"
///     /// </summary>
///     public class ActionsRemovViolMap : BaseEntityMap<ActionsRemovViol>
///     {
///         public ActionsRemovViolMap()
///             : base("GJI_DICT_ACTREMOVVIOL")
///         {
///             Map(x => x.Name, "NAME").Length(500).Not.Nullable();
///             Map(x => x.Code, "CODE").Length(300);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Мероприятия по устранению нарушений"</summary>
    public class ActionsRemovViolMap : BaseEntityMap<ActionsRemovViol>
    {
        
        public ActionsRemovViolMap() : 
                base("Мероприятия по устранению нарушений", "GJI_DICT_ACTREMOVVIOL")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "ДНаименование").Column("NAME").Length(500).NotNull();
            Property(x => x.Code, "Код").Column("CODE").Length(300);
        }
    }
}
