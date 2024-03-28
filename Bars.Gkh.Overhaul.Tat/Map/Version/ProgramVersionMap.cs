namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Overhaul.Tat.Enum;   
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.ProgramVersion"</summary>
    public class ProgramVersionMap : BaseEntityMap<ProgramVersion>
    {
        
        public ProgramVersionMap() : 
                base("Bars.Gkh.Overhaul.Tat.Entities.ProgramVersion", "OVRHL_PRG_VERSION")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ExportId, "Идентификатор для экспорта").Column("EXPORT_ID").NotNull();
            this.Property(x => x.Name, "Name").Column("NAME").Length(250).NotNull();
            this.Reference(x => x.Municipality, "Municipality").Column("MUNICIPALITY_ID").Fetch();
            this.Property(x => x.VersionDate, "VersionDate").Column("VERSION_DATE").NotNull();
            this.Property(x => x.IsMain, "IsMain").Column("IS_MAIN").DefaultValue(false).NotNull();
            this.Reference(x => x.State, "State").Column("STATE_ID").Fetch();
            this.Property(x => x.CopyingState, "CopyingState").Column("COPYING_STATE").DefaultValue(ProgramVersionCopyingState.NotCopied).NotNull();
            this.Property(x => x.IsProgramPublished, "IsProgramPublished").Column("IS_PROGRAM_PUBLISHED");
        }
    }

    public class ProgramVersionNhMap : BaseHaveExportIdMapping<ProgramVersion>
    {
    }
}
