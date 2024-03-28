/// <mapping-converter-backup>
/// using Bars.Gkh.RegOperator.Entities;
/// 
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
/// 
///     public class DeliveryAgentRealObjMap : BaseImportableEntityMap<DeliveryAgentRealObj>
///     {
///         public DeliveryAgentRealObjMap()
///             : base("REGOP_DEL_AGENT_REAL_OBJ")
///         {
///             Map(x => x.DateStart, "DATE_START", true);
///             Map(x => x.DateEnd, "DATE_END", false);
/// 
/// 
///             References(x => x.DeliveryAgent, "DEL_AGENT_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.RealityObject, "REAL_OBJ_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Жилой дом агента доставки"</summary>
    public class DeliveryAgentRealObjMap : BaseImportableEntityMap<DeliveryAgentRealObj>
    {
        
        public DeliveryAgentRealObjMap() : 
                base("Жилой дом агента доставки", "REGOP_DEL_AGENT_REAL_OBJ")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealityObject, "Жилой дом").Column("REAL_OBJ_ID").NotNull().Fetch();
            Reference(x => x.DeliveryAgent, "Агент доставки").Column("DEL_AGENT_ID").NotNull().Fetch();
            Property(x => x.DateStart, "Дата начала действия договора").Column("DATE_START").NotNull();
            Property(x => x.DateEnd, "Дата начала действия договора").Column("DATE_END");
        }
    }
}
