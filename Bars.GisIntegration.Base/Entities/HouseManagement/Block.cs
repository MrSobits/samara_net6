namespace Bars.GisIntegration.Base.Entities.HouseManagement
{
    using System;
    using GisIntegration.Base.Entities;

    /// <summary>
    /// Блок (для ЖД блокированной застройки)
    /// </summary>
    public class Block : BaseRisEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RisHouse House { get; set; }

        /// <summary>
        /// Кадастровый номер
        /// </summary>
        public virtual string CadastralNumber { get; set; }

        /// <summary>
        /// Номер блока
        /// </summary>
        public virtual string BlockNum { get; set; }

        /// <summary>
        /// Характеристика помещения (НСИ 30) - код
        /// </summary>
        public virtual string PremisesCharacteristicCode { get; set; }

        /// <summary>
        /// Характеристика помещения (НСИ 30) - идентификатор
        /// </summary>
        public virtual string PremisesCharacteristicGuid { get; set; }

        /// <summary>
        /// Общая площадь жилого помещения по паспорту помещения
        /// </summary>
        public virtual decimal? TotalArea { get; set; }

        /// <summary>
        /// Жилая площадь жилого помещения по паспорту помещения
        /// </summary>
        public virtual decimal? GrossArea { get; set; }

        /// <summary>
        /// Дата прекращения существования объекта
        /// </summary>
        public virtual DateTime? TerminationDate { get; set; }
    }
}