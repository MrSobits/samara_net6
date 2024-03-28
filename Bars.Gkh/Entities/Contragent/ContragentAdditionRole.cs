namespace Bars.Gkh.Entities
{
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Роли контагента
    /// </summary>
    public class ContragentAdditionRole : BaseGkhEntity
    {
        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Дополнительная роль контагента
        /// </summary>
        public virtual ContragentRole Role { get; set; }
    }
}