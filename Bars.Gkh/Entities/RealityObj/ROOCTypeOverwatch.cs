namespace Bars.Gkh.Entities
{
    using System;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Организации, осуществляющие видеонаблюдение на доме
    /// </summary>
    public class ROOCTypeOverwatch : BaseEntity
    {
        /// <summary>
        /// Обсуживающая видеонаблюдение организация
        /// </summary>
        public virtual RealityObjectOverwatchContragent RealityObjectOverwatchContragent { get; set; }

        /// <summary>
        /// Тип видеонаблюдения
        /// </summary>
        public virtual VideoOverwatchType VideoOverwatchType { get; set; }

    }
}