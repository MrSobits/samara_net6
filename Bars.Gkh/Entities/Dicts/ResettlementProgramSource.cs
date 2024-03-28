namespace Bars.Gkh.Entities.Dicts
{
    /// <summary>
    /// Источник по программе переселения
    /// </summary>
    public class ResettlementProgramSource : BaseGkhEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }
    }
}