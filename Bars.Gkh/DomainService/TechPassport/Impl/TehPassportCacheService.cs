namespace Bars.Gkh.DomainService.TechPassport.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Config;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    using Castle.Core;
    using Castle.Core.Internal;
    using Castle.Windsor;

    using Dapper;

    /// <summary>
    /// Сервис для работы с кэшем технического паспорта
    /// </summary>
    public class TehPassportCacheService : ITehPassportCacheService, IInitializable
    {
        private static readonly object locker = new object();

        public ISessionProvider SessionProvider { get; set; }
        public IWindsorContainer Container { get; set; }

        public IDomainService<TehPassportCacheCell> TehPassportCacheCellDomain { get; set; }

        private const string NamespaceName = "mat";
        private const string ViewName = "view_tp_teh_passport";
        private const string DropCacheSeqName = "view_tp_teh_passport_drop_cache_seq";
        public const string ViewFullName = TehPassportCacheService.NamespaceName + "." + TehPassportCacheService.ViewName;
        private const string DropCacheSeqNameFullName = TehPassportCacheService.NamespaceName + "." + TehPassportCacheService.DropCacheSeqName;
        private const int BulkSize = 5000;

        /// <summary>
        /// Время жизни кэша в минутах
        /// </summary>
        private readonly int cacheDuration;

        /// <summary>
        /// Следующее время сброса кэша
        /// </summary>
        private DateTime dropCacheDateTime;

        private bool CanDropCache => this.dropCacheDateTime < DateTime.UtcNow;

        public TehPassportCacheService(IConfigProvider configProvider)
        {
            var defaultDuration = 60;
            this.cacheDuration = configProvider?.GetConfig().AppSettings.GetAs("TehPassportCacheDuration", defaultDuration, true) ?? defaultDuration;
        }

        public void Initialize()
        {
            using (var connection = this.GetNewConnection())
            {
                this.dropCacheDateTime = this.GetDropCacheTime(connection); 
            }
        }

        private void UpdateCacheTable()
        {
            if (!this.CanDropCache)
            {
                return;
            }

            lock (TehPassportCacheService.locker)
            {
                if (this.CanDropCache)
                {
                    this.CreateOrUpdateCacheTable();
                }
            }
        }

        /// <inheritdoc />
        public string GetValue(long realityObjectId, string formCode, int row, int column)
        {
            this.UpdateCacheTable();
            return this.TehPassportCacheCellDomain
                .FirstOrDefault(x => x.FormCode == formCode && x.RowId == row
                && x.ColumnId == column && x.RealityObjectId == realityObjectId)?
                .Value;
        }

        /// <inheritdoc />
        public Dictionary<long, string> GetCacheByRealityObjects(string formCode, int row, int column, ICollection<long> filterIds)
        {
            this.UpdateCacheTable();
            var query = this.TehPassportCacheCellDomain.GetAll()
                .Where(x => x.FormCode == formCode && x.RowId == row && x.ColumnId == column);

            if (filterIds.IsNullOrEmpty())
            {
               return query.AsEnumerable()
                    .GroupBy(x => x.RealityObjectId, x => x.Value)
                    .ToDictionary(
                        x => x.Key,
                        x => x.FirstOrDefault(v => v.IsNotEmpty()));
            }

            var count = filterIds.Count;
            var result = new List<TehPassportCacheCell>();

            for (var skip = 0; skip < count; skip += TehPassportCacheService.BulkSize)
            {
                var partIds = filterIds
                    .Skip(skip)
                    .Take(TehPassportCacheService.BulkSize);

                var partResult = query.Where(x => partIds.Contains(x.RealityObjectId))
                    .ToList();

                result.AddRange(partResult);
            }

            return result.GroupBy(x => x.RealityObjectId, x => x.Value)
                .ToDictionary(
                    x => x.Key,
                    x => x.FirstOrDefault(v => v.IsNotEmpty()));
        }

        /// <inheritdoc />
        public List<TehPassportCacheCell> GetCacheByRealityObjectsAndRows(string formCode, int column, ICollection<long> filterIds)
        {
            this.UpdateCacheTable();
            var query = this.TehPassportCacheCellDomain.GetAll()
                .Where(x => x.FormCode == formCode && x.ColumnId == column);

            if (filterIds.IsNullOrEmpty())
            {
                return query.ToList();
            }

            var count = filterIds.Count;
            var result = new List<TehPassportCacheCell>();

            for (var skip = 0; skip < count; skip += TehPassportCacheService.BulkSize)
            {
                var partIds = filterIds
                    .Skip(skip)
                    .Take(TehPassportCacheService.BulkSize);

                var partResult = query.Where(x => partIds.Contains(x.RealityObjectId))
                    .ToList();

                result.AddRange(partResult);
            }

            return result;
        }

        /// <inheritdoc />
        public List<TehPassportCacheCell> GetCacheByRealityObjectsAndColumns(string formCode, int row, ICollection<long> filterIds)
        {
            this.UpdateCacheTable();
            var query = this.TehPassportCacheCellDomain.GetAll()
                .Where(x => x.FormCode == formCode && x.RowId == row);

            if (filterIds.IsNullOrEmpty())
            {
                return query.ToList();
            }

            var count = filterIds.Count;
            var result = new List<TehPassportCacheCell>();

            for (var skip = 0; skip < count; skip += TehPassportCacheService.BulkSize)
            {
                var partIds = filterIds
                    .Skip(skip)
                    .Take(TehPassportCacheService.BulkSize);

                var partResult = query.Where(x => partIds.Contains(x.RealityObjectId))
                    .ToList();

                result.AddRange(partResult);
            }

            return result;
        }

        /// <inheritdoc />
        public Dictionary<int, List<TehPassportCacheCell>> GetRows(long realityObjectId, string formCode)
        {
            this.UpdateCacheTable();
            return this.TehPassportCacheCellDomain.GetAll()
                .Where(x => x.RealityObjectId == realityObjectId && x.FormCode == formCode)
                .AsEnumerable()
                .GroupBy(x => x.RowId)
                .ToDictionary(x => x.Key, x => x.ToList());
        }

        /// <inheritdoc />
        public Dictionary<int, List<TehPassportCacheCell>> FindRowsByColumnValue(long realityObjectId, string formCode, int column, string value)
        {
            return this.GetRows(realityObjectId, formCode)
                    ?.Where(x => x.Value.Any(v => v.ColumnId == column))
                    .ToDictionary(x => x.Key, x => x.Value.ToList())
                ?? new Dictionary<int, List<TehPassportCacheCell>>();
        }

        /// <inheritdoc />
        public bool HasValue(long realityObjectId, string formCode, int row, int column)
        {
            return this.GetValue(realityObjectId, formCode, row, column).IsNotEmpty();
        }

        /// <inheritdoc />
        public void CreateOrUpdateCacheTable()
        {
            using (var connection = this.GetNewConnection())
            {
                connection.Execute($@"
CREATE OR REPLACE FUNCTION create_view_tp_teh_passport()
RETURNS void AS
$BODY$
BEGIN
    CREATE SCHEMA IF NOT EXISTS {TehPassportCacheService.NamespaceName};
    IF (SELECT NOT EXISTS (
            SELECT 1
            FROM pg_catalog.pg_class c
                JOIN pg_namespace n ON n.oid = c.relnamespace
            WHERE c.relkind = 'S'
                AND n.nspname = '{TehPassportCacheService.NamespaceName}'
                AND c.relname = '{TehPassportCacheService.DropCacheSeqName}')) 
    THEN
        CREATE SEQUENCE {TehPassportCacheService.DropCacheSeqNameFullName};
    END IF;            
    IF (SELECT NOT EXISTS (
            SELECT 1
            FROM pg_catalog.pg_class c
                JOIN pg_namespace n ON n.oid = c.relnamespace
            WHERE c.relkind = 'm'
                AND n.nspname = '{TehPassportCacheService.NamespaceName}'
                AND c.relname = '{TehPassportCacheService.ViewName}')) THEN
        BEGIN
            DROP SEQUENCE IF EXISTS id_generator;
            CREATE SEQUENCE id_generator start with 1 increment by 1;
            CREATE MATERIALIZED VIEW {TehPassportCacheService.ViewFullName} AS
                SELECT
                nextval('id_generator') as id,
                tp.reality_obj_id,
                tpv.form_code,
                split_part(tpv.cell_code::text, ':'::text, 1)::numeric AS row_id,
                split_part(tpv.cell_code::text, ':'::text, 2)::numeric AS column_id,
                tpv.value
                FROM tp_teh_passport_value tpv
                    JOIN tp_teh_passport tp ON tp.id = tpv.teh_passport_id
            WITH DATA;

            CREATE INDEX ON {TehPassportCacheService.ViewFullName} (reality_obj_id);
            CREATE INDEX ON {TehPassportCacheService.ViewFullName} (form_code);
            CREATE INDEX ON {TehPassportCacheService.ViewFullName} (row_id);
            CREATE INDEX ON {TehPassportCacheService.ViewFullName} (column_id);
        END;
    ELSE
        REFRESH MATERIALIZED VIEW {TehPassportCacheService.ViewFullName};
    END IF;

    ANALYZE {TehPassportCacheService.ViewFullName};

END;
$BODY$
  LANGUAGE 'plpgsql';
SELECT create_view_tp_teh_passport();");
                this.dropCacheDateTime = DateTime.UtcNow.AddMinutes(this.cacheDuration);
                this.SetDropCacheTime(connection, this.dropCacheDateTime); 
            }
        }

        /// <inheritdoc />
        public void DropCacheTable()
        {
            using (var connection = this.GetNewConnection())
            {
                connection.Execute($"DROP MATERIALIZED VIEW {TehPassportCacheService.ViewFullName};"); 
            }
        }

        /// <summary>
        /// Получить время жизни кэша
        /// </summary>
        private DateTime GetDropCacheTime(IDbConnection connection)
        {
            try
            {
                var nextDropTicks = connection
                    .Query<long>($"SELECT last_value FROM {TehPassportCacheService.DropCacheSeqNameFullName};")
                    .FirstOrDefault();

                return new DateTime(nextDropTicks);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Проставить время жизни кэша
        /// </summary>
        /// <param name="connection">Соединение к БД</param>
        /// <param name="date">Время, которое нужно установить</param>
        private void SetDropCacheTime(IDbConnection connection, DateTime date)
        {
            connection.Execute($"SELECT setval('{TehPassportCacheService.DropCacheSeqNameFullName}'::regclass, {date.Ticks}, false);");
        }

        /// <summary>
        /// Получить новое соединение к БД
        /// <remarks>Открывает новую "тихую" сессию</remarks>
        /// </summary>
        private IDbConnection GetNewConnection()
        {
            return this.SessionProvider.OpenStatelessSession().Connection;
        }
    }
}