using Bars.Gkh.Enums;
using Bars.GkhGji.Enums;

namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;

    /*
     * Данная вьюха предназначена для реестра требований прокуратуры
     * чтобы получать количественные и Агрегированные показатели:
     * номер главного распоряжения,
     * типы обследований главного распоряжения через запятую,
     * ФИО инспекторов,
     * количество жилых домов,
     * идентификаторы жилых домов в строке вида /1/2/4/,
     * адреса жилых домов через ";"
    */
    public class ViewBaseProsClaim : PersistentObject
    {
        /// <summary>
        /// Инспекторы
        /// </summary>
        public virtual string InspectorNames { get; set; }

        /// <summary>
        /// Количество домов
        /// </summary>
        public virtual int? RealityObjectCount { get; set; }

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
        /// Юридическое лицо
        /// </summary>
        public virtual string ContragentName { get; set; }

        /// <summary>
        /// Дата проверки
        /// </summary>
        public virtual DateTime? ProsClaimDateCheck { get; set; }

        /// <summary>
        /// номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// номер проверки
        /// </summary>
        public virtual string InspectionNumber { get; set; }

        /// <summary>
        /// Объект проверки
        /// </summary>
        public virtual PersonInspection PersonInspection { get; set; }

        /// <summary>
        /// Тип контрагента
        /// </summary>
        public virtual TypeJurPerson? TypeJurPerson { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }
    }
}