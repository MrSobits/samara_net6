namespace Bars.GisIntegration.Base.Map.Inspection
{
    using Bars.GisIntegration.Base.Entities.Inspection;
    using Bars.GisIntegration.Base.Map;

    /// <summary>
    /// Маппинг для "Bars.GisIntegration.Inspection.Entities.Examination"
    /// </summary>
    public class ExaminationMap : BaseRisEntityMap<Examination>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public ExaminationMap() :
            base("Bars.GisIntegration.Inspection.Entities.Examination", "GI_INSPECTION_EXAMINATION")
        {
        }
        
        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.InspectionPlan, "InspectionPlan").Column("PLAN_ID").Fetch();
            this.Property(x => x.InspectionNumber, "InspectionNumber").Column("INSPECTIONNUMBER");
            this.Property(x => x.ExaminationFormCode, "ExaminationFormCode").Column("EXAMFORM_CODE");
            this.Property(x => x.ExaminationFormGuid, "ExaminationFormGuid").Column("EXAMFORM_GUID");
            this.Property(x => x.OrderNumber, "OrderNumber").Column("ORDER_NUMBER");
            this.Property(x => x.OrderDate, "OrderDate").Column("ORDER_DATE");
            this.Property(x => x.IsScheduled, "IsScheduled").Column("IS_SCHEDULED");
            this.Property(x => x.SubjectType, "SubjectType").Column("SUBJECT_TYPE");
            this.Reference(x => x.GisContragent, "GisContragent").Column("GIS_CONTRAGENT");
            this.Property(x => x.FirstName, "FirstName").Column("FIRSTNAME");
            this.Property(x => x.LastName, "LastName").Column("LASTNAME");
            this.Property(x => x.MiddleName, "MiddleName").Column("MIDDLENAME");
            this.Property(x => x.OversightActivitiesCode, "OversightActivitiesCode").Column("OVERSIGHT_ACT_CODE");
            this.Property(x => x.OversightActivitiesGuid, "OversightActivitiesGuid").Column("OVERSIGHT_ACT_GUID");
            this.Property(x => x.BaseCode, "BaseCode").Column("BASE_CODE");
            this.Property(x => x.BaseGuid, "BaseGuid").Column("BASE_GUID");
            this.Property(x => x.Objective, "Objective").Column("OBJECTIVE").Length(2000);
            this.Property(x => x.From, "From").Column("DATE_FROM");
            this.Property(x => x.To, "To").Column("DATE_TO");
            this.Property(x => x.Duration, "Duration").Column("DURATION");
            this.Property(x => x.Tasks, "Tasks").Column("TASKS").Length(2000);
            this.Property(x => x.EventDescription, "EventDescription").Column("EVENT_DESC").Length(2000);
            this.Property(x => x.HasResult, "HasResult").Column("HAS_RESULT");
            this.Property(x => x.ResultDocumentTypeCode, "ResultDocumentTypeCode").Column("RESULT_DOC_TYPE_CODE");
            this.Property(x => x.ResultDocumentTypeGuid, "ResultDocumentTypeGuid").Column("RESULT_DOC_TYPE_GUID");
            this.Property(x => x.ResultDocumentNumber, "ResultDocumentNumber").Column("RESULT_DOC_NUMBER");
            this.Property(x => x.ResultDocumentDateTime, "ResultDocumentDateTime").Column("RESULT_DOC_DATETIME");
            this.Property(x => x.HasOffence, "HasOffence").Column("HAS_OFFENCE");
            this.Property(x => x.UriRegistrationNumber, "UriRegistrationNumber").Column("URI_REGISTRATION_NUMBER");
            this.Property(x => x.UriRegistrationDate, "UriRegistrationDate").Column("URI_REGISTRATION_DATE");
            this.Property(x => x.ProsecutorAgreementInformation, "ProsecutorAgreementInformation").Column("PROSECUTOR_AGREEMENT_INFORMATION");
            this.Property(x => x.ShouldNotBeRegistered, "ShouldNotBeRegistered").Column("SHOULD_NOT_BE_REGISTERED");
            this.Property(x => x.FunctionRegistryNumber, "FunctionRegistryNumber").Column("FUNCTION_REGISTRY_NUMBER");
            this.Property(x => x.AuthorizedPersons, "AuthorizedPersons").Column("AUTHORIZED_PERSONS");
            this.Property(x => x.InvolvedExperts, "InvolvedExperts").Column("INVOLVED_EXPERTS");
            this.Property(x => x.PreceptGuid, "PreceptGuid").Column("PRECEPT_GUID");
            this.Property(x => x.ObjectCode, "ObjectCode").Column("OBJECT_CODE");
            this.Property(x => x.ObjectGuid, "ObjectGuid").Column("OBJECT_GUID");
            this.Property(x => x.IdentifiedOffences, "IdentifiedOffences").Column("IDENTIFIED_OFFENCES");
            this.Property(x => x.ResultFrom, "ResultFrom").Column("RESULT_FROM");
            this.Property(x => x.ResultTo, "ResultTo").Column("RESULT_TO");
            this.Property(x => x.ResultPlace, "ResultPlace").Column("RESULT_PLACE");
            this.Property(x => x.FamiliarizationDate, "FamiliarizationDate").Column("FAMILIARIZATION_DATE");
            this.Property(x => x.IsSigned, "IsSigned").Column("IS_SIGNED");
            this.Property(x => x.FamiliarizedPerson, "FamiliarizedPerson").Column("FAMILIARIZED_PERSON");
        }
    }
}