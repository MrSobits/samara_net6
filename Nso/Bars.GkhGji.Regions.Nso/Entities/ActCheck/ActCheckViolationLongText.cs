namespace Bars.GkhGji.Regions.Nso.Entities
{
    using B4.DataAccess;
    using GkhGji.Entities;

    public class ActCheckViolationLongText : BaseEntity
    {
        /// <summary>
        /// Нарушение акта проверки
        /// </summary>
        public virtual ActCheckViolation Violation { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual byte[] Description { get; set; }
    }
}