namespace Bars.GisIntegration.Base.Map.HouseManagement
{
    using Bars.GisIntegration.Base.Entities.HouseManagement;
    using Bars.GisIntegration.Base.Map;

    /// <summary>
    /// Маппинг для "Bars.Gkh.Ris.Entities.HouseManagement.SupplyResourceContract"
    /// </summary>
    public class SupplyResourceContractMap : BaseRisEntityMap<SupplyResourceContract>
    {
        public SupplyResourceContractMap() :
            base("Bars.Gkh.Ris.Entities.HouseManagement.SupplyResourceContract", "SUPPLY_RESOURCE_CONTRACT")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.ComptetionDate, "ComptetionDate").Column("COMPTETION_DATE");
            this.Property(x => x.StartDate, "StartDate").Column("START_DATE");
            this.Property(x => x.StartDateNextMonth, "StartDateNextMonth").Column("START_DATE_NEXT_MONTH");
            this.Property(x => x.EndDate, "EndDate").Column("END_DATE");
            this.Property(x => x.EndDateNextMonth, "EndDateNextMonth").Column("END_DATE_NEXT_MONTH");
            this.Property(x => x.ContractBaseCode, "ContractBaseCode").Column("CONTRACT_BASE_CODE");
            this.Property(x => x.ContractBaseGuid, "ContractBaseGuid").Column("CONTRACT_BASE_GUID");
            this.Property(x => x.ContractType, "ContractType").Column("CONTRACT_TYPE");
            this.Property(x => x.PersonType, "PersonType").Column("PERSON_TYPE");
            this.Property(x => x.PersonTypeOrganization, "PersonTypeOrganization").Column("PERSON_TYPE_ORGANIZATION");
            this.Reference(x => x.JurPerson, "JurPerson").Column("CONTRAGENT_ID").Fetch();
            this.Property(x => x.IndSurname, "IndSurname").Column("IND_SURNAME");
            this.Property(x => x.IndFirstName, "IndFirstName").Column("IND_FIRSTNAME");
            this.Property(x => x.IndPatronymic, "IndPatronymic").Column("IND_PATRONYMIC");
            this.Property(x => x.IndSex, "IndSex").Column("IND_SEX");
            this.Property(x => x.IndDateOfBirth, "IndDateOfBirth").Column("IND_DATE_OF_BIRTH");
            this.Property(x => x.IndSnils, "IndSnils").Column("IND_SNILS");
            this.Property(x => x.IndIdentityTypeCode, "IndIdentityTypeCode").Column("IND_IDENTITY_TYPE_CODE");
            this.Property(x => x.IndIdentityTypeGuid, "IndIdentityTypeGuid").Column("IND_IDENTITY_TYPE_GUID");
            this.Property(x => x.IndIdentitySeries, "IndIdentitySeries").Column("IND_IDENTITY_SERIES");
            this.Property(x => x.IndIdentityNumber, "IndIdentityNumber").Column("IND_IDENTITY_NUMBER");
            this.Property(x => x.IndIdentityIssueDate, "IndIdentityIssueDate").Column("IND_IDENTITY_ISSUE_DATE");
            this.Property(x => x.CommercialMeteringResourceType, "CommercialMeteringResourceType").Column("COMM_METERING_RES_TYPE");
            this.Property(x => x.IndPlaceBirth, "IndPlaceBirth").Column("IND_PLACE_BIRTH");
            this.Property(x => x.FiasHouseGuid, "FiasHouseGuid").Column("FIAS_HOUSE_GUID");
            this.Property(x => x.ContractNumber, "ContractNumber").Column("CONTRACT_NUMBER");
            this.Property(x => x.SigningDate, "SigningDate").Column("SIGNING_DATE");
            this.Property(x => x.EffectiveDate, "EffectiveDate").Column("EFFECTIVE_DATE");

            this.Property(x => x.BillingDate, "BillingDate").Column("BILLING_DATE");
            this.Property(x => x.PaymentDate, "PaymentDate").Column("PAYMENT_DATE");
            this.Property(x => x.ProvidingInformationDate, "ProvidingInformationDate").Column("PROVIDING_INFORMATION_DATE");

            this.Property(x => x.TerminateReasonCode, "TerminateReasonCode").Column("TERMINATE_REASON_CODE");
            this.Property(x => x.TerminateReasonGuid, "TerminateReasonGuid").Column("TERMINATE_REASON_GUID");
            this.Property(x => x.TerminateDate, "TerminateDate").Column("TERMINATE_DATE");
            this.Property(x => x.RollOverDate, "RollOverDate").Column("ROLLOVER_DATE");
        }
    }
    
}
