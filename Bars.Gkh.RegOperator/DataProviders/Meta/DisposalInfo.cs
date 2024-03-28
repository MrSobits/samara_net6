namespace Bars.Gkh.RegOperator.DataProviders.Meta
{
    /// <summary>
    /// Данные для печатки распоряжение о заимствовании
    /// </summary>
    public class DisposalInfo
    {
        /// <summary>
        /// ID займа
        /// </summary>
        public long LoanId { get; set; }
        /// <summary>
        /// Номер займа
        /// </summary>
        public string Номер { get; set; }
        /// <summary>
        /// Дата формирования печатной формы
        /// </summary>
        public string Дата { get; set; }
        /// <summary>
        /// Регион, к которому относится дом-заниматель
        /// </summary>
        public string Регион { get; set; }
        /// <summary>
        /// Муниципальный район дома-занимателя
        /// </summary>
        public string МР { get; set; }
        /// <summary>
        /// Муниципальное образование дома-занимателя
        /// </summary>
        public string МО { get; set; }
        /// <summary>
        /// Адрес дома-занимателя, по которому формируется печатка
        /// </summary>
        public string АдресЗанимателя { get; set; }
        /// <summary>
        /// ФИО оператора, который запустил формирование печатки
        /// </summary>
        public string ФИОКонтроль { get; set; }
        /// <summary>
        ///  ФИО оператора, который запустил формирование печатки
        /// </summary>
        public string ФИОДолжЛица { get; set; }
        /// <summary>
        /// Оставляем пустой. Красивой логики нет.
        /// </summary>
        public string Должность { get; set; }
        /// <summary>
        /// Уровень формирования займов
        /// </summary>
        public string УровеньЗайма { get; set; }
    }
}
