namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Enums;

    /*
     * вьюха предназначенная для реестра Предписаний с 
     * количественными и агрегированными показателями:
     * ФИО инспекторов,
     * количество жилых домов
     * количество нарушений, по которым создано предписание
     * идентификаторы жилых домов, по которым найдены нарушения, вида /1/2/4/ для фильтрации в реестре документов
    */
    public class ViewPrescription : PersistentObject
    {
        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Предписание
        /// </summary>
        public virtual long? PrescriptionGjiId { get; set; }

        /// <summary>
        /// Инспекторы
        /// </summary>
        public virtual string InspectorNames { get; set; }

        /// <summary>
        /// Количество домов
        /// </summary>
        public virtual int? CountRealityObject { get; set; }

        /// <summary>
        /// Количество нарушений
        /// </summary>
        public virtual int? CountViolation { get; set; }

        /// <summary>
        /// строка идентификаторов жилых домов вида /1/2/4/ 
        /// </summary>
        public virtual string RealityObjectIds { get; set; }
        
        /// <summary>
        /// Адреса жилых домов
        /// </summary>
        /// <remarks>Из этапов нарушений (разделитель - ';')</remarks>
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
        /// Наименования муниципальных образований жилых домов
        /// </summary>
        public virtual string MoNames { get; set; }

        /// <summary>
        /// Наименования населенных пунктов жилых домов
        /// </summary>
        public virtual string PlaceNames { get; set; }

        /// <summary>
        /// Дата дкоумента
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Целая часть номер документа
        /// </summary>
        public virtual int? DocumentNum { get; set; }

        /// <summary>
        /// Контрагент (Исполнитель)
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
        /// Тип исполнителя
        /// </summary>
        public virtual string TypeExecutant { get; set; }

        /// <summary>
        /// Тип основания проверки
        /// </summary>
        public virtual TypeBase TypeBase { get; set; }

        /// <summary>
        /// Идентификатор Основания проверки
        /// </summary>
        public virtual long? InspectionId { get; set; }

        /// <summary>
        /// Тип документа ГЖИ
        /// </summary>
        public virtual TypeDocumentGji TypeDocumentGji { get; set; }

        /// <summary>
        /// Дата самого позднего срока устранения нарушений (во вкладке "Нарушения")
        /// </summary>
        public virtual DateTime? DateRemoval { get; set; }

        /// <summary>
        /// Распоряжение на проверку этого предписания
        /// </summary>
        public virtual long? DisposalId { get; set; }

        /// <summary>
        /// Список нарушений
        /// </summary>
        public virtual string ViolationList { get; set; }

        /// <summary>
        /// Номер обращения
        /// </summary>
        public virtual string AppealNumber { get; set; }

        /// <summary>
        /// INN contragent
        /// </summary>
        public virtual string INN { get; set; }

        /// <summary>
        /// Описание обращения
        /// </summary>
        public virtual string AppealDescription { get; set; }

        /// <summary>
        /// Дата обращения
        /// </summary>
        public virtual DateTime? AppealDate { get; set; }

        /// <summary>
        /// Дата обращения
        /// </summary>
        public virtual bool HasNotRemoovedViolations { get; set; }

        /// <summary>
        /// Вид контроля(надзора)
        /// </summary>
        public virtual ControlType ControlType { get; set; }

        /// <summary>
        /// Вид контроля(надзора)
        /// </summary>
        public virtual TypePrescriptionExecution TypePrescriptionExecution { get; set; }

        /// <summary>
        /// Вид контроля(надзора)
        /// </summary>
        public virtual bool CancelledGJI { get; set; }

        /// <summary>
        /// Вид контроля(надзора)
        /// </summary>
        public virtual PrescriptionState PrescriptionState { get; set; }
    }
}