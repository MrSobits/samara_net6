namespace Bars.Gkh.Overhaul.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Сущность записи взноса на КР по муниципальному образованию
    /// </summary>
    public class PaymentSizeMuRecord : BaseImportableEntity
    {
        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Размер взноса на КР
        /// </summary>
        public virtual PaymentSizeCr PaymentSizeCr { get; set; }
    }
}
