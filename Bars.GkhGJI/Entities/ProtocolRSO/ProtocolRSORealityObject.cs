namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Дома в Протоколе прокуратуры
    /// </summary>
    public class ProtocolRSORealityObject : BaseGkhEntity
    {
        /// <summary>
        /// Протокол прокуратуры
        /// </summary>
        public virtual ProtocolRSO ProtocolRSO { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }
    }
}