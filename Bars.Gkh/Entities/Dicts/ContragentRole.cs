namespace Bars.Gkh.Entities.Dicts
{
    /// <summary>
    /// Роли контрагента
    /// </summary>
    public class ContragentRole : BaseGkhEntity
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Полное наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Краткое наименование
        /// </summary>
        public virtual string ShortName { get; set; }
    }
}
