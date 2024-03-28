namespace Bars.Gkh.RegOperator.Imports.Ches.PreImport
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Microsoft.Extensions.Logging;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    using Dapper;

    using Npgsql;

    /// <inheritdoc />
    public class ChesTempDataProvider : ChesTempDataProvider<IPeriodImportFileInfo>
    {
        /// <inheritdoc />
        public ChesTempDataProvider(IWindsorContainer container,
            IPeriodImportFileInfo fileInfo,
            BaseParams baseParams)
            : base(container, fileInfo, baseParams)
        {
        }
    }

    /// <summary>
    /// Провайде для работы с временными таблицами импорта ЧЭС
    /// </summary>
    public abstract class ChesTempDataProvider<T> : IChesTempDataProvider
        where T : IPeriodImportFileInfo
    {
        protected readonly IWindsorContainer Container;
        protected readonly BaseParams BaseParams;

        IPeriodImportFileInfo IChesTempDataProvider.FileInfo => this.FileInfo;

        public T FileInfo { get; }

        public ChargePeriod Period => this.FileInfo.Period;

        protected ChesTempDataProvider(IWindsorContainer container, T fileInfo, BaseParams baseParams)
        {
            this.Container = container;
            this.FileInfo = fileInfo;
            this.BaseParams = baseParams;
            var tableName = $"CHES_{this.FileInfo.FileType.ToString().ToUpper()}_{this.Period?.Id}";
            this.TableName = new SchemaQualifiedObjectName
            {
                Name = tableName,
                Schema = "IMPORT"
            };
            this.SummaryViewName = new SchemaQualifiedObjectName
            {
                Name = "view_" + tableName,
                Schema = "IMPORT"
            };
        }

        /// <inheritdoc />
        public SchemaQualifiedObjectName TableName { get; }

        /// <inheritdoc />
        public SchemaQualifiedObjectName SummaryViewName { get; }

        /// <inheritdoc />
        public SchemaQualifiedObjectName CheckViewName { get; protected set; }

        /// <inheritdoc />
        public virtual void Import()
        {
            this.Container.InStatelessConnectionTransaction(
                (con, tr) =>
                {
                    this.CreateSchema(con);
                    this.CreateTable(con);
                    this.InStatelessImportAction(con, tr);
                    this.UpdateSummaryView(con, tr);
                });
        }

        /// <inheritdoc />
        public Stream GetOutputStream()
        {
            Stream stream = Stream.Null;

            this.Container.InStatelessConnectionTransaction((connection, transaction) =>
            {
                stream = this.InStatelessExportAction(connection);
            });

            return stream;
        }

        /// <inheritdoc />
        public void DropData()
        {
            var session = this.Container.Resolve<ISessionProvider>().GetCurrentSession();
            var transformProvider = MigratorUtils.GetTransformProvider(session.Connection);

            this.DropDataInternal(transformProvider);
        }

        /// <inheritdoc />
        public IDictionary<string, object>[] GetData()
        {
            IDictionary<string, object>[] resultData = null;
            var columns = this.FileInfo.GetColumns();

            this.Container.InStatelessConnectionTransaction((connection, transaction) =>
            {
                resultData = connection
                    .Query($"SELECT {string.Join(",", columns.Select(x => $"{x.Value.Name.ToLower()} AS {x.Key}"))} "
                        + $"FROM {this.TableName}")
                    .Cast<IDictionary<string, object>>().ToArray();
            });

            return resultData;
        }

        /// <inheritdoc />
        public virtual IDictionary<string, object> GetSummaryData(BaseParams baseParams)
        {
            IDictionary<string, object> resultData = null;
            var columns = this.FileInfo.GetSummaryColumns();

            this.Container.InStatelessConnectionTransaction((connection, transaction) =>
            {
                var transformProvider = MigratorUtils.GetTransformProvider(connection);
                if (transformProvider.MaterializedViewExists(this.SummaryViewName))
                {
                    resultData = connection
                        .Query($"SELECT {string.Join(",", columns.Select(x => $"{x.ColumnName} AS \"{x.PropertyName}\""))} "
                            + $"FROM {this.SummaryViewName}")
                        .First() as IDictionary<string, object>;
                }
                else
                {
                    resultData = new Dictionary<string, object>();
                }
                
            });

            return resultData;
        }

        protected virtual void InStatelessImportAction(IDbConnection connection, IDbTransaction transaction)
        {
            var transformProvider = MigratorUtils.GetTransformProvider(connection);
            var allFields = this.GetColumns();
            var copyFields = this.GetColumns(true);

            var connectionToCopy = connection as NpgsqlConnection;
            if (connectionToCopy.IsNull())
            {
                throw new InvalidCastException("connection must be a NpgsqlConnection");
            }

            var sqlQuery = $@"
                COPY {this.TableName} ({string.Join(",", copyFields.Select(x => x.Name))}) 
                FROM STDIN
                WITH DELIMITER ';'
                NULL ''
                QUOTE '}}'
                HEADER 
                CSV";

            try
            {
                using (var memoryStream = new MemoryStream(this.FileInfo.FileData.Data))
                {
                    using (var copier = connectionToCopy.BeginTextImport(sqlQuery))
                    {
                        using (var sr = new StreamReader(memoryStream, Encoding.GetEncoding(1251)))
                        {
                            while (!sr.EndOfStream)
                            {
                                copier.WriteLine(sr.ReadLine());
                            }
                        }
                    }
                }

                // строим индексы
                foreach (var indexColumn in allFields.Where(x => x.HasIndex))
                {
                    transformProvider.AddIndex($"{this.TableName.Name}_{indexColumn.Name}", false, this.TableName, indexColumn.Name);
                }
                
                transformProvider.ExecuteNonQuery($"ANALYZE {this.TableName}");
            }
            catch (PostgresException exception)
            {
                var errorMsg = $"Данные файла {this.FileInfo.FileData.FileName} не прошли валидацию при импорте в таблицу {this.TableName}{Environment.NewLine}"
                    + $"Ошибка: {exception.Message}{Environment.NewLine}"
                    + $"Информация об ошибке: {exception.Message}{Environment.NewLine}"
                    + $"Строка: {exception.Line}{Environment.NewLine}"
                    + $"Детали ошибки: {exception.Where}";

                ApplicationContext.Current.Container.Resolve<ILogger>().LogError(errorMsg, exception);
                throw new ValidationException(errorMsg, exception);
            }
        }

        protected virtual void CreateSchema(IDbConnection connection)
        {
            var transformProvider = MigratorUtils.GetTransformProvider(connection);
            transformProvider.ExecuteNonQuery($"CREATE SCHEMA IF NOT EXISTS {this.TableName.Schema}");
        }

        protected virtual void CreateTable(IDbConnection connection, bool isDelete = true)
        {
            var transformProvider = MigratorUtils.GetTransformProvider(connection);
            var allFields = this.GetColumns();

            if (isDelete)
            {
                // на данном этапе удаляем вью и таблицу, если кто-то на них ссылается, схватим ошибку и импорт прекратится
                this.DropDataInternal(transformProvider);
            }

            if (!transformProvider.TableExists(this.TableName))
            {
                transformProvider.AddTable(this.TableName, allFields.Select(x => x.Column).ToArray());
            }
        }

        private Stream InStatelessExportAction(IDbConnection connection)
        {
            var fields = this.GetColumns();

            var connectionToCopy = connection as NpgsqlConnection;
            if (connectionToCopy.IsNull())
            {
                throw new InvalidCastException("connection must be a NpgsqlConnection");
            }

            var sqlQuery = this.GetOutSqlQuery(fields);

            MemoryStream memoryStream = null;

            try
            {
                memoryStream = new MemoryStream();

                using (var copier = connectionToCopy.BeginTextExport(sqlQuery))
                {
                    byte[] byteArray;
                    while ((byteArray = Encoding.GetEncoding(1251).GetBytes(copier.ReadLine())) != null)
                    {
                        memoryStream.Write(byteArray, 0, byteArray.Length);
                        byteArray = null;
                    }
                }

                memoryStream.Seek(0, SeekOrigin.Begin);
            }
            catch (PostgresException exception)
            {
                var errorMsg = $"Данные файла {this.FileInfo.FileData.FileName} не прошли валидацию при импорте в таблицу {this.TableName}{Environment.NewLine}"
                    + $"Ошибка: {exception.Message}{Environment.NewLine}"
                    + $"Информация об ошибке: {exception.Message}{Environment.NewLine}"
                    + $"Строка: {exception.Line}{Environment.NewLine}"
                    + $"Детали ошибки: {exception.Where}";

                memoryStream?.Dispose();

                ApplicationContext.Current.Container.Resolve<ILogger>().LogError(errorMsg, exception);
                throw new ValidationException(errorMsg, exception);
            }

            return memoryStream;
        }

        protected virtual string GetOutSqlQuery(ColumnDef[] fields)
        {
            var sqlQuery = $@"
                COPY {this.TableName} ({string.Join(",", fields.Select(x => x.Name))}) 
                TO STDOUT
                WITH DELIMITER ';'
                NULL ''
                QUOTE '}}'
                HEADER 
                CSV
                encoding 'windows-1251'";
            return sqlQuery;
        }

        private void DropDataInternal(ITransformationProvider transformProvider)
        {
            transformProvider.ExecuteNonQuery($"DROP MATERIALIZED VIEW IF EXISTS {this.SummaryViewName}");
            if (this.CheckViewName != null)
            {
                transformProvider.ExecuteNonQuery($"DROP MATERIALIZED VIEW IF EXISTS {this.CheckViewName}");
            } 
            transformProvider.ExecuteNonQuery($"DROP TABLE IF EXISTS {this.TableName}");
        }

        protected virtual string GetSummaryViewCreateSql()
        {
            var summaryViewFields = this.FileInfo.GetSummaryColumns();
            if (summaryViewFields.Any())
            {
                var columns = string.Join(",", summaryViewFields.Select(x => $"sum({x.Formula}) \"{x.ColumnName.ToLower()}\""));
                return $"CREATE MATERIALIZED VIEW {this.SummaryViewName} AS "
                    + $"SELECT {columns} FROM {this.TableName} WITH DATA";
            }
            else
            {
                return string.Empty;
            }
        }

        protected virtual string GetCheckViewCreateSql()
        {
            return string.Empty;
        }

        protected void UpdateSummaryView(IDbConnection connection, IDbTransaction transaction)
        {
            var summaryViewCreateSql = this.GetSummaryViewCreateSql();
            this.CreateOrRefreshView(connection, transaction, this.SummaryViewName, summaryViewCreateSql);
        }

        /// <inheritdoc />
        public void UpdateSummaryView()
        {
            if (this.SummaryViewName != null)
            {
                this.Container.InStatelessConnectionTransaction(
                    (con, tr) =>
                    {
                        var summaryViewCreateSql = this.GetSummaryViewCreateSql();
                        this.CreateOrRefreshView(con, tr, this.SummaryViewName, summaryViewCreateSql);
                    });
            }
        }

        public void UpdateCheckView()
        {
            if (this.CheckViewName != null)
            {
                this.Container.InStatelessConnectionTransaction(
                    (con, tr) =>
                    {
                        var checkViewCreateSql = this.GetCheckViewCreateSql();
                        this.CreateOrRefreshView(con, tr, this.CheckViewName, checkViewCreateSql);
                    });
            }
        }

        private void CreateOrRefreshView(IDbConnection connection, IDbTransaction transaction, SchemaQualifiedObjectName viewName, string createSql)
        {
            if (viewName != null && !string.IsNullOrEmpty(createSql))
            {
                if (!DatabaseExtensions.MaterializedViewExists(connection, transaction, viewName))
                {
                    connection.Execute(createSql, transaction: transaction);
                }
                else
                {
                    connection.Execute($"REFRESH MATERIALIZED VIEW {viewName}");
                }
            }
        }

        protected ColumnDef[] GetColumns(bool onlyImportFields = false)
        {
            return this.FileInfo.GetColumns().Values.WhereIf(onlyImportFields, x => x.HasImport).ToArray();
        }
    }
}