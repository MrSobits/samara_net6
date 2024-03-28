namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Жилой дом организации поставщика жил. услуг
    /// </summary>
    public class ServiceOrgRealityObject : BaseGkhEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Организация поставщик жил. услуг
        /// </summary>
        public virtual ServiceOrganization ServiceOrg { get; set; }

    }
}