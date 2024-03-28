namespace Bars.Gkh.RegOperator.Utils.OrmChecker.Database
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;

    using Castle.Windsor;

    using NHibernate.Cfg;

    /// <summary>Провайдер для получения информации об таблицах на основании маппинга NHibernate</summary>
    public class NhibernateInfoProvider
    {
        private readonly IWindsorContainer container;

        private readonly Dictionary<string, List<TableColumnInfo>> dictColumns;
        private readonly Dictionary<string, List<TableConstraintInfo>> dictConstraints;
        private readonly Dictionary<string, List<TableIndexInfo>> dictIndexes;

        public NhibernateInfoProvider(IWindsorContainer container)
        {
            this.container = container;
            dictColumns = new Dictionary<string, List<TableColumnInfo>>();
            dictConstraints = new Dictionary<string, List<TableConstraintInfo>>();
            dictIndexes = new Dictionary<string, List<TableIndexInfo>>();
        }

        /// <summary>Получить все таблицы</summary>
        /// <returns>Словарь</returns>
        public Dictionary<Type, TableInfo> GetAllTableInfo()
        {
            var result = new Dictionary<Type, TableInfo>();
            
            FillTableColumns();
            //FillTableConstrains();
            //FillTableIndexes();

            var subClasses = new Dictionary<TableInfo, string>();

            var nhconfig = container.Resolve<Configuration>();
            foreach (var mapping in nhconfig.ClassMappings
                                            .Where(x => !x.MappedClass.IsAbstract)
                                            .Where(x => x.RootTable != null && !string.IsNullOrEmpty(x.RootTable.Name)))
            {
                var tableName = mapping.Table.Name;
                var tableInfo = new TableInfo { TableName = tableName };

                FillByDicts(tableInfo, tableName);

                // JoinedSubclassMapping
                if (mapping.IsJoinedSubclass && tableName != mapping.RootTable.Name)
                {
                    subClasses.Add(tableInfo, mapping.RootTable.Name);
                }

                result.Add(mapping.MappedClass, tableInfo);
            }

            foreach (var subClass in subClasses)
            {
                subClass.Key.IsJoinedSubClass = true;
                subClass.Key.Parent = result.Values.Single(x => x.TableName == subClass.Value);
            }

            return result;
        }

        private void FillByDicts(TableInfo ClassInfo, string tableName)
        {
            var tableNameUpper = tableName.ToUpper();
            if (dictColumns.ContainsKey(tableNameUpper))
            {
                ClassInfo.Columns.AddRange(dictColumns[tableNameUpper].Where(x => ClassInfo.Columns.All(y => y.ColumnName != x.ColumnName)));
            }

            if (dictConstraints.ContainsKey(tableNameUpper))
            {
                ClassInfo.Constraints.AddRange(dictConstraints[tableNameUpper].Where(x => ClassInfo.Constraints.All(y => y.Name != x.Name)));
            }

            if (dictIndexes.ContainsKey(tableNameUpper))
            {
                ClassInfo.Indexes.AddRange(dictIndexes[tableNameUpper].Where(x => ClassInfo.Indexes.All(y => y.Name != x.Name)));
            }
        }

        private void FillTableIndexes()
        {
            dictIndexes.Clear();

            var session = container.Resolve<ISessionProvider>().GetCurrentSession();

            var data = session.CreateSQLQuery(@"select s.table_name, s.index_name, decode(s.uniqueness,'UNIQUE',1,0) as is_unique, s.index_type, i.column_name, c.data_default
from user_indexes s, user_ind_columns i, user_tab_cols c    
where s.index_name = i.index_name
and i.table_name = c.table_name
and i.column_name = c.column_name
order by s.index_name, i.column_position").List();
            foreach (object[] record in data)
            {
                var tableName = record[0].ToStr();
                var indexName = record[1].ToStr();

                if (!dictIndexes.ContainsKey(tableName))
                {
                    dictIndexes.Add(tableName, new List<TableIndexInfo>());
                }

                var indexInfo = dictIndexes[tableName].SingleOrDefault(x => x.Name == indexName);
                if (indexInfo == null)
                {
                    indexInfo = new TableIndexInfo
                    {
                        Name = indexName,
                        Unique = record[2].ToBool(),
                        Type = record[3].ToStr(),
                        Formula = record[5].ToStr()
                    };

                    dictIndexes[tableName].Add(indexInfo);
                }

                indexInfo.Columns.Add(record[4].ToStr());
            }
        }

        private void FillTableConstrains()
        {
            dictConstraints.Clear();

            var session = container.Resolve<ISessionProvider>().GetCurrentSession();

            var data = session.CreateSQLQuery(@"SELECT tc.table_name, tc.constraint_name, tc.constraint_type, null as search_condition, ccu.table_name AS references_table, ccu.column_name AS references_field, case when tc.is_deferrable = 'NO' then 0 else 1 end as is_deferrable, kcu.column_name
FROM information_schema.table_constraints tc
LEFT JOIN information_schema.key_column_usage kcu
ON tc.constraint_catalog = kcu.constraint_catalog
AND tc.constraint_schema = kcu.constraint_schema
AND tc.constraint_name = kcu.constraint_name
LEFT JOIN information_schema.referential_constraints rc
ON tc.constraint_catalog = rc.constraint_catalog
AND tc.constraint_schema = rc.constraint_schema
AND tc.constraint_name = rc.constraint_name
LEFT JOIN information_schema.constraint_column_usage ccu
ON rc.unique_constraint_catalog = ccu.constraint_catalog
AND rc.unique_constraint_schema = ccu.constraint_schema
AND rc.unique_constraint_name = ccu.constraint_name").List();
            foreach (object[] record in data)
            {
                var tableName = record[0].ToStr();

                if (!dictConstraints.ContainsKey(tableName))
                {
                    dictConstraints.Add(tableName, new List<TableConstraintInfo>());
                }

                dictConstraints[tableName].Add(new TableConstraintInfo
                {
                    Name = record[1].ToStr(),
                    Type = record[2].ToStr(),
                    Text = record[3].ToStr(),
                    ReferenceTableName = record[4].ToStr(),
                    ReferenceColumnName = record[5].ToStr(),
                    Deferrable = record[6].ToBool(),
                    ColumnName = record[7].ToStr().ToUpper()
                });
            }
        }

        private void FillTableColumns()
        {
            dictColumns.Clear();

            var session = container.Resolve<ISessionProvider>().GetCurrentSession();
            var data = session
                .CreateSQLQuery("select c.table_name, c.column_name, c.data_type, c.character_maximum_length, case when c.is_nullable = 'YES' then 0 else 1 end as notnull, c.numeric_precision, c.numeric_scale, c.column_default from information_schema.columns c")
                .List();

            foreach (object[] record in data)
            {
                var tableName = record[0].ToStr().ToUpper();

                if (!dictColumns.ContainsKey(tableName))
                {
                    dictColumns.Add(tableName, new List<TableColumnInfo>());
                }

                dictColumns[tableName].Add(new TableColumnInfo
                {
                    ColumnName = record[1].ToStr().ToUpper(),
                    DataType = record[2].ToStr(),
                    Size = record[3].ToInt(),
                    NotNull = record[4].ToBool(),
                    Precision = record[5].ToInt(),
                    Scale = record[6].ToInt(),
                    DefaultValue = record[7].ToStr()
                });
            }
        }
    }
}