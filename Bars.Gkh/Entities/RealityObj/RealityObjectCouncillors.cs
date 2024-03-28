namespace Bars.Gkh.Entities
{
    using Bars.Gkh.Enums;

    /// <summary>
    /// Члены совета Многоквартирного дома (МКД)
    /// </summary>
    public class RealityObjectCouncillors : BaseGkhEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        public virtual string Fio { get; set; }

        /// <summary>
        /// Должность
        /// </summary>
        public virtual TypeCouncillors Post { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>
        public virtual string Phone { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public virtual string Email { get; set; }
    }
}
