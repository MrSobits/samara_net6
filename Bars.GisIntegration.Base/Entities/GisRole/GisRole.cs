namespace Bars.GisIntegration.Base.Entities.GisRole
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Роль пользователя ГИС (соответствует НСИ-20)
    /// </summary>
    public class GisRole : BaseEntity
    {
        /// <summary>
        /// Наименование роли
        /// </summary>
        public virtual string Name { get; set; }
    }
}