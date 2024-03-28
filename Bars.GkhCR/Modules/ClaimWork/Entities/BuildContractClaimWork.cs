namespace Bars.GkhCr.Modules.ClaimWork.Entities
{
    using GkhCr.Entities;
    using Enums;
    using Gkh.Modules.ClaimWork.Entities;

    /// <summary>
    /// Основание претензионно исковой работы для Договоров Подряда
    /// </summary>
    public class BuildContractClaimWork : BaseClaimWork
    {
        /// <summary>
        /// Договор подряда
        /// </summary>
        public virtual BuildContract BuildContract { get; set; }

        /// <summary>
        /// тип создания записи
        /// </summary>
        public virtual BuildContractCreationType CreationType { get; set; }
    }
}