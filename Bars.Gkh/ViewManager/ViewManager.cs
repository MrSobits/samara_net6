namespace Bars.Gkh
{
    using System;
    using System.Linq;

    using Bars.B4.Application;

    using global::Bars.B4.Modules.Ecm7.Framework;

    public static class ViewManager
    {
        /// <summary>
        /// Создание 1 компонента (представления или функции)
        /// </summary>
        /// <param name="database">Database</param>
        /// <param name="moduleId">Идентификатор модуля</param>
        /// <param name="name">Наименование компонента</param>
        public static void Create(ITransformationProvider database, string moduleId, string name)
        {
            var viewCollection = ApplicationContext.Current.Container.Resolve<IViewCollection>(moduleId + "ViewCollection");

            if (viewCollection != null)
            {
                if (database.DatabaseKind == DbmsKind.Oracle)
                {
                    var oracleStr = viewCollection.GetCreateScript(DbmsKind.Oracle, name);
                    if (!string.IsNullOrEmpty(oracleStr))
                    {
                        database.ExecuteNonQuery(oracleStr);
                    }
                }
                else if (database.DatabaseKind == DbmsKind.PostgreSql)
                {
                    var postgreStr = viewCollection.GetCreateScript(DbmsKind.PostgreSql, name);
                    if (!string.IsNullOrEmpty(postgreStr))
                    {
                        database.ExecuteNonQuery(postgreStr);
                    }
                }
            }
        }

        /// <summary>
        /// Удаление 1 компонента (представления или функции)
        /// </summary>
        /// <param name="database">Database</param>
        /// <param name="moduleId">Идентификатор модуля</param>
        /// <param name="name">Наименование компонента</param>
        public static void Drop(ITransformationProvider database, string moduleId, string name)
        {
            var viewCollection = ApplicationContext.Current.Container.Resolve<IViewCollection>(moduleId + "ViewCollection");

            if (viewCollection != null)
            {
                if (database.DatabaseKind == DbmsKind.Oracle)
                {
                    var oracleStr = viewCollection.GetDeleteScript(DbmsKind.Oracle, name);
                    if (!string.IsNullOrEmpty(oracleStr))
                    {
                        database.ExecuteNonQuery(oracleStr);
                    }
                }
                else if (database.DatabaseKind == DbmsKind.PostgreSql)
                {
                    var postgreStr = viewCollection.GetDeleteScript(DbmsKind.PostgreSql, name);
                    if (!string.IsNullOrEmpty(postgreStr))
                    {
                        database.ExecuteNonQuery(postgreStr);
                    }
                }
            }
        }

        /// <summary>
        /// Создание всех компонентов модуля (зависимости не учитываются)
        /// </summary>
        /// <param name="database">Database</param>
        /// <param name="moduleId">Идентификатор модуля</param>
        public static void Create(ITransformationProvider database, string moduleId)
        {
            var viewCollection = ApplicationContext.Current.Container.Resolve<IViewCollection>(moduleId + "ViewCollection");

            if (viewCollection != null)
            {
                if (database.DatabaseKind == DbmsKind.Oracle)
                {
                    var oracleQueries = viewCollection.GetCreateAll(DbmsKind.Oracle);
                    foreach (var oracleQuery in oracleQueries)
                    {
                        database.ExecuteNonQuery(oracleQuery);
                    }
                }
                else if (database.DatabaseKind == DbmsKind.PostgreSql)
                {
                    var postgreQueries = viewCollection.GetCreateAll(DbmsKind.PostgreSql);
                    foreach (var postgreQuery in postgreQueries)
                    {
                        database.ExecuteNonQuery(postgreQuery);
                    }
                }
            }
        }

        /// <summary>
        /// Удаление всех компонентов модуля (зависимости не учитываются)
        /// </summary>
        /// <param name="database">Database</param>
        /// <param name="moduleId">Идентификатор модуля</param>
        public static void Drop(ITransformationProvider database, string moduleId)
        {
            var viewCollection = ApplicationContext.Current.Container.Resolve<IViewCollection>(moduleId + "ViewCollection");

            if (viewCollection != null)
            {
                if (database.DatabaseKind == DbmsKind.Oracle)
                {
                    var oracleQueries = viewCollection.GetDropAll(DbmsKind.Oracle);
                    foreach (var oracleQuerie in oracleQueries)
                    {
                        try
                        {
                            database.ExecuteNonQuery(oracleQuerie);
                        }
                        catch (Exception exp)
                        {
                            if (!exp.Message.Contains("ORA-00942")
                                && !exp.Message.Contains("ORA - 00942")
                                && !exp.Message.Contains("ORA-04043")
                                && !exp.Message.Contains("ORA - 04043"))
                            {
                                throw;
                            }
                        }
                    }
                }
                else if (database.DatabaseKind == DbmsKind.PostgreSql)
                {
                    var postgreQueries = viewCollection.GetDropAll(DbmsKind.PostgreSql);
                    foreach (var postgreQuerie in postgreQueries)
                    {
                        database.ExecuteNonQuery(postgreQuerie);
                    }
                }
            }
        }

        /// <summary>
        /// Создание всех компонентов, во всех модулях (с учетом зависимостей)
        /// </summary>
        /// <param name="database">Database</param>
        public static void CreateAll(ITransformationProvider database)
        {
            var viewCollections = ApplicationContext.Current.Container.ResolveAll<IViewCollection>().OrderBy(x => x.Number);

            foreach (var item in viewCollections)
            {
                if (database.DatabaseKind == DbmsKind.Oracle)
                {
                    var oracleQueries = item.GetCreateAll(DbmsKind.Oracle);

                    foreach (var oracleQuerie in oracleQueries)
                    {
                        database.ExecuteNonQuery(oracleQuerie);
                    }
                }
                else if (database.DatabaseKind == DbmsKind.PostgreSql)
                {
                    var postgreQueries = item.GetCreateAll(DbmsKind.PostgreSql);
                    foreach (var postgreQuerie in postgreQueries)
                    {
                        database.ExecuteNonQuery(postgreQuerie);
                    }
                }
            }
        }

        /// <summary>
        /// Удаление всех компонентов, во всех модулях (с учетом зависимостей)
        /// </summary>
        /// <param name="database">Database</param>
        public static void DropAll(ITransformationProvider database)
        {
            var viewCollections = ApplicationContext.Current.Container.ResolveAll<IViewCollection>().OrderByDescending(x => x.Number);

            foreach (var item in viewCollections)
            {
                if (database.DatabaseKind == DbmsKind.Oracle)
                {
                    var oracleQueries = item.GetDropAll(DbmsKind.Oracle);

                    foreach (var oracleQuerie in oracleQueries)
                    {
                        database.ExecuteNonQuery(oracleQuerie);
                    }
                }
                else if (database.DatabaseKind == DbmsKind.PostgreSql)
                {
                    var postgreQueries = item.GetDropAll(DbmsKind.PostgreSql);
                    foreach (var postgreQuerie in postgreQueries)
                    {
                        database.ExecuteNonQuery(postgreQuerie);
                    }
                }
            }
        }
    }
}