/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Tat.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Tat.Entities;
/// 
///     public class SubsidyMunicipalityMap : BaseEntityMap<SubsidyMunicipality>
///     {
///         public SubsidyMunicipalityMap()
///             : base("OVRHL_SUBSIDY_MU")
///         {
///             References(x => x.Municipality, "MU_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.SubsidyMunicipality"</summary>
    public class SubsidyMunicipalityMap : BaseEntityMap<SubsidyMunicipality>
    {
        
        public SubsidyMunicipalityMap() : 
                base("Bars.Gkh.Overhaul.Tat.Entities.SubsidyMunicipality", "OVRHL_SUBSIDY_MU")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Municipality, "Municipality").Column("MU_ID").NotNull().Fetch();
        }
    }
}
