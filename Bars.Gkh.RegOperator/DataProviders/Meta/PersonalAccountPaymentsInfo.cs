namespace Bars.Gkh.RegOperator.DataProviders.Meta
{
    using System;

    /// <summary>
    /// Информация по оплатам по лицевому счету
    /// </summary>
    public class PersonalAccountPaymentsInfo
    {
        public string КредитнаяОрганизация { get; set; }

        public string ТипОплаты { get; set; }

        public decimal Сумма { get; set; }

        public DateTime Дата { get; set; }

        public int WalletType { get; set; }

        public long PeriodId { get; set; }
    }
}