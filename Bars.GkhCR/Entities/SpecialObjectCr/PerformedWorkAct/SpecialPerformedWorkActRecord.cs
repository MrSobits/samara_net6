namespace Bars.GkhCr.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Запись акта выполненных работ мониторинга СМР
    /// </summary>
    public class SpecialPerformedWorkActRecord : BaseEstimate
    {
        /// <summary>
        /// Акт выполненных работ мониторинга СМР
        /// </summary>
        public virtual SpecialPerformedWorkAct PerformedWorkAct { get; set; }
    }
}
