/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.Administration
/// {
///     using Bars.Gkh.Entities;
/// 
///     public class OperatorMunicipalityMap : BaseGkhEntityMap<OperatorMunicipality>
///     {
///         public OperatorMunicipalityMap() : base("GKH_OPERATOR_MUNIC")
///         {
///             References(x => x.Operator, "OPERATOR_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Municipality, "MUNICIPALITY_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Entities.OperatorMunicipality"</summary>
    public class OperatorMunicipalityMap : BaseImportableEntityMap<OperatorMunicipality>
    {
        
        public OperatorMunicipalityMap() : 
                base("Bars.Gkh.Entities.OperatorMunicipality", "GKH_OPERATOR_MUNIC")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.Operator, "Оператор").Column("OPERATOR_ID").NotNull().Fetch();
            Reference(x => x.Municipality, "Муниципальное образование").Column("MUNICIPALITY_ID").NotNull().Fetch();
        }
    }
}
