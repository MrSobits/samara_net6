namespace Bars.Gkh.Modules.Gkh1468.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Дом в Договоре РСО с Домами 
    /// </summary>
    public class PublicServiceOrgContractRealObj : BaseImportableEntity
    {
        /// <summary>
        /// Договор РСО с Домами 
        /// </summary>
        public virtual PublicServiceOrgContract RsoContract { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }
    }
}
