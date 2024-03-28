namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.GkhGji.Enums;

    /*
     * Данная вьюха предназначена для реестра Актов обследования чтобы получать количественные 
     * и Агрегированные показатели:
     * ФИО инспекторов, 
     * адреса и мун. образования жилых домов по которым составлен акт, 
     * идентификаторы жилых домов в строке вида /1/2/4/ для фильтрации в реестре документов
     */
    public class ViewActSurvey : PersistentObject
    {
        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Акт обследования
        /// </summary>
        public virtual long? ActSurveyGjiId { get; set; }

        /// <summary>
        /// Инспекторы
        /// </summary>
        public virtual string InspectorNames { get; set; }

        /// <summary>
        /// Адреса жилых домов
        /// </summary>
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
        /// Наименование муниципального образования
        /// </summary>
        public virtual long? MunicipalityId { get; set; }

        /// <summary>
        /// строка идентификаторов жилых домов вида /1/2/4/ 
        /// </summary>
        public virtual string RealityObjectIds { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Целая часть номера документа
        /// </summary>
        public virtual int? DocumentNum { get; set; }

        /// <summary>
        /// Идентификатор проверки
        /// </summary>
        public virtual long? InspectionId { get; set; }

        /// <summary>
        /// Тип основания проверки
        /// </summary>
        public virtual TypeBase TypeBase { get; set; }

        /// <summary>
        /// Результат обследования
        /// </summary>
        public virtual SurveyResult TypeSurvey { get; set; }

        /// <summary>
        /// Тип документа ГЖИ
        /// </summary>
        public virtual TypeDocumentGji TypeDocumentGji { get; set; }
    }
}