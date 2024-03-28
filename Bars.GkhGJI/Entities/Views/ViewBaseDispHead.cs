namespace Bars.GkhGji.Entities
{
    using System;

    using B4.DataAccess;
    using B4.Modules.States;

    using Gkh.Enums;
    using Enums;

    /*
     * Данная вьюха прденазначена для реестра распоряжений руководства 
     * чтобы получать количественные и агрегированные показатели:
     * ФИО инспекторов,
     * количество жилых домов, по которым будет проведена проверка
     * строка идентификаторов жилых домов вида /1/2/4/
     * адреса жилых домов через ";"
     */
    public class ViewBaseDispHead : PersistentObject
    {
        /// <summary>
        /// Типы обследований
        /// </summary>
        public virtual string DisposalTypeSurveys { get; set; }

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
        /// ФИО руководителя
        /// </summary>
        public virtual string HeadFio { get; set; }

        /// <summary>
        /// Идентификатор руководителя
        /// </summary>
        public virtual long? HeadId { get; set; }

        /// <summary>
        /// Номер проверки
        /// </summary>
        public virtual string InspectionNumber { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Юридическое лицо
        /// </summary>
        public virtual string ContragentName { get; set; }

        /// <summary>
        /// Дата проверки
        /// </summary>
        public virtual DateTime? DispHeadDate { get; set; }

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