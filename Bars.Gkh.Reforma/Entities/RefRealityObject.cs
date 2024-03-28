namespace Bars.Gkh.Reforma.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Синхронизируемый жилой дом
    /// </summary>
    public class RefRealityObject : BaseEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Код дома в Реформе
        /// </summary>
        public virtual int ExternalId { get; set; }

        /// <summary>
        /// Текущая УО
        /// </summary>
        public virtual RefManagingOrganization RefManagingOrganization { get; set; }
    }
}