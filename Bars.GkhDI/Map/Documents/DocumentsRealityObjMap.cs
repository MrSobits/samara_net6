/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
///     using Bars.GkhDi.Entities;
/// 
///     public class DocumentsRealityObjMap : BaseGkhEntityMap<DocumentsRealityObj>
///     {
///         /// <summary>
///         /// Документы по 731 постановлению правительства РФ
///         /// </summary>
///         public DocumentsRealityObjMap(): base("DI_DISINFO_DOC_RO")
///         {
///             Map(x => x.DescriptionActState, "DESCR_ACT_STATE").Length(1000);
/// 
///             References(x => x.DisclosureInfoRealityObj, "DISINFO_RO_ID").Not.Nullable().Fetch.Join();
///             References(x => x.FileActState, "FILE_ACT_STATE_ID").Fetch.Join();
///             References(x => x.FileCatalogRepair, "FILE_CATREPAIR_ID").Fetch.Join();
///             References(x => x.FileReportPlanRepair, "FILE_PLAN_REPAIR_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.DocumentsRealityObj"</summary>
    public class DocumentsRealityObjMap : BaseImportableEntityMap<DocumentsRealityObj>
    {
        
        public DocumentsRealityObjMap() : 
                base("Bars.GkhDi.Entities.DocumentsRealityObj", "DI_DISINFO_DOC_RO")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.HasGeneralMeetingOfOwners, "HasGeneralMeetingOfOwners").DefaultValue(YesNoNotSet.NotSet).Column("HAS_GEN_MEET_OWNERS");
            Property(x => x.DescriptionActState, "DescriptionActState").Column("DESCR_ACT_STATE").Length(1000);
            Reference(x => x.DisclosureInfoRealityObj, "DisclosureInfoRealityObj").Column("DISINFO_RO_ID").NotNull().Fetch();
            Reference(x => x.FileActState, "FileActState").Column("FILE_ACT_STATE_ID").Fetch();
            Reference(x => x.FileCatalogRepair, "FileCatalogRepair").Column("FILE_CATREPAIR_ID").Fetch();
            Reference(x => x.FileReportPlanRepair, "FileReportPlanRepair").Column("FILE_PLAN_REPAIR_ID").Fetch();
        }
    }
}
