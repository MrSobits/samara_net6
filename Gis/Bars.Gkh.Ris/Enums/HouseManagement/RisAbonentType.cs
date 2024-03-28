﻿namespace Bars.Gkh.Ris.Enums.HouseManagement
{
    using B4.Utils;

    /// <summary>
    /// Тип Счета
    /// </summary>
    public enum RisAbonentType
    {
        /// <summary>
        /// Счет физ.лица
        /// </summary>
        [Display("Счет физ.лица")]
        IndividualAccount = 1,

        /// <summary>
        /// Счет юр.лица
        /// </summary>
        [Display("Счет юр.лица")]
        LegalEntityAccount = 2
    }
}
