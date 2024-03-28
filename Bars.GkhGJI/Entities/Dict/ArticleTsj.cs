using Bars.Gkh.Entities;

namespace Bars.GkhGji.Entities
{
    /// <summary>
    /// Статья ТСЖ
    /// </summary>
    public class ArticleTsj : BaseGkhEntity
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Группа
        /// </summary>
        public virtual string Group { get; set; }

        /// <summary>
        /// Статья ЖК
        /// </summary>
        public virtual string Article { get; set; }
    }
}