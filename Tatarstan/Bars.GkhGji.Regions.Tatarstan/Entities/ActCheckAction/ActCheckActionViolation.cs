namespace Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Нарушение действия акта проверки
    /// </summary>
    public class ActCheckActionViolation : BaseEntity
    {
        /// <summary>
        /// Действие акта проверки
        /// </summary>
        public virtual ActCheckAction ActCheckAction { get; set; }

        /// <summary>
        /// Нарушение
        /// </summary>
        public virtual ViolationGji Violation { get; set; }

        /// <summary>
        /// Пояснение контролируемого лица
        /// </summary>
        public virtual string ContrPersResponse { get; set; }
    }
}