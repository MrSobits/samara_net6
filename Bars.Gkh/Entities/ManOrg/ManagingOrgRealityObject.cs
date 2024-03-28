namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Жилой дом управляющей организации
    /// </summary>
    public class ManagingOrgRealityObject : BaseGkhEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Управляющая организация
        /// </summary>
        public virtual ManagingOrganization ManagingOrganization { get; set; }

    }
}