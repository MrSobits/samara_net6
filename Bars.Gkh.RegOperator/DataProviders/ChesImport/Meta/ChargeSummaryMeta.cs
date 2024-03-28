namespace Bars.Gkh.RegOperator.DataProviders.ChesImport.Meta
{
    public class ChargeSummaryMeta
    {
        public decimal ВходящееСальдо { get; set; }
        public decimal ВходящееСальдоПоБазовомуТарифу { get; set; }
        public decimal ВходящееСальдоПоПени { get; set; }
        public decimal ВходящееСальдоПоТарифуРешения { get; set; }
        public decimal НачисленоВсего => this.НачисленоПоБазовомуТарифу + this.НачисленоПоПени + this.НачисленоПоТарифуРешения;
        public decimal НачисленоПоБазовомуТарифу { get; set; }
        public decimal НачисленоПоПени { get; set; }
        public decimal НачисленоПоТарифуРешения { get; set; }
        public decimal ОплаченоВсего => this.ОплаченоПоБазовомуТарифу + this.ОплаченоПоПени + this.ОплаченоПоТарифуРешения;
        public decimal ОплаченоПоБазовомуТарифу { get; set; }
        public decimal ОплаченоПоПени { get; set; }
        public decimal ОплаченоПоТарифуРешения { get; set; }
        public decimal ИзменениеСальдоВсего => this.ИзменениеСальдоПоБазовомуТарифу + this.ИзменениеСальдоПоПени + this.ИзменениеСальдоПоТарифуРешения;
        public decimal ИзменениеСальдоПоБазовомуТарифу { get; set; }
        public decimal ИзменениеСальдоПоПени { get; set; }
        public decimal ИзменениеСальдоПоТарифуРешения { get; set; }
        public decimal ПерерасчетВсего => this.ПерерасчетПоБазовомуТарифу + this.ПерерасчетПоПени + this.ПерерасчетПоТарифуРешения;
        public decimal ПерерасчетПоБазовомуТарифу { get; set; }
        public decimal ПерерасчетПоПени { get; set; }
        public decimal ПерерасчетПоТарифуРешения { get; set; }
        public decimal ИсходящееСальдо { get; set; }
        public decimal ИсходящееСальдоПоБазовомуТарифу { get; set; }
        public decimal ИсходящееСальдоПоПени { get; set; }
        public decimal ИсходящееСальдоПоТарифуРешения { get; set; }
    }
}