namespace Bars.Gkh.ConfigSections.Overhaul.Enums
{
    using B4.Utils;

    /// <summary>
    /// Приоритет записей при расчета бюджет
    /// </summary>
    public enum PriorityBudget
    {
        [Display("1.Незафиксированные записи. 2.Зафиксированные записи.")]
        NotFixed = 0,

        [Display("1.Зафиксированные записи. 2.Незафиксированные записи.")]
        Fixed = 10
    }
}