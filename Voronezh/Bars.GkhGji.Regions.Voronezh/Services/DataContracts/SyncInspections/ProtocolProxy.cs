namespace Bars.GkhGji.Regions.Voronezh.Services.DataContracts.SyncInspections
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums;

    [DataContract]
    [XmlType(TypeName = "ProtocolProxy")]
    public class ProtocolProxy
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// Проверяемая площадь - берется из МКД
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Area")]
        public decimal? Area { get; set; }

        /// <summary>
        /// Место составления
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "DocumentPlace")]
        public string DocumentPlace { get; set; }

        /// <summary>
        /// Контрагент
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ContragentProxy")]
        public ContragentProxy ContragentProxy { get; set; }

        /// <summary>
        /// Физическое лицо
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "PhysicalPerson")]
        public string PhysicalPerson { get; set; }

        /// <summary>
        /// Реквизиты физ. лица
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "PhysicalPersonInfo")]
        public string PhysicalPersonInfo { get; set; }

        /// <summary>
        /// Нарушения
        /// </summary>
        [DataMember]
        [XmlArray(ElementName = "RealityObjectViolations")]
        public RealityObjectViolation[] RealityObjectViolations { get; set; }

        /// <summary>
        /// Статьи закона
        /// </summary>
        [DataMember]
        [XmlArray(ElementName = "ArticleLawGjiProxys")]
        public ArticleLawGjiProtProxy[] ArticleLawGjiProtProxyes { get; set; }

        /// <summary>
        /// Тип исполнителя документа
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Executant")]
        public long ExecutantId { get; set; }


        /// <summary>
        /// Тип документа физ лица
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "PhysicalPersonDocType")]
        public long PhysicalPersonDocTypeId { get; set; }

        /// <summary>
        /// Номер документа физлица
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "PhysicalPersonDocumentNumber")]
        public string PhysicalPersonDocumentNumber { get; set; }

        /// <summary>
        /// Серия документа физлица
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "PhysicalPersonDocumentSerial")]
        public string PhysicalPersonDocumentSerial { get; set; }

        /// <summary>
        /// Не является гражданином РФ
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "PhysicalPersonIsNotRF")]
        public bool PhysicalPersonIsNotRF { get; set; }

        /// <summary>
        /// Дата передачи в суд
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "DateToCourt")]
        public DateTime? DateToCourt { get; set; }

        /// <summary>
        /// Документ передан в суд
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ToCourt")]
        public bool ToCourt { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// Дата рассмотрения дела
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "DateOfProceedings")]
        public DateTime? DateOfProceedings { get; set; }

        /// <summary>
        /// Время рассмотрения дела(час)
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "HourOfProceedings")]
        public int HourOfProceedings { get; set; }

        /// <summary>
        /// Время рассмотрения дела(мин)
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "MinuteOfProceedings")]
        public int MinuteOfProceedings { get; set; }

        /// <summary>
        /// Лицо, выполнившее перепланировку/переустройство
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "PersonFollowConversion")]
        public string PersonFollowConversion { get; set; }

        /// <summary>
        /// Место составления
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "FormatPlace")]
        public string FormatPlace { get; set; }

        /// <summary>
        /// Часы времени составления
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "FormatHour")]
        public int? FormatHour { get; set; }

        /// <summary>
        /// Минуты времени составления
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "FormatMinute")]
        public int? FormatMinute { get; set; }

        /// <summary>
        /// Доставлено через канцелярию
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "NotifDeliveredThroughOffice")]
        public bool NotifDeliveredThroughOffice { get; set; }

        /// <summary>
        ///  Дата  составления протокола 
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "FormatDate")]
        public DateTime? FormatDate { get; set; }

        /// <summary>
        /// Номер уведомления о месте и времени составления протокола
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "NotifNumber")]
        public string NotifNumber { get; set; }

        /// <summary>
        /// Количество экземпляров
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ProceedingCopyNum")]
        public int ProceedingCopyNum { get; set; }

        /// <summary>
        /// Место рассмотрения дела
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ProceedingsPlace")]
        public string ProceedingsPlace { get; set; }

        /// <summary>
        /// Замечания к протоколу со стороны нарушителя
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Remarks")]
        public string Remarks { get; set; }

        /// <summary>
        /// УИН
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "UIN")]
        public string UIN { get; set; }

        /// <summary>
        /// Адрес регистрации (место жительства, телефон)
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "PersonRegistrationAddress")]
        public string PersonRegistrationAddress { get; set; }

        /// <summary>
        /// Фактический адрес
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "PersonFactAddress")]
        public string PersonFactAddress { get; set; }

        /// <summary>
        /// Место работы
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "PersonJob")]
        public string PersonJob { get; set; }

        /// <summary>
        /// Должность
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "PersonPosition")]
        public string PersonPosition { get; set; }

        /// <summary>
        /// Дата, место рождения
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "PersonBirthDatePlace")]
        public string PersonBirthDatePlace { get; set; }

        /// <summary>
        /// Документ, удостоверяющий личность
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "PersonDoc")]
        public string PersonDoc { get; set; }

        /// <summary>
        /// Заработная плата
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "PersonSalary")]
        public string PersonSalary { get; set; }

        /// <summary>
        /// Семейное положение, кол-во иждивенцев
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "PersonRelationship")]
        public string PersonRelationship { get; set; }

        /// <summary>
        /// Протокол - Реквизиты - В присуствии/отсутствии
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "TypePresence")]
        public TypeRepresentativePresence TypePresence { get; set; }

        /// <summary>
        /// Представитель
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Representative")]
        public string Representative { get; set; }

        /// <summary>
        /// Вид и реквизиты основания
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ReasonTypeRequisites")]
        public string ReasonTypeRequisites { get; set; }

        /// <summary>
        /// Нарушения - Дата правонарушения
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "DateOfViolation")]
        public DateTime? DateOfViolation { get; set; }

        /// <summary>
        /// Нарушения - Час правонарушения
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "HourOfViolation")]
        public int? HourOfViolation { get; set; }

        /// <summary>
        /// Нарушения - Минута правонарушения
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "MinuteOfViolation")]
        public int? MinuteOfViolation { get; set; }

        /// <summary>
        /// Нарушения - Наименование требования
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ResolveViolationClaimId")]
        public long ResolveViolationClaimId { get; set; }
    }

    /// <summary>
    /// статья закона протокола ГЖИ
    /// </summary>
    [DataContract]
    [XmlType(TypeName = "ArticleLawGjiProtProxy")]
    public class ArticleLawGjiProtProxy
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }


        /// <summary>
        /// Описание
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// Статья
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ArticleLawGjiId")]
        public long ArticleLawGjiId { get; set; }
    }

}