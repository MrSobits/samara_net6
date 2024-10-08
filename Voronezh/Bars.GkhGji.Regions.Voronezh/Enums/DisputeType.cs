﻿namespace Bars.GkhGji.Regions.Voronezh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Вид спора
    /// </summary>
    public enum DisputeType
    {
        /// <summary>
        /// Административный (АПК РФ)
        /// </summary>
        [Display("Административный (АПК РФ)")]
        APKRF = 10,

        /// <summary>
        /// Административный (КАС РФ)
        /// </summary>
        [Display("Административный (КАС РФ)")]
        KASRF = 20,

        /// <summary>
        /// Гражданский
        /// </summary>
        [Display("Гражданский")]
        Civil = 30,

        /// <summary>
        /// Административный КоАП
        /// </summary>
        [Display("Административный (КоАП)")]
        AdmCoap = 40

    }
}