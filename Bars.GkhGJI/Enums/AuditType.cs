namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип проверки
    /// </summary>
    /// <remarks>Используется в экспорте в РИС ЖКХ</remarks>
    public enum AuditType
    {
        /// <summary>
        /// Все
        /// </summary>
        [Display("Все")]
        All = 0,

        /// <summary>
        /// Плановые
        /// </summary>
        [Display("Плановые")]
        Planned = 1,

        /// <summary>
        /// Внеплановые
        /// </summary>
        [Display("Внеплановые")]
        NotPlanned = 2,
    }
}