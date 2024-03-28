namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Тип проекта
    /// </summary>
    public class TypeProject : BaseGkhEntity
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
