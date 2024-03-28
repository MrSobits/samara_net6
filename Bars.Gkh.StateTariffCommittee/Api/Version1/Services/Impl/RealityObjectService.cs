namespace Bars.Gkh.StateTariffCommittee.Api.Version1.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.BaseApiIntegration.Controllers;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.Regions.Tatarstan.Entities.Administration;
    using Bars.Gkh.Regions.Tatarstan.Enums;
    using Bars.Gkh.SqlExecutor;
    using Bars.Gkh.StateTariffCommittee.Api.Version1.Models.RealityObject;
    using Newtonsoft.Json;
    using Npgsql;
    using System.Web;

    using Bars.Gkh.BaseApiIntegration.Models;
    using Bars.Gkh.Regions.Tatarstan.Enums.Administration;

    /// <summary>
    /// API сервис для работы с <see cref="Entities.RealityObject"/>
    /// </summary>
    public class RealityObjectService : IRealityObjectService
    {
        /// <summary>
        /// OKTMO
        /// </summary>
        private long? oktmo;

        /// <summary>
        /// Отчетный период
        /// </summary>
        private DateTime? period;

        /// <summary>
        /// Наименование таблицы с информацией о ЛС
        /// </summary>
        private string lsAccountInfoTableName;

        /// <summary>
        /// Наименование таблицы с информацией о постраничном выводе
        /// </summary>
        private string pagingTableName;

        /// <summary>
        /// Информация о постраничном выводе
        /// </summary>
        private PagedTableInfo pagedTableInfo;

        /// <summary>
        /// Необходимость удаления таблиц после выполнения метода
        /// </summary>
        private bool needDeleteTablesAfterExecution = true;

        /// <summary>
        /// Лог операции
        /// </summary>
        private TariffDataIntegrationLog log;

        private readonly IGkhUserManager _userManager;
        private readonly IFileManager _fileManager;
        private readonly IDomainService<TariffDataIntegrationLog> _logDomain;

        /// <inheritdoc cref="RealityObjectService" />
        public RealityObjectService(
            IGkhUserManager userManager,
            IFileManager fileManager,
            IDomainService<TariffDataIntegrationLog> logDomain)
        {
            _userManager = userManager;
            _fileManager = fileManager;
            _logDomain = logDomain;
        }

        /// <inheritdoc />
        public IEnumerable<RealityObject> List(long? oktmo, DateTime? period) =>
            (IEnumerable<RealityObject>)this.Execute(TariffDataIntegrationMethod.RequestHousingFacilities, oktmo, period, sqlExecutor =>
            {
                var sqlQuery = this.GetSqlQueryForCreatingLsAccountInfoTable();
                sqlExecutor.ExecuteSqlWithOutErrorHandler(sqlQuery);

                sqlQuery = this.GetSqlQueryForFuncWithMainTable("get_consumer_full_address");
                return sqlExecutor.ExecuteSqlWithOutErrorHandler<RealityObject>(sqlQuery)
                    .GroupBy(x => new
                    {
                        x.HouseGuid,
                        x.Municipality,
                        x.Locality,
                        x.Ulica,
                        x.HouseNum,
                        x.StrucNum,
                        x.BuildNum,
                        x.HouseType.Code,
                        x.HouseType.Name,
                        x.Floors
                    })
                    .Select(x =>
                    {
                        var result = x.First();

                        result.Premises = x.Select(y => new BasePremise
                        {
                            PremiseId = y.PremiseId,
                            PremiseNum = y.PremiseNum,
                            PremiseTypeCode = y.PremiseTypeCode,
                            PremiseTypeName = y.PremiseTypeName
                        });

                        return result;
                    });;
            });

        /// <inheritdoc />
        public IEnumerable<RealityObjectInConsumerContext> ListInConsumerContext(long? oktmo, DateTime? period) =>
            (IEnumerable<RealityObjectInConsumerContext>) this.Execute(TariffDataIntegrationMethod.RequestPremises, oktmo, period, sqlExecutor =>
            {
                var sqlQuery = this.GetSqlQueryForCreatingLsAccountInfoTable();
                sqlExecutor.ExecuteSqlWithOutErrorHandler(sqlQuery);

                sqlQuery = this.GetSqlQueryForFuncWithMainTable("get_main_data_house");
                var houses = sqlExecutor.ExecuteSqlWithOutErrorHandler<RealityObjectInConsumerContext>(sqlQuery);

                sqlQuery = this.GetSqlQueryForFuncWithMainTableAndPeriod("get_parameters_object");
                var options = sqlExecutor.ExecuteSqlWithOutErrorHandler<Options>(sqlQuery);

                sqlQuery = this.GetSqlQueryForFuncWithMainTable("get_main_data_room");
                var rooms = sqlExecutor.ExecuteSqlWithOutErrorHandler<RoomInConsumerContext>(sqlQuery);

                sqlQuery = this.GetSqlQueryForFuncWithMainTable("get_main_data_premise");
                var premisesLookUp = sqlExecutor.ExecuteSqlWithOutErrorHandler<PremiseInConsumerContext>(sqlQuery)
                    .ToLookup(x => x.HouseGuid);

                var roomsOptionsDict = options
                    .Where(x => x.RoomId != default)
                    .ToDictionary(x => x.RoomId);

                rooms.ForEach(x => x.Options = roomsOptionsDict.Get(x.RoomId));

                var premisesRoomsLookUp = rooms.Where(x => x.PremiseId != 0)
                    .ToLookup(x => x.PremiseId);
                var housesRoomsLookUp = rooms.Where(x => x.PremiseId == 0 && !string.IsNullOrEmpty(x.HouseGuid))
                    .ToLookup(x => x.HouseGuid);

                var premisesOptionsDict = options
                    .Where(x => x.PremiseId != default)
                    .Where(x => x.RoomId == default)
                    .ToDictionary(x => x.PremiseId);

                premisesLookUp.ForEach(x =>
                    x.ForEach(y =>
                    {
                        y.Options = premisesOptionsDict.Get(y.PremiseId);
                        y.Rooms = premisesRoomsLookUp.Get(y.PremiseId);
                    })
                );

                var housesOptionsDict = options
                    .Where(x => !string.IsNullOrEmpty(x.HouseGuid))
                    .Where(x => x.PremiseId == default && x.RoomId == default)
                    .ToDictionary(x => x.HouseGuid);
                
                houses.ForEach(x =>
                {
                    x.Options = housesOptionsDict.Get(x.HouseGuid);
                    x.Premises = premisesLookUp.Get(x.HouseGuid);
                    x.Rooms = housesRoomsLookUp.Get(x.HouseGuid);
                });

                return houses;
            });

        /// <inheritdoc />
        public PagedApiServiceResult ListInConsumerTariffContext(long? oktmo, DateTime? period, Guid? pageGuid) =>
            (PagedApiServiceResult)this.Execute(TariffDataIntegrationMethod.RequestInformationTariffs, oktmo, period,
                sqlExecutor =>
                {
                    /*
                     * Данный метод подразумевает только ПОСЛЕДОВАТЕЛЬНОЕ выполнение и только от ОДНОГО пользователя
                     * При параллельном запуске несколькими пользователями корректность данных для них обоих будет нарушена!
                     */

                    var sqlQuery = string.Empty;
                    this.pagingTableName = this.GetTableNameWithPrefix("paging_info");

                    // Подменяем стандартное наименование т.к. метод ориентируется на конкретное название (по указанным ОКТМО и периоду)
                    this.lsAccountInfoTableName = this.GetTableNameWithPrefix("paged_ls_account_info");

                    if (pageGuid == null)
                    {
                        sqlQuery = this.GetSqlQueryForCreatingLsAccountInfoTable();
                        sqlExecutor.ExecuteSqlWithOutErrorHandler(sqlQuery);
                    }
                    else
                    {
                        // При выводе конкретной страницы проставляем отрицательный признак удаления,
                        // чтобы при возникновении ошибок при формироании данных сохранялась возможность их повторно запросить 
                        this.needDeleteTablesAfterExecution = false;
                    }

                    sqlQuery = this.GetSqlQueryForGettingPagedTableInfo(pageGuid);
                    this.pagedTableInfo = sqlExecutor.ExecuteSqlWithOutErrorHandler<PagedTableInfo>(sqlQuery).First();

                    // Дополняем описание параметров метода
                    this.log.Parameters += $" (nextPageGuid '{this.pagedTableInfo.NextPageGuid}')";

                    sqlQuery = this.GetSqlQueryForFuncWithMainTableAndPaging("get_information_house");
                    var houses = sqlExecutor.ExecuteSqlWithOutErrorHandler<RealityObjectInConsumerTariffContext>(sqlQuery);

                    sqlQuery = this.GetSqlQueryForFuncWithMainTableAndPaging("get_information_premise");
                    var premisesLookup = sqlExecutor.ExecuteSqlWithOutErrorHandler<PremiseInConsumerTariffContext>(sqlQuery)
                        .ToLookup(x => x.HouseGuid);

                    sqlQuery = this.GetSqlQueryForFuncWithMainTableAndPaging("get_information_room");
                    var rooms = sqlExecutor.ExecuteSqlWithOutErrorHandler<RoomInConsumerTariffContext>(sqlQuery);

                    sqlQuery = this.GetSqlQueryForFuncWithMainTablePeriodAndPaging("get_information_tariff");
                    var tariffData = sqlExecutor.ExecuteSqlWithOutErrorHandler<TariffData>(sqlQuery);

                    var roomsWithPremiseLookup = rooms.Where(x => x.PremiseId != default)
                        .ToLookup(x => new { x.HouseGuid, x.PremiseId });

                    var roomsWithOutPremiseLookup = rooms.Where(x => x.PremiseId == default)
                        .ToLookup(x => x.HouseGuid);

                    var houseTariffDataLookup = tariffData
                        .Where(x => x.PremiseId == default && x.RoomId == default)
                        .ToLookup(x => x.HouseGuid);

                    var premiseTariffDataLookup = tariffData
                        .Where(x => x.PremiseId != default && x.RoomId == default)
                        .ToLookup(x => new { x.HouseGuid, x.PremiseId });

                    var roomWithPremiseTariffDataLookup = tariffData
                        .Where(x => x.PremiseId != default && x.RoomId != default)
                        .ToLookup(x => new { x.HouseGuid, x.PremiseId, x.RoomId });

                    var roomWithOutPremiseTariffDataLookup = tariffData
                        .Where(x => x.PremiseId == default && x.RoomId != default)
                        .ToLookup(x => new { x.HouseGuid, x.RoomId });

                    roomsWithPremiseLookup.ForEach(x => x.ForEach(y =>
                    {
                        y.TariffData = roomWithPremiseTariffDataLookup.Get(new { y.HouseGuid, y.PremiseId, y.RoomId });
                    }));

                    roomsWithOutPremiseLookup.ForEach(x => x.ForEach(y =>
                    {
                        y.TariffData = roomWithOutPremiseTariffDataLookup.Get(new { y.HouseGuid, y.RoomId });
                    }));

                    premisesLookup.ForEach(x => x.ForEach(y =>
                    {
                        y.TariffData = premiseTariffDataLookup.Get(new { y.HouseGuid, y.PremiseId });
                        y.Rooms = roomsWithPremiseLookup.Get(new { y.HouseGuid, y.PremiseId });
                    }));

                    houses.ForEach(x =>
                    {
                        x.TariffData = houseTariffDataLookup.Get(x.HouseGuid);
                        x.Premises = premisesLookup.Get(x.HouseGuid);
                        x.Rooms = roomsWithOutPremiseLookup.Get(x.HouseGuid);
                    });

                    var parsedNextPageGuid = this.pagedTableInfo.NextPageGuid.IsNotEmpty()
                        ? Guid.Parse(this.pagedTableInfo.NextPageGuid)
                        : (Guid?)null;

                    // Удаление таблиц только после отправки последней страницы
                    this.needDeleteTablesAfterExecution = !parsedNextPageGuid.HasValue;

                    return new PagedApiServiceResult { Data = houses, NextPageGuid = parsedNextPageGuid };
                },
                () =>
                {
                    // Корректируем стандартное описание параметров метода
                    this.log.Parameters += $" pageGuid = '{pageGuid?.ToString()}'";
                });

        /// <summary>
        /// Инициализация параметров ОКТМО и отчетного периода и лога
        /// </summary>
        private void OktmoAndPeriodParametersInit(long? oktmo, DateTime? period)
        {
            if (oktmo == null)
            {
                throw new ApiServiceException("Не указан обязательный параметр: oktmo");
            }

            if (_logDomain.GetAll().Any(x => x.ExecutionStatus == ExecutionStatus.InProgress))
            {
                throw new ApiServiceException("Метод уже выполняется, просьба дождаться завершения данного метода");
            }

            if (period == null)
            {
                period = DateTime.Today.AddMonths(-1);
            }

            this.oktmo = oktmo;
            this.period = period;
            this.lsAccountInfoTableName = $"{this.GetTableNameWithPrefix("ls_account_info")}_{DateTime.Now.Ticks}";

            this.log = new TariffDataIntegrationLog
            {
                User = _userManager.GetActiveUser(),
                StartMethodTime = DateTime.Now,
                Parameters = $"OKTMO = {oktmo}, period = {period:dd.MM.yyyy}",
                ExecutionStatus = ExecutionStatus.InProgress
            };
        }

        /// <summary>
        /// Получить наименование таблицы с префиксом
        /// </summary>
        private string GetTableNameWithPrefix(string prefix) =>
            $"{prefix}_{this.oktmo}_{this.period:yyyy_MM}";

        /// <summary>
        /// Получить запрос для создания таблицы с информацией о привязках ЛС к домам, помещениям и комнатам
        /// </summary>
        private string GetSqlQueryForCreatingLsAccountInfoTable() =>
            $"SELECT * FROM tariff.create_ls_account_info_table('{this.lsAccountInfoTableName}', '{this.oktmo}', '{this.period:dd.MM.yyyy}');";

        /// <summary>
        /// Получить запрос для определения информации о постраничном выводе
        /// </summary>
        private string GetSqlQueryForGettingPagedTableInfo(Guid? pageGuid) =>
            $"SELECT * FROM tariff.get_paged_table_info('{this.lsAccountInfoTableName}', '{this.pagingTableName}', '{pageGuid?.ToString()}');";

        /// <summary>
        /// Получить запрос для функции с указанием основной таблицы
        /// </summary>
        private string GetSqlQueryForFuncWithMainTable(string funcName) =>
            $"SELECT * FROM tariff.{funcName}('{this.lsAccountInfoTableName}');";

        /// <summary>
        /// Получить запрос для функции с указанием основной таблицы и периода
        /// </summary>
        private string GetSqlQueryForFuncWithMainTableAndPeriod(string funcName) =>
            $"SELECT * FROM tariff.{funcName}('{this.lsAccountInfoTableName}', '{this.period:dd.MM.yyyy}');";

        /// <summary>
        /// Получить запрос для функции с указанием основной таблицы, таблицы с пагинацией и страницы
        /// </summary>
        private string GetSqlQueryForFuncWithMainTableAndPaging(string funcName) =>
            $"SELECT * FROM tariff.{funcName}('{this.lsAccountInfoTableName}', '{this.pagingTableName}', '{this.pagedTableInfo.PageGuid}');";

        /// <summary>
        /// Получить запрос для функции с указанием основной таблицы, таблицы с пагинацией и страницы
        /// </summary>
        private string GetSqlQueryForFuncWithMainTablePeriodAndPaging(string funcName) =>
            $"SELECT * FROM tariff.{funcName}('{this.lsAccountInfoTableName}', '{this.period:dd.MM.yyyy}', '{this.pagingTableName}', '{this.pagedTableInfo.PageGuid}');";

        /// <summary>
        /// Получить запрос для удаления таблицы
        /// </summary>
        private string GetSqlQueryForDropTable(string tableName) =>
            $"DROP TABLE IF EXISTS tariff.{tableName};";

        /// <summary>
        /// Выполнение метода по получению данных
        /// </summary>
        /// <typeparam name="T">Тип возвращаемого результата</typeparam>
        /// <param name="method">Наименование метода</param>
        /// <param name="oktmo">ОКТМО</param>
        /// <param name="period">Отчетный период</param>
        /// <param name="operation">Операция получения данных</param>
        /// <param name="beforeOperationAction"></param>
        /// <returns>Коллекция полученных данных</returns>
        private object Execute(TariffDataIntegrationMethod method, long? oktmo, DateTime? period,
            Func<RisDatabaseSqlExecutor, object> operation, Action beforeOperationAction = null)
        {
            this.OktmoAndPeriodParametersInit(oktmo, period);

            string errorMessage = null;

            using (var sqlExecutor = new RisDatabaseSqlExecutor())
            {
                try
                {
                    beforeOperationAction?.Invoke();

                    this.CreateUpdateLog(method, errorMessage);

                    return operation(sqlExecutor);
                }
                catch (Exception e)
                {
                    errorMessage = e is PostgresException npgSqlEx
                        ? npgSqlEx.MessageText
                        : e.Message;

                    throw;
                }
                finally
                {
                    this.CreateUpdateLog(method, errorMessage);

                    try
                    {
                        if (this.needDeleteTablesAfterExecution)
                        {
                            sqlExecutor.ExecuteSqlWithOutErrorHandler(this.GetSqlQueryForDropTable(this.lsAccountInfoTableName));

                            if (this.pagingTableName.IsNotEmpty())
                            {
                                sqlExecutor.ExecuteSqlWithOutErrorHandler(this.GetSqlQueryForDropTable(this.pagingTableName));
                            }
                        }
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
        }

        /// <summary>
        /// Создание лога операции
        /// </summary>
        /// <param name="method">Выполняемый метод</param>
        /// <param name="errorMessage">Сообщение об ошибке</param>
        private void CreateUpdateLog(TariffDataIntegrationMethod method, string errorMessage)
        {
            log.TariffDataIntegrationMethod = method;

            if (log.Id == default(long))
            {
                _logDomain.Save(log);
                return;
            }
            
            var jsonSerializer = new JsonSerializer();
            using (var ms = new MemoryStream())
            using (var sw = new StreamWriter(ms))
            using (var jsonWriter = new JsonTextWriter(sw))
            {
                var status = ExecutionStatus.Success;

                if (errorMessage == null)
                {
                    jsonSerializer.Serialize(jsonWriter, new
                    {
                        status = $"{200} {HttpWorkerRequest.GetStatusDescription(200)}"
                    });
                }
                else
                {
                    status = ExecutionStatus.Error;

                    jsonSerializer.Serialize(jsonWriter, new
                    {
                        status = $"{400} {HttpWorkerRequest.GetStatusDescription(400)}",
                        error = new { message = errorMessage }
                    });
                }

                jsonWriter.Flush();
                ms.Seek(0, SeekOrigin.Begin);

                log.ExecutionStatus = status;
                log.LogFile = _fileManager.SaveFile($"{status.GetDisplayName()}.json", ms.ReadAllBytes());
            }

            _logDomain.Update(log);
        }

        /// <summary>
        /// Информация о постраничном выводе
        /// </summary>
        private class PagedTableInfo
        {
            /// <summary>
            /// Guid текущей страницы
            /// </summary>
            public string PageGuid { get; set; }

            /// <summary>
            /// Guid следующей страницы
            /// </summary>
            public string NextPageGuid { get; set; }
        }
    }
}