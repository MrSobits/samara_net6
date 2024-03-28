/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.Administration
/// {
///     using Bars.Gkh.Entities;
/// 
///     public class OperatorContragentMap : BaseGkhEntityMap<OperatorContragent>
///     {
///         public OperatorContragentMap()
///             : base("GKH_OPERATOR_CONTRAGENT")
///         {
///             References(x => x.Operator, "OPERATOR_ID").Not.Nullable();
///             References(x => x.Contragent, "CONTRAGENT_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Entities.OperatorContragent"</summary>
    public class OperatorContragentMap : BaseImportableEntityMap<OperatorContragent>
    {
        
        public OperatorContragentMap() : 
                base("Bars.Gkh.Entities.OperatorContragent", "GKH_OPERATOR_CONTRAGENT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.Operator, "Оператор").Column("OPERATOR_ID").NotNull();
            Reference(x => x.Contragent, "УК").Column("CONTRAGENT_ID");
        }
    }
}
