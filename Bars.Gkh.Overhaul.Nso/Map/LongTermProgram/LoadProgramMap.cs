/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.Map.LongTermProgram
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Nso.Entities;
/// 
///     public class LoadProgramMap : BaseEntityMap<LoadProgram>
///     {
///         public LoadProgramMap()
///             : base("OVRHL_LOADED_PROGRAM")
///         {
///             Map(x => x.IndexNumber, "INDEX_NUMBER", true);
///             Map(x => x.Locality, "LOCALITY", true);
///             Map(x => x.Street, "STREET", true);
///             Map(x => x.House, "HOUSE", true);
///             Map(x => x.Housing, "HOUSING");
///             Map(x => x.Address, "ADDRESS", true);
///             Map(x => x.CommissioningYear, "YEAR_COMMISSIONING", true);
///             Map(x => x.CommonEstateobject, "COMMON_ESTATE_OBJECT", true);
///             Map(x => x.Wear, "WEAR", true);
///             Map(x => x.LastOverhaulYear, "YEAR_LAST_OVERHAUL", true);
///             Map(x => x.PlanOverhaulYear, "YEAR_PLAN_OVERHAUL", true);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.LoadProgram"</summary>
    public class LoadProgramMap : BaseEntityMap<LoadProgram>
    {
        
        public LoadProgramMap() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.LoadProgram", "OVRHL_LOADED_PROGRAM")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.IndexNumber, "IndexNumber").Column("INDEX_NUMBER").NotNull();
            Property(x => x.Locality, "Locality").Column("LOCALITY").Length(250).NotNull();
            Property(x => x.Street, "Street").Column("STREET").Length(250).NotNull();
            Property(x => x.House, "House").Column("HOUSE").Length(250).NotNull();
            Property(x => x.Housing, "Housing").Column("HOUSING").Length(250);
            Property(x => x.Address, "Address").Column("ADDRESS").Length(250).NotNull();
            Property(x => x.CommissioningYear, "CommissioningYear").Column("YEAR_COMMISSIONING").NotNull();
            Property(x => x.CommonEstateobject, "CommonEstateobject").Column("COMMON_ESTATE_OBJECT").Length(250).NotNull();
            Property(x => x.Wear, "Wear").Column("WEAR").Length(250).NotNull();
            Property(x => x.LastOverhaulYear, "LastOverhaulYear").Column("YEAR_LAST_OVERHAUL").NotNull();
            Property(x => x.PlanOverhaulYear, "PlanOverhaulYear").Column("YEAR_PLAN_OVERHAUL").NotNull();
        }
    }
}
