namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;

    public class GjiParam : BaseEntity
    {
        /// <summary>
        /// Ключ
        /// </summary>
        public virtual string Key { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public virtual string Value { get; set; }
    }
}