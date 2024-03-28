namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Жилой дом договора управляющей организации
    /// </summary>
    public class ManOrgContractRealityObject : BaseGkhEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Договор управления
        /// </summary>
        public virtual ManOrgBaseContract ManOrgContract { get; set; }
    }
}