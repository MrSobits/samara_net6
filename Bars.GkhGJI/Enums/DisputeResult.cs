namespace Bars.GkhGji.Regions.Voronezh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Результат рассмотрения
    /// </summary>
    public enum DisputeResult
    {
        /// <summary>
        /// -
        /// </summary>
        [Display("-")]
        NotSet = 0,

        /// <summary>
        /// В удовлетворении требований отказано
        /// </summary>
        [Display("В удовлетворении требований отказано")]
        Denied = 10,

        /// <summary>
        /// Заявленные требования удовлетворены частично
        /// </summary>
        [Display("Заявленные требования удовлетворены частично")]
        PartiallySatisfied = 20,

        /// <summary>
        /// Заявленные требования удовлетворены полностью
        /// </summary>
        [Display("Заявленные требования удовлетворены полностью")]
        CompletelySatisfied = 30,

        /// <summary>
        /// Производство по делу прекращено
        /// </summary>
        [Display("Производство по делу прекращено")]
        Stoped = 40,

        /// <summary>
        /// Оставлено без рассмотрения
        /// </summary>
        [Display("Оставлено без рассмотрения")]
        LeftWithoutConsideration = 50,

        /// <summary>
        /// Возвращено
        /// </summary>
        [Display("Возвращено")]
        Returned = 60

    }
}