/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.Administration
/// {
///     using Bars.Gkh.Entities;
/// 
///     public class OperatorInspectorMap : BaseGkhEntityMap<OperatorInspector>
///     {
///         public OperatorInspectorMap()
///             : base("GKH_OPERATOR_INSPECT")
///         {
///             References(x => x.Operator, "OPERATOR_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Inspector, "INSPECTOR_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Хранит инспекторов у оператора, по которым применяются фильтры"</summary>
    public class OperatorInspectorMap : BaseImportableEntityMap<OperatorInspector>
    {
        
        public OperatorInspectorMap() : 
                base("Хранит инспекторов у оператора, по которым применяются фильтры", "GKH_OPERATOR_INSPECT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.Operator, "Оператор").Column("OPERATOR_ID").NotNull().Fetch();
            Reference(x => x.Inspector, "Инспектор").Column("INSPECTOR_ID").NotNull().Fetch();
        }
    }
}
