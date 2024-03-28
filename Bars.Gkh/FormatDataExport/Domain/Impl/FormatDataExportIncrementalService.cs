namespace Bars.Gkh.FormatDataExport.Domain.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Analytics.Reports.Extensions;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;

    using Castle.Core;
    using Castle.Windsor;

    using Dapper;

    using NHibernate;
    using NHibernate.Util;

    public class FormatDataExportIncrementalService : IFormatDataExportIncrementalService, IInitializable, IDisposable
    {
        /// <summary>
        /// Имя таблицы
        /// </summary>
        public const string TableName = "GKH_FORMAT_DATA_EXPORT_INCREMENTAL_DATA";

        private readonly List<FormatDataExportRowInfo> dataInfoToStore = new List<FormatDataExportRowInfo>();
        private readonly ISet<string> ignoreSectionCodes = new HashSet<string>();

        private HashAlgorithm HashSum { get; set; }
        private IStatelessSession StatelessSession { get; set; }

        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public void Initialize()
        {
            this.HashSum = MD5.Create();
            this.StatelessSession = this.Container.Resolve<ISessionProvider>().OpenStatelessSession();
        }

        public void SetIgnoreEtities(IList<string> entityCodeList)
        {
            entityCodeList.ForEach(x => this.ignoreSectionCodes.Add(x));
        }

        /// <inheritdoc />
        public IList<ExportableRow> GetIncrementalData(string entityCode, IList<ExportableRow> data)
        {
            if (this.ignoreSectionCodes.Contains(entityCode))
            {
                return data;
            }

            var nowDate = DateTime.Now;

            var storedDataInfo = this.GetStoredDataInfo(entityCode);

            var newDataInfo = data.Select(x => new FormatDataExportRowInfo
                {
                    EntityCode = entityCode,
                    ExportDate = nowDate,
                    RowId = x.Id,
                    HashSum = this.GetHash(x)
                })
                .ToList();

            var delta = newDataInfo.Except(storedDataInfo, new FormatDataExportRowInfoComparator())
                .ToList();

            this.dataInfoToStore.AddRange(delta);

            var deltaIds = delta.Select(x => x.RowId).ToList();

            return data.Where(x => deltaIds.Contains(x.Id)).ToList();
        }

        private IEnumerable<FormatDataExportRowInfo> GetStoredDataInfo(string entityCode)
        {
            var connection = this.StatelessSession.Connection;

            var sql = $@"SELECT ENTITY_CODE as EntityCode, EXPORT_DATE as ExportDate, ROW_ID as RowId, HASH_SUM as HashSum
            FROM {FormatDataExportIncrementalService.TableName}
            WHERE ENTITY_CODE = '{entityCode}';";

            return connection.Query<FormatDataExportRowInfo>(sql).ToList();
        }

        /// <inheritdoc />
        public void SaveNewDataInfo()
        {
            var connection = this.StatelessSession.Connection;

            var infoDict = this.dataInfoToStore.GroupBy(x => x.EntityCode)
                .ToDictionary(x => x.Key, x => x.ToList());

            using (var transaction = connection.BeginTransaction())
            {
                var deleteSql = new StringBuilder();
                var insertSql = $@"INSERT INTO {FormatDataExportIncrementalService.TableName} (
                    ENTITY_CODE, EXPORT_DATE, ROW_ID, HASH_SUM
                ) VALUES (
                    @EntityCode, @ExportDate, @RowId, @HashSum
                );";
                foreach (var info in infoDict)
                {
                    var rowIds = string.Join(",", info.Value.Select(x => x.RowId.ToString()));

                    deleteSql.AppendFormat("DELETE FROM {0} WHERE ENTITY_CODE = '{1}' AND ROW_ID IN ({2});\n",
                        FormatDataExportIncrementalService.TableName,
                        info.Key,
                        rowIds);
                }

                connection.Execute(deleteSql.ToString(), null, transaction);
                connection.Execute(insertSql, this.dataInfoToStore.ToArray(), transaction);

                transaction.Commit();
                this.dataInfoToStore.Clear();
            }
        }

        private byte[] GetHash(ExportableRow row)
        {
            var data = Encoding.GetEncoding(1251).GetBytes(row.Cells.Values.AggregateWithSeparator(";"));

            return this.HashSum.ComputeHash(data);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.HashSum?.Dispose();
            this.StatelessSession?.Dispose();
        }

        private class FormatDataExportRowInfo
        {
            public string EntityCode { get; set; }
            public DateTime ExportDate { get; set; }
            public long RowId { get; set; }
            public byte[] HashSum { get; set; }
        }

        private class FormatDataExportRowInfoComparator : IEqualityComparer<FormatDataExportRowInfo>
        {
            /// <inheritdoc />
            public bool Equals(FormatDataExportRowInfo x, FormatDataExportRowInfo y)
            {
                if (x.RowId != y.RowId || x.HashSum.Length != y.HashSum.Length)
                {
                    return false;
                }

                for (int i = 0; i < x.HashSum.Length; i++)
                {
                    if (x.HashSum[i] != y.HashSum[i])
                    {
                        return false;
                    }
                }

                return true;
            }

            /// <inheritdoc />
            public int GetHashCode(FormatDataExportRowInfo obj)
            {
                return 42; // Для срабатывания Equals
            }
        }
    }
}