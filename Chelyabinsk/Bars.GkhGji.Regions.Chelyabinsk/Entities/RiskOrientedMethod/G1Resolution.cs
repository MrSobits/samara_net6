﻿namespace Bars.GkhGji.Regions.Chelyabinsk.Entities
{
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Enums;
    using B4.Modules.States;

    /// <summary>
    /// Постановление для расчета категории риска по коэффициенту Vpr
    /// </summary>
    public class G1Resolution : BaseGkhEntity

    {
        /// <summary>
        /// Расчет категории для контрагента
        /// </summary>
        public virtual ROMCategory ROMCategory { get; set; }

        /// <summary>
        /// Постановление
        /// </summary>
        public virtual Resolution Resolution { get; set; }

        /// <summary>
        ///Статьи закона
        /// </summary>
        public virtual string ArtLaws { get; set; }

    }
}