namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities.Dict;

    /// <summary>
    /// Таблица связи Направдения деятелньости субъекта проверки и Протокола
    /// </summary>
    public class ProtocolActivityDirection : BaseEntity
    {
        /// <summary>
        /// Направление деятельности субъекта првоерки
        /// </summary>
        public virtual ActivityDirection ActivityDirection { get; set; }

        /// <summary>
        /// Протокол
        /// </summary>
        public virtual GkhGji.Entities.Protocol Protocol { get; set; }
    }
}