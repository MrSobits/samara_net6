namespace Bars.GkhDi.Entities
{
    using System;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Сведение о договорах в доме
    /// </summary>
    public class InformationOnContracts : BaseGkhEntity
    {
        /// <summary>
        /// Объект в управление
        /// </summary>
        public virtual DisclosureInfo DisclosureInfo { get; set; }

        /// <summary>
        /// Объект недвижимости
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Номер
        /// </summary>
        public virtual string Number { get; set; }

        /// <summary>
        /// от
        /// </summary>
        public virtual DateTime? From { get; set; }

        /// <summary>
        /// Дата начала
        /// </summary>
        public virtual DateTime? DateStart { get; set; }

        /// <summary>
        /// Дата окончания
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }

        /// <summary>
        /// стороны договора
        /// </summary>
        public virtual string PartiesContract { get; set; }

        /// <summary>
        /// стоимость
        /// </summary>
        public virtual decimal? Cost { get; set; }

        /// <summary>
        /// Примечания
        /// </summary>
        public virtual string Comments { get; set; }
    }
}
