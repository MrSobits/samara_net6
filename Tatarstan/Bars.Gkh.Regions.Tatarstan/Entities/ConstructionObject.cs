namespace Bars.Gkh.Regions.Tatarstan.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.B4.Modules.FIAS;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;

    public class ConstructionObject : BaseEntity, IStatefulEntity
    {
        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// МО - поселение
        /// </summary>
        public virtual Municipality MoSettlement { get; set; }

        /// <summary>
        /// Адрес ФИАС
        /// </summary>
        public virtual FiasAddress FiasAddress { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Сумма на СМР
        /// </summary>
        public virtual decimal? SumSmr { get; set; }

        /// <summary>
        /// Сумма на разработку экспертизы ПСД
        /// </summary>
        public virtual decimal? SumDevolopmentPsd { get; set; }

        /// <summary>
        /// Дата завершения работ подрядчиком
        /// </summary>
        public virtual DateTime? DateEndBuilder { get; set; }

        /// <summary>
        /// Дата начала работ
        /// </summary>
        public virtual DateTime? DateStartWork { get; set; }

        /// <summary>
        /// Дата остановки работ
        /// </summary>
        public virtual DateTime? DateStopWork { get; set; }

        /// <summary>
        /// Дата возобновления работ
        /// </summary>
        public virtual DateTime? DateResumeWork { get; set; }

        /// <summary>
        /// Причина остановки работ
        /// </summary>
        public virtual string ReasonStopWork { get; set; }

        /// <summary>
        /// Дата сдачи в эксплуатацию
        /// </summary>
        public virtual DateTime? DateCommissioning { get; set; }

        /// <summary>
        /// Лимит на дом
        /// </summary>
        public virtual decimal? LimitOnHouse { get; set; }

        /// <summary>
        /// Общая площадь дома
        /// </summary>
        public virtual decimal? TotalArea { get; set; }

        /// <summary>
        /// Количество квартир всего
        /// </summary>
        public virtual int? NumberApartments { get; set; }

        /// <summary>
        /// Количество квартир по программе переселения
        /// </summary>
        public virtual int? ResettleProgNumberApartments { get; set; }

        /// <summary>
        /// Количетсво этажей
        /// </summary>
        public virtual int? NumberFloors { get; set; }

        /// <summary>
        /// Количество подъездов
        /// </summary>
        public virtual int? NumberEntrances { get; set; }

        /// <summary>
        /// Количество лифтов
        /// </summary>
        public virtual int? NumberLifts { get; set; }

        /// <summary>
        /// Материал кровли
        /// </summary>
        public virtual RoofingMaterial RoofingMaterial { get; set; }

        /// <summary>
        /// Тип кровли
        /// </summary>
        public virtual TypeRoof TypeRoof { get; set; }

        /// <summary>
        /// Материал стен
        /// </summary>
        public virtual WallMaterial WallMaterial { get; set; }

		/// <summary>
		/// Программа переселения
		/// </summary>
        public virtual ResettlementProgram ResettlementProgram { get; set; }
    }
}