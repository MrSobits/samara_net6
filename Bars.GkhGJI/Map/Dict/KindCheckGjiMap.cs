/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map.Dict
/// {
///     using B4.DataAccess;
/// 
///     using Enums;
///     using Entities;
/// 
///     /// <summary>
///     /// Маппинг сущности "Вид проверки"
///     /// </summary>
///     public class KindCheckGjiMap : BaseEntityMap<KindCheckGji>
///     {
///         public KindCheckGjiMap() : base("GJI_DICT_KIND_CHECK")
///         {
///             Map(x => x.Name, "NAME").Length(300);
///             Map(x => x.Description, "DESCRIPTION").Length(500);
///             Map(x => x.Code, "CODE").Not.Nullable().CustomType<TypeCheck>();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Вид проверки"</summary>
    public class KindCheckGjiMap : BaseEntityMap<KindCheckGji>
    {
        
        public KindCheckGjiMap() : 
                base("Вид проверки", "GJI_DICT_KIND_CHECK")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Наименование").Column("NAME").Length(300);
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            Property(x => x.Code, "Тип проверки, в этом справочнике он выполняет роль кода").Column("CODE").NotNull();
        }
    }
}
