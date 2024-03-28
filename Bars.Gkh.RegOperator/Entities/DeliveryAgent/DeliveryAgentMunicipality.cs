namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Связь Агента доставки с МО
    /// </summary>
    public class DeliveryAgentMunicipality : BaseImportableEntity
    {
        /// <summary>
        /// Агент доставки
        /// </summary>
        public virtual DeliveryAgent DeliveryAgent { get; set; }

        /// <summary>
        /// МО
        /// </summary>
        public virtual Municipality Municipality { get; set; }
    }
}