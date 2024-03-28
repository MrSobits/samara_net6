namespace Bars.GkhGji.Entities.Dict
{
    using B4.DataAccess;

    /// <summary>
    /// Цель проведения проверки
    /// </summary>
    public class AuditPurposeGji : BaseEntity
    {
        /// <summary>
        /// Наименование цели
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }
    }
}