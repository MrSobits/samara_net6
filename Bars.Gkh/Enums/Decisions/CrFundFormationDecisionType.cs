namespace Bars.Gkh.Enums.Decisions
{
    using B4.Utils;

    /// <summary>
    /// Тип расчетного счета
    /// </summary>
    public enum CrFundFormationDecisionType
    {
        /// <summary>
        /// -
        /// </summary>
        [Display("-")]
        Unknown = -1,

        /// <summary>
        /// Специальный счет регионального оператора
        /// </summary>
        [Display("Специальный счет")]
        SpecialAccount = 0,

        /// <summary>
        /// Счет регионального оператора
        /// </summary>
        [Display("Счет регионального оператора")]
        RegOpAccount = 1
    }

    /// <summary> Конвертер в строку </summary>
    public static class CrFundFormationDecisionTypeConverter
    {
        public static string EnumToStr(this CrFundFormationDecisionType type)
        {
            switch (type)
            {
                case CrFundFormationDecisionType.SpecialAccount:
                    return "Специальный счет";
                case CrFundFormationDecisionType.RegOpAccount:
                    return "Счет регионального оператора";
                default:
                    return string.Empty;
            }
        }
    }
}