/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.Map.LongTermProgram
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Nso.Entities;
/// 
///     public class SpecialAccountDecisionNoticeMap : BaseEntityMap<SpecialAccountDecisionNotice>
///     {
///         public SpecialAccountDecisionNoticeMap()
///             : base("OVRHL_DEC_SPEC_ACC_NOTICE")
///         {
///             Map(x => x.NoticeNumber, "NOTICE_NUMBER");
///             Map(x => x.NoticeNum, "NOTICE_NUM");
///             Map(x => x.NoticeDate, "NOTICE_DATE");
///             Map(x => x.RegDate, "REG_DATE");
///             Map(x => x.GjiNumber, "GJI_NUMBER");
///             Map(x => x.HasOriginal, "HAS_ORIG", true);
///             Map(x => x.HasCopyProtocol, "HAS_COPY_PROT", true);
///             Map(x => x.HasCopyCertificate, "HAS_COPY_CERT", true);
/// 
///             References(x => x.State, "STATE_ID", ReferenceMapConfig.Fetch);
///             References(x => x.SpecialAccountDecision, "SPEC_ACC_DEC_ID", ReferenceMapConfig.Fetch);
///             References(x => x.File, "FILE_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.SpecialAccountDecisionNotice"</summary>
    public class SpecialAccountDecisionNoticeMap : BaseEntityMap<SpecialAccountDecisionNotice>
    {
        
        public SpecialAccountDecisionNoticeMap() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.SpecialAccountDecisionNotice", "OVRHL_DEC_SPEC_ACC_NOTICE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.State, "State").Column("STATE_ID").Fetch();
            Reference(x => x.SpecialAccountDecision, "SpecialAccountDecision").Column("SPEC_ACC_DEC_ID").Fetch();
            Property(x => x.NoticeNum, "NoticeNum").Column("NOTICE_NUM");
            Property(x => x.NoticeNumber, "NoticeNumber").Column("NOTICE_NUMBER").Length(250);
            Property(x => x.NoticeDate, "NoticeDate").Column("NOTICE_DATE");
            Reference(x => x.File, "File").Column("FILE_ID").Fetch();
            Property(x => x.RegDate, "RegDate").Column("REG_DATE");
            Property(x => x.GjiNumber, "GjiNumber").Column("GJI_NUMBER").Length(250);
            Property(x => x.HasOriginal, "HasOriginal").Column("HAS_ORIG").NotNull();
            Property(x => x.HasCopyProtocol, "HasCopyProtocol").Column("HAS_COPY_PROT").NotNull();
            Property(x => x.HasCopyCertificate, "HasCopyCertificate").Column("HAS_COPY_CERT").NotNull();
        }
    }
}
