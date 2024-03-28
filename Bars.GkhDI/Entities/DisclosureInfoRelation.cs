namespace Bars.GkhDi.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Связь деятельности УО и деятельности УО в доме
    /// </summary>
    public class DisclosureInfoRelation : BaseImportableEntity
    {
        /// <summary>
        /// Деятельность УО
        /// </summary>
        public virtual DisclosureInfo DisclosureInfo { get; set; }

        /// <summary>
        /// Деятельность УО в доме
        /// </summary>
        public virtual DisclosureInfoRealityObj DisclosureInfoRealityObj { get; set; }
    }
}
