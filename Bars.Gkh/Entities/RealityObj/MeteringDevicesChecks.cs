namespace Bars.Gkh.Entities
{
    using System;

    using Bars.B4.Modules.States;

    /// <summary>
    /// Проверка прибора учёта
    /// </summary>
    public class MeteringDevicesChecks : BaseGkhEntity
    {
        /// <summary>
        /// Прибор учета жилого дома
        /// </summary>
        public virtual RealityObjectMeteringDevice MeteringDevice { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Контрольное показание
        /// </summary>
        public virtual long ControlReading { get; set; }

        /// <summary>
        /// Дата снятия контрольного показания
        /// </summary>
        public virtual DateTime? RemovalControlReadingDate { get; set; }

        /// <summary>
        /// Дата начала проверки
        /// </summary>
        public virtual DateTime? StartDateCheck { get; set; }

        /// <summary>
        /// Значение показаний прибора учета на момент начала проверки
        /// </summary>
        public virtual long StartValue { get; set; }

        /// <summary>
        /// Дата окончания проверки
        /// </summary>
        public virtual DateTime? EndDateCheck { get; set; }

        /// <summary>
        /// Значение показаний на момент окончания проверки
        /// </summary>
        public virtual long EndValue { get; set; }

        /// <summary>
        /// Марка прибора учёта
        /// </summary>
        public virtual string MarkMeteringDevice { get; set; }

        /// <summary>
        /// Межпроверочный интервал (лет)
        /// </summary>
        public virtual long? IntervalVerification { get; set; }

        /// <summary>
        /// Плановая дата следующей проверки
        /// </summary>
        public virtual DateTime? NextDateCheck { get; set; }
    }
}