namespace Bars.Gkh.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.Ecm7.Providers;

    using NHibernate.Util;

    /// <summary>
    /// Методы для работы с мигратором
    /// </summary>
    public static class MigratorUtils
    {
        private static readonly IDictionary<Type, DbType> mappingType = new Dictionary<Type, DbType>
        {
            { typeof(byte), DbType.Byte },
            { typeof(short), DbType.Int16 },
            { typeof(int), DbType.Int32 },
            { typeof(long), DbType.Int64 },
            { typeof(char), DbType.SByte },
            { typeof(ushort), DbType.UInt16 },
            { typeof(uint), DbType.UInt32 },
            { typeof(ulong), DbType.UInt64 },
            { typeof(string), DbType.String },
            { typeof(decimal), DbType.Decimal },
            { typeof(DateTime), DbType.DateTime },
            { typeof(float), DbType.Double },
            { typeof(double), DbType.Double },
            { typeof(Guid), DbType.Guid },
            { typeof(bool), DbType.Boolean }
        };

        /// <summary>
        /// Метод конвертирует тип рантайма в тип мигратора базы данных
        /// </summary>
        public static DbType ConvertToDbType(Type type)
        {
            if (type.IsGenericType)
            {
                type = type.GetGenericArguments()[0];
            }

            if (type.IsEnum)
            {
                type = type.GetEnumUnderlyingType();
            }

            return MigratorUtils.mappingType[type];
        }

        /// <summary>
        /// Метод возвращает флаг, может ли указанный тип принимать значение null
        /// </summary>
        public static bool CanBeNull(Type type)
        {
            return type.IsNullableOrReference();
        }

        /// <summary>
        /// Метод возвращает текущую реализацию поставщика модификации базы данных
        /// </summary>
        /// <param name="connection">Соединение с бд</param>
        public static ITransformationProvider GetTransformProvider(IDbConnection connection)
        {
            var container = ApplicationContext.Current.Container;
            var configProvider = container.Resolve<IDbConfigProvider>();

            var providerType = MigratorUtils.GetDialectType(configProvider);
            return ProviderFactory.Create(providerType, connection);
        }

        private static string GetDialectType(IDbConfigProvider configProvider)
        {
            var dbDialect = configProvider.DbDialect;
            string providerType;

            switch (dbDialect)
            {
                case DbDialect.Oracle10:
                    providerType = "Bars.B4.Modules.Ecm7.Providers.Oracle.System.OracleTransformationProvider, Bars.B4.Modules.Ecm7.Providers.Oracle.System";
                    break;
                case DbDialect.Oracle10Native:
                    throw new NotImplementedException();
                case DbDialect.OracleManaged:
                    providerType = "Bars.B4.Modules.Ecm7.Providers.Oracle.ManagedDataAccess.OracleTransformationProvider, Bars.B4.Modules.Ecm7.Providers.Oracle.ManagedDataAccess";
                    break;
                case DbDialect.PostgreSql:
                    providerType = "Bars.B4.Modules.Ecm7.Providers.PostgreSQL.PostgreSQLTransformationProvider, Bars.B4.Modules.Ecm7.Providers.PostgreSQL";
                    break;
                case DbDialect.MsSqlServer:
                    providerType = "Bars.B4.Modules.Ecm7.Providers.SqlServer.SqlServerTransformationProvider, Bars.B4.Modules.Ecm7.Providers.SqlServer";
                    break;
                case DbDialect.Sqlite:
                    providerType = "Bars.B4.Modules.Ecm7.Providers.SQLite.SQLiteTransformationProvider, Bars.B4.Modules.Ecm7.Providers.SQLite";
                    break;
                case DbDialect.MySql:
                    providerType = "Bars.B4.Modules.Ecm7.Providers.MySql.MySqlTransformationProvider, Bars.B4.Modules.Ecm7.Providers.MySql";
                    break;
                case DbDialect.Firebird:
                    providerType = "Bars.B4.Modules.Ecm7.Providers.Firebird.FirebirdTransformationProvider, Bars.B4.Modules.Ecm7.Providers.Firebird";
                    break;
                default:
                    throw new InvalidOperationException($"Некорректное значение перечисления DbDialect: {dbDialect}");
            }
            return providerType;
        }
    }
}