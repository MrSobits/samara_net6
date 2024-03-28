/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhDi.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Работы по ТО"
///     /// </summary>
///     public class WorkToMap : BaseImportableEntityMap<WorkTo>
///     {
///         public WorkToMap() : base("DI_DICT_WORK_TO")
///         {
///             Map(x => x.Name, "NAME").Length(300);
///             Map(x => x.IsNotActual, "IS_NOT_ACTUAL").Not.Nullable();
/// 
///             References(x => x.GroupWorkTo, "GROUP_WORK_TO_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.WorkTo"</summary>
    public class WorkToMap : BaseImportableEntityMap<WorkTo>
    {
        
        public WorkToMap() : 
                base("Bars.GkhDi.Entities.WorkTo", "DI_DICT_WORK_TO")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Name").Column("NAME").Length(300);
            Property(x => x.IsNotActual, "IsNotActual").Column("IS_NOT_ACTUAL").NotNull();
            Reference(x => x.GroupWorkTo, "GroupWorkTo").Column("GROUP_WORK_TO_ID").Fetch();
        }
    }
}
