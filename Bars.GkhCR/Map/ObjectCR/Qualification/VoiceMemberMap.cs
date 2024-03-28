/// <mapping-converter-backup>
/// namespace Bars.GkhCr.Map
/// {
///     using Bars.Gkh.Map;;
///     using Bars.GkhCr.Entities;
///     using Bars.GkhCr.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Голос участника квалификационного отбора"
///     /// </summary>
///     public class VoiceMemberMap : BaseGkhEntityMap<VoiceMember>
///     {
///         public VoiceMemberMap() : base("CR_VOICE_QUAL_MEMBER")
///         {
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.Reason, "REASON").Length(300);
///             Map(x => x.TypeAcceptQualification, "TYPE_ACCEPT_QUAL").Not.Nullable().CustomType<TypeAcceptQualification>();
/// 
///             References(x => x.Qualification, "QUALIFICATION_ID").Not.Nullable().Fetch.Join();
///             References(x => x.QualificationMember, "QUAL_MEMBER_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Голос участника квалификационного отбора"</summary>
    public class VoiceMemberMap : BaseImportableEntityMap<VoiceMember>
    {
        
        public VoiceMemberMap() : 
                base("Голос участника квалификационного отбора", "CR_VOICE_QUAL_MEMBER")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.Reason, "Причина").Column("REASON").Length(300);
            Property(x => x.TypeAcceptQualification, "Тип принятия голоса квалификационного отбора").Column("TYPE_ACCEPT_QUAL").NotNull();
            Reference(x => x.Qualification, "Квалификационный отбор").Column("QUALIFICATION_ID").NotNull().Fetch();
            Reference(x => x.QualificationMember, "Участник квалификационного отбора").Column("QUAL_MEMBER_ID").Fetch();
        }
    }
}
