namespace Bars.GkhCr.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Виды работ договора подряда КР
    /// </summary>
    public class MassBuildContractWork : BaseImportableEntity
    {
        /// <summary>
        /// Договор подряда КР
        /// </summary>
        public virtual MassBuildContract MassBuildContract { get; set; }

        /// <summary>
        /// Вид работы
        /// </summary>
        public virtual Work Work { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public virtual decimal? Sum { get; set; }
    }
}
