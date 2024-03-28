using Bars.B4.DataAccess;

namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Группа капитальности
    /// </summary>
    public class MonitoringTypeDict : BaseEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }
    }
}