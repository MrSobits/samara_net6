namespace Bars.Gkh.Regions.Tatarstan.DataProviders.Meta
{
    /// <summary>
    /// Сущность для экспорта "Договор УО с РСО на предоставление услуги"
    /// </summary>
    public class Оператор
    {
        /// <summary>
        /// Версия формата
        /// </summary>
        public string ВерсияФормата { get; set; }

        /// <summary>
        /// ИНН Контрагента
        /// </summary>
        public string Инн { get; set; }
        
        /// <summary>
        /// КПП Контрагента
        /// </summary>
        public string Кпп { get; set; }

        /// <summary>
        /// Огрн (Огрнип) Контрагента
        /// </summary>
        public string ОгрнОгрнип { get; set; }

        /// <summary>
        /// Год периода
        /// </summary>
        public int Год { get; set; }

        /// <summary>
        /// Месяц периода
        /// </summary>
        public int Месяц { get; set; }

        /// <summary>
        /// Дата и вреся формирования файла
        /// </summary>
        public string ДатаиВремяФормированияФайла { get; set; }

        /// <summary>
        /// ФИО отправителя
        /// </summary>
        public string ФиоОтправителя { get; set; }

        /// <summary>
        /// Телефон отправителя
        /// </summary>
        public string ТелефонОтправителя { get; set; }

        /// <summary>
        /// Тип поставщика информации
        /// </summary>
        public string ТипПоставщикаИнформации { get; set; }

    }
}