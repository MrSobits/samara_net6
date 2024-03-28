namespace Bars.Gkh.ClaimWork.ExecutionAction
{
    using System;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.ExecutionAction;

    /// <summary>
    /// Действие по пересчитыванию формулы Расчета пени для существующих записей ПИР
    /// </summary>
    public class SetPenaltyFormulaAction : BaseExecutionAction
    {
        /// <inheritdoc />
        public override string Name => "Пересчитать формулу Расчета пени для существующих записей ПИР";

        /// <inheritdoc />
        public override string Description => @"Пересчитать формулу Расчета пени для существующих записей ПИР";

        /// <inheritdoc />
        public override Func<IDataResult> Action => this.Execute;

        /// <summary>
        /// Провайдер сессии NH
        /// </summary>
        public ISessionProvider SessionProvider { get; set; }

        private BaseDataResult Execute()
        {
            try
            {
                using (var session = this.SessionProvider.OpenStatelessSession())
                {
                    var sql = @"
UPDATE CLW_DOCUMENT_ACC_DETAIL
SET PENALTY_CALC_FORMULA = GET_PENALTY_FORMULA(ACCOUNT_ID, CAST(:periodDate as date), 2)
WHERE PENALTY_CALC_FORMULA is null;
";
                    session.CreateSQLQuery(sql)
                        .SetDateTime("periodDate", DateTime.Today)
                        .ExecuteUpdate();
                }
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }

            return new BaseDataResult();
        }
    }
}