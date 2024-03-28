namespace Bars.Gkh.RegOperator.DomainModelServices.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.ConfigSections.RegOperator.Enums;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Exceptions;
    using Bars.Gkh.Modules.RegOperator.DomainService;
    using Bars.Gkh.Overhaul.Utils;
    using Bars.Gkh.RegOperator.DomainService;
    using Bars.Gkh.RegOperator.Dto;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Loan;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Extenstions;
    using Bars.Gkh.Utils;

    using NHibernate.Linq;

    /// <summary>
    /// Репозиторий источников займа
    /// </summary>
    public class LoanSourceRepository : ILoanSourceRepository
    {
        private readonly IDomainService<RealityObject> roDomain;
        private readonly IDomainService<CalcAccountRealityObject> calcaccRobjectDomain;
        private readonly IDomainService<RealityObjectPaymentAccount> payaccDomain;
        private readonly IDomainService<RealityObjectLoanWallet> loanWalletDomain;
        private readonly IDomainService<CalcAccountRealityObject> calcaccRoDomain;
        private readonly ICalcAccountService calcAccountService;
        private readonly IGkhConfigProvider configProvider;
        private readonly IRegopService regopService;

        /// <summary>
        /// .ctor
        /// </summary>
        public LoanSourceRepository(
            IDomainService<RealityObject> roDomain,
            IDomainService<CalcAccountRealityObject> calcaccRobjectDomain,
            IDomainService<RealityObjectPaymentAccount> payaccDomain,
            IDomainService<RealityObjectLoanWallet> loanWalletDomain,
            ICalcAccountService calcAccountService,
            IGkhConfigProvider configProvider,
            IRegopService regopService,
            IDomainService<CalcAccountRealityObject> calcaccRoDomain)
        {
            this.roDomain = roDomain;
            this.calcaccRobjectDomain = calcaccRobjectDomain;
            this.payaccDomain = payaccDomain;
            this.calcAccountService = calcAccountService;
            this.configProvider = configProvider;
            this.regopService = regopService;
            this.calcaccRoDomain = calcaccRoDomain;
            this.loanWalletDomain = loanWalletDomain;
        }

        /// <summary>
        /// Получить список доступных источников для займа
        /// </summary>
        public IDataResult ListAvailableSources(BaseParams baseParams)
        {
            try
            {
                var roIds = baseParams.Params.GetAs<long[]>("roIds");
                var robjects = this.roDomain.GetAll()
                    .Where(x => roIds.Contains(x.Id))
                    .ToArray();

                if (robjects.IsEmpty())
                    return BaseDataResult.Error("Не удалось получить информацию о жилых домах");

                var result = this.ListAvailableSources(robjects);

                var data = result
                    .Where(x => x.AvailableMoney > 0)
                    .OrderBy(x => x.Name)
                    .ToArray();

                return new ListDataResult(data, data.Length);
            }
            catch (GkhException e)
            {
                return BaseDataResult.Error(e.Message);
            }
            catch
            {
                return BaseDataResult.Error("Ошибка получения списка доступных источников для займа");
            }
        }

        /// <summary>
        /// Получить список доступных источников для займа
        /// </summary>
        public IEnumerable<LoanAvailableSource> ListAvailableSources(RealityObject[] robjects)
        {
            if (robjects.IsEmpty())
            {
                throw new GkhException("Не указаны дома");
            }

            var roIds = robjects
                .Select(x => x.Id)
                .ToArray();

            var calcAccount = this.GetCalcAccount(roIds);

            var availRoIds = this.GetAvailRobjectIds(calcAccount, robjects);

            var existsLoans = this.loanWalletDomain.GetAll()
                .Where(x => x.Loan.Source.Id == calcAccount.Id)
                .Where(x => roIds.Contains(x.Loan.LoanTaker.RealityObject.Id)
                        || availRoIds.Contains(x.Loan.LoanTaker.RealityObject.Id))
                .Select(x => new
                {
                    x.TypeSourceLoan,
                    Sum = x.Sum - x.ReturnedSum
                })
                .AsEnumerable()
                .GroupBy(x => x.TypeSourceLoan)
                .ToDictionary(x => x.Key, z => z.Sum(x => x.Sum));

            var payaccs = this.payaccDomain.GetAll()
                .Where(x => availRoIds.Contains(x.RealityObject.Id))
                .Where(x => !roIds.Contains(x.RealityObject.Id))
                .Fetch(x => x.BaseTariffPaymentWallet)
                .Fetch(x => x.DecisionPaymentWallet)
                .Fetch(x => x.PenaltyPaymentWallet)
                .Fetch(x => x.FundSubsidyWallet)
                .Fetch(x => x.TargetSubsidyWallet)
                .Fetch(x => x.RegionalSubsidyWallet)
                .Fetch(x => x.StimulateSubsidyWallet)
                .ToArray();

            var typesSource = (TypeSourceLoan[]) Enum.GetValues(typeof (TypeSourceLoan));

            var availSources = typesSource.ToDictionary(x => x, x => -existsLoans.Get(x));// вычитаем взятые займы

            foreach (var payacc in payaccs)
            {
                foreach (var type in typesSource)
                {
                    var wallet = payacc.GetWallet(type);

                    availSources[type] += wallet.Balance;
                }
            }

            return availSources
                .Select(x => new LoanAvailableSource
                {
                    TypeSource = x.Key,
                    Name = x.Key.GetEnumMeta().Display,
                    AvailableMoney = x.Value.RegopRoundDecimal(2)
                });
        }

        /// <summary>
        /// Сальдо регоператора
        /// </summary>
        /// <returns></returns>
        public RegopLoanInfo GetRegoperatorSaldo(long muId)
        {
            var loanLevel = this.configProvider.Get<RegOperatorConfig>().GeneralConfig.LoanConfig.Level;
            var fundMinSize = this.configProvider.Get<RegOperatorConfig>().GeneralConfig.FundMinSize;

            var regop = this.regopService.GetCurrentRegOperator();

            var contragentId = regop
                .Return(x => x.Contragent)
                .Return(x => x.Id);

            var query = this.calcaccRoDomain.GetAll()
                .Where(x => x.Account.AccountOwner.Id == contragentId)
                .Where(x => x.DateStart <= DateTime.Today)
                .Where(x => !x.DateEnd.HasValue || x.DateEnd > DateTime.Today)
                .Where(x => x.Account.TypeAccount == TypeCalcAccount.Regoperator)
                .Where(x => !((RegopCalcAccount)x.Account).IsTransit);

            switch (loanLevel)
            {
                case LoanLevel.Municipality:
                    query = query
                        .Where(x => x.RealityObject.Municipality.Id == muId);
                    break;
                case LoanLevel.Settlement:
                    query = query
                        .Where(x => x.RealityObject.MoSettlement.Id == muId);
                    break;
            }

            var calcAccountInfo = this.calcAccountService.GetAccountSummary(query);
            var avaliableSum = calcAccountInfo.SafeSum(x => x.Value.Saldo - (x.Value.LoanSum - x.Value.LoanReturnedSum));
            var availableLoanSum = avaliableSum - avaliableSum * (fundMinSize?.ToDivisional() ?? 0);

            return new RegopLoanInfo
            {
                AvailableSum = avaliableSum.RegopRoundDecimal(2),
                AvailableLoanSum = availableLoanSum.RegopRoundDecimal(2),

                // сумма заблокированных средств = сумма неоплаченных займов
                // далее к этой сумме мы прибавим ещё и сальдо домов с потребностью  + сальдо домов, у которых есть непогашенный займ, когда этот список сформируется
                BlockedSum = 0//calcAccountInfo.SafeSum(x => x.Value.LoanSum - x.Value.LoanReturnedSum)
            };
        }

        private RegopCalcAccount GetCalcAccount(long[] roIds)
        {
            var calcAccounts = this.calcAccountService.GetRobjectsAccounts(roIds, DateTime.Today);

            var accIds = calcAccounts.Select(x => x.Value.Id).ToHashSet();

            if (accIds.Count > 1)
            {
                throw new GkhException("Жилые дома принадлежат к разным счетам");
            }

            if (accIds.Count < 1)
            {
                throw new GkhException("Не удалось получить расчетный счет регоператора");
            }

            var calcAccount = calcAccounts.First().Value;

            if (calcAccount.TypeAccount != TypeCalcAccount.Regoperator)
            {
                throw new GkhException("Не удалось получить расчетный счет регоператора");
            }

            return (RegopCalcAccount)calcAccount;
        }

        private long[] GetAvailRobjectIds(CalcAccount calcAccount, RealityObject[] robjects)
        {
            var level = this.configProvider.Get<RegOperatorConfig>().GeneralConfig.LoanConfig.Level;

            var roIds = robjects
                .Select(x => x.Id)
                .ToArray();

            var roQuery = this.calcaccRobjectDomain.GetAll()
                .Where(x => x.Account.Id == calcAccount.Id)
                .Where(x => !roIds.Contains(x.RealityObject.Id))
                .Where(x => x.DateStart <= DateTime.Today)
                .Where(x => !x.DateEnd.HasValue || x.DateEnd > DateTime.Today)
                .Where(x => !((RegopCalcAccount) x.Account).IsTransit)
                .Select(x => x.RealityObject);

            var robject = robjects.First();

            switch (level)
            {
                case LoanLevel.Municipality:
                {
                    var muId = robject.Municipality.Id;
                    roQuery = roQuery.Where(x => x.Municipality.Id == muId);
                    break;
                }
                    
                case LoanLevel.Settlement:
                {
                    var stlId = robjects.FirstOrDefault(x => x.MoSettlement != null)?.MoSettlement.Id ?? 0;
                    roQuery = roQuery.Where(x => x.MoSettlement.Id == stlId);
                    break;
                }
            }

            return roQuery
                .Select(x => x.Id)
                .ToArray();
        }
    }
}

