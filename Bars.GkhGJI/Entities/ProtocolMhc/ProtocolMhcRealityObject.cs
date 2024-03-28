namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Дома в Протоколе МЖК 
    /// </summary>
    public class ProtocolMhcRealityObject : BaseGkhEntity
    {
        /// <summary>
        /// Протокол МЖК
        /// </summary>
        public virtual ProtocolMhc ProtocolMhc { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }
    }
}