namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.DomainModelServices;

    /// <summary>
    /// Действие перерасчета дебета и кредита для всех домов
    /// </summary>
    public class RecalcRoDebtAndCreditAction : BaseExecutionAction
    {
        /// <summary>
        /// Код действия
        /// </summary>
        public static string Code = "RecalcRoDebtAndCreditAction";

        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description => "Действие перерасчета дебета и кредита для всех домов";

        /// <summary>
        /// Название для отображения
        /// </summary>
        public override string Name => "РегОператор - Перерасчет дебета и кредита для всех домов";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => RecalcRoDebtAndCreditAction.Execute;

        private static BaseDataResult Execute()
        {
            var container = ApplicationContext.Current.Container;
            try
            {
                var updater = container.Resolve<IRealityObjectAccountUpdater>();
                var realityObjectDomain = container.ResolveDomain<RealityObject>();

                updater.UpdateCreditsAndDebts(realityObjectDomain.GetAll());
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }

            return new BaseDataResult();
        }
    }
}