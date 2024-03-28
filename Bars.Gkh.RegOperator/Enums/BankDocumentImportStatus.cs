namespace Bars.Gkh.RegOperator.Enums
{
    using Bars.B4.Utils;

    public enum BankDocumentImportStatus
    {
        [Display("Загружен без предупреждений")]
        SuccessfullyImported = 10,

        [Display("Загружен с предупреждениями")]
        ImportedWithWarnings = 20,

        [Display("Ошибка при загрузке")]
        Failed = 30
    }
}