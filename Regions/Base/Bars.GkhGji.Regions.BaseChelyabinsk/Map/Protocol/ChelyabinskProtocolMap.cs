

namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map.Protocol
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Chelyabinsk.Entities.ChelyabinskProtocol"</summary>
    public class ChelyabinskProtocolMap : JoinedSubClassMap<ChelyabinskProtocol>
    {
        
        public ChelyabinskProtocolMap() : 
                base("Bars.GkhGji.Regions.Chelyabinsk.Entities.ChelyabinskProtocol", "GJI_NSO_PROTOCOL")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.PersonRegistrationAddress, "PersonRegistrationAddress").Column("PERSON_REG_ADDRESS").Length(250);
            this.Property(x => x.PersonFactAddress, "PersonFactAddress").Column("PERSON_FACT_ADDRESS").Length(250);
            this.Property(x => x.PersonJob, "PersonJob").Column("PERSON_JOB");
            this.Property(x => x.PersonPosition, "PersonPosition").Column("PERSON_POSITION");
            this.Property(x => x.PersonBirthDatePlace, "PersonBirthDatePlace").Column("PERSON_BIRTHDATE").Length(250);
            this.Property(x => x.PersonDoc, "PersonDoc").Column("PERSON_DOC");
            this.Property(x => x.PersonSalary, "PersonSalary").Column("PERSON_SALARY");
            this.Property(x => x.PersonRelationship, "PersonRelationship").Column("PERSON_RELAT");
            this.Property(x => x.TypePresence, "TypePresence").Column("TYPE_PRESENCE");
            this.Property(x => x.Representative, "Representative").Column("REPRESENTATIVE").Length(500);
            this.Property(x => x.ReasonTypeRequisites, "ReasonTypeRequisites").Column("REASON_TYPE_REQ").Length(1000);
            this.Property(x => x.DateOfViolation, "DateOfViolation").Column("DATE_OF_VIOLATION");
            this.Property(x => x.HourOfViolation, "HourOfViolation").Column("HOUR_OF_VIOLATION");
            this.Property(x => x.MinuteOfViolation, "MinuteOfViolation").Column("MINUTE_OF_VIOLATION");
            this.Reference(x => x.ResolveViolationClaim, "ResolveViolationClaim").Column("RESOLVE_VIOL_CLAIM_ID");
            this.Reference(x => x.NormativeDoc, "NormativeDoc").Column("NORMATIVE_DOC_ID");
        }
    }
}
