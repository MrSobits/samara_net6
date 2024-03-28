/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Hmao.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Hmao.Entities.PriorityParams;
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

namespace Bars.Gkh.Overhaul.Hmao.Map.PriorityParams
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities.PriorityParams;
    
    
    /// <summary>Маппинг для "Параметры очередности. (Доп. поля)"</summary>
    public class PriorityParamAdditionMap : PersistentObjectMap<PriorityParamAddition>
    {
        
        public PriorityParamAdditionMap() : 
                base("Параметры очередности. (Доп. поля)", "OVRHL_PRIOR_PAR_ADDITION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Code, "Код").Column("CODE").Length(100);
            Property(x => x.AdditionFactor, "Доп.множитель").Column("ADDITION_FACTOR");
            Property(x => x.FactorValue, "Значение множителя").Column("FACTOR_VALUE").Length(300);
            Property(x => x.FinalValue, "Конечное значение").Column("FINAL_VALUE");
        }
    }
}
