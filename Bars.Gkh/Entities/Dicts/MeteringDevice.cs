namespace Bars.Gkh.Entities
{
    using Bars.Gkh.Enums;

    /// <summary>
    /// Прибор учета
    /// </summary>
    public class MeteringDevice : BaseGkhEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Класс точности
        /// </summary>
        public virtual string AccuracyClass { get; set; }

        /// <summary>
        /// Тип учета
        /// </summary>
        public virtual TypeAccounting TypeAccounting { get; set; }

        /// <summary>
        /// Стоимость замены
        /// </summary>
        public virtual decimal? ReplacementCost { get; set; }

        /// <summary>
        /// Срок эксплуатации
        /// </summary>
        public virtual int? LifeTime { get; set; }

        /// <summary>
        /// Гpуппа
        /// </summary>
        public virtual MeteringDeviceGroup Group { get; set; }
    }
}