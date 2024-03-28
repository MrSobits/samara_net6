namespace Bars.GkhGji.Regions.BaseChelyabinsk.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип КНМ
    /// </summary>
    public enum RequesterRole
    {

        /// <summary>
        /// Физическое лицо
        /// </summary>
        [Display("Физическое лицо")]
        PERSON = 0,

        /// <summary>
        /// Плановая проверка
        /// </summary>
        [Display("Юридическое лицо")]
        EMPLOYEE = 1,

        /// <summary>
        /// Индивидуальный предприниматель
        /// </summary>
        [Display("Индивидуальный предприниматель")]
        BUSINESSMAN = 2
    }
}