namespace Bars.GkhCr.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Виды работ договора подряда КР
    /// </summary>
    public class BuildContractTypeWork : BaseImportableEntity
    {
        /// <summary>
        /// Договор подряда КР
        /// </summary>
        public virtual BuildContract BuildContract { get; set; }

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
