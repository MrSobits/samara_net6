namespace Bars.GisIntegration.Base.Entities.GisRole
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Метод, доступный для роли ГИС
    /// </summary>
    public class GisRoleMethod : BaseEntity
    {
        /// <summary>
        /// Роль ГИС
        /// </summary>
        public virtual GisRole Role { get; set; }

        /// <summary>
        /// Наименование доступного метода
        /// </summary>
        public virtual string MethodName { get; set; }

        /// <summary>
        /// exporter.GetType().Name доступного метода
        /// </summary>
        public virtual string MethodId { get; set; }
    }
}