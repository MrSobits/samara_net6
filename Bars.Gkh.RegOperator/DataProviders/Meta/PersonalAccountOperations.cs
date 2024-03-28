namespace Bars.Gkh.RegOperator.DataProviders.Meta
{
    /// <summary>
    /// Операции по лицевому счету
    /// </summary>
    public class PersonalAccountOperations
    {
        public string Период { get; set; }

        public decimal ВходящееСальдо { get; set; }

        public decimal НачисленоВзносов { get; set; }

        public decimal НачисленоПени { get; set; }

        public decimal Перерасчет { get; set; }

        public decimal ПерерасчетБазовый { get; set; }

        public decimal ПерерасчетРешений { get; set; }

        public decimal ПерерасчетПени { get; set; }

        public decimal УстановкаИзменениеСальдо { get; set; }

        public decimal ИсходящееСальдо { get; set; }

        public decimal Тариф { get; set; }

        public decimal Площадь { get; set; }

        public decimal Доля { get; set; }

        public long PeriodId { get; set; }

        public decimal Зачет { get; set; }

        public decimal УплаченоВзносов { get; set; }

        public decimal УплаченоПени { get; set; }

        public string ДатаОплаты { get; set; }
    }
}