﻿namespace Bars.GkhGji.Regions.Chelyabinsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using System;

    /// <summary>
    /// Юристы
    /// </summary>
    public class CourtPracticeRealityObject : BaseEntity
    {
        /// <summary>
        /// CourtPractice
        /// </summary>
        public virtual CourtPractice CourtPractice { get; set; }

        /// <summary>
        /// RealityObject
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

    }
}