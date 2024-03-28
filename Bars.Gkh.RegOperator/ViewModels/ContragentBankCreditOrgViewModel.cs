namespace Bars.Gkh.RegOperator.ViewModels
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Utils;

    public class ContragentBankCreditOrgViewModel : BaseViewModel<ContragentBankCreditOrg>
    {
        public override IDataResult List(IDomainService<ContragentBankCreditOrg> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var contragentId = baseParams.Params.GetAsId("contragentId");
            var manorgId = baseParams.Params.GetAsId("manorgId");
            var builderId = baseParams.Params.GetAsId("builderId");
            var fromBankStatement = baseParams.Params.GetAs<bool>("fromBankStatement");

            var regOperatorDomainService = this.Container.ResolveDomain<RegOperator>();
            var calcAccountDomainService = this.Container.ResolveDomain<CalcAccount>();
            var accountroDomain = this.Container.ResolveDomain<CalcAccountRealityObject>();
            var builderDomainService = this.Container.ResolveDomain<Builder>();

            try
            {
                var regOperatorId = regOperatorDomainService.GetAll()
                    .Select(x => x.Contragent.Id)
                    .FirstOrDefault();

                //фейспалм
                if (manorgId != 0)
                {
                    contragentId = manorgId;
                }

                if (builderId != 0)
                {
                    contragentId = builderDomainService.GetAll()
                        .Where(x => x.Id == builderId)
                        .Select(x => x.Contragent.Id)
                        .FirstOrDefault();
                }

                if (fromBankStatement && contragentId == regOperatorId)
                {
                    var accountIds = accountroDomain.GetAll()
                        .Where(x => x.Account.TypeAccount == TypeCalcAccount.Special)
                        .Select(x => x.Account.Id);

                    var data = calcAccountDomainService.GetAll()
                        .Where(x => x.AccountOwner.Id == contragentId)
                        .Where(x => x.TypeOwner == TypeOwnerCalcAccount.Regoperator)
                        .WhereNotEmptyString(x => x.AccountNumber)
                        .Where(
                            x => (x.TypeAccount == TypeCalcAccount.Special && accountIds.Any(z => z == x.Id))
                                || x.TypeAccount == TypeCalcAccount.Regoperator)
                        .Select(
                            x => new
                            {
                                x.Id,
                                Name = x.CreditOrg.Name ?? x.AccountOwner.Name,
                                x.CreditOrg.Bik,
                                x.CreditOrg.CorrAccount,
                                x.CreditOrg.Okpo,
                                SettlementAccount = x.AccountNumber
                            })
                        .Filter(loadParams, this.Container);

                    return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
                }
                else
                {
                    var data = domainService.GetAll()
                        .Where(x => x.Contragent.Id == contragentId)
                        .Select(x => new
                        {
                            x.Id,
                            Name = x.CreditOrg.Name ?? x.Name,
                            Bik = x.CreditOrg.Bik ?? x.Bik,
                            CorrAccount = x.CreditOrg.CorrAccount ?? x.CorrAccount,
                            Okpo = x.CreditOrg.Okpo ?? x.Okpo,
                            x.Okonh,
                            x.SettlementAccount
                        })
                        .Filter(loadParams, this.Container);

                    return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
                }
            }
            finally
            {
                this.Container.Release(regOperatorDomainService);
                this.Container.Release(calcAccountDomainService);
                this.Container.Release(accountroDomain);
                this.Container.Release(builderDomainService);
            }
        }

        public override IDataResult Get(IDomainService<ContragentBankCreditOrg> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");
            var obj = domainService.GetAll().FirstOrDefault(x => x.Id == id);

            return new BaseDataResult(new
            {
                obj.Id,
                CreditOrg = obj.CreditOrg != null ? obj.CreditOrg.Return(x => new { x.Id, x.Name, x.CorrAccount, x.Bik, x.Okpo }) 
                                                  : new { Id = 0L, Name ="", obj.CorrAccount, obj.Bik, obj.Okpo },
                Contragent = obj.Contragent.Id, 
                obj.Description,
                obj.File,
                obj.Name,
                obj.Okonh,
                obj.SettlementAccount,
                CorrAccount = obj.CreditOrg != null
                    ? !string.IsNullOrEmpty(obj.CreditOrg.CorrAccount) ? obj.CreditOrg.CorrAccount : obj.CorrAccount
                    : obj.CorrAccount,
                Bik = obj.CreditOrg != null ? obj.CreditOrg.Return(x => x.Bik) : obj.Bik,
                Okpo = obj.CreditOrg != null ? obj.CreditOrg.Return(x => x.Okpo) : obj.Okpo
            });
        }
    }
}