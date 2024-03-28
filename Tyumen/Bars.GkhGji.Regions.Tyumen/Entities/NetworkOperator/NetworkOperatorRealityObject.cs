namespace Bars.GkhGji.Regions.Tyumen.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Связка оператор связи - дом
    /// </summary>
    public class NetworkOperatorRealityObject : BaseEntity
    {
        /// <summary>
        /// Оператор связи
        /// </summary>
        public virtual NetworkOperator NetworkOperator { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Ширина полосы
        /// </summary>
        public virtual string Bandwidth { get; set; }
    }
}