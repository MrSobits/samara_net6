namespace Bars.GkhGji.Regions.Nso.Map
{
	using Bars.GkhGji.Regions.Nso.Entities;
	using Bars.B4.Modules.Mapping.Mappers;

	/// <summary>
    /// Маппинг для сущности "Протокол"
    /// </summary>
	public class Protocol197Map : JoinedSubClassMap<Protocol197>
    {
		public Protocol197Map() :
			base("Протокол по ст.19.7 КоАП РФ", "GJI_PROTOCOL197")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.PhysicalPerson, "Физическое лицо").Column("PHYSICAL_PERSON").Length(300);
            Property(x => x.PhysicalPersonInfo, "Реквизиты физ. лица").Column("PHYSICAL_PERSON_INFO").Length(500);
            Property(x => x.DateToCourt, "Дата передачи в суд").Column("DATE_TO_COURT");
            Property(x => x.ToCourt, "Документ передан в суд").Column("TO_COURT");
            Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(2000);
            Property(x => x.DateOfProceedings, "Дата рассмотрения дела").Column("DATE_OF_PROCEEDINGS");
            Property(x => x.HourOfProceedings, "Время рассмотрения дела(час)").Column("HOUR_OF_PROCEEDINGS");
            Property(x => x.MinuteOfProceedings, "Время рассмотрения дела(мин)").Column("MINUTE_OF_PROCEEDINGS");
            Property(x => x.PersonFollowConversion, "Лицо, выполнившее перепланировку/переустройство").Column("PERSON_FOLLOW_CONVERSION");
			Property(x => x.FormatDate, "FormatDate").Column("FORMAT_DATE");
			Property(x => x.FormatPlace, "FormatPlace").Column("FORMAT_PLACE").Length(500);
			Property(x => x.FormatHour, "FormatHour").Column("FORMAT_HOUR");
			Property(x => x.FormatMinute, "FormatMinute").Column("FORMAT_MINUTE");
			Property(x => x.NotifNumber, "NotifNumber").Column("NOTIF_NUM").Length(100);
			Property(x => x.ProceedingsPlace, "ProceedingsPlace").Column("PROCEEDINGS_PLACE").Length(100);
			Property(x => x.Remarks, "Remarks").Column("REMARKS").Length(100);
			Property(x => x.PersonRegistrationAddress, "PersonRegistrationAddress").Column("PERSON_REG_ADDRESS").Length(250);
			Property(x => x.PersonFactAddress, "PersonFactAddress").Column("PERSON_FACT_ADDRESS").Length(250);
			Property(x => x.PersonJob, "PersonJob").Column("PERSON_JOB");
			Property(x => x.PersonPosition, "PersonPosition").Column("PERSON_POSITION");
			Property(x => x.PersonBirthDatePlace, "PersonBirthDatePlace").Column("PERSON_BIRTHDATE").Length(250);
			Property(x => x.PersonDoc, "PersonDoc").Column("PERSON_DOC");
			Property(x => x.PersonSalary, "PersonSalary").Column("PERSON_SALARY");
			Property(x => x.PersonRelationship, "PersonRelationship").Column("PERSON_RELAT");
			Property(x => x.TypePresence, "TypePresence").Column("TYPE_PRESENCE");
			Property(x => x.Representative, "Representative").Column("REPRESENTATIVE").Length(500);
			Property(x => x.ReasonTypeRequisites, "ReasonTypeRequisites").Column("REASON_TYPE_REQ").Length(1000);
			Property(x => x.NotifDeliveredThroughOffice, "NotifDeliveredThroughOffice").Column("DELIV_THROUGH_OFFICE");
			Property(x => x.ProceedingCopyNum, "ProceedingCopyNum").Column("PROCEEDING_COPY_NUM");
			Property(x => x.DateOfViolation, "DateOfViolation").Column("DATE_OF_VIOLATION");
			Property(x => x.HourOfViolation, "HourOfViolation").Column("HOUR_OF_VIOLATION");
			Property(x => x.MinuteOfViolation, "MinuteOfViolation").Column("MINUTE_OF_VIOLATION");
			
			Reference(x => x.ResolveViolationClaim, "ResolveViolationClaim").Column("RESOLVE_VIOL_CLAIM_ID");
			Reference(x => x.Executant, "Тип исполнителя документа").Column("EXECUTANT_ID").Fetch();
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID");
			Reference(x => x.NormativeDoc, "NormativeDoc").Column("NORMATIVE_DOC_ID");
        }
    }
}