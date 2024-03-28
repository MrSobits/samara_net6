namespace Bars.Gkh.RegOperator.DataProviders.Meta
{
    using System;

    /// <summary>
    /// Информация по оплатам по лицевому счету
    /// </summary>
    public class PersonalAccountLawPaymentsInfo
    {

        public string AccountNumber { get; set; }

        public string PaymentDate { get; set; }

        public string PeriodName { get; set; }

        public long PeriodId { get; set; }

        public decimal BaseTariff { get; set; }

        public decimal RoomArea { get; set; }

        public decimal AreaShare { get; set; }

        public decimal TariffCharged { get; set; }

        public decimal TarifPayment { get; set; }

        public decimal TarifDebt { get; set; }

        public string Description { get; set; }
    }
}