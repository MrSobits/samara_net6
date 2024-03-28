namespace Bars.GkhGji.Regions.Tomsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Описание Дома акта проверки
    /// </summary>
    public class ActCheckRealityObjectDescription : BaseEntity
    {
        /// <summary>
        /// Дом акта проверки
        /// </summary>
        public virtual ActCheckRealityObject ActCheckRealityObject { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual byte[] Description { get; set; }
    }
}