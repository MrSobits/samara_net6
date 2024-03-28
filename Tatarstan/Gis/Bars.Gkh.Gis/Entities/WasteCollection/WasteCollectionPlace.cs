namespace Bars.Gkh.Gis.Entities.WasteCollection
{
    using System;
    using B4.DataAccess;
    using B4.Modules.FileStorage;
    using Enum;
    using Gkh.Entities;

    /// <summary>
    /// Площадка сбора ТБО и ЖБО
    /// </summary>
    public class WasteCollectionPlace : BaseEntity
    {
        /// <summary>
        /// Адрес
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Компания-заказчик
        /// </summary>
        public virtual Contragent Customer { get; set; }

        /// <summary>
        /// Тип хранимых бытовых отходов
        /// </summary>
        public virtual TypeWaste TypeWaste { get; set; }

        /// <summary>
        /// Тип объекта
        /// </summary>
        public virtual TypeWasteCollectionPlace TypeWasteCollectionPlace { get; set; }

        /// <summary>
        /// Количество населения, чел.
        /// </summary>
        public virtual int? PeopleCount { get; set; }

        /// <summary>
        /// Количество контейнеров, шт.
        /// </summary>
        public virtual int? ContainersCount { get; set; }

        /// <summary>
        /// Накопление ТБО в сутки, кг.
        /// </summary>
        public virtual int? WasteAccumulationDaily { get; set; }

        /// <summary>
        /// Расстояние от полигона, км.
        /// </summary>
        public virtual int? LandfillDistance { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Comment { get; set; }

        /// <summary>
        /// Зима
        /// </summary>
        public virtual ExportWasteDays ExportDaysWinter { get; set; }

        /// <summary>
        /// Лето
        /// </summary>
        public virtual ExportWasteDays ExportDaysSummer { get; set; }

        /// <summary>
        /// Компания-подрядчик
        /// </summary>
        public virtual Contragent Contractor { get; set; }

        /// <summary>
        /// Номер договора
        /// </summary>
        public virtual string NumberContract { get; set; }

        /// <summary>
        /// Дата договора
        /// </summary>
        public virtual DateTime? DateContract { get; set; }

        /// <summary>
        /// Файл договора
        /// </summary>
        public virtual FileInfo FileContract { get; set; }

        /// <summary>
        /// Адрес полигона
        /// </summary>
        public virtual string LandfillAddress { get; set; }
    }

    public class ExportWasteDays
    {
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
        public bool All { get; set; }
    }
}