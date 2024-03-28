namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;

    /// <summary>
    /// Муниципальное образование регионального оператора
    /// </summary>
    public class RegOperatorMunicipality : BaseImportableEntity
    {
        /// <summary>
        /// Региональный оператор
        /// </summary>
        public virtual RegOperator RegOperator { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual Municipality Municipality { get; set; }
    }
}