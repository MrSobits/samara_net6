namespace Bars.B4.Modules.Analytics.Reports.Web.Controllers
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Analytics.Web.Data;
    using Bars.B4.Modules.Security;
    using B4.Utils;
    using DataAccess;
    using Entities;
    using Npgsql;
    using System.Linq.Expressions;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Ecm7.Exceptions;

    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Управление отчётами
    /// </summary>
    public class StoredReportController : Alt.DataController<StoredReport>
    {
        /// <summary>
        /// Вернуть поддерево отчёта
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult GetTree(BaseParams baseParams)
        {
            var treeBuilder = new DataSourceTreeBuilder();
            var reportDomain = this.Container.Resolve<IDomainService<StoredReport>>();
            var reportId = baseParams.Params.GetAs<long>("reportId", ignoreCase: true);
            var report = reportDomain.FirstOrDefault(x => x.Id == reportId);
            var tree = treeBuilder.BuildTree(report.DataSources, true);
            this.Container.Release(reportDomain);
            return this.JsSuccess(tree.children);
        }

        /// <summary>
        /// Вернуть шаблон отчёта
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult GetTemplate(BaseParams baseParams)
        {
            var reportDomain = this.Container.Resolve<IDomainService<StoredReport>>();
            var reportId = baseParams.Params.GetAs<long>("reportId", ignoreCase: true);
            var report = reportDomain.FirstOrDefault(x => x.Id == reportId);
            var reportName = report != null ? report.Name : string.Empty;
            var template = report != null ? report.GetTemplate() : new MemoryStream(Properties.Resources.EmptyReport);
            this.Container.Release(reportDomain);
            template.Seek(0, SeekOrigin.Begin);
            var fileNameEncode = System.Web.HttpUtility.UrlEncode(string.Format("{0}_{1}.mrt", "Пример_шаблона", reportName));

            return this.File(template, System.Net.Mime.MediaTypeNames.Application.Octet, fileNameEncode);
        }

        /// <summary>
        /// Вернуть роли
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult GetRoles(BaseParams baseParams)
        {
            var roleDomain = this.Container.Resolve<IDomainService<Role>>();

            try
            {
                var loadParams = baseParams.GetLoadParam();
                var data = roleDomain.GetAll()
                    .OrderIf(loadParams.Order.Length == 0, true, x => x.Name)
                    .Filter(loadParams, this.Container);

                var totalCount = data.Count();

                return new JsonListResult(data.Order(loadParams).ToList(), totalCount);
            }
            finally
            {
                this.Container.Release(roleDomain);
            }
        }

        /// <summary>
        /// Вернуть разрешения отчёта
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult GetPermissions(BaseParams baseParams)
        {
            var rolePermDomain = this.Container.ResolveDomain<RolePermission>();
            var storedReportDomain = this.Container.ResolveDomain<StoredReport>();
            var reportId = baseParams.Params.GetAs<string>("reportId");

            try
            {
                var storedReport = storedReportDomain.Get(reportId.ToLong());
                if (storedReport == null)
                {
                    return this.JsFailure("Не удалось получить отчет");
                }

                var rolePerms = rolePermDomain.GetAll()
                    .Where(x => x.PermissionId == reportId)
                    .Select(x => x.Role.Id)
                    .ToArray();

                return this.JsSuccess(new RolePermissionsObject { ForAll = storedReport.ForAll, Permissions = rolePerms });
            }
            finally
            {
                this.Container.Release(rolePermDomain);
                this.Container.Release(storedReportDomain);
            }
        }

        /// <summary>
        /// Сохранить разрешения отчёта
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult SavePermissions(BaseParams baseParams)
        {
            var rolePermDomain = this.Container.Resolve<IDomainService<RolePermission>>();
            var roleDomain = this.Container.ResolveDomain<Role>();
            var storedReportDomain = this.Container.ResolveDomain<StoredReport>();

            var reportId = baseParams.Params.GetAs<string>("reportId");
            var roleIds = baseParams.Params.GetAs<long[]>("roleIds");

            try
            {
                var storedReport = storedReportDomain.Get(reportId.ToLong());
                if (storedReport == null)
                {
                    return this.JsFailure("Не удалось получить отчет");
                }

                rolePermDomain.GetAll()
                    .Where(x => x.PermissionId == reportId)
                    .Select(x => x.Id)
                    .ToArray()
                    .ForEach(x => rolePermDomain.Delete(x));

                foreach (var roleId in roleIds)
                {
                    var permission = new RolePermission { PermissionId = reportId, Role = new Role { Id = roleId } };

                    rolePermDomain.Save(permission);
                }

                // если не выбрана ни одна роль, значит для всех
                storedReport.ForAll = roleIds.Length == 0;
                storedReportDomain.Save(storedReport);

                return this.JsSuccess();
            }
            catch (Exception e)
            {
                return this.JsFailure(e.Message);
            }
            finally
            {
                this.Container.Release(roleDomain);
                this.Container.Release(rolePermDomain);
                this.Container.Release(storedReportDomain);
            }
        }

        /// <summary>
        /// Проверить Sql-запрос
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult CheckSqlQueryParameter(BaseParams baseParams)
        {
            var queryString = baseParams.Params.GetAs<string>("sqlQuery");

            if (queryString.Length > 2000)
            {
                return this.JsFailure("Длина запроса должна быть не более 2000 символов");
            }

            var pattern = new Regex("delete|insert|drop|create|alter|truncate", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            if (pattern.IsMatch(queryString.ToLower()))
            {
                return this.JsFailure("Обнаружена запрещенная команда");
            }

            return this.RunSqlQueryParameter(baseParams);
        }

        /// <summary>
        /// Вызвать Sql-запрос
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult RunSqlQueryParameter(BaseParams baseParams)
        {
            try
            {
                var queryString = string.Empty;
                var id = baseParams.Params.GetAs<long>("Id");

                if (id > 0)
                {
                    var reportParamDomain = this.Container.ResolveDomain<ReportParamGkh>();
                    try
                    {
                        var reportParam = reportParamDomain.Get(id);
                        queryString = reportParam != null ? reportParam.SqlQuery : string.Empty;
                    }
                    finally
                    {
                        this.Container.Release(reportParamDomain);
                    }
                }
                if (queryString.IsEmpty())
                {
                    queryString = baseParams.Params.GetAs<string>("sqlQuery");
                }
                var command = this.Container.Resolve<ISessionProvider>().GetCurrentSession().Connection.CreateCommand();
                command.CommandText = queryString;
                command.CommandTimeout = 180;

                using (var reader = command.ExecuteReader())
                {
                    var dt = new DataTable();
                    dt.Load(reader);

                    var dataFromTable = dt.Select();

                    if (!dataFromTable.Any())
                    {
                        return this.JsFailure("Запрос не возвращает данных");
                    }

                    if (dataFromTable.Any(x => !(x.ItemArray[0] is Int32 || x.ItemArray[0] is Int64)))
                    {
                        return this.JsFailure("Первый параметр должен быть типа Int64");
                    }

                    if (dataFromTable.Select(x => x.ItemArray[0]).Distinct().Count() != dataFromTable.Count())
                    {
                        return this.JsFailure("Первый параметр должен быть уникальным");
                    }

                    if (dataFromTable.All(x => x.ItemArray[1] is DBNull))
                    {
                        return this.JsFailure("Значения второго столбца не могут быть пустыми");
                    }

                    if (dataFromTable.Any(x => !(x.ItemArray[1] is string)))
                    {
                        return this.JsFailure("Второй параметр должен быть типа string");
                    }

                    var data = dataFromTable.Select(x =>
                        new QueryObject
                        {
                            Id = x.ItemArray[0].ToLong(),
                            Name = x.ItemArray[1].ToString()
                        })
                        .ToList();

                    var loadParams = this.GetLoadParam(baseParams);

                    var totalCount = data.Count;

                    var filtredData = data.AsQueryable()
                        .Filter(loadParams, this.Container);

                    data = this.GkhOrder(filtredData, loadParams, false)
                        .Paging(loadParams)
                        .ToList();

                    return new JsonNetResult(new ListDataResult(data, totalCount));
                }
            }

            catch (SqlException)
            {
                return this.JsFailure("Превышено время выполнения запроса");
            }
            catch (NpgsqlException)
            {
                return this.JsFailure("Ошибка выполнения, проверьте введенный запрос");
            }
            catch (Exception)
            {
                return this.JsFailure("Обнаружена непредвиденная ошибка");
            }
        }

        /// <summary>
        /// Метод сортировки с возможностью явного отключения сортировки по первичному ключу
        /// </summary>
        /// <param name="data">
        /// Запрос
        /// </param>
        /// <param name="loadParam">
        /// Параметры загрузки списка
        /// </param>
        /// <param name="addOrderById">
        /// Добавить в конце сортировку по Id.
        /// <para>В случае, если сортируем по неуникальному полю, порядок данных может поменяться</para>
        /// </param>
        /// <typeparam name="T">
        /// Тип данных запроса
        /// </typeparam>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        private IQueryable<T> GkhOrder<T>(IQueryable<T> data, LoadParam loadParam, bool addOrderById)
        {
            var result = data;

            var first = true;
            foreach (var field in loadParam.Order)
            {
                var x = Expression.Parameter(typeof(T), "x");

                var expression = Expression.Lambda<Func<T, object>>(Expression.Convert(ReflectionUtil.GetPropertyForExpression(x, field.Name), typeof(object)), x);

                result = first
                    ? result.OrderIf(true, field.Asc, expression)
                    : result.OrderThenIf(true, field.Asc, expression);

                first = false;
            }

            // Добавляем сортировку по ID для стабильной сортировки
            if (addOrderById && typeof(T).GetProperty("Id") != null)
            {
                var x = Expression.Parameter(typeof(T), "x");
                var expression = Expression.Lambda<Func<T, object>>(Expression.Convert(ReflectionUtil.GetPropertyForExpression(x, "Id"), typeof(object)), x);

                var sortAsc = true;
                if (loadParam.Order.Length > 0)
                {
                    sortAsc = loadParam.Order[loadParam.Order.Length - 1].Asc;
                }

                result = first
                    ? result.OrderIf(true, sortAsc, expression)
                    : result.OrderThenIf(true, sortAsc, expression);
            }

            return result;
        }

        /// <summary>
        /// Запрос
        /// </summary>
        public class QueryObject
        {
            /// <summary>
            /// Идентификатор
            /// </summary>
            public long Id { get; set; }
            /// <summary>
            /// Наименование
            /// </summary>
            public string Name { get; set; }
        }

        /// <summary>
        /// Разрешения 
        /// </summary>
        public class RolePermissionsObject
        {
            /// <summary>
            /// Для всех
            /// </summary>
            public bool ForAll { get; set; }

            /// <summary>
            /// Разрешения
            /// </summary>
            public long[] Permissions { get; set; }
        }
    }
}