namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип изменения (для логирования импортов)
    /// </summary>
    public enum LogTypeChanged
    {
        [Display("Изменен")]
        Changed = 10,

        [Display("Добавлен")]
        Added = 20
    }
}
