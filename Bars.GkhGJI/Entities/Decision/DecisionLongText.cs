namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;

    public class DecisionLongText : BaseEntity
    {
        /// <summary>
        /// Решение
        /// </summary>
        public virtual Decision Decision { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual byte[] Description { get; set; }
    }
}