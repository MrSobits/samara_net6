namespace Bars.Gkh.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Параметр конфигурации
    /// </summary>
    public class GkhConfigParam : BaseEntity
    {
        /// <summary>
        /// Имя параметра
        /// </summary>
        public virtual string Key { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public virtual string Value { get; set; }
    }
}