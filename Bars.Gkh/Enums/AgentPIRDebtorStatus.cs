using Bars.B4.Utils;

namespace Bars.Gkh.Enums
{
    public enum AgentPIRDebtorStatus
    {
        /// <summary>
        /// Погашен
        /// </summary>
        [Display("Погашен")]
        Repaid = 10,

        /// <summary>
        /// Не погашен 
        /// </summary>
        [Display("Не погашен")]
        NotRepaid = 20,

        /// <summary>
        /// В работе 
        /// </summary>
        [Display("В работе")]
        ToWork = 30
    }
}
