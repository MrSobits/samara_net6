namespace Bars.Gkh.Utils
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Колонка статуса
    /// </summary>
    public class StateColumn : RefColumn
    {
        /// <summary>
        /// Колонка статуса, содержащая индекс и внешний ключ
        /// </summary>
        /// <param name="columnName">Имя колонки</param>
        /// <param name="columnProperty"></param>
        /// <param name="indexAndForeignKeyName">Имя индекса и внешнего ключа</param>
        public StateColumn(string columnName, ColumnProperty columnProperty, string indexAndForeignKeyName)
            : base(columnName, columnProperty, indexAndForeignKeyName, "B4_STATE", "ID")
        {

        }

        /// <summary>
        /// Колонка статуса, содержащая индекс и внешний ключ
        /// </summary>
        /// <param name="columnName">Имя колонки</param>
        /// <param name="indexAndForeignKeyName">Имя индекса и внешнего ключа</param>
        public StateColumn(string columnName, string indexAndForeignKeyName)
            : base(columnName, indexAndForeignKeyName, "B4_STATE", "ID")
        {

        }
    }
}