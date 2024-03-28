namespace Bars.Gkh.RegOperator.DataProviders.Meta
{
    /// <summary>
    /// Мета - Оплаты подрядчикам
    /// </summary>
    public class AccountTransferCtrPaymentMeta
    {
        public long ИдентификаторЛС { get; set; }
        public string МуниципальныйРайон { get; set; }
        public string АдресДома { get; set; }
        public string НомерКвартиры { get; set; }
        public string НомерЛС { get; set; }
        public string Собственник { get; set; }

        public decimal ОплаченоВсегоБазовый { get; set; }
        public decimal ОплаченоВсегоТариф { get; set; }
        public decimal ОплаченоВсегоПени { get; set; }

        public decimal ВсегоОплатНаЛСБазовый { get; set; }
        public decimal ВсегоОплатНаЛСТариф { get; set; }
        public decimal ВсегоОплатНаЛСПени { get; set; }

        public decimal ОплаченоПодрядчикамБазовый { get; set; }
        public decimal ОплаченоПодрядчикамТариф { get; set; }
        public decimal ОплаченоПодрядчикамПени { get; set; }

        public decimal ОстатокБазовый => this.ВсегоОплатНаЛСБазовый - this.ОплаченоПодрядчикамБазовый;
        public decimal ОстатокТариф => this.ВсегоОплатНаЛСТариф - this.ОплаченоПодрядчикамТариф;
        public decimal ОстатокПени => this.ВсегоОплатНаЛСПени - this.ОплаченоПодрядчикамПени;
    }
}
