/// <mapping-converter-backup>
/// using Bars.Gkh.RegOperator.Entities;
/// 
/// 
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
/// 
///     public class DeliveryAgentMap : BaseImportableEntityMap<DeliveryAgent>
///     {
///         public DeliveryAgentMap()
///             : base("REGOP_DELIVERY_AGENT")
///         {
///             References(x => x.Contragent, "CONTRAGENT_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Агент доставки"</summary>
    public class DeliveryAgentMap : BaseImportableEntityMap<DeliveryAgent>
    {
        
        public DeliveryAgentMap() : 
                base("Агент доставки", "REGOP_DELIVERY_AGENT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").NotNull().Fetch();
        }
    }
}
