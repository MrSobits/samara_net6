namespace Bars.Gkh.Overhaul.Tat.Enum
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип протокола собственников жилых помещений
    /// </summary>
    public enum PropertyOwnerProtocolType
    {
        /// <summary>
        /// Протокол о формировании фонда капитального ремонта
        /// </summary>
        [Display("Протокол о формировании фонда капитального ремонта")]
        FormationFund = 0,

        /// <summary>
        /// Протокол о выборе управляющей организации
        /// </summary>
        [Display("Протокол о выборе управляющей организации")]
        SelectManOrg = 10,

        /// <summary>
        /// Постановление Исполкома МО
        /// </summary>
        [Display("Постановление Исполкома МО")]
        ResolutionOfTheBoard = 20,

        /// <summary>
        /// Протокол о займе
        /// </summary>
        [Display("Протокол о займе")]
        Loan = 30
    }
}