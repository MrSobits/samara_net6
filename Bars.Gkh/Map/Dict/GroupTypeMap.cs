/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.Dict
/// {
///     using B4.DataAccess.ByCode;
///     using Entities.CommonEstateObject;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Тип группы ООИ"
///     /// </summary>
///     public class GroupTypeMap : BaseImportableEntityMap<GroupType>
///     {
///         public GroupTypeMap()
///             : base("OVRHL_DICT_CEO_GROUP_TYPE")
///         {
///             Map(x => x.Code, "GROUP_TYPE_CODE", true, 200);
///             Map(x => x.Name, "NAME", true, 300);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map.CommonEstateObject
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.CommonEstateObject;
    
    
    /// <summary>Маппинг для "Тип группы ООИ"</summary>
    public class GroupTypeMap : BaseImportableEntityMap<GroupType>
    {
        
        public GroupTypeMap() : 
                base("Тип группы ООИ", "OVRHL_DICT_CEO_GROUP_TYPE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Code, "Код").Column("GROUP_TYPE_CODE").Length(200);
            Property(x => x.Name, "Наименование").Column("NAME").Length(300);
        }
    }
}
