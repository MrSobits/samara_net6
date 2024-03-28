namespace Bars.GkhGji.Regions.Tomsk.Controller
{
    using System;
    using GkhGji.Entities;

    /// <summary>
    /// Протокол ГЖИ для Томск (расширяется дополнительными полями)
    /// </summary>
    public class TomskProtocol : Protocol
    {
        /// <summary>
        /// Нарушения - Дата правонарушения
        /// </summary>
        public virtual DateTime? DateOfViolation { get; set; }

        /// <summary>
        /// Нарушения - Час правонарушения
        /// </summary>
        public virtual int? HourOfViolation { get; set; }

        /// <summary>
        /// Нарушения - Минута правонарушения
        /// </summary>
        public virtual int? MinuteOfViolation { get; set; }
    }
}