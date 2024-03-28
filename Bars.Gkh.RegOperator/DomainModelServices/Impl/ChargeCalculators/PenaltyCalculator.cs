namespace Bars.Gkh.RegOperator.DomainModelServices.Impl.ChargeCalculators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Utils;
    using Bars.Gkh.RegOperator.Domain;
    using Bars.Gkh.RegOperator.Domain.Interfaces;
    using Bars.Gkh.RegOperator.Domain.ParametersVersioning;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Utils;
    using Dto;

    /// <summary>
    /// Калькулятор пени
    /// </summary>
    public partial class PenaltyCalculator
    {
        //константа, радость тракториста
        //исторически сложилось, что при расчете пени нужно делить на 300
        private const int TraktorsDriverJoy = 300;
        private readonly BasePersonalAccount account;

        private readonly ICalculationGlobalCache cache;
        private readonly List<TransferDto> payments;
        private readonly List<TransferDto> cancelPayments;
        private readonly List<TransferDto> cancelBaseTariffPayments;
        private readonly List<TransferDto> paymentsBaseTariff;
        private readonly List<TransferDto> returnsPaymentBaseTariff;
        private readonly List<TransferDto> returnsPaymentDecisionTariff;
        private readonly List<TransferDto> perfWorks;
        private readonly IPeriod period;
        private readonly bool simpleCalc;
        private readonly NumberDaysDelay numberDaysDelay;
        private readonly DateTime? DatePenaltyCalcTo;
        private readonly DateTime? DatePenaltyCalcFrom;
        private readonly bool calculatePenaltyForDecisionTarif;
        private readonly DateTime? NewPenaltyCalcStart;
        private readonly int? NewPenaltyCalcDays;

        private bool IsNewPenaltyCalc => this.NewPenaltyCalcStart <= this.period.StartDate && this.NewPenaltyCalcDays != null;

        private bool IsNewPenaltyCalcOfPeriod(IPeriod period) => this.NewPenaltyCalcStart <= period.StartDate && this.NewPenaltyCalcDays != null;

        private readonly List<PersonalAccountPeriodSummaryDto> summaries;

        private readonly List<RecalcHistoryDto> recalcHistory;

        private readonly IParameterTracker tracker;

        private decimal restructSum;

        private readonly List<RecalcHistory> chargeRecalcHistory;

        private readonly List<PersonalAccountChargeDto> accountCharges;
        private readonly HashSet<RecalcHistory> chargeRecalcHistoryToUse;
        private readonly List<PerformedWorkCharge> performedWorkCharge;

        private RecalcReasonProxy RecalculationReason;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="cache">Кэш расчетов</param>
        /// <param name="tracker">Версионируемые параметры</param>
        /// <param name="account">Лицевой счет</param>
        /// <param name="period">Период</param>
        public PenaltyCalculator(
            ICalculationGlobalCache cache,
            IParameterTracker tracker,
            BasePersonalAccount account,
            IPeriod period,
            List<RecalcHistory> chargeRecalcHistory)
        {
            this.cache = cache;
            this.tracker = tracker;
            this.account = account;
            this.period = period;
            this.summaries = this.cache.GetAllSummaries(this.account).ToList();
            this.recalcHistory = this.cache.GetRecalcHistory(this.account).ToList();
            this.payments = this.cache.GetPaymentTransfers(account).ToList();
            this.cancelPayments = this.cache.GetCancelPaymentTransfers(account).ToList();
            this.cancelBaseTariffPayments = this.cache.GetBaseTariffCancelPaymentTransfers(account).ToList();

            this.paymentsBaseTariff = this.cache.GetPaymentTransfersBaseTariff(account).ToList();
            this.returnsPaymentBaseTariff = this.cache.GetReturnTransfersBaseTariff(account).ToList();
            this.returnsPaymentDecisionTariff = this.cache.GetReturnTransfersDecisionTariff(account).ToList();
            this.perfWorks = this.cache.GetPerfWorks(account).ToList();
            this.simpleCalc = this.cache.SimpleCalculatePenalty;
            this.numberDaysDelay = this.cache.NumberDaysDelay;
            this.DatePenaltyCalcTo = this.cache.DatePenaltyCalcTo;
            this.DatePenaltyCalcFrom = this.cache.DatePenaltyCalcFrom;
            this.calculatePenaltyForDecisionTarif = this.cache.CalculatePenaltyForDecisionTarif;
            this.NewPenaltyCalcStart = this.cache.NewPenaltyCalcStart;
            this.NewPenaltyCalcDays = this.cache.NewPenaltyCalcDays;
            this.chargeRecalcHistory = chargeRecalcHistory;
            this.accountCharges = this.cache.GetAllCharges(account).ToList();
            this.performedWorkCharge = this.cache.GetPerfWorkCharge(account).ToList();

            this.chargeRecalcHistoryToUse = new HashSet<RecalcHistory>();
            this.Traces = new List<CalculationParameterTrace>();

            this.NewRecalcHistory = new List<RecalcHistory>();
        }

        public List<CalculationParameterTrace> Traces { get; private set; }

        public List<RecalcHistory> NewRecalcHistory { get; private set; }

        /// <summary>
        /// Расчет пени
        /// </summary>
        /// <returns>Результат расчета пени</returns>
        public CalculationResult<PenaltyResult> Calculate()
        {
            var needToRecalcPeriods = this.GetPeriodsForRecalc(this.period);
            var parameter = this.GetPenaltyParameter(this.period.StartDate, this.period.GetEndDate(), this.account);
            var currentPenalty = 0m;
            var recalc = 0m;
            var deadLine = this.DatePenaltyCalcTo != null
                ? this.DatePenaltyCalcTo.Value.AddDays(parameter.Return(x => x.Days))
                : this.period.StartDate.AddDays(parameter.Return(x => x.Days));

            var paymentsSum =
                this.paymentsBaseTariff.Where(x => x.PaymentDate.Date <= deadLine).SafeSum(x => x.Amount) -
                    this.returnsPaymentBaseTariff.Where(x => x.PaymentDate.Date <= deadLine).SafeSum(x => x.Amount);

            var simplePeriods = needToRecalcPeriods.Where(x => x.Simple).OrderBy(x => x.DateStart).ToList();

            PenaltyPeriodProxy prevBalance = null;

            foreach (var recalcPeriod in needToRecalcPeriods.OrderBy(x => x.DateStart))
            {
                prevBalance = recalcPeriod.Simple
                    ? this.SimpleCalcPeriod(recalcPeriod, ref paymentsSum)
                    : this.numberDaysDelay == NumberDaysDelay.StartDateMonth
                        ? this.CalcPeriod(recalcPeriod, this.account, simplePeriods, prevBalance)
                        : this.CalcDebtPeriod(recalcPeriod, this.account, simplePeriods, deadLine, prevBalance);

                if (prevBalance == null || recalcPeriod.Ignore)
                {
                    continue;
                }

                if (recalcPeriod.Id == this.period.Id)
                {
                    currentPenalty = this.ProcessCurrentPeriod(prevBalance, recalcPeriod);
                }
                else
                {
                    var recalcPenalty = this.ProcessClosedPeriod(prevBalance, recalcPeriod);

                    recalc += recalcPenalty;

                    var newRecalcHistory = new RecalcHistory
                    {
                        PersonalAccount = this.account,
                        CalcPeriod = (ChargePeriod)this.period,
                        RecalcPeriod = this.cache.GetClosedPeriods().FirstOrDefault(x => x.Id == recalcPeriod.Id),
                        RecalcSum = recalcPenalty,
                        RecalcType = RecalcType.Penalty
                    };

                    this.NewRecalcHistory.Add(newRecalcHistory);
                }
            }

            var result = new CalculationResult<PenaltyResult>(new PenaltyResult(currentPenalty, recalc));

            result.Traces.AddRange(this.Traces);
            result.RecalcHistory.AddRange(this.NewRecalcHistory);

            return result;
        }

        //расчет периода
        // 1) берем задолженность за предыдущий период
        // 2) для перерасчета пересчитываем задолженность
        // 3) применяем оплаты и начисления
        private PenaltyPeriodProxy CalcPeriod(
            Period period,
            BasePersonalAccount account,
            List<Period> simplePeriods,
            PenaltyPeriodProxy previous = null)
        {
            var periodStartDate = period.DateStart;

            if (this.NewPenaltyCalcStart != null && this.NewPenaltyCalcStart < period.DateStart)
            {
                periodStartDate = period.DateStart.AddMonths(-1);
            }

            //-----1
            var prevSummary = this.summaries
                .Where(x => x.Period.StartDate < periodStartDate)
                .OrderByDescending(x => x.Period.StartDate)
                .FirstOrDefault();

            decimal balanceIn = 0;
            decimal charge = 0;
            decimal percentage = 0;

            if (prevSummary != null)
            {
                balanceIn = this.calculatePenaltyForDecisionTarif 
                    ? prevSummary.BaseTariffDebt + prevSummary.DecisionTariffDebt - (prevSummary.TariffPayment + prevSummary.TariffDecisionPayment)
                    : prevSummary.BaseTariffDebt - prevSummary.TariffPayment;

                charge = this.calculatePenaltyForDecisionTarif
                    ? prevSummary.ChargeTariff
                    : prevSummary.ChargedByBaseTariff;

                var startPayment = new DateTime(period.DateEnd.Year, period.DateEnd.Month, 1);

                var paymentSum = this.calculatePenaltyForDecisionTarif
                   ? this.payments
                       .Where(x => x.OperationDate >= startPayment && x.PaymentDate < period.DateStart)
                       .SafeSum(x => x.Amount)
                       - this.cancelPayments.Where(x => x.OperationDate >= startPayment && x.PaymentDate < period.DateStart).SafeSum(x => x.Amount)
                   : this.paymentsBaseTariff
                       .Where(x => x.OperationDate >= startPayment && x.PaymentDate < period.DateStart)
                       .SafeSum(x => x.Amount)
                       - this.cancelBaseTariffPayments.Where(x => x.OperationDate >= startPayment && x.PaymentDate < period.DateStart)
                           .SafeSum(x => x.Amount);

                //вычитаем оплаты
                balanceIn -= paymentSum;
            }

            //----1

            decimal recalcBalanceIn;
            decimal recalcCharge;

            //----2
            var startDate = prevSummary != null ? prevSummary.Period.StartDate : period.DateStart.AddMonths(-1);
            var endDate = prevSummary != null ? prevSummary.Period.GetEndDate() : period.DateStart.AddDays(-1);

            //Если предыдущий период был простой, то нужно пересчитать задолженность до расчитываемого периода
            if (simplePeriods != null && previous.ReturnSafe(x => x.Simple) && previous != null)
            {
                previous.RecalcBalance = 0;
                balanceIn += charge;

                foreach (var per in simplePeriods)
                {
                    previous.RecalcBalance += this.RecalcCharge(
                        account,
                        per.DateStart,
                        per.DateEnd).RegopRoundDecimal(2);
                }

                previous.RecalcBalance -= this.calculatePenaltyForDecisionTarif
                    ? this.payments
                        .Where(x => x.PaymentDate < period.Deadline)
                        .SafeSum(x => x.Amount) - this.cancelPayments.Where(x => x.PaymentDate < period.Deadline).SafeSum(x => x.Amount)
                    : this.paymentsBaseTariff
                        .Where(x => x.PaymentDate < period.Deadline)
                        .SafeSum(x => x.Amount) - this.cancelBaseTariffPayments.Where(x => x.PaymentDate < period.Deadline)
                            .SafeSum(x => x.Amount);

                previous.RecalcBalance += this.RecalcCharge(
                    account,
                    startDate,
                    endDate);
            }            

            if (previous != null)
            {
                recalcBalanceIn = previous.RecalcBalance;
                recalcCharge = this.RecalcCharge(
                    account,
                    startDate,
                    endDate);

                // в следующем периоде после первого периода по новым настройкам нужно прибавить начисления
                // потому что в прошлом мы их не прибавили
                if (this.NewPenaltyCalcStart != null && this.NewPenaltyCalcStart == period.DateStart.AddMonths(-1))
                {
                    recalcBalanceIn += this.RecalcCharge(
                    account,
                    startDate.AddMonths(-1),
                    startDate.AddDays(-1));
                }

                percentage = period.RecalcPercent;
            }
            else
            {
                var currentRecalc = this.chargeRecalcHistory
                   .Where(x => x.CalcPeriod.Id == this.period.Id && x.RecalcPeriod.StartDate <= periodStartDate)
                   .SafeSum(x => x.RecalcSum);

                recalcBalanceIn = balanceIn + Math.Min(currentRecalc, 0);
                recalcCharge = prevSummary != null ? charge : this.RecalcCharge(account, startDate, endDate);

                percentage = period.RecalcPercent;
            }

            // Если считаем первый период с новой настройкой, то не прибавляем начисление ко второму периоду задолженности
            if ((this.NewPenaltyCalcStart != null && this.NewPenaltyCalcStart.Value.Month == period.DateEnd.Month) || prevSummary == null)
            {
                recalcCharge = 0;
            }

            if (prevSummary != null)
            {
                var prevRecalcCharge = this.chargeRecalcHistoryToUse.SafeSum(x => x.RecalcSum);
                this.chargeRecalcHistoryToUse.Clear();

                recalcBalanceIn += prevRecalcCharge < 0 ? prevRecalcCharge : 0M;
            }
            else
            {
                this.chargeRecalcHistory
                    .Where(x => x.CalcPeriod.Id == this.period.Id && x.RecalcPeriod.StartDate == startDate)
                    .AddTo(this.chargeRecalcHistoryToUse);
            }

            recalcBalanceIn += this.recalcHistory
                    .WhereIf(!this.calculatePenaltyForDecisionTarif, x => x.RecalcType == RecalcType.BaseTariffCharge)
                    .WhereIf(
                        this.calculatePenaltyForDecisionTarif,
                        x => x.RecalcType == RecalcType.BaseTariffCharge || x.RecalcType == RecalcType.DecisionTariffCharge)
                    .Where(x => x.RecalcPeriod.Id == period.Id).SafeSum(x => x.RecalcSum);

            recalcBalanceIn = Math.Min(recalcBalanceIn, balanceIn);

            if (this.NewPenaltyCalcStart == null || this.NewPenaltyCalcStart != period.DateStart)
            {
                var recalcSum = this.recalcHistory
                .WhereIf(!this.calculatePenaltyForDecisionTarif, x => x.RecalcType == RecalcType.BaseTariffCharge)
                .WhereIf(
                    this.calculatePenaltyForDecisionTarif,
                    x => x.RecalcType == RecalcType.BaseTariffCharge || x.RecalcType == RecalcType.DecisionTariffCharge)
                .Where(x => x.CalcPeriod.StartDate == startDate)
                .SafeSum(x => x.RecalcSum);

                recalcCharge += recalcSum > 0 ? recalcSum : 0;
            }

            // считаем сумму всех возвратов по базовому тарифу
            var returnPaymentsByBase = this.returnsPaymentBaseTariff
                .Where(x => x.PaymentDate > period.DateStart && x.PaymentDate <= period.DateEnd)
                .SafeSum(x => x.Amount);

            // считаем сумму всех возвратов по тарифу решения
            var returnPaymentsByDecision = this.returnsPaymentDecisionTariff
                .Where(x => x.PaymentDate > period.DateStart && x.PaymentDate <= period.DateEnd)
                .SafeSum(x => x.Amount);

            var transfersMerge = this.payments
                .Where(x => x.Operation.Reason == "Перенос долга при слиянии" && x.PaymentDate <= endDate && x.PaymentDate >= startDate)
                .SafeSum(x => x.Amount);

            // зачет средств за проделанные работы
            var perfWorkCharge = this.performedWorkCharge
                .Where(x => x.ChargePeriod.StartDate == periodStartDate.AddMonths(-1))
                .SafeSum(x => x.Sum);

            recalcCharge -= perfWorkCharge;

            //увеличиваем задолженность на сумму возвратов и уменьшаем на сумму возвратов
            recalcBalanceIn += this.calculatePenaltyForDecisionTarif
                ? returnPaymentsByBase + returnPaymentsByDecision + transfersMerge
                : returnPaymentsByBase + transfersMerge;

            var operations = this.GetPartitionOperations(period);

            var periodBalance = new PenaltyPeriodProxy(period.DateStart, recalcBalanceIn, percentage, period.StartFix, period.EndFix);

            if (period.Id != this.period.Id)
            {
                operations.Add(
                    new OperationProxy
                    {
                        Date = period.Deadline.AddDays(-1),
                        Amount = recalcCharge,
                        SkipOperation = period.Deadline == period.DateEnd,
                        NeedSavePayment = false
                    });
            }

            // важен порядок и группировка по дате без времени операции
            operations.OrderBy(x => x.Date).GroupBy(x => x.Date.Date).Select(
                x =>
                    {
                        var partitions = x.GroupBy(y => y.PeriodPartitionType)
                                .ToDictionary(y => y.Key, this.CreatePartitionPartition)
                                .Values.ToArray();

                        return new OperationProxyResult
                                   {
                                       Date = x.Key,
                                       NeedSavePayment = x.Last().NeedSavePayment,
                                       SkipOperation = x.All(z => z.SkipOperation),
                                       Percentage = x.Select(z => z.Percentage).FirstOrDefault(z => z > 0),
                            Partitions = partitions
                                   };
                    }).OrderBy(x => x.Date).ForEach(x => periodBalance.ApplySum(x));

            return periodBalance;
        }

        private PenaltyPeriodProxy CalcDebtPeriod(
            Period period,
            BasePersonalAccount account,
            List<Period> simplePeriods,
            DateTime deadLine,
            PenaltyPeriodProxy previous = null)
        {
            var periodStartDate = period.DateStart;

            if (this.NewPenaltyCalcStart != null && this.NewPenaltyCalcStart <= period.DateStart)
            {
                periodStartDate = period.DateStart.AddMonths(-1);
            }

            //получаем предпредыдущий период
            var prevSummary = this.summaries
                .Where(x => x.Period.StartDate < periodStartDate)
                .OrderByDescending(x => x.Period.StartDate)
                .FirstOrDefault();

            //получаем предыдущий период
            var summary = this.summaries
                .FirstOrDefault(x => x.Period.StartDate == periodStartDate);

            var prevCharge = this.accountCharges.Where(x => x.ChargeDate < periodStartDate)
                .OrderByDescending(x => x.ChargeDate)
                .FirstOrDefault();

            decimal balanceIn = 0;
            decimal charge = 0;
            decimal recalcBalanceIn = 0;
            decimal percentage = 0;

            if (summary != null && prevCharge != null)
            {
                charge = this.calculatePenaltyForDecisionTarif ? prevCharge.ChargeTariff + prevCharge.OverPlus : prevCharge.ChargeTariff;

                balanceIn = this.calculatePenaltyForDecisionTarif ? summary.BaseTariffDebt + summary.DecisionTariffDebt : summary.BaseTariffDebt;
            }

            //все оплаты, которые были в пердыдущем периоде
            var paymentSum = this.calculatePenaltyForDecisionTarif
                ? this.payments
                    .Where(x => x.OperationDate >= periodStartDate && x.PaymentDate < period.Deadline)
                    .SafeSum(x => x.Amount)
                    - this.cancelPayments.Where(x => x.OperationDate >= periodStartDate && x.PaymentDate <= period.Deadline).SafeSum(x => x.Amount)
                : this.paymentsBaseTariff
                    .Where(x => x.OperationDate >= periodStartDate && x.PaymentDate < period.Deadline)
                    .SafeSum(x => x.Amount)
                    - this.cancelBaseTariffPayments.Where(x => x.OperationDate >= periodStartDate && x.PaymentDate <= period.Deadline)
                        .SafeSum(x => x.Amount);

            //вычитаем оплаты
            balanceIn -= paymentSum;

            var startDate = prevSummary != null ? prevSummary.Period.StartDate : period.DateStart.AddMonths(-1);
            var endDate = prevSummary != null ? prevSummary.Period.GetEndDate() : period.DateStart.AddDays(-1);

            //Если предыдущий период был простой, то нужно пересчитать задолженность до расчитываемого периода
            if (simplePeriods != null && previous.ReturnSafe(x => x.Simple) && previous != null)
            {
                previous.RecalcBalance = 0;
                balanceIn += charge;

                foreach (var per in simplePeriods)
                {
                    previous.RecalcBalance += this.RecalcCharge(
                        account,
                        per.DateStart,
                        per.DateEnd).RegopRoundDecimal(2);
                }

                previous.RecalcBalance -= this.paymentsBaseTariff.Where(x => x.PaymentDate.Date <= deadLine).SafeSum(x => x.Amount);
            }

            if (previous == null)
            {
                var recalcHistorySum = this.recalcHistory
                    .WhereIf(!this.calculatePenaltyForDecisionTarif, x => x.RecalcType == RecalcType.BaseTariffCharge)
                    .WhereIf(
                        this.calculatePenaltyForDecisionTarif,
                        x => x.RecalcType == RecalcType.BaseTariffCharge || x.RecalcType == RecalcType.DecisionTariffCharge)
                    .Where(x => x.RecalcPeriod.StartDate <= startDate)
                    .Where(x => x.CalcPeriod.StartDate >= period.DateStart)
                    .Where(x => x.RecalcSum < 0)
                    .SafeSum(x => x.RecalcSum);

                var currentRecalc = this.chargeRecalcHistory
                    .Where(x => x.CalcPeriod.Id == this.period.Id && x.RecalcPeriod.StartDate <= periodStartDate)
                    .SafeSum(x => x.RecalcSum);

                recalcBalanceIn = balanceIn + Math.Min(recalcHistorySum + currentRecalc, 0);

                if (summary == null)
                {
                    recalcBalanceIn -= this.calculatePenaltyForDecisionTarif
                    ? this.payments
                        .Where(x => x.OperationDate >= periodStartDate && x.PaymentDate < period.Deadline)
                        .SafeSum(x => x.Amount)
                        - this.cancelPayments.Where(x => x.OperationDate >= periodStartDate && x.PaymentDate <= period.Deadline).SafeSum(x => x.Amount)
                    : this.paymentsBaseTariff
                        .Where(x => x.OperationDate >= periodStartDate && x.PaymentDate < period.Deadline)
                        .SafeSum(x => x.Amount)
                        - this.cancelBaseTariffPayments.Where(x => x.OperationDate >= periodStartDate && x.PaymentDate <= period.Deadline)
                            .SafeSum(x => x.Amount);
                }

                percentage = period.RecalcPercent;
            }
            else
            {
                var banRecalc = this.cache.GetBanRecalc(account)
                    .Any(x => x.DateStart <= startDate && x.DateEnd >= startDate && x.Type.HasFlag(BanRecalcType.Penalty));

                charge = banRecalc
                    ? charge
                    : this.RecalcCharge(
                        account,
                        startDate,
                        endDate);

                var recalcHistorySum = this.recalcHistory
                    .WhereIf(!this.calculatePenaltyForDecisionTarif, x => x.RecalcType == RecalcType.BaseTariffCharge)
                    .WhereIf(
                        this.calculatePenaltyForDecisionTarif,
                        x => x.RecalcType == RecalcType.BaseTariffCharge || x.RecalcType == RecalcType.DecisionTariffCharge)
                    .Where(x => x.RecalcPeriod.StartDate <= startDate.AddMonths(-1))
                    .Where(x => x.CalcPeriod.StartDate >= startDate.AddMonths(-1))
                    .SafeSum(x => x.RecalcSum);


                // Если считаем первый период с новой настройкой, то не прибавляем начисление ко второму периоду задолженности
                if ((this.NewPenaltyCalcStart != null && this.NewPenaltyCalcStart == period.DateStart) || prevSummary == null)
                {
                    recalcBalanceIn = previous.RecalcBalance;
                }
                else
                {
                    previous.RecalcBalance += charge;

                    recalcBalanceIn = banRecalc || recalcHistorySum < 0 ? previous.RecalcBalance : Math.Min(balanceIn, previous.RecalcBalance);

                    var recalcSum = this.recalcHistory
                        .WhereIf(!this.calculatePenaltyForDecisionTarif, x => x.RecalcType == RecalcType.BaseTariffCharge)
                        .WhereIf(
                            this.calculatePenaltyForDecisionTarif,
                            x => x.RecalcType == RecalcType.BaseTariffCharge || x.RecalcType == RecalcType.DecisionTariffCharge)
                        .Where(x => x.CalcPeriod.StartDate == startDate)
                        .SafeSum(x => x.RecalcSum);

                    recalcBalanceIn += recalcSum > 0 ? recalcSum : 0;
                }

                percentage = period.RecalcPercent;
            }

            if (prevSummary != null)
            {
                var prevRecalcCharge = this.chargeRecalcHistoryToUse.SafeSum(x => x.RecalcSum);
                this.chargeRecalcHistoryToUse.Clear();

                recalcBalanceIn += prevRecalcCharge < 0 ? prevRecalcCharge : 0M;
            }
            else
            {
                this.chargeRecalcHistory
                    .Where(x => x.CalcPeriod.Id == this.period.Id && x.RecalcPeriod.StartDate == startDate)
                    .AddTo(this.chargeRecalcHistoryToUse);
            }

            // считаем сумму всех возвратов по базовому тарифу
            var returnPaymentsByBase = this.returnsPaymentBaseTariff
                .Where(x => x.PaymentDate > periodStartDate && x.PaymentDate <= period.DateEnd)
                .SafeSum(x => x.Amount);

            // считаем сумму всех возвратов по тарифу решения
            var returnPaymentsByDecision = this.returnsPaymentDecisionTariff
                .Where(x => x.PaymentDate > periodStartDate && x.PaymentDate <= period.DateEnd)
                .SafeSum(x => x.Amount);

            var transfersMerge = this.payments
                .Where(x => x.Operation.Reason == "Перенос долга при слиянии" && x.PaymentDate <= periodStartDate.AddMonths(1).AddDays(-1) && x.PaymentDate >= periodStartDate)
                .SafeSum(x => x.Amount);

            // зачет средств за проделанные работы
            var perfWorkCharge = this.performedWorkCharge
                .Where(x => x.ChargePeriod.StartDate == periodStartDate.AddMonths(-1))
                .SafeSum(x => x.Sum);

            //увеличиваем задолженность на сумму возвратов и уменьшаем на сумму зачетов
            if (this.NewPenaltyCalcStart == null || this.NewPenaltyCalcStart != period.DateStart)
            {
                recalcBalanceIn += this.calculatePenaltyForDecisionTarif
                ? returnPaymentsByBase + returnPaymentsByDecision + transfersMerge - perfWorkCharge
                : returnPaymentsByBase + transfersMerge - perfWorkCharge;
            }

            var operations = this.GetDebtPartitionOperations(period);

            var periodBalance = new PenaltyPeriodProxy(period.Deadline, recalcBalanceIn, period.DebtEnd, percentage);

            // важен порядок и группировка по дате без времени операции
            operations.OrderBy(x => x.Date).GroupBy(x => x.Date.Date).Select(
                x =>
                    {
                        var partitions = x.GroupBy(y => y.PeriodPartitionType)
                                .ToDictionary(y => y.Key, this.CreatePartitionPartition)
                                .Values.ToArray();

                        return new OperationProxyResult
                                   {
                                       Date = x.Key,
                                       NeedSavePayment = x.Last().NeedSavePayment,
                                       SkipOperation = x.All(z => z.SkipOperation),
                                       Percentage = x.Select(z => z.Percentage).FirstOrDefault(z => z > 0),
                                       Partitions = partitions
                                   };
                    }).OrderBy(x => x.Date).ForEach(x => periodBalance.ApplyDebtSum(x));

            if (periodBalance.Penalties.Count == 0)
            {
                var newPenalty = new PenaltyPeriod
                {
                    Start = period.Deadline,
                    RecalcBalance = periodBalance.RecalcBalance,
                    End = periodBalance.DebtEnd,
                    Percentage = percentage
                };

                periodBalance.Penalties.Add(newPenalty);
            }

            return periodBalance;
        }

        // расчет периода
        // берем текущее начисление + перерасчет и вычитаем из нее оствшуюся оплату
        private PenaltyPeriodProxy SimpleCalcPeriod(Period period, ref decimal paymentBalance)
        {
            var charge = this.RecalcCharge(this.account, period.DateStart, period.DateEnd);

            PenaltyPeriodProxy periodBalance;

            if ((charge > paymentBalance && charge != 0) || charge < 0)
            {
                var balanceIn = charge - paymentBalance;
                paymentBalance = 0;
                periodBalance = new PenaltyPeriodProxy(period.DateStart, balanceIn, period.RecalcPercent, null, null);
            }
            else
            {
                paymentBalance -= charge;
                periodBalance = new PenaltyPeriodProxy(period.DateStart, 0, period.RecalcPercent, null, null);
            }

            periodBalance.SimpleApplySum(period.Deadline);

            periodBalance.Simple = true;

            return periodBalance;
        }

        private decimal ProcessCurrentPeriod(PenaltyPeriodProxy prevBalance, Period recalcPeriod)
        {
            var penalty = 0m;

            foreach (var penaltyPeriod in prevBalance.Penalties)
            {
                var end = penaltyPeriod.End ?? recalcPeriod.DateEnd;

                var days = (end - penaltyPeriod.Start.Date).Days + 1;

                if (days > 0)
                {
                    this.RestructMe(penaltyPeriod);

                    //задолженность должна быть неотрицательной
                    var recalcDebt = Math.Max(penaltyPeriod.RecalcBalance, 0m);

                    penalty += recalcDebt
                        * penaltyPeriod.Percentage.ToDivisional()
                        * days / PenaltyCalculator.TraktorsDriverJoy;

                    var paramTrace = new CalculationParameterTrace
                    {
                        DateStart = penaltyPeriod.Start.Date,
                        DateEnd = end,
                        CalculationType = CalculationTraceType.Penalty,
                        ParameterValues = new Dictionary<string, object>
                    {
                        { "payment_penalty_percentage", penaltyPeriod.Percentage },
                        { "penalty_debt", recalcDebt },
                        { "numberdays_delay", this.numberDaysDelay },
                        { "penalty_decision", this.calculatePenaltyForDecisionTarif },
                        { "payment", penaltyPeriod.Payment },
                        { "payment_date", penaltyPeriod.End },
                        { "is_fix_calc_period", this.cache.IsFixCalcPeriod },
                        { "start_fix", recalcPeriod.StartFix },
                        { "end_fix", recalcPeriod.EndFix },
                        { "partition_details", penaltyPeriod.Partitions }
                    },
                    ChargePeriod = (ChargePeriod)this.period
                    };

                    this.Traces.Add(paramTrace);
                }
            }

            return penalty;
        }

        private decimal ProcessClosedPeriod(PenaltyPeriodProxy prevBalance, Period recalcPeriod)
        {
            var currentPeriodRecalc = 0m;

            foreach (var penaltyPeriod in prevBalance.Penalties)
            {
                //если упрощенный расчет то задолженность может быть отрицательной
                var debt = recalcPeriod.Simple ? penaltyPeriod.RecalcBalance : Math.Max(penaltyPeriod.RecalcBalance, 0m);
                var end = penaltyPeriod.End ?? recalcPeriod.DateEnd;
                var days = (end - penaltyPeriod.Start.Date).Days + 1;

                if (days > 0)
                {
                    var currentRecalc = (debt * penaltyPeriod.Percentage.ToDivisional() * days / TraktorsDriverJoy).RegopRoundDecimal(2);

                    currentPeriodRecalc += currentRecalc;

                    this.Traces.Add(
                        new CalculationParameterTrace
                        {
                            DateStart = penaltyPeriod.Start.Date,
                            DateEnd = penaltyPeriod.End,
                            CalculationType = CalculationTraceType.PenaltyRecalcDetail,
                            ParameterValues = new Dictionary<string, object>
                            {
                                {"payment", penaltyPeriod.Payment},
                                {"payment_date", penaltyPeriod.End},
                                {"partition_details", penaltyPeriod.Partitions},
                                {"debt", debt},
                                {"recalc_penalty", currentRecalc},
                                {"recalc_percent", penaltyPeriod.Percentage},
                                {"is_fix_calc_period", this.cache.IsFixCalcPeriod},
                                {"start_fix", recalcPeriod.StartFix},
                                {"end_fix", recalcPeriod.EndFix}
                        },
                        ChargePeriod =  (ChargePeriod)this.period
                        });
                }
            }

            //ищем перерасчеты в других периодах и прибавляем
            var recalcInNextPeriods = this.recalcHistory
                .Where(x => x.RecalcPeriod.Id == recalcPeriod.Id)
                .Where(x => x.RecalcType == RecalcType.Penalty)
                .SafeSum(x => x.RecalcSum);

            var factPenaltyCharge = recalcPeriod.Penalty + recalcInNextPeriods;

            this.Traces.Add(
                new CalculationParameterTrace
                {
                    DateStart = prevBalance.periodStart,
                    DateEnd = prevBalance.DebtEnd ?? prevBalance.periodStart.AddMonths(1).AddDays(-1),
                    CalculationType = recalcPeriod.Simple ? CalculationTraceType.DelayPenaltyRecalc : CalculationTraceType.PenaltyRecalc,
                    ParameterValues = new Dictionary<string, object>
                    {
                        {"recalc_percent", prevBalance.Percentage},
                        {"recalc_penalty", currentPeriodRecalc.RegopRoundDecimal(2) },
                        {"penalty", factPenaltyCharge },
                        {"recalc_reason", this.RecalculationReason?.Reason},
                        {"recalc_reason_date", this.RecalculationReason?.Date},
                        {"numberdays_delay", this.numberDaysDelay},
                        {"is_fix_calc_period", this.cache.IsFixCalcPeriod},
                        {"start_fix", recalcPeriod.StartFix },
                        {"end_fix", recalcPeriod.EndFix },
                        {"penalty_decision", this.calculatePenaltyForDecisionTarif}
                    },
                    ChargePeriod = (ChargePeriod)this.period
                });

            return currentPeriodRecalc.RegopRoundDecimal(2) - factPenaltyCharge;
        }

        private void RestructMe(PenaltyPeriod penaltyPeriod)
        {
            if (this.restructSum != 0M)
            {
                if (this.restructSum >= penaltyPeriod.RecalcBalance)
                {
                    this.restructSum -= penaltyPeriod.RecalcBalance;
                    penaltyPeriod.RecalcBalance = 0M;
                }
                else
                {
                    penaltyPeriod.RecalcBalance -= this.restructSum;
                    this.restructSum = 0M;
                }
            }
        }
    }
}