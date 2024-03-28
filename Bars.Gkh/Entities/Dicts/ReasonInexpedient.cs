namespace Bars.Gkh.Entities.Dicts
{
    /// <summary>
    /// Основание нецелесообразности
    /// </summary>
    public class ReasonInexpedient : BaseGkhEntity
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