/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Виды суда"
///     /// </summary>
///     public class TypeCourtGjiMap : BaseGkhEntityMap<TypeCourtGji>
///     {
///         public TypeCourtGjiMap()
///             : base("GJI_DICT_COURT")
///         {
///             Map(x => x.Name, "NAME").Length(300).Not.Nullable();
///             Map(x => x.Code, "CODE").Length(300);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "виды суда ГЖИ"</summary>
    public class TypeCourtGjiMap : BaseEntityMap<TypeCourtGji>
    {
        
        public TypeCourtGjiMap() : 
                base("виды суда ГЖИ", "GJI_DICT_COURT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            Property(x => x.Code, "Код").Column("CODE").Length(300);
        }
    }
}
