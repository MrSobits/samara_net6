namespace Bars.Gkh.RegOperator.DataProviders.ChesImport.Meta
{
    public class PaymentSummaryMeta
    {
        public decimal ОплаченоВсего => this.ОплаченоПоБазовомуТарифу + this.ОплаченоПени + this.ОплаченоПоТарифуРешения;
        public decimal ОплаченоПоБазовомуТарифу { get; set; }
        public decimal ОплаченоПени { get; set; }
        public decimal ОплаченоПоТарифуРешения { get; set; }

        public decimal ОтменаОплатВсего => this.ОтменаОплатПоБазовомуТарифу + this.ОтменаОплатПени + this.ОтменаОплатПоТарифуРешения;
        public decimal ОтменаОплатПоБазовомуТарифу { get; set; }
        public decimal ОтменаОплатПени { get; set; }
        public decimal ОтменаОплатПоТарифуРешения { get; set; }

        public decimal ВозвратОплатВсего => this.ВозвратОплатПоБазовомуТарифу + this.ВозвратОплатПени + this.ВозвратОплатПоТарифуРешения;
        public decimal ВозвратОплатПоБазовомуТарифу { get; set; }
        public decimal ВозвратОплатПени { get; set; }
        public decimal ВозвратОплатПоТарифуРешения { get; set; }

        public decimal ИтогоОплатВсего => this.ИтогоОплатПоБазовомуИарифу + this.ИтогоОплатПени + this.ИтогоОплатПоТарифуРешения;
        public decimal ИтогоОплатПоБазовомуИарифу => this.ОплаченоПоБазовомуТарифу - this.ОтменаОплатПоБазовомуТарифу - this.ВозвратОплатПоБазовомуТарифу;
        public decimal ИтогоОплатПени => this.ОплаченоПени - this.ОтменаОплатПени - this.ВозвратОплатПени;
        public decimal ИтогоОплатПоТарифуРешения => this.ОплаченоПоТарифуРешения - this.ОтменаОплатПоТарифуРешения - this.ВозвратОплатПоТарифуРешения;

    }
}