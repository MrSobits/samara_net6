namespace Bars.Gkh.Decisions.Nso.Entities.Decisions
{
    using Bars.Gkh.Enums.Decisions;

    /// <summary>
    ///     Решение по ведение лицевого счета
    /// </summary>
    public class AccountManagementDecision: UltimateDecision
    {
        /// <summary>
        ///     Способ ведения лицевого счета
        /// </summary>
        public virtual AccountManagementType Decision { get; set; }
    }
}
