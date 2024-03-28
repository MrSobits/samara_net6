namespace Bars.B4.Modules.FIAS
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус актуальности
    /// </summary>
    public enum FiasActualStatusEnum : byte
    {
        [Display("Не актуальный")]
        NotActual = 0,

        [Display("Актуальный")]
        Actual = 1
    }
}