namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities.Dict;

    /// <summary>
    /// Таблица связи Направдения деятелньости субъекта проверки и Протокола 19.7
    /// </summary>
	public class Protocol197ActivityDirection : BaseEntity
    {
        /// <summary>
        /// Направление деятельности субъекта првоерки
        /// </summary>
        public virtual ActivityDirection ActivityDirection { get; set; }

        /// <summary>
        /// Протокол
        /// </summary>
        public virtual Protocol197 Protocol197 { get; set; }
    }
}