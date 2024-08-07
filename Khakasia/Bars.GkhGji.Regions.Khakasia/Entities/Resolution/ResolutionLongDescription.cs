﻿namespace Bars.GkhGji.Regions.Khakasia.Entities
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Длинные описания для постановления (Хакасия).
    /// </summary>
    public class ResolutionLongDescription : BaseEntity
    {
        /// <summary>
        /// Постановление.
        /// </summary>
        public virtual Resolution Resolution { get; set; }

        /// <summary>
        /// Описание.
        /// </summary>
        public virtual byte[] Description { get; set; }
    }
}
