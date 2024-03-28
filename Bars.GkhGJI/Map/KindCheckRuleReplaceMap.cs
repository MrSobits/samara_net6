/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using B4.DataAccess;
///     using Enums;
///     using Entities;
/// 
///     public class KindCheckRuleReplaceMap : BaseEntityMap<KindCheckRuleReplace>
///     {
///         public KindCheckRuleReplaceMap() : base("GJI_KIND_CHECK_RULE")
///         {
///             Map(x => x.Name, "NAME").Length(300);
///             Map(x => x.Code, "CODE").Not.Nullable().CustomType<TypeCheck>();
///             Map(x => x.RuleCode, "RULE_CODE").Length(100);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Entities.KindCheckRuleReplace"</summary>
    public class KindCheckRuleReplaceMap : BaseEntityMap<KindCheckRuleReplace>
    {
        
        public KindCheckRuleReplaceMap() : 
                base("Bars.GkhGji.Entities.KindCheckRuleReplace", "GJI_KIND_CHECK_RULE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Наименование").Column("NAME").Length(300);
            Property(x => x.Code, "Код вида проверки").Column("CODE").NotNull();
            Property(x => x.RuleCode, "Код правила").Column("RULE_CODE").Length(100);
        }
    }
}
