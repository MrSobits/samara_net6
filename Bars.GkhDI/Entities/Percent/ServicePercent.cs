namespace Bars.GkhDi.Entities
{
    /// <summary>
    /// Процент блока
    /// </summary>
    public class ServicePercent : BasePercent
    {
        /// <summary>
        /// Услуга
        /// </summary>
        public virtual BaseService Service { get; set; }
    }
}
