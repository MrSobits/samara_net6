namespace Bars.Gkh.Entities.Dicts.ContractService
{
    using Bars.Gkh.Enums;

    /// <summary>
    /// Услуги/Работы по ДУ (договорам управления)
    /// </summary>
    public class AgreementContractService : ManagementContractService
    {
        /// <summary>
        /// Назначение работ
        /// </summary>
        public virtual WorkAssignment WorkAssignment { get; set; }

        /// <summary>
        /// Тип работ
        /// </summary>
        public virtual TypeWork TypeWork { get; set; }
    }
}
