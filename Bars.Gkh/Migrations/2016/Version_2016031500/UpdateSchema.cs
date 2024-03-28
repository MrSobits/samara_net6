#pragma warning disable 1591
namespace Bars.Gkh.Migrations._2016.Version_2016031500
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Bars.B4.Application;
    using Bars.B4.IoC;
    using B4.Metadata.Types;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.QueryDesigner.Entities;
    using Bars.B4.Modules.QueryDesigner.Interfaces;
    using Bars.B4.Modules.QueryDesigner.Manifests;
    using Bars.B4.Modules.QueryDesigner.Queries;
    using Bars.B4.Utils;

    using Newtonsoft.Json;

    /// <summary>
    /// Миграция конвертации запросов в дизайнера
    /// </summary>
    [Migration("2016031500")]
    [MigrationDependsOn(typeof(Bars.B4.Modules.QueryDesigner.Migrations.Ver_2016_01_29_11_02_59))]
    [MigrationReverseDependency(typeof(Bars.B4.Modules.QueryDesigner.Migrations.Ver_2016_02_04_13_45_09))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            var idToOldQueryMap = new Dictionary<long, OldQuery>();

            // 1. получаем запросы
            var queryQuery = @"SELECT id, name, commentar, json FROM b4_indicators_query;";
            using (var reader = this.Database.ExecuteQuery(queryQuery))
            {
                while (reader.Read())
                {
                    var json = reader["json"].ToStr();

                    if (string.IsNullOrEmpty(json))
                    {
                        continue;
                    }

                    var queryFromJson = JsonConvert.DeserializeObject<JsonQuery>(json);

                    if (queryFromJson.SqlMode && !string.IsNullOrEmpty(queryFromJson.SqlText))
                    {
                        var oldQuery = new OldQuery
                        {
                            Id = reader["id"].ToLong(),
                            Name = reader["name"].ToStr(),
                            Description = reader["commentar"].ToStr(),
                            SqlText = queryFromJson.SqlText,
                            SqlFields = queryFromJson.SqlFields
                        };

                        idToOldQueryMap[oldQuery.Id] = oldQuery;
                    }
                }
            }

            // 2. добавляем параметры запросов
            var parameterQuery = @"SELECT id, code, name, description, param_type, query_id, allow_blank, default_value FROM b4_query_parameter";
            using (var reader = this.Database.ExecuteQuery(parameterQuery))
            {
                while (reader.Read())
                {
                    var oldQueryParameter = new OldQueryParameter
                    {
                        Id = reader["id"].ToLong(),
                        QueryId = reader["query_id"].ToLong(),
                        Code = reader["code"].ToStr(),
                        Name = reader["name"].ToStr(),
                        Description = reader["description"].ToStr(),
                        ParameterType = reader["param_type"].As<IndicatorType>(),
                        AllowBlank = reader["allow_blank"].ToBool(),
                        DefaultValue = reader["default_value"].ToStr()
                    };

                    OldQuery oldQuery;
                    if (oldQueryParameter.QueryId > 0 && idToOldQueryMap.TryGetValue(oldQueryParameter.QueryId, out oldQuery))
                    {
                        oldQuery.Parameters.Add(oldQueryParameter);
                    }
                }
            }

            if (idToOldQueryMap.Count > 0)
            {
                // 3. формируем запросы нового типа
                var queryService = ApplicationContext.Current.Container.Resolve<IQueryService>();
                using (ApplicationContext.Current.Container.Using(queryService))
                {
                    foreach (var oldQuery in idToOldQueryMap.Values)
                    {
                        var sqlQueryStructure = new SqlQueryStructure
                        {
                            SqlText = this.ConverSqtTextParameters(oldQuery.SqlText, oldQuery.Parameters)
                        };

                        foreach (var oldQueryParameter in oldQuery.Parameters)
                        {
                            var parameter = new QueryParameter
                            {
                                Id = oldQueryParameter.Code,
                                DisplayName = oldQueryParameter.Name,
                                ParameterType = this.GetParameterType(oldQueryParameter)
                            };
                            sqlQueryStructure.Parameters.Add(parameter);
                        }

                        foreach (var sqlFieldInfo in oldQuery.SqlFields)
                        {
                            if (sqlFieldInfo.Name.Contains(" "))
                            {
                                var newFieldName = sqlFieldInfo.Name.Replace(" ", "_");

                                sqlQueryStructure.SqlText = sqlQueryStructure.SqlText.Replace(sqlFieldInfo.Name, newFieldName);

                                sqlFieldInfo.Name = newFieldName;
                            }

                            var sqlQueryResultColumn = new SqlQueryResultColumn
                            {
                                Id = sqlFieldInfo.Name,
                                Alias = sqlFieldInfo.Name,
                                DisplayName = sqlFieldInfo.Display,
                                Type = this.GetParameterType(sqlFieldInfo.Type, !sqlFieldInfo.IsKey)
                            };
                            sqlQueryStructure.ResultColumns.Add(sqlQueryResultColumn);
                        }

                        oldQuery.SqlQuery = queryService.CreateSqlQuery(oldQuery.Name, "OldQuery_" + oldQuery.Id.ToString(), sqlQueryStructure);
                    }
                }

                // 4. перепроставляем источники данных у отчётов и параметров
                var updateReportQuery = @"
                    UPDATE b4_report_data_source
                    SET data_query_id = '{0}{1}', query_provider_name = 'Bars.B4.Modules.QueryDesigner.Services.QueryDesignerDataQueryProvider'
                    WHERE data_query_id = '{2}'";

                var updateReportParamsQuery = @"
                    UPDATE b4_report_param
                    SET data_query_id = '{0}{1}', query_provider_name = 'Bars.B4.Modules.QueryDesigner.Services.QueryDesignerDataQueryProvider'
                    WHERE type = 5
                      AND query_provider_name = 'QueryByIndicatorsDataQueryProvider'
                      AND data_query_id = '{2}'";

                foreach (var oldQuery in idToOldQueryMap.Values)
                {
                    this.Database.ExecuteNonQuery(string.Format(updateReportQuery, QueriesDataSourceManifest.DataSourceIdPrefix, oldQuery.SqlQuery.Id, oldQuery.Id));

                    this.Database.ExecuteNonQuery(string.Format(updateReportParamsQuery, QueriesDataSourceManifest.DataSourceIdPrefix, oldQuery.SqlQuery.Id, oldQuery.Id));
                }
            }
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
        }

        private string ConverSqtTextParameters(string sqlText, List<OldQueryParameter> parameters)
        {
            if (parameters.Count > 0)
            {
                var stringBuilder = new StringBuilder(sqlText);

                foreach (var parameter in parameters)
                {
                    stringBuilder.Replace(":" + parameter.Code, "@p:" + parameter.Code);
                }

                return stringBuilder.ToString();
            }
            else
            {
                return sqlText;
            }
        }

        private ValueTypeRef GetParameterType(OldQueryParameter oldQueryParameter)
        {
            return this.GetParameterType(oldQueryParameter.ParameterType, oldQueryParameter.AllowBlank);
        }

        private ValueTypeRef GetParameterType(IndicatorType parameterType, bool isNullable)
        {
            ValueTypeRef result = null;
            switch (parameterType)
            {
                case IndicatorType.String:
                    result = PrimitiveTypeRef.GetStringType(isNullable);
                    break;
                case IndicatorType.Int:
                    result = PrimitiveTypeRef.GetIntType(isNullable);
                    break;
                case IndicatorType.Double:
                    result = PrimitiveTypeRef.GetDecimalType(isNullable);
                    break;
                case IndicatorType.Date:
                    result = PrimitiveTypeRef.GetDateTimeType(isNullable);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return result;
        }

        private class OldQuery
        {
            public OldQuery()
            {
                this.Parameters = new List<OldQueryParameter>();
                this.SqlFields = new List<SqlFieldInfo>();
            }

            public SqlQuery SqlQuery { get; set; }

            public long Id { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }

            public string SqlText { get; set; }

            public List<SqlFieldInfo> SqlFields { get; set; }

            public List<OldQueryParameter> Parameters { get; private set; }
        }

        public class JsonQuery
        {
            public JsonQuery()
            {
                this.SqlFields = new List<SqlFieldInfo>();
            }

            public bool SqlMode { get; set; }

            public string SqlText { get; set; }

            public List<SqlFieldInfo> SqlFields { get; set; }
        }

        public class SqlFieldInfo
        {
            public string Name { get; set; }

            public string Display { get; set; }

            public IndicatorType Type { get; set; }

            public bool IsKey { get; set; }
        }

        public enum IndicatorType
        {
            String = 0,
            Int = 1,
            Double = 2,
            Date = 3
        }

        private class OldQueryParameter
        {
            public long Id { get; set; }

            public long QueryId { get; set; }

            public string Code { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }

            public IndicatorType ParameterType { get; set; }

            public bool AllowBlank { get; set; }

            public string DefaultValue { get; set; }
        }
    }
}