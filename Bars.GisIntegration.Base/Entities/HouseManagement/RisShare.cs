﻿namespace Bars.GisIntegration.Base.Entities.HouseManagement
{
    using System;

    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Доля собственности
    /// </summary>
    public class RisShare : BaseRisEntity
    {
        /// <summary>
        /// Приватизирована
        /// </summary>
        public virtual bool? IsPrivatized { get; set; }

        /// <summary>
        /// Дата прекращения права
        /// </summary>
        public virtual DateTime? TermDate { get; set; }

        /// <summary>
        /// Организация
        /// </summary>
        public virtual RisContragent RisShareContragent { get; set; }

        /// <summary>
        /// Счет
        /// </summary>
        public virtual RisAccount Account { get; set; }
    }
}
