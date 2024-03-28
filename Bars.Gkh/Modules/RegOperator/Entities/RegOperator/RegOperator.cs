namespace Bars.Gkh.Modules.RegOperator.Entities.RegOperator
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Региональный оператор
    /// </summary>
    public class RegOperator : BaseImportableEntity
    {
        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }
    }
}