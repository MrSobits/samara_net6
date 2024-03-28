namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.ExecutionAction;

    /// <summary>
    /// Действие по заполнению причины несоответствия ЛС для существующих записей реестра неопределенных оплат
    /// </summary>
    public class SetImportedPaymentPaNotDeterminationStateReasonAction : BaseExecutionAction
    {
        /// <summary>
        /// Описание
        /// </summary>
        public override string Description => "Заполнение причины несоответствия ЛС для существующих записей реестра неопределенных оплат";

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => "Заполнение причины несоответствия ЛС для существующих записей реестра неопределенных оплат";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var sessionProvider = this.Container.Resolve<ISessionProvider>();
            try
            {
                var session = sessionProvider.GetCurrentSession();

                session.CreateSQLQuery(@"
                        update REGOP_IMPORTED_PAYMENT 
                        set PAND_STATE_REASON = 10 
                        where PAD_STATE = 10;")
                    .ExecuteUpdate();
            }
            finally
            {
                this.Container.Release(sessionProvider);
            }

            return new BaseDataResult();
        }
    }
}