namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Обновление сальдо
    /// </summary>
    public class SaldoRefresh : BaseImportableEntity
    {
        /// <summary>
        /// Номер счета
        /// </summary>
        public virtual PersAccGroup Group { get; set; }
    }
}