namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.TatarstanResolutionGji
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanResolutionGji;

    public class TatarstanResolutionGjiViewModel : BaseViewModel<TatarstanResolutionGji>
    {
        /// <inheritdoc />
        public override IDataResult Get(IDomainService<TatarstanResolutionGji> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            var entity = domainService.FirstOrDefault(x => x.Id == id);

            if (entity == null)
            {
                return new BaseDataResult { Success = false };
            }

            var bankDomainService = this.Container.ResolveDomain<ContragentBankCreditOrg>();

            ContragentBankCreditOrg account = null;
            using (this.Container.Using(bankDomainService))
            {
                if (entity.Contragent != null)
                {
                    account = bankDomainService
                        .GetAll().FirstOrDefault(x => x.Contragent.Id == entity.Contragent.Id);
                }
            }

            return new BaseDataResult(new
            {
                entity.Id,
                entity.State,
                entity.OffenderWas,
                entity.DeliveryDate,
                entity.TypeInitiativeOrg,
                entity.SectorNumber,
                entity.Municipality,
                entity.Official,
                entity.FineMunicipality,
                Executant = entity.TypeExecutant,
                entity.Sanction,
                entity.Paided,
                entity.PenaltyAmount,
                entity.DateTransferSsp,
                TerminationBasement = entity.TypeTerminationBasement,
                entity.TerminationDocumentNum,
                entity.SurName,
                entity.Name,
                entity.Patronymic,
                entity.BirthDate,
                entity.BirthPlace,
                entity.Address,
                entity.MaritalStatus,
                entity.DependentCount,
                entity.CitizenshipType,
                entity.Citizenship,
                entity.IdentityDocumentType,
                entity.SerialAndNumberDocument,
                entity.IssueDate,
                entity.IssuingAuthority,
                entity.Company,
                entity.RegistrationAddress,
                entity.Salary,
                entity.ResponsibilityPunishment,
                entity.Contragent,
                entity.DelegateFio,
                entity.DelegateCompany,
                entity.ProcurationDate,
                entity.ProcurationNumber,
                entity.DelegateResponsibilityPunishment,
                entity.ImprovingFact,
                entity.DisimprovingFact,
                entity.RulingNumber,
                entity.RulingDate,
                entity.RulinFio,
                //контрагент
                entity.Contragent?.Ogrn,
                entity.Contragent?.Inn,
                entity.Contragent?.Kpp,
                account?.SettlementAccount,
                BankName = account?.CreditOrg?.Name ?? account?.Name,
                CorrAccount = account?.CreditOrg?.CorrAccount ?? account?.CorrAccount,
                Bik = account?.CreditOrg?.Bik ?? account?.Bik,
                Okpo = account?.CreditOrg?.Okpo ?? account?.Okpo,
                account?.Okonh,
                entity.Contragent?.Okved,
                //док гжи
                entity.DocumentNumber,
                entity.DocumentDate,
                entity.TypeDocumentGji,
                DocumentYear = entity.DocumentDate?.Year,
                entity.DocumentNum,
                entity.LiteralNum,
                entity.DocumentSubNum
            });
        }
    }
}
