namespace Bars.Gkh.RegOperator.DomainModelServices.Impl.ChargeCalculators
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using B4.Utils;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;

    using Decisions.Nso.Entities;
    using Domain;
    using Domain.Interfaces;
    using Domain.ParametersVersioning;
    using Domain.ProxyEntity;
    using Entities;
    using Entities.PersonalAccount;
    using Enums;

    public class ChargeCalculator
    {
        private readonly ICalculationGlobalCache cache;
        private readonly IParameterTracker tracker;

        public ChargeCalculator(ICalculationGlobalCache cache, IParameterTracker tracker)
        {
            this.cache = cache;
            this.tracker = tracker;
        }

        public CalculationResult<TariffCharge> Calculate(IPeriod period, BasePersonalAccount account, UnacceptedCharge unaccepted)
        {
            var traces = new List<CalculationParameterTrace>();
            TariffSource tariffSource;
            
            var end = period.GetEndDate();

            if (account.OpenDate > end)
            {
                return new CalculationResult<TariffCharge>();
            }

            var tariffByDate = new Dictionary<DateTime, TariffWithOverplus>();

            var tariffParam = this.tracker.GetParameter(VersionedParameters.BaseTariff, account, period);
            var shareParam = this.tracker.GetParameter(VersionedParameters.AreaShare, account, period);
            var areaParam = this.tracker.GetParameter(VersionedParameters.RoomArea, account, period);

            var begin =
                period.StartDate >= account.OpenDate
                    ? period.StartDate
                    : account.OpenDate;

            decimal tariff;

            var decisionTarif = this.cache.GetDecisionTarif(account.Room.RealityObject, begin);

            if (decisionTarif != null
                && decisionTarif.Decision.Value > tariffParam.GetActualByDate<decimal>(account, begin, true).Value)
            {
                tariff = decisionTarif.Decision.Value;
                tariffSource = new TariffSource
                {
                    TariffSourceType = TariffSourceType.PaySizeByProtocol,
                    ProtocolNum = decisionTarif.Protocol.DocumentNum,
                    ProtocolDate = decisionTarif.Protocol.ProtocolDate
                };
            }
            else
            {
                tariff = tariffParam.GetActualByDate<decimal>(account, begin, true).Value;

                tariffSource = (TariffSource)tariffParam.GetActualByDate<decimal>(account, begin, true).Key;
            }

            var area = areaParam.GetActualByDate<decimal>(account, begin, true);
            var share = shareParam.GetActualByDate<decimal>(account, begin, true);

            var currentParams = new CalculationTraceProxy
            {
                Area = area.Value,
                DateActualArea = ((DateTime?)area.Key).GetValueOrDefault(),
                Share = shareParam.GetActualByDate<decimal>(account, begin, true).Value,
                DateActualShare = ((DateTime?)share.Key).GetValueOrDefault(),
                Tariff = tariff,
                TariffSource = tariffSource,
                DateStart = begin,
                DateEnd = end
            };

            var tempTrace = new List<CalculationTraceProxy> { currentParams };

            for (var currentDate = begin; currentDate <= end; currentDate = currentDate.AddDays(1))
            {
                var currentTariff = tariffParam.GetActualByDate<decimal>(account, currentDate, true).Value;
                var baseTariff = currentTariff;
                var currentShare = shareParam.GetActualByDate<decimal>(account, currentDate, true);
                var currentArea = areaParam.GetActualByDate<decimal>(account, currentDate, true);


                var date = currentDate;
                var decision = this.cache.GetDecisionTarif(account.Room.RealityObject, date);

                if (decision != null && decision.Decision.Value > currentTariff)
                {
                    currentTariff = decision.Decision.Value;
                    tariffSource = new TariffSource
                    {
                        TariffSourceType = TariffSourceType.PaySizeByProtocol,
                        ProtocolNum = decision.Protocol.DocumentNum,
                        ProtocolDate = decision.Protocol.ProtocolDate
                    };
                }
                else
                {
                    tariffSource = (TariffSource) tariffParam.GetActualByDate<decimal>(account, currentDate, true).Key;
                }


                decimal substract = currentTariff - baseTariff;

                //если изменился один из параметров, 
                //то закрываем текущий "период параметров" и создаем новый, который добавляем в темповый список
                if (currentTariff != currentParams.Tariff
                    || currentShare.Value != currentParams.Share
                    || currentArea.Value != currentParams.Area)
                {
                    currentParams.DateEnd = currentDate.AddDays(-1);

                    currentParams = new CalculationTraceProxy
                    {
                        Area = currentArea.Value,
                        DateActualArea = ((DateTime?)currentArea.Key).GetValueOrDefault(),
                        Share = currentShare.Value,
                        DateActualShare = ((DateTime?)currentShare.Key).GetValueOrDefault(),
                        Tariff = currentTariff,
                        TariffSource = tariffSource,
                        DateStart = currentDate,
                        DateEnd = end
                    };

                    tempTrace.Add(currentParams);
                }

                var tariffShare = currentTariff / DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
                var substractShare = substract / DateTime.DaysInMonth(currentDate.Year, currentDate.Month);

                var todayCalc = currentShare.Value * currentArea.Value * tariffShare;

                var tarifOver = new TariffWithOverplus
                {
                    Tariff = currentTariff,
                    DayCalc = todayCalc,
                    OverPlus = currentShare.Value * currentArea.Value * substractShare
                };

                tariffByDate.Add(currentDate, tarifOver);
            }
            

            //формируем из темпового списка "периодов параметров"
            //сохраняемые параметры
            foreach (var t in tempTrace)
            {
                var trace = new CalculationParameterTrace
                {
                    ParameterValues = new Dictionary<string, object>
                    {
                        {tariffParam.ParameterName, t.Tariff.ToString(CultureInfo.InvariantCulture)},
                        {shareParam.ParameterName, t.Share.ToString(CultureInfo.InvariantCulture)},
                        {"DateActualShare", t.DateActualShare },
                        {areaParam.ParameterName, t.Area.ToString(CultureInfo.InvariantCulture)},
                        {"DateActualArea", t.DateActualArea },
                        {"TariffSource", t.TariffSource }
                    },
                    DateStart = t.DateStart,
                    DateEnd = t.DateEnd,
                    CalculationType = CalculationTraceType.Charge,
                    CalculationGuid = unaccepted.Guid,
                    ChargePeriod = (ChargePeriod)period
                };
                traces.Add(trace);
            }
            
            decimal a = 0.00001M;

            var tc = new TariffCharge(
                tariffByDate.Values.Sum(x => x.DayCalc) + a,
                tariffByDate.Values.Sum(x => x.OverPlus));

            var result = new CalculationResult<TariffCharge>(tc);

            result.Traces.AddRange(traces);

            return result;
        }

        public decimal Calculate(IPeriod period, BasePersonalAccount account, DateTime dateStart, DateTime dateEnd)
        {
            var startDate = account.OpenDate > dateStart ? account.OpenDate : dateStart;

            if (startDate >= dateEnd)
            {
                return 0m;
            }

            var tariffByDate = new Dictionary<DateTime, TariffWithOverplus>();

            var tariffParam = this.tracker.GetParameter(VersionedParameters.BaseTariff, account, period);
            var shareParam = this.tracker.GetParameter(VersionedParameters.AreaShare, account, period);
            var areaParam = this.tracker.GetParameter(VersionedParameters.RoomArea, account, period);

            var decisions = this.cache.GetFees(account.Room.RealityObject) ?? new List<MonthlyFeeAmountDecision>();

            for (var date = startDate; date <= dateEnd; date = date.AddDays(1))
            {
                var currentTariff = tariffParam.GetActualByDate<decimal>(account, date, true).Value;
                var baseTariff = currentTariff;
                var currentShare = shareParam.GetActualByDate<decimal>(account, date, true).Value;
                var currentArea = areaParam.GetActualByDate<decimal>(account, date, true).Value;

                if (decisions.IsNotEmpty())
                {
                    var decision = this.cache.GetDecisionTarif(account.Room.RealityObject, date);

                    if (decision != null && decision.Decision.Value > currentTariff)
                    {
                        currentTariff = decision.Decision.Value;
                    }
                }

                decimal substract = currentTariff - baseTariff;

                var tariffShare = currentTariff / DateTime.DaysInMonth(date.Year, date.Month);
                var substractShare = substract / DateTime.DaysInMonth(date.Year, date.Month);

                var todayCalc = currentShare * currentArea * tariffShare;

                var tarifOver = new TariffWithOverplus
                {
                    Tariff = currentTariff,
                    DayCalc = todayCalc,
                    OverPlus = currentShare * currentArea * substractShare
                };

                tariffByDate.Add(date, tarifOver);
            }

            return tariffByDate.Values.Sum(x => x.DayCalc).RegopRoundDecimal(2);
        }
    }
}