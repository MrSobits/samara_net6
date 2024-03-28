/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhDi.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Группы работ по ТО"
///     /// </summary>
///     public class GroupWorkToMap : BaseImportableEntityMap<GroupWorkTo>
///     {
///         public GroupWorkToMap() : base("DI_DICT_GROUP_WORK_TO")
///         {
///             Map(x => x.Name, "NAME").Length(300);
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
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.GroupWorkTo"</summary>
    public class GroupWorkToMap : BaseImportableEntityMap<GroupWorkTo>
    {
        
        public GroupWorkToMap() : 
                base("Bars.GkhDi.Entities.GroupWorkTo", "DI_DICT_GROUP_WORK_TO")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Name").Column("NAME").Length(300);
            Property(x => x.Code, "Code").Column("CODE").Length(300);
        }
    }
}
