namespace Bars.Gkh.RegOperator.Enums
{
    using Bars.B4.Utils;

    public enum BankDocumentImportCheckState
    {
        [Display("Нет")]
        NotChecked = 0,

        [Display("Да")]
        Checked = 10,

        [Display("С ошибками")]
        Error = 20
    }
}