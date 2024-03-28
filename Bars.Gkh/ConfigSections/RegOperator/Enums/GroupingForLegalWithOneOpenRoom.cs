namespace Bars.Gkh.ConfigSections.RegOperator.Enums
{
    using B4.Utils;

    //Группировка квитанции юр.лица с 1 помещением
    /// <summary>
    /// Группировка по организационно-правовой форме
    /// </summary>
    public enum GroupingForLegalWithOneOpenRoom
    {
        /// <summary>
        /// В папке с юр.лицами
        /// </summary>
        [Display("В папке с юр.лицами")]
        LegalFolder = 1,

        /// <summary>
        /// В папке с физ.лицами
        /// </summary>
        [Display("В папке с физ.лицами")]
        IndividualFolder = 2
    }
}
