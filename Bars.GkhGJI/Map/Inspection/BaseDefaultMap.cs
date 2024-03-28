/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.GkhGji.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     public class BaseDefaultMap : SubclassMap<BaseDefault>
///     {
///         public BaseDefaultMap()
///         {
///             Table("GJI_INSPECTION_BASEDEF");
///             KeyColumn("ID");
/// 
///             Map(x => x.ObjectVersion, "OBJECT_VERSION").Not.Nullable();
///             Map(x => x.ObjectCreateDate, "OBJECT_CREATE_DATE").Not.Nullable();
///             Map(x => x.ObjectEditDate, "OBJECT_EDIT_DATE").Not.Nullable();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Пустое основания, нужно только для отображения тех проверок и документов, у которых не было основания проверки на Б3"</summary>
    public class BaseDefaultMap : JoinedSubClassMap<BaseDefault>
    {
        
        public BaseDefaultMap() : 
                base("Пустое основания, нужно только для отображения тех проверок и документов, у котор" +
                        "ых не было основания проверки на Б3", "GJI_INSPECTION_BASEDEF")
        {
        }
        
        protected override void Map()
        {
        }
    }
}
