namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Муниципальные образования контрагенты
    /// </summary>
    public class ContragentMunicipality : BaseImportableEntity
    {
        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual Municipality Municipality { get; set; }
    }
}