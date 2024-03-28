namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Entities;

    using Castle.Windsor;

    public class SaldoRoChargeAccountRecalcAction : BaseExecutionAction
    {
        public static string Code = "SaldoRoChargeAccountRecalcAction";
        private readonly IWindsorContainer _container;

        public SaldoRoChargeAccountRecalcAction(IWindsorContainer container)
        {
            this._container = container;
        }

        /// <summary>
        /// Код для регистрации
        /// </summary>
        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description => "Пересчет входящего и исходящего сальдо в счетах домов. " +
            "Начиная с первого периода и далее идет присваивание исходящего сальдо во входящее след. периода и т.д.";

        /// <summary>
        /// Название для отображения
        /// </summary>
        public override string Name => "Пересчет входящего и исходящего сальдо в счетах домов";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => this.DoAction;

        private BaseDataResult DoAction()
        {
            var opRepo = this._container.ResolveRepository<RealityObjectChargeAccountOperation>();
            var sessions = this._container.Resolve<ISessionProvider>();

            var result = new BaseDataResult(true);

            try
            {
                using (this._container.Using(opRepo, sessions))
                {
                    var data = opRepo.GetAll()
                        .Select(
                            x => new
                            {
                                x.Id,
                                PeriodStart = x.Period.StartDate,
                                AccId = x.Account.Id,
                                x.SaldoIn,
                                Charge = x.ChargedTotal,
                                Paid = x.PaidTotal + x.PaidPenalty
                            })
                        .ToList()
                        .GroupBy(x => x.AccId);

                    using (var session = sessions.OpenStatelessSession())
                    {
                        using (var tr = session.BeginTransaction())
                        {
                            foreach (var item in data)
                            {
                                decimal lastSaldo = 0;
                                foreach (var subItem in item.OrderBy(x => x.PeriodStart))
                                {
                                    session.CreateQuery(
                                        "update RealityObjectChargeAccountOperation set SaldoIn=:saldoIn, SaldoOut=:saldoOut"
                                            + " where Id=:id")
                                        .SetInt64("id", subItem.Id)
                                        .SetDecimal("saldoIn", lastSaldo)
                                        .SetDecimal("saldoOut", lastSaldo + subItem.Charge - subItem.Paid)
                                        .ExecuteUpdate();

                                    lastSaldo = lastSaldo + subItem.Charge - subItem.Paid;
                                }
                            }

                            tr.Commit();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Message = e.Message;
            }

            return result;
        }
    }
}