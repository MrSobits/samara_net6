/// <mapping-converter-backup>
/// using Bars.Gkh.RegOperator.Entities;
/// 
/// 
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
/// 
///     public class DeliveryAgentMunicipalityMap : BaseImportableEntityMap<DeliveryAgentMunicipality>
///     {
///         public DeliveryAgentMunicipalityMap()
///             : base("REGOP_DEL_AGENT_MU")
///         {
///             References(x => x.DeliveryAgent, "DEL_AGENT_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.Municipality, "MUNICIPALITY_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Связь Агента доставки с МО"</summary>
    public class DeliveryAgentMunicipalityMap : BaseImportableEntityMap<DeliveryAgentMunicipality>
    {
        
        public DeliveryAgentMunicipalityMap() : 
                base("Связь Агента доставки с МО", "REGOP_DEL_AGENT_MU")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.DeliveryAgent, "Агент доставки").Column("DEL_AGENT_ID").NotNull().Fetch();
            Reference(x => x.Municipality, "МО").Column("MUNICIPALITY_ID").NotNull().Fetch();
        }
    }
}
