namespace Bars.Gkh.RegOperator.DataProviders.ChesImport.Meta
{
    public class SaldoChangeSummaryMeta
    {
        public decimal ИзменениеСальдоВсего => this.ИзменениеСальдоПоБазовомуТарифу + this.ИзменениеСальдоПоПени + this.ИзменениеСальдоПоТарифуРешения;
        public decimal ИзменениеСальдоПоБазовомуТарифу { get; set; }
        public decimal ИзменениеСальдоПоПени { get; set; }
        public decimal ИзменениеСальдоПоТарифуРешения { get; set; }
    }
}