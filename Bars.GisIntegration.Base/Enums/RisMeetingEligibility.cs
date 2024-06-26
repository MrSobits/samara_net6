﻿namespace Bars.GisIntegration.Base.Enums
{
    using Bars.B4.Utils;

    public enum RisMeetingEligibility
    {
        /// <summary>
        /// Правомочно
        /// </summary>
        [Display("COMPETENT")]
        C = 10,

        /// <summary>
        /// Не правомочно
        /// </summary>
        [Display("NOT_COMPETENT")]
        N = 20
    }
}
