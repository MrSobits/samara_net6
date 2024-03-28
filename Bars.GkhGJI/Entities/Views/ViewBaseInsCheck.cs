namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.GkhGji.Enums;

    /*
     * Данная вьюха предназначена для реестра плановых инспекционных проверок
     * чтобы получить количественные а агрегированные показатели:
     * номер главного распоряжения,
     * типы обследований главного распоряжения через запятую,
     * ФИО инспекторов,
     * количество жилых домов,
     * адреса жилых домов через ";"
     */
    public class ViewBaseInsCheck : PersistentObject
    {
        /// <summary>
        /// Номер распоряжения
        /// </summary>
        public virtual string DisposalNumber { get; set; }

        /// <summary>
        /// Инспекторы
        /// </summary>
        public virtual string InspectorNames { get; set; }

        /// <summary>
        /// Количество домов
        /// </summary>
        public virtual int? RealityObjectCount { get; set; }

        /// <summary>
        /// Адреса домов разделенных ';'
        /// </summary>
        public virtual string RealityObjectAddress { get; set; }

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
        /// Мунниципальное образование первого жилого дома
        /// </summary>
        public virtual long? MunicipalityId { get; set; }

        /// <summary>
        /// Идентификатор плана
        /// </summary>
        public virtual long? PlanId { get; set; }

        /// <summary>
        /// Наименование плана
        /// </summary>
        public virtual string PlanName { get; set; }

        /// <summary>
        /// Номер проверки
        /// </summary>
        public virtual string InspectionNumber { get; set; }

        /// <summary>
        /// Факт проверки
        /// </summary>
        public virtual TypeFactInspection TypeFact { get; set; }

        /// <summary>
        /// Юридическое лицо
        /// </summary>
        public virtual string ContragentName { get; set; }

        /// <summary>
        /// Дата проверки
        /// </summary>
        public virtual DateTime? InsCheckDate { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }
    }
}