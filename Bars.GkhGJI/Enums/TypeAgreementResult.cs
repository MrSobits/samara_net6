namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип результата согласования
    /// </summary>
    public enum TypeAgreementResult
    {
        [Display("Не задано")]
        NotSet = 10,

        [Display("Согласовано")]
        Agreed = 20,

        [Display("Не согласовано")]
        NotAgreed = 30
    }
}