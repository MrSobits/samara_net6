namespace Bars.Gkh.Utils
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Колонка файла
    /// </summary>
    public class FileColumn : RefColumn
    {
        /// <summary>
        /// Колонка файла, содержащая индекс и внешний ключ
        /// </summary>
        /// <param name="columnName">Имя колонки</param>
        /// <param name="property"></param>
        /// <param name="indexAndForeignKeyName">Имя индекса и внешнего ключа</param>
        public FileColumn(string columnName, ColumnProperty property, string indexAndForeignKeyName)
            : base(columnName, property, indexAndForeignKeyName, "B4_FILE_INFO", "ID")
        {
        }

        /// <summary>
        /// Колонка файла, содержащая индекс и внешний ключ
        /// </summary>
        /// <param name="columnName">Имя колонки</param>
        /// <param name="indexAndForeignKeyName">Имя индекса и внешнего ключа</param>
        public FileColumn(string columnName, string indexAndForeignKeyName)
            : base(columnName, indexAndForeignKeyName, "B4_FILE_INFO", "ID")
        {
        }
    }
}