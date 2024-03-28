namespace Bars.GkhGji.Entities
{
    using System;

    using B4.DataAccess;
    using B4.Modules.States;
    using Bars.Gkh.Entities;
    using Enums;

    /*
     * Данная вьюха прденазначена для реестра распоряжений 
     * чтобы получать количественные и Агрегированные показатели:
     * ФИО инспекторов,
     * типы обследования через запятую
     * количество жилых домов по которым составлен акт, 
     * создан ли на основе распоряжения акт проверки общий
     * идентификаторы жилых домов в строке вида /1/2/4/ для фильтрации в реестре документов
     */
    public class ViewDisposal : PersistentObject
    {
        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }
        
        /// <summary>
        /// Наименования населенных пунктов жилых домов
        /// </summary>
        public virtual string PlaceNames { get; set; }
        
        /// <summary>
        /// Наименования муниципальных образований жилых домов
        /// </summary>
        public virtual string MoNames { get; set; }

        /// <summary>
        /// Распоряжение
        /// </summary>
        public virtual long? DisposalGjiId { get; set; }

        /// <summary>
        /// Типы обследования
        /// </summary>
        public virtual string TypeSurveyNames { get; set; }

        /// <summary>
        /// ФИО инспекторов
        /// </summary>
        public virtual string InspectorNames { get; set; }

        /// <summary>
        /// ФИО инспекторов
        /// </summary>
        public virtual string IssuedDisposal { get; set; }

        /// <summary>
        /// Количество домов
        /// </summary>
        public virtual int? RealityObjectCount { get; set; }

        /// <summary>
        /// Создан акт проверки общий
        /// </summary>
        public virtual bool? IsActCheckExist { get; set; }

        /// <summary>
        /// строка идентификаторов жилых домов вида /1/2/4/ 
        /// </summary>
        public virtual string RealityObjectIds { get; set; }

        /// <summary>
        /// Адреса жилых домов
        /// </summary>
        /// <remarks>Проверяемые дома в инспекционной проверке ГЖИ (разделитель - ';')</remarks>
        public virtual string RealityObjectAddresses { get; set; }

        /// <summary>
        /// Наименования муниципальных образований жилых домов
        /// </summary>
        public virtual string MunicipalityNames { get; set; }

        /// <summary>
        /// Муниципальное образование первого жилого дома
        /// </summary>
        public virtual long? MunicipalityId { get; set; }

        /// <summary>
        /// Дата начала обследования
        /// </summary>
        public virtual DateTime? DateStart { get; set; }

        /// <summary>
        /// Дата окончания обследования
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Номер документа (целая часть)
        /// </summary>
        public virtual int? DocumentNum { get; set; }

        /// <summary>
        /// Тип основания проверки
        /// </summary>
        public virtual TypeBase TypeBase { get; set; }

        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual string ContragentName { get; set; }

        /// <summary>
        /// Контрагент МО Id 
        /// </summary>
        public virtual long? ContragentMuId { get; set; }

        /// <summary>
        /// Контрагент МО Name
        /// </summary>
        public virtual string ContragentMuName { get; set; }

        /// <summary>
        /// Основание проверки
        /// </summary>
        public virtual long? InspectionId { get; set; }

        /// <summary>
        /// Наименование вида проверки
        /// </summary>
        public virtual string KindCheckName { get; set; }

        /// <summary>
        /// Тип документа ГЖИ
        /// </summary>
        public virtual TypeDocumentGji TypeDocumentGji { get; set; }

        /// <summary>
        /// Тип документа ГЖИ
        /// </summary>
        public virtual TypeDisposalGji TypeDisposal { get; set; }

        /// <summary>
        /// Тип согласования с прокуратурой
        /// </summary>
        public virtual TypeAgreementProsecutor TypeAgreementProsecutor { get; set; }

        /// <summary>
        /// Вид контроля
        /// </summary>
        public virtual ControlType? ControlType { get; set; }

        /// <summary>
        /// Уведомление отправлено (Уведомление о проверке)
        /// </summary>
        public virtual KindKNDGJI KindKNDGJI { get; set; }

        /// <summary>
        /// Ид проверки в ЕРП
        /// </summary>
        public virtual string ERPID { get; set; }
        
        /// Лицензия УО
        /// </summary>
        public virtual ManOrgLicense License { get; set; }

        /// <summary>
        /// Флаг наличия акта обследования
        /// </summary>
        public virtual bool HasActSurvey { get; set; }
    }
}