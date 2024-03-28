namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using Bars.Gkh.Entities;

    using Gkh.Entities.CommonEstateObject;

    /// <summary>
    /// Доля финанскирования ООИ
    /// </summary>
    public class ShareFinancingCeo : BaseImportableEntity
    {
        /// <summary>
        /// ООИ
        /// </summary>
        public virtual CommonEstateObject CommonEstateObject { get; set; }

        /// <summary>
        /// Доля
        /// </summary>
        public virtual decimal Share { get; set; }
    }
}
