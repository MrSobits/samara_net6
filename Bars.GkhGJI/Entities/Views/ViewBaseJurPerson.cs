namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.GkhGji.Enums;

    /*
     * Данная вьюха прденазначена для реестра плановых проверок по юр лицам 
     * чтобы получать количественные и Агрегированные показатели:
     * номер главного распоряжения,
     * типы обследований главного распоряжения через запятую,
     * ФИО инспекторов,
     * количество жилых домов,
     * идентификаторы жилых домов в строке вида /1/2/4/,
     * адреса жилых домов через ";"
     */
    public class ViewBaseJurPerson : PersistentObject
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
        /// Отделы
        /// </summary>
        public virtual string ZonalInspectionNames { get; set; }

        /// <summary>
        /// Количество домов
        /// </summary>
        public virtual int? RealityObjectCount { get; set; }

        /// <summary>
        /// Строка идентификаторов жилых домов вида /1/2/4/ 
        /// </summary>
        public virtual string RealityObjectIds { get; set; }

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
        /// Идентификатор плана проверок
        /// </summary>
        public virtual long? PlanId { get; set; }

        /// <summary>
        /// Наименование плана проверок
        /// </summary>
        public virtual string PlanName { get; set; }

        /// <summary>
        /// Муниципальное образование контрагента
        /// </summary>
        public virtual string ContragentName { get; set; }

        /// <summary>
        /// Факт проверки ЮЛ
        /// </summary>
        public virtual TypeFactInspection TypeFact { get; set; }

        /// <summary>
        /// Количество дней проверки
        /// </summary>
        public virtual int? CountDays { get; set; }

        /// <summary>
        /// Дата начала проверки
        /// </summary>
        public virtual DateTime? DateStart { get; set; }

        /// <summary>
        /// Номер проверки
        /// </summary>
        public virtual string InspectionNumber { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }
    }
}