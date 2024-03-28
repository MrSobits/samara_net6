namespace Bars.Gkh.Utils
{
    using System;
    using System.Collections.Generic;

    using B4.Utils;
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;
    using System.Linq;

    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Entities.Dicts;

    using Dapper;

    using ForeignKeyConstraint = Bars.B4.Modules.Ecm7.Framework.ForeignKeyConstraint;

    /// <summary>
    /// Расширения для <see cref="ITransformationProvider"/>
    /// </summary>
    public static class DatabaseExtensions
    {
        /// <summary>
        /// Преобразовать колонку в null/not null
        /// </summary>
        /// <param name="database"><see cref="ITransformationProvider"/></param>
        /// <param name="table">Таблица</param>
        /// <param name="column">Колонка</param>
        /// <param name="nullable">null/not null</param>
        public static void AlterColumnSetNullable(this ITransformationProvider database, string table, string column, bool nullable)
        {
            string query;

            if (database.DatabaseKind == DbmsKind.PostgreSql)
            {
                query = "ALTER TABLE {0} ALTER COLUMN {1} {2} NULL".FormatUsing(table, column, nullable ? "DROP NOT" : "SET NOT");
            }
            else if (database.DatabaseKind == DbmsKind.Oracle)
            {
                query = "ALTER TABLE {0} MODIFY {1} {2} NULL".FormatUsing(table, column, nullable ? "" : "NOT");
            }
            else
            {
                throw new NotImplementedException("Неизместный диалект БД");
            }

            database.ExecuteNonQuery(query);
        }

        /// <summary>
        /// Переименовать таблицу и её последовательность для id
        /// </summary>
        /// <param name="database"></param>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        public static void RenameTableAndSequence(this ITransformationProvider database, string oldName, string newName)
        {
            database.RenameTable(oldName, newName);

            var oldSequence = oldName.ToLower() + "_id_seq";
            var newSequence = newName.ToLower() + "_id_seq";
            var sequenceExists = false;

            using (var reader = DatabaseExtensions.SequenceReader(database, oldSequence))
            {
                // есть последовательность со старым наименованием таблицы
                if (reader.Read())
                {
                    sequenceExists = reader[0].ToBool();
                }
            }

            if (sequenceExists)
            {
                if (database.DatabaseKind == DbmsKind.PostgreSql)
                {
                    database.ExecuteNonQuery($"alter sequence {oldSequence} rename to {newSequence}");
                }
            }
        }

        /// <summary>
        /// Добавить внешний ключ для все дочерних таблиц
        /// </summary>
        /// <param name="transformation">Провайдер</param>
        /// <param name="name">Наименование внешнего ключа (указывается только постфикс, формат fk_TABLENAME_<paramref name="name"/>)</param>
        /// <param name="parentForeignTable">Внешняя таблица</param>
        /// <param name="foreignColumn">Колонка</param>
        /// <param name="primaryTable">Основная таблица</param>
        /// <param name="primaryColumn">Основная колонка</param>
        /// <param name="onDeleteConstraint">Действие при удалении</param>
        /// <param name="onUpdateConstraint">Действии при обновлении</param>
        /// <param name="deferrability"></param>
        public static void AddForeignKeyToChildren(
            this ITransformationProvider transformation,
            string name,
            SchemaQualifiedObjectName parentForeignTable,
            string foreignColumn,
            SchemaQualifiedObjectName primaryTable,
            string primaryColumn,
            ForeignKeyConstraint onDeleteConstraint = ForeignKeyConstraint.NoAction,
            ForeignKeyConstraint onUpdateConstraint = ForeignKeyConstraint.NoAction,
            ConstraintDeferrability deferrability = ConstraintDeferrability.NonDeferrable)
        {
            var listChilds = transformation.GetChildTables(parentForeignTable);
            foreach (var childTable in listChilds)
            {
                transformation.AddForeignKey($"FK_{childTable}_{name}", childTable, foreignColumn, primaryTable, primaryColumn, onDeleteConstraint, onUpdateConstraint, deferrability);
            }
        }

        /// <summary>
        /// Добавить внешний ключ для все дочерних таблиц
        /// </summary>
        /// <param name="transformation">Провайдер</param>
        /// <param name="indexName">Наименование индекса (указывается только постфикс, формат ind_TABLENAME_<paramref name="indexName"/>)</param>
        /// <param name="unique">Уникальный индекс</param>
        /// <param name="parentTable">Внешняя таблица</param>
        /// <param name="columnNames">Колонки</param>
        public static void AddIndexToChildren(
            this ITransformationProvider transformation,
            string indexName,
            bool unique,
            SchemaQualifiedObjectName parentTable,
            params string[] columnNames)
        {
            var listChilds = transformation.GetChildTables(parentTable);
            foreach (var childTable in listChilds)
            {
                transformation.AddIndex($"IND_{childTable}_{indexName}", unique, childTable, columnNames);
            }
        }

        /// <summary>
        /// Удалить столбец с таблицы с удалением столбца в дочерних, если существует
        /// </summary>
        /// <param name="transformation">Провайдер</param>
        /// <param name="table">Таблица</param>
        /// <param name="column">Колонка</param>
        /// <param name="dropFromChild">Если передать true, то также будет произведено удаление столбца из всех дочерних, если колонка существует</param>
        public static void RemoveColumn(this ITransformationProvider transformation, SchemaQualifiedObjectName table, string column, bool dropFromChild)
        {
            transformation.RemoveColumn(table, column);

            if (dropFromChild)
            {
                var children = transformation.GetChildTables(table);
                foreach (var child in children)
                {
                    transformation.RemoveColumn(child, column);
                }
            }
        }

        /// <summary>
        /// Метод возвращает наименования дочерних таблиц
        /// </summary>
        /// <param name="transformation">Провайдер</param>
        /// <param name="parentForeignTable">Название родительского отношения</param>
        /// <returns></returns>
        public static List<string> GetChildTables(this ITransformationProvider transformation, SchemaQualifiedObjectName parentForeignTable)
        {
            var childTableQuery = $@"SELECT c.relname AS child
                    FROM
                    pg_inherits JOIN pg_class AS c ON(inhrelid = c.oid)
                    JOIN pg_class as p ON(inhparent = p.oid)
                    where p.relname = '{parentForeignTable.Name.ToLower()}'";

            var listChilds = new List<string>();

            using (var childTableNamesReader = transformation.ExecuteQuery(childTableQuery))
            {
                while (childTableNamesReader.Read())
                {
                    listChilds.Add(childTableNamesReader[0].ToString());
                }
            }
            return listChilds;
        }

        private static IDataReader SequenceReader(ITransformationProvider database, string oldSequence)
        {
            if (database.DatabaseKind == DbmsKind.PostgreSql)
            {
                return database.ExecuteQuery($"SELECT 1 as name FROM pg_class where relname = '{oldSequence}'");
            }

            return null;
        }

        /// <summary>
        /// Добавить идентификатор для экспорта
        /// </summary>
        public static void AddExportId(this ITransformationProvider database, string tableName, string sequenceName)
        {
            if (database.TableExists(tableName))
            {
                database.AddColumn(tableName,
                    "EXPORT_ID",
                    DbType.Int64,
                    ColumnProperty.Unique | ColumnProperty.NotNull,
                    $"nextval('{sequenceName}'::regclass)");

                database.AddIndex($"{tableName}_EXPORT_ID_IDX", true, tableName, "EXPORT_ID");
            }
        }

        /// <summary>
        /// Удалить идентификатор для экспорта
        /// </summary>
        public static void RemoveExportId(this ITransformationProvider database, string tableName)
        {
            if (database.TableExists(tableName))
            {
                database.RemoveColumn(tableName, "EXPORT_ID");
            }
        }

        /// <summary>
        /// Метод проверяет существование материализованного представления
        /// </summary>
        /// <param name="database">Поставщик трансформации бд</param>
        /// <param name="viewName">Имя представления</param>
        public static bool MaterializedViewExists(this ITransformationProvider database, SchemaQualifiedObjectName viewName)
        {
            var materializedViewExistsSql = $@"SELECT count(*) > 0
                    FROM pg_catalog.pg_class c
                    JOIN pg_namespace n ON n.oid = c.relnamespace
                    WHERE c.relkind = 'm'
                    AND n.nspname = '{viewName.Schema.ToLower()}'
                    AND c.relname = '{viewName.Name.ToLower()}';";

            return database.Connection.ExecuteScalar<bool>(materializedViewExistsSql);
        }

        /// <summary>
        /// Метод проверяет существование материализованного представления
        /// </summary>
        /// <param name="connection">Соединение с бд</param>
        /// <param name="transaction">Тразнакция</param>
        /// <param name="viewName">Имя представления</param>
        public static bool MaterializedViewExists(IDbConnection connection, IDbTransaction transaction, SchemaQualifiedObjectName viewName)
        {
            var materializedViewExistsSql = $@"SELECT count(*) > 0
                    FROM pg_catalog.pg_class c
                    JOIN pg_namespace n ON n.oid = c.relnamespace
                    WHERE c.relkind = 'm'
                    AND n.nspname = '{viewName.Schema.ToLower()}'
                    AND c.relname = '{viewName.Name.ToLower()}';";

            return connection.ExecuteScalar<bool>(materializedViewExistsSql, transaction: transaction);
        }

        /// <summary>
        /// Добавить дочернюю таблицу к базовой
        /// </summary>
        /// <remarks>Отличается от основной тем, что названия таблиц принимаются типа <see cref="SchemaQualifiedObjectName"/></remarks>
        public static void AddJoinedSubclassTable(this ITransformationProvider database, SchemaQualifiedObjectName tableName, SchemaQualifiedObjectName inheritTableName, string indexAndForeignKeyName, params Column[] columns)
        {
            var dictColumns = new List<Column>
            {
                new Column("ID", DbType.Int64, ColumnProperty.PrimaryKey)
            };

            dictColumns.AddRange(columns);

            database.AddTable(tableName, dictColumns.ToArray());
            database.AddForeignKeys(tableName, columns);
            database.AddForeignKey("FK_" + indexAndForeignKeyName, tableName, "ID", inheritTableName, "ID");
        }

        /// <summary>
        /// Добавить внешний ключ
        /// </summary>
        /// <remarks>Отличается от основной тем, что названия таблиц принимаются типа <see cref="SchemaQualifiedObjectName"/></remarks>
        public static void AddRefColumn(this ITransformationProvider database, SchemaQualifiedObjectName tableName, RefColumn refColumn)
        {
            database.AddColumn(tableName, new Column(refColumn.Name, DbType.Int64, refColumn.ColumnProperty));
            database.AddIndex($"IND_{refColumn.IndexAndForeignKeyName}", false, tableName, refColumn.Name);
            database.AddForeignKey($"FK_{refColumn.IndexAndForeignKeyName}", tableName, refColumn.Name, refColumn.PrimaryTable, refColumn.PrimaryColumn, ForeignKeyConstraint.NoAction, ForeignKeyConstraint.NoAction, refColumn.Deferrability);
        }

        private static void AddForeignKeys(this ITransformationProvider database, SchemaQualifiedObjectName tableName, params Column[] columns)
        {
            foreach (var column in columns)
            {
                if (column is RefColumn)
                {
                    var refColumn = column as RefColumn;
                    database.AddIndex("IND_" + refColumn.IndexAndForeignKeyName, false, tableName, refColumn.Name);
                    database.AddForeignKey("FK_" + refColumn.IndexAndForeignKeyName, tableName, refColumn.Name, refColumn.PrimaryTable, refColumn.PrimaryColumn, ForeignKeyConstraint.NoAction, ForeignKeyConstraint.NoAction, refColumn.Deferrability);
                }
            }
        }

        /// <summary>
        /// Добавить таблицу справочника <see cref="BaseGkhDict"/>
        /// </summary>
        /// <param name="database">Провайдер БД</param>
        /// <param name="tableName">Имя таблицы</param>
        /// <param name="columns">Колонки</param>
        public static void AddGkhDictTable(this ITransformationProvider database, string tableName, params Column[] columns)
        {
            var dictColumns = new List<Column>
            {
                new Column("CODE", DbType.String, ColumnProperty.NotNull),
                new Column("NAME", DbType.String, ColumnProperty.NotNull)
            };
            if (columns.Any())
            {
                dictColumns.RemoveAll(x => columns.Any(y => y.Name == x.Name));
            }

            dictColumns.AddRange(columns);
            database.AddEntityTable(tableName, dictColumns.ToArray());
        }

        /// <summary>
        /// Изменить максимальный размер string'овой колонки
        /// </summary>
        /// <param name="database">Провайдер БД</param>
        /// <param name="table">Таблица</param>
        /// <param name="columnName">Наименование колонки</param>
        /// <param name="size">Новый максимальный размер</param>
        public static void StringColumnChangeSize(this ITransformationProvider database, SchemaQualifiedObjectName table, string columnName, int size = 255)
        {
            var column = new Column(columnName, DbType.String.WithSize(size));

            database.ChangeColumn(table, column);
        }
    }
}