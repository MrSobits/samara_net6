namespace Bars.Gkh.Gis.DomainService.ImportData.Impl.ImportIncremetalData
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text;
    using B4;
    using B4.DataAccess;
    using B4.Modules.FileStorage;
    using B4.Utils;

    using Bars.Gkh.Gis.DomainService.BilConnection;

    using DataResult;
    using Entities.ImportIncrementalData.LoadFromOtherSystems;
    using Entities.Kp50;
    using Entities.Register.LoadedFileRegister;
    using Enum;
    using Gkh.Entities;
    using Ionic.Zip;
    using LoadFromOtherSystems;
    using Npgsql;
    using Supplier = Entities.ImportIncrementalData.LoadFromOtherSystems.Supplier;
    using NHibernate.Transform;

    /// <summary>
    /// Загрузка инкрементальных данных для ГИС ЖКХ РТ
    /// </summary>
    public class ImportIncremetalDataForGis : BaseImportDataHandler
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public ImportIncremetalDataForGis(IRepository<BilDictSchema> bilDictSchemaRepository)
        {
            _bilDictSchemaRepository = bilDictSchemaRepository;
            _importLog = new StringBuilder();
        }

        private readonly StringBuilder _importLog;

        /// <summary>
        /// Справочник префиксов схем баз данных биллинга
        /// </summary>
        private readonly IRepository<BilDictSchema> _bilDictSchemaRepository;

        /// <summary>
        /// Файловый менеджер
        /// </summary>
        public IFileManager FileManager { get; set; }

        /// <summary>
        /// Импорт данных
        /// </summary>
        /// <param name="loadedFiles">Загруженные файлы с данными</param>
        /// <returns>Результат импорта</returns>
        public override IEnumerable<ImportDataResult> ImportData(IEnumerable<LoadedFileRegister> loadedFiles)
        {
            return loadedFiles.Select(ImportData);
        }

        /// <summary>
        /// Импорт данных
        /// </summary>
        /// <param name="loadedFile">Загруженный файл с данными</param>
        /// <returns>Результат импорта</returns>
        public override ImportDataResult ImportData(LoadedFileRegister loadedFile)
        {

            var loadStatus = TypeStatus.InProgress;
            _importLog.Clear();
            _importLog.AppendLine("Дата загрузки: " + DateTime.Now);
            _importLog.AppendLine("Тип загрузки: 'Загрузка инкрементальных данных в ГИС ЖКХ РТ'");
            _importLog.AppendLine("Наименование файла: " + loadedFile.File.FullName);

            //признак успешной загрузки всех банков данных
            var successLoaded = true;
            var loadFromOtherSystem = false;
            IDataResult result = null;
            try
            {
                //проверяем расширение файла
                if (!loadedFile.File.Extention.ToLower().Contains("zip"))
                    throw new Exception("Архив не прошел валидацию! Aрхив должен быть в формате zip");

                using (var mainArchive = ZipFile.Read(FileManager.GetFile(loadedFile.File)))
                {
                    var infoDescript = new InfoDescript();
                    loadFromOtherSystem = mainArchive.FirstOrDefault(x => x.FileName.ToLower().Contains("_filelist.csv")) != null;
                    //Обрабатываем "Файл информационного описания" общего архива
                    var verificationInfo =
                        mainArchive.FirstOrDefault(x => x.FileName.ToLower().Contains("verificationinfo.txt"));

                    //если файл от ППП "Коммунальные платежи"
                    if (verificationInfo != null)
                    {
                        using (var ms = new MemoryStream())
                        {
                            verificationInfo.Extract(ms);
                            ms.Seek(0, SeekOrigin.Begin);

                            AnalyzeVerificationInfo(new StreamReader(ms, Encoding.GetEncoding(1251)), loadedFile,
                                out infoDescript);
                        }
                    }
                    //если файл от сторонних систем
                    else if (loadFromOtherSystem)
                    {
                        var service = Container.Resolve<IRepository<OperatorContragent>>();
                        var config = Container.Resolve<IDbConfigProvider>();
                        var session = Container.Resolve<ISessionProvider>().GetCurrentSession();
                        //Получаем список банков данных
                        var dataBankList = session.CreateSQLQuery(@"select contragent_id as ""ContragentId"",
                            key as ""DataBankKey"", array_to_string(array_agg(d.name), ', ') as ""DataBankName"",
                                    array_to_string(array_agg(m.name), ', ') as ""RajonList""
                                    from public.gis_databank d, public.gkh_dict_municipality m
                                    where d.municipality_id = m.id
                                    group by 1,2
                                    order by 1,2").SetResultTransformer(new AliasToBeanResultTransformer(typeof(DataBank))).List<DataBank>();
                        var contragents = service.GetAll().Where(x => x.Operator.User.Id == loadedFile.B4User.Id).ToList();

                        var suppliers = contragents.Select(contragent => new Supplier
                        {
                            DataSupplier = contragent.Contragent.Name,
                            DataSupplierId = contragent.Contragent.Id,
                            Inn = contragent.Contragent.Inn,
                            Kpp = contragent.Contragent.Kpp,
                            Ogrn = contragent.Contragent.Ogrn,
                            DataBankList = dataBankList.Where(y => y.ContragentId == contragent.Contragent.Id).Select(y => new DataBank
                            {
                                DataBankKey = y.DataBankKey,
                                DataBankName = y.DataBankName,
                                RajonList = y.RajonList
                            }).ToList()
                        }).ToList();
                        if (!suppliers.Any())
                        {
                            loadFromOtherSystem = false;
                            throw new Exception("Данный пользователь не привязан к контрагентам");
                        }
                        var parameters = new IncrementalParams { MgfFileId = loadedFile.File.Id, Suppliers = suppliers };
                        
                        using (var load = new LoadFileFromOtherSystemInstance(mainArchive, parameters, loadedFile, config.ConnectionString, Container.Resolve<IBilConnectionService>()))
                        {
                            result = load.Run();
                            successLoaded = ((BaseDataResult) result.Data).Success;
                        }
                    }
                    else
                    {
                        throw new Exception("Архив не прошел валидацию! Не найден заголовочный файл!");
                    }

                    //названия буферных таблиц (в которые грузим данные и проверяем контрольные показатели)
                    List<string> loadedBufferTables;

                    //названия буферных таблиц по всему банку данных
                    var loadedBufferTablesByBank = new List<string>();

                    //перебираем все банки данных, которые были в файле с информационным описанием
                    foreach (var oneBankInfo in infoDescript.BankInfo)
                    {
                        loadedBufferTablesByBank.Clear();

                        //получаем архив по выбранному банку (в названии файла есть префикс банка данных)
                        var banksData =
                                mainArchive.SelectEntries(String.Format("name = *_{0}_*.zip",
                                    oneBankInfo.SchemaPrefix.Trim()));


                        BilDictSchema billingDataStorage;
                        try
                        {
                            //получаем куда надо загрузить данные
                            billingDataStorage =
                                _bilDictSchemaRepository.GetAll()
                                    .Single(
                                        x =>
                                            x.SenderLocalSchemaPrefix.Trim().ToLower() ==
                                            oneBankInfo.SchemaIdentifier.Trim().ToLower());

                        }
                        catch (Exception ex)
                        {
                            //это означает, что в МЖФ, в таблице public.bil_dict_schema не найдены данные
                            throw new Exception(
                                String.Format(
                                    "Системная ошибка! Не найден адрес хранилища для идентификатора '{0}': {1}",
                                    oneBankInfo.SchemaIdentifier, ex.Message));
                        }

                        //подключаемся к серверу, где лежат данные по данному банку
                        using (var sqlExecutor = new SqlExecutor.SqlExecutor(billingDataStorage.ConnectionString))
                        {
                            try
                            {
                                //перебираем архивы по банку данных
                                foreach (var oneBankData in banksData)
                                {
                                    using (var ms = new MemoryStream())
                                    {
                                        oneBankData.Extract(ms);
                                        ms.Seek(0, SeekOrigin.Begin);

                                        using (var oneBankArchive = ZipFile.Read(ms))
                                        {
                                            LoadData(oneBankArchive, billingDataStorage.LocalSchemaPrefix, sqlExecutor,
                                                infoDescript.LoadId, out loadedBufferTables);

                                            loadedBufferTablesByBank.AddRange(loadedBufferTables);
                                        }
                                    }
                                }

                                //производим контроль целостности данных
                                if (!VerificateData(sqlExecutor, oneBankInfo, billingDataStorage.LocalSchemaPrefix,
                                    infoDescript.LoadId))
                                {
                                    /*
                                    throw new Exception(String.Format(
                                            "Архив не прошел валидацию! Обнаружено нарушение целостности данных в банке '{0}'! " +
                                            " Данные по банку '{0}' не будут загружены!",
                                            oneBankInfo.SchemaPrefix));
                                     * 
                                     * */
                                    _importLog.AppendLine(String.Format(
                                        "Архив не прошел валидацию! Обнаружено нарушение целостности данных в банке '{0}'! "
                                        //+" Данные по банку '{0}' не будут загружены!"
                                            , oneBankInfo.SchemaPrefix));

                                }
                                //else
                                {
                                    //перенос данных в основные таблицы
                                    TransferDataToMainTables(sqlExecutor, loadedBufferTablesByBank, infoDescript.LoadId);

                                    _importLog.AppendLine(String.Format(
                                        "Данные по банку '{0}' успешно загружены!",
                                        oneBankInfo.SchemaPrefix));
                                }
                            }
                            catch (Exception ex)
                            {
                                _importLog.AppendLine("Ошибка при загрузке данных! Текст ошибки: " + ex);
                                successLoaded = false;
                                //откатываем в случае ошибки
                                RoolBack(sqlExecutor, loadedBufferTablesByBank);
                            }
                        }
                    }
                    if (!loadFromOtherSystem)
                    {
                        loadedBufferTablesByBank.Clear();

                        //получаем куда надо загрузить данные
                        //берем первый локальный банк данных, на том же сервере и находится центральный банк
                        var billingDataStorage =
                            _bilDictSchemaRepository.GetAll()
                                .Single(
                                    x =>
                                        x.SenderLocalSchemaPrefix.Trim().ToLower() ==
                                        infoDescript.BankInfo.First().SchemaIdentifier.Trim().ToLower());

                        using (var sqlExecutor = new SqlExecutor.SqlExecutor(billingDataStorage.ConnectionString))
                        {
                            try
                            {
                                //количество загруженных оплат
                                var paymentsCountInDb = 0;

                                //сначала записываем в БД информацию о попытке загрузки
                                var sqlQuery = String.Format(
                                    " INSERT INTO public.sys_imports_register( " +
                                    " calculation_date, " +
                                    " load_date, " +
                                    " filename, " +
                                    " gis_loaded_file_register_id, " +
                                    " import_result)" +
                                    " VALUES ('{0}', '{1}', '{2}', {3}, {4})" +
                                    " RETURNING id ",
                                    infoDescript.CalculationDate,
                                    DateTime.Now,
                                    loadedFile.File.Name,
                                    loadedFile.Id,
                                        (int)TypeStatus.InProgress
                                    );


                                //уникальный код загрузки
                                var sysImportsRegisterId = sqlExecutor.ExecuteScalar<int>(sqlQuery);

                                sqlQuery = String.Format(
                                    " INSERT INTO public.sys_data_sender_register( " +
                                    " sys_imports_register_id, " +
                                    " erc_code)" +
                                    " VALUES ({0}, {1})",
                                    sysImportsRegisterId,
                                    infoDescript.ErcCode
                                    );
                                sqlExecutor.ExecuteSql(sqlQuery);

                                //проверяем не является ли нижний банк верхним
                                //(в таком случае всего один банк, он и верхний и нижний)
                                if (infoDescript.BankInfo.Count() != 1 ||
                                    infoDescript.BankInfo.First().SchemaPrefix.Trim().ToLower() !=
                                    infoDescript.CentralSchemaPrefix.Trim().ToLower())
                                {
                                    //получаем архив по центральному банку
                                    var banksData =
                                        mainArchive.SelectEntries(String.Format("name = *_{0}_*.zip",
                                            infoDescript.CentralSchemaPrefix.Trim()));

                                    //перебираем архивы по центральному банку данных
                                    foreach (var oneBankData in banksData)
                                    {
                                        using (var ms = new MemoryStream())
                                        {
                                            oneBankData.Extract(ms);
                                            ms.Seek(0, SeekOrigin.Begin);

                                            using (var oneBankArchive = ZipFile.Read(ms))
                                            {

                                                LoadData(oneBankArchive, billingDataStorage.CentralSchemaPrefix,
                                                    sqlExecutor,
                                                    infoDescript.LoadId, out loadedBufferTables);

                                                loadedBufferTablesByBank.AddRange(loadedBufferTables);
                                            }
                                        }
                                    }

                                    //проверяем - сходится ли кол-во оплат в центральном банке данных
                                    paymentsCountInDb = sqlExecutor.ExecuteScalar<int>(
                                       String.Format("SELECT COUNT(*) AS total FROM {0}_fin_{1}.pack_ls_{2}",
                                           billingDataStorage.CentralSchemaPrefix,
                                           infoDescript.CalculationDate.ToString("yy"),
                                           infoDescript.LoadId));
                                }
                                else
                                {
                                    //првоерка кол-ва оплат другая в случае ситуации: нижний банк является верхним
                                    paymentsCountInDb = sqlExecutor.ExecuteScalar<int>(
                                       String.Format("SELECT COUNT(*) AS total FROM {0}_fin_{1}.pack_ls",
                                           billingDataStorage.CentralSchemaPrefix,
                                           infoDescript.CalculationDate.ToString("yy")));
                                }




                                if (paymentsCountInDb != infoDescript.PaymentsCount)
                                {
                                    //successLoaded = false;
                                    _importLog.AppendLine();
                                    /*
                                    throw new Exception(
                                        String.Format(
                                        "Архив не прошел валидацию! Обнаружено нарушение целостности данных в центральном банке '{0}'! " +
                                        " Данные по банку '{0}' не будут загружены! Не совпадает количество оплат! {1} вместо {2}",
                                            infoDescript.CentralSchemaPrefix,
                                            paymentsCountInDb,
                                            infoDescript.PaymentsCount));
                                    */

                                    _importLog.AppendLine(
                                        String.Format(
                                            "Архив не прошел валидацию! Обнаружено нарушение целостности данных в центральном банке '{0}'! " +
                                            " Не совпадает количество оплат! {1} вместо {2}",
                                            infoDescript.CentralSchemaPrefix,
                                            paymentsCountInDb,
                                            infoDescript.PaymentsCount));
                                }
                                //else
                                {
                                    //перенос данных в основные таблицы
                                    TransferDataToMainTables(sqlExecutor, loadedBufferTablesByBank,
                                        infoDescript.LoadId);


                                    //ставим статус загрузки 
                                    sqlExecutor.ExecuteSql(
                                        String.Format(
                                            "UPDATE public.sys_imports_register SET import_result = {0} WHERE id = {1} ",
                                                (int)TypeStatus.Done, sysImportsRegisterId));


                                    _importLog.AppendLine(String.Format("Данные по банку '{0}' успешно загружены!",
                                        infoDescript.CentralSchemaPrefix));
                                }
                            }
                            catch (Exception ex)
                            {
                                successLoaded = false;
                                _importLog.AppendLine("Ошибка при загрузке данных! Текст ошибки: " + ex.Message);
                                //откатываем в случае ошибки
                                RoolBack(sqlExecutor, loadedBufferTablesByBank);
                            }
                        }
                    }
                }
                if (!successLoaded)
                    throw new Exception("Обнаружено нарушение целостности данных!");

                loadStatus = TypeStatus.Done;
                _importLog.AppendLine("Данные успешно загружены!");
            }
            catch (PostgresException ex)
            {
                //пишем в протокол загрузки
                var error = String.Format(
                    "Ошибка при загрузке! Загружаемые файлы некорректны! Текст ошибки: '{0}'.\n Местоположение некорректных данных:'{1}'",
                    ex.Message,
                    ex.Where);

                _importLog.AppendLine(error);

                loadStatus = TypeStatus.ProcessingError;
                return new ImportDataResult(false, "Ошибка при загрузке!", loadedFile.Id);
            }
            catch (Exception ex)
            {
                _importLog.AppendLine("Ошибка при загрузке! Текст ошибки: " + ex.Message);
                loadStatus = TypeStatus.ProcessingError;
                return new ImportDataResult(false, "Ошибка при загрузке!", loadedFile.Id);
            }
            finally
            {
                _importLog.AppendLine(DateTime.Now.ToString());
                //сохраняем в файл протокол-загрузки
                var logFileName = String.Format("ПРОТОКОЛ_ЗАГРУЗКИ_{0}", loadedFile.File.Name);
                //сохраняем лог-файл
                if (!loadFromOtherSystem)
                {
                    var resultString = _importLog.ToString();
                    var bytes = Encoding.GetEncoding(1251).GetBytes(resultString);
                    loadedFile.Log = Container.Resolve<IFileManager>().SaveFile(logFileName, "txt", bytes);
                }
                else
                {
                    loadedFile.Log = Container.Resolve<IFileManager>().SaveFile(logFileName, "zip", (Byte[])((BaseDataResult)result.Data).Data);
                }
                loadedFile.TypeStatus = loadStatus;
                Container.Resolve<IDomainService<LoadedFileRegister>>().Update(loadedFile);

            }
            return new ImportDataResult(true, "Успешно загружено!", loadedFile.Id);
        }

        /// <summary>
        /// Обработка файла с инфомрационным описанием
        /// </summary>
        /// <param name="textReader"></param>
        /// <param name="loadedFile"></param>
        /// <param name="infoDescript"></param>
        private void AnalyzeVerificationInfo(TextReader textReader, LoadedFileRegister loadedFile,
            out InfoDescript infoDescript)
        {
            infoDescript = new InfoDescript();
            var mainInformation = textReader.ReadLine();
            if (mainInformation == null)
            {
                throw new Exception(Environment.NewLine +
                                    "Архив не прошел валидацию! Не заполнено информационное описание!");
            }

            //разделитель полей 
            var fields = mainInformation.Split('|');

            if (fields.Count() != 14)
            {
                throw new Exception(Environment.NewLine +
                                    "Архив не прошел валидацию! Информационное описание не соответствует формату!");
            }

            try
            {
                //Считываем версию формата
                var formatVersion = fields[1];

                //Наименование организации-отправителя 
                var senderOrganizationName = fields[2];

                //Подразделение организации-отправителя 
                var rajonName = fields[3];

                //ИНН организации-отправителя
                var senderInn = fields[4];

                //КПП организации-отправителя
                var senderKpp = fields[5];

                //Код расчетного центра
                infoDescript.ErcCode = Convert.ToInt64(fields[6]);

                //Время начала выгрузки файла
                var unloadStartTime = fields[7];

                //Время окончания выгрузки файла
                var unloadEndTime = fields[8];

                //№ файла выгрузки
                var fileNumber = fields[9];

                //Месяц и год начислений
                infoDescript.CalculationDate = Convert.ToDateTime(fields[10]);

                //Количество оплат
                infoDescript.PaymentsCount = Convert.ToInt32(fields[11]);

                //наименование префикса центральной схемы
                infoDescript.CentralSchemaPrefix = fields[12];

                //Уникальный код загружаемого файла
                infoDescript.LoadId = loadedFile.Id;


                _importLog.AppendFormat("Версия формата: {0}{1}", formatVersion, Environment.NewLine);
                _importLog.AppendFormat("Код расчетного центра: {0}{1}", infoDescript.ErcCode, Environment.NewLine);
                _importLog.AppendFormat("Наименование организации-отправителя: '{0}'{1}", senderOrganizationName,
                    Environment.NewLine);
                _importLog.AppendFormat("Наименование района: '{0}'{1}", rajonName, Environment.NewLine);
                _importLog.AppendFormat("ИНН: '{0}', КПП: '{1}'{2}", senderInn, senderKpp, Environment.NewLine);
                _importLog.AppendFormat("Месяц и год начислений:{0}{1}",
                    infoDescript.CalculationDate.ToShortDateString(),
                    Environment.NewLine);
                _importLog.AppendLine();

                //проверяем формат выгрузки
                if (Convert.ToDecimal(formatVersion.Replace('.', ',')) < 1.04m)
                {
                    throw new Exception(Environment.NewLine +
                                        " Архив не прошел валидацию! Неактуальная версия формата загрузки! " +
                                        " Необходимо обновить программное обеспечение и повторно выгрузить файл.");
                }

                //считываем контрольные показатели
                //каждая строка - один локальный банк
                var verificationInfo = textReader.ReadToEnd().Replace('.', ',')
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                //считываем контрольные показатели
                foreach (var oneBankInfo in verificationInfo.Select(line => line.Split('|')))
                {
                    if (oneBankInfo.Count() != 15)
                    {
                        _importLog.AppendLine(String.Format(
                            "Архив не прошел валидацию! Некорректно заполнено информационное описание по банку '{0}'! " +
                            " Данные по этому банку не будут загружены!", oneBankInfo[2]));
                        continue;
                    }

                    infoDescript.BankInfo.Add(new VerificationInfo
                    {
                        CalculationDate = infoDescript.CalculationDate,
                        SchemaIdentifier = oneBankInfo[1],
                        SchemaPrefix = oneBankInfo[2],
                        AccountsCount = Convert.ToInt32(oneBankInfo[3]),
                        HousesCount = Convert.ToInt32(oneBankInfo[4]),
                        ServicesCount = Convert.ToInt32(oneBankInfo[5]),
                        CountersCount = Convert.ToInt32(oneBankInfo[6]),
                        NondeliveryCount = Convert.ToInt32(oneBankInfo[7]),
                        AccountsTotalSquare = Convert.ToDecimal(oneBankInfo[8]),
                        HousesTotalSquare = Convert.ToDecimal(oneBankInfo[9]),
                        Insaldo = Convert.ToDecimal(oneBankInfo[10]),
                        Accrued = Convert.ToDecimal(oneBankInfo[11]),
                        Paid = Convert.ToDecimal(oneBankInfo[12]),
                        OutSaldo = Convert.ToDecimal(oneBankInfo[13])
                    }
                        );
                }

                //проставляем загружаемый расчетный месяц
                loadedFile.CalculationDate = infoDescript.CalculationDate;
                Container.Resolve<IDomainService<LoadedFileRegister>>().Update(loadedFile);
            }
            catch (Exception ex)
            {
                throw new Exception("Архив не прошел валидацию! Некорректно заполнено информационное описание! " +
                                    ex.Message);
            }
        }

        /// <summary>
        /// Файлы по одному банку данных
        /// </summary>
        /// <param name="oneBankArchive"></param>
        /// <param name="schemaPrefix"></param>
        /// <param name="loadId"></param>
        /// <param name="sqlExecutor"></param>
        /// <param name="loadedTables"></param>
        private void LoadData(ZipFile oneBankArchive, string schemaPrefix, SqlExecutor.SqlExecutor sqlExecutor,
            long loadId, out List<string> loadedTables)
        {
            loadedTables = new List<string>();

            //считываем файл "_main.txt", который содержит в себе информацию о локальном банке
            //пока из этой всей информации берем только тип банка данных (data, kernel, charge_YY, fin_YY (где YY - год))
            var bankType = "";
            var mainFile = oneBankArchive.FirstOrDefault(x => x.FileName.ToLower().Contains("_main.txt"));
            if (mainFile != null)
            {
                using (var mainFileMs = new MemoryStream())
                {
                    mainFile.Extract(mainFileMs);
                    mainFileMs.Seek(0, SeekOrigin.Begin);

                    try
                    {
                        //варианты, которые могут быть: data, kernel, charge_YY, fin_YY (где YY - год)
                        bankType = new StreamReader(mainFileMs, Encoding.GetEncoding(1251)).ReadLine().Split('|')[7];
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(
                            String.Format(
                                "Архив не прошел валидацию! Неверные параметры файла _main.txt в файле '{0}_{1}'! Текст ошибки: {2}",
                                schemaPrefix.Trim(), bankType, ex));
                    }
                }
            }
            else
            {
                throw new Exception(
                           String.Format(
                               "Архив не прошел валидацию! Не найден файл '_main.txt' в банке '{0}'!",
                               schemaPrefix.Trim()));
            }

            //считываем файл "opis.csv", который содержит в себе информацию о структуре таблиц
            var opisFile = oneBankArchive.FirstOrDefault(x => x.FileName.ToLower().Contains("opis.csv"));
            if (opisFile != null)
            {
                using (var opisMs = new MemoryStream())
                {
                    opisFile.Extract(opisMs);
                    opisMs.Seek(0, SeekOrigin.Begin);

                    try
                    {
                        var schemaName = String.Format("{0}_{1}", schemaPrefix.Trim(), bankType);
                        CreateTablesForLoad(new StreamReader(opisMs, Encoding.GetEncoding(1251)),
                            sqlExecutor, schemaName, loadId, out loadedTables);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(
                            String.Format(
                                "Архив не прошел валидацию! Неверные параметры файла opis.csv в файле '{0}_{1}'! {2} Текст ошибки: {3}",
                                schemaPrefix.Trim(),
                                bankType,
                                Environment.NewLine,
                                ex));
                    }
                }
            }
            else
            {
                throw new Exception(
                           String.Format(
                               "Архив не прошел валидацию! Не найден файл 'opis.csv' в файле '{0}_{1}'!",
                               schemaPrefix.Trim(), bankType));
            }

            //считываем файлы с данными (файлы с расширением ".unl" в кодировке 'ISO-8859-5')
            foreach (var data in oneBankArchive.Where(x => x.FileName.ToLower().Contains(".unl")))
            {
                using (var dataMs = (Stream)data.OpenReader())
                {
                    //собираем названия таблицы для загрузки
                    //пример: n1_charge_15.charge_03_XXXXXX, где XXXXX - уникальный код загрузки
                    //таблица создана ранее в методе CreateTablesForLoad();
                    var loadedTableName = String.Format("{0}_{1}.{2}_{3}",
                        schemaPrefix.Trim(),
                        bankType,
                        Path.GetFileNameWithoutExtension(data.FileName),
                        loadId);


                    try
                    {
                        var sqlQuery = String.Format(
                            " COPY {0} " +
                            " FROM stdin " +
                            " WITH DELIMITER AS '|' " +
                            " NULL AS ''",
                            loadedTableName);

                        sqlExecutor.CopyIn(sqlQuery, dataMs, Encoding.GetEncoding(28595));
                    }
                    catch (PostgresException ex)
                    {
                        throw new Exception(String.Format(
                            "Архив не прошел валидацию! Ошибка при загрузке данных из файла '{0}' в таблицу '{1}': {2} {3}Детали ошибки: {4} ",
                            Path.GetFileNameWithoutExtension(data.FileName),
                            loadedTableName,
                            ex.Message,
                            Environment.NewLine,
                            ex.Where
                            ));
                    }
                }
            }
        }

        /// <summary>
        /// Создание буфферных таблиц, в которые будут перенесены данные из файла
        /// </summary>
        /// <param name="textReader"></param>
        /// <param name="sqlExecutor"></param>
        /// <param name="schemaName"></param>
        /// <param name="loadId"></param>
        /// <param name="loadedTables"></param>
        private void CreateTablesForLoad(TextReader textReader, SqlExecutor.SqlExecutor sqlExecutor, string schemaName,
            long loadId, out List<string> loadedTables)
        {
            loadedTables = new List<string>();
            //считываем описание структур таблиц
            //каждая строка - одна колонка таблицы
            var structure = textReader.ReadToEnd()
                .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);


            var tables = structure
                .Select(line => line.Split('|'))
                .Select(fields => new TableStructure
                {
                    TableName = fields[0],
                    OrdinalPosition = Convert.ToInt32(fields[1]),
                    ColumnName = fields[2],
                    ColumnDataType = fields[3],
                    Scale = fields[4],
                    Nullable = fields[5]
                })
                .GroupBy(x => x.TableName)
                .ToDictionary(x => x.Key, x => x.Select(y => new
                {
                    y.TableName,
                    y.OrdinalPosition,
                    y.ColumnName,
                    y.ColumnDataType,
                    y.Scale,
                    y.Nullable
                }));

            foreach (var tableName in tables.Keys)
            {
                var tableFullName = String.Format("{0}.{1}_{2}", schemaName, tableName, loadId);
                var sqlQuery = String.Format(" CREATE TABLE {0}();", tableFullName);
                sqlExecutor.ExecuteSql(sqlQuery);

                loadedTables.Add(tableFullName);

                foreach (var item in tables[tableName].OrderBy(x => x.OrdinalPosition))
                {
                    string columnDefaultValue;
                    string nullable;
                    if (item.Nullable == string.Empty)
                    {
                        nullable = "";
                        columnDefaultValue = string.Empty;
                    }
                    else
                    {
                        nullable = " NOT NULL ";
                        switch (item.ColumnDataType.Trim().ToLower())
                        {
                            case "nchar":
                            case "char":
                                columnDefaultValue = " default '-' ";
                                break;
                            case "serial":
                                columnDefaultValue = " ";
                                break;
                            case "date":
                            case "datetime":
                                columnDefaultValue = " default NOW() ";
                                break;
                            default:
                                columnDefaultValue = " default 0 ";
                                break;
                        }
                    }

                    string columnType;
                    switch (item.ColumnDataType.Trim().ToLower())
                    {
                        case "nchar":
                        case "char":
                        case "float":
                            columnType = item.ColumnDataType + " ( " + item.Scale + " ) ";
                            break;
                        case "decimal":
                            columnType = " NUMERIC ";
                            break;
                        case "datetime":
                            columnType = " TIMESTAMP WITHOUT TIME ZONE ";
                            break;
                        case "byte":
                            columnType = " varchar ";
                            break;
                        default:
                            columnType = item.ColumnDataType.Trim().ToLower();
                            break;
                    }

                    //проверка для типов YEAR to SECOND, DAY to SECOND
                    if (item.ColumnDataType.Trim().ToLower() == "datetime" &&
                        item.Scale.Trim().ToLower() == "day to second")
                    {
                        columnType = " interval ";
                        columnDefaultValue = item.Scale;
                    }
                    if (item.ColumnDataType.Trim().ToLower() == "datetime" &&
                        item.Scale.Trim().ToLower() == "year to second")
                    {
                        columnType = " TIMESTAMP WITHOUT TIME ZONE ";
                        columnDefaultValue = "";
                    }


                    //Добавление колонки
                    sqlQuery = String.Format(
                        "ALTER TABLE {0} ADD COLUMN \"{1}\" {2} {3} {4}",
                        tableFullName,
                        item.ColumnName,
                        columnType,
                        nullable,
                        columnDefaultValue);
                    sqlExecutor.ExecuteSql(sqlQuery);
                }

                //Добавление лишней колонки
                //костылек такой
                sqlQuery = String.Format(
                    "ALTER TABLE {0} ADD COLUMN additional_column VARCHAR",
                    tableFullName);
                sqlExecutor.ExecuteSql(sqlQuery);
            }
        }

        /// <summary>
        /// Проверка целостности данных
        /// </summary>
        /// <param name="sqlExecutor"></param>
        /// <param name="verificationInfo"></param>
        /// <param name="schemaPrefix"></param>
        /// <param name="loadId"></param>
        /// <returns></returns>
        private bool VerificateData(SqlExecutor.SqlExecutor sqlExecutor, VerificationInfo verificationInfo,
            string schemaPrefix, long loadId)
        {
            var result = true;

            //проверяем количество лицевых счетов
            var sqlQuery = String.Format("SELECT COUNT(*) AS count FROM {0}_data.kvar_{1}", schemaPrefix, loadId);
            var countInDb = sqlExecutor.ExecuteScalar<decimal>(sqlQuery);
            if (verificationInfo.AccountsCount != countInDb)
            {
                _importLog.AppendLine(String.Format(
                    "Обнаружено нарушение целостности данных! Загрузка не будет произведена! Не совпадает количество лицевых счетов! {0} вместо {1}",
                    countInDb,
                    verificationInfo.AccountsCount));
                result = false;
            }

            //проверяем количество домов
            sqlQuery = String.Format("SELECT COUNT(*) AS count FROM {0}_data.dom_{1}", schemaPrefix, loadId);
            countInDb = sqlExecutor.ExecuteScalar<decimal>(sqlQuery);
            if (verificationInfo.HousesCount != countInDb)
            {
                _importLog.AppendLine(String.Format(
                    "Обнаружено нарушение целостности данных! Загрузка не будет произведена! Не совпадает количество домов! {0} вместо {1}",
                    countInDb,
                    verificationInfo.HousesCount));
                result = false;
            }


            //проверяем количество услуг
            sqlQuery = String.Format("SELECT COUNT(*) AS count FROM {0}_data.tarif_{1}", schemaPrefix, loadId);
            countInDb = sqlExecutor.ExecuteScalar<decimal>(sqlQuery);
            if (verificationInfo.ServicesCount != countInDb)
            {
                _importLog.AppendLine(String.Format(
                    "Обнаружено нарушение целостности данных! Загрузка не будет произведена! Не совпадает количество услуг! {0} вместо {1}",
                    countInDb,
                    verificationInfo.ServicesCount));
                result = false;
            }

            //проверяем количество ПУ
            sqlQuery = String.Format("SELECT COUNT(*) AS count FROM {0}_data.counters_spis_{1}", schemaPrefix, loadId);
            countInDb = sqlExecutor.ExecuteScalar<decimal>(sqlQuery);
            if (verificationInfo.CountersCount != countInDb)
            {
                _importLog.AppendLine(String.Format(
                    "Обнаружено нарушение целостности данных! Загрузка не будет произведена! Не совпадает количество приборов учета! {0} вместо {1}",
                    countInDb,
                    verificationInfo.CountersCount));
                result = false;
            }

            //проверяем количество недопоставок
            sqlQuery = String.Format("SELECT COUNT(*) AS count FROM {0}_data.nedop_kvar_{1}", schemaPrefix, loadId);
            countInDb = sqlExecutor.ExecuteScalar<decimal>(sqlQuery);
            if (verificationInfo.NondeliveryCount != countInDb)
            {
                _importLog.AppendLine(String.Format(
                    "Обнаружено нарушение целостности данных! Загрузка не будет произведена! Не совпадает количество недопоставок! {0} вместо {1}",
                    countInDb,
                    verificationInfo.NondeliveryCount));
                result = false;
            }

            //проверяем показатель общая площадь лицевых счетов
            sqlQuery =
                String.Format(
                    "SELECT SUM(replace(val_prm,',','.')::numeric)::numeric(14,2) AS total FROM {0}_data.prm_1_{1} WHERE nzp_prm = 4 ",
                    schemaPrefix, loadId);
            countInDb = sqlExecutor.ExecuteScalar<decimal>(sqlQuery);
            if (verificationInfo.AccountsTotalSquare != countInDb)
            {
                _importLog.AppendLine(String.Format(
                    "Обнаружено нарушение целостности данных! Загрузка не будет произведена! Не совпадает общая площадь лицевых счетов! {0} вместо {1}",
                    countInDb,
                    verificationInfo.AccountsTotalSquare));
                result = false;
            }


            //проверяем показатель общая площадь домов
            sqlQuery =
                String.Format(
                    "SELECT SUM(replace(val_prm,',','.')::numeric)::numeric(14,2) AS total FROM {0}_data.prm_2_{1} WHERE nzp_prm = 40 ",
                    schemaPrefix, loadId);
            countInDb = sqlExecutor.ExecuteScalar<decimal>(sqlQuery);
            if (verificationInfo.HousesTotalSquare != countInDb)
            {
                _importLog.AppendLine(String.Format(
                    "Обнаружено нарушение целостности данных! Загрузка не будет произведена! Не совпадает общая площадь домов! {0} вместо {1}",
                    countInDb,
                    verificationInfo.HousesTotalSquare));
                result = false;
            }

            //проверяем показатель входящее сальдо
            sqlQuery =
                String.Format("SELECT SUM(sum_insaldo) AS total FROM {0}_charge_{1}.charge_{2}_{3}",
                    schemaPrefix,
                    verificationInfo.CalculationDate.ToString("yy"),
                    verificationInfo.CalculationDate.ToString("MM"),
                    loadId);
            countInDb = sqlExecutor.ExecuteScalar<decimal>(sqlQuery);
            if (verificationInfo.Insaldo != countInDb)
            {
                _importLog.AppendLine(String.Format(
                    "Обнаружено нарушение целостности данных! Загрузка не будет произведена! Не совпадает входящее сальдо! {0} вместо {1}",
                    countInDb,
                    verificationInfo.Insaldo));
                result = false;
            }

            //проверяем показатель исходящее сальдо
            sqlQuery =
                String.Format(
                    "SELECT SUM(sum_outsaldo) AS total FROM {0}_charge_{1}.charge_{2}_{3}",
                    schemaPrefix,
                    verificationInfo.CalculationDate.ToString("yy"),
                    verificationInfo.CalculationDate.ToString("MM"),
                    loadId);
            countInDb = sqlExecutor.ExecuteScalar<decimal>(sqlQuery);
            if (verificationInfo.OutSaldo != countInDb)
            {
                _importLog.AppendLine(String.Format(
                    "Обнаружено нарушение целостности данных! Загрузка не будет произведена! Не совпадает исходящее сальдо! {0} вместо {1}",
                    countInDb,
                    verificationInfo.OutSaldo));
                result = false;
            }

            //проверяем показатель начислено
            sqlQuery =
                String.Format("SELECT SUM(sum_real) AS total FROM {0}_charge_{1}.charge_{2}_{3}",
                    schemaPrefix,
                    verificationInfo.CalculationDate.ToString("yy"),
                    verificationInfo.CalculationDate.ToString("MM"),
                    loadId);
            countInDb = sqlExecutor.ExecuteScalar<decimal>(sqlQuery);
            if (verificationInfo.Accrued != countInDb)
            {
                _importLog.AppendLine(String.Format(
                    "Обнаружено нарушение целостности данных! Загрузка не будет произведена! Не совпадает начисленная сумма! {0} вместо {1}",
                    countInDb,
                    verificationInfo.Accrued));
                result = false;
            }

            //проверяем показатель оплачено
            sqlQuery =
                String.Format("SELECT SUM(sum_money) AS total FROM {0}_charge_{1}.charge_{2}_{3}",
                    schemaPrefix,
                    verificationInfo.CalculationDate.ToString("yy"),
                    verificationInfo.CalculationDate.ToString("MM"),
                    loadId);
            countInDb = sqlExecutor.ExecuteScalar<decimal>(sqlQuery);
            if (verificationInfo.Paid != countInDb)
            {
                _importLog.AppendLine(String.Format(
                    "Обнаружено нарушение целостности данных! Загрузка не будет произведена! Не совпадает оплаченная сумма! {0} вместо {1}",
                    countInDb,
                    verificationInfo.Paid));
                result = false;
            }

            return result;
        }


        /// <summary>
        /// Откат загруженных данных
        /// </summary>
        /// <param name="sqlExecutor"></param>
        /// <param name="loadedTables"></param>
        private void RoolBack(SqlExecutor.SqlExecutor sqlExecutor, IEnumerable<string> loadedTables)
        {
            loadedTables.Where(x => !x.Contains("recalc_"))
                .ForEach(x => sqlExecutor.ExecuteSql("DROP TABLE IF EXISTS " + x));
        }

        /// <summary>
        /// Перенос данных в основные таблицы
        /// </summary>
        private void TransferDataToMainTables(SqlExecutor.SqlExecutor sqlExecutor, IEnumerable<string> loadedTables,
            long loadId)
        {
            var transaction = sqlExecutor.BeginTransaction();

            foreach (var tableName in loadedTables)
            {
                if (tableName.Contains("recalc_"))
                    continue;

                var oldTableName = tableName.Replace("_" + loadId, "");
                //удаляем внешние ключи, ссылающиеся на удаляемую таблицу
                var sqlQuery = String.Format(
                    " SELECT 'ALTER TABLE '||table_schema||'.'||table_name||' DROP CONSTRAINT IF EXISTS '||constraint_name||' CASCADE;' " +
                    " FROM information_schema.constraint_table_usage " +
                    " WHERE table_schema||'.'||table_name ILIKE '{0}'", oldTableName.Trim());

                //генерируем запросы на удаление внешних ключей и сразу удаляем
                sqlExecutor.ExecuteSql<string>(sqlQuery).ForEach(x => sqlExecutor.ExecuteSql(x, transaction));


                sqlQuery = String.Format(" DROP TABLE IF EXISTS {0}; ALTER TABLE {1} RENAME TO {2}; ", oldTableName,
                    tableName,
                    oldTableName.Split('.').Last());
                sqlExecutor.ExecuteSql(sqlQuery, transaction);
            }
            transaction.Commit();
        }

        /// <summary>
        /// Вспомогательная сущность для загрузки
        /// </summary>
        private class InfoDescript
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public InfoDescript()
            {
                BankInfo = new List<VerificationInfo>();
            }

            /// <summary>
            /// Уникальный код загружаемого файла
            /// </summary>
            public long LoadId { get; set; }

            /// <summary>
            /// Наименование префикса центральной схемы
            /// </summary>
            public string CentralSchemaPrefix { get; set; }

            /// <summary>
            /// Код ЕРЦ
            /// </summary>
            public long ErcCode { get; set; }

            /// <summary>
            /// Расчетный месяц за который грузятся данные
            /// </summary>
            public DateTime CalculationDate { get; set; }

            /// <summary>
            /// Информация о банках данных
            /// </summary>
            public List<VerificationInfo> BankInfo { get; set; }

            /// <summary>
            /// Количество оплат
            /// </summary>
            public int PaymentsCount { get; set; }
        }

        /// <summary>
        /// Вспомогательная сущность для контроля корректности загрузки
        /// </summary>
        private class VerificationInfo
        {
            /// <summary>
            /// Префикс банка данных
            /// </summary>
            public string SchemaPrefix { get; set; }

            /// <summary>
            /// Идентификатор банка данных
            /// </summary>
            public string SchemaIdentifier { get; set; }

            /// <summary>
            /// Расчетный месяц за который грузятся данные
            /// </summary>
            public DateTime CalculationDate { get; set; }

            /// <summary>
            /// Количество лицевых счетов
            /// </summary>
            public int AccountsCount { get; set; }


            /// <summary>
            /// Количество домов
            /// </summary>
            public int HousesCount { get; set; }

            /// <summary>
            /// Количество услуг
            /// </summary>
            public int ServicesCount { get; set; }

            /// <summary>
            /// Количество приборов учета
            /// </summary>
            public int CountersCount { get; set; }

            /// <summary>
            /// Количество недопоставок
            /// </summary>
            public int NondeliveryCount { get; set; }

            /// <summary>
            /// Общая площадь лицевых счетов
            /// </summary>
            public decimal AccountsTotalSquare { get; set; }

            /// <summary>
            /// Общая площадь домов
            /// </summary>
            public decimal HousesTotalSquare { get; set; }

            /// <summary>
            /// Входящее вальдо
            /// </summary>
            public decimal Insaldo { get; set; }

            /// <summary>
            /// Исходящее сальдо
            /// </summary>
            public decimal OutSaldo { get; set; }

            /// <summary>
            /// Начислено
            /// </summary>
            public decimal Accrued { get; set; }

            /// <summary>
            /// Оплачено
            /// </summary>
            public decimal Paid { get; set; }

        }

        /// <summary>
        /// Вспомогательная сущность для создания таблиц
        /// </summary>
        private class TableStructure
        {
            /// <summary>
            /// Наименование таблицы
            /// </summary>
            public string TableName { get; set; }

            /// <summary>
            /// Порядковый номер таблицы
            /// </summary>
            public int OrdinalPosition { get; set; }

            /// <summary>
            /// Наименование колонки
            /// </summary>
            public string ColumnName { get; set; }

            /// <summary>
            /// Наименование типа колонки
            /// </summary>
            public string ColumnDataType { get; set; }

            /// <summary>
            /// Размерность колонки
            /// </summary>
            public string Scale { get; set; }

            /// <summary>
            /// Может ли принимать значение NULL
            /// </summary>
            public string Nullable { get; set; }
        }
    }
}
