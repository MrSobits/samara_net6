namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Зональная жилищная инспекция
    /// </summary>
    public class ZonalInspection : BaseGkhEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Зональное наименование
        /// </summary>
        public virtual string ZoneName { get; set; }

        /// <summary>
        /// Наименование для бланка
        /// </summary>
        public virtual string BlankName { get; set; }

        /// <summary>
        /// Краткое наименование
        /// </summary>
        public virtual string ShortName { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// Наименование (2 гос.язык)
        /// </summary>
        public virtual string NameSecond { get; set; }

        /// <summary>
        /// Зональное наименование (2 гос.язык)
        /// </summary>
        public virtual string ZoneNameSecond { get; set; }

        /// <summary>
        /// Наименование для бланка (2 гос.язык)
        /// </summary>
        public virtual string BlankNameSecond { get; set; }

        /// <summary>
        /// Краткое наименование (2 гос.язык)
        /// </summary>
        public virtual string ShortNameSecond { get; set; }

        /// <summary>
        /// Адрес (2 гос.язык)
        /// </summary>
        public virtual string AddressSecond { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>
        public virtual string Phone { get; set; }

        /// <summary>
        /// E-mail
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// ОКАТО - Общероссийский классификатор объектов административно-территориального деления.
        /// Пример: ОКАТО Татарстана 92000000000.
        /// </summary>
        public virtual string Okato { get; set; }

        /// <summary>
        /// Наименование Родительный падеж
        /// </summary>
        public virtual string NameGenetive { get; set; }

        /// <summary>
        /// Наименование Дательный падеж
        /// </summary>
        public virtual string NameDative { get; set; }

        /// <summary>
        /// Наименование Винительный падеж
        /// </summary>
        public virtual string NameAccusative { get; set; }

        /// <summary>
        /// Наименование Творительный падеж
        /// </summary>
        public virtual string NameAblative { get; set; }

        /// <summary>
        /// Наименование Предложный падеж
        /// </summary>
        public virtual string NamePrepositional { get; set; }

        /// <summary>
        /// Краткое наименование Родительный падеж
        /// </summary>
        public virtual string ShortNameGenetive { get; set; }

        /// <summary>
        /// Краткое наименование Дательный падеж
        /// </summary>
        public virtual string ShortNameDative { get; set; }

        /// <summary>
        /// Краткое наименование Винительный падеж
        /// </summary>
        public virtual string ShortNameAccusative { get; set; }

        /// <summary>
        /// Краткое наименование Творительный падеж
        /// </summary>
        public virtual string ShortNameAblative { get; set; }

        /// <summary>
        /// Краткое наименование Предложный падеж
        /// </summary>
        public virtual string ShortNamePrepositional { get; set; }

        /// <summary>
        /// Индекс отдела гжи
        /// </summary>
        public virtual string IndexOfGji { get; set; }

        /// <summary>
        /// Код нумерации обращения
        /// </summary>
        public virtual string AppealCode { get; set; }

        /// <summary>
        /// ОКТМО
        /// </summary>
        public virtual string Oktmo { get; set; }

        /// <summary>
        /// Населенный пункт
        /// </summary>
        public virtual string Locality { get; set; }

        /// <summary>
        /// Код отдела - используется при формировании документа ГЖИ
        /// </summary>
        public virtual string DepartmentCode { get; set; }

        /// <summary>
        /// Код отдела - используется при формировании документа ГЖИ
        /// </summary>
        public virtual bool IsNotGZHI { get; set; }

    }
}
