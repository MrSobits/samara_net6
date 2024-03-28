namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Microsoft.Extensions.Logging;
    using Bars.Gkh.Domain;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Entities;
    using NHibernate.Linq;

    /// <summary>
    /// Пересчитать столбцы Итого в счете начислений по периодам
    /// </summary>
    public class RecalculateTotalRoChargeAccount : BaseExecutionAction
    {


        /// <summary>
        /// The list of charge account for save.
        /// </summary>
        private readonly List<RealityObjectChargeAccountOperation> chargeAccountForSave = new List<RealityObjectChargeAccountOperation>();

        /// <summary>
        /// Gets the description.
        /// </summary>
        public override string Description
            =>
                "Пересчитать столбцы Итого начислено и Итого оплачено в счете начислений по периодам.\nДействие суммирует значения по столбцам детализации и обновляет значения.\nДлительность зависит от количества обновляемых данных. Может выполнятся достаточно долго."
            ;

        /// <summary>
        /// Gets the name.
        /// </summary>
        public override string Name => "Пересчитать столбцы Итого в счете начислений по периодам";

        /// <summary>
        /// Gets the action.
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;


        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public ILogger Logger { get; set; }



        /// <summary>
        /// The execute.
        /// </summary>
        /// <returns>
        /// The <see cref="BaseDataResult" />.
        /// </returns>
        private BaseDataResult Execute()
        {
            var roChargeOperDomain = this.Container.ResolveDomain<RealityObjectChargeAccountOperation>();
            var rentPaymentDomain = this.Container.ResolveDomain<RentPaymentIn>();
            var accumFundsDomain = this.Container.ResolveDomain<AccumulatedFunds>();
            var persAccPerDomain = this.Container.ResolveDomain<PersonalAccountPeriodSummary>();

            var chargeAccOperQuery = roChargeOperDomain.GetAll()
                .Fetch(x => x.Period)
                .Fetch(x => x.Account)
                .ThenFetch(x => x.RealityObject)
                .ToList()
                .GroupBy(x => x.Period.Id);

            var rentPaymentQuery = rentPaymentDomain.GetAll().GroupBy(x => x.Account.Id).ToDictionary(x => x.Key);
            var accumFundsQuery = accumFundsDomain.GetAll().GroupBy(x => x.Account.Id).ToDictionary(x => x.Key);

            foreach (var chargeAccGroupByPeriod in chargeAccOperQuery)
            {
                var persAccPerQuery = persAccPerDomain.GetAll()
                    .Where(x => x.Period.Id == chargeAccGroupByPeriod.Key)
                    .GroupBy(x => x.PersonalAccount.Room.RealityObject.Id)
                    .Select(
                        x => new
                        {
                            x.Key,
                            PaidSum = x.Sum(y => y.TariffPayment + y.TariffDecisionPayment + y.PenaltyPayment),
                            PenaltyPayment = x.Sum(y => y.PenaltyPayment),
                            ChargeSum = x.Sum(
                                y =>
                                    y.ChargeTariff + y.RecalcByBaseTariff + y.RecalcByDecisionTariff +
                                        y.BaseTariffChange + y.DecisionTariffChange + y.PenaltyChange +
                                        y.Penalty + y.RecalcByPenalty) // TODO fix recalc
                        })
                    .ToDictionary(x => x.Key);

                var chargeAccGroupByRoId = chargeAccGroupByPeriod.ToDictionary(x => x.Account.RealityObject.Id);

                foreach (var chargeAcc in chargeAccGroupByRoId.Values)
                {
                    var paidTotal = decimal.Zero;
                    var chargeTotal = decimal.Zero;
                    var paidPenalty = decimal.Zero;

                    if (persAccPerQuery.ContainsKey(chargeAcc.Account.RealityObject.Id))
                    {
                        paidTotal += persAccPerQuery[chargeAcc.Account.RealityObject.Id].PaidSum;
                        chargeTotal += persAccPerQuery[chargeAcc.Account.RealityObject.Id].ChargeSum;
                        paidPenalty += persAccPerQuery[chargeAcc.Account.RealityObject.Id].PenaltyPayment;
                    }

                    if (rentPaymentQuery.ContainsKey(chargeAcc.Account.Id))
                    {
                        paidTotal += rentPaymentQuery[chargeAcc.Account.Id]
                            .Where(x => x.OperationDate >= chargeAcc.Period.StartDate)
                            .Where(x => !chargeAcc.Period.EndDate.HasValue || x.OperationDate <= chargeAcc.Period.EndDate)
                            .Sum(x => x.Sum);
                    }

                    if (accumFundsQuery.ContainsKey(chargeAcc.Account.Id))
                    {
                        paidTotal += accumFundsQuery[chargeAcc.Account.Id]
                            .Where(x => x.ObjectCreateDate >= chargeAcc.Period.StartDate)
                            .Where(x => x.ObjectCreateDate <= chargeAcc.Period.EndDate)
                            .Sum(x => x.Sum);
                    }

                    // Будем обновлять записи только если данные изменились
                    var updateAcc = false;

                    if (chargeAcc.PaidTotal != paidTotal)
                    {
                        chargeAcc.PaidTotal = paidTotal;
                        updateAcc = true;
                    }

                    if (chargeAcc.ChargedTotal != chargeTotal)
                    {
                        chargeAcc.ChargedTotal = chargeTotal;
                        updateAcc = true;
                    }

                    if (chargeAcc.PaidPenalty != paidPenalty)
                    {
                        chargeAcc.PaidPenalty = paidPenalty;
                        updateAcc = true;
                    }

                    if (updateAcc)
                    {
                        this.chargeAccountForSave.Add(chargeAcc);
                    }
                }
            }

            TransactionHelper.InsertInManyTransactions(this.Container, this.chargeAccountForSave);

            this.Logger.LogInformation(
                string.Format(
                    "Действие: {0}. Изменено {1} записей типа RealityObjectChargeAccountOperation.",
                    this.Code,
                    this.chargeAccountForSave.Count));
            return new BaseDataResult(true, string.Format("Изменено {0} записей", this.chargeAccountForSave.Count));
        }
    }
}