/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.Entities;
/// 
///     public class TemplateReplacementMap : BaseEntityMap<TemplateReplacement>
///     {
///         public TemplateReplacementMap() : base("GKH_TEMPLATE_REPLACEMENT")
///         {
///             Map(x => x.Code, "CODE").Not.Nullable().Length(255).Unique();
/// 
///             References(x => x.File, "FILE_INFO_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Замена шаблона"</summary>
    public class TemplateReplacementMap : BaseEntityMap<TemplateReplacement>
    {
        
        public TemplateReplacementMap() : 
                base("Замена шаблона", "GKH_TEMPLATE_REPLACEMENT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Code, "Код шаблона").Column("CODE").Length(255).NotNull();
            Reference(x => x.File, "Файл шаблона").Column("FILE_INFO_ID").NotNull().Fetch();
        }
    }
}
