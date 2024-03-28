namespace Bars.GkhGji.Integration.AppealCits
{
    using System;
    using System.Xml.Serialization;
    /// <summary>
    /// Принимаемые поля в xml
    /// </summary>
    [Serializable]
    [XmlRoot("appeal")]
    [XmlType(AnonymousType = true, Namespace = "")]
    public class SedAppealCitDto
    {
        /// <summary>
        /// Абстрактный уникальный идентификатор обращения гражданина зарегистрированного в ГЖИ
        /// </summary>
        [XmlElement("id_sed")]
        public string SedId { get; set; }

        /// <summary>
        /// Номер регистрации обращения гражданина
        /// </summary>
        [XmlElement("rnumber")]
        public string Number { get; set; }

        /// <summary>
        /// Форма поступления обращения гражданина в Общественную приёмную Губернатора НСО (Возможно больше не используется)
        /// </summary>
        [XmlElement("from")]
        public string From { get; set; }

        /// <summary>
        /// Дата регистрации обращения гражданина в ГЖИ
        /// </summary>
        [XmlElement("date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// ФИО автора как указанно в обращении
        /// </summary>
        [XmlElement("author")]
        public string Author { get; set; }

        /// <summary>
        /// Предпочитаемый заявителем вид ответа
        /// </summary>
        [XmlElement("answertype")]
        public string AnswerType { get; set; }

        /// <summary>
        /// Адрес гражданина
        /// </summary>
        [XmlElement("address")]
        public string Address { get; set; }

        /// <summary>
        /// Заголовок (краткое описание обращения)
        /// </summary>
        [XmlElement("text")]
        public string Text { get; set; }

        /// <summary>
        /// Контакты гражданина
        /// </summary>
        [XmlElement("contacts")]
        public Contacts Contacts { get; set; }

        /// <summary>
        /// Источник поступления обращения гражданина в ГЖИ.
        /// В данном случае значение всегда будет = Общественная приёмная Губернатора НСО
        /// </summary>
        [XmlElement("source")]
        public string Source { get; set; }

        /// <summary>
        /// Дата регистрации обращения гражданина в Общественной приёмной Губернатора НСО
        /// </summary>
        [XmlElement("date_source")]
        public DateTime DateSource { get; set; }

        /// <summary>
        /// Номер обращения гражданина присвоенный в Общественной приёмной Губернатора НСО
        /// </summary>
        [XmlElement("rnumber_source")]
        public string RnumberSource { get; set; }

        /// <summary>
        /// Форма поступления обращения гражданина в Общественную приёмную Губернатора НСО
        /// </summary>
        [XmlElement("receipt_form")]
        public string ReceiptForm { get; set; }

        /// <summary>
        /// Контрольный срок
        /// </summary>
        [XmlElement("chektime")]
        public DateTime CheckTime { get; set; }
    }

    /// <summary>
    /// Контакты гражданина
    /// </summary>
    [Serializable]
    [XmlType(AnonymousType = true)]
    public class Contacts
    {
        /// <summary>
        /// Номер телефона
        /// </summary>
        [XmlElement("phone")]
        public string Phone { get; set; }

        /// <summary>
        /// E-mail
        /// </summary>
        [XmlElement("email")]
        public string Email { get; set; }
    }
}