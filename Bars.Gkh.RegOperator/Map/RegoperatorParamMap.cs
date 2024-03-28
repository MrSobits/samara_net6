/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class RegoperatorParamMap : BaseImportableEntityMap<RegoperatorParam>
///     {
///         public RegoperatorParamMap() : base("REGOP_PARAMETER")
///         {
///             Map(x => x.Key, "KEY", true);
///             Map(x => x.Value, "VALUE");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.RegOperator.Entities.RegoperatorParam"</summary>
    public class RegoperatorParamMap : BaseImportableEntityMap<RegoperatorParam>
    {
        
        public RegoperatorParamMap() : 
                base("Bars.Gkh.RegOperator.Entities.RegoperatorParam", "REGOP_PARAMETER")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Key, "Ключ").Column("KEY").Length(250).NotNull();
            Property(x => x.Value, "Значение").Column("VALUE").Length(250);
        }
    }
}
