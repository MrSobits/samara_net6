namespace Bars.Gkh.RegOperator.DomainModelServices.Impl.ChargeCalculators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.DomainModelServices.Impl.ChargeCalculators.PenaltyCalculators;
    using Bars.Gkh.RegOperator.DomainModelServices.Impl.ChargeCalculators.PenaltyCalculators.Impl;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Castle.Windsor;
    using Dapper;
    using Domain.Interfaces;
    using Domain.ParametersVersioning;
    using Entities;
    using Entities.PersonalAccount;
    using Gkh.Entities;

    public partial class PersonalAccountChargeCalculator : IPersonalAccountChargeCaculator
    {
        private readonly IParameterTracker tracker;
        private readonly ICalculationGlobalCache cache;
        private readonly IChargePeriodRepository periodRepo;

        public IDomainService<Period> PeriodDomain { get; set; }
        public IDomainService<PersonalAccountPeriodSummary> SummaryDomain { get; set; }

        private TariffCharge charge;
        private TariffRecalc recalc;
        private PenaltyResult penalty;

        private readonly List<CalculationParameterTrace> traces;
        private readonly List<RecalcHistory> recalcHistory;
        private readonly List<PersonalAccountCalcParam_tmp> _params;
        private IPenaltyCalculator penaltyCalculator;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="tracker"></param>
        /// <param name="cache"></param>
        /// <param name="periodRepo"></param>
        public PersonalAccountChargeCalculator(IParameterTracker tracker, ICalculationGlobalCache cache, IChargePeriodRepository periodRepo)
        {
            this.tracker = tracker;
            this.cache = cache;
            this.periodRepo = periodRepo;

            this.traces = new List<CalculationParameterTrace>();
            this.recalcHistory = new List<RecalcHistory>();
            this._params = new List<PersonalAccountCalcParam_tmp>();
        }

        /// <summary>
        /// Расчитать
        /// </summary>
        public ChargeResult Calculate(IPeriod period, BasePersonalAccount account, UnacceptedCharge unaccepted)
        {
            CollectParameters(period, account);

            CalcCharge(period, account, unaccepted);

            RecalculateCharge(period, account, unaccepted);

            CalcPenalty(period, account, this.recalcHistory);

            var result = new ChargeResult(this.charge, this.recalc, this.penalty);
            result.Traces.AddRange(this.traces);
            result.Params.AddRange(this._params);
            result.RecalcHistory.AddRange(this.recalcHistory);

            result.Traces.ForEach(x => x.CalculationGuid = unaccepted.Guid);
            result.RecalcHistory.ForEach(x => x.UnacceptedChargeGuid = unaccepted.Guid);

            return result;
        }

        private void CalcCharge(IPeriod period, BasePersonalAccount account, UnacceptedCharge unaccepted)
        {
            var chargeCalc = new ChargeCalculator(this.cache, this.tracker).Calculate(period, account, unaccepted);
            this.charge = chargeCalc.Result;
            this.traces.AddRange(chargeCalc.Traces);
            this.recalcHistory.AddRange(chargeCalc.RecalcHistory);
        }

        private void CalcPenalty(IPeriod period, BasePersonalAccount account, List<RecalcHistory> recalcHistory)
        {
            var container = ApplicationContext.Current.Container;
            bool calcPeny = true;

            try
            {
                calcPeny = this.cache.CalcPenByAccId.ContainsKey(account.Id) ? this.cache.CalcPenByAccId[account.Id] : true;
                var manuallyRecalcDate = this.cache.GetManuallyRecalcDate(account);
                if (manuallyRecalcDate.HasValue && manuallyRecalcDate.Value != DateTime.MinValue)
                {
                    calcPeny = true;
                }
                if (account.IsNotDebtor)
                {
                    calcPeny = false;
                }
                //var sessionProvider = container.Resolve<ISessionProvider>();
                //using (container.Using(sessionProvider))
                //{
                //    var session = sessionProvider.GetCurrentSession();
                //    var conn = sessionProvider.OpenStatelessSession().Connection;
                //    string prevPeriodQuery = $@"select r.* from (select id from regop_period where id < {period.Id}) r";
                //    var requestAnswer = session.CreateSQLQuery(prevPeriodQuery);
                //    var prevPeriod = conn.Query<Period>(prevPeriodQuery)
                //        .ToList()
                //        .Max(x => x.Id);

                //    string summaryQuery = $@"select sum(saldo_in) saldo_in from regop_pers_acc_period_summ where account_id = {account.Id} and period_id in ({period.Id},{prevPeriod})";
                //    var requestSummaryAnswer = session.CreateSQLQuery(summaryQuery);

                //    var lastPeriodsSaldoIn = conn.Query<double>(summaryQuery)
                //        .FirstOrDefault();

                //    if (lastPeriodsSaldoIn <= 0)
                //    {
                //        calcPeny = false;
                //    }
                //}
            }
            catch (Exception e)
            { 

            }
            
            if (this.cache.CalculatePenalty && calcPeny)
            {
                if (this.cache.CalcPenaltyOneTimeMunicipalProperty && (account.Room.OwnershipType == RoomOwnershipType.Municipal || account.Room.OwnershipType == RoomOwnershipType.Federal || account.Room.OwnershipType == RoomOwnershipType.Goverenment || account.Room.OwnershipType == RoomOwnershipType.MunicipalAdm || account.Room.OwnershipType == RoomOwnershipType.Regional))
                {
                    if (this.cache.CalcPenaltyOneTimeMunicipalProperty && period.Name.Contains("Февраль"))
                    {
                        var penaltyCalc = this.GetPenaltyCalculator().Init(account, period, recalcHistory).Calculate();
                        this.penalty = penaltyCalc.Result;

                        this.traces.AddRange(penaltyCalc.Traces);
                        this.recalcHistory.AddRange(penaltyCalc.RecalcHistory);
                    }
                }
                else
                {
                    var penaltyCalc = this.GetPenaltyCalculator().Init(account, period, recalcHistory).Calculate();
                    this.penalty = penaltyCalc.Result;

                    this.traces.AddRange(penaltyCalc.Traces);
                    this.recalcHistory.AddRange(penaltyCalc.RecalcHistory);
                }
            }
        }

        private void CollectParameters(IPeriod period, BasePersonalAccount account)
        {
            var allChanges = this.tracker.GetChanges(account, period.StartDate).ToList();
            allChanges.ForEach(x => this._params.Add(new PersonalAccountCalcParam_tmp
            {
                PersonalAccount = account,
                LoggedEntity = new EntityLogLight { Id = x.LogId }
            }));
        }

        private IPenaltyCalculator GetPenaltyCalculator()
        {
            if (this.penaltyCalculator != null)
            {
                return this.penaltyCalculator;
            }

            switch (this.cache.NumberDaysDelay)
            {
                case NumberDaysDelay.StartDateMonth:
                    this.penaltyCalculator = new PenaltyCalculatorClassic(this.cache, this.tracker);
                    break;

                case NumberDaysDelay.StartDateDebt:
                    this.penaltyCalculator = new PenaltyCalculatorDebt(this.cache, this.tracker);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return this.penaltyCalculator;
        }
    }
}