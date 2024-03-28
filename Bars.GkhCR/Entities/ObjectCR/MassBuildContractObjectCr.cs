namespace Bars.GkhCr.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Объект КР договора подряда КР
    /// </summary>
    public class MassBuildContractObjectCr : BaseImportableEntity
    {
        /// <summary>
        /// Договор подряда КР
        /// </summary>
        public virtual MassBuildContract MassBuildContract { get; set; }

        /// <summary>
        /// Вид работы
        /// </summary>
        public virtual ObjectCr ObjectCr { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public virtual decimal? Sum { get; set; }
    }
}
