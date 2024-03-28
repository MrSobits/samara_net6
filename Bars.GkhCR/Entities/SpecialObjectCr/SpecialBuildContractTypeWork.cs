namespace Bars.GkhCr.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Виды работ договора подряда КР
    /// </summary>
    public class SpecialBuildContractTypeWork : BaseImportableEntity
    {
        /// <summary>
        /// Договор подряда КР
        /// </summary>
        public virtual SpecialBuildContract BuildContract { get; set; }

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
