namespace Bars.GkhCr.Enums
{
    using B4.Utils;

    /// <summary>
    /// Тип источников финансирования (это не то же самое что справочник источник фин-я)
    /// </summary>
    public enum TypeFinanceSource
    {
        [Display("Федеральные cредства")]
        FederalFunds = 10,

        [Display("Средства бюджета субъекта")]
        SubjectBudgetFunds = 20,

        [Display("Средства местного бюджета")]
        PlaceBudgetFunds = 30,

        [Display("Средства граждан")]
        OccupantFunds = 40,

        [Display("Не указан")]
        NotSet = 50,

        [Display("Отсутствует")]
        Not = 60
    }
}
