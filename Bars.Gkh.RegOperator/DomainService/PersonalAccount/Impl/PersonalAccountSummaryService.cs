namespace Bars.Gkh.RegOperator.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.Overhaul.Utils;
    using Bars.Gkh.RegOperator.Domain;
    using Bars.Gkh.RegOperator.Domain.ParametersVersioning;
    using Bars.Gkh.RegOperator.Domain.Repository;
    using Bars.Gkh.RegOperator.DomainModelServices.Impl.ChargeCalculators;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Dict;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.Gkh.Utils;

    using Castle.Windsor;
    using ConfigSections.RegOperator;
    /// <summary>
    /// Сервис
    /// </summary>
    public class PersonalAccountSummaryService : IPersonalAccountSummaryService
    {
        private static PeriodPartitionType[] accesibleTypes =
            {
                PeriodPartitionType.Payment,
                PeriodPartitionType.PaymentCorrection,
                PeriodPartitionType.Refund,
                PeriodPartitionType.PercentageChange,
                PeriodPartitionType.CloseAccount,
                PeriodPartitionType.Restruct,
                PeriodPartitionType.LatePymentRestruct
            };

        public PersonalAccountSummaryService(IWindsorContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Получить суммы по периоду
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult GetPeriodAccountSummary(BaseParams baseParams)
        {
            var accountId = baseParams.Params.GetAs<long>("accountId");
            var periodId = baseParams.Params.GetAs<long>("periodId");

            var period = this.ChargePeriodDomain.GetAll().FirstOrDefault(x => x.Id == periodId);

            if (period == null)
            {
                return new BaseDataResult(false, "Не удалось получить период");
            }

            var chargeSummary = this.AccountChargeDomain.GetAll()
                .Where(x => x.BasePersonalAccount.Id == accountId)
                .Where(x => x.ChargePeriod.Id == period.Id)
                .Select(
                    x => new
                    {
                        x.Penalty,
                        x.Charge,
                        Recalc = x.RecalcByBaseTariff,
                        x.ChargeTariff
                    })
                .ToList();

            var paymentSummary = this.AccountPaymentDomain.GetAll()
                .Where(x => x.BasePersonalAccount.Id == accountId)
                .Where(x => period.StartDate <= x.PaymentDate)
                .WhereIf(period.EndDate.HasValue, x => period.EndDate >= x.PaymentDate)
                .Select(
                    x => new
                    {
                        x.Sum,
                        x.Type
                    })
                .AsEnumerable()
                .GroupBy(x => x.Type)
                .ToDictionary(x => x.Key, y => y.Sum(x => x.Sum));

            var paymentTariff = paymentSummary.Get(PaymentType.Basic);
            var paymentPenalty = paymentSummary.Get(PaymentType.Penalty);
            var chargedTariff = chargeSummary.Sum(x => (decimal?)x.ChargeTariff).GetValueOrDefault();
            var chargedPenalty = chargeSummary.Sum(x => (decimal?)x.Penalty).GetValueOrDefault();
            var recalc = chargeSummary.Sum(x => (decimal?)x.Recalc).GetValueOrDefault();

            return new BaseDataResult(
                new
                {
                    ChargedTariff = chargedTariff,
                    ChargedPenalty = chargedPenalty,
                    PaymentTariff = paymentTariff,
                    PaymentPenalty = paymentPenalty,
                    Recalc = recalc,
                    Summary =
                        chargedTariff
                            + chargedPenalty
                            + recalc
                            - (paymentTariff + paymentPenalty)
                });
        }

        /// <summary>
        /// Получить параметры расчета начислений
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult ListChargeParameterTrace(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var periodId = loadParams.Filter.GetAs<long>("periodId");
            var accountId = loadParams.Filter.GetAs<long>("accountId");

            var account = this.BasePersonalAccountDomain.FirstOrDefault(x => x.Id == accountId);

            var period = this.ChargePeriodDomain.FirstOrDefault(x => x.Id == periodId);

            var result = new List<object>();

            var lastStartDate =
                period.StartDate.Date > account.OpenDate
                    ? period.StartDate.Date
                    : account.OpenDate;

            var periodEndDate = period.GetEndDate();

            var daysInPeriod = (periodEndDate - period.StartDate).Days + 1;

            this.CalcParamTraceDomain.GetAll()
                .Where(x => x.ChargePeriod.Id == period.Id)
                .Where(
                    y => this.AccountChargeDomain.GetAll()
                        .Where(x => x.ChargePeriod.Id == period.Id)
                        .Where(x => x.BasePersonalAccount.Id == accountId)
                        .Any(x => x.Guid == y.CalculationGuid && x.IsActive))
                .Where(x => x.CalculationType == CalculationTraceType.Charge)
                .Select(
                    x => new
                    {
                        x.ParameterValues,
                        x.DateStart,
                        x.DateEnd
                    })
                .AsEnumerable()
                .Select(
                    x => new
                    {
                        x.DateStart,
                        x.DateEnd,
                        Tariff = x.ParameterValues.Get(VersionedParameters.BaseTariff).ToDecimal(),
                        Share = x.ParameterValues.Get(VersionedParameters.AreaShare).ToDecimal(),
                        DateActualShare = x.ParameterValues.Get("DateActualShare").ToDateTime(),
                        RoomArea = x.ParameterValues.Get(VersionedParameters.RoomArea).ToDecimal(),
                        DateActualArea = x.ParameterValues.Get("DateActualArea").ToDateTime(),
                        Charge = x.ParameterValues.Get("charge").ToDecimal(),
                        TariffSource = x.ParameterValues.Get("TariffSource").To<TariffSource>()
                    })
                .GroupBy(x => new { x.DateStart, DateEnd = x.DateEnd ?? DateTime.MinValue })
                .ForEach(
                    x =>
                    {
                        var countDays = (x.Key.DateEnd - x.Key.DateStart).Days + 1;

                        var first = x.First();

                        // это все из-за импорта от ЧЭС
                        decimal summary;
                        if (first.Charge == 0)
                        {
                            summary = x.Sum(y => y.RoomArea * y.Share * y.Tariff) * ((decimal)countDays / daysInPeriod);
                        }
                        else
                        {
                            summary = first.Charge;
                        }

                        result.Add(
                            new
                            {
                                Period = "{0} - {1}".FormatUsing(x.Key.DateStart.ToShortDateString(), x.Key.DateEnd.ToShortDateString()),
                                first.Tariff,
                                first.TariffSource,
                                first.Share,
                                first.DateActualShare,
                                first.RoomArea,
                                first.DateActualArea,
                                Summary = summary.RegopRoundDecimal(2),
                                CountDays = countDays
                            });
                    });

            return new ListDataResult(result, result.Count);
        }

        /// <summary>
        /// Получить параметры расчета пени
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult ListPenaltyParameterTrace(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var periodId = loadParams.Filter.GetAs<long>("periodId");
            var accountId = loadParams.Filter.GetAs<long>("accountId");
            var penaltyConstant = 1M / 300;
            var account = this.BasePersonalAccountDomain.GetAll().FirstOrDefault(x => x.Id == accountId);
            var period = this.ChargePeriodDomain.GetAll().FirstOrDefault(x => x.Id == periodId);
            var decisionType = account.Room.RealityObject.AccountFormationVariant.ToDecisionType();

            var newPenaltyCalcStart =
                this.container.GetGkhConfig<RegOperatorConfig>().PaymentPenaltiesNodeConfig.NewPenaltyCalcConfig.NewPenaltyCalcStart;
            var newPenaltyCalcDays = this.container.GetGkhConfig<RegOperatorConfig>().PaymentPenaltiesNodeConfig.NewPenaltyCalcConfig.NewPenaltyCalcDays;

            var penaltyDays =
                this.PaymentPenaltiesDomain.GetAll()
                    .Where(x => x.DecisionType == decisionType);

            if (account == null || period == null)
            {
                return new ListDataResult();
            }

            var result = new List<object>();

            var data = this.CalcParamTraceDomain.GetAll()
              .Where(x => x.ChargePeriod.Id == period.Id)
                .Where(
                    y => this.AccountChargeDomain.GetAll()
                        .Where(x => x.ChargePeriod.Id == period.Id)
                        .Where(x => x.BasePersonalAccount.Id == accountId)
                        .Any(x => x.Guid == y.CalculationGuid && x.IsActive))
                .Where(x => x.CalculationType == CalculationTraceType.Penalty)
                .Select(
                    x => new
                    {
                        x.ParameterValues,
                        x.DateStart,
                        x.DateEnd
                    })
                .AsEnumerable()
                .Select(
                    x => new
                    {
                        x.DateStart,
                        x.DateEnd,
                        Percentage = x.ParameterValues.Get("payment_penalty_percentage").ToDecimal(),
                        PeriodPartitionType = x.ParameterValues.Get("partititon_type").To<PeriodPartitionType>(),
                        PenaltyDebt = x.ParameterValues.Get("penalty_debt").ToDecimal().RegopRoundDecimal(2),
                        CalcType = x.ParameterValues.Get("numberdays_delay").To<NumberDaysDelay>().GetDisplayName(),
                        CalcDecision = x.ParameterValues.Get("penalty_decision").To<bool>(),
                        Payment = x.ParameterValues.Get("payment").ToDecimal().RegopRoundDecimal(2),
                        PaymentDate = x.ParameterValues.Get("payment_date").ToDateTime(),
                        Partitions = x.ParameterValues.Get("partition_details").To<Partition[]>(),
                        x.ParameterValues
                    })
                .Where(x => x.PenaltyDebt > 0)
                .ToList();

            var dateStart = data.FirstOrDefault()?.DateStart ?? DateTime.MinValue;

            data.GroupBy(x => new { x.DateStart, DateEnd = x.DateEnd ?? DateTime.MinValue })
                .ForEach(
                    x =>
                    {
                        var partitions = x.Where(z => z.Partitions != null).SelectMany(z => z.Partitions).ToArray();
                        var countDays = (x.Key.DateEnd - x.Key.DateStart).Days + 1;
                        var first = x.First();
                        var payments = x.Sum(y => y.Payment);

                        var penaltyDetails = new PenaltyDetailProxy
                        {
                            PartitionType = first.PeriodPartitionType,
                            PaymentDate = first.PaymentDate,
                            Payments = payments,
                            OldDays = dateStart.Day - 1,
                            NewDays = x.Key.DateEnd.Day,
                            Partitions = partitions
                        };

                        var penaltyDay = penaltyDays
                            .Where(y => y.DateStart <= x.Key.DateStart && (y.DateEnd >= x.Key.DateStart || !y.DateEnd.HasValue))
                            .Select(y => y.Days).FirstOrDefault();

                        var penaltyDate = new DateTime(
                            x.Key.DateStart.Year,
                            x.Key.DateStart.Month,
                            Math.Min(penaltyDay, DateTime.DaysInMonth(x.Key.DateStart.Year, x.Key.DateStart.Month)));

                        if (newPenaltyCalcStart?.Date <= first.DateStart)
                        {
                            penaltyDate = penaltyDate.AddMonths(-1).AddDays(newPenaltyCalcDays.Value);
                        }

                        var summary = x.Sum(y => countDays * y.PenaltyDebt) * penaltyConstant * first.Percentage.ToDivisional();

                        result.Add(
                            new
                            {
                                x.Key.DateStart,
                                x.Key.DateEnd,
                                Period = "{0} - {1}".FormatUsing(x.Key.DateStart.ToShortDateString(), x.Key.DateEnd.ToShortDateString()),
                                first.PenaltyDebt,
                                Reason = PersonalAccountSummaryService.GetReason(penaltyDetails),
                                first.Percentage,
                                Summary = summary,
                                CountDays = countDays,
                                first.CalcType,
                                first.CalcDecision,
                                first.Payment,
                                PaymentDate = first.PaymentDate.ToShortDateString(),
                                PenaltyDate = penaltyDate
                            });
                    });

            return new ListDataResult(result, result.Count);
        }

        /// <summary>
        /// Получить параметры перерасчета начислений
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult ListReCalcParameterTrace(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var periodId = loadParams.Filter.GetAs<long>("periodId");
            var accountId = loadParams.Filter.GetAs<long>("accountId");

            var period = this.ChargePeriodDomain.FirstOrDefault(x => x.Id == periodId);

            var result = new List<object>();

            var recalcHistory = this.RecalHistoryDomain.GetAll()
                .Where(x => x.PersonalAccount.Id == accountId)
                .Where(x => x.CalcPeriod.StartDate < period.StartDate)
                .Where(x => x.RecalcType != RecalcType.Penalty)
                .AsEnumerable()
                .Where(
                    x => this.AccountChargeDomain.GetAll().Any(y => x.CalcPeriod.Id == y.ChargePeriod.Id && y.IsFixed))
                .Where(x => x.RecalcSum.RegopRoundDecimal(2) != 0)
                .GroupBy(x => x.RecalcPeriod.Id)
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(
                        z => new
                        {
                            CalcPeriod = z.CalcPeriod.Name,
                            RecalcPeriod = z.RecalcPeriod.Name,
                            z.RecalcSum
                        }).ToArray());

            var accountCharge = this.AccountChargeDomain.GetAll()
               .Where(x => x.BasePersonalAccount.Id == accountId && x.IsActive);

            this.CalcParamTraceDomain.GetAll()
                .Where(x => x.ChargePeriod.Id == period.Id)
                .Where(
                    y => this.AccountChargeDomain.GetAll()
                        .Where(x => x.ChargePeriod.Id == period.Id)
                        .Where(x => x.BasePersonalAccount.Id == accountId)
                        .Any(x => x.Guid == y.CalculationGuid && x.IsActive))
                .Where(x => x.CalculationType == CalculationTraceType.Recalc)
                .Select(
                    x => new
                    {
                        x.ParameterValues,
                        x.DateStart,
                        x.DateEnd
                    })
                .AsEnumerable()
                .Select(
                    x => new
                    {
                        x.DateStart,
                        x.DateEnd,
                        DecisionTariff = x.ParameterValues.Get("Decision_Tariff").ToDecimal(),
                        Tariff = x.ParameterValues.Get(VersionedParameters.BaseTariff).ToDecimal(),
                        Share = x.ParameterValues.Get(VersionedParameters.AreaShare).ToDecimal(),
                        DateActualShare = x.ParameterValues.Get("DateActualShare").ToDateTimeNullable(),
                        RoomArea = x.ParameterValues.Get(VersionedParameters.RoomArea).ToDecimal(),
                        DateActualArea = x.ParameterValues.Get("DateActualArea").ToDateTimeNullable(),
                        Recalc_Charge =
                            (x.ParameterValues.Get("Recalc_ByBase").ToDecimal() + x.ParameterValues.Get("Recalc_Decision").ToDecimal()).RegopRoundDecimal(3),
                        Fact_Charge = x.ParameterValues.Get("Fact_Charge").ToDecimal().RegopRoundDecimal(3),
                        TariffSource = x.ParameterValues.Get("TariffSource").To<TariffSource>(),
                        RecalcReason = x.ParameterValues.Get("RecalcReason").To<RecalcReasonProxy>()
                    })
                .OrderBy(x => x.DateStart)
                .GroupBy(x => new { x.DateStart, DateEnd = x.DateEnd ?? DateTime.MinValue })
                .ForEach(
                    x =>
                    {
                        var countDays = (x.Key.DateEnd - x.Key.DateStart).Days + 1;

                        var first = x.First();

                        var currentPeriod = this.ChargePeriodRepository.GetPeriodByDate(first.DateStart);

                        var countDaysMonth = DateTime.DaysInMonth(first.DateStart.Year, first.DateStart.Month); 

                        var currentCharge = accountCharge
                         .Where(z => z.ChargePeriod.Id == currentPeriod.Id)
                         .Select(z => z.ChargeTariff)
                         .FirstOrDefault();

                        result.Add(
                            new
                            {
                                Period = "{0} - {1}".FormatUsing(x.Key.DateStart.ToShortDateString(), x.Key.DateEnd.ToShortDateString()),
                                Tariff = Math.Max(first.DecisionTariff, first.Tariff),
                                first.TariffSource,
                                first.Share,
                                first.DateActualShare,
                                first.RoomArea,
                                first.DateActualArea,
                                first.Recalc_Charge,
                                first.Fact_Charge,
                                RecalcReason = first.RecalcReason?.Reason.GetDisplayName(),
                                RecalcReasonDate = first.RecalcReason?.Date.ToShortDateString(),
                                RecalcReasonValue = first.RecalcReason?.Value,
                                Summary = first.Recalc_Charge - first.Fact_Charge,
                                CountDays = countDays,
                                CountDaysMonth = countDaysMonth,
                                RecalcHistory = recalcHistory.Get(currentPeriod.Id),
                                CurrentCharge = currentCharge,
                                PeriodName = currentPeriod.Name
                            });
                    });

            return new ListDataResult(result, result.Count);
        }

        /// <summary>
        /// Получить параметры перерасчета пени
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult ListRecalcPenaltyTrace(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var periodId = loadParams.Filter.GetAs<long>("periodId");
            var accountId = loadParams.Filter.GetAs<long>("accountId");

            var period = this.ChargePeriodDomain.FirstOrDefault(x => x.Id == periodId);

            var result = new List<object>();

            var recalcHistory = this.RecalHistoryDomain.GetAll()
                .Where(x => x.PersonalAccount.Id == accountId)
                .Where(x => x.CalcPeriod.StartDate < period.StartDate)
                .Where(x => x.RecalcType == RecalcType.Penalty)
                .AsEnumerable()
                .Where(
                    x => this.AccountChargeDomain.GetAll().Any(y => x.CalcPeriod.Id == y.ChargePeriod.Id && y.Guid == x.UnacceptedChargeGuid && y.IsFixed))
                .Where(x => x.RecalcSum.RegopRoundDecimal(2) != 0)
                .GroupBy(x => x.RecalcPeriod.StartDate)
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(
                        z => new
                        {
                            CalcPeriod = z.CalcPeriod.Name,
                            RecalcPeriod = z.RecalcPeriod.Name,
                            z.RecalcSum
                        }).ToArray());

            var accountCharge = this.AccountChargeDomain.GetAll()
                .Where(x => x.BasePersonalAccount.Id == accountId && x.IsActive);

            this.CalcParamTraceDomain.GetAll()
                .Where(x => x.ChargePeriod.Id == period.Id)
                .Where(
                    y => accountCharge
                        .Where(x => x.ChargePeriod.Id == period.Id)
                        .Any(x => x.Guid == y.CalculationGuid))
                .Where(x => x.CalculationType == CalculationTraceType.PenaltyRecalc || x.CalculationType == CalculationTraceType.DelayPenaltyRecalc)
                .Select(
                    x => new
                    {
                        x.ParameterValues,
                        x.DateStart,
                        x.DateEnd,
                        x.CalculationGuid
                    })
                .AsEnumerable()
                .Select(
                    x => new
                    {
                        x.DateStart,
                        x.DateEnd,
                        x.CalculationGuid,
                        RecalcPenalty = x.ParameterValues.Get("recalc_penalty").ToDecimal(),
                        Penalty = x.ParameterValues.Get("penalty").ToDecimal(),
                        CalcType = x.ParameterValues.Get("numberdays_delay")?.To<NumberDaysDelay>(),
                        CalcDecision = x.ParameterValues.Get("penalty_decision").To<bool>(),
                        RecalcReason = x.ParameterValues.Get("recalc_reason")?.To<RecalcReason>().GetDisplayName(),
                        RecalcDate = x.ParameterValues.Get("recalc_reason_date")?.ToDateTime().ToShortDateString(),
                        IsFixPeriod = x.ParameterValues.Get("is_fix_calc_period").To<bool>(),
                        StartFix = x.ParameterValues.Get("start_fix").To<int?>(),
                        EndFix = x.ParameterValues.Get("end_fix").To<int?>(),
                        CalcPeriodId = x.ParameterValues.Get("calc_period_id").To<long>()
                    })
                .GroupBy(x => new { x.DateStart, DateEnd = x.DateEnd ?? DateTime.MaxValue })
                .ForEach(
                    x =>
                    {
                        var countDays = (x.Key.DateEnd - x.Key.DateStart).Days + 1;

                        var first = x.First();

                        var dateStart = first.CalcType == NumberDaysDelay.StartDateMonth
                            ? x.Key.DateStart
                            : new DateTime(x.Key.DateStart.AddMonths(1).Year, x.Key.DateStart.AddMonths(1).Month, 1);

                        if (first.IsFixPeriod)
                        {
                            dateStart = new DateTime(first.DateEnd.Value.Year, first.DateEnd.Value.Month, 1); 
                        }

                        var curPeriod = first.CalcPeriodId != 0
                            ? this.ChargePeriodRepository.Get(first.CalcPeriodId)
                            : this.ChargePeriodRepository.GetPeriodByDate(dateStart);

                        var currentPenalty = accountCharge
                            .Where(z => z.ChargePeriod.Id == curPeriod.Id)
                            .Select(z => z.Penalty)
                            .FirstOrDefault();

                        if (first.Penalty != 0 || first.RecalcPenalty != 0)
                        {
                            result.Add(
                                new
                                {
                                    x.Key.DateStart,
                                    x.Key.DateEnd,
                                    Period = "{0} - {1}".FormatUsing(
                                        x.Key.DateStart.ToShortDateString(),
                                        x.Key.DateEnd.ToShortDateString()),
                                    CountDays = countDays,
                                    first.Penalty,
                                    first.RecalcPenalty,
                                    CalcType = first.CalcType?.GetDisplayName(),
                                    first.CalcDecision,
                                    first.RecalcReason,
                                    first.RecalcDate,
                                    first.CalculationGuid,
                                    Summary = first.RecalcPenalty - first.Penalty,
                                    RecalcHistory = recalcHistory.Get(dateStart),
                                    CurrentPenalty = currentPenalty,
                                    PeriodName = curPeriod.Name
                                });
                        }
                    });

            return new ListDataResult(result, result.Count);
        }

        /// <summary>
        /// Получить суммы по текущему периоду
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult GetAccountSummaryInfoInCurrentPeriod(BaseParams baseParams)
        {
            var accountId = baseParams.Params.GetAs<long>("accountId");

            var period = this.ChargePeriodRepository.GetCurrentPeriod();
            var account = this.BasePersonalAccountDomain.FirstOrDefault(x => x.Id == accountId);

            if (period == null)
            {
                return new BaseDataResult(false, "Не удалось получить текщий период");
            }

            if (account == null)
            {
                return new BaseDataResult(false, "Не удалось найти лицевой счет");
            }

            var summary = this.AccountPeriodSummaryDomain.GetAll()
                .FirstOrDefault(x => x.Period.Id == period.Id && x.PersonalAccount.Id == account.Id);

            if (summary == null)
            {
                return new BaseDataResult(false, "Не удалось найти информацию о состоянии счета");
            }

            return new BaseDataResult(
                new
                {
                    SaldoIn = summary.SaldoIn.RegopRoundDecimal(2),
                    SaldoOut = summary.SaldoOut.RegopRoundDecimal(2),
                    Penalty = account.Summaries.ToList().SafeSum(x => x.GetPenaltyDebt()).RegopRoundDecimal(2)
                });
        }

        /// <summary>
        /// Получить детализацию параметров перерасчета пени
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult ListRecalcPenaltyTraceDetail(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var guid = loadParams.Filter.GetAs<string>("guid");
            var dateStart = loadParams.Filter.GetAs<DateTime>("dateStart");
            var dateEnd = loadParams.Filter.GetAs<DateTime>("dateEnd");

            var result = new List<object>();

            this.CalcParamTraceDomain.GetAll()
                .Where(x => x.CalculationGuid == guid)
                .Where(x => x.CalculationType == CalculationTraceType.PenaltyRecalcDetail)
                .Where(x => x.DateStart >= dateStart && x.DateStart <= dateEnd)
                .Select(
                    x => new
                    {
                        x.ParameterValues,
                        x.DateStart,
                        x.DateEnd
                    })
                .AsEnumerable()
                .Select(
                    x => new
                    {
                        x.DateStart,
                        x.DateEnd,
                        RecalcPenalty = x.ParameterValues.Get("recalc_penalty").ToDecimal(),
                        Percentage = x.ParameterValues.Get("recalc_percent").ToDecimal(),
                        Payment = x.ParameterValues.Get("payment").ToDecimal().RegopRoundDecimal(2),
                        PaymentDate = x.ParameterValues.Get("payment_date").ToDateTime(),
                        Debt = x.ParameterValues.Get("debt").ToDecimal(),
                        PeriodPartitionType = x.ParameterValues.Get("partititon_type").To<PeriodPartitionType>(),
                        Partitions = x.ParameterValues.Get("partition_details").To<Partition[]>(),
                    })
                .GroupBy(x => new { x.DateStart, DateEnd = x.DateEnd ?? DateTime.MaxValue })
                .OrderBy(x => x.Key.DateStart)
                .ForEach(
                    x =>
                    {
                        var countDays = (x.Key.DateEnd - x.Key.DateStart).Days + 1;

                        var first = x.First();

                        var payments = x.Sum(y => y.Payment);

                        var penaltyDetails = new PenaltyDetailProxy
                        {
                            PartitionType = first.PeriodPartitionType,
                            PaymentDate = first.PaymentDate,
                            Payments = payments,
                            OldDays = dateStart.Day - 1,
                            NewDays = x.Key.DateEnd.Day,
                            Partitions = x.Where(z => z.Partitions != null).SelectMany(z => z.Partitions).ToArray()
                        };

                        result.Add(
                            new
                            {
                                Period = "{0} - {1}".FormatUsing(
                                    x.Key.DateStart.ToShortDateString(),
                                    x.Key.DateEnd.ToShortDateString()),
                                CountDays = countDays,
                                Reason = PersonalAccountSummaryService.GetReason(penaltyDetails),
                                first.Percentage,
                                first.RecalcPenalty,
                                Payment = payments,
                                PaymentDate = first.PaymentDate.ToShortDateString(),
                                first.Debt
                            });
                    });

            return new ListDataResult(result, result.Count);
        }

        private static string GetReason(PenaltyDetailProxy penaltyDetail)
        {
            // если есть детализация
            if (penaltyDetail.Partitions.IsNotNull())
            {
                var paymentPartitions = penaltyDetail.Partitions
                    .Where(x => PersonalAccountSummaryService.accesibleTypes.Contains(x.PartitionType))
                    .ToArray();

                var reason = string.Empty;
                if (paymentPartitions.IsNotEmpty())
                {
                    reason = paymentPartitions
                        .Select(x => PersonalAccountSummaryService.GetReason(x, penaltyDetail.PaymentDate))
                        .Where(x => x != null)
                        .AggregateWithSeparator(", ");
                }

                var daysChanged = penaltyDetail.Partitions.Any(x => x.PartitionType == PeriodPartitionType.DebtDaysChange);

                if (daysChanged)
                {
                    var dayParams = penaltyDetail.Partitions
                        .FirstOrDefault(x => x.PartitionType == PeriodPartitionType.DebtDaysChange && x.PeriodDaysChange != null)?.PeriodDaysChange;

                    var oldDays = dayParams?.OldDays ?? penaltyDetail.OldDays;
                    var newDays = dayParams?.NewDays ?? penaltyDetail.NewDays;

                    reason += (reason.IsNotEmpty() ? "," : string.Empty)
                              + PeriodPartitionType.DebtDaysChange.GetDisplayName().FormatUsing(oldDays, newDays);
                }

                return reason;
            }

            // если есть новые поля
            switch (penaltyDetail.PartitionType)
            {
                case PeriodPartitionType.Payment:
                case PeriodPartitionType.Refund:
                case PeriodPartitionType.PaymentCorrection:
                    if (penaltyDetail.Payments != 0)
                    {
                        return penaltyDetail.PartitionType.GetDisplayName()
                            .FormatUsing(penaltyDetail.PaymentDate.ToShortDateString(), penaltyDetail.Payments.RegopRoundDecimal(2));
                    }

                    break;

                case PeriodPartitionType.PercentageChange:
                    return penaltyDetail.PartitionType.GetDisplayName().FormatUsing(penaltyDetail.PaymentDate.AddDays(1).ToShortDateString());

                case PeriodPartitionType.DebtDaysChange:
                    return penaltyDetail.PartitionType.GetDisplayName().FormatUsing(penaltyDetail.OldDays, penaltyDetail.NewDays);
            }

            // старая реализация
            if (penaltyDetail.Payments != 0)
            {
                return (penaltyDetail.Payments > 0 ? PeriodPartitionType.Payment : PeriodPartitionType.Refund).GetDisplayName()
                            .FormatUsing(penaltyDetail.PaymentDate.ToShortDateString(), penaltyDetail.Payments.RegopRoundDecimal(2));
            }

            return string.Empty;
        }

        private static string GetReason(Partition partition, DateTime paymentDate)
        {
            switch (partition.PartitionType)
            {
                case PeriodPartitionType.Payment:
                case PeriodPartitionType.Refund:
                case PeriodPartitionType.PaymentCorrection:
                    if (partition.Amount != 0)
                    {
                        var restruct = partition.RestructSum != 0 ? $" ({partition.RestructSum} рублей в счет погашения договора реструктуризации)" : string.Empty;
                        return partition.PartitionType.GetDisplayName().FormatUsing(paymentDate.ToShortDateString(), partition.Amount.RegopRoundDecimal(2), restruct);
                    }

                    break;

                case PeriodPartitionType.PercentageChange:
                case PeriodPartitionType.CloseAccount:
                    return partition.PartitionType.GetDisplayName().FormatUsing(paymentDate.AddDays(1).ToShortDateString());
                case PeriodPartitionType.Restruct:
                case PeriodPartitionType.LatePymentRestruct:
                    return partition.PartitionType.GetDisplayName();
            }

            return null;
        }

        #region Fields
        private readonly IWindsorContainer container;

        private IDomainService<PersonalAccountPeriodSummary> accountPeriodSummaryDomain;

        private IDomainService<PersonalAccountCharge> accountChargeDomain;

        private IDomainService<PersonalAccountPayment> accountPaymentDomain;

        private IDomainService<UnacceptedCharge> unacceptedChargeDomain;

        private IDomainService<BasePersonalAccount> basePersonalAccountDomain;

        private IDomainService<ChargePeriod> chargePeriodDomain;

        private IDomainService<CalculationParameterTrace> calcParamTraceDomain;

        private IDomainService<RecalcHistory> recalcHistoryDomain;

        private IDomainService<PaymentPenalties> paymentPenaltiesDomain;

        private IChargePeriodRepository chargePeriodRepository;
        #endregion Fields

        #region Properties
        protected IDomainService<PersonalAccountPeriodSummary> AccountPeriodSummaryDomain
        {
            get
            {
                return this.accountPeriodSummaryDomain
                    ?? (this.accountPeriodSummaryDomain = this.container.ResolveDomain<PersonalAccountPeriodSummary>());
            }
        }

        protected IDomainService<PersonalAccountCharge> AccountChargeDomain
        {
            get
            {
                return this.accountChargeDomain
                    ?? (this.accountChargeDomain = this.container.ResolveDomain<PersonalAccountCharge>());
            }
        }

        protected IChargePeriodRepository ChargePeriodRepository
        {
            get
            {
                return this.chargePeriodRepository
                    ?? (this.chargePeriodRepository = this.container.Resolve<IChargePeriodRepository>());
            }
        }

        protected IDomainService<PersonalAccountPayment> AccountPaymentDomain
        {
            get
            {
                return this.accountPaymentDomain
                    ?? (this.accountPaymentDomain = this.container.ResolveDomain<PersonalAccountPayment>());
            }
        }

        protected IDomainService<UnacceptedCharge> UnacceptedChargeDomain
        {
            get
            {
                return this.unacceptedChargeDomain
                    ?? (this.unacceptedChargeDomain = this.container.ResolveDomain<UnacceptedCharge>());
            }
        }

        protected IDomainService<BasePersonalAccount> BasePersonalAccountDomain
        {
            get
            {
                return this.basePersonalAccountDomain
                    ?? (this.basePersonalAccountDomain = this.container.ResolveDomain<BasePersonalAccount>());
            }
        }

        protected IDomainService<ChargePeriod> ChargePeriodDomain
        {
            get
            {
                return this.chargePeriodDomain
                    ?? (this.chargePeriodDomain = this.container.ResolveDomain<ChargePeriod>());
            }
        }

        protected IDomainService<CalculationParameterTrace> CalcParamTraceDomain
        {
            get
            {
                return this.calcParamTraceDomain
                    ?? (this.calcParamTraceDomain = this.container.ResolveDomain<CalculationParameterTrace>());
            }
        }

        protected IDomainService<RecalcHistory> RecalHistoryDomain
        {
            get
            {
                return this.recalcHistoryDomain
                    ?? (this.recalcHistoryDomain = this.container.ResolveDomain<RecalcHistory>());
            }
        }

        protected IDomainService<PaymentPenalties> PaymentPenaltiesDomain
        {
            get
            {
                return this.paymentPenaltiesDomain
                    ?? (this.paymentPenaltiesDomain = this.container.ResolveDomain<PaymentPenalties>());
            }
        }
        #endregion

        private class PenaltyDetailProxy
        {
            public PeriodPartitionType PartitionType { get; set; }

            public DateTime PaymentDate { get; set; }

            public decimal Payments { get; set; }

            public int OldDays { get; set; }

            public int NewDays { get; set; }

            public Partition[] Partitions { get; set; }
        }
    }
}