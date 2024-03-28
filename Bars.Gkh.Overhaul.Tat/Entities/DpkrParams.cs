namespace Bars.Gkh.Overhaul.Tat.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Параметры ДПКР
    /// </summary>
    public class DpkrParams: BaseEntity
    {
        /// <summary>
        /// Параметры (в виде JSON)
        /// </summary>
        public virtual string Params { get; set; }
    }
}