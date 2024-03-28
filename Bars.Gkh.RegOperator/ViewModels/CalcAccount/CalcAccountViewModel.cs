namespace Bars.Gkh.RegOperator.ViewModels
{
    using System;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Entities;
    using Enums;

    public class CalcAccountViewModel : BaseViewModel<CalcAccount>
    {
        public override IDataResult List(IDomainService<CalcAccount> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var calcaccroDomain = Container.ResolveDomain<CalcAccountRealityObject>();

            var dictOwners = calcaccroDomain.GetAll()
                .Select(x => new
                {
                    x.Account.Id,
                    Owner = x.Account.TypeAccount == TypeCalcAccount.Regoperator
                        ? x.Account.AccountOwner.Name
                        : x.RealityObject.Address
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x.Owner).First());

            var data = domainService.GetAll()
                .Where(x => x.DateOpen <= DateTime.Today && x.DateOpen != default(DateTime))
                .Where(x => !x.DateClose.HasValue || x.DateClose >= DateTime.Today)
                .Where(x => x.TypeAccount == TypeCalcAccount.Regoperator || ((SpecialCalcAccount) x).IsActive)
                .Select(x => new
                {
                    x.Id,
                    x.AccountNumber,
                    AccountOwner = x.AccountOwner.Name,
                    ContragentAddress = x.AccountOwner.FactAddress,
                    CreditOrg = x.CreditOrg.Name,
                    x.TypeAccount
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    AccountNumber = x.AccountNumber ?? string.Empty,
                    AccountOwner = dictOwners.Get(x.Id) ?? x.AccountOwner ?? string.Empty,
                    CreditOrg = x.CreditOrg ?? string.Empty,
                    x.TypeAccount,
                    Address = 
                        x.TypeAccount == TypeCalcAccount.Regoperator 
                            ? string.Empty
                            : dictOwners.Get(x.Id)
                })
                .AsQueryable()
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}