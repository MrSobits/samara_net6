using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Bars.GkhGji.Regions.Habarovsk.Services.DataContracts.SyncAppealCitFromEDM
{
    [DataContract]
    public class RegCardToBarsDto
    {
        /// <summary>
        /// Данные заявителя
        /// </summary>        
        [DataMember]
        public List<BarsApplicantDto> Applicants { get; set; }

        /// <summary>
        /// Идентификатор обращения
        /// </summary>        
        [DataMember]
        public Guid ComplaintId { get; set; }

        /// <summary>
        /// Наименование обращения
        /// </summary>        
        [DataMember]
        public string ComplaintName { get; set; }

        /// <summary>
        /// Тип корреспондента
        /// </summary>        
        [DataMember]
        public string BarsComplaintSourceName { get; set; }

        /// <summary>
        /// Дата создания документа
        /// </summary>        
        [DataMember]
        public DateTime DeliveryDate { get; set; }

        /// <summary>
        /// Год создания документа
        /// </summary>        
        [DataMember]
        public int DeliveryYear { get; set; }

        /// <summary>
        /// Контрольный срок
        /// </summary>        
        [DataMember]
        public DateTime ControlDueDate { get; set; }

        /// <summary>
        /// Вид обращения
        /// </summary>        
        [DataMember]
        public string DeliveryMethod { get; set; }

        /// <summary>
        /// Количество вопросов
        /// </summary>        
        [DataMember]
        public int QuestionCount { get; set; }

        /// <summary>
        /// Описание обращения
        /// </summary>        
        [DataMember]
        public string Content { get; set; }

        /// <summary>
        /// Повторность обращения
        /// </summary>        
        [DataMember]
        public string Frequency { get; set; }

        /// <summary>
        /// Связанные аналогичные
        /// </summary>        
        [DataMember]
        public List<CardToCardLinkDto> CardToCardLinks { get; set; }

        /// <summary>
        /// Предыдущее обращение
        /// </summary>        
        [DataMember]
        public MainCaseFileDto MainCaseFile { get; set; }

        /// <summary>
        /// Письменное обращение
        /// </summary>        
        [DataMember]
        public FileDto ComplaintFile { get; set; }

        /// <summary>
        /// Вложения
        /// </summary>        
        [DataMember]
        public List<FileDto> Attachments { get; set; }

        /// <summary>
        /// Муниципальное образование (откуда поступило обращение)
        /// </summary>        
        [DataMember]
        public string SubjectName { get; set; }

        /// <summary>
        /// Адрес (Место действия факта или события описанного заявителем)
        /// </summary>        
        [DataMember]
        public string AdmLocationSubjectName { get; set; }

        /// <summary>
        /// Тематики
        /// </summary>        
        [DataMember]
        public List<string> ComplaintQuestionThemeNames { get; set; }
    }

    [DataContract]
    public class RegCardToEdmAdapterDto
    {
        /// <summary>
        /// Идентификатор обращения
        /// </summary>        
        [DataMember]
        public Guid ComplaintId { get; set; }

        /// <summary>
        /// Наименование обращения
        /// </summary>        
        [DataMember]
        public string ComplaintName { get; set; }

        /// <summary>
        /// Тип ответа
        /// </summary>        
        [DataMember]
        public string AnswerKind { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>        
        [DataMember]
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Порядковый номер
        /// </summary>        
        [DataMember]
        public string OrderNumber { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>        
        [DataMember]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Адресат
        /// </summary>        
        [DataMember]
        public string Correspondent { get; set; }

        /// <summary>
        /// Исполнитель
        /// </summary>        
        [DataMember]
        public string ExecutorName { get; set; }

        /// <summary>
        /// Подписант
        /// </summary>        
        [DataMember]
        public string SignatoryName { get; set; }

        /// <summary>
        /// Содержание ответа
        /// </summary>        
        [DataMember]
        public string Content { get; set; }

        /// <summary>
        /// Комментарий к документу
        /// </summary>        
        [DataMember]
        public string DocumentComment { get; set; }

        /// <summary>
        /// Вложения
        /// </summary>        
        [DataMember]
        public List<FileDto> Attachments { get; set; }
    }

    [DataContract]
    public class BarsApplicantDto
    {
        /// <summary>
        /// ФИО заявителя
        /// </summary>        
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Адрес заявителя
        /// </summary>        
        [DataMember]
        public string Address { get; set; }

        /// <summary>
        /// Электронная почта заявителя
        /// </summary>        
        [DataMember]
        public string Email { get; set; }

        /// <summary>
        /// Номер телефона заявителя
        /// </summary>        
        [DataMember]
        public string Phone { get; set; }

        /// <summary>
        /// Социальное положение заявителя
        /// </summary>        
        [DataMember]
        public List<string> SocialStatuses { get; set; }
    }

    [DataContract]
    public class CardToCardLinkDto
    {
        /// <summary>
        /// Идентификатор связанного обращения
        /// </summary>        
        [DataMember]
        public Guid Id { get; set; }

        /// <summary>
        /// Наименование связанного обращения
        /// </summary>        
        [DataMember]
        public string Name { get; set; }
    }

    [DataContract]
    public class MainCaseFileDto
    {
        /// <summary>
        /// Идентификатор предыдущего обращения
        /// </summary>        
        [DataMember]
        public Guid Id { get; set; }

        /// <summary>
        /// Наименование предыдущего обращения
        /// </summary>        
        [DataMember]
        public string Name { get; set; }
    }

    [DataContract]
    public class FileDto
    {
        /// <summary>
        /// Данные прикрепленного документа
        /// </summary>        
        [DataMember]
        public ObjectMinDto File { get; set; }

        /// <summary>
        /// Данные подписей документа
        /// </summary>        
        [DataMember]
        public List<BarsEdmSignDto> Signs { get; set; }
    }

    [DataContract]
    public class ObjectMinDto
    {
        /// <summary>
        ///  Идентификатор контейнера документа
        /// </summary>        
        [DataMember]
        public Guid Id { get; set; }

        /// <summary>
        /// Наименование документа
        /// </summary>        
        [DataMember]
        public string Name { get; set; }
    }

    [DataContract]
    public class BarsEdmSignDto
    {
        /// <summary>
        /// Идентификатор подписи в CS
        /// </summary>        
        [DataMember]
        public Guid Id { get; set; }

        /// <summary>
        /// Наименование файла
        /// </summary>        
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Владелец сертификата
        /// </summary>        
        [DataMember]
        public string Owner { get; set; }

        /// <summary>
        /// Серийный номер сертификата
        /// </summary>        
        [DataMember]
        public string Serial { get; set; }

        /// <summary>
        /// Дата истечения срока действия сертификата
        /// </summary>        
        [DataMember]
        public string ExpiryDate { get; set; }

        /// <summary>
        /// Дата подписания файла в формате yyyy-MMdd'T'HH:mm:ss'Z'
        /// </summary>        
        [DataMember]
        public DateTime SignDate { get; set; }
    }

    [DataContract]
    public class ErrorDto
    {
        /// <summary>
        ///  HTTP код
        /// </summary>        
        [DataMember]
        public int Status { get; set; }

        /// <summary>
        /// Ошибка
        /// </summary>        
        [DataMember]
        public string Message { get; set; }
    }
}
