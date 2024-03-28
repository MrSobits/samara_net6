namespace Bars.GisIntegration.Base.Entities.HouseManagement
{
    using System;

    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Лифт
    /// </summary>
    public class RisLift : BaseRisEntity 
    {
        /// <summary>
        /// Многоквартирный дом
        /// </summary>
        public virtual RisHouse ApartmentHouse { get; set; }

        /// <summary>
        /// Номер подъезда
        /// </summary>
        public virtual string EntranceNum { get; set; }

        /// <summary>
        /// Заводской номер
        /// </summary>
        public virtual string FactoryNum { get; set; }

        /// <summary>
        /// Предельный срок эксплуатации
        /// </summary>
        public virtual string OperatingLimit { get; set; } 

        /// <summary>
        /// Дата прекращения существования объекта
        /// </summary>
        public virtual DateTime? TerminationDate { get; set; }

        /// <summary>
        /// Код строки в справочнике Форма описания объектов ЖФ
        /// </summary>
        public virtual string OgfDataCode { get; set; }

        /// <summary>
        /// Значение показателя
        /// </summary>
        public virtual string OgfDataValue { get; set; }

        /// <summary>
        /// Код строки в справочнике Форма описания объектов ЖФ
        /// </summary>
        public virtual string TypeCode { get; set; }

        /// <summary>
        /// Значение показателя
        /// </summary>
        public virtual string TypeGuid { get; set; }
    }
}