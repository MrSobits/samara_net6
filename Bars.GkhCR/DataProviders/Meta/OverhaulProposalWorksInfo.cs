namespace Bars.GkhCR.DataProviders.Meta
{
    /// <summary>
    /// Данные для печатки предложения о капремонте
    /// </summary>
    public class OverhaulProposalWorksInfo
    {
        /// <summary>
        /// Ид предложения о КР
        /// </summary>
        public long OverhaulProposalId { get; set; }
        /// <summary>
        /// Тип работы
        /// </summary>
        public string ТипРаботы { get; set; }
        /// <summary>
        /// Наименование работы
        /// </summary>
        public string НаименованиеРаботы { get; set; }
        /// <summary>
        /// Сумма работы
        /// </summary>
        public decimal Сумма { get; set; }
        /// <summary>
        /// Объем работы
        /// </summary>
        public decimal Объем { get; set; }
        /// <summary>
        /// Дата начала работы
        /// </summary>
        public string ДатаНачалаРаботы { get; set; }
        
    }
}
