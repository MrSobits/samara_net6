/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
///     using Bars.GkhDi.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Система налогооблажения"
///     /// </summary>
///     public class TaxSystemMap : BaseGkhEntityMap<TaxSystem>
///     {
///         public TaxSystemMap() : base("DI_DICT_TAX_SYSTEM")
///         {
///             Map(x => x.Name, "NAME").Length(300);
///             Map(x => x.ShortName, "SHORT_NAME").Length(250);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.TaxSystem"</summary>
    public class TaxSystemMap : BaseImportableEntityMap<TaxSystem>
    {
        
        public TaxSystemMap() : 
                base("Bars.GkhDi.Entities.TaxSystem", "DI_DICT_TAX_SYSTEM")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Name").Column("NAME").Length(300);
            Property(x => x.ShortName, "ShortName").Column("SHORT_NAME").Length(250);
        }
    }
}
