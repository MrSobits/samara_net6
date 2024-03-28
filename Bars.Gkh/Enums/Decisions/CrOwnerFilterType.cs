namespace Bars.Gkh.Enums.Decisions
{
    /// <summary>
    ///     Перечисление для значений фильтра crOwnerType на форме
    ///     B4.view.regop.personal_account.CrOwnerFilterType
    /// </summary>
    public enum CrOwnerFilterType
    {
        /// <summary>
        ///     -
        /// </summary>
        Unknown = -1,

        /// <summary>
        ///     Физические лица
        /// </summary>
        PrysicalPerson = 0,

        /// <summary>
        ///     Юридические лица
        /// </summary>
        LegalPerson = 1,

        /// <summary>
        ///     Юридические лица с 1 помещением
        /// </summary>
        LegalPersonWithOneRoom = 2
    }
}

