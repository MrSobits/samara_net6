namespace Bars.Gkh.Enums
{
    using System;

    using Bars.B4.Utils;

    /// <summary>
    /// Причина по которой проставился признак Дом не участвует в программе КР
    /// </summary>
    [Flags]
    public enum IsNotInvolvedCrReason
    {
        /// <summary>
        /// Дом не участвует в программе КР, т.к. физический износ более 70%
        /// </summary>
        [Display ("т.к. физический износ более 70%")]
        PhysicalWear = 1,

        /// <summary>
        /// Дом не участвует в программе КР, т.к. это дом блокированной застройки
        /// </summary>
        [Display("т.к. это дом блокированной застройки")]
        BlockedBuilding = 2,

        /// <summary>
        /// Дом не участвует в программе КР, т.к. в нём менее 3х квартир
        /// </summary>
        [Display("т.к. в нём менее 3х квартир")]
        NumberApartments = 4
    }
}