namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Дома протокола деятельности ТСЖ
    /// </summary>
    public class ActivityTsjProtocolRealObj : BaseGkhEntity
    {
        /// <summary>
        /// Протокол деятельности ТСЖ
        /// </summary>
        public virtual ActivityTsjProtocol ActivityTsjProtocol { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }
    }
}