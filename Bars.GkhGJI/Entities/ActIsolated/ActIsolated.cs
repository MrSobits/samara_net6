namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.Modules.FIAS;

    /// <summary>
    /// Акт без взаимодействия
    /// </summary>
    public class ActIsolated : DocumentGji
    {
        /// <summary>
        /// Проверяемая площадь
        /// </summary>
        public virtual decimal? Area { get; set; }
        
        /// <summary>
        /// Квартира
        /// </summary>
        public virtual string Flat { get; set; }
        
        /// <summary>
        /// Место составления (выбор из ФИАС)
        /// </summary>
        public virtual FiasAddress DocumentPlaceFias { get; set; }

        /// <summary>
        /// Время составления акта
        /// </summary>
        public virtual DateTime? DocumentTime { get; set; }
    }
}
