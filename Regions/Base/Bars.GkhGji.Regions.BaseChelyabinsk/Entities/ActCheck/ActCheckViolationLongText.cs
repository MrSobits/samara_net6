namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActCheck
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

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