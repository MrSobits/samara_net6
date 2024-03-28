/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class SubsidyRecordVersionDataMap : BaseEntityMap<SubsidyRecordVersionData>
///     {
///         public SubsidyRecordVersionDataMap()
///             : base("OVRHL_SM_RECORD_VERSION")
///         {
///             Map(x => x.RecommendedTarif, "RECOMMEND_TARIF", true, 0);
///             Map(x => x.RecommendedTarifCollection, "RECOM_TARIF_COLL", true, 0);
///             Map(x => x.FinanceNeedBefore, "FINANCE_NEED_BEFORE", true, 0);
///             Map(x => x.DeficitBefore, "DEFICIT_BEFORE", true, 0);
///             Map(x => x.DeficitAfter, "DEFICIT_AFTER", true, 0);
///             Map(x => x.FinanceNeedAfter, "FINANCE_NEED_AFTER", true, 0);
/// 
///             References(x => x.SubsidyRecordUnversioned, "SM_RECORD_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.Version, "VERSION_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.SubsidyRecordVersionData"</summary>
    public class SubsidyRecordVersionDataMap : BaseEntityMap<SubsidyRecordVersionData>
    {
        
        public SubsidyRecordVersionDataMap() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.SubsidyRecordVersionData", "OVRHL_SM_RECORD_VERSION")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Version, "Version").Column("VERSION_ID").NotNull().Fetch();
            Reference(x => x.SubsidyRecordUnversioned, "SubsidyRecordUnversioned").Column("SM_RECORD_ID").NotNull().Fetch();
            Property(x => x.FinanceNeedBefore, "FinanceNeedBefore").Column("FINANCE_NEED_BEFORE").NotNull();
            Property(x => x.FinanceNeedAfter, "FinanceNeedAfter").Column("FINANCE_NEED_AFTER").NotNull();
            Property(x => x.DeficitBefore, "DeficitBefore").Column("DEFICIT_BEFORE").NotNull();
            Property(x => x.DeficitAfter, "DeficitAfter").Column("DEFICIT_AFTER").NotNull();
            Property(x => x.RecommendedTarif, "RecommendedTarif").Column("RECOMMEND_TARIF").NotNull();
            Property(x => x.RecommendedTarifCollection, "RecommendedTarifCollection").Column("RECOM_TARIF_COLL").NotNull();
        }
    }
}
