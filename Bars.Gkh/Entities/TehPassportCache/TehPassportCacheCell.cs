namespace Bars.Gkh.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Кэшированное значение технического паспорта
    /// <para>Не хранимая сущность</para>
    /// </summary>
    public class TehPassportCacheCell : PersistentObject
    {
        /// <summary>
        /// Идентификатор дома <see cref="RealityObject"/>
        /// </summary>
        public virtual long RealityObjectId { get; set; }

        /// <summary>
        /// Код формы
        /// </summary>
        public virtual string FormCode { get; set; }

        /// <summary>
        /// Номер строки
        /// </summary>
        public virtual int RowId { get; set; }

        /// <summary>
        /// Номер столбца
        /// </summary>
        public virtual int ColumnId { get; set; }

        /// <summary>
        /// Значение ячейки
        /// </summary>
        public virtual string Value { get; set; }

        /// <summary>
        /// Код ячейки
        /// </summary>
        public virtual string CellCode => $"{this.RowId}:{this.ColumnId}";
    }
}