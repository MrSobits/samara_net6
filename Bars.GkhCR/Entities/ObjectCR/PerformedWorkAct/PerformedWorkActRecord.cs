namespace Bars.GkhCr.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Запись акта выполненных работ мониторинга СМР
    /// </summary>
    public class PerformedWorkActRecord : BaseEstimate
    {
        /// <summary>
        /// Акт выполненных работ мониторинга СМР
        /// </summary>
        public virtual PerformedWorkAct PerformedWorkAct { get; set; }
    }
}
