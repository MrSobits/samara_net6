namespace Bars.GkhDi.Enums
{
    using B4.Utils;

    /// <summary>
    /// Региональный фонд
    /// </summary>
    public enum RegionalFundDi
    {
        [Display("Не участвует в формировании Рег.фонда")]
        NotParticipateInFormationRegFund = 10,

        [Display("Участвует в формировании Рег.фонда")]
        ParticipateInFormationRegFund = 20,
    }
}
