namespace Bars.Gkh.ExecutionAction.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.SystemDataTransfer.Meta;

    using Dapper;

    using NHibernate.Persister.Entity;
    using NHibernate.Transform;

    /// <summary>
    /// Добавление поля IMPORT_ENTITY_ID (для синхронизации Воронежа)
    /// </summary>
    public class AddEntityImportIdAction : BaseExecutionAction
    {
        private const int Timeout = 10000;

        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description => "Добавление поля IMPORT_ENTITY_ID";

        /// <summary>
        /// Название для отображения
        /// </summary>
        public override string Name => "ЖКХ - Добавление поля IMPORT_ENTITY_ID";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;

        public ISessionProvider SessionProvider { get; set; }

        private readonly string columnExistsQuery = @"SELECT table_name as TableName
                FROM information_schema.columns
                WHERE table_schema = 'public'
                and column_name = 'import_entity_id'";

        private BaseDataResult Execute()
        {
            var session = this.SessionProvider.GetCurrentSession();

            var columnExistTables = session.CreateSQLQuery(this.columnExistsQuery)
                .SetResultTransformer(Transformers.AliasToBean<QueryDto>())
                .List<QueryDto>()
                .Select(x => x.TableName)
                .ToList();

            //получаем всех наследников BaseImportableEntity
            var list = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.DefinedTypes.Where(type => type.Is<IImportableEntity>()))
                .ToList();

            var alterScripts = new List<string>();
            list.ForEach(
                x =>
                {
                    var typeMeta = session.SessionFactory.GetClassMetadata(x) as AbstractEntityPersister;
                    if (typeMeta != null
                        && !columnExistTables.Contains(typeMeta.TableName.ToLower())
                        && ((typeMeta as JoinedSubclassEntityPersister)?.HasSubclasses ?? true))
                    {
                        alterScripts.Add($"ALTER TABLE IF EXISTS {typeMeta.TableName} ADD COLUMN IMPORT_ENTITY_ID BIGINT");
                    }
                });

            var query = string.Join(";", alterScripts.Distinct());

            this.SessionProvider.InStatelessConnectionTransaction(
                (connection, transaction) => { connection.Execute(query, transaction: transaction, commandTimeout: AddEntityImportIdAction.Timeout); });

            return new BaseDataResult();
        }

        /// <inheritdoc />
        /*
        public override bool IsNeedAction()
        {
            var session = this.SessionProvider.GetCurrentSession();

            var tableExistsQuery = @"SELECT table_name as TableName
                FROM information_schema.tables
                WHERE table_schema = 'public'";

            var columnExistTables = session.CreateSQLQuery(this.columnExistsQuery)
                .SetResultTransformer(Transformers.AliasToBean<QueryDto>())
                .List<QueryDto>()
                .Select(x => x.TableName)
                .ToList();

            var existsTables = session.CreateSQLQuery(tableExistsQuery)
                .SetResultTransformer(Transformers.AliasToBean<QueryDto>())
                .List<QueryDto>()
                .Select(x => x.TableName)
                .ToList();

            //получаем всех наследников BaseImportableEntity
            var list = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.DefinedTypes.Where(type => type.Is<IImportableEntity>()))
                .ToList();

            foreach (var typeInfo in list)
            {
                var typeMeta = session.SessionFactory.GetClassMetadata(typeInfo) as AbstractEntityPersister;
                if (typeMeta == null || !existsTables.Contains(typeMeta.TableName.ToLower()))
                {
                    continue;
                }
                if (!columnExistTables.Contains(typeMeta.TableName.ToLower())
                    && ((typeMeta as JoinedSubclassEntityPersister)?.HasSubclasses ?? true))
                {
                    return true;
                }
            }

            return false;
        }
        */
        protected struct QueryDto
        {
            public string TableName { get; set; }
        }
    }
}