namespace Bars.GkhCr.Report.FinancingAnnex4F1
{
    using System;

    using Bars.GkhCr.Enums;

    public class PaymentOrderProxy
    {
        public string Municipality { get; set; }

        public string Address { get; set; }

        /// <summary>
        /// Номер выписки
        /// </summary>
        public string BankStatementNum { get; set; }

        /// <summary>
        /// Дата платежного поручения
        /// </summary>
        public DateTime? DocumentDate { get; set; }

        public string DocumentNum { get; set; }

        public string DocumentDateNum
        {
            get
            {
                return string.Format("{0}{1}", DocumentNum, DocumentDate.HasValue ? " от " + DocumentDate.Value.ToShortDateString() : string.Empty);
            }
        }

        /// <summary>
        /// Дата заявки
        /// </summary>
        public DateTime? BidDate { get; set; }

        public string BidNum { get; set; }

        public string BidDateNum
        {
            get
            {
                return string.Format("{0}{1}", BidNum, BidDate.HasValue ? " от " + BidDate.Value.ToShortDateString() : string.Empty);
            }
        }

        public TypeFinanceSource TypeFinanceSource { get; set; }

        /// <summary>
        /// Приход
        /// </summary>
        public decimal? IncomingSum { get; set; }

        /// <summary>
        /// Плательщик
        /// </summary>
        public string PayerContragent { get; set; }

        /// <summary>
        /// Получатель
        /// </summary>
        public string ReceiverContragent { get; set; }

        /// <summary>
        /// Назаначение платежа
        /// </summary>
        public string PayPurpose { get; set; }

        /// <summary>
        /// Повторно направленные средства
        /// </summary>
        public bool RepeatSend { get; set; }

        /// <summary>
        /// Приход повторных
        /// </summary>
        public decimal? RedirectFunds { get; set; }

    }
}
