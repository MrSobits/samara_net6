namespace Bars.GisIntegration.Base.Entities.Inspection
{
    using System;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Enums;

    /// <summary>
    /// Проверка ГЖИ
    /// </summary>
    public class Examination : BaseRisEntity
    {
        /// <summary>
        /// План проверок. Используется только для плановых проверок юр. лиц
        /// </summary>
        public virtual InspectionPlan InspectionPlan { get; set; }

        /// <summary>
        /// Порядковый номер проверки
        /// </summary>
        public virtual int? InspectionNumber { get; set; }

        /// <summary>
        /// Код формы проведения проверки. НСИ "Форма проведения проверки" (реестровый номер 71)
        /// </summary>
        public virtual string ExaminationFormCode { get; set; }

        /// <summary>
        /// GUID формы проведения проверки. GUID НСИ "Форма проведения проверки" (реестровый номер 71)
        /// </summary>
        public virtual string ExaminationFormGuid { get; set; }

        /// <summary>
        /// Номер распоряжения (приказа)
        /// </summary>
        public virtual string OrderNumber { get; set; }

        /// <summary>
        /// Дата утверждения распоряжения (приказа)
        /// </summary>
        public virtual DateTime? OrderDate { get; set; }

        /// <summary>
        /// Плановая или внеплановая проверка
        /// </summary>
        public virtual bool IsScheduled { get; set; }

        /// <summary>
        /// Тип субъекта проверки
        /// </summary>
        public virtual ExaminationSubjectType SubjectType { get; set; }

        /// <summary>
        /// Котрагент ГИС
        /// </summary>
        public virtual RisContragent GisContragent { get; set; }

        /// <summary>
        /// Имя
        /// Заполняется, если тип субъекта проверки = Citizen
        /// </summary>
        public virtual string FirstName { get; set; }

        /// <summary>
        /// Фамилия
        /// Заполняется, если тип субъекта проверки = Citizen
        /// </summary>
        public virtual string LastName { get; set; }

        /// <summary>
        /// Отчество
        /// Заполняется, если тип субъекта проверки = Citizen
        /// </summary>
        public virtual string MiddleName { get; set; }

        /// <summary>
        /// Код вида осуществления контрольной деятельности (НСИ 65)
        /// </summary>
        public virtual string OversightActivitiesCode { get; set; }

        /// <summary>
        /// GUID вида осуществления контрольной деятельности (НСИ 65)
        /// </summary>
        public virtual string OversightActivitiesGuid { get; set; }

        /// <summary>
        /// Код основания проведения проверки. НСИ "Основание проведения проверки" (реестровый номер 68)
        /// </summary>
        public virtual string BaseCode { get; set; }

        /// <summary>
        /// GUID основания проведения проверки. НСИ "Основание проведения проверки" (реестровый номер 68)
        /// </summary>
        public virtual string BaseGuid { get; set; }

        /// <summary>
        /// Цель проведения проверки с реквизитами документов основания
        /// </summary>
        public virtual string Objective { get; set; }

        /// <summary>
        /// Срок проведения проверки с (включительно)
        /// </summary>
        public virtual DateTime? From { get; set; }

        /// <summary>
        /// Срок проведения проверки по (включительно)
        /// </summary>
        public virtual DateTime? To { get; set; }

        /// <summary>
        /// Срок проведения проверки. Количество рабочих дней
        /// </summary>
        public virtual double? Duration { get; set; }

        /// <summary>
        /// Задачи проведения проверки
        /// </summary>
        public virtual string Tasks { get; set; }

        /// <summary>
        /// Описание мероприятния проверки
        /// </summary>
        public virtual string EventDescription { get; set; }

        /// <summary>
        /// Имеется ли результат проверки
        /// </summary>
        public virtual bool HasResult { get; set; }

        /// <summary>
        /// Код вида документа результата проверки. НСИ "Вид документа по результатам проверки" (реестровый номер 64)
        /// </summary>
        public virtual string ResultDocumentTypeCode { get; set; }

        /// <summary>
        /// GUID вида документа результата проверки. НСИ "Вид документа по результатам проверки" (реестровый номер 64)
        /// </summary>
        public virtual string ResultDocumentTypeGuid { get; set; }

        /// <summary>
        /// Номер документа результата проверки
        /// </summary>
        public virtual string ResultDocumentNumber { get; set; }

        /// <summary>
        /// Дата и время составления документа результата проверки
        /// </summary>
        public virtual DateTime? ResultDocumentDateTime { get; set; }

        /// <summary>
        /// Результат проверки. True - нарушения выявлены, False - нарушений не выявлено
        /// </summary>
        public virtual bool HasOffence { get; set; }

        /// <summary>
        /// Учетный номер проверки в едином реестре проверок
        /// </summary>
        public virtual int? UriRegistrationNumber { get; set; }

        /// <summary>
        /// Дата присвоения учетного номера проверки
        /// </summary>
        public virtual DateTime? UriRegistrationDate { get; set; }

        /// <summary>
        /// Информация о согласовании проведения проверки с органами прокуратуры
        /// </summary>
        public virtual string ProsecutorAgreementInformation { get; set; }

        /// <summary>
        /// Проверка не должна быть зарегистрирована в Едином реестре проверок
        /// </summary>
        public virtual bool ShouldNotBeRegistered { get; set; }

        /// <summary>
        /// Реестровый номер функции органа жилищного надзора в системе «Федеральный реестр государственных и муниципальных услуг»
        /// </summary>
        public virtual string FunctionRegistryNumber { get; set; }

        /// <summary>
        /// ФИО и должность лиц, уполномоченных на проведение проверки
        /// </summary>
        public virtual string AuthorizedPersons { get; set; }

        /// <summary>
        /// ФИО и должность экспертов, привлекаемых к проведению проверки
        /// </summary>
        public virtual string InvolvedExperts { get; set; }

        /// <summary>
        /// Идентификатор предписания в ГИС ЖКХ
        /// </summary>
        public virtual string PreceptGuid { get; set; }

        /// <summary>
        /// Код НСИ "Предмет проверки" (реестровый номер 69)
        /// </summary>
        public virtual string ObjectCode { get; set; }

        /// <summary>
        /// GUID НСИ "Предмет проверки" (реестровый номер 69)
        /// </summary>
        public virtual string ObjectGuid { get; set; }

        /// <summary>
        /// Выявленные нарушения
        /// </summary>
        public virtual string IdentifiedOffences { get; set; }

        /// <summary>
        /// Дата и время начала проведения проверки
        /// </summary>
        public virtual DateTime? ResultFrom { get; set; }

        /// <summary>
        /// Дата окончания проведения проверки
        /// </summary>
        public virtual DateTime? ResultTo { get; set; }

        /// <summary>
        /// Место проведения проверки
        /// </summary>
        public virtual string ResultPlace { get; set; }

        /// <summary>
        /// Дата ознакомления
        /// </summary>
        public virtual DateTime? FamiliarizationDate { get; set; }

        /// <summary>
        /// Наличие подписи
        /// </summary>
        public virtual bool? IsSigned { get; set; }

        /// <summary>
        /// ФИО должностного лица, ознакомившегося с актом проверки
        /// </summary>
        public virtual string FamiliarizedPerson { get; set; }
    }
}