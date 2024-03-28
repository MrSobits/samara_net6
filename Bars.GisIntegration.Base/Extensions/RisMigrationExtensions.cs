namespace Bars.GisIntegration.Base.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.B4.Utils;

    public static class RisMigrationExtensions
    {
        /// <summary>
        /// Добавить таблицу базовой сущности BaseEntity с дополнительными колонками
        /// </summary>
        /// <param name="database">ITransformationProvider</param>
        /// <param name="tableName">Имя таблицы</param>
        /// <param name="columns">Дополнительные колонки</param>
        public static void AddRisEntityTable(this ITransformationProvider database, string tableName,
            params Column[] columns)
        {
            var dictColumns = new List<Column>
            {
                new Column("OPERATION", DbType.Int16, ColumnProperty.NotNull, "0"),
                new Column("EXTERNAL_ID", DbType.Int64, ColumnProperty.NotNull),
                new Column("EXTERNAL_SYSTEM_NAME", DbType.String, 50, ColumnProperty.NotNull, "'gkh'"),
                new RefColumn("GI_CONTRAGENT_ID", string.Format("{0}_CONTRAGENT", tableName), "GI_CONTRAGENT", "ID"),
                new Column("GUID", DbType.String, 50)
            };

            RisMigrationExtensions.RemoveReplaceableColumns(dictColumns, columns);
            dictColumns.AddRange(columns);

            database.AddEntityTable(tableName, dictColumns.ToArray());
        }

        ///// <summary>
        ///// Добавить таблицу базовой сущности BaseRisEntity с доп. колонками начисления по услуге
        ///// </summary>
        ///// <param name="database">ITransformationProvider</param>
        ///// <param name="tableName">Имя таблицы</param>
        ///// <param name="columns">Дополнительные колонки</param>
        //public static void AddRisChargeInfoTable(
        //    this ITransformationProvider database,
        //    string tableName,
        //    params Column[] columns)
        //{
        //    var dictColumns = new List<Column>
        //    {
        //        new Column("OBJECT_VERSION", DbType.Int64, ColumnProperty.NotNull),
        //        new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
        //        new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull),
        //        new Column("EXTERNAL_ID", DbType.Int64, ColumnProperty.NotNull),
        //        new Column("EXTERNAL_SYSTEM_NAME", DbType.String, 50, ColumnProperty.NotNull, "'gkh'"),
        //        new RefColumn("GI_CONTAINER_ID", string.Format("{0}_CONTAINER", tableName), "GI_CONTAINER", "ID"),
        //        new Column("GUID", DbType.String, 50),

        //        new Column("SERVICE_TYPE_CODE", DbType.String, 20, ColumnProperty.NotNull),
        //        new Column("SERVICE_TYPE_GUID", DbType.String, 50, ColumnProperty.NotNull),
        //        new Column("OKEI", DbType.String, 3),
        //        new Column("RATE", DbType.Decimal),
        //        new Column("INDIVIDUAL_CONSUMPTION_CURRENT_VALUE", DbType.Decimal),
        //        new Column("HOUSE_OVERALL_NEEDS_CURRENT_VALUE", DbType.Decimal),
        //        new Column("HOUSE_TOTAL_INDIVIDUAL_CONSUMPTION", DbType.Decimal),
        //        new Column("HOUSE_TOTAL_OVERALL_NEEDS", DbType.Decimal),
        //        new Column("HOUSE_OVERALL_NEEDS_NORM", DbType.Decimal),
        //        new Column("INDIVIDUAL_CONSUMPTION_NORM", DbType.Decimal),
        //        new Column("INDIVIDUAL_CONSUMPTION", DbType.Decimal),
        //        new Column("HOUSE_OVERALL_NEEDS_CONSUMPTION", DbType.Decimal)
        //    };

        //    RisMigrationExtensions.RemoveReplaceableColumns(dictColumns, columns);
        //    dictColumns.AddRange(columns);

        //    database.AddPersistentObjectTable(tableName, dictColumns.ToArray());
        //}

        /// <summary>
        /// Добавить таблицу сущности NsiRef с дополнительными колонками
        /// </summary>
        /// <param name="database">ITransformationProvider</param>
        /// <param name="tableName">Имя таблицы</param>
        /// <param name="columns">Дополнительные колонки</param>
        [Obsolete("Оставлен чтобы не ломать старые миграции.")]
        public static void AddNsiRefTable(this ITransformationProvider database, string tableName,
            params Column[] columns)
        {   
            var dictColumns = new List<Column>
            {
                new Column("EXTERNAL_ID", DbType.Int64, ColumnProperty.NotNull),
                new Column("EXTERNAL_SYSTEM_NAME", DbType.String, 50, ColumnProperty.NotNull, "'gkh'"),
                new RefColumn("GI_CONTAINER_ID", string.Format("{0}_CONTAINER", tableName), "GI_CONTAINER", "ID"),
                new Column("GUID", DbType.String, 50),

                new RefColumn("GI_NSICATALOG_ID", string.Format("{0}_NSIREFCATALOG", tableName), "GI_NSICATALOG", "ID"),
                new Column("VALUE", DbType.String, 100),
                new Column("CODE", DbType.String, 50)
            };

            RisMigrationExtensions.RemoveReplaceableColumns(dictColumns, columns);
            dictColumns.AddRange(columns);

            database.AddEntityTable(tableName, dictColumns.ToArray());
        }

        /// <summary>Удалить колонки по списку</summary>
        /// <param name="dictColumns">Колонки</param>
        /// <param name="columns">Список</param>
        private static void RemoveReplaceableColumns(List<Column> dictColumns, IList<Column> columns)
        {
            if (columns.Any())
            {
                dictColumns.RemoveAll(x => columns.Any(y => y.Name == x.Name));
            }
        }

        public static IList<ForeignKey> GetForeignKeys(this ITransformationProvider database, string tableName)
        {
            tableName = tableName.ToLower();

            var query =
                $@"SELECT tc.constraint_name,
                          tc.table_schema AS schema_name,
                          tc.table_name AS table_name,
                          kcu.column_name, 
                          ccu.table_schema AS foreign_schema_name,
                          ccu.table_name AS foreign_table_name,
                          ccu.column_name AS foreign_column_name
FROM information_schema.table_constraints tc
JOIN information_schema.key_column_usage kcu ON tc.constraint_name = kcu.constraint_name
JOIN information_schema.constraint_column_usage ccu ON ccu.constraint_name = tc.constraint_name
WHERE constraint_type = 'FOREIGN KEY' AND ccu.table_name='{tableName}'";

            var fks = new List<ForeignKey>();
            using (var reader = database.ExecuteQuery(query))
            {
                while (reader.Read())
                {
                    fks.Add(
                        new ForeignKey
                        {
                            ConstraintName = reader["constraint_name"].ToStr(),
                            TableName = reader["table_name"].ToStr(),
                            ColumnName = reader["column_name"].ToStr(),
                            ForeignTableName = reader["foreign_table_name"].ToStr(),
                            ForeignColumnName = reader["foreign_column_name"].ToStr()
                        });
                }
            }

            return fks;
        }

        public class ForeignKey
        {
            public string ConstraintName { get; set; }

            public string TableName { get; set; }

            public string ColumnName { get; set; }

            public string ForeignTableName { get; set; }

            public string ForeignColumnName { get; set; }
        }
    }
}