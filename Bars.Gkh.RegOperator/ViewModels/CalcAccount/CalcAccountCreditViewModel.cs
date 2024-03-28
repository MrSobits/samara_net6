namespace Bars.Gkh.RegOperator.ViewModels
{
    using System;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Entities;
    using Enums;

    public class CalcAccountCreditViewModel : BaseViewModel<CalcAccountCredit>
    {
        public override IDataResult List(IDomainService<CalcAccountCredit> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var onlyOpen = baseParams.Params.GetAs<bool>("onlyOpen");

            var accountroDomain = Container.ResolveDomain<CalcAccountRealityObject>();

            var dictOwners = accountroDomain.GetAll()
                .Where(x => x.Account.TypeAccount == TypeCalcAccount.Special)
                .Select(x => new
                {
                    x.Account.Id,
                    x.RealityObject.Address
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x.Address).First());

            var data = domainService.GetAll()
                .WhereIf(onlyOpen, x => x.DateEnd == null && (x.CreditDebt > 0 || x.PercentDebt > 0))
                .Select(x => new
                {
                    x.Id,
                    x.DateStart,
                    Account = x.Account.AccountNumber,
                    AccountOwner = x.Account.AccountOwner.Name,
                    x.CreditSum,
                    x.PercentSum,
                    x.PercentRate,
                    x.CreditPeriod,
                    x.CreditDebt,
                    x.PercentDebt,
                    x.DateEnd,
                    x.Document
                })
                .ToList()
                .Select(x => new
                {
                    x.Id,
                    x.DateStart,
                    Account = !x.Account.IsEmpty() ? x.Account : "Без номера",
                    AccountOwner = dictOwners.Get(x.Id) ?? x.AccountOwner,
                    x.CreditSum,
                    x.PercentSum,
                    x.PercentRate,
                    x.CreditPeriod,
                    x.CreditDebt,
                    x.PercentDebt,
                    x.DateEnd,
                    x.Document
                })
                .AsQueryable()
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}