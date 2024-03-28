namespace Bars.Gkh.RegOperator.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;

    using Entities;
    using Enums;

    public class CalcAccountOverdraftViewModel : BaseViewModel<CalcAccountOverdraft>
    {
        public override IDataResult List(IDomainService<CalcAccountOverdraft> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

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
                .Select(x => new
                {
                    x.Id,
                    x.DateStart,
                    Account = x.Account.AccountNumber,
                    AccountOwner = x.Account.AccountOwner.Name,
                    x.PercentRate,
                    x.OverdraftLimit,
                    x.OverdraftPeriod
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.DateStart,
                    Account = !x.Account.IsEmpty() ? x.Account : "Без номера",
                    AccountOwner = dictOwners.Get(x.Id) ?? x.AccountOwner,
                    x.PercentRate,
                    x.OverdraftLimit,
                    x.OverdraftPeriod
                })
                .AsQueryable()
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public override IDataResult Get(IDomainService<CalcAccountOverdraft> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId("id");

            var accountRoDomain = Container.ResolveDomain<CalcAccountRealityObject>();

            Dictionary<long, string> dictOwners;

            using (Container.Using(accountRoDomain))
            {
                dictOwners = accountRoDomain.GetAll()
                .Where(x => x.Account.TypeAccount == TypeCalcAccount.Special)
                .Select(x => new
                {
                    x.Account.Id,
                    x.RealityObject.Address
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x.Address).First());
            }
           
            var rec = domainService.GetAll()
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.DateStart,
                    Account = x.Account.AccountNumber,
                    AccountOwner = x.Account.AccountOwner.Name,
                    x.PercentRate,
                    x.OverdraftLimit,
                    x.OverdraftPeriod
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.DateStart,
                    x.Account,
                    AccountOwner = dictOwners.Get(x.Id) ?? x.AccountOwner,
                    x.PercentRate,
                    x.OverdraftLimit,
                    x.OverdraftPeriod
                })
                .FirstOrDefault();

            return new BaseDataResult(rec);
        }
    }
}
