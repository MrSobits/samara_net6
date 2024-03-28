namespace Bars.GkhGji.Regions.Tatarstan.Map.TatarstanProtocolGji
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanProtocolGji;

    public class TatarstanProtocolGjiMap : JoinedSubClassMap<TatarstanProtocolGji>
    {
        public TatarstanProtocolGjiMap() :
            base(typeof(TatarstanProtocolGji).FullName, "GJI_TATARSTAN_PROTOCOL_GJI")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.Municipality, "Municipality").Column("MUNICIPALITY_ID").Fetch();
            this.Reference(x => x.ZonalInspection, "ZonalInspection").Column("ZONAL_INSPECTION_ID").Fetch();
            this.Property(x => x.Executant, "Executant").Column("TYPE_EXECUTANT").NotNull();
            this.Reference(x => x.Pattern, "Шаблон ГИС ГМП").Column("GIS_GMP_PATTERN_ID").Fetch();
            this.Reference(x => x.Sanction, "Вид санкции").Column("SANCTION_ID").Fetch();
            this.Property(x => x.DateSupply, "DateSupply").Column("DATE_SUPPLY");
            this.Property(x => x.DateOffense, "DateOffense").Column("DATE_OFFENSE");
            this.Property(x => x.TimeOffense, "TimeOffense").Column("TIME_OFFENSE");
            this.Property(x => x.AnnulReason, "Причина аннулирования").Column("ANNUL_REASON");
            this.Property(x => x.UpdateReason, "Причина изменения").Column("UPDATE_REASON");
            this.Property(x => x.Paided, "Штраф оплачен").Column("PAIDED").NotNull().DefaultValue(30);
            this.Property(x => x.PenaltyAmount, "Сумма штрафа").Column("PENALTY_AMOUNT");
            this.Property(x => x.DateTransferSsp, "Дата передачи в ССП").Column("DATE_TRANSFER_SSP");
            this.Property(x => x.TerminationBasement, "Основание прекращения").Column("TERMINATION_BASEMENT");
            this.Property(x => x.TerminationDocumentNum, "Номер документа").Column("TERMINATION_DOC_NUM");
            this.Property(x => x.SurName, "SurName").Column("SUR_NAME");
            this.Property(x => x.Name, "Name").Column("NAME");
            this.Property(x => x.Patronymic, "Patronymic").Column("PATRONYMIC");
            this.Property(x => x.BirthDate, "BirthDate").Column("BIRTH_DATE").Length(255);
            this.Property(x => x.BirthPlace, "BirthPlace").Column("BIRTH_PLACE").Length(255);
            this.Property(x => x.Address, "Address").Column("FACT_ADDRESS").Length(255);
            this.Property(x => x.CitizenshipType, "CitizenshipType").Column("CITIZENSHIP_TYPE");
            this.Reference(x => x.IdentityDocumentType, nameof(TatarstanProtocolGji.IdentityDocumentType)).Column("IDENTITY_DOCUMENT_ID");
            this.Property(x => x.SerialAndNumberDocument, "SerialAndNumberDocument").Column("SERIAL_AND_NUMBER").Length(255);
            this.Property(x => x.IssueDate, "IssueDate").Column("ISSUE_DATE");
            this.Property(x => x.IssuingAuthority, "IssuingAuthority").Column("ISSUING_AUTHORITY").Length(255);
            this.Property(x => x.Company, "Company").Column("COMPANY").Length(255);
            this.Reference(x => x.Citizenship, "Citizenship").Column("CITIZENSHIP_ID").Fetch();
            this.Property(x => x.MaritalStatus, "MaritalStatus").Column("MARITAL_STATUS");
            this.Property(x => x.DependentCount, "DependentCount").Column("DEPENDENT_COUNT");
            this.Property(x => x.RegistrationAddress, "RegistrationAddress").Column("REGISTRATION_ADDRESS");
            this.Property(x => x.Salary, "Salary").Column("SALARY");
            this.Property(x => x.ResponsibilityPunishment, "ResponsibilityPunishment").Column("RESPONSIBILITY_PUNISHMENT");
           this.Property(x => x.ProtocolExplanation, "ProtocolExplanation").Column("PROTOCOL_EXPLANATION");
            this.Property(x => x.IsInTribunal, "IsInTribunal").Column("IS_IN_TRIBUNAL");
            this.Property(x => x.TribunalName, "TribunalName").Column("TRIBUNAL_NAME");
            this.Property(x => x.OffenseAddress, "OffenseAddress").Column("OFFENCE_ADDRESS");
            this.Property(x => x.AccusedExplanation, "AccusedExplanation").Column("ACCUSED_EXPLANATION");
            this.Property(x => x.RejectionSignature, "RejectionSignature").Column("REJECTION_SIGNATURE");
            this.Property(x => x.ResidencePetition, "ResidencePetition").Column("RESIDENCE_PETITION");
        }
    }
}