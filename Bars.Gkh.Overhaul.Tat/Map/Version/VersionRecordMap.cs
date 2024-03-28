namespace Bars.Gkh.Overhaul.Tat.Map.Version
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;

    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.VersionRecord"</summary>
    public class VersionRecordMap : BaseEntityMap<VersionRecord>
    {
        
        public VersionRecordMap() : 
                base("Bars.Gkh.Overhaul.Tat.Entities.VersionRecord", "OVRHL_VERSION_REC")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Year, "Year").Column("YEAR").NotNull();
            this.Property(x => x.FixedYear, "FixedYear").Column("FIXED_YEAR").NotNull();
            this.Property(x => x.CorrectYear, "CorrectYear").Column("CORRECT_YEAR");
            this.Property(x => x.Sum, "Sum").Column("SUM").NotNull();
            this.Property(x => x.CommonEstateObjects, "CommonEstateObjects").Column("CEO_STRING");
            this.Property(x => x.Point, "Point").Column("POINT").NotNull();
            this.Property(x => x.IndexNumber, "IndexNumber").Column("INDEX_NUM").NotNull();
            this.Property(x => x.TypeDpkrRecord, "TypeDpkrRecord").Column("TYPE_DPKR_RECORD").NotNull();
            this.Reference(x => x.ProgramVersion, "ProgramVersion").Column("VERSION_ID").NotNull().Fetch();
            this.Reference(x => x.RealityObject, "RealityObject").Column("RO_ID").NotNull().Fetch();
        }
    }
}
