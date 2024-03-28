namespace Bars.GkhCr.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Виды работ договора на услугу
    /// </summary>
    public class SpecialContractCrTypeWork : BaseImportableEntity
    {
        /// <summary>
        /// Договор на услугу
        /// </summary>
        public virtual SpecialContractCr ContractCr { get; set; }

        /// <summary>
        /// Вид работ
        /// </summary>
        public virtual SpecialTypeWorkCr TypeWork { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public virtual decimal? Sum { get; set; }
    }
}
