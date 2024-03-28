namespace Bars.Gkh.Gis.DomainService.ImportExport.Impl
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Authentification;
    using B4;
    using B4.Config;
    using B4.DataAccess;
    using B4.Modules.FileStorage;
    using B4.Modules.Security;
    using B4.Utils;

    using Bars.B4.IoC;
    using Bars.Gkh.Gis.DomainService.BilConnection;
    using Bars.Gkh.Gis.KP_legacy;
    using Bars.Gkh.RegOperator.Entities;

    using Castle.MicroKernel.Lifestyle;
    using Castle.Windsor;
    using Dapper;
    using Entities.Register.LoadedFileRegister;
    using Enum;
    using Gkh.Entities;
    using Gkh.Utils;

    public class UnloadCounterValuesService : IUnloadCounterValuesService
    {
        private ConcurrentDictionary<int, LoadedFileRegister> LogDict = new ConcurrentDictionary<int, LoadedFileRegister>();
        
        /// <summary>
        /// IWindsorContainer
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Cервис получения строк соединения к серверам БД биллинга
        /// </summary>
        public IBilConnectionService BilConnectionService { get; set; }

        /// <summary>
        /// Выгрузить показания ПУ
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult Unload(BaseParams baseParams)
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var operatorContragentDomain = this.Container.ResolveDomain<OperatorContragent>();
            var cashPaymentCenter = this.Container.ResolveDomain<CashPaymentCenter>();
            var loadFileRegisterDomain = this.Container.ResolveDomain<LoadedFileRegister>();

            try
            {
                var user = userManager.GetActiveUser();

                if (user == null)
                {
                    return new BaseDataResult(false, "Не удалось получить текущего пользователя");
                }

                var userContragentList = operatorContragentDomain.GetAll()
                    .Join(
                        cashPaymentCenter.GetAll(),
                        x => x.Contragent.Id,
                        y => y.Contragent.Id,
                        (x, y) => new { operContr = x, cashPaymCent = y })
                    .Where(x => x.operContr.Operator != null)
                    .Where(x => x.operContr.Operator.User.Id == user.Id)
                    .Select(
                        x => new
                        {
                            OrganizationCode = x.cashPaymCent.Identifier,
                            contragentName = x.cashPaymCent.Contragent.Name
                        })
                    .ToList();

                if (!userContragentList.Any())
                {
                    return new BaseDataResult(
                          false,
                          "У вашей организации отсутствует код ЕРЦ, для его получения просьба написать на электронную почту " +
                              "МСАЖКХ РТ-Хасанова Фарида Ирековна Farida.Hasanova@tatar.ru");
                }

                //проверяем коды ЕРЦ
                foreach (var contragent in userContragentList)
                {
                    var ercCode = contragent.OrganizationCode.ToInt();
                    if (ercCode == 0)
                        return new BaseDataResult(
                            false,
                            "У вашей организации отсутствует код ЕРЦ, для его получения просьба написать на электронную почту " +
                                "МСАЖКХ РТ-Хасанова Фарида Ирековна Farida.Hasanova@tatar.ru");
                }

                //выгрузка уже запущена
                if(loadFileRegisterDomain
                    .GetAll()
                    .Where(x => x.Format == TypeImportFormat.UnloadCounterValuesFromPgmuRt)
                    .Where(x =>
                        x.B4User.Id == user.Id)
                    .Any(
                        x => x.TypeStatus == TypeStatus.InProgress ||
                            x.TypeStatus == TypeStatus.Queuing ||
                            x.TypeStatus == TypeStatus.PreQueuing ||
                            x.TypeStatus == TypeStatus.Checking))
                {
                    return new BaseDataResult(false, "Происходит выгрузка показаний приборов учета");
                }

                //проверки прошли успешно, начинаем выгрузку
                var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 5 };

                Task.Run(() =>
                    Parallel.ForEach(userContragentList,
                        parallelOptions,
                        contragent =>
                        {
                            using (Container.BeginScope())
                            {
                                this.UnloadCountersValuesFromBilling(loadFileRegisterDomain,
                                    contragent.OrganizationCode.ToInt(),
                                    contragent.contragentName,
                                    user);
                            }
                        }));

                return new BaseDataResult();
            }
            finally
            {
                this.Container.Release(userManager);
                this.Container.Release(operatorContragentDomain);
                this.Container.Release(cashPaymentCenter);
                this.Container.Release(loadFileRegisterDomain);
            }
        }

        /// <summary>
        /// Получить список выгрузок показаний ПУ
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult GetList(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var loadFileRegisterDomain = this.Container.ResolveDomain<LoadedFileRegister>();
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var operatorContragentDomain = this.Container.ResolveDomain<OperatorContragent>();
            var cashPaymentCenter = this.Container.ResolveDomain<CashPaymentCenter>();
            var userDomain = this.Container.ResolveDomain<User>();

            try
            {
                var user = userManager.GetActiveUser();

                if (user == null)
                {
                    return null;
                }

                var monthFilter = loadParam.FindInComplexFilter("Month");

                DateTime? filterForMonth = null;

                if (monthFilter != null)
                {
                    filterForMonth = new DateTime(monthFilter.Value.ToDateTime().Year, monthFilter.Value.ToDateTime().Month, 1);
                    loadParam.SetComplexFilterNull("Month");
                }
                
                var loadFileRegister = loadFileRegisterDomain.GetAll()
                    .Where(x => x.Format == TypeImportFormat.UnloadCounterValuesFromPgmuRt)
                    .Where(x => x.CalculationDate.HasValue)
                    .Where(x => x.B4User != null);

                var userIsAdmin = user.Roles.Any(y => y.Role.Name == "Администратор");
                var userIds = userIsAdmin
                    ? loadFileRegister
                        .Select(x => x.B4User.Id)
                        .Distinct()
                    : userDomain.GetAll().Where(x => x.Id == user.Id).Select(x => x.Id);

                var userContragentDict = operatorContragentDomain
                    .GetAll()
                    .Where(x => x.Operator != null)
                    .Where(x => userIds.Contains(x.Operator.User.Id))
                    .Join(cashPaymentCenter.GetAll(),
                        x => x.Contragent.Id,
                        y => y.Contragent.Id,
                        (x, y) => new
                        {
                            UserId = x.Operator.User.Id,
                            OrganizationName = y.Contragent.Name,
                            OrganizationCode = y.Identifier
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.UserId, x => new
                    {
                        x.OrganizationCode,
                        x.OrganizationName
                    })
                    .ToDictionary(x => x.Key, x => x.First());

                var data = loadFileRegister
                    //если текущий пользователь не админ, то показываем только его файлы
                    .WhereIf(!userIsAdmin, x => x.B4User.Id == user.Id)
                    .WhereIf(filterForMonth.HasValue, x => x.CalculationDate.Value == filterForMonth.Value)
                    .AsEnumerable()
                    .Select(x => new
                    {
                        FormationDate = x.ObjectCreateDate.ToUniversalTime(),
                        Month = new DateTime(x.CalculationDate.Value.Year, x.CalculationDate.Value.Month, 1),
                        User = x.B4User.Name,
                        OrganizationName = x.SupplierName ?? userContragentDict.Get(x.B4User.Id)?.OrganizationName ?? "",
                        OrganizationCode = userContragentDict.Get(x.B4User.Id)?.OrganizationCode ?? "",
                        x.TypeStatus,
                        x.File,
                        x.Log
                    })
                    .AsQueryable()
                    .OrderIf(loadParam.Order.Length == 0, false, x => x.FormationDate)
                    .OrderThenIf(loadParam.Order.Length == 0, false, x => x.Month)
                    .Filter(loadParam, this.Container);

                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), data.Count());
            }
            finally
            {
                this.Container.Release(loadFileRegisterDomain);
                this.Container.Release(userManager);
                this.Container.Release(operatorContragentDomain);
                this.Container.Release(cashPaymentCenter);
            }
        }

        /// <summary>
        /// Выгрузка показаний ПУ из ПГУ по одному ЕРЦ
        /// </summary>
        /// <param name="loadFileRegisterDomain"></param>
        /// <param name="ercCode">Код ЕРЦ, для которого надо выгрузить показания ПУ</param>
        /// <param name="user">Пользователь в МЖФ</param>
        /// <returns></returns>
        public void UnloadCountersValuesFromBilling(IDomainService<LoadedFileRegister> loadFileRegisterDomain, int ercCode, string contragentName, User user)
        {
            // Здесь будет результат работы, который потом запишется в лог-файл
            var listCounters = new List<string>();

            // Здесь будет протокол выгрузки
            var unloadLog = new List<string>();

            //статус загрузки
            var unloadStatus = TypeStatus.InProgress;

            //расчетный месяц
            var calculationDate = new DateTime();

            //уникальный код пачки
            var newNzpPack = 0;

            unloadLog.Add("Время начала выгрузки: " + DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss"));

            this.AddLog(loadFileRegisterDomain, unloadLog, newNzpPack, ercCode, listCounters, user, unloadStatus, calculationDate, contragentName, true);

            //подключаемся к БД ПГУ
            var provider = new ConnectionProvider(this.Container.Resolve<IConfigProvider>());
            provider.Init(this.BilConnectionService.GetConnection(ConnectionType.GisConnStringPgu));

            using (var dbConnection = provider.CreateConnection())
            {
                try
                {
                    dbConnection.Open();
                }
                catch (Exception ex)
                {
                    unloadLog.Add("Ошибка при подключении к БД ПГМУ: " + ex);
                    unloadStatus = TypeStatus.Error;

                    this.AddLog(loadFileRegisterDomain, unloadLog, newNzpPack, ercCode, listCounters, user, unloadStatus, calculationDate, contragentName);
                }

                using (var transaction = dbConnection.BeginTransaction())
                {
                    try
                    {
                        // Проверяем наличие новых показаний ПУ в counters_ord
                        var sqlQuery =
                            $@" SELECT COUNT(*) as count 
                            FROM webfon.counters_ord 
                            WHERE cur_val IS NOT NULL 
                            AND nzp_pack_ls IS NULL
                            AND webfon.extract_erc(pkod) = {ercCode};";

                        if (dbConnection.ExecuteScalar<int>(sqlQuery) == 0)
                        {
                            unloadStatus = TypeStatus.NoData;
                            unloadLog.Add("Нет новых показаний ПУ!");

                            this.AddLog(loadFileRegisterDomain, unloadLog, newNzpPack, ercCode, listCounters, user, unloadStatus, calculationDate, contragentName);
                            return;
                        }

                        try
                        {
                            // Создаем описание пачки в counters_pack
                            sqlQuery =
                                @" insert into webfon.counters_pack (source_name, dat_pack, dat_when) 
                                   values ('portal',current_date,current_timestamp);";
                            dbConnection.Execute(sqlQuery, transaction: transaction);

                            // Определяем новое serial-значение nzp_pack
                            sqlQuery =
                                "select lastval() as key";

                            newNzpPack = dbConnection.ExecuteScalar<int>(sqlQuery, transaction: transaction);

                            unloadLog.Add("Код пачки: " + newNzpPack);

                            // Выбираем те записи, которые собираемся выгрузить, и переносим информацию о них в counters_pack_ls
                            sqlQuery =
                                $@" INSERT INTO webfon.counters_pack_ls
                                (nzp_pack, nzp_ck, pref, num_ls, pkod, dat_month, dat_vvod, order_num, cur_val, nzp_serv, service, num_cnt) 
                                SELECT {newNzpPack} , nzp_ck, pref, num_ls, pkod, dat_month, dat_vvod, order_num, cur_val, nzp_serv, service, num_cnt 
                                from webfon.counters_ord where 1=1 and cur_val is not null and nzp_pack_ls is null 
                                and webfon.extract_erc(pkod) = {ercCode};";
                            dbConnection.Execute(sqlQuery, transaction: transaction);

                            // Проставляем показаниям из counters_ord коды nzp_pack_ls
                            sqlQuery =
                                $@" UPDATE webfon.counters_ord c 
                                SET nzp_pack_ls = p.nzp_pack_ls 
                                FROM webfon.counters_pack_ls p 
                                WHERE p.nzp_ck = c.nzp_ck
                                AND p.nzp_pack = {newNzpPack}
                                AND c.cur_val is NOT NULL
                                AND c.nzp_pack_ls IS NULL
                                AND webfon.extract_erc(c.pkod) = {ercCode};";

                            dbConnection.Execute(sqlQuery, transaction: transaction, commandTimeout: 3600);

                            // Подсчет кол-ва ЛС
                            sqlQuery =
                                $@" update webfon.counters_pack set cnt_ls = 
                                coalesce(( select count(distinct pkod) from webfon.counters_pack_ls  ls 
                                where ls.nzp_pack = webfon.counters_pack.nzp_pack), 0) 
                                where nzp_pack={newNzpPack};";
                            dbConnection.Execute(sqlQuery, transaction: transaction);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Ошибка при считывании из БД показаний ПУ: " + ex);
                        }

                        // Промежуточная таблица с записями на выгрузку
                        var tempPackLsTable = $"t_cnt_pck_ls_{DateTime.Now.Ticks}";
                        sqlQuery = $"drop table if exists {tempPackLsTable}";
                        dbConnection.Execute(sqlQuery);

                        sqlQuery =
                            $@" create temp table {tempPackLsTable} as 
                            select * from webfon.counters_pack_ls limit 0; ";
                        dbConnection.Execute(sqlQuery);

                        sqlQuery =
                            $@" insert into {tempPackLsTable} 
                            select * 
                            from webfon.counters_pack_ls 
                            where nzp_pack = {newNzpPack};";
                        dbConnection.Execute(sqlQuery);
                        sqlQuery = $"create index ix_kljwh5344_01 on {tempPackLsTable} (pref)";
                        dbConnection.Execute(sqlQuery);
                        sqlQuery = $"create index ix_kljwh5344_02 on {tempPackLsTable} (dat_month, pkod)";
                        dbConnection.Execute(sqlQuery);
                        sqlQuery = $"analyze {tempPackLsTable}";
                        dbConnection.Execute(sqlQuery);

                        // Реализация выгрузки в файл
                        sqlQuery =
                            $@" select 
                            nzp_pack AS NzpPack, 
                            TRIM(source_name) AS SourceName, 
                            dat_pack AS PackDate 
                            from webfon.counters_pack 
                            where nzp_pack = {newNzpPack}";

                        var packInfo = dbConnection.Query<CounterValuePackInfo>(sqlQuery).FirstOrDefault();

                        sqlQuery =
                            $@" select pkod AS Pkod, 
                            dat_month AS DatMonth, 
                            TRIM(pref) AS Pref, 
                            count(*) AS Cnt 
                            from {tempPackLsTable} 
                            group by 1,2,3 
                            order by pkod ";

                        var pack = dbConnection.Query<CounterValuePack>(sqlQuery);

                        calculationDate = pack.FirstOrDefault().DatMonth;
                        unloadLog.Add("Расчетный месяц: " + calculationDate.ToShortDateString());

                        sqlQuery =
                            $@" select distinct pkod AS Pkod,
                            dat_month DatMonth, 
                            dat_vvod DatVvod, 
                            TRIM(pref) Pref, 
                            order_num OrderNum, 
                            cur_val CurVal, 
                            service Service, 
                            TRIM(num_cnt)  NumCnt
                            from {tempPackLsTable} 
                            order by pkod, dat_month, order_num ";

                        var counterValues = dbConnection.Query<CounterValue>(sqlQuery);

                        //формируем информационное описание
                        listCounters.Add(
                            $"***|{packInfo.SourceName.Trim()}|{packInfo.NzpPack}|{packInfo.PackDate.ToShortDateString()}|{packInfo.PackDate.ToShortDateString()}|{pack.Count()}|");

                        foreach (var onePack in pack)
                        {
                            listCounters.Add($"###|{onePack.Pkod}|{onePack.DatMonth.ToShortDateString()}|{onePack.Cnt}|{onePack.Pref.Trim()}|");

                            counterValues
                                .Where(
                                    x => x.Pkod == onePack.Pkod
                                        && x.DatMonth == onePack.DatMonth
                                        && x.Pref == onePack.Pref)
                                .ForEach(
                                    x =>
                                        listCounters.Add(
                                            $"@@@|{x.OrderNum}|{x.CurVal}|{x.Service.Trim()}|{x.DatVvod.ToShortDateString()}|{x.NumCnt.Trim()}|"));
                        }

                        unloadLog.Add("Показания ПУ успешно выгружены!");
                        unloadStatus = TypeStatus.Done;

                        this.AddLog(loadFileRegisterDomain, unloadLog, newNzpPack, ercCode, listCounters, user, unloadStatus, calculationDate, contragentName);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        unloadLog.Add("Ошибка: " + ex);
                        unloadStatus = TypeStatus.ProcessingError;

                        this.AddLog(loadFileRegisterDomain, unloadLog, newNzpPack, ercCode, listCounters, user, unloadStatus, calculationDate, contragentName);
                    }
                }
            }
        }

        private void AddLog(IDomainService<LoadedFileRegister> loadFileRegisterDomain, List<string> unloadLog, int newNzpPack, int ercCode,
            List<string> listCounters, User user, TypeStatus unloadStatus, DateTime calculationDate, string contragentName, bool createNewLog = false)
        {
            var fileName = $"ПОКАЗАНИЯ_ПУ_{newNzpPack}_{DateTime.Now:yyyyMMdd}_ПГМУ_РТ_{ercCode}.txt";

            if (createNewLog)
            {
                //пишем в gis_loaded_file_register
                LogDict[ercCode] = new LoadedFileRegister
                {
                    B4User = user,
                    FileName = "",
                    Size = 0,
                    TypeStatus = unloadStatus,
                    CalculationDate = calculationDate,
                    ImportName = TypeImportFormat.UnloadCounterValuesFromPgmuRt.GetDisplayName(),
                    Format = TypeImportFormat.UnloadCounterValuesFromPgmuRt,
                    SupplierName = contragentName
                };

                loadFileRegisterDomain.Save(LogDict[ercCode]);
                return;
            }

            //сохраняем в файл протокол-загрузки
            var logFileName = $"ПРОТОКОЛ_ВЫГРУЗКИ_{fileName}";
            var exceptionMessage = "Не удалось сохранить лог выгрузки приборов учета";
            var logFile = this.CreateFile(logFileName, unloadLog, exceptionMessage);

            //сохраняем в файл показания ПУ
            exceptionMessage = "Не удалось сохранить файл показаний";
            var file = this.CreateFile(fileName, listCounters, exceptionMessage);
            var log = LogDict[ercCode];
            unloadLog.Add("Время окончания выгрузки: " + DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss"));

            if (LogDict[ercCode] == null)
            {
                throw new Exception("Не удалось сохранить запись о выгрузке приборов учета");
            }

            log.TypeStatus = unloadStatus;
            log.FileName = file.FullName;
            log.File = file;
            log.Size = file.Size;
            log.Log = logFile;
            log.CalculationDate = calculationDate;

            loadFileRegisterDomain.Update(log);
        }

        private FileInfo CreateFile(string fileName, List<string> listData, string exceptionMessage)
        {
            var resultString = string.Join(Environment.NewLine, listData);
            var bytes = System.Text.Encoding.GetEncoding(1251).GetBytes(resultString);

            var fileManager = this.Container.Resolve<IFileManager>();
            using (this.Container.Using(fileManager))
            {
                var file = fileManager.SaveFile(fileName, bytes);
                return file ?? throw new Exception(exceptionMessage);
            }
        }

        /// <summary>
        /// Вспомогательная сущность "Описатель пачки показаний ПУ"
        /// </summary>
        private class CounterValuePackInfo
        {
            /// <summary>
            /// Уникальный код пачки
            /// </summary>
            public int NzpPack { get; set; }

            /// <summary>
            /// Наименование источника показаний ПУ
            /// </summary>
            public string SourceName { get; set; }

            /// <summary>
            /// Дата формирования пачки
            /// </summary>
            public DateTime PackDate { get; set; }
        }

        /// <summary>
        /// Вспомогательная сущность "Пачка показаний ПУ"
        /// </summary>
        private class CounterValuePack
        {
            /// <summary>
            /// Платежный код
            /// </summary>
            public decimal Pkod { get; set; }

            /// <summary>
            /// Расчетный месяц
            /// </summary>
            public DateTime DatMonth { get; set; }

            /// <summary>
            /// Префикс банка данных
            /// </summary>
            public string Pref { get; set; }

            /// <summary>
            /// Кол-во показаний ПУ в пачке
            /// </summary>
            public int Cnt { get; set; }

        }

        /// <summary>
        /// Вспомогательная сущность "Показание ПУ"
        /// </summary>
        private class CounterValue
        {
            /// <summary>
            /// Платежный код
            /// </summary>
            public decimal Pkod { get; set; }

            /// <summary>
            /// Расчетный месяц
            /// </summary>
            public DateTime DatMonth { get; set; }

            /// <summary>
            /// Дата вводка показаний ПУ
            /// </summary>
            public DateTime DatVvod { get; set; }

            /// <summary>
            /// Префикс банка данных
            /// </summary>
            public string Pref { get; set; }

            /// <summary>
            /// Порядковый номер ПУ в ЕПД
            /// </summary>
            public int OrderNum { get; set; }

            /// <summary>
            /// Значение показания ПУ
            /// </summary>
            public decimal CurVal { get; set; }

            /// <summary>
            /// Наименование услуги
            /// </summary>
            public string Service { get; set; }

            /// <summary>
            /// Заводской номер ПУ
            /// </summary>
            public string NumCnt { get; set; }

        }
    }
}