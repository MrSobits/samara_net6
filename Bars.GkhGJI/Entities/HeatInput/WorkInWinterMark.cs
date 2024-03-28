namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Показатели о готовности ЖКС к зимнему периоду
    /// </summary>
    public class WorkInWinterMark : BaseEntity
    {
        /// <summary>
        /// Наименование Показателей 
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// № строки
        /// </summary>
        public virtual int RowNumber { get; set; }

        /// <summary>
        /// Единица измерения
        /// </summary>
        public virtual string Measure { get; set; }

        /// <summary>
        /// Код по ОКЕИ
        /// </summary>
        public virtual string Okei { get; set; }
    }
}