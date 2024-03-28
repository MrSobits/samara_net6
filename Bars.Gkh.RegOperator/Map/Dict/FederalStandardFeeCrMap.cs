/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map.Dict
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.RegOperator.Entities.Dict;
/// 
///     public class FederalStandardFeeCrMap : BaseImportableEntityMap<FederalStandardFeeCr>
///     {
///         public FederalStandardFeeCrMap()
///             : base("REGOP_FED_STANDARD_FEE_CR")
///         {
///             Map(x => x.DateStart, "DATE_START", true);
///             Map(x => x.DateEnd, "DATE_END");
///             Map(x => x.Value, "VALUE", true);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities.Dict;
    
    
    /// <summary>Маппинг для "Справочник "Федеральный стандарт взноса на КР""</summary>
    public class FederalStandardFeeCrMap : BaseImportableEntityMap<FederalStandardFeeCr>
    {
        
        public FederalStandardFeeCrMap() : 
                base("Справочник \"Федеральный стандарт взноса на КР\"", "REGOP_FED_STANDARD_FEE_CR")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DateStart, "Дата начала действия").Column("DATE_START").NotNull();
            Property(x => x.DateEnd, "Дата окончания действия").Column("DATE_END");
            Property(x => x.Value, "Значение").Column("VALUE").NotNull();
        }
    }
}
