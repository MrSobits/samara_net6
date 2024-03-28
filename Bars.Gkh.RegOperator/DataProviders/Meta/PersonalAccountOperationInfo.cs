namespace Bars.Gkh.RegOperator.DataProviders.Meta
{
    /// <summary>
    /// Информация об операциях лицевого счета
    /// </summary>
    class PersonalAccountOperationInfo
    {
        public string Период { get; set; }

        public decimal ВходящееСальдо { get; set; }

        public decimal НачисленоВзносов { get; set; }

        public decimal НачисленоПени { get; set; }

        public decimal Перерасчет { get; set; }

        public decimal УплаченоВзносов { get; set; }

        public string ДатаОплаты { get; set; }

        public decimal УплаченоПени { get; set; }

        public decimal УстановкаИзменениеСальдо { get; set; }

        public decimal ИсходящееСальдо { get; set; }
    }
}