/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Учебные заведения"
///     /// </summary>
///     public class InstitutionsMap : BaseGkhEntityMap<Institutions>
///     {
///         public InstitutionsMap()
///             : base("GKH_DICT_INSTITUTIONS")
///         {
///             Map(x => x.Name, "NAME").Not.Nullable().Length(300);
///             Map(x => x.Abbreviation, "ABBREVIATION").Length(50);
/// 
///             References(x => x.Address, "ADDRESS_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Учебное заведение"</summary>
    public class InstitutionsMap : BaseImportableEntityMap<Institutions>
    {
        
        public InstitutionsMap() : 
                base("Учебное заведение", "GKH_DICT_INSTITUTIONS")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            Property(x => x.Abbreviation, "Аббревиатура").Column("ABBREVIATION").Length(50);
            Reference(x => x.Address, "Адрес").Column("ADDRESS_ID").Fetch();
        }
    }
}
