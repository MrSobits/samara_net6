namespace Bars.Gkh.Overhaul.Tat.Enum
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип записи ДПКР
    /// </summary>
    public enum TypeDpkrRecord
    {
        [Display("Добавлена в ручную")]
        UserRecord = 10,

        [Display("Добавлена при расчете")]
        CalcRecord = 20,

        [Display("Добавлена при актуализации")]
        ActualizeAddRecord = 30
    }
}