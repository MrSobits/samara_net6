namespace Bars.Gkh.RegOperator.Regions.Tatarstan.ViewModels
{
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;

    using Bars.Gkh.RegOperator.DomainService;
    using Bars.Gkh.RegOperator.Entities;

    using DataResult;
    using Enums;
    using Gkh.Domain;

    public class SpecialCalcAccountViewModel : BaseViewModel<SpecialCalcAccount>
    {
        public override IDataResult List(IDomainService<SpecialCalcAccount> domainService, BaseParams baseParams)
        {
            var calcaccService = this.Container.Resolve<ICalcAccountService>();
            var accountroDomain = this.Container.ResolveDomain<CalcAccountRealityObject>();

            try
            {
                var loadParams = baseParams.GetLoadParam();

                var typeOwner = baseParams.Params.GetAs<TypeOwnerCalcAccount>("typeOwner");
                var ownerId = baseParams.Params.GetAsId("ownerId");

                var accroQuery = accountroDomain.GetAll()
                    .WhereIf(ownerId > 0, x => x.Account.AccountOwner.Id == ownerId)
                    .WhereIf(typeOwner != 0, x => x.Account.TypeOwner == typeOwner)
                    .Where(x => x.Account.TypeAccount == TypeCalcAccount.Special);

                var dictOwners = accroQuery
                    .Select(x => new
                    {
                        x.Account.Id,
                        x.RealityObject.Address,
                        Municipality = x.RealityObject.Municipality.Name,
                        RoId = x.RealityObject.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => new { x.Address, x.Municipality, x.RoId }).First());

                var dictMoney = calcaccService.GetAccountSummary(accroQuery);

                var data = domainService.GetAll()
                    .Where(x => x.IsActive)
                    .WhereIf(ownerId > 0, x => x.AccountOwner.Id == ownerId)
                    .WhereIf(typeOwner != 0, x => x.TypeOwner == typeOwner)
                    .Select(x => new
                    {
                        x.Id,
                        x.AccountNumber,
                        x.TotalIn,
                        x.TotalOut,
                        CreditOrg = x.CreditOrg.Name,
                        x.DateOpen,
                        x.DateClose
                    })
                    .AsEnumerable()
                    .Select(x =>
                    {
                        var money = dictMoney.Get(x.Id);
                        var owner = dictOwners.Get(x.Id);
                        return new
                        {
                            x.Id,
                            AccountNumber = x.AccountNumber ?? string.Empty,
                            x.TotalIn,
                            x.TotalOut,
                            ContragentCreditOrg = x.CreditOrg,
                            x.DateOpen,
                            x.DateClose,
                            money?.Debt,
                            money?.Credit,
                            money?.Saldo,
                            owner?.Address,
                            owner?.Municipality
                        };
                    })
                    .AsQueryable()
                    .Filter(loadParams, this.Container);

                return new ListSummaryResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
            finally
            {
                this.Container.Release(calcaccService);
                this.Container.Release(accountroDomain);
            }
        }
    }
}