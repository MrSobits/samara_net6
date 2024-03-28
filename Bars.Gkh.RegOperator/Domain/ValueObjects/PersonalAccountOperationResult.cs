namespace Bars.Gkh.RegOperator.Domain.ValueObjects
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;

    /// <summary>
    /// Результат операции лицевого счета
    /// </summary>
    public class PersonalAccountOperationResult
    {
        private IList<Transfer> transfers;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="moneyOperation">Операция</param>
        public PersonalAccountOperationResult(MoneyOperation moneyOperation)
        {
            this.transfers = new List<Transfer>();
            this.Operation = moneyOperation;
        }

        /// <summary>
        /// Операция
        /// </summary>
        public MoneyOperation Operation { get; }

        /// <summary>
        /// Трансферы
        /// </summary>
        public IReadOnlyCollection<Transfer> Transfers => new ReadOnlyCollection<Transfer>(this.transfers);

        /// <summary>
        /// Добавить транфсер
        /// </summary>
        /// <param name="transfer">Трансфер</param>
        public void AddTransfer(Transfer transfer)
        {
            if (transfer.IsNull())
            {
                return;
            }

            this.Operation.Amount += transfer.Amount;
            this.transfers.Add(transfer);
        }
    }
}