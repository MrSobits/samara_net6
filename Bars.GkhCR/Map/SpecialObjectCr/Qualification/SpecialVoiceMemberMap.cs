namespace Bars.GkhCr.Map
{
    using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Голос участника квалификационного отбора"</summary>
    public class SpecialVoiceMemberMap : BaseImportableEntityMap<SpecialVoiceMember>
    {
        public SpecialVoiceMemberMap() : 
                base("Голос участника квалификационного отбора", "CR_SPECIAL_VOICE_QUAL_MEMBER")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            this.Property(x => x.Reason, "Причина").Column("REASON").Length(300);
            this.Property(x => x.TypeAcceptQualification, "Тип принятия голоса квалификационного отбора").Column("TYPE_ACCEPT_QUAL").NotNull();

            this.Reference(x => x.Qualification, "Квалификационный отбор").Column("QUALIFICATION_ID").NotNull().Fetch();
            this.Reference(x => x.QualificationMember, "Участник квалификационного отбора").Column("QUAL_MEMBER_ID").Fetch();
        }
    }
}
