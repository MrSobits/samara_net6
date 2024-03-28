/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map.Dict
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг сущности "Сущность связи между Нарушением и Мероприятиями по устранению нарушений"
///     /// </summary>
///     public class ViolationActionsRemovGjiMap : BaseEntityMap<ViolationActionsRemovGji>
///     {
///         public ViolationActionsRemovGjiMap()
///             : base("GJI_DICT_VIOLACTIONREMOV")
///         {
///             References(x => x.ViolationGji, "VIOLATION_ID").Not.Nullable().Fetch.Join();
///             References(x => x.ActionsRemovViol, "ACTIONSREMOVVIOL_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Сущность связи между Нарушением и Мероприятиями по устранению нарушений"</summary>
    public class ViolationActionsRemovGjiMap : BaseEntityMap<ViolationActionsRemovGji>
    {
        
        public ViolationActionsRemovGjiMap() : 
                base("Сущность связи между Нарушением и Мероприятиями по устранению нарушений", "GJI_DICT_VIOLACTIONREMOV")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ViolationGji, "Нарушение").Column("VIOLATION_ID").NotNull().Fetch();
            Reference(x => x.ActionsRemovViol, "Мероприятия по устранению нарушений").Column("ACTIONSREMOVVIOL_ID").NotNull().Fetch();
        }
    }
}
