namespace Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanProtocolGji
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    public class TatarstanProtocolGjiRealityObject : BaseEntity
    {
        /// <summary>
        /// Протокол ГЖИ РТ
        /// </summary>
        public virtual TatarstanProtocolGji TatarstanProtocolGji { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }
    }
}
