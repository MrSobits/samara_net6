namespace Bars.GkhDi.Regions.Tatarstan.Entities
{
    using Bars.GkhDi.Entities;

    /// <summary>
    /// ППР список работ (Для Татарстана)
    /// </summary>
    public class WorkRepairListTat : WorkRepairList
    {
        /// <summary>
        /// Сведения о выполнении
        /// </summary>
        public virtual string InfoAboutExec { get; set; }

        /// <summary>
        /// Причина отклонения
        /// </summary>
        public virtual string ReasonRejection { get; set; }
    }
}
