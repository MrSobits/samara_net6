namespace Bars.Gkh.RegOperator.DataProviders.Meta
{
    [System.Obsolete("use class InvoiceInfo")]
    internal class InvoiceRegistryAndActInfo //: InvoiceInfo
    {
        public string АдресКонтрагента { get; set; }

        public string ПочтовыйИндексКонтрагента { get; set; }

        public string ПочтовыйАдресКонтрагента { get; set; }

        public string Руководитель { get; set; }

        public string СуммаСтрокой { get; set; }

        public string ГлавБух { get; set; }
        
        public string РуководительПолучателя { get; set; }

        public string ГлБухПолучателя { get; set; }

        public string НаименованиеПолучателяКраткое { get; set; }

        public string ПериодНачислений { get; set; }

        public int ТипСобственника { get; set; }

		public string Наименование { get; set; }

		public string РСчетБанка { get; set; }

		public string ОКПО { get; set; }

		public string ОКОНХ { get; set; }
    }
}