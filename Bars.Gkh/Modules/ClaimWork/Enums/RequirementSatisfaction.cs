namespace Bars.Gkh.Modules.ClaimWork.Enums
{
    using B4.Utils;

    /// <summary>
    /// Удовлетворение требований
    /// </summary>
    public enum RequirementSatisfaction
    {
        /// <summary>
        /// Не удовлетворено
        /// </summary>
        [Display("Не удовлетворено")]
        Not = 0,

        /// <summary>
        /// Удовлетворено полностью
        /// </summary>
        [Display("Удовлетворено полностью")]
        Full = 10,

        /// <summary>
        /// Удовлетворено частично
        /// </summary>
        [Display("Удовлетворено частично")]
        Partial = 20,

        /// <summary>
        /// Получен ответ с отказом в удовлетворении
        /// </summary>
        [Display("Получен ответ с отказом в удовлетворении")]
        Refusal = 30
    }
}