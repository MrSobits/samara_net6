namespace Bars.Gkh.Entities.Dicts
{
    /// <summary>
    /// Работы по содержанию и ремонту МКД
    /// </summary>
    public class ContentRepairMkdWork : BaseGkhEntity
    {
        /// <summary>
        /// Вид работы
        /// </summary>
        public virtual Work Work { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }
    }
}
