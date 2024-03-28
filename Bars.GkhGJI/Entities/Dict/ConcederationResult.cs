namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;
    /// <summary>
    /// Результат рассмотрения
    /// </summary>
    public class ConcederationResult: BaseEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }
    }
}