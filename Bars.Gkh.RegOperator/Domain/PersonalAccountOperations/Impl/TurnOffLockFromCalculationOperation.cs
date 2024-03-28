namespace Bars.Gkh.RegOperator.Domain.PersonalAccountOperations.Impl
{
    using Bars.B4;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.PersonalAccountGroup;

    using Castle.Windsor;

    /// <summary>
    /// Операция снятия блокировки расчета
    /// </summary>
    public class TurnOffLockFromCalculationOperation : PersonalAccountOperationBase
    {
        /// <summary>
        /// Имя системной группы
        /// </summary>
        public string SystemGroupName = "Сформирован документ в открытом периоде";

        /// <summary>
        /// Код операции
        /// </summary>
        public static string Key => "TurnOffLockFromCalculationOperation";

        /// <summary>
        /// Код операции
        /// </summary>
        public override string Code => TurnOffLockFromCalculationOperation.Key;

        /// <summary>
        /// Наименование операции
        /// </summary>
        public override string Name => "Снять блокировку расчета";

        /// <summary>
        /// Ключ прав доступа
        /// </summary>
        public override string PermissionKey => "GkhRegOp.PersonalAccount.Registry.Action.TurnOffLock";

        private readonly IWindsorContainer container;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="container"></param>
        public TurnOffLockFromCalculationOperation(IWindsorContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Метод выполнения операции
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат</returns>
        public override IDataResult Execute(BaseParams baseParams)
        {
            var isDeleteSnapshots = baseParams.Params.GetAs<bool>("deletesnapshot");
            var formedInOpenPeriodSystemGroup = this.container.Resolve<IGroupManager>("FormedInOpenPeriodSystemGroup");

            return this.container.InTransactionWithResult(
                 () =>
                        {
                            IDataResult result = null;
                            //удаляем из системной группы "Сформирован документ в открытом периоде"
                            result = formedInOpenPeriodSystemGroup.RemoveFromGroup(baseParams);

                            //удаляем слепки
                            if (result.Success && isDeleteSnapshots)
                            {
                                var paymentDocumentService = this.container.Resolve<IPaymentDocumentService>();
                                result = paymentDocumentService.DeleteSnapshots(baseParams);
                                this.container.Release(paymentDocumentService);
                            }

                            this.container.Release(formedInOpenPeriodSystemGroup);
                            return result;
                        });
       
        }
    }
}
