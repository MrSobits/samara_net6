namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.TatarstanProtocolGji
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.TatarstanProtocolGji;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanProtocolGji;

    public class TatarstanProtocolGjiContragentViewModel : BaseViewModel<TatarstanProtocolGjiContragent>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<TatarstanProtocolGjiContragent> domainService, BaseParams baseParams)
        {
            var service = this.Container.Resolve<ITatarstanProtocolGjiService>();
            var domain = this.Container.ResolveDomain<TatarstanProtocolGji>();
            using (this.Container.Using(service, domain))
            {
                return service.GetListResult(domain, baseParams).ToListDataResult(baseParams.GetLoadParam(), this.Container);
            }
        }

        /// <inheritdoc />
        public override IDataResult Get(IDomainService<TatarstanProtocolGjiContragent> domainService, BaseParams baseParams)
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

            entity.InspectionId = entity.Inspection?.Id ?? default(long);
            return new BaseDataResult(new
            {
                entity.Id,
                entity.State,
                entity.Municipality,
                entity.ZonalInspection,
                entity.Executant,
                entity.DateSupply,
                entity.DateOffense,
                TimeOffense = entity.TimeOffense?.ToString("HH:mm"),
                entity.InspectionId,
                entity.Pattern,
                entity.GisUin,
                entity.AnnulReason,
                entity.UpdateReason,
                entity.Sanction,
                entity.Paided,
                entity.PenaltyAmount,
                entity.DateTransferSsp,
                entity.TerminationBasement,
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
                entity.ProtocolExplanation,
                entity.IsInTribunal,
                entity.TribunalName,
                entity.OffenseAddress,
                entity.AccusedExplanation,
                entity.RejectionSignature,
                entity.ResidencePetition,
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
                DocumentYear = entity.DocumentDate?.Year
            });
        }
    }
}
