/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tyumen.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhGji.Regions.Tyumen.Entities;
/// 
///     public class NetworkOperatorMap : BaseEntityMap<NetworkOperator>
///     {
///         public NetworkOperatorMap() : base("GKH_NETWORK_OPERATOR")
///         {
///             References(x => x.Contragent, "CONTRAGENT_ID", ReferenceMapConfig.NotNullAndFetchAndCascadeDelete);
///             Map(x => x.Description, "DESCRIPTION");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tyumen.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tyumen.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tyumen.Entities.NetworkOperator"</summary>
    public class NetworkOperatorMap : BaseEntityMap<NetworkOperator>
    {
        
        public NetworkOperatorMap() : 
                base("Bars.GkhGji.Regions.Tyumen.Entities.NetworkOperator", "GKH_NETWORK_OPERATOR")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Contragent, "Contragent").Column("CONTRAGENT_ID").NotNull().Fetch();
            Property(x => x.Description, "Description").Column("DESCRIPTION").Length(250);
        }
    }
}
