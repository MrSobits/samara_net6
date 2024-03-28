namespace Bars.GkhCr.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Объект КР договора подряда КР
    /// </summary>
    public class MassBuildContractObjectCrWork : BaseEntity
    {
        /// <summary>
        /// Договор подряда КР
        /// </summary>
        public virtual MassBuildContractObjectCr MassBuildContractObjectCr { get; set; }

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
