namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Заполнения акт выполненных работ по дебету
    /// </summary>
    public class FillingPerformedWorkActOfDebit : BaseExecutionAction
    {
        /// <summary>
        /// Доме Операций счета по рассчету с поставщиками
        /// </summary>
        public IDomainService<RealityObjectSupplierAccountOperation> RealityObjectSupplierAccountOperation { get; set; }

        /// <summary>
        /// Доме "Связь операции по рассчету с поставщиками и актом выполненных работ"
        /// </summary>
        public IDomainService<RealObjSupplierAccOperPerfAct> RealObjSuppAccOperPerfActDomainService { get; set; }

        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description => "Заполнения акт выполненных работ по дебету";

        /// <summary>
        /// Название для отображения
        /// </summary>
        public override string Name => "Заполнения акт выполненных работ по дебету";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => this.ExecutionAction;

        private BaseDataResult ExecutionAction()
        {
            this.Container.InTransaction(
                () =>
                {
                    this.RealObjSuppAccOperPerfActDomainService.GetAll()
                        .Where(x => x.PerformedWorkAct.State.FinalState)
                        .Select(x => x.SupplierAccOperation)
                        .ForEach(this.SetRealObjSuppAccOper);
                });

            return new BaseDataResult(true);
        }

        private void SetRealObjSuppAccOper(RealityObjectSupplierAccountOperation x)
        {
            x.Debt = x.Credit;
            this.RealityObjectSupplierAccountOperation.Update(x);
        }
    }
}