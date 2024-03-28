/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.Overhaul.Nso.Entities;
///     using Bars.Gkh.Overhaul.Nso.Enum;
/// 
///     public class VersionActualizeLogMap : BaseEntityMap<VersionActualizeLog>
///     {
///         public VersionActualizeLogMap()
///             : base("OVRHL_ACTUALIZE_LOG")
///         {
///             Map(x => x.CountActions, "COUNT_ACTIONS");
///             Map(x => x.UserName, "USER_NAME");
///             Map(x => x.DateAction, "DATE_ACTION").Not.Nullable();
///             Map(x => x.ActualizeType, "TYPE_ACTUALIZE").Not.Nullable().CustomType<VersionActualizeType>();
/// 
///             References(x => x.ProgramVersion, "VERSION_ID").Not.Nullable().Fetch.Join();
///             References(x => x.LogFile, "FILE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.VersionActualizeLog"</summary>
    public class VersionActualizeLogMap : BaseEntityMap<VersionActualizeLog>
    {
        
        public VersionActualizeLogMap() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.VersionActualizeLog", "OVRHL_ACTUALIZE_LOG")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.CountActions, "CountActions").Column("COUNT_ACTIONS");
            Property(x => x.UserName, "UserName").Column("USER_NAME");
            Property(x => x.DateAction, "DateAction").Column("DATE_ACTION").NotNull();
            Property(x => x.ActualizeType, "ActualizeType").Column("TYPE_ACTUALIZE").NotNull();
            Reference(x => x.ProgramVersion, "ProgramVersion").Column("VERSION_ID").NotNull().Fetch();
            Reference(x => x.LogFile, "LogFile").Column("FILE_ID").Fetch();
        }
    }
}
