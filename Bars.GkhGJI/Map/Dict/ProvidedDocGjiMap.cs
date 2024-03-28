/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Предоставляемые документы"
///     /// </summary>
///     public class ProvidedDocGjiMap : BaseEntityMap<ProvidedDocGji>
///     {
///         public ProvidedDocGjiMap()
///             : base("GJI_DICT_PROVIDEDDOCUMENT")
///         {
///             Map(x => x.Name, "NAME").Length(2000).Not.Nullable();
///             Map(x => x.Code, "CODE").Length(300);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Предоставляемые документы ГЖИ"</summary>
    public class ProvidedDocGjiMap : BaseEntityMap<ProvidedDocGji>
    {
        
        public ProvidedDocGjiMap() : 
                base("Предоставляемые документы ГЖИ", "GJI_DICT_PROVIDEDDOCUMENT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Наименование").Column("NAME").Length(2000).NotNull();
            Property(x => x.Code, "Код").Column("CODE").Length(300);
        }
    }
}
