namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Таблица связи документов (Какой документ из какого был сформирован)
    /// </summary>
    public class DocumentGjiChildren : BaseGkhEntity
    {
        /// <summary>
        /// Родительский документ
        /// </summary>
        public virtual DocumentGji Parent { get; set; }

        /// <summary>
        /// Дочерний документ
        /// </summary>
        public virtual DocumentGji Children { get; set; }
    }
}