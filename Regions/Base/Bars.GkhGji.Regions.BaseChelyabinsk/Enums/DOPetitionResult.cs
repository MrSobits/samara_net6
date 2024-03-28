namespace Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV
{
    using Bars.B4.Utils;

    /// <summary>
    /// Рассмотрение/отклонение
    /// </summary>
    public enum DOPetitionResult
    {
        [Display("-")]
        notSet = 0,

        [Display("Восстановление срока не требуется")]
        notNeeded = 10,

        [Display("Удволетворить")]
        Complete = 20,

        [Display("Отказать")]
        Reject = 30
    }
}