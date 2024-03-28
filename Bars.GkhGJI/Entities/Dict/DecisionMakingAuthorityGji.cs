namespace Bars.GkhGji.Entities.Dict
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Органы, принимающие решение по предписанию
    /// </summary>
    public class DecisionMakingAuthorityGji : BaseEntity
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }
    }
}
