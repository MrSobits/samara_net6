namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Enums;

    /*
     * Данная вьюха прденазначена для реестра Актов провекри 
     * чтобы получать количественные и Агрегированные показатели:
     * ФИО инспекторов, 
     * количество жилых домов по которым составлен акт, 
     * количество исполнительных документов, созданных из акта,
     * идентификаторы жилых домов в строке вида /1/2/4/ для фильтрации в реестре документов
     */
    public class ViewActCheck : PersistentObject
    {
        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Документ ГЖИ
        /// </summary>
        public virtual long? ActCheckGjiId { get; set; }

        /// <summary>
        /// Инспекторы
        /// </summary>
        public virtual string InspectorNames { get; set; }

        /// <summary>
        /// Количество домов
        /// </summary>
        public virtual int? RealityObjectCount { get; set; }

        /// <summary>
        /// Нарушения выявлены
        /// </summary>
        public virtual YesNoNotSet HasViolation { get; set; }

        /// <summary>
        /// Количество исполнительных документов
        /// </summary>
        public virtual int? CountExecDoc { get; set; }

        /// <summary>
        /// строка идентификаторов жилых домов вида /1/2/4/ 
        /// </summary>
        public virtual string RealityObjectIds { get; set; }

        /// <summary>
        /// Адреса жилых домов
        /// </summary>
        /// <remarks>Из результатов проверки акта проверки (разделитель - ';')</remarks>
        public virtual string RealityObjectAddresses { get; set; }

        /// <summary>
        /// Наименования муниципальных образований жилых домов
        /// </summary>
        public virtual string MunicipalityNames { get; set; }

        /// <summary>
        /// Мунниципальное образование первого жилого дома
        /// </summary>
        public virtual long? MunicipalityId { get; set; }

        /// <summary>
        /// Идентификатор основания проверки
        /// </summary>
        public virtual long? InspectionId { get; set; }

        /// <summary>
        /// Тип основания
        /// </summary>
        public virtual TypeBase TypeBase { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Целая часть номера документа
        /// </summary>
        public virtual int? DocumentNum { get; set; }

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
        /// Тип документа ГЖИ
        /// </summary>
        public virtual TypeDocumentGji TypeDocumentGji { get; set; }

        /// <summary>
        /// Вид контроля(надзора)
        /// </summary>
        public virtual ControlType ControlType { get; set; }
    }
}