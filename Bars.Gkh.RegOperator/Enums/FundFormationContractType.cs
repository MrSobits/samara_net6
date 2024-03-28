namespace Bars.Gkh.RegOperator.Enum
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип договора на формирование фонда капитального ремонта
    /// </summary>

    public enum FundFormationContractType
    {
        [Display("О формировании фонда на счете регионального оператора")]
        RegOperatorAccount = 10,

        [Display("О формировании фонда на специальном счете")]
        SpecialAccount = 20
    }
}