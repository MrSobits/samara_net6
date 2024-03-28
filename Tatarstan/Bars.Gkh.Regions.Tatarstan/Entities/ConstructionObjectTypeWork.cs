namespace Bars.Gkh.Regions.Tatarstan.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Вид работы КР
    /// </summary>
    public class ConstructionObjectTypeWork : BaseEntity
    {
        /// <summary>
        /// Объект капитального ремонта
        /// </summary>
        public virtual ConstructionObject ConstructionObject { get; set; }

        /// <summary>
        /// Вид работы
        /// </summary>
        public virtual Work Work { get; set; }

        /// <summary>
        /// Год строительства
        /// </summary>
        public virtual int YearBuilding { get; set; }

        /// <summary>
        /// Наличие ПСД
        /// </summary>
        public virtual bool HasPsd { get; set; }

        /// <summary>
        /// Наличие экспертизы
        /// </summary>
        public virtual bool HasExpertise { get; set; }

        /// <summary>
        /// Объем (плановый)
        /// </summary>
        public virtual decimal? Volume { get; set; }

        /// <summary>
        /// Сумма (плановая)
        /// </summary>
        public virtual decimal? Sum { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Дата начала работ
        /// </summary>
        public virtual DateTime? DateStartWork { get; set; }

        /// <summary>
        /// Дата окончания работ
        /// </summary>
        public virtual DateTime? DateEndWork { get; set; }

        /// <summary>
        /// Объем выполнения
        /// </summary>
        public virtual decimal? VolumeOfCompletion { get; set; }

        /// <summary>
        /// Процент выполнения
        /// </summary>
        public virtual decimal? PercentOfCompletion { get; set; }

        /// <summary>
        /// Сумма расходов
        /// </summary>
        public virtual decimal? CostSum { get; set; }

        /// <summary>
        /// Чиленность рабочих(дробное так как м.б. смысл поля как пол ставки)
        /// </summary>
        public virtual decimal? CountWorker { get; set; }

		/// <summary>
		/// Контрольный срок
		/// </summary>
		public virtual DateTime? Deadline { get; set; }
    }
}