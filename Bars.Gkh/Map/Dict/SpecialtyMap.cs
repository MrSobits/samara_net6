/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Специальности"
///     /// </summary>
///     public class SpecialtyMap : BaseGkhEntityMap<Specialty>
///     {
///         public SpecialtyMap()
///             : base("GKH_DICT_SPECIALTY")
///         {
///             Map(x => x.Name, "NAME").Not.Nullable().Length(300);
///             Map(x => x.Description, "DESCRIPTION").Length(500);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Специальность"</summary>
    public class SpecialtyMap : BaseImportableEntityMap<Specialty>
    {
        
        public SpecialtyMap() : 
                base("Специальность", "GKH_DICT_SPECIALTY")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
        }
    }
}
