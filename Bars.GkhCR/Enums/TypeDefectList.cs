namespace Bars.GkhCr.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип дефектной ведомости
    /// </summary>
    public enum TypeDefectList
    {
        /// <summary>
        /// Не задано(по-умолчанию)
        /// </summary>
        [Display("Не задано")]
        NotSet = 0,

        /// <summary>
        /// Ведомость заказчика
        /// </summary>
        [Display("Ведомость заказчика")]
        Customer = 10,

        /// <summary>
        /// Ведомость подрядчика
        /// </summary>
        [Display("Ведомость подрядчика")]
        Contractor = 20,

        /// <summary>
        /// На непредвиденные работы, затраты
        /// </summary>
        [Display("На непредвиденные работы, затраты")]
        UnexpectedExpenses = 30
    }
}