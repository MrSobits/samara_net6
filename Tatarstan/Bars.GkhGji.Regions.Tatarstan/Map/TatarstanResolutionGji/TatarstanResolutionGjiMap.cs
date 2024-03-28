namespace Bars.GkhGji.Regions.Tatarstan.Map.TatarstanResolutionGji
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanResolutionGji;

    public class TatarstanResolutionGjiMap : JoinedSubClassMap<TatarstanResolutionGji>
    {
        /// <inheritdoc />
        public TatarstanResolutionGjiMap()
            : base(typeof(TatarstanResolutionGji).FullName, "GJI_TATARSTAN_RESOLUTION_GJI")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.TerminationDocumentNum, "Номер документа").Column("TERMINATION_DOC_NUM");
            this.Property(x => x.TypeExecutant, "Executant").Column("TYPE_EXECUTANT").NotNull();
            this.Property(x => x.SurName, "SurName").Column("SUR_NAME");
            this.Property(x => x.Name, "Name").Column("NAME");
            this.Property(x => x.Patronymic, "Patronymic").Column("PATRONYMIC");
            this.Property(x => x.BirthDate, "BirthDate").Column("BIRTH_DATE");
            this.Property(x => x.BirthPlace, "BirthPlace").Column("BIRTH_PLACE");
            this.Property(x => x.Address, "Address").Column("FACT_ADDRESS");
            this.Property(x => x.CitizenshipType, "CitizenshipType").Column("CITIZENSHIP_TYPE");
            this.Reference(x => x.IdentityDocumentType, nameof(TatarstanResolutionGji.IdentityDocumentType)).Column("IDENTITY_DOCUMENT_ID");
            this.Property(x => x.SerialAndNumberDocument, "SerialAndNumberDocument").Column("SERIAL_AND_NUMBER");
            this.Property(x => x.IssueDate, "IssueDate").Column("ISSUE_DATE");
            this.Property(x => x.IssuingAuthority, "IssuingAuthority").Column("ISSUING_AUTHORITY");
            this.Property(x => x.Company, "Company").Column("COMPANY");
            this.Reference(x => x.Citizenship, "Citizenship").Column("CITIZENSHIP_ID").Fetch();
            this.Property(x => x.MaritalStatus, "MaritalStatus").Column("MARITAL_STATUS");
            this.Property(x => x.DependentCount, "DependentCount").Column("DEPENDENT_COUNT");
            this.Property(x => x.RegistrationAddress, "RegistrationAddress").Column("REGISTRATION_ADDRESS");
            this.Property(x => x.Salary, "Salary").Column("SALARY");
            this.Property(x => x.ResponsibilityPunishment, "ResponsibilityPunishment").Column("RESPONSIBILITY_PUNISHMENT");
            this.Property(x => x.DelegateFio, "DelegateFio").Column("DELEGATE_FIO");
            this.Property(x => x.DelegateCompany, "DelegateCompany").Column("DELEGATE_COMPANY");
            this.Property(x => x.ProcurationNumber, "ProcurationNumber").Column("PROCURATION_NUMBER");
            this.Property(x => x.ProcurationDate, "ProcurationDate").Column("PROCURATION_DATE");
            this.Property(x => x.DelegateResponsibilityPunishment, "DelegateResponsibilityPunishment").Column("DELEGATE_RESPONSIBILITY_PUNISHMENT");
            this.Property(x => x.ImprovingFact, "ImprovingFact").Column("IMPROVING_FACT");
            this.Property(x => x.DisimprovingFact, "DisimprovingFact").Column("DISIMPROVING_FACT");
        }
    }
}
