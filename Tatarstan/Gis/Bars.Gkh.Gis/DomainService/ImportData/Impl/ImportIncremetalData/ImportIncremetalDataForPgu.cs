namespace Bars.Gkh.Gis.DomainService.ImportData.Impl.ImportIncremetalData
{
    using B4;
    using B4.Modules.FileStorage;
    using Bars.B4.Config;
    using Bars.Gkh.Gis.DomainService.ImportData.Impl.ImportIncremetalData.ForPgu;
    using BilConnection;
    using DataResult;
    using Entities.Register.LoadedFileRegister;
    using Enum;
    using Ionic.Zip;
    using Npgsql;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Bars.B4.DataAccess;

    using Utils;

    /// <summary>
    /// Загрузка инкрементальных данных для ПГМУ РТ
    /// </summary>
    public class ImportIncremetalDataForPgu : BaseImportDataHandler
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public ImportIncremetalDataForPgu(IConfigProvider configProvider)
        {
            this.importLog = new StringBuilder();
            this.appConfig = configProvider.GetConfig();
        }

        private readonly StringBuilder importLog;
        private readonly AppConfig appConfig;
        private TypeStatus loadStatus;

        // Загружаемые секции
        private List<FileSection> sections = new List<FileSection>();

        /// <summary>
        /// Файловый менеджер
        /// </summary>
        public IFileManager FileManager { get; set; }

        /// <summary>
        /// Файловый менеджер
        /// </summary>
        public IBilConnectionService BilConnectionService { get; set; }

        /// <summary>
        /// Менеджер настроек БД
        /// </summary>
        public IDbConfigProvider DbConfigProvider { get; set; }

        /// <inheritdoc />
        public override IEnumerable<ImportDataResult> ImportData(IEnumerable<LoadedFileRegister> loadedFiles)
        {
            return loadedFiles.Select(this.ImportData);
        }

        /// <inheritdoc />
        public override ImportDataResult ImportData(LoadedFileRegister loadedFile)
        {
            this.loadStatus = TypeStatus.InProgress;
            this.importLog.Clear();
            this.importLog.AppendLine("Дата загрузки: " + DateTime.Now);
            this.importLog.AppendLine("Тип загрузки: 'Загрузка инкрементальных данных в ПГМУ РТ'");
            this.importLog.AppendLine("Наименование файла: " + loadedFile.File.FullName);
            var infoDescript = new InfoDescript();

            try
            {
                // Проверяем расширение файла
                if (!loadedFile.File.Extention.ToLower().Contains("zip"))
                    throw new Exception("Архив не прошел проверку! Архив должен быть в формате zip");

                // Обрабатываем архив
                using (var zipFile = ZipFile.Read(this.FileManager.GetFile(loadedFile.File)))
                {
                    // Файлы в архиве
                    var zipEntries = zipFile.Where(x => x.FileName.EndsWith(".txt")).ToArray();

                    using (var sqlExecutor = new SqlExecutor.SqlExecutor(this.BilConnectionService.GetConnection(ConnectionType.GisConnStringPgu)))
                    {
                        // Обрабатываем "Файл информационного описания"
                        var entry = this.GetFileSection(zipEntries, PguFileSection.InfoDescript);

                        using (var ms = new MemoryStream())
                        {
                            entry.Extract(ms);
                            ms.Seek(0, SeekOrigin.Begin);
                            this.AnalyzeInfoDescriptFile(
                                new StreamReader(ms, Encoding.GetEncoding(1251)),
                                sqlExecutor,
                                loadedFile,
                                out infoDescript);
                        }

                        this.sections = FormatSections.GetSections(infoDescript.FormatVersion);

                        if (zipEntries.Length != this.sections.Count)
                        {
                            throw new Exception(
                                $"Архив не прошел проверку! В архиве версии {infoDescript.FormatVersion} должно быть {this.sections.Count} файлов!" +
                                $" Кол-во файлов в архиве: {zipEntries.Length}");
                        }

                        // Обрабатываем все секции в файле
                        foreach (var section in this.sections.Where(section => section.PguFileSection != PguFileSection.InfoDescript))
                        {
                            entry = this.GetFileSection(zipEntries, section.PguFileSection);
                            if (entry != null)
                            {
                                using (var stream = (Stream)entry.OpenReader())
                                {
                                    this.ImportFile(stream, sqlExecutor, infoDescript, section);
                                }
                            }
                        }

                        try
                        {
                            //создаем партицию для счетчиков в схеме веб-сервисов
                            //веб-сервисы в нее будут класть данные
                            var sqlQuery = $@"
                                CREATE TABLE IF NOT EXISTS webfon.counters_ord_{infoDescript.CalculationDate:yyyyMM}
                                (LIKE {"webfon.counters_ord"}  INCLUDING ALL, 
                                    CHECK (dat_month = '{infoDescript.CalculationDate.ToShortDateString()}') 
                                ) 
                                INHERITS({"webfon.counters_ord"}) 
                                WITH (OIDS=TRUE)";
                            sqlExecutor.ExecuteSql(sqlQuery);

                            // Смотрим, является ли загружаемый месяц более новым
                            sqlQuery = $@" 
                                SELECT COUNT(*) AS count 
                                FROM public.saldo_date 
                                WHERE erc_code = {infoDescript.ErcCode}
                                AND saldo_date < '{infoDescript.CalculationDate.ToShortDateString()}'::DATE
                                AND active = 1 ";
                            if (sqlExecutor.ExecuteScalar<int>(sqlQuery) > 0)
                            {
                                if (!this.appConfig.AppSettings.GetAs<bool?>("NotSendNotificationPgmu") ?? true)
                                {
                                    // Отправляем уведомление на ПГМУ РТ о появлении новых начислений
                                    new NotificationPgmu().SendNotification(
                                        infoDescript.ErcCode,
                                        infoDescript.ErcName.Replace('"', '\''),
                                        infoDescript.CalculationDate);
                                    this.importLog.AppendLine("Уведомление отправлено.");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            this.importLog.AppendLine($"Уведомление не отправлено. Текст ошибки:{ex.Message}");
                        }

                        try
                        {
                            // Подсчитываем кол-во ЛС и вставляем в таблицу
                            var sqlQuery =
                                $@"WITH accounts_states AS (
                                    SELECT 
                                    SUM(CASE WHEN account_state = 1 THEN 1 else 0 END) AS opened_accounts_count,
                                    SUM(CASE WHEN account_state = 2 THEN 1 else 0 END) AS closed_accounts_count,
                                    SUM(CASE WHEN account_state = 3 THEN 1 else 0 END) AS undefined_accounts_count,
                                    COUNT(*) AS total_count
                                    FROM parameters_{infoDescript.ErcCode}_{infoDescript.CalculationDate:yyyyMM})
                                    UPDATE erc SET 
                                    accounts_count = s.total_count,
                                    opened_accounts_count = s.opened_accounts_count,
                                    closed_accounts_count = s.closed_accounts_count,
                                    undefined_accounts_count = s.undefined_accounts_count 
                                    FROM accounts_states s
                                    WHERE erc_code = {infoDescript.ErcCode};";

                            sqlExecutor.ExecuteSql(sqlQuery);
                            this.importLog.AppendLine("Кол-во ЛС подсчитано и вставлено в таблицу.");
                        }
                        catch (Exception ex)
                        {
                            this.importLog.AppendLine($"Ошибка при подсчете кол-ва ЛС! {ex.Message}");
                        }

                        // Добавление записи в saldo_date
                        this.InsertIntoSaldoDate(sqlExecutor, infoDescript.CalculationDate, infoDescript.ErcCode);
                    }

                    this.ImportAddressesToMgf(zipEntries);
                }

                this.loadStatus = TypeStatus.Done;
                this.importLog.AppendLine("Данные успешно загружены!");
            }
            catch (PostgresException ex)
            {
                // Пишем в протокол загрузки
                var error = string.Format(
                    "Ошибка при загрузке! Архив не прошел проверку! {0}Текст ошибки:'{1}'.{0}Местоположение некорректных данных:'{2}'",
                    Environment.NewLine,
                    ex.Message,
                    ex.Where);

                this.importLog.AppendLine(error);
                this.loadStatus = TypeStatus.ProcessingError;

                // В случае ошибки удаляем все таблицы по этому ЕРЦ по загружаемому месяцу
                return this.DropTables(infoDescript, loadedFile.Id);
            }
            catch (PguVersionException ex)
            {
                this.importLog.AppendLine("Ошибка при загрузке: " + ex.Message);
                this.loadStatus = TypeStatus.Error;

                // В случае ошибки удаляем все таблицы по этому ЕРЦ по загружаемому месяцу
                return this.DropTables(infoDescript, loadedFile.Id);
            }
            catch (Exception ex)
            {
                this.importLog.AppendLine("Ошибка при загрузке: " + ex.Message);
                this.loadStatus = TypeStatus.ProcessingError;

                // В случае ошибки удаляем все таблицы по этому ЕРЦ по загружаемому месяцу
                return this.DropTables(infoDescript, loadedFile.Id);
            }
            finally
            {
                this.importLog.AppendLine(DateTime.Now.ToString(CultureInfo.InvariantCulture));

                // Сохраняем в файл протокол-загрузки
                var logFileName = $"ПРОТОКОЛ_ЗАГРУЗКИ_{loadedFile.File.Name}_{loadedFile.Id}";
                var resultString = this.importLog.ToString();
                var bytes = Encoding.GetEncoding(1251).GetBytes(resultString);

                // Сохраняем лог-файл
                loadedFile.Log = this.Container.Resolve<IFileManager>().SaveFile(logFileName, "txt", bytes);
                loadedFile.TypeStatus = this.loadStatus;
                this.Container.Resolve<IDomainService<LoadedFileRegister>>().Update(loadedFile);

                using (var sqlExecutor = new SqlExecutor.SqlExecutor(this.BilConnectionService.GetConnection(ConnectionType.GisConnStringPgu)))
                {
                    // Ставим статус загрузки 
                    sqlExecutor.ExecuteSql($"UPDATE public.sys_imports_register SET import_result = {(int)this.loadStatus} WHERE id = {infoDescript.LoadId} ");
                }
            }

            return new ImportDataResult(true, "Успешно загружено!", loadedFile.Id);
        }

        /// <summary>
        /// Получение файла секции из списка
        /// </summary>
        /// <param name="zipEntries">Список файлов в архиве</param>
        /// <param name="section">Искомая секция</param>
        /// <returns>Файл архива</returns>
        private ZipEntry GetFileSection(ZipEntry[] zipEntries, PguFileSection section)
        {
            var sectionName = Enum.GetName(typeof(PguFileSection), section);
            var entry = zipEntries.FirstOrDefault(x => x.FileName.Split('.').First() == sectionName);

            if (entry == null && section == PguFileSection.InfoDescript)
            {
                throw new Exception("Архив не прошел проверку! Не найден заголовочный файл!");
            }

            return entry;
        }

        /// <summary>
        /// Обрабатываем "Файл информационного описания"
        /// InfoDescript.txt
        /// </summary>
        /// <param name="streamReader"></param>
        /// <param name="sqlExecutor"></param>
        /// <param name="loadedFile"></param>
        /// <param name="infoDescript"></param>
        private void AnalyzeInfoDescriptFile(TextReader streamReader, SqlExecutor.SqlExecutor sqlExecutor, LoadedFileRegister loadedFile, out InfoDescript infoDescript)
        {
            infoDescript = new InfoDescript();

            var line = streamReader.ReadLine();
            if (line == null)
            {
                throw new Exception("Архив не прошел проверку! Не заполнено информационное описание!");
            }

            //разделитель полей 
            var fields = line.Split('|');

            if (fields.Length != 13)
            {
                throw new Exception("Архив не прошел проверку! Информационное описание не соответствует формату!");
            }

            //Считываем версию формата
            var formatVersion = fields[0];

            //Наименование организации-отправителя 
            infoDescript.ErcName = fields[1].Trim();

            //Подразделение организации-отправителя 
            var senderOrganizationUnitName = fields[2];

            //ИНН организации-отправителя
            var senderInn = fields[3];

            //КПП организации-отправителя
            var senderKpp = fields[4];

            //Код расчетного центра
            infoDescript.ErcCode = Convert.ToInt64(fields[5]);

            //№ файла
            var fileNumber = fields[6];

            //Дата файла
            var unloadDate = fields[7];

            //Телефон отправителя
            var senderPhone = fields[8];

            //Ф.И.О. отправителя
            var senderFio = fields[9];

            //Месяц и год начислений
            infoDescript.CalculationDate = Convert.ToDateTime(fields[10]);

            //Количество выгруженных лицевых счетов
            var unloadedAccountsCount = fields[11];

            //Дата начала работы системы
            var systemStartDate = fields[12];

            this.importLog.AppendFormat("Версия формата: {0}{1}", formatVersion, Environment.NewLine);
            this.importLog.AppendFormat("Код расчетного центра: {0}{1}", infoDescript.ErcCode, Environment.NewLine);
            this.importLog.AppendFormat("Наименование организации-отправителя: '{0}'{1}", infoDescript.ErcName, Environment.NewLine);
            this.importLog.AppendFormat("ИНН: '{0}', КПП: '{1}'{2}", senderInn, senderKpp, Environment.NewLine);
            this.importLog.AppendFormat("Месяц и год начислений: {0}{1}", infoDescript.CalculationDate.ToShortDateString(), Environment.NewLine);
            this.importLog.AppendLine();

            infoDescript.FormatVersion = formatVersion;

            //проверяем код ЕРЦ на наличие в справочнике
            var sqlQuery = "SELECT COUNT(*) AS count FROM public.erc WHERE erc_code = " + infoDescript.ErcCode;
            if (sqlExecutor.ExecuteScalar<int>(sqlQuery) == 0)
            {
                throw new Exception(" Архив не прошел проверку! Не найден соответствующий код ЕРЦ. Код ЕРЦ из файла: " + infoDescript.ErcCode);
            }

            sqlQuery = $@"
                INSERT INTO public.sys_imports_register( 
                calculation_date, 
                load_date, 
                unload_date, 
                format_version, 
                filename, 
                file_number, 
                unloaded_accounts_count, 
                gis_loaded_file_register_id, 
                import_result)
                VALUES ('{infoDescript.CalculationDate}', '{DateTime.Now}', '{unloadDate}', 
                        '{formatVersion}', '{loadedFile.File.Name}', {fileNumber}, {unloadedAccountsCount}, 
                        {loadedFile.Id}, {(int)TypeStatus.InProgress})
                RETURNING id ";

            //уникальный код загрузки
            infoDescript.LoadId = sqlExecutor.ExecuteScalar<int>(sqlQuery);

            sqlQuery = $@"
                INSERT INTO public.sys_data_sender_register(  
                sys_imports_register_id,  
                erc_code,  
                sender_organization_name, 
                sender_organization_unit_name,  
                sender_inn,  
                sender_kpp,  
                sender_phone,  
                sender_fio)
                VALUES ({infoDescript.LoadId}, {infoDescript.ErcCode}, '{infoDescript.ErcName}', 
                        '{senderOrganizationUnitName}','{senderInn}', '{senderKpp}', '{senderPhone}', '{senderFio}')";
            sqlExecutor.ExecuteSql(sqlQuery);

            //сначала записываем в БД информацию о попытке загрузки, только потом проверяем
            if (infoDescript.CalculationDate == default(DateTime))
            {
                throw new Exception(
                    " Архив не прошел проверку! " +
                    " В информационном описании некорректно указан месяц и год начислений. Значение из файла: " + infoDescript.CalculationDate);
            }

            //проставляем загружаемый расчетный месяц
            loadedFile.CalculationDate = infoDescript.CalculationDate;
            this.Container.Resolve<IDomainService<LoadedFileRegister>>().Update(loadedFile);

            sqlQuery = $@"
                UPDATE public.erc SET 
                dat_sys_start = '{systemStartDate}' 
                WHERE erc_code = {infoDescript.ErcCode}";
            sqlExecutor.ExecuteSql(sqlQuery);
        }

        /// <summary>
        /// Импорт секции
        /// </summary>
        /// <param name="stream">Поток загружаемого файла</param>
        /// <param name="sqlExecutor">Исполнитель sql команд</param>
        /// <param name="infoDescript">Информационное описание</param>
        /// <param name="section">Обрабатываемая секция</param>
        private void ImportFile(Stream stream, SqlExecutor.SqlExecutor sqlExecutor, InfoDescript infoDescript, FileSection section)
        {
            var inheritTableName = $"{section.TableName}_{infoDescript.ErcCode}_{infoDescript.CalculationDate:yyyyMM}";
            var inheritTempTableName = inheritTableName;

            var sqlQuery = $"DROP TABLE IF EXISTS {inheritTableName}";
            sqlExecutor.ExecuteSql(sqlQuery);

            var check = section.PguFileSection == PguFileSection.RevalServ
                ? $"dat_month = '{infoDescript.CalculationDate.ToShortDateString()}'"
                : $"erc_code = {infoDescript.ErcCode} AND dat_month = '{infoDescript.CalculationDate.ToShortDateString()}'";

            sqlQuery = $@"
                CREATE TABLE IF NOT EXISTS {inheritTableName} 
                (LIKE {section.TableName} INCLUDING ALL, 
                 CHECK ({check}) 
                ) 
                INHERITS({section.TableName}) 
                WITH (OIDS=TRUE)";
            sqlExecutor.ExecuteSql(sqlQuery);

            var columns = section.Columns != null ? $"({string.Join(", ", section.Columns)})" : "";

            if (section.PguFileSection == PguFileSection.ChargExpenseServ)
            {
                sqlQuery = @"
                    DROP TABLE IF EXISTS temp_charge;                    
                    CREATE TEMP TABLE temp_charge 
                    (LIKE public.charge INCLUDING ALL);
                    ALTER TABLE temp_charge ALTER COLUMN ordering TYPE INTEGER;";

                sqlExecutor.ExecuteSql(sqlQuery);
                inheritTableName = "temp_charge";
            }

            sqlQuery = $@"
                COPY {inheritTableName} {columns}
                FROM stdin 
                WITH
                DELIMITER AS '|' 
                NULL AS ''";

            try
            {
                sqlExecutor.CopyIn(sqlQuery, stream, Encoding.GetEncoding(1251));

                if (section.PguFileSection == PguFileSection.ChargExpenseServ)
                {
                    var condition = "(CASE WHEN ordering > 32767 THEN 32767 ELSE ordering END) ordering";
                    var tempSections = section.Columns.Select(x => x == "ordering" ? condition : x).ToList();
                    var tempColumns = $"{string.Join(", ", tempSections)}";

                    sqlExecutor.ExecuteSql($@"
                        INSERT INTO {inheritTempTableName} {columns}
                        SELECT {tempColumns} FROM temp_charge;");
                }
            }
            catch (PostgresException ex)
            {
                var sectionName = Enum.GetName(typeof(PguFileSection), section.PguFileSection);

                //расшифровываем ошибку
                var errorType = GetErrorMessage(ex, sectionName);

                throw new Exception($@"
                    Архив не прошел проверку! {errorType}{Environment.NewLine}
                    Текст ошибки: '{ex.Message}'.{Environment.NewLine}
                    Расшифровка ошибки: '{ex.Detail}'.{Environment.NewLine}
                    Номер строки в файле с некорректными данными: '{ex.Line}'.{Environment.NewLine}
                    Местоположение некорректных данных:'{ex.Where}'");
            }
        }

        /// <summary>
        /// Копирование адресов из секции "Характеристики жилищного фонда" в МЖФ
        /// </summary>
        /// <param name="zipEntries">Файлы загружаемого архива</param>
        private void ImportAddressesToMgf(ZipEntry[] zipEntries)
        {
            var section = this.sections.First(x => x.PguFileSection == PguFileSection.CharacterGilFond);
            var entry = this.GetFileSection(zipEntries, section.PguFileSection);
            var tempTableName = $"tmp_{DateTime.Now.Ticks}";

            if (entry != null)
            {
                const string ercColumn = "erc_code";
                var columns = $"({string.Join(", ", section.Columns)})";

                var tempColumns = section.Columns
                    .Where(x => x != ercColumn)
                    .Select(x => $"{x} text")
                    .ToList();

                tempColumns.Add($"{ercColumn} int");

                using (var sqlExecutor = new SqlExecutor.SqlExecutor(this.DbConfigProvider.ConnectionString))
                {
                    var sql = $@"CREATE TEMP TABLE {tempTableName} ({string.Join(",", tempColumns)})";
                    sqlExecutor.ExecuteSql(sql);

                    using (var stream = (Stream)entry.OpenReader())
                    {
                        sql = $@"
                        COPY {tempTableName} {columns}
                        FROM stdin 
                        WITH
                        DELIMITER AS '|' 
                        NULL AS ''";
                        sqlExecutor.CopyIn(sql, stream, Encoding.GetEncoding(1251));
                    }

                    sql = $@"
                    INSERT INTO public.pgmu_addresses (erc_code, post_code, town, district, street, house, building, apartment, room)
                    SELECT erc_code, post_code, town, rajon, ulica, ndom, nkor, nkvar, nkvar_n
                    FROM {tempTableName}
                    ON CONFLICT DO NOTHING;";
                    sqlExecutor.ExecuteSql(sql);
                }
            }
        }

        /// <summary>
        /// Добавление записи в saldo_date
        /// </summary>
        /// <param name="sqlExecutor">Объект для работы с БД</param>
        /// <param name="ercCode">Код ЕРЦ</param>
        /// <param name="calculationDate">Загружаемый расчетный месяц</param>
        private void InsertIntoSaldoDate(SqlExecutor.SqlExecutor sqlExecutor, DateTime calculationDate, long ercCode)
        {
            var sqlQuery = $@"
                SELECT COALESCE(CAST(MAX(saldo_date) AS DATE), '1900-1-1') 
                FROM public.saldo_date 
                WHERE active = 1 
                AND erc_code = {ercCode}";

            //если загружается месяц, который раньше последнего загруженного
            if (sqlExecutor.ExecuteScalar<DateTime>(sqlQuery) > calculationDate)
            {
                //вставляем запись, но делаем ее НЕ активной
                sqlQuery = $@"
                    INSERT INTO public.saldo_date(erc_code,saldo_month,saldo_year,saldo_date,active) 
                    VALUES ({ercCode},{calculationDate.Month},{calculationDate.Year},'{calculationDate.ToShortDateString()}',{0})";
                sqlExecutor.ExecuteSql(sqlQuery);
            }
            else
            {
                //делаем все записи НЕактивными, и вставляем новую запись активную
                sqlQuery = $@" 
                    UPDATE public.saldo_date 
                    SET active = 0 
                    WHERE erc_code = {ercCode} ";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = $@"
                    INSERT INTO public.saldo_date(erc_code,saldo_month,saldo_year,saldo_date,active)
                    VALUES ({ercCode},{calculationDate.Month},{calculationDate.Year},'{calculationDate.ToShortDateString()}',{1})";
                sqlExecutor.ExecuteSql(sqlQuery);
            }
        }

        /// <summary>
        /// Удаление созданных таблиц
        /// </summary>
        /// <param name="infoDescript"></param>
        /// <param name="loadedFileId"></param>
        private ImportDataResult DropTables(InfoDescript infoDescript, long loadedFileId)
        {
            using (var sqlExecutor = new SqlExecutor.SqlExecutor(this.BilConnectionService.GetConnection(ConnectionType.GisConnStringPgu)))
            {
                // Делаем расчетный месяц не активным
                var sqlQuery = $@"
                        UPDATE public.saldo_date 
                        SET active = 0 
                        WHERE saldo_date = '{infoDescript.CalculationDate.ToShortDateString()}'::DATE
                        AND erc_code =  {infoDescript.ErcCode}";
                sqlExecutor.ExecuteSql(sqlQuery);

                var inheritTableNamePostfix = $"{infoDescript.ErcCode}_{infoDescript.CalculationDate:yyyyMM}";

                // Удаляем все таблицы по этому ЕРЦ по загружаемому месяцу
                var sqlQueryDrop = new StringBuilder();
                this.sections.ForEach(x => sqlQueryDrop.Append($"DROP TABLE IF EXISTS {x.TableName}_{inheritTableNamePostfix};"));
                sqlExecutor.ExecuteSql(sqlQueryDrop.ToString());
 
                return new ImportDataResult(false, "Ошибка при загрузке!", loadedFileId);
            }
        }

        /// <summary>
        /// Получение текста ошибки по коду 
        /// </summary>
        /// <param name="ex">Ошибка</param>
        /// <param name="sectionName">Наименование секции</param>
        /// <returns>Текст ошибки</returns>
        private static string GetErrorMessage(PostgresException ex, string sectionName)
        {
            if (ex.Code == "23514")
            {
                switch (ex.ConstraintName)
                {
                    case "num_cnt_notnull":
                        return "Не заполнено обязательное поле Заводской номер прибора учета";
                    default:
                        return $"Нарушено ограничение {ex.ConstraintName}";
                }
            }

            switch (ex.Code)
            {
                case "23505":
                    return $"В секции '{sectionName}' имеются дубли!";
                case "22505":
                case "22P04":
                    return $"В секции '{sectionName}' имеются некорректные символы!";
                case "22001":
                    return $"В секции '{sectionName}' имеются строковые значения превышающие разрешенную длину!";
                case "22003":
                    return $"В секции '{sectionName}' имеются численные значения превышающие разрешенную размерность!";
                default: return ex.Message;
            }
        }

        /// <summary>
        /// Вспомогательная сущность для загрузки
        /// </summary>
        private class InfoDescript
        {
            /// <summary>
            /// Уникальный код загрузки
            /// </summary>
            public int LoadId { get; set; }

            /// <summary>
            /// Код ЕРЦ
            /// </summary>
            public long ErcCode { get; set; }

            /// <summary>
            /// Наименование ЕРЦ
            /// </summary>
            public string ErcName { get; set; }

            /// <summary>
            /// Расчетный месяц за который грузятся данные
            /// </summary>
            public DateTime CalculationDate { get; set; }

            /// <summary>
            /// Версия формата данных
            /// </summary>
            public string FormatVersion { get; set; }
        }
    }
}