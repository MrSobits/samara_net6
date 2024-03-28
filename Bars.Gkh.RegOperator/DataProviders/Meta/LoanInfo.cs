namespace Bars.Gkh.RegOperator.DataProviders.Meta
{
    /// <summary>
    /// Данные для печатки распоряжение о заимствовании
    /// </summary>
    public class LoanInfo
    {
        /// <summary>
        /// ID займа
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// Номер ПодПункта
        /// </summary>
        public string НомерПП { get; set; }
        /// <summary>
        /// Дата займа
        /// </summary>
        public string ДатаЗайма { get; set; }
        /// <summary>
        /// Источник займа
        /// </summary>
        public string ИсточникЗайма { get; set; }
        /// <summary>
        /// Краткосрочная программа
        /// </summary>
        public string КраткосрочнаяПрограмма { get; set; }
        /// <summary>
        /// Сумма по одному источнику
        /// </summary>
        public string Сумма { get; set; }
        /// <summary>
        /// Погашено
        /// </summary>
        public string Погашено { get; set; }
        /// <summary>
        /// Задолженность
        /// </summary>
        public string Задолженность { get; set; }
        /// <summary>
        /// Фактическая дата возврата
        /// </summary>
        public string ФактДатаВозврата { get; set; }
    }
}
