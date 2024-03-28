namespace Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV
{
    using Bars.B4.Utils;

    /// <summary>
    /// Рассмотрение/отклонение
    /// </summary>
    public enum DOTypeStep
    {
        [Display("Запрос дополнительной информации")]
        Info = 0,

        [Display("Рассмотрение ходатайства о восстановлении срока")]
        renewTermStep = 10,

        [Display("Рассмотрения ходотайства о приостановлении исполнения решения")]
        pauseResolutionStep = 20,

    }
}