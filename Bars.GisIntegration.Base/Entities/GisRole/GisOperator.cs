namespace Bars.GisIntegration.Base.Entities.GisRole
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Сущность для связи РИС контрагента и ролей ГИС
    /// </summary>
    public class GisOperator : BaseEntity
    {
        /// <summary>
        /// Контрагент РИС
        /// </summary>
        public virtual RisContragent Contragent { get; set; }
    }
}