namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Параметры ДПКР
    /// </summary>
    public class DpkrParams: BaseImportableEntity
    {
        /// <summary>
        /// Параметры (в виде JSON)
        /// </summary>
        public virtual string Params { get; set; }
    }
}