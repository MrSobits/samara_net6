/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class PriorityParamAdditionMap : PersistentObjectMap<PriorityParamAddition>
///     {
///         public PriorityParamAdditionMap()
///             : base("OVRHL_PRIOR_PAR_ADDITION")
///         {
///             Map(x => x.Code, "CODE", false, 100);
///             Map(x => x.FactorValue, "FACTOR_VALUE", false, 300);
///             Map(x => x.AdditionFactor, "ADDITION_FACTOR");
///             Map(x => x.FinalValue, "FINAL_VALUE");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.PriorityParamAddition"</summary>
    public class PriorityParamAdditionMap : PersistentObjectMap<PriorityParamAddition>
    {
        
        public PriorityParamAdditionMap() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.PriorityParamAddition", "OVRHL_PRIOR_PAR_ADDITION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Code, "Code").Column("CODE").Length(100);
            Property(x => x.AdditionFactor, "AdditionFactor").Column("ADDITION_FACTOR");
            Property(x => x.FactorValue, "FactorValue").Column("FACTOR_VALUE").Length(300);
            Property(x => x.FinalValue, "FinalValue").Column("FINAL_VALUE");
        }
    }
}
