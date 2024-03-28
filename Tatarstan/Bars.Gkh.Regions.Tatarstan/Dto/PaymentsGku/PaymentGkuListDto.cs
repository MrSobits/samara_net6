namespace Bars.Gkh.Regions.Tatarstan.Dto.PaymentsGku
{
    using System;

    /// <summary>
    /// DTO списка оплат по ЖКУ
    /// </summary>
    public class PaymentGkuListDto
    {
        /// <summary>
        /// Период
        /// </summary>
        public DateTime Period { get; set; }
        
        /// <summary>
        /// Номер ЛС
        /// </summary>
        public string AccountNumber { get; set; }
        
        /// <summary>
        /// Сумма задолженности
        /// </summary>
        public decimal DebtSum { get; set; }
        
        /// <summary>
        /// Начислено
        /// </summary>
        public decimal Accured { get; set; }
        
        /// <summary>
        /// Оплачено за предыдущий месяц
        /// </summary>
        public decimal PayedForPreviousMonth { get; set; }
    }
}