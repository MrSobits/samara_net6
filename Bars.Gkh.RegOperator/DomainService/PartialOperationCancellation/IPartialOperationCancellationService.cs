namespace Bars.Gkh.RegOperator.DomainService.PartialOperationCancellation
{
    using System.Linq;

    using Bars.B4;

    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Entities.Refactor.TransferOwner;

    /// <summary>
    /// Сервис частичной отмены операции
    /// </summary>
    public interface IPartialOperationCancellationService
    {
        /// <summary>
        /// Отменить оплаты
        /// </summary>
        /// <param name="transferQuery">Поздапрос трансферов для отмены</param>
        /// <param name="repaymentTarget">Поздапрос получателей оплаты</param>
        /// <returns>Результат операции</returns>
        IDataResult UndoAndRepayment(IQueryable<Transfer> transferQuery, ITransferOwner repaymentTarget);
    }
}