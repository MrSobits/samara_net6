namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;
    using Bars.Gkh.Enums.Decisions;

    /// <summary>
    /// Способ формирования фонда КР
    /// </summary>
    public enum CrFundFormationType
    {
        /// <summary>
        /// Неизвестный
        /// </summary>
        [Display("")]
        Unknown = -1,

        /// <summary>
        /// Специальный счет регионального оператора
        /// </summary>
        [Display("Специальный счет регионального оператора")]
        SpecialRegOpAccount = 0,

        /// <summary>
        /// Счет регионального оператора
        /// </summary>
        [Display("Счет регионального оператора")]
        RegOpAccount = 1,

        /// <summary>
        /// Специальный счет
        /// </summary>
        [Display("Специальный счет")]
        SpecialAccount = 2,

        /// <summary>
        /// Не выбран
        /// </summary>
        [Display("Не выбран")]
        NotSelected = 3
    }

    public static class CrFundFormationTypeExtension
    {
        /// <summary>
        /// Преобразовать в <see cref="CrFundFormationDecisionType"/>
        /// </summary>
        public static CrFundFormationDecisionType ToDecisionType(this CrFundFormationType type)
        {
            switch (type)
            {
                case CrFundFormationType.SpecialAccount:
                case CrFundFormationType.SpecialRegOpAccount:
                    return CrFundFormationDecisionType.SpecialAccount;

                case CrFundFormationType.RegOpAccount:
                    return CrFundFormationDecisionType.RegOpAccount;

                default:
                    return CrFundFormationDecisionType.Unknown;
            }
        }

        /// <summary>
        /// Преобразовать в <see cref="CrFundFormationDecisionType"/>
        /// </summary>
        public static CrFundFormationDecisionType ToDecisionType(this CrFundFormationType? type)
        {
            return type.HasValue ? type.Value.ToDecisionType() : CrFundFormationDecisionType.Unknown;
        }
    }
}