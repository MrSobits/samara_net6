namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Dapper;

    /// <summary>
    /// Удаление незафиксированных charge в закрытых периодах
    /// </summary>
    public class DeleteNotFixedChargesAction : BaseExecutionAction
    {
        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description => "Удаление незафиксированных charge в закрытых периодах";

        /// <summary>
        /// Название для отображения
        /// </summary>
        public override string Name => "РегОператор - Удаление незафиксированных charge в закрытых периодах";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {

            var perioIds = this.Container.Resolve<IChargePeriodRepository>().GetAllClosedPeriods().OrderBy(x => x.Id).Select(x => x.Id).ToArray();

            this.Container.InStatelessConnectionTransaction((connection, transaction) =>
            {
                foreach (var perioId in perioIds)
                {
                    connection.Execute(
                        $"delete from regop_pers_acc_charge_period_{perioId} where not is_fixed", 
                        transaction: transaction, 
                        commandTimeout: 360000);
                    }
            });

            return new BaseDataResult();
        }
    }
}