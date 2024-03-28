/// <mapping-converter-backup>
/// using Bars.B4.DataAccess;
/// using Bars.GkhGji.Entities;
/// using Bars.GkhGji.Enums;
/// 
/// namespace Bars.GkhGji.Map
/// {
///     /// <summary>
///     /// Маппинг сущности "Правило проставления номера документа гжи"
///     /// </summary>
///     public class DocNumValidationRuleMap : BaseEntityMap<DocNumValidationRule>
///     {
///         public DocNumValidationRuleMap() : base("GJI_DOCNUM_VALID_RULE")
///         {
///             Map(x => x.RuleId, "RULE_ID").Not.Nullable();
///             Map(x => x.TypeDocumentGji, "TYPE_DOCUMENT_GJI").Not.Nullable().CustomType<TypeDocumentGji>();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Правило проставления номера документа гжи"</summary>
    public class DocNumValidationRuleMap : BaseEntityMap<DocNumValidationRule>
    {
        
        public DocNumValidationRuleMap() : 
                base("Правило проставления номера документа гжи", "GJI_DOCNUM_VALID_RULE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.RuleId, "Id правила").Column("RULE_ID").NotNull();
            Property(x => x.TypeDocumentGji, "Тип документа").Column("TYPE_DOCUMENT_GJI").NotNull();
        }
    }
}
