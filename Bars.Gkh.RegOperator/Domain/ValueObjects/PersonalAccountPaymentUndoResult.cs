namespace Bars.Gkh.RegOperator.Domain.ValueObjects
{
    using System;
    using System.Collections.Generic;
    using Entities.ValueObjects;
    using Entities.Wallet;

    /// <summary>
    /// Результат отмены оплаты
    /// </summary>
    public class PersonalAccountPaymentUndoResult
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public PersonalAccountPaymentUndoResult()
        {
            this.Wallets = new List<Tuple<IWallet, Transfer>>();
            this.Transfers = new List<Transfer>();
        }

        /// <summary>
        /// Отменено по базовому
        /// </summary>
        public decimal UndoByBaseTariff { get; set; }

        /// <summary>
        /// Отменено по соц.поддержке
        /// </summary>
        public decimal UndoBySocSupport { get; set; }

        /// <summary>
        /// Отменено по тарифу решений
        /// </summary>
        public decimal UndoByDecisionTariff { get; set; }

        /// <summary>
        /// Отменено по пени
        /// </summary>
        public decimal UndoPenalty { get; set; }

        /// <summary>
        /// Кошельки
        /// </summary>
        public List<Tuple<IWallet, Transfer>> Wallets { get; set; }

        /// <summary>
        /// Трансферы
        /// </summary>
        public List<Transfer> Transfers { get; set; }

        /// <summary>
        /// Добавление трансфера
        /// </summary>
        /// <param name="transfer">Трансфер</param>
        public void AddTransfer(Transfer transfer)
        {
            if (transfer != null)
            {
                this.Transfers.Add(transfer);
            }
        }
    }
}