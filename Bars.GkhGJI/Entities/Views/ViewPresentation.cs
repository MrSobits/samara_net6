namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.GkhGji.Enums;

    /*
     Вьюха документов представлений с количественными и агрегированными показателями:
     * строка идентификаторов жилых домов вида /1/2/4/ для фильтрации в реестре документов
    */
    public class ViewPresentation : PersistentObject
    {
        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Представление
        /// </summary>
        public virtual long? PresentationGjiId { get; set; }

        /// <summary>
        /// Строка идентификаторов жилых домов вида /1/2/4/ 
        /// </summary>
        public virtual string RealityObjectIds { get; set; }

        /// <summary>
        /// Дата документа
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
        /// Физическое лицо
        /// </summary>
        public virtual string PhysicalPerson { get; set; }

        /// <summary>
        /// Контрагент (исполнитель)
        /// </summary>
        public virtual string ContragentName { get; set; }

        /// <summary>
        /// Тип основания проверки
        /// </summary>
        public virtual TypeBase TypeBase { get; set; }

        /// <summary>
        /// Идентификатор основания проверки
        /// </summary>
        public virtual long? InspectionId { get; set; }

        /// <summary>
        /// Кем вынесено
        /// </summary>
        public virtual TypeInitiativeOrgGji TypeInitiativeOrg { get; set; }

        /// <summary>
        /// Наименование муниципального образования контрагента
        /// </summary>
        public virtual string MunicipalityName { get; set; }

        /// <summary>
        /// Наименования муниципальных образований жилых домов
        /// </summary>
        public virtual string MoNames { get; set; }

        /// <summary>
        /// Наименования населенных пунктов жилых домов
        /// </summary>
        public virtual string PlaceNames { get; set; }

        /// <summary>
        /// Идентификатор муниципального образования контрагента
        /// </summary>
        public virtual long? MunicipalityId { get; set; }

        /// <summary>
        /// Идентификатор ДЛ вынесшего представление
        /// </summary>
        public virtual long? OfficialId { get; set; }

        /// <summary>
        /// Тип документа ГЖИ
        /// </summary>
        public virtual TypeDocumentGji TypeDocumentGji { get; set; }
    }
}