namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Enums;
    using Enums.SMEV;
    using System;

    public class SMEVFNSLicRequest : BaseEntity
    {
        /// <summary>
        /// Инициатор запроса
        /// </summary>
        public virtual Inspector Inspector { get; set; }

        /// <summary>
        /// дата запроса
        /// </summary>
        public virtual DateTime CalcDate { get; set; }

        /// <summary>
        /// Лицензия
        /// </summary>
        public virtual ManOrgLicense ManOrgLicense { get; set; }

        /// <summary>
        /// Тип запроса по начислению
        /// </summary>
        public virtual FNSLicRequestType FNSLicRequestType { get; set; }

        /// <summary>
        /// Тип лицензиата
        /// </summary>
        public virtual FNSLicPersonType FNSLicPersonType { get; set; }

        // остальное для ответа
        /// <summary>
        /// ИНН
        /// </summary>
        public virtual string INN { get; set; }

        /// <summary>
        /// НаимЮЛПолн
        /// </summary>
        public virtual string NameUL { get; set; }

        /// <summary>
        /// ОГРН
        /// </summary>
        public virtual string OGRN { get; set; }

        /// <summary>
        /// FirstName
        /// </summary>
        public virtual string FirstName { get; set; }

        /// <summary>
        /// FamilyName
        /// </summary>
        public virtual string FamilyName { get; set; }

        /// <summary>
        /// ВидЛиц
        /// </summary>
        public virtual string KindLic { get; set; }

        /// <summary>
        /// ДатаЛиц 
        /// </summary>
        public virtual DateTime? DateLic { get; set; }

        /// <summary>
        /// ДатаНачЛиц 
        /// </summary>
        public virtual DateTime? DateStartLic { get; set; }

        /// <summary>
        /// ДатаОкончЛиц 
        /// </summary>
        public virtual DateTime? DateEndLic { get; set; }

        /// <summary>
        /// НомЛиц
        /// </summary>
        public virtual string NumLic { get; set; }

        /// <summary>
        /// СерЛиц
        /// </summary>
        public virtual string SerLic { get; set; }

        /// <summary>
        /// КодСЛВД
        /// </summary>
        public virtual string SLVDCode { get; set; }

        /// <summary>
        /// НаимВД
        /// </summary>
        public virtual string VDName { get; set; }

        /// <summary>
        /// ПрДейств
        /// </summary>
        public virtual string PrAction { get; set; }

        /// <summary>
        /// АдресТекст 
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// ВидРеш
        /// </summary>
        public virtual string DecisionKind { get; set; }

        /// <summary>
        /// ДатаНачРеш
        /// </summary>
        public virtual DateTime? DecisionDateStart { get; set; }

        /// <summary>
        /// ДатаОкончРеш
        /// </summary>
        public virtual DateTime? DecisionDateEnd { get; set; }


        /// <summary>
        /// ДатаРеш
        /// </summary>
        public virtual DateTime? DecisionDate { get; set; }

        /// <summary>
        /// НомРеш
        /// </summary>
        public virtual string DecisionNum { get; set; }

        /// <summary>
        /// ЛицОргРеш
        /// </summary>
        public virtual string DecisionOrgLic { get; set; }

        /// <summary>
        /// ИННЛО
        /// </summary>
        public virtual string LicOrgINN { get; set; }

        /// <summary>
        /// НаимЛОПолн
        /// </summary>
        public virtual string LicOrgFullName { get; set; }

        /// <summary>
        /// НаимЛОСокр
        /// </summary>
        public virtual string LicOrgShortName { get; set; }

        /// <summary>
        /// ОГРНЛО
        /// </summary>
        public virtual string LicOrgOGRN { get; set; }

        /// <summary>
        /// ОКОГУ
        /// </summary>
        public virtual string LicOrgOKOGU { get; set; }

        /// <summary>
        /// Регион
        /// </summary>
        public virtual string LicOrgRegion { get; set; }

        /// <summary>
        /// Ответ
        /// </summary>
        public virtual string Answer { get; set; }

        /// <summary>
        /// Статус запроса
        /// </summary>
        public virtual RequestState RequestState { get; set; }

        /// <summary>
        /// MessageId
        /// </summary>
        public virtual string MessageId { get; set; }

        /// <summary>
        /// IdDoc
        /// </summary>
        public virtual string IdDoc { get; set; }

        /// <summary>
        /// DeleteIdDoc
        /// </summary>
        public virtual string DeleteIdDoc { get; set; }

        /// <summary>
        /// DeleteIdDoc
        /// </summary>
        public virtual FNSLicDecisionType FNSLicDecisionType { get; set; }
        
    }
}
