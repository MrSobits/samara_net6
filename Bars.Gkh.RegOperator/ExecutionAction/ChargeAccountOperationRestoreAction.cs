namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Entities;

    using NHibernate.Linq;

    public class ChargeAccountOperationRestoreAction : BaseExecutionAction
    {
        public IDomainService<RealityObjectChargeAccount> RoChargeAccDomain { get; set; }

        public IDomainService<RealityObjectChargeAccountOperation> RoChargeOperationDomain { get; set; }

        public IDomainService<PersonalAccountPeriodSummary> PeriodSummaryDomain { get; set; }

        public IDomainService<RealityObject> RoDomain { get; set; }

        public IDomainService<ChargePeriod> ChargePeriodDomain { get; set; }

        public ISessionProvider SessionProvider { get; set; }

        public override string Description => @"Восстановление операций по счету начислений дома";

        public override string Name => "Восстановление операций по счету начислений дома";

        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var chargeAccountByRoId = this.RoChargeAccDomain.GetAll()
                .Select(
                    x => new
                    {
                        RoId = x.RealityObject.Id,
                        Acc = x
                    })
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, y => y.Select(x => x.Acc).First());

            var chargeOpers = this.PeriodSummaryDomain.GetAll()
                .Fetch(x => x.Period)
                .Fetch(x => x.PersonalAccount)
                .ThenFetch(x => x.Room)
                .ThenFetch(x => x.RealityObject)
                .Select(
                    x => new
                    {
                        RoId = x.PersonalAccount.Room.RealityObject.Id,
                        x.Period,
                        x.SaldoIn,
                        ChargedTotal = x.ChargeTariff + x.RecalcByBaseTariff + x.RecalcByDecisionTariff + x.BaseTariffChange + x.DecisionTariffChange + x.Penalty + x.PenaltyChange,
                        ChargedPenalty = x.Penalty + x.PenaltyChange,
                        PaidTotal = x.TariffPayment + x.TariffDecisionPayment,
                        PaidPenalty = x.PenaltyPayment,
                        x.SaldoOut
                    })
                .ToList()
                .GroupBy(x => x.RoId)
                .ToDictionary(
                    x => x.Key,
                    y => y.GroupBy(x => x.Period)
                        .Select(
                            x => new RealityObjectChargeAccountOperation
                            {
                                Account = chargeAccountByRoId.Get(x.Select(z => z.RoId).First()),
                                Period = x.Key,
                                SaldoIn = x.Sum(z => z.SaldoIn),
                                ChargedPenalty = x.Sum(z => z.ChargedPenalty),
                                ChargedTotal = x.Sum(z => z.ChargedTotal),
                                PaidTotal = x.Sum(z => z.PaidTotal),
                                PaidPenalty = x.Sum(z => z.PaidPenalty),
                                SaldoOut = x.Sum(z => z.SaldoOut)
                            })).SelectMany(x => x.Value)
                .ToList();

            var accounts = chargeOpers
                .GroupBy(x => x.Account)
                .Select(
                    x =>
                    {
                        x.Key.PaidTotal = x.Sum(y => y.PaidTotal + y.PaidPenalty);
                        return x.Key;
                    })
                .ToList();

            var session = this.SessionProvider.GetCurrentSession();
            session.CreateSQLQuery("delete from REGOP_RO_CHARGE_ACC_CHARGE").ExecuteUpdate();

            TransactionHelper.InsertInManyTransactions(this.Container, chargeOpers, 10000, true, true);
            TransactionHelper.InsertInManyTransactions(this.Container, accounts, 10000, true, true);

            return new BaseDataResult
            {
                Success = true
            };
        }
    }
}