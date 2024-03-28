/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Nso.Entities;
///     using Bars.Gkh.Overhaul.Nso.Enum;
/// 
///     public class ProgramVersionMap : BaseEntityMap<ProgramVersion>
///     {
///         public ProgramVersionMap()
///             : base("OVRHL_PRG_VERSION")
///         {
///             Map(x => x.Name, "NAME", true);
///             Map(x => x.VersionDate, "VERSION_DATE", true);
///             Map(x => x.IsMain, "IS_MAIN", true, false);
///             Map(x => x.CopyingState, "COPYING_STATE", true, ProgramVersionCopyingState.NotCopied);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using Bars.Gkh.Overhaul.Nso.Enum;
    using System;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.ProgramVersion"</summary>
    public class ProgramVersionMap : BaseEntityMap<ProgramVersion>
    {
        
        public ProgramVersionMap() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.ProgramVersion", "OVRHL_PRG_VERSION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Name").Column("NAME").Length(250).NotNull();
            Property(x => x.VersionDate, "VersionDate").Column("VERSION_DATE").NotNull();
            Property(x => x.IsMain, "IsMain").Column("IS_MAIN").DefaultValue(false).NotNull();
            Property(x => x.CopyingState, "CopyingState").Column("COPYING_STATE").DefaultValue(ProgramVersionCopyingState.NotCopied).NotNull();
        }
    }
}
