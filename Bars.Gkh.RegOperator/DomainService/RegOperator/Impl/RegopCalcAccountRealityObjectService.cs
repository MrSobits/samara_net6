namespace Bars.Gkh.RegOperator.DomainService.RegOperator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Castle.Windsor;
    using Decisions.Nso.Entities;
    using Entities;
    using Gkh.Domain;
    using Gkh.Domain.CollectionExtensions;
    using Gkh.Entities;
    using RealityObjectAccount;
    using Enums;
    using Gkh.Enums.Decisions;
    using Gkh.Utils;

    public class RegopCalcAccountRealityObjectService : IRegopCalcAccountRealityObjectService
    {
        private readonly IWindsorContainer _container;
        private readonly IDomainService<CalcAccountRealityObject> _domain;
        private readonly IDomainService<SpecialCalcAccount> _specAccDomain;
        private readonly IDomainService<CreditOrgDecision> _crDecDomain;
        private readonly IDomainService<RealityObjectPaymentAccountOperation> _roPayOpDomain;
        private readonly IDomainService<RegOpCalcAccount> _regopCalcDomain;
        private readonly IBankAccountDataProvider _bankProvider;

        public RegopCalcAccountRealityObjectService(
            IWindsorContainer container,
            IDomainService<CalcAccountRealityObject> domain,
            IDomainService<SpecialCalcAccount> specAccDomain,
            IDomainService<CreditOrgDecision> crDecDomain,
            IDomainService<RealityObjectPaymentAccountOperation> roPayOpDomain,
            IDomainService<RegOpCalcAccount> regopCalcDomain,
            IBankAccountDataProvider bankProvider)
        {
            _container = container;
            _domain = domain;
            _specAccDomain = specAccDomain;
            _crDecDomain = crDecDomain;
            _roPayOpDomain = roPayOpDomain;
            _regopCalcDomain = regopCalcDomain;
            _bankProvider = bankProvider;
        }

        public IDataResult MassCreate(BaseParams baseParams)
        {
            var roIds = baseParams.Params.GetAs<string>("roIds").ToLongArray();
            var accId = baseParams.Params.GetAsId("accId");
            var regopAccount = _regopCalcDomain.GetAll()
                .Where(x => x.Id == accId)
                .Select(x => new {x.DateOpen, x.DateClose})
                .FirstOrDefault();

            if (regopAccount == null)
            {
                return new BaseDataResult();
            }

            var existing =
                _domain.GetAll()
                    .Where(x => roIds.Contains(x.RealityObject.Id))
                    .Where(x => (x.Account.DateOpen <= regopAccount.DateOpen && (!x.Account.DateClose.HasValue || x.Account.DateClose >= regopAccount.DateOpen))
                    || (!regopAccount.DateClose.HasValue && x.Account.DateOpen >= regopAccount.DateOpen)
                    || (regopAccount.DateClose.HasValue && x.Account.DateOpen <= regopAccount.DateClose
                               && (!x.Account.DateClose.HasValue || x.Account.DateClose >= regopAccount.DateClose)))
                    .Select(x => x.RealityObject.Id)
                    .ToHashSet();

            roIds = roIds.Except(existing).ToArray();

            using (var tr = _container.Resolve<IDataTransaction>())
            {
                try
                {
                    roIds.ForEach(x => _domain.Save(new RegopCalcAccountRealityObject
                    {
                        RealityObject = new RealityObject {Id = x},
                        RegOpCalcAccount = new RegOpCalcAccount {Id = accId}
                    }));

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            }

            if (existing.Count > 0)
            {
                return new BaseDataResult(true, "Один из выбранных домов уже обслуживается другим расчетным счетом");
            }

            return new BaseDataResult();
        }

        public IDataResult ListAccounts(BaseParams baseParams)
        {
            var regopId = baseParams.Params.GetAsId("regopid");
            var loadParam = baseParams.GetLoadParam();

            var data = GetOperations(regopId)
                .AsQueryable()
                .Filter(loadParam, _container)
                .Order(loadParam);

            return new ListDataResult(data.Paging(loadParam), data.Count());
        }

        private IEnumerable<Proxy> GetOperations(long regopId)
        {
            var data = _domain.GetAll()
                .Where(x => x.Account.RegOperator.Id == regopId)
                //.Join(_domain.GetAll(), acc => acc, accRo => accRo.RegOpCalcAccount, (acc, accRo) => accRo)
                .Join(_roPayOpDomain.GetAll(), acc => acc.RealityObject, operation => operation.Account.RealityObject,
                    (acc, op) => new
                    {
                        ContragentBank = acc.RegOpCalcAccount.ContragentBankCrOrg.SettlementAccount,
                        CreditOrg = acc.RegOpCalcAccount.ContragentBankCrOrg.CreditOrg.Name ?? acc.RegOpCalcAccount.ContragentBankCrOrg.Name,
                        acc.RegOpCalcAccount.OpenDate,
                        acc.RegOpCalcAccount.CloseDate,
                        op.OperationSum,
                        op.OperationType,
                        RoId = acc.RealityObject.Id,
                        AccId = acc.RegOpCalcAccount.Id
                    })
                .ToList()
                .GroupBy(x => x.AccId)
                .Select(x => new
                {
                    Id = x.Key,
                    OpenDate = x.FirstOrDefault().Return(f => f.OpenDate),
                    CloseDate = x.FirstOrDefault().Return(f => f.CloseDate),
                    CreditOrg = x.FirstOrDefault().Return(f => f.CreditOrg),
                    Credit =
                        x.Where(c => c.OperationType == PaymentOperationType.OutcomeAccountPayment)
                            .SafeSum(c => c.OperationSum),
                    Debt = x.Where(c => c.OperationType == PaymentOperationType.Income
                                        || c.OperationType == PaymentOperationType.IncomeRegionalSubsidy
                                        || c.OperationType == PaymentOperationType.IncomeStimulateSubsidy
                                        || c.OperationType == PaymentOperationType.IncomeGrantInAid
                                        || c.OperationType == PaymentOperationType.IncomeFundSubsidy)
                        .SafeSum(c => c.OperationSum),
                    PercentSum = x.Where(c => c.OperationType == PaymentOperationType.BankPercent).SafeSum(c => c.OperationSum),
                    ContragentBank = x.FirstOrDefault().Return(f => f.ContragentBank)
                })
                .Select(x => new Proxy
                {
                    id = x.Id,
                    OpenDate = x.OpenDate,
                    CloseDate = x.CloseDate,
                    CreditOrg = x.CreditOrg,
                    Credit = x.Credit,
                    Debt = x.Debt,
                    Saldo = x.Debt - x.Credit,
                    ContragentBank = x.ContragentBank,
                    PercentSum = x.PercentSum
                }).ToList();

            var accids = data.Select(x => x.id).ToList();

            var accs = _regopCalcDomain.GetAll()
                .Where(x => !accids.Contains(x.Id) && x.RegOperator.Id == regopId)
                .Select(x => new Proxy
                {
                    id = x.Id,
                    OpenDate = x.OpenDate,
                    CloseDate = x.CloseDate,
                    CreditOrg = x.ContragentBankCrOrg.CreditOrg.Name ?? x.ContragentBankCrOrg.Name,
                    Credit = 0,
                    Debt = 0,
                    Saldo = 0,
                    ContragentBank = x.ContragentBankCrOrg.SettlementAccount,
                    PercentSum = 0
                }).ToList();

            return accs.Union(data);
        }

        public IDataResult ListSpecialAccounts(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            // Получаем счета, у которых есть операции оплаты чере джоин
            // Данный запрос вернет не все счета
            var data = _specAccDomain.GetAll()
                .Where(x => x.IsActive)
                .Where(x => x.AccountType == CrFundFormationDecisionType.SpecialAccount && x.RegOperator != null)
                .Join(_roPayOpDomain.GetAll(), account => account.RealityObjectChargeAccount.RealityObject,
                    account => account.Account.RealityObject, (x, y) => new
                    {
                        x.Id,
                        x.RealityObjectChargeAccount.RealityObject.Address,
                        Municipality = x.RealityObjectChargeAccount.RealityObject.Municipality.Name,
                        y.OperationSum,
                        y.OperationType,
                        RoId = x.RealityObjectChargeAccount.RealityObject.Id
                    });

            var tmpData = data.Paging(loadParams).ToList()
                .GroupBy(x => x.RoId)
                .Select(x => new
                {
                    Address = x.FirstOrDefault().Return(r => r.Address),
                    Municipality = x.FirstOrDefault().Return(r => r.Municipality),
                    Credit =
                        x.Where(c => c.OperationType == PaymentOperationType.OutcomeAccountPayment)
                            .SafeSum(c => c.OperationSum),
                    Debt = x.Where(c => c.OperationType == PaymentOperationType.Income
                                        || c.OperationType == PaymentOperationType.IncomeRegionalSubsidy
                                        || c.OperationType == PaymentOperationType.IncomeStimulateSubsidy
                                        || c.OperationType == PaymentOperationType.IncomeGrantInAid
                                        || c.OperationType == PaymentOperationType.IncomeFundSubsidy)
                        .SafeSum(c => c.OperationSum),
                    RoId = x.Key
                }).ToList();

            // Т.к. предыдущий запрос вернул только счета с операциями оплаты,
            // То получим еще и остальные
            var withOps = data.Select(x => x.Id).ToList();
            var withNoOperations = _specAccDomain.GetAll()
                .Where(x => x.IsActive)
                .Where(x => x.AccountType == CrFundFormationDecisionType.SpecialAccount && x.RegOperator != null)
                .Where(x => !withOps.Contains(x.Id))
                .Select(x => new
                {
                    AccountNumber = string.Empty,
                    OpenDate = (DateTime?)DateTime.MinValue,
                    Debt = 0m,
                    Credit = 0m,
                    Saldo = 0m,
                    x.RealityObjectChargeAccount.RealityObject.Address,
                    Municipality = x.RealityObjectChargeAccount.RealityObject.Municipality.Name,
                    CreditOrg = string.Empty,
                    RoId = x.RealityObjectChargeAccount.RealityObject.Id
                }).ToList();

            // Получим решения по кредитной организации по домам
            var ids =
                tmpData.Select(x => new RealityObject {Id = x.RoId})
                    .Union(withNoOperations.Select(x => new RealityObject {Id = x.RoId}));
            var numbers = GetDecisionCreditOrgNumber(ids);
            var nums = _bankProvider.GetBankNumbersForCollection(ids);

            var resData = tmpData.Select(x => new
            {
                AccountNumber = nums.ContainsKey(x.RoId) ? nums[x.RoId] : string.Empty,
                OpenDate = numbers.ContainsKey(x.RoId) && numbers[x.RoId].Protocol != null ? (DateTime?)numbers[x.RoId].Protocol.ProtocolDate : null,
                x.Debt,
                x.Credit,
                Saldo = x.Debt - x.Credit,
                x.Address,
                x.Municipality,
                CreditOrg = numbers.ContainsKey(x.RoId) ? numbers[x.RoId].Decision.Return(d => d.Name) : null,
                x.RoId
            })
                .ToList()
                .Union(withNoOperations.Select(x => new
                {
                    AccountNumber = nums.ContainsKey(x.RoId) ? nums[x.RoId] : string.Empty,
                    OpenDate = numbers.ContainsKey(x.RoId) && numbers[x.RoId].Protocol != null ? (DateTime?)numbers[x.RoId].Protocol.ProtocolDate : null,
                    x.Debt,
                    x.Credit,
                    Saldo = x.Debt - x.Credit,
                    x.Address,
                    x.Municipality,
                    CreditOrg = numbers.ContainsKey(x.RoId) ? numbers[x.RoId].Decision.Return(d => d.Name) : null,
                    x.RoId
                }))
                .AsQueryable()
                .Order(loadParams)
                .Filter(loadParams, _container);

            return new ListDataResult(resData.Paging(loadParams), resData.Count());
        }

        public IDataResult ListOperations(BaseParams baseParams)
        {
            var accId = baseParams.Params.GetAsId("accId");
            var isCredit = baseParams.Params.GetAs<bool>("isCredit");

            var loadParam = baseParams.GetLoadParam();

            var roIds = _domain.GetAll()
                .Where(x => x.RegOpCalcAccount.Id == accId)
                .Select(x => x.RealityObject.Id).ToList();

            var data = _roPayOpDomain.GetAll()
                .WhereIf(isCredit, c => c.OperationType == PaymentOperationType.OutcomeAccountPayment)
                .WhereIf(!isCredit, c => c.OperationType == PaymentOperationType.Income
                                         || c.OperationType == PaymentOperationType.IncomeRegionalSubsidy
                                         || c.OperationType == PaymentOperationType.IncomeStimulateSubsidy
                                         || c.OperationType == PaymentOperationType.IncomeGrantInAid
                                         || c.OperationType == PaymentOperationType.IncomeFundSubsidy)
                .Where(x => roIds.Contains(x.Account.RealityObject.Id))
                .Select(op => new
                {
                    op.OperationSum,
                    op.OperationType,
                    op.Date,
                    op.OperationStatus
                })
                .Filter(loadParam, _container)
                .Order(loadParam);

            return new ListDataResult(data.Paging(loadParam), data.Count());
        }

        public IDataResult GetRegopMoneyInfo(BaseParams baseParams)
        {
            var regopId = baseParams.Params.GetAsId("regopId");

            var data = GetOperations(regopId).ToList();

            return new BaseDataResult(new
            {
                Debt = data.SafeSum(x => x.Debt),
                Credit = data.SafeSum(x => x.Credit),
                Saldo = data.SafeSum(x => x.Saldo),
                PercentSum = data.SafeSum(x => x.PercentSum)
            });
        }

        public IDataResult ListRealObjForRegopCalcAcc(BaseParams baseParams)
        {
            var realObjDomain = _container.ResolveDomain<RealityObject>();

            using (_container.Using(realObjDomain))
            {
                var loadParams = baseParams.GetLoadParam();

                var speccAccQuery = _specAccDomain.GetAll()
                    .Where(x => x.IsActive)
                    .Where(x => x.AccountType == CrFundFormationDecisionType.SpecialAccount && x.RegOperator != null);

                var data = realObjDomain.GetAll()
                    .Where(x => !speccAccQuery.Any(y => y.RealityObjectChargeAccount.RealityObject.Id == x.Id))
                    .Select(x => new
                    {
                        x.Id,
                        x.Address,
                        Municipality = x.Municipality.Name
                    })
                    .Order(loadParams)
                    .Filter(loadParams, _container);

                return new ListDataResult(data.Paging(loadParams), data.Count());
            }
        }

        private Dictionary<long, CreditOrgDecision> GetDecisionCreditOrgNumber(IEnumerable<RealityObject> ros)
        {
            var ids = ros.Select(x => x.Id).ToList();

            return _crDecDomain.GetAll()
                .Where(x => ids.Contains(x.Protocol.RealityObject.Id))
                //.Where(x => x.BankAccountNumber != null && x.BankAccountNumber != string.Empty)
                .OrderByDescending(x => x.Protocol.ProtocolDate)
                .ToList()
                .GroupBy(x => x.Protocol.RealityObject.Id)
                .ToDictionary(x => x.Key, x => x.FirstOrDefault());
        }

        private class Proxy
        {
            public long id;
            public DateTime? OpenDate;
            public DateTime? CloseDate;
            public string CreditOrg;
            public decimal Credit;
            public decimal Debt;
            public decimal Saldo;
            public decimal PercentSum;
            public string ContragentBank;
        }
    }
}