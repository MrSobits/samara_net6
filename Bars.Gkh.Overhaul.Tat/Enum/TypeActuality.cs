namespace Bars.Gkh.Overhaul.Tat.Enum
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип актуальности записи краткосрочки с версией
    /// </summary>
    public enum TypeActuality
    {
        [Display("Запись не актуализирована с версией")]
        NoActual = 10,

        [Display("Запись актуализирована с версией")]
        Actual = 20
    }
}