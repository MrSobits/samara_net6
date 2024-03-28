/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
///     using Bars.GkhDi.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Периодичность услуги"
///     /// </summary>
///     public class PeriodicityTemplateServiceMap : BaseGkhEntityMap<PeriodicityTemplateService>
///     {
///         public PeriodicityTemplateServiceMap(): base("DI_DICT_PERIODICITY")
///         {
///             Map(x => x.Name, "NAME").Not.Nullable().Length(300);
///             Map(x => x.Code, "CODE").Length(300);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.PeriodicityTemplateService"</summary>
    public class PeriodicityTemplateServiceMap : BaseImportableEntityMap<PeriodicityTemplateService>
    {
        
        public PeriodicityTemplateServiceMap() : 
                base("Bars.GkhDi.Entities.PeriodicityTemplateService", "DI_DICT_PERIODICITY")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Name").Column("NAME").Length(300);
            Property(x => x.Code, "Code").Column("CODE").Length(300);
        }
    }
}
