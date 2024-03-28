namespace Bars.GkhCr.Enums
{
    using B4.Utils;

    /// <summary>
    /// Тип платежного поручения
    /// </summary>
    public enum TypePaymentOrder
    {
        [Display("Приход")]
        In = 10,

        [Display("Расход")]
        Out = 20
    }
}
