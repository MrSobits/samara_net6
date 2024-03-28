namespace Bars.Gkh.RegOperator.DataProviders.ChesImport.Meta
{
    public class RecalcSummaryMeta
    {
        public decimal ПерерасчетВсего => this.ПерерасчетПоБазовомуТарифу + this.ПерерасчетПоПени + this.ПерерасчетПоТарифуРешения;
        public decimal ПерерасчетПоБазовомуТарифу { get; set; }
        public decimal ПерерасчетПоПени { get; set; }
        public decimal ПерерасчетПоТарифуРешения { get; set; }
    }
}