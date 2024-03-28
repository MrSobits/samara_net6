namespace Bars.GkhGji.Regions.Voronezh.Entities
{
    using Bars.Gkh.Entities;
    using System;
    using Enums;
    using B4.Modules.States;

    /// <summary>
    /// МКД в управлении рассчитываемой организации
    /// </summary>
    public class ROMCategoryMKD : BaseGkhEntity

    {
        /// <summary>
        /// Расчет категории для контрагента
        /// </summary>
        public virtual ROMCategory ROMCategory { get; set; }

        /// <summary>
        /// Мкд в управлении
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }


        /// <summary>
        /// Дата начала управления Мкд
        /// </summary>
        public virtual DateTime? DateStart { get; set; }

    }
}