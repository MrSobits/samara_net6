namespace Bars.GkhRf.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Перечисление средств рег. фонда В ГИСУ
    /// </summary>
    public class TransferRf : BaseGkhEntity
    {
        /// <summary>
        /// Договор рег. фонда
        /// </summary>
        public virtual ContractRf ContractRf { get; set; }

        /// <summary>
        /// Не хранимое
        /// </summary>
        public virtual int ContractRfObjectsCount { get; set; }
    }
}
