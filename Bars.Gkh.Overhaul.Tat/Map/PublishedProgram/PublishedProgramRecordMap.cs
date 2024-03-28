namespace Bars.Gkh.Overhaul.Tat.Map.PublishedProgram
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Tat.Entities;

    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.PublishedProgramRecord"</summary>
    public class PublishedProgramRecordMap : BaseEntityMap<PublishedProgramRecord>
    {
        
        public PublishedProgramRecordMap() : 
                base("Bars.Gkh.Overhaul.Tat.Entities.PublishedProgramRecord", "OVRHL_PUBLISH_PRG_REC")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.PublishedProgram, "PublishedProgram").Column("PUBLISH_PRG_ID").NotNull().Fetch();
            this.Reference(x => x.Stage2, "Stage2").Column("STAGE2_ID").NotNull().Fetch();
            this.Property(x => x.Sum, "Sum").Column("SUM").NotNull();
            this.Property(x => x.IndexNumber, "IndexNumber").Column("INDEX_NUMBER").NotNull();
            this.Property(x => x.Locality, "Locality").Column("LOCALITY").Length(250);
            this.Property(x => x.Street, "Street").Column("STREET").Length(250);
            this.Property(x => x.House, "House").Column("HOUSE").Length(250);
            this.Property(x => x.Housing, "Housing").Column("HOUSING").Length(250);
            this.Property(x => x.Address, "Address").Column("ADDRESS").Length(250);
            this.Property(x => x.CommissioningYear, "CommissioningYear").Column("YEAR_COMMISSIONING").NotNull();
            this.Property(x => x.CommonEstateobject, "CommonEstateobject").Column("COMMON_ESTATE_OBJECT").Length(250);
            this.Property(x => x.Wear, "Wear").Column("WEAR").NotNull();
            this.Property(x => x.LastOverhaulYear, "LastOverhaulYear").Column("YEAR_LAST_OVERHAUL").NotNull();
            this.Property(x => x.PublishedYear, "PublishedYear").Column("YEAR_PUBLISHED").NotNull();
        }
    }
}
