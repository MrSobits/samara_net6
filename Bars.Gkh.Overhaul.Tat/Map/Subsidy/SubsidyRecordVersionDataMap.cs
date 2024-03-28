/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Tat.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class SubsidyRecordVersionDataMap : BaseEntityMap<SubsidyRecordVersionData>
///     {
///         public SubsidyRecordVersionDataMap()
///             : base("OVRHL_SM_RECORD_VERSION")
///         {
///             References(x => x.Version, "VERSION_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.SubsidyRecordUnversioned, "SUBSIDY_REC_UNV_ID", ReferenceMapConfig.NotNullAndFetch);
/// 
///             Map(x => x.NeedFinance, "NEED_FINANCE", true, (object)0);
///             Map(x => x.Deficit, "DEFICIT", true, (object)0);
///             Map(x => x.CorrDeficit, "CORR_DEFICIT", true, (object) 0);
///             Map(x => x.CorrNeedFinance, "CORR_NEED_FINANCE", true, (object)0);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using System;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.SubsidyRecordVersionData"</summary>
    public class SubsidyRecordVersionDataMap : BaseEntityMap<SubsidyRecordVersionData>
    {
        
        public SubsidyRecordVersionDataMap() : 
                base("Bars.Gkh.Overhaul.Tat.Entities.SubsidyRecordVersionData", "OVRHL_SM_RECORD_VERSION")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Version, "Version").Column("VERSION_ID").NotNull().Fetch();
            Reference(x => x.SubsidyRecordUnversioned, "SubsidyRecordUnversioned").Column("SUBSIDY_REC_UNV_ID").NotNull().Fetch();
            Property(x => x.NeedFinance, "NeedFinance").Column("NEED_FINANCE").DefaultValue(0m).NotNull();
            Property(x => x.Deficit, "Deficit").Column("DEFICIT").DefaultValue(0m).NotNull();
            Property(x => x.CorrNeedFinance, "CorrNeedFinance").Column("CORR_NEED_FINANCE").DefaultValue(0m).NotNull();
            Property(x => x.CorrDeficit, "CorrDeficit").Column("CORR_DEFICIT").DefaultValue(0m).NotNull();
        }
    }
}
