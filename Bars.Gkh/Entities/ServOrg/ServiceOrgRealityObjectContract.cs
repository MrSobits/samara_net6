namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Жилой дом договора организации поставщика жил. услуг
    /// </summary>
    public class ServiceOrgRealityObjectContract : BaseGkhEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Договор
        /// </summary>
        public virtual ServiceOrgContract ServOrgContract { get; set; }
    }
}