namespace Bars.Gkh.RegOperator.Tasks.Loans
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Exceptions;
    using Bars.Gkh.Overhaul.Utils;
    using Bars.Gkh.RegOperator.DomainEvent;
    using Bars.Gkh.RegOperator.DomainEvent.Events.RealityObjectLoan;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.DomainService;
    using Bars.Gkh.RegOperator.Dto;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.Entities;

    using ExecutionContext = Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    /// <summary>
    /// Исполнитель задачи "Автоматическое взятие займа"
    /// </summary>
    public class LoanTakerTaskExecutor : ITaskExecutor
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        private readonly ILoanReserver loanReserver;
        private readonly ICalcAccountService calcAccountService;
        private readonly ILoanSourceRepository loanSourceRepo;
        private readonly IRealityObjectLoanRepository roLoanRepo;
        private readonly IGkhConfigProvider configProvider;
        private readonly IRepository<RealityObject> roRepo;
        private readonly IDomainService<RealityObjectLoan> loanService;

        /// <summary>
        /// .ctor
        /// </summary>
        public LoanTakerTaskExecutor(
            ILoanReserver loanReserver,
            ILoanSourceRepository loanSourceRepo,
            IRepository<RealityObject> roRepo,
            IRealityObjectLoanRepository roLoanRepo,
            ICalcAccountService calcAccountService,
            IGkhConfigProvider configProvider,
            IDomainService<RealityObjectLoan> loanService)
        {
            this.loanReserver = loanReserver;
            this.loanSourceRepo = loanSourceRepo;
            this.roRepo = roRepo;
            this.roLoanRepo = roLoanRepo;
            this.calcAccountService = calcAccountService;
            this.configProvider = configProvider;
            this.loanService = loanService;
        }

        #region Implementation of ITaskExecutor

        /// <summary>
        /// Выполнить задачу
        /// </summary>
        public IDataResult Execute(BaseParams @params, ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            var roIds = @params.Params.GetAs<long[]>("roIds");
            var programId = @params.Params.GetAsId("programId");

            try
            {
                return this.ExecuteInternal(roIds, programId);
            }
            finally 
            {
                roIds.ForEach(x => DomainEvents.Raise(new RealityObjectLoanTaskEndEvent(new RealityObject { Id = x })));
            }
            
        }

        private IDataResult ExecuteInternal(long[] roIds, long programId)
        {
            var robjects = this.roRepo.GetAll()
                .Where(x => roIds.Contains(x.Id))
                .ToArray();

            var needLoans = this.roLoanRepo
                .ListRealtyObjectNeedLoan(robjects, new ProgramCr { Id = programId })
                .ToArray();

            var needLoanSum = needLoans
                .SafeSum(x => x.NeedSum)
                .RegopRoundDecimal(2);

            var availSources = this.loanSourceRepo.ListAvailableSources(robjects)
                .Where(x => x.AvailableMoney > 0)
                .ToArray();

            var account = this.GetSingleCalcAccount(roIds);

            var accSummary = this.calcAccountService.GetAccountSummary(account)
                .Return(x => x.Get(account.Id));

            var minSize = this.configProvider.Get<RegOperatorConfig>().GeneralConfig.FundMinSize;

            //если указан неснижаемый размер фонда
            //то не даем доступных для займа денег больше чем сальдо - сальдо*неснижаемый размер
            if (minSize.HasValue && minSize.Value > 0)
            {
                var saldo = accSummary.Saldo.RegopRoundDecimal(2);
                var loans = accSummary.LoanSum - accSummary.LoanReturnedSum;

                var minSaldo = (saldo * minSize.Value.ToDivisional()).RegopRoundDecimal(2);

                var maxMoneyToTake = (saldo - minSaldo - loans - needLoanSum).RegopRoundDecimal(2);

                if (maxMoneyToTake < 0)
                {
                    return new BaseDataResult(false, "Недостаточно средств для взятия займа");
                }

                foreach (var availSource in availSources)
                {
                    var money = Math.Min(availSource.AvailableMoney, maxMoneyToTake);

                    availSource.AvailableMoney = money;

                    maxMoneyToTake -= money;
                }
            }

            var sources = availSources
                .Where(x => x.AvailableMoney > 0)
                .Select(x =>
                {
                    //нужно брать не больше чем нужно
                    var money = Math.Min(needLoanSum, x.AvailableMoney);

                    needLoanSum -= money;

                    return new LoanTakenMoney
                    {
                        TakenMoney = money,
                        TypeSource = x.TypeSource
                    };
                })
                .ToArray();

            var loanCount = this.loanService.GetAll()
                .SafeMax(x => x.DocumentNum);

            var reserverParams = new BaseParams
            {
                Params = new DynamicDictionary
                {
                    { "roIds", roIds },
                    { "programId", programId },
                    { "takenMoney", sources },
                    { "loanCount", loanCount + 1 }
                }
            };

            return this.loanReserver.ReserveLoan(reserverParams);
        }

        private RegopCalcAccount GetSingleCalcAccount(long[] roIds)
        {
            var calcAccounts = this.calcAccountService.GetRobjectsAccounts(roIds, DateTime.Today);

            var accountIds = calcAccounts.Select(x => x.Value.Id).ToHashSet();

            if (accountIds.Count > 1)
            {
                throw new GkhException("Жилые дома принадлежат к разным счетам");
            }

            if (accountIds.Count < 1)
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

        /// <summary>
        /// Код исполнителя
        /// </summary>
        public string ExecutorCode { get; private set; }

        #endregion
    }
}
