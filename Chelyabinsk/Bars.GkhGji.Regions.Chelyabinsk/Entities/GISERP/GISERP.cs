using Bars.B4.DataAccess;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Regions.Chelyabinsk.Enums;
using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
using System;

namespace Bars.GkhGji.Regions.Chelyabinsk.Entities
{
    public class GISERP : BaseEntity
    {
        /// <summary>
        /// ID сообщения в системе СМЭВ
        /// </summary>
        public virtual string MessageId { get; set; }

        /// <summary>
        /// ID проверки
        /// </summary>
        public virtual string checkId { get; set; }

        /// <summary>
        /// Тип запроса в гис ЕРП
        /// </summary>
        public virtual GisErpRequestType GisErpRequestType { get; set; }

        /// <summary>
        /// Текущее состояние запроса
        /// </summary>
        public virtual RequestState RequestState { get; set; }

        /// <summary>
        /// Тип проверки
        /// </summary>
        public virtual ERPInspectionType ERPInspectionType { get; set; }

        /// <summary>
        /// Тип проверки
        /// </summary>
        public virtual KindKND KindKND { get; set; }

        /// <summary>
        /// Ответ от сервера
        /// </summary>
        public virtual String Answer { get; set; }

        /// <summary>
        /// Тип адреса
        /// </summary>
        public virtual ERPAddressType ERPAddressType { get; set; }

        /// <summary>
        /// Простой тип - коды способов уведомления
        /// </summary>
        public virtual ERPNoticeType ERPNoticeType { get; set; }

        /// <summary>
        /// Простой тип - коды типов объектов проведения проверки
        /// </summary>
        public virtual ERPObjectType ERPObjectType { get; set; }

        /// <summary>
        /// Простой тип - коды оснований проведения проверки
        /// </summary>
        public virtual ERPReasonType ERPReasonType { get; set; }

        /// <summary>
        /// Простой тип - коды категорий риска
        /// </summary>
        public virtual ERPRiskType ERPRiskType { get; set; }

        /// <summary>
        /// Простой тип - коды категорий риска
        /// </summary>
        public virtual ProsecutorOffice ProsecutorOffice { get; set; }

        /// <summary>
        /// ОКАТО
        /// </summary>
        public virtual String OKATO { get; set; }

        /// <summary>
        /// ОКАТО
        /// </summary>
        public virtual String RegistryDisposalNumber { get; set; }

        /// <summary>
        /// Адрес субъекта проверки
        /// </summary>
        public virtual String SubjectAddress { get; set; }

        /// <summary>
        /// Инициатор запроса
        /// </summary>
        public virtual Inspector Inspector { get; set; }

        /// <summary>
        /// дата запроса
        /// </summary>
        public virtual DateTime RequestDate { get; set; }

        /// <summary>
        /// Распоряжение основание
        /// </summary>
        public virtual DocumentGji Disposal { get; set; }

        /// <summary>
        /// Мероприятия проверки
        /// </summary>
        public virtual String CarryoutEvents { get; set; }

        /// <summary>
        /// Наименование проверки
        /// </summary>
        public virtual String InspectionName { get; set; }

        /// <summary>
        /// Цели и задачи проверки
        /// </summary>
        public virtual String Goals { get; set; }

        /// <summary>
        /// Дата и время составления акта о проведении проверки (КНМ)
        /// </summary>
        public virtual DateTime? ACT_DATE_CREATE { get; set; }

        /// <summary>
        /// ФИО руководителя, иного должностного лица юридического лица присутствовавшего на проверке
        /// </summary>
        public virtual String REPRESENTATIVE_FULL_NAME { get; set; }

        /// <summary>
        /// Должность руководителя, иного должностного лица юридического лица присутствовавшего на проверке
        /// </summary>
        public virtual String REPRESENTATIVE_POSITION { get; set; }

        /// <summary>
        /// Дата и время проведения проверки
        /// </summary>
        public virtual DateTime? START_DATE { get; set; }

        /// <summary>
        /// Длительность КНМ (в часах)
        /// </summary>
        public virtual int? DURATION_HOURS { get; set; }

        /// <summary>
        /// Выявлены нарушения
        /// </summary>
        public virtual YesNoNotSet HasViolations { get; set; }

        /// <summary>
        /// Требуется коррекция
        /// </summary>
        public virtual YesNoNotSet NeedToUpdate { get; set; }

        /// <summary>
        /// гуид проверки в ЕРП
        /// </summary>
        public virtual String INSPECTION_GUID { get; set; }

        /// <summary>
        /// гуид проверяющего в ерп
        /// </summary>
        public virtual String INSPECTOR_GUID { get; set; }

        /// <summary>
        /// гуид объекта проверки в ерп
        /// </summary>
        public virtual String OBJECT_GUID { get; set; }

        /// <summary>
        /// гуид результата проверки в ерп
        /// </summary>
        public virtual String RESULT_GUID { get; set; }

        /// <summary>
        /// гуид результата проверки в ерп
        /// </summary>
        public virtual String ERPID { get; set; }

        /// <summary>
        /// гуид результата проверки в ерп
        /// </summary>
        public virtual String CTADDRESS { get; set; }


    }
}
