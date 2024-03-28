namespace Bars.Gkh.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Связь между двумя договорами
    /// </summary>
    public class ManOrgContractRelation : BaseGkhEntity
    {
        /// <summary>
        /// Родительский договор
        /// </summary>
        public virtual ManOrgBaseContract Parent { get; set; }

        /// <summary>
        /// Дочерний договор
        /// </summary>
        public virtual ManOrgBaseContract Children { get; set; }

        /// <summary>
        /// Тип связи
        /// </summary>
        public virtual TypeContractRelation TypeRelation { get; set; }
    }
}