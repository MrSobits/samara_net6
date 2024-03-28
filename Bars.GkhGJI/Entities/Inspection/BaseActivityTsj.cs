namespace Bars.GkhGji.Entities
{
    using System.Collections.Generic;

    /// <summary>
    /// Основание деятельность ТСЖ
    /// </summary>
    public class BaseActivityTsj : InspectionGji
    {
        /// <summary>
        /// Деятельность ТСЖ
        /// </summary>
        public virtual ActivityTsj ActivityTsj { get; set; }

        /// <summary>
        /// Список домов проверки (не хранимое поле)
        /// </summary>
        public virtual List<long> RealityObjects { get; set; }
    }
}