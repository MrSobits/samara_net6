namespace Bars.GkhGji.Services.DataContracts
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Chelyabinsk.Services.Impl;

    /// <summary>
    /// Обращение гражданина
    /// </summary>
    [DataContract]
    [XmlType(TypeName = "CitizenAppeal")]
    public class CitizenAppeal
    {
        /// <summary>
        /// Номер обращения в ЕАИС «Обращения граждан»
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlAttribute(AttributeName = "AppealNumber")]
        [Validation(Description = "Номер обращения в ЕАИС «Обращения граждан»", 
            TypeData = TypeData.Text, 
            Required = true,
            MaxLength = 50)]
        public string AppealNumber { get; set; }

        /// <summary>
        /// Дата регистрации обращения в ЕАИС «Обращения граждан»
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlAttribute(AttributeName = "AppealDateOfRegistration")]
        [Validation(Description = "Дата регистрации обращения в ЕАИС «Обращения граждан»", 
            TypeData = TypeData.Date, 
            Required = true)]
        public DateTime AppealDateOfRegistration { get; set; }

        /// <summary>
        /// Источник поступления в ЕАИС «Обращения граждан»
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = "SourceOfReceipts", IsNullable = false)]
        public SourceOfReceipt[] SourceOfReceipts { get; set; }

        /// <summary>
        /// Форма обращения. Код из справочника «Формы обращений»
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlAttribute(AttributeName = "AppealForm")]
        [Validation(Description = "Форма обращения. Код из справочника «Формы обращений»",
            TypeData = TypeData.Number,
            Required = true)]
        public long AppealForm { get; set; }

        /// <summary>
        /// Количество листов в обращении
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "AmountOfPages")]
        [Validation(Description = "Количество листов в обращении",
            TypeData = TypeData.Number,
            Required = false,
            MinValue = 0)]
        public long? AmountOfPages { get; set; }

        /// <summary>
        /// ФИО заявителя
        /// </summary>
        [DataMember(IsRequired = true)] 
        [XmlAttribute(AttributeName = "FioOfDeclarant")]
        [Validation(Description = "ФИО заявителя",
            TypeData = TypeData.Text,
            Required = true,
            MaxLength = 600)]
        public string FioOfDeclarant { get; set; }

        /// <summary>
        /// Гражданство заявителя. Цифровой код страны указывается согласно ОКСМ
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "CitizenshipOfDeclarant")]
        [Validation(Description = "Гражданство заявителя. Цифровой код страны указывается согласно ОКСМ",
            TypeData = TypeData.Text,
            Required = false,
            MaxLength = 3)]
        public string CitizenshipOfDeclarant { get; set; }

        /// <summary>
        /// Почтовый адрес заявителя
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "MailingAddressOfDeclarant")]
        [Validation(Description = "Почтовый адрес заявителя",
            TypeData = TypeData.Text,
            Required = false,
            MaxLength = 200)]
        public string MailingAddressOfDeclarant { get; set; }

        /// <summary>
        /// Место работы заявителя
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "PlaceOfWorkOfDeclarant")]
        [Validation(Description = "Место работы заявителя",
            TypeData = TypeData.Text,
            Required = false,
            MaxLength = 300)]
        public string PlaceOfWorkOfDeclarant { get; set; }

        /// <summary>
        /// Номер телефона заявителя
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "PhoneNumberOfDeclarant")]
        [Validation(Description = "Номер телефона заявителя",
            TypeData = TypeData.Text,
            Required = false,
            MaxLength = 50)]
        public string PhoneNumberOfDeclarant { get; set; }

        /// <summary>
        /// E-mail заявителя
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "EmailOfDeclarant")]
        [Validation(Description = "E-mail заявителя",
            TypeData = TypeData.Text,
            Required = false,
            MaxLength = 100)]
        public string EmailOfDeclarant { get; set; }

        /// <summary>
        /// Пол заявителя: 1 – мужской; 2 – женский.
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "SexOfDeclarant")]
        [Validation(Description = "Пол заявителя: 1 – мужской; 2 – женский.",
            TypeData = TypeData.Number,
            Required = false,
            MinValue = 1,
            MaxValue = 2)]
        public long? SexOfDeclarant { get; set; }

        /// <summary>
        /// Категория заявителя
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = "CategoriesOfDeclarant")]
        public long[] CategoriesOfDeclarant { get; set; }

        /// <summary>
        /// Содержание обращения
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlAttribute(AttributeName = "AppealContent")]
        [Validation(Description = "Содержание обращения",
            TypeData = TypeData.Text,
            Required = true,
            MaxLength = 8000)]
        public string AppealContent { get; set; }

        /// <summary>
        /// Скан(ы) обращения
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "AppealAttachments")]
        public AppealAttachment[] AppealAttachments { get; set; }

        /// <summary>
        /// Виды и типы вопросов
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = "KindOfQuestions")]
        public long[] KindOfQuestions { get; set; }

        /// <summary>
        /// Список вопросов, поставленных в обращении (из Тематического классификатора)
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = "AppealQuestions", IsNullable = false)]
        public AppealQuestion[] AppealQuestions { get; set; }

        /// <summary>
        /// Кем рассмотрено обращение (руководитель)
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = "Executives", IsNullable = false)]
        public Executive[] Executives { get; set; }

        /// <summary>
        /// Исполнитель
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = "Executants", IsNullable = false)]
        public Executant[] Executants { get; set; }

        /// <summary>
        /// Основная резолюция
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlAttribute(AttributeName = "MainResolve")]
        [Validation(Description = "Основная резолюция",
            TypeData = TypeData.Text,
            Required = true,
            MaxLength = 8000)]
        public string MainResolve { get; set; }

        /// <summary>
        /// Статус «контрольное / не контрольное»: 1 – контрольное; 2 – не контрольное.
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Status")]
        [Validation(Description = "Статус «контрольное / не контрольное»: 1 – контрольное; 2 – не контрольное.",
            TypeData = TypeData.Number,
            Required = false,
            MinValue = 1,
            MaxValue = 2)]
        public long? Status { get; set; }

        /// <summary>
        /// Планируемая дата исполнения
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "PlannedDateExtension")]
        [Validation(Description = "Планируемая дата исполнения",
            TypeData = TypeData.Date,
            Required = false)]
        public DateTime? PlannedDateExtension { get; set; }

        /// <summary>
        /// Регистратор обращения (оператор)
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "RegistrarOfAppeal")]
        public RegistrarOfAppeal RegistrarOfAppeal { get; set; }

        /// <summary>
        /// UID обращения
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlAttribute(AttributeName = "AppealUid")]
        [Validation(Description = "UID обращения",
            TypeData = TypeData.Text,
            Required = true,
            MaxLength = 100)]
        public Guid AppealUid { get; set; }
    }
}
