namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.GkhGji.Enums;

    /*
     * Вьюха предназначена для реестра протоколов 
     * с количественными и агрегированными показателями:
     * количество нарушений по которым создан протокол
     * фио инспекторов
     * строка идентификаторов жилых домов вида /1/2/4 для фильтрации в реестре документов
     */
    public class ViewProtocol : PersistentObject
    {
        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Протокол
        /// </summary>
        public virtual long? ProtocolGjiId { get; set; }

        /// <summary>
        /// Количество нарушений
        /// </summary>
        public virtual int? CountViolation { get; set; }

        /// <summary>
        /// Инспекторы
        /// </summary>
        public virtual string InspectorNames { get; set; }

        /// <summary>
        /// строка идентификаторов жилых домов вида /1/2/4/ 
        /// </summary>
        public virtual string RealityObjectIds { get; set; }
        
        /// <summary>
        /// Адреса жилых домов
        /// </summary>
        /// <remarks>Из этапов нарушений по протоколу (разделитель - ';')</remarks>
        public virtual string RealityObjectAddresses { get; set; }

        /// <summary>
        /// Наименования муниципальных образований жилых домов
        /// </summary>
        public virtual string MunicipalityNames { get; set; }

        /// <summary>
        /// Наименования муниципальных образований жилых домов
        /// </summary>
        public virtual string MoNames { get; set; }

        /// <summary>
        /// Наименования населенных пунктов жилых домов
        /// </summary>
        public virtual string PlaceNames { get; set; }

        /// <summary>
        /// Муниципальное образование первого жилого дома
        /// </summary>
        public virtual long? MunicipalityId { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Целая часть номер документа
        /// </summary>
        public virtual int? DocumentNum { get; set; }

        /// <summary>
        /// Контрагент МО Id 
        /// </summary>
        public virtual long? ContragentMuId { get; set; }

        /// <summary>
        /// Контрагент МО Name
        /// </summary>
        public virtual string ContragentMuName { get; set; }

        /// <summary>
        /// Контрагент (исполнитель)
        /// </summary>
        public virtual string ContragentName { get; set; }

        /// <summary>
        /// Тип исполнителя
        /// </summary>
        public virtual string TypeExecutant { get; set; }

        /// <summary>
        /// Идентификатор основания проверки
        /// </summary>
        public virtual long? InspectionId { get; set; }

        /// <summary>
        /// Тип основания проверки
        /// </summary>
        public virtual TypeBase TypeBase { get; set; }

        /// <summary>
        /// Тип документа ГЖИ
        /// </summary>
        public virtual TypeDocumentGji TypeDocumentGji { get; set; }

        /// <summary>
        /// УИН
        /// </summary>
        public virtual string UIN { get; set; }

        /// <summary>
        /// Вид контроля(надзора)
        /// </summary>
        public virtual ControlType ControlType { get; set; }

        /// <summary>
        /// Статьи закона
        /// </summary>
        public virtual string ArticleLaw { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DateToCourt { get; set; }

        /// <summary>
        /// Часы времени составления
        /// </summary>
        public virtual int? FormatHour { get; set; }

        /// <summary>
        /// Минуты времени составления
        /// </summary>
        public virtual int? FormatMinute { get; set; }

    }
}