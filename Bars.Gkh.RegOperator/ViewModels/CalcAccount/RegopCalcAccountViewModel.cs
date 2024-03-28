using System;
using Bars.Gkh.RegOperator.DomainModelServices;

namespace Bars.Gkh.RegOperator.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using B4.IoC;
    using DataResult;
    using DomainService;
    using Entities;
    using Gkh.Domain;

    public class RegopCalcAccountViewModel : BaseViewModel<RegopCalcAccount>
    {
        public override IDataResult List(IDomainService<RegopCalcAccount> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var ownerId = baseParams.Params.GetAsId("ownerId");

            var accountroDomain = Container.ResolveDomain<CalcAccountRealityObject>();
            var calcaccService = Container.Resolve<ICalcAccountService>();

            Dictionary<long, CalcAccountSummaryProxy> dictMoney;

            using (Container.Using(accountroDomain, calcaccService))
            {
                var accountroQuery = accountroDomain.GetAll()
                    .WhereIf(ownerId > 0, x => x.Account.AccountOwner.Id == ownerId)
                    .Where(x => x.DateStart <= DateTime.Today)
                    .Where(x => !x.DateEnd.HasValue || x.DateEnd > DateTime.Today);

                dictMoney = calcaccService.GetAccountSummary(accountroQuery);
            }

            var data = domainService.GetAll()
                .WhereIf(ownerId > 0, x => x.AccountOwner.Id == ownerId)
                .Select(x => new
                {
                    x.Id,
                    x.AccountNumber,
                    x.TotalIn,
                    x.TotalOut,
                    CreditOrg = x.ContragentCreditOrg.CreditOrg.Name,
                    ContragentAccountNumber = x.ContragentCreditOrg.SettlementAccount,
                    x.DateOpen,
                    x.DateClose,
                    x.Balance
                })
                .ToList()
                .Select(x => new
                {
                    x.Id,
                    x.AccountNumber,
                    x.TotalIn,
                    x.TotalOut,
                    ContragentCreditOrg = x.CreditOrg,
                    x.ContragentAccountNumber,
                    x.DateOpen,
                    x.DateClose,
                    Credit = x.TotalOut + dictMoney.Get(x.Id).Return(y => y.Credit),
                    Debt = x.TotalIn + dictMoney.Get(x.Id).Return(y => y.Debt),
                    PercentSum = dictMoney.Get(x.Id).Return(y => y.PercentSum),
                    Saldo = dictMoney.Get(x.Id).Return(y => y.Saldo)
                })
                .AsQueryable()
                .Filter(loadParams, Container);

            return new ListSummaryResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public override IDataResult Get(IDomainService<RegopCalcAccount> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId("id");

            var calcAccountOverdraftDomain = Container.Resolve<IDomainService<CalcAccountOverdraft>>();
            decimal overdraftLimit;

            using (Container.Using(calcAccountOverdraftDomain))
            {
                overdraftLimit =
                    calcAccountOverdraftDomain.GetAll()
                    .Where(x => x.Account.Id == id)
                    .OrderByDescending(x => x.DateStart)
                    .Select(x => x.OverdraftLimit)
                    .FirstOrDefault();
            }

            //var lastOperationDate = GetLastOperationDate(id);

            var rec = domainService.GetAll()
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.Balance,
                    x.BalanceIn,
                    x.BalanceOut,
                    x.DateClose,
                    x.DateOpen,
                    TotalIncome = x.TotalIn,
                    x.TotalOut,
                    ContragentCreditOrg =
                        x.ContragentCreditOrg != null
                            ? new
                            {
                                x.ContragentCreditOrg.Id,
                                Name = x.ContragentCreditOrg.CreditOrg.Name ?? x.ContragentCreditOrg.Name,
                                Bik = x.ContragentCreditOrg.CreditOrg.Bik ?? x.ContragentCreditOrg.Bik,
                                CorrAccount = x.ContragentCreditOrg.CreditOrg.CorrAccount ?? x.ContragentCreditOrg.CorrAccount,
                                Okpo = x.ContragentCreditOrg.CreditOrg.Okpo ?? x.ContragentCreditOrg.Okpo,
                                x.ContragentCreditOrg.SettlementAccount
                            }
                            : null,
                    OverdraftLimit = overdraftLimit,
                    x.IsTransit,
                    LastOperationDate = (DateTime?)null
                })
                .FirstOrDefault();

            return new BaseDataResult(rec);
        }

        private DateTime GetLastOperationDate(long accountId)
        {
            var calcMoneyService = Container.Resolve<ICalcAccountMoneyService>();
            using (Container.Using(calcMoneyService))
            {
                var parameters = new BaseParams();
                parameters.Params["accId"] = accountId;

                return calcMoneyService.GetLastOperationDate(parameters);
            }
        }
    }
}