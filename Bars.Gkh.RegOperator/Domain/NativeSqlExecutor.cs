namespace Bars.Gkh.RegOperator.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using B4.DataAccess;
    using B4.Utils;
    using B4.IoC;
    using Castle.Windsor;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using NHibernate;
    using NHibernate.Persister.Entity;

    public class NativeSqlExecutor<T> where T : IEntity
    {
        private readonly IWindsorContainer _container;
        private readonly JsonSerializerSettings _settings;

        public NativeSqlExecutor(IWindsorContainer container)
        {
            _container = container;

            _settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.None,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                TypeNameHandling = TypeNameHandling.Auto
            };
            _settings.Converters.Clear();

            _settings.Converters.Add(new StringEnumConverter());
        }
        
        public void Insert(int batchSize, IEnumerable<T> items)
        {
            var provider = _container.Resolve<ISessionProvider>();

            var session = provider.GetCurrentSession();

            using (_container.Using(provider))
            {
                if (!items.Any())
                {
                    return;
                }

                var savedCount = 0;

                var connection = session.Connection;

                if (!connection.GetType().Name.Contains("NpgsqlConnection"))
                {
                    return;
                }

                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                var query = new StringBuilder();

                while (savedCount <= items.Count())
                {
                    query.Clear();
                    var toSave = items.Skip(savedCount).Take(batchSize);

                    savedCount += batchSize + 1;

                    query = CreateBulkInsertQuery(toSave, session);

                    ExecuteCommand(query, connection);
                }
            }
        }

        public void Delete(IEnumerable<long> ids)
        {
            if (!ids.Any())
            {
                return;
            }

            var provider = _container.Resolve<ISessionProvider>();

            var session = provider.GetCurrentSession();

            using (_container.Using(provider))
            {
                var sb = new StringBuilder();
                ids.ForEach(x => sb.AppendFormat("{0},", x));

                var query = string.Format("delete from {0} where Id in ({1})", GetTableName(typeof (T), session),
                    sb.ToString().TrimEnd(','));

                session.CreateSQLQuery(query).ExecuteUpdate();
            }
        }

        private object GetTableName(Type type, ISession session)
        {
            var meta = GetMetaData(type, session);

            return meta.TableName;
        }

        private AbstractEntityPersister GetMetaData(Type type, ISession session)
        {
            return session.SessionFactory.GetClassMetadata(type) as AbstractEntityPersister;
        }

        [Obsolete]
        private StringBuilder CreateBulkInsertQuery(IEnumerable<T> items, ISession session)
        {
            string query = @"insert into {0} ({1}) values({2});" + Environment.NewLine;

            return CreateBulkInsertUpdateQuery(items, session, query);
        }

        [Obsolete]
        private StringBuilder CreateBulkInsertUpdateQuery(IEnumerable<T> items, ISession session, string query)
        {
            var meta = GetMetaData(typeof(T), session);

            var sb = new StringBuilder();

            var props = typeof(T).GetProperties();

            var colValues = new Dictionary<string, string>();

            foreach (var item in items)
            {
                foreach (var prop in props)
                {
                    var columnName = string.Empty;
                    try
                    {
                        columnName = meta.GetPropertyColumnNames(prop.Name).First();
                    }
                    catch (HibernateException)
                    {
                        continue;
                    }

                    if (columnName.ToUpperInvariant() == meta.IdentifierPropertyName.ToUpperInvariant())
                    {
                        continue;
                    }

                    var value = GetValue(prop, item);

                    colValues.Add(columnName, value);
                }

                sb.AppendFormat(query, meta.TableName, string.Join(",", colValues.Keys),
                    string.Join(",", colValues.Values), item.Id);

                colValues = new Dictionary<string, string>();
            }

            return sb;
        }

        [Obsolete]
        private StringBuilder CreateBulkUpdateQuery(IEnumerable<T> items, ISession session)
        {
            var query = @"update {0} set ({1}) = ({2}) where id = {3};" + Environment.NewLine;

            return CreateBulkInsertUpdateQuery(items, session, query);
        }

        private IMultiQuery CreateInsertQuery(IEnumerable<T> items)
        {
            var session = _container.Resolve<ISessionProvider>().GetCurrentSession();

            var properties = GetProperties<T>(session);

            var meta = GetMetaData(typeof (T), session);

            var query = string.Format("insert into {0} ({1}) values ({2})", meta.TableName,
                string.Join(", ", properties.Select(x => GetColumnName<T>(x, session))), string.Join(", ", properties.Select(x => ":" + x.Name)));

            return CreateMultiQuery(query, items);
        }

        private string GetColumnName<T>(PropertyInfo x, ISession session)
        {
            if (_propertyColumnName.ContainsKey(x.Name))
            {
                return _propertyColumnName[x.Name];
            }

            var meta = GetMetaData(typeof (T), session);

            var column = meta.GetPropertyColumnNames(x.Name).First();

            _propertyColumnName.Add(x.Name, column);

            return column;
        }

        private IMultiQuery CreateMultiQuery(string query, IEnumerable<T> items)
        {
            var session = _container.Resolve<ISessionProvider>().GetCurrentSession();

            var mq = session.CreateMultiQuery();

            foreach (var item in items)
            {
                var hql = session.CreateSQLQuery(query);

                AppendParameter(hql, item, session);

                mq.Add(hql);
            }

            return mq;
        }

        private void AppendParameter(IQuery hql, T item, ISession session)
        {
            var props = GetProperties<T>(session);

            foreach (var prop in props)
            {
                var value = prop.GetValue(item, new object[0]);

                var propType = Nullable.GetUnderlyingType(prop.PropertyType);

                if (propType == null)
                {
                    propType = prop.PropertyType;
                }

                if (propType == typeof(DateTime))
                {
                    hql.SetDateTime(prop.Name, value.ToDateTime());
                }
                else if (propType == typeof(Decimal))
                {
                    hql.SetDecimal(prop.Name, value.ToDecimal());
                }
                else if (propType == typeof (int))
                {
                    hql.SetInt32(prop.Name, value.ToInt());
                }
                else if (propType == typeof (long))
                {
                    hql.SetInt64(prop.Name, value.ToLong());
                }
                else if (propType == typeof(string))
                {
                    hql.SetString(prop.Name, value.ToStr());
                }
                else if (typeof(IEntity).IsAssignableFrom(propType))
                {
                    var ientity = value as IEntity;
                    hql.SetEntity(prop.Name, ientity);
                }
                else if (propType.IsEnum)
                {
                    hql.SetInt32(prop.Name, value.ToInt());
                }
                else
                {
                    value = SerializeToJson(value);
                    hql.SetString(prop.Name, SerializeToJson(value));
                }
            }
        }

        private static void ExecuteCommand(StringBuilder query, IDbConnection con)
        {
            if (query.Length == 0)
            {
                return;
            }

            using (var com = con.CreateCommand())
            {
                com.CommandText = query.ToString();
                using (var tr = con.BeginTransaction())
                {
                    try
                    {
                        com.ExecuteNonQuery();

                        tr.Commit();

                        Trace.Write(query);
                    }
                    catch
                    {
                        tr.Rollback();
                        throw;
                    }
                }
            }
        }

        private string GetValue(PropertyInfo prop, T item)
        {
            var value = prop.GetValue(item, new object[0]);

            var propType = Nullable.GetUnderlyingType(prop.PropertyType);
            var isNullable = true;

            if (propType == null)
            {
                isNullable = false;
                propType = prop.PropertyType;
            }

            if (propType == typeof(DateTime))
            {
                return string.Format("to_date('{0}', '{1}')", value.ToDateTime(), "DD.MM.YYYY HH24.MI.SS");
            }
            else if (propType == typeof(Decimal))
            {
                return value.ToStr().Replace(",", ".");
            }
            else if (propType == typeof(string))
            {
                if (value != null)
                {
                    return string.Format("'{0}'", value.ToStr());
                }

                return "null";
            }
            else if (typeof(IEntity).IsAssignableFrom(propType))
            {
                var ientity = value as IEntity;
                if (ientity != null && ientity.Id.ToInt() > 0)
                {
                    return ientity.Id.ToStr();
                }

                return "null";
            }
            else if (propType.IsEnum)
            {
                return value.ToInt().ToStr();
            }
            else
            {
                value = SerializeToJson(value);
            }

            return value != null ? value.ToStr() : isNullable ? "null" : "''";
        }

        private string SerializeToJson(object value)
        {
            if (value == null)
            {
                return null;
            }

            return string.Format("'{0}'", JsonConvert.SerializeObject(value, _settings));
        }

        private PropertyInfo[] GetProperties<T>(ISession session)
        {
            var type = typeof (T);
            if (_propertyDict.ContainsKey(type))
            {
                return _propertyDict[type];
            }

            PropertyInfo[] props = FilterProperties(type, type.GetProperties(), session);

            _propertyDict.Add(type, props);

            return props;
        }

        private PropertyInfo[] FilterProperties(Type type, PropertyInfo[] props, ISession session)
        {
            var meta = GetMetaData(type, session);

            var list = new List<PropertyInfo>();

            foreach (var prop in props)
            {
                try
                {
                    meta.GetPropertyColumnNames(prop.Name).First();
                }
                catch (HibernateException)
                {
                    continue;
                }

                if (prop.Name == meta.IdentifierPropertyName)
                {
                    continue;
                }

                list.Add(prop);
            }

            return list.ToArray();
        }

        private Dictionary<Type, PropertyInfo[]> _propertyDict = new Dictionary<Type, PropertyInfo[]>();
        private Dictionary<string, string> _propertyColumnName = new Dictionary<string, string>(); 
    }
}