/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
///     using Bars.GkhDi.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Группы работ по ППР"
///     /// </summary>
///     public class GroupWorkPprMap : BaseGkhEntityMap<GroupWorkPpr>
///     {
///         public GroupWorkPprMap() : base("DI_DICT_GROUP_WORK_PPR")
///         {
///             Map(x => x.Name, "NAME").Length(300);
///             Map(x => x.Code, "CODE").Length(300);
///             Map(x => x.IsNotActual, "IS_NOT_ACTUAL").Not.Nullable();
/// 
///             References(x => x.Service, "TEMPLATE_SERVICE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.GroupWorkPpr"</summary>
    public class GroupWorkPprMap : BaseImportableEntityMap<GroupWorkPpr>
    {
        
        public GroupWorkPprMap() : 
                base("Bars.GkhDi.Entities.GroupWorkPpr", "DI_DICT_GROUP_WORK_PPR")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Name").Column("NAME").Length(300);
            Property(x => x.Code, "Code").Column("CODE").Length(300);
            Property(x => x.IsNotActual, "IsNotActual").Column("IS_NOT_ACTUAL").NotNull();
            Reference(x => x.Service, "Service").Column("TEMPLATE_SERVICE_ID").Fetch();
        }
    }
}
