namespace Bars.Gkh.RegOperator.DomainModelServices
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.RegOperator.Entities.Refactor.TransferOwner;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;

    /// <summary>
    /// Поставщик, поддерживаемый отмену
    /// </summary>
    
    public interface ICancellableSourceProvider
    {
        /// <summary>
        /// Отменить операцию
        /// </summary>
        /// <param name="operation">Операция</param>
        /// <param name="parameter">Параметры перераспределения средств</param>
        /// <returns>Результат операции</returns>
        IDataResult CancelOperation([NotNull]MoneyOperation operation, [NotNull]RepaymentParameters parameter);
    }

    /// <summary>
    /// Результат отмены
    /// </summary>
    public class RepaymentParameters
    {
        /// <summary>
        /// Список объектов, на которые будет производиться перезачисление
        /// </summary>
        public ITransferOwner OwnerToRepayment { get; set; }

        /// <summary>
        /// Объект, с которого списываются средства
        /// </summary>
        public IEnumerable<ITransferOwner> OldOwners { get; set; }

        /// <summary>
        /// Старая операция
        /// </summary>
        public MoneyOperation OldMoneyOperation { get; set; }

        /// <summary>
        /// Трансферы, которые будут отменены
        /// </summary>
        public IEnumerable<Transfer> UndoTransfers { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        public RepaymentParameters()
        {
            this.OldOwners = Enumerable.Empty<ITransferOwner>();
        }
    }
}