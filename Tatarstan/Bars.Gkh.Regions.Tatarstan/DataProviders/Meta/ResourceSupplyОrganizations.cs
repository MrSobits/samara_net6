namespace Bars.Gkh.Regions.Tatarstan.DataProviders.Meta
{
    /// <summary>
    /// Сущность для экспорта "Договор УО с РСО на предоставление услуги"
    /// </summary>
    public class ДоговорРесурсоснабжения
    {
        /// <summary>
        /// УправляющаяОрганизация ИНН
        /// </summary>
        public string УправляющаяОрганизацияИнн { get; set; }

        /// <summary>
        /// Управляющая организация КПП
        /// </summary>
        public string УправляющаяОрганизацияКпп { get; set; }

        /// <summary>
        /// Наименование управляющей организации
        /// </summary>
        public string НаименованиеУправляющейОрганизации { get; set; }

        /// <summary>
        /// Ресурсоснабжающая организация ИНН
        /// </summary>
        public string РесурсоснабжающаяОрганизацияИнн { get; set; }

        /// <summary>
        /// Ресурсоснабжающая организация КПП
        /// </summary>
        public string РесурсоснабжающаяОрганизацияКпп { get; set; }

        /// <summary>
        /// Наименование ресурсоснабжающая организация
        /// </summary>
        public string НаименованиеРесурсоснабжающаяОрганизация { get; set; }

        /// <summary>
        /// Код Услуги
        /// </summary>
        public int КодУслуги { get; set; }

        /// <summary>
        /// Наименование услуги
        /// </summary>
        public string НаименованиеУслуги { get; set; }

        /// <summary>
        /// ID Дома 
        /// </summary>
        public long КодДома { get; set; }

        /// <summary>
        /// Адрес дома
        /// </summary>
        public string АдресДома { get; set; }

        /// <summary>
        /// Начислено УО за месяц
        /// </summary>
        public decimal НачисленоУОзаМесяц { get; set; }

        /// <summary>
        /// Поступившие сплаты от УО
        /// </summary>
        public decimal ПоступившиеОплатыОтУо { get; set; }

        /// <summary>
        /// Исходящее сальдо
        /// </summary>
        public decimal ИсходящееСальдо { get; set; }
    }
}
