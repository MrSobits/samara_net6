namespace Bars.Gkh.Entities.TechnicalPassport
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Технический паспорт
    /// </summary>
    public class TechnicalPassport : BaseEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }
    }
}