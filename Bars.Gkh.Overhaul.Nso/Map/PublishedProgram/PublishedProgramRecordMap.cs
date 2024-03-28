/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.ShortTermProgram
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Nso.Entities;
/// 
///     public class PublishedProgramRecordMap : BaseEntityMap<PublishedProgramRecord>
///     {
///         public PublishedProgramRecordMap()
///             : base("OVRHL_PUBLISH_PRG_REC")
///         {
///             Map(x => x.IndexNumber, "INDEX_NUMBER", true);
///             Map(x => x.Locality, "LOCALITY");
///             Map(x => x.Street, "STREET");
///             Map(x => x.House, "HOUSE");
///             Map(x => x.Housing, "HOUSING");
///             Map(x => x.Address, "ADDRESS");
///             Map(x => x.CommissioningYear, "YEAR_COMMISSIONING", true, 0);
///             Map(x => x.CommonEstateobject, "COMMON_ESTATE_OBJECT");
///             Map(x => x.Wear, "WEAR", true, 0);
///             Map(x => x.LastOverhaulYear, "YEAR_LAST_OVERHAUL", true, 0);
///             Map(x => x.PublishedYear, "YEAR_PUBLISHED", true, 0);
///             Map(x => x.Sum, "SUM", true, 0);
/// 
///             References(x => x.PublishedProgram, "PUBLISH_PRG_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.Stage2, "STAGE2_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.PublishedProgramRecord"</summary>
    public class PublishedProgramRecordMap : BaseEntityMap<PublishedProgramRecord>
    {
        
        public PublishedProgramRecordMap() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.PublishedProgramRecord", "OVRHL_PUBLISH_PRG_REC")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.PublishedProgram, "PublishedProgram").Column("PUBLISH_PRG_ID").NotNull().Fetch();
            Reference(x => x.Stage2, "Stage2").Column("STAGE2_ID").NotNull().Fetch();
            Property(x => x.Sum, "Sum").Column("SUM").NotNull();
            Property(x => x.IndexNumber, "IndexNumber").Column("INDEX_NUMBER").NotNull();
            Property(x => x.Locality, "Locality").Column("LOCALITY").Length(250);
            Property(x => x.Street, "Street").Column("STREET").Length(250);
            Property(x => x.House, "House").Column("HOUSE").Length(250);
            Property(x => x.Housing, "Housing").Column("HOUSING").Length(250);
            Property(x => x.Address, "Address").Column("ADDRESS").Length(250);
            Property(x => x.CommissioningYear, "CommissioningYear").Column("YEAR_COMMISSIONING").NotNull();
            Property(x => x.CommonEstateobject, "CommonEstateobject").Column("COMMON_ESTATE_OBJECT").Length(250);
            Property(x => x.Wear, "Wear").Column("WEAR").NotNull();
            Property(x => x.LastOverhaulYear, "LastOverhaulYear").Column("YEAR_LAST_OVERHAUL").NotNull();
            Property(x => x.PublishedYear, "PublishedYear").Column("YEAR_PUBLISHED").NotNull();
        }
    }
}
