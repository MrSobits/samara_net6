namespace Bars.Gkh.Overhaul.Tat.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Муниципальное образование регионального оператора
    /// </summary>
    public class RegOperatorMunicipality : BaseEntity
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