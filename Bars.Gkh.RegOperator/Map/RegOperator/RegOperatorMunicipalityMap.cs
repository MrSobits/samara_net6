/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using B4.DataAccess;
///     using Entities;
/// 
///     /// <summary>
///     /// Маппинг сущности "Муниципальное образование регионального оператора"
///     /// </summary>
///     public class RegOperatorMunicipalityMap : BaseImportableEntityMap<RegOperatorMunicipality>
///     {
///         public RegOperatorMunicipalityMap()
///             : base("OVRHL_REG_OPERATOR_MU")
///         {
///             References(x => x.RegOperator, "REG_OPERATOR_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Municipality, "MUNICIPALITY_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Муниципальное образование регионального оператора"</summary>
    public class RegOperatorMunicipalityMap : BaseImportableEntityMap<RegOperatorMunicipality>
    {
        
        public RegOperatorMunicipalityMap() : 
                base("Муниципальное образование регионального оператора", "OVRHL_REG_OPERATOR_MU")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RegOperator, "Региональный оператор").Column("REG_OPERATOR_ID").NotNull().Fetch();
            Reference(x => x.Municipality, "Муниципальное образование").Column("MUNICIPALITY_ID").NotNull().Fetch();
        }
    }
}
