namespace Bars.GkhCr.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Виды работ договора на услугу
    /// </summary>
    public class ContractCrTypeWork : BaseImportableEntity
    {
        /// <summary>
        /// Договор на услугу
        /// </summary>
        public virtual ContractCr ContractCr { get; set; }

        /// <summary>
        /// Вид работ
        /// </summary>
        public virtual TypeWorkCr TypeWork { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public virtual decimal? Sum { get; set; }
    }
}
