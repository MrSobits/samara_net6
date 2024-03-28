namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActCheck
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

    public class ActCheckRoLongDescription : BaseEntity
    {
        /// <summary>
        /// Жилой дом акта проверки
        /// </summary>
        public virtual ActCheckRealityObject ActCheckRo { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual byte[] Description { get; set; }

        /// <summary>
        /// Нарушения не выявлены
        /// </summary>
        public virtual byte[] NotRevealedViolations { get; set; }

        /// <summary>
        /// Дополнительные характеристики
        /// </summary>
        public virtual byte[] AdditionalChars { get; set; }
    }
}