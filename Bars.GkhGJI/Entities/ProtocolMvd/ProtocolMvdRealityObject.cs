namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Дома в протоколе МВД
    /// </summary>
    public class ProtocolMvdRealityObject : BaseGkhEntity
    {
        /// <summary>
        /// Протокол МВД
        /// </summary>
        public virtual ProtocolMvd ProtocolMvd { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }
    }
}