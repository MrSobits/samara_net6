namespace Bars.Gkh.RegOperator.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using B4.IoC;
    using Castle.Windsor;
    using Domain.Repository.RealityObjectAccount;
    using Entities;
    using Enums;
    using Gkh.Domain;
    using Gkh.Domain.CollectionExtensions;
    using Gkh.Entities;
    using Gkh.Utils;

    public class CalcAccountService : ICalcAccountService
    {
        public IWindsorContainer Container { get; set; }
        public IRealtyObjectMoneyRepository MoneyRepo { get; set; }
        public IDomainService<RealityObjectPaymentAccount> PaymentAccountRepo { get; set; }

        public IDomainService<CalcAccountRealityObject> CalcAccRoDomain { get; set; }

        public Dictionary<long, CalcAccountSummaryProxy> GetAccountSummary(IQueryable<CalcAccountRealityObject> query)
        {
            var realtyObjects = query.Select(x => x.RealityObject);

            var money = this.MoneyRepo.GetRealtyBalances(realtyObjects).ToList();
            var loanData = this.MoneyRepo.GetRealityObjectLoanSum(realtyObjects).ToList();

            var result = new Dictionary<long, CalcAccountSummaryProxy>();

            var accounts = query
                .Select(x => new
                {
                    AccountId = x.Account.Id,
                    RoId = x.RealityObject.Id
                })
                .ToList()
                .GroupBy(x => x.AccountId)
                .ToDictionary(x => x.Key, x => x.Select(r => r.RoId).ToHashSet());

            foreach (var account in accounts)
            {
                var account1 = account;
                result.Add(account.Key, new CalcAccountSummaryProxy
                {
                    AccountId = account.Key,
                    Credit = money.Where(x => account1.Value.Contains(x.RealtyObjectId)).Sum(x => x.Credit),
                    Debt = money.Where(x => account1.Value.Contains(x.RealtyObjectId)).Sum(x => x.Debt),
                    MoneyLocks = money.Where(x => account1.Value.Contains(x.RealtyObjectId)).Sum(x => x.Lock),
                    LoanSum = loanData.Where(x => account1.Value.Contains(x.RealityObjectId)).Sum(x => x.LoanSum),
                    LoanReturnedSum = loanData.Where(x => account1.Value.Contains(x.RealityObjectId)).Sum(x => x.LoanReturnedSum)
                });
            }

            return result;
        }

        public Dictionary<long, CalcAccountSummaryProxy> GetAccountSummary(CalcAccount account)
        {
            if(account == null) return new Dictionary<long, CalcAccountSummaryProxy>();

            return this.GetAccountSummary(this.CalcAccRoDomain.GetAll()
                .Where(x => x.Account.Id == account.Id));
        }

        public IDataResult GetRegopAccountSummary(BaseParams baseParams)
        {
            var ownerId = baseParams.Params.GetAsId("ownerId");

            var filterQuery = this.CalcAccRoDomain.GetAll()
                .Where(x => x.Account.AccountOwner.Id == ownerId)
                .Where(x => x.DateStart <= DateTime.Today)
                .Where(x => !x.DateEnd.HasValue || x.DateEnd > DateTime.Today)
                .Where(x => !((RegopCalcAccount) x.Account).IsTransit);

            var dictMoney = this.GetAccountSummary(filterQuery).ToList();

            var regopCalAccDomain = this.Container.ResolveDomain<RegopCalcAccount>();

            var list =
                regopCalAccDomain.GetAll()
                    .WhereIf(ownerId > 0, x => x.AccountOwner.Id == ownerId)
                    .Select(x => x.Id)
                    .ToList()
                    .Select(x => new
                    {
                        Credit = dictMoney.Get(x).Return(y => y.Credit),
                        Debt = dictMoney.Get(x).Return(y => y.Debt),
                        PercentSum = dictMoney.Get(x).Return(y => y.PercentSum),
                        Saldo = dictMoney.Get(x).Return(y => y.Saldo)
                    })
                    .ToList();

            var debet = list.SafeSum(x => x.Debt);
            var credit = list.SafeSum(x => x.Credit);
            var saldo = list.SafeSum(x => x.Saldo);

            return new BaseDataResult(new
            {
                Debet = debet,
                Credit = credit,
                Saldo = saldo,
                ExpenditureShare = debet == 0 ? 0 : (credit / debet) * 100
            });
        }

        public IDataResult ListOperationsSum(BaseParams baseParams)
        {
            var accountId = baseParams.Params.GetAsId("accId");
            var isCredit = baseParams.Params.GetAs<bool>("isCredit");

            var ropayaccopDomain = this.Container.ResolveDomain<RealityObjectPaymentAccountOperation>();
            var calcaccroDomain = this.Container.ResolveDomain<CalcAccountRealityObject>();

            using (this.Container.Using(ropayaccopDomain, calcaccroDomain))
            {
                var filterQuery = calcaccroDomain.GetAll()
                    .Where(x => x.Account.Id == accountId);

                var data = ropayaccopDomain.GetAll()
                    .Where(y => filterQuery.Any(x => x.RealityObject == y.Account.RealityObject))

                    .WhereIf(isCredit, c => c.OperationType == PaymentOperationType.OutcomeAccountPayment
                                            || c.OperationType == PaymentOperationType.ExpenseLoan
                                            || c.OperationType == PaymentOperationType.OutcomeLoan
                                            || c.OperationType == PaymentOperationType.OpeningAcc
                                            || c.OperationType == PaymentOperationType.CashService
                                            || c.OperationType == PaymentOperationType.CreditPayment
                                            || c.OperationType == PaymentOperationType.CreditPercentPayment
                                            || c.OperationType == PaymentOperationType.UndoPayment
                                            || c.OperationType == PaymentOperationType.CancelPayment)

                    .WhereIf(!isCredit, c => c.OperationType != PaymentOperationType.OutcomeAccountPayment
                                             && c.OperationType != PaymentOperationType.ExpenseLoan
                                             && c.OperationType != PaymentOperationType.OutcomeLoan
                                             && c.OperationType != PaymentOperationType.CashService
                                             && c.OperationType != PaymentOperationType.OpeningAcc
                                             && c.OperationType != PaymentOperationType.CreditPayment
                                             && c.OperationType != PaymentOperationType.CreditPercentPayment
                                             && c.OperationType != PaymentOperationType.UndoPayment
                                             && c.OperationType != PaymentOperationType.CancelPayment)
                    .Select(x => new
                    {
                        x.OperationSum,
                        x.OperationType,
                        x.Date,
                        x.OperationStatus
                    })
                    .ToList();

                if (!isCredit)
                {
                    var creditDomain = this.Container.ResolveDomain<CalcAccountCredit>();

                    data.AddRange(creditDomain.GetAll()
                        .Where(x => x.Account.Id == accountId)
                        .Select(x => new
                        {
                            OperationSum = x.CreditSum,
                            OperationType = PaymentOperationType.IncomeCredit,
                            Date = x.DateStart,
                            OperationStatus = OperationStatus.Approved
                        })
                        .AsEnumerable());
                }

                var resultData = data;

                return new BaseDataResult(resultData.Sum(x => x.OperationSum));
            }
        }

        public IDataResult ListOperations(BaseParams baseParams)
        {
            throw new NotImplementedException();
        }

        public T GetRobjectAccount<T>(RealityObject ro) where T : CalcAccount
        {
            var result = this.CalcAccRoDomain.GetAll()
                    .Where(x => x.DateStart <= DateTime.Today)
                    .Where(x => !x.DateEnd.HasValue || x.DateEnd > DateTime.Today)
                    .Where(x => x.RealityObject.Id == ro.Id)
                    .Where(x => x.Account.DateOpen <= DateTime.Today)
                    .Where(x => !x.Account.DateClose.HasValue
                                || x.Account.DateClose <= DateTime.MinValue
                                || x.Account.DateClose >= DateTime.Today)
                    /*костыль блджат*/
                    .WhereIf(typeof (T) == typeof (RegopCalcAccount), x => x.Account.TypeAccount != TypeCalcAccount.Special)
                    .WhereIf(typeof (T) == typeof (SpecialCalcAccount), x => x.Account.TypeAccount == TypeCalcAccount.Special && ((SpecialCalcAccount) x.Account).IsActive)
                    .Select(x => x.Account)
                    .OrderByDescending(x => x.DateOpen)
                    .FirstOrDefault();
                
                return result.As<T>();
        }

        public Dictionary<long, CalcAccount> GetRobjectsAccounts(IQueryable<RealityObject> query = null, DateTime date = new DateTime())
        {
            return this.CalcAccRoDomain.GetAll()
                .WhereIf(query != null, y => query.Any(x => x == y.RealityObject))
                .WhereIf(date != DateTime.MinValue, x => x.DateStart <= date && (!x.DateEnd.HasValue || x.DateEnd.Value >= date))
                .Where(x => x.Account.TypeAccount == TypeCalcAccount.Regoperator || ((SpecialCalcAccount) x.Account).IsActive)
                .Select(x => new
                {
                    RoId = x.RealityObject.Id,
                    x.Account,
                    x.DateStart
                })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, y => y.OrderByDescending(x => x.DateStart).Select(x => x.Account).First());
        }

        public Dictionary<long, CalcAccount> GetRobjectsAccounts(IQueryable<long> roIdsQuery = null, DateTime date = new DateTime())
        {
            var result =  this.CalcAccRoDomain.GetAll()
                .WhereIf(roIdsQuery != null, y => roIdsQuery.Any(x => x == y.RealityObject.Id))
                .Where(y => y.Account.TypeAccount == TypeCalcAccount.Special || y.Account.TypeAccount == TypeCalcAccount.Regoperator)
                .Where(y => y.Account.DateOpen <= DateTime.Today)
                .Where(y => !y.Account.DateClose.HasValue || y.Account.DateClose.Value >= DateTime.Today)
                .Where(y => (y.Account.TypeAccount == TypeCalcAccount.Special && (!y.DateEnd.HasValue || y.DateEnd >= DateTime.Today)) 
                                        || (y.Account.TypeAccount == TypeCalcAccount.Regoperator && y.DateStart <= DateTime.Today
                                            && (!y.DateEnd.HasValue || y.DateEnd >= DateTime.Today)))
                .Select(x => new
                {
                    RoId = x.RealityObject.Id,
                    x.Account
                })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, y => y.Select(x => x.Account).OrderByDescending(x => x.AccountNumber).First());

            return result;
        }

        public Dictionary<long, List<CalcAccount>> GetRobjectsAllAccounts(IQueryable<RealityObject> query, DateTime date)
        {
            return this.CalcAccRoDomain.GetAll()
                .Where(y => query.Any(x => x == y.RealityObject))
                .WhereIf(date.IsValid(), x => x.DateStart <= date)
                .Select(x => new
                {
                    RoId = x.RealityObject.Id,
                    x.Account,
                    x.DateStart
                })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, y => y.OrderByDescending(x => x.DateStart).Select(x => x.Account).ToList());
        }

        public Dictionary<long, CalcAccount> GetRobjectsAccounts(long[] roIds = null, DateTime date = new DateTime())
        {
            var result = this.CalcAccRoDomain.GetAll()
                .WhereIf(roIds != null, y => roIds.Contains(y.RealityObject.Id))
                .WhereIf(date != DateTime.MinValue, x => x.DateStart <= date && (!x.DateEnd.HasValue || x.DateEnd.Value >= date))
                .Where(x => x.Account.TypeAccount == TypeCalcAccount.Regoperator || ((SpecialCalcAccount)x.Account).IsActive)
                .Select(x => new
                {
                    RoId = x.RealityObject.Id,
                    x.Account
                })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, y => y.Select(x => x.Account).OrderByDescending(x => x.AccountNumber).First());

            return result;
        }
    }
}