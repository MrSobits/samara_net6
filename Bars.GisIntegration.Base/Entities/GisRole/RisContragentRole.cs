namespace Bars.GisIntegration.Base.Entities.GisRole
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Роли контрагента РИС
    /// </summary>
    public class RisContragentRole : BaseEntity
    {
        /// <summary>
        /// Роль ГИС
        /// </summary>
        public virtual GisRole Role { get; set; }

        /// <summary>
        /// Контрагент РИС
        /// </summary>
        public virtual GisOperator GisOperator { get; set; }
    }
}