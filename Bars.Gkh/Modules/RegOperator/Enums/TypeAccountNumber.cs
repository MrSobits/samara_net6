namespace Bars.Gkh.RegOperator.Enums
{
    using B4.Utils;

    /// <summary>
    /// Способ генерации ЛС
    /// </summary>
    public enum TypeAccountNumber
    {
        [Display("17 знаков")]
        Long = 10,

        [Display("9 знаков")]
        Short = 20
    }
}