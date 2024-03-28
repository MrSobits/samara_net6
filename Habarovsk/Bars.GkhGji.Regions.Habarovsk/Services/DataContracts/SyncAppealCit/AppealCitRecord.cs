namespace Bars.GkhGji.Regions.Habarovsk.Services.DataContracts.SyncAppealCit
{
    using Bars.Gkh.Enums;
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "AppealCitRecord")]
    public class AppealCitRecord
    {

        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public string Id { get; set; }

        /// <summary>
        /// Заголовок РКК ОГ.
        /// </summary>        
        [DataMember]
        [XmlAttribute(AttributeName = "AppealCaption")]
        public string AppealCaption { get; set; }

        /// <summary>
        /// Рег.номер РКК ОГ.
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "AppealRegistrationNumber")]
        public string AppealRegistrationNumber { get; set; }

        /// <summary>
        /// Дата регистрации РКК ОГ.
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "AppealRegistrationDate")]
        public string AppealRegistrationDate { get; set; }

        /// <summary>
        /// Номер сопроводительного письма
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "TransmittalLetterNumber")]
        public string TransmittalLetterNumber { get; set; }

        /// <summary>
        /// Дата сопроводительного письма
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "TransmittalLetterDate")]
        public string TransmittalLetterDate { get; set; }

        /// <summary>
        /// Корреспондент
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Correspondent")]
        public string Correspondent { get; set; }

        /// <summary>
        /// Адрес корреспондента
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "CorrespondentAddress")]
        public string CorrespondentAddress { get; set; }

        /// <summary>
        /// Срок по ОГ в целом.
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "AppealTerm")]
        public string AppealTerm { get; set; }

        /// <summary>
        /// Признак "Контроля" (Да/Нет)
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "IsControl")]
        public bool IsControl { get; set; }

        /// <summary>
        /// Признак "Неоднократное" (Да/Нет)
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "IsRepeated")]
        public bool IsRepeated { get; set; }

        /// <summary>
        /// Признак "Вторичное" (Да/Нет)
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "IsSecondary")]
        public bool IsSecondary { get; set; }

        /// <summary>
        /// Признак "Много пишущий автор"(Да/Нет)
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "IsFrequentAuthor")]
        public bool IsFrequentAuthor { get; set; }

        /// <summary>
        /// Гражданство
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Nationality")]
        public string Nationality { get; set; }

        /// <summary>
        /// Льготный состав
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Privilege")]
        public string Privilege { get; set; }

        /// <summary>
        /// Представитель
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Representative")]
        public string Representative { get; set; }

        /// <summary>
        /// Адрес для ответа
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ReplyAdress")]
        public string ReplyAdress { get; set; }

        /// <summary>
        /// e-mail
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Признак поступления
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Admission")]
        public string Admission { get; set; }

        /// <summary>
        /// Адресаты
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Addressees")]
        public string Addressees { get; set; }

        /// <summary>
        /// Предмет ведения
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ObjectConduct")]
        public string ObjectConduct { get; set; }

        /// <summary>
        /// Социальное положение.
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "SocialStatus")]
        public string SocialStatus { get; set; }

        /// <summary>
        /// Тематика+Вид вопроса+Тип вида вопроса(передается по каждой тематике в РКК ОГ)
        /// </summary>
        [DataMember]
        [XmlArray(ElementName = "AppealSubjects")]
        public AppealSubject[] AppealSubjects { get; set; }

        [DataMember]
        [XmlArray(ElementName = "Files")]
        public File[] Files { get; set; }

        [DataMember]
        [XmlArray(ElementName = "Resolutions")]
        public Resolution[] Resolutions { get; set; }
    }

    [DataContract]
    [XmlType(TypeName = "PortalAppeal")]
    public class PortalAppeal
    {

        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public string Id { get; set; }

        /// <summary>
        /// Заголовок РКК ОГ.
        /// </summary>        
        [DataMember]
        [XmlAttribute(AttributeName = "AppealCaption")]
        public string AppealCaption { get; set; }

        /// <summary>
        /// Содержание обращения
        /// </summary>        
        [DataMember]
        [XmlAttribute(AttributeName = "AppealText")]
        public string AppealText { get; set; }

        /// <summary>
        /// Особая отметка
        /// </summary>        
        [DataMember]
        [XmlAttribute(AttributeName = "AppealNote")]
        public string AppealNote { get; set; }

        /// <summary>
        /// Содержание обращения
        /// </summary>        
        [DataMember]
        [XmlAttribute(AttributeName = "AppealDate")]
        public DateTime AppealDate { get; set; }

        /// <summary>
        /// Рег.номер РКК ОГ.
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "AppealRegistrationNumber")]
        public string AppealRegistrationNumber { get; set; }

        /// <summary>
        /// Дата регистрации РКК ОГ.
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "AppealRegistrationDate")]
        public string AppealRegistrationDate { get; set; }

        /// <summary>
        /// Корреспондент
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Correspondent")]
        public string Correspondent { get; set; }

        /// <summary>
        /// Адрес корреспондента
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "CorrespondentAddress")]
        public string CorrespondentAddress { get; set; }

        /// <summary>
        /// Срок по ОГ в целом.
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "AppealTerm")]
        public string AppealTerm { get; set; }

        /// <summary>
        /// Признак "Контроля" (Да/Нет)
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "IsControl")]
        public bool IsControl { get; set; }

        /// <summary>
        /// Признак "Неоднократное" (Да/Нет)
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "IsRepeated")]
        public bool IsRepeated { get; set; }

        /// <summary>
        /// Признак "Вторичное" (Да/Нет)
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "IsSecondary")]
        public bool IsSecondary { get; set; }

        /// <summary>
        /// Признак "Много пишущий автор"(Да/Нет)
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "IsFrequentAuthor")]
        public bool IsFrequentAuthor { get; set; }

        /// <summary>
        /// Гражданство
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Nationality")]
        public string Nationality { get; set; }

        /// <summary>
        /// Льготный состав
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Privilege")]
        public string Privilege { get; set; }

        /// <summary>
        /// Представитель
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Representative")]
        public string Representative { get; set; }

        /// <summary>
        /// Адрес для ответа
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ReplyAdress")]
        public string ReplyAdress { get; set; }

        /// <summary>
        /// e-mail
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Признак поступления
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Admission")]
        public string Admission { get; set; }

        /// <summary>
        /// Адресаты
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Addressees")]
        public string Addressees { get; set; }

        /// <summary>
        /// Предмет ведения
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ObjectConduct")]
        public string ObjectConduct { get; set; }

        /// <summary>
        /// Социальное положение.
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "SocialStatus")]
        public string SocialStatus { get; set; }

        /// <summary>
        /// Тематика+Вид вопроса+Тип вида вопроса(передается по каждой тематике в РКК ОГ)
        /// </summary>
        [DataMember]
        [XmlArray(ElementName = "AppealSubjects")]
        public AppealSubject[] AppealSubjects { get; set; }

        [DataMember]
        [XmlArray(ElementName = "Files")]
        public File[] Files { get; set; }

        [DataMember]
        [XmlArray(ElementName = "Resolutions")]
        public Resolution[] Resolutions { get; set; }
    }

    public class AppealSubject
    {
        [DataMember]
        [XmlElement(ElementName = "SSTUCode")]
        public string SSTUCode { get; set; }

        [DataMember]
        [XmlElement(ElementName = "SubjectName")]
        public string SubjectName { get; set; }

        [DataMember]
        [XmlElement(ElementName = "QuestionKind")]
        public string QuestionKind { get; set; }

        [DataMember]
        [XmlElement(ElementName = "QuestionType")]
        public string QuestionType { get; set; }
    }

    public class AppealCitResult
    {
        [DataMember]
        [XmlAttribute("Code")]
        public int Code { get; set; }

        [DataMember]
        [XmlAttribute("Message")]
        public string Message { get; set; }

        [DataMember]
        [XmlAttribute("AppealID")]
        public string AppealID { get; set; }
    }

    public class ReportResult
    {
        [DataMember]
        [XmlAttribute("Code")]
        public int Code { get; set; }

        [DataMember]
        [XmlAttribute("Message")]
        public string Message { get; set; }

        [DataMember]
        [XmlAttribute("ReportId")]
        public string ReportId { get; set; }
    }

    public class AppealCitPortalResult
    {
        [DataMember]
        [XmlAttribute("Code")]
        public int Code { get; set; }

        [DataMember]
        [XmlAttribute("Message")]
        public string Message { get; set; }

        [DataMember]
        [XmlAttribute("State")]
        public string State { get; set; }

        [DataMember]
        [XmlAttribute("AppealID")]
        public string AppealID { get; set; }
    }

    public class AppealCitStateResult
    {
        [DataMember]
        [XmlElement(ElementName = "Code")]
        public int Code { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Message")]
        public string Message { get; set; }

        [DataMember]
        [XmlElement(ElementName = "State")]
        public string State { get; set; }

        [DataMember]
        [XmlElement(ElementName = "AppealID")]
        public string AppealID { get; set; }

        [DataMember]
        [XmlElement(ElementName = "RegDate")]
        public string RegDate { get; set; }

        [DataMember]
        [XmlElement(ElementName = "File")]
        public File File { get; set; }
    }   

    public class ReportState
    {
        [DataMember]
        [XmlAttribute("ReportId")]
        public string ReportId { get; set; }

        [DataMember]
        [XmlAttribute("Accepted")]
        public bool Accepted { get; set; }

        [DataMember]
        [XmlAttribute("DeclineReason")]
        public string DeclineReason { get; set; }
    }
}