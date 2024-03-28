namespace Bars.Gkh.Overhaul.Tat.Enum
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип операции по счету
    /// </summary>

    public enum PaymentSizeCrType
    {
        [Display("Превышает минимальный размер взноса")]
        OverMinSize = 10,

        [Display("Не превышает минимальный размер взноса")]
        NoMoreMinSize = 20
    }
}