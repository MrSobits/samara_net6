/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.GkhDi.Entities;
///     using Bars.B4.DataAccess;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Настраиваемые поля шаблонной услуги"
///     /// </summary>
///     public class TemplateServiceOptionFieldsMap : BaseImportableEntityMap<TemplateServiceOptionFields>
///     {
///         public TemplateServiceOptionFieldsMap(): base("DI_TEMPL_SERV_OPT_FIELDS")
///         {
///             Map(x => x.Name, "NAME").Length(300);
///             Map(x => x.FieldName, "FIELD_NAME").Length(300);
///             Map(x => x.IsHidden, "IS_HIDDEN").Not.Nullable();
/// 
///             References(x => x.TemplateService, "TEMPLATE_SERVICE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.TemplateServiceOptionFields"</summary>
    public class TemplateServiceOptionFieldsMap : BaseImportableEntityMap<TemplateServiceOptionFields>
    {
        
        public TemplateServiceOptionFieldsMap() : 
                base("Bars.GkhDi.Entities.TemplateServiceOptionFields", "DI_TEMPL_SERV_OPT_FIELDS")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Name").Column("NAME").Length(300);
            Property(x => x.FieldName, "FieldName").Column("FIELD_NAME").Length(300);
            Property(x => x.IsHidden, "IsHidden").Column("IS_HIDDEN").NotNull();
            Reference(x => x.TemplateService, "TemplateService").Column("TEMPLATE_SERVICE_ID").Fetch();
        }
    }
}
