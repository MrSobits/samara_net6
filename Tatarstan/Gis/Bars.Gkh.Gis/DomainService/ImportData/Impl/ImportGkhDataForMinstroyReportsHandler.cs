namespace Bars.Gkh.Gis.DomainService.ImportData.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Gis.DataResult;
    using Bars.Gkh.Gis.DomainService.BilConnection;
    using Bars.Gkh.Gis.DomainService.CrpCryptoProvider;
    using Bars.Gkh.Gis.DomainService.CrpCryptoProvider.Impl;
    using Bars.Gkh.Gis.DomainService.JExtractor;
    using Bars.Gkh.Gis.DomainService.JExtractor.Impl;
    using Bars.Gkh.Gis.Entities.Register.LoadedFileRegister;
    using Bars.Gkh.Gis.Enum;
    using Bars.Gkh.SqlExecutor;

    using Microsoft.Extensions.Logging;

    using Npgsql;

    /// <summary>
    /// Класс загрузки данных ЖКХ для отчетов для Минстроя
    /// </summary>
    public class ImportGkhDataForMinstroyReportsHandler : BaseImportDataHandler
    {
        /// <summary>
        /// Название схемы, в которую грузятся данные
        /// </summary>
        private const string SchemaName = "gkh";

        protected IJExtractorService JExtractor;

        private TypeStatus resultStatus;

        private readonly StringBuilder importLog;

        protected ICrpCryptoProvider CryptoProvider;

        /// <summary>
        /// Начальный загружаемый расчетный месяц
        /// </summary>
        private DateTime startCalculationMonth;

        /// <summary>
        /// Код загружаемого района
        /// </summary>
        private int districtCode;

        public ImportGkhDataForMinstroyReportsHandler()
        {
            // загружаемые файлы приходят в формате ".J", 
            // поэтому создаем инстанс разархиватора
            this.JExtractor = new JExtractorService();
            this.importLog = new StringBuilder();

            this.CryptoProvider = new CrpCryptoProvider();
        }

        public IFileManager FileManager { get; set; }

        public IBilConnectionService BilConnectionService { get; set; }

        public override IEnumerable<ImportDataResult> ImportData(IEnumerable<LoadedFileRegister> loadedFile)
        {
            return loadedFile.Select(this.ImportData);
        }

        public override ImportDataResult ImportData(LoadedFileRegister loadedFileId)
        {
            // директория для распаковки (директория создастся в процессе распаковки)
            var extractionDirectory = string.Empty;

            try
            {
                this.importLog.Clear();
                this.importLog.AppendLine("Дата загрузки: " + DateTime.Now);
                this.importLog.AppendLine("Тип загрузки: 'Загрузка данных для отчетов (стат.оплат)'");
                this.importLog.AppendLine("Наименование файла: " + loadedFileId.File.FullName);

                // получаем полный путь к архиву
                var archive = this.FileManager.LoadFile(loadedFileId.File.Id);

                extractionDirectory = Path.Combine(Path.GetDirectoryName(archive.Path), "temp" + DateTime.Now.Ticks);

                // распаковываем архив
                var extraction = this.JExtractor.ExtractToDirectory(archive.Path, extractionDirectory);
                if (!extraction.Success)
                {
                    throw new Exception($"Не удалось распаковать архив: {extraction.Message}");
                }

                // заголовочный информационный файл
                var headerFile = extractionDirectory + @"\_link.txt";

                if (!File.Exists(headerFile))
                    throw new Exception($"Не найден заголовочный файл '{headerFile}'");

                // версия ПО, в котором выгузили файл
                string unloaderVersion;

                // конечный расчетный месяц
                DateTime endCalculationMonth;

                // наименование района
                string districtName;

                // Разбираем заголовочный инфомрационный файл
                using (var streamReader = new StreamReader(File.OpenRead(headerFile), Encoding.GetEncoding(65001)))
                {
                    // строка файла
                    string line;

                    // элементры строки файла
                    string[] fields;
                    {
                        // считываем первую строку, в ней лежит версия файла
                        line = streamReader.ReadLine();
                        if (line == null)
                        {
                            throw new Exception("Файл пуст");
                        }

                        var formatVersion = Convert.ToDecimal(line.Replace("VERSION=", string.Empty).Replace('.', ','));

                        // проверяем формат выгрузки
                        if (formatVersion < 5.31m)
                        {
                            throw new Exception(
                                $" Архив не прошел валидацию! Неактуальная версия формата загрузки! (версия выгрузки в файле: '{formatVersion}') "
                                + " Необходимо обновить программное обеспечение и повторно выгрузить файл.");
                        }

                        this.importLog.AppendLine($"Версия выгрузки: '{formatVersion}' ");
                    }
                    {
                        // считываем вторую строку, в ней лежит период, за который пришли данные
                        line = streamReader.ReadLine();

                        if (!Regex.IsMatch(line, "^date_begin="))
                        {
                            throw new Exception("Неверный заголовок");
                        }

                        // считываем поля, убрав лишние символы
                        fields = line.Replace("date_begin=", "").Split('|');

                        // считываем начальный расчетный месяц
                        this.startCalculationMonth = Convert.ToDateTime(fields[0]);

                        // считываем конечный расчетный месяц
                        endCalculationMonth = Convert.ToDateTime(fields[1]);

                        // считываем версию ПО, в котором выгузили файл
                        unloaderVersion = fields[3].Trim();
                    }
                    {
                        // считываем третью строку, в ней лежит информация о районе
                        line = streamReader.ReadLine();

                        if (!Regex.IsMatch(line, "^area="))
                        {
                            throw new Exception("Неверный формат заголовочного файла");
                        }

                        // считываем поля, убрав лишние символы
                        fields = line.Replace("area=", string.Empty).Split('|');

                        // считываем код района
                        this.districtCode = Convert.ToInt32(fields[0]);

                        // считываем наименование района
                        districtName = fields[1];
                    }
                }

                this.importLog.AppendLine($"Версия выгружаемого ПО: '{unloaderVersion}' ");
                this.importLog.AppendLine($"Начальный расчетный месяц: '{this.startCalculationMonth.ToShortDateString()}' ");
                this.importLog.AppendLine($"Конечный расчетный месяц: '{endCalculationMonth.ToShortDateString()}' ");
                this.importLog.AppendLine($"Код района: '{this.districtCode}' ");
                this.importLog.AppendLine($"Наименование района: '{districtName.Trim()}' ");

                if ((endCalculationMonth - this.startCalculationMonth).Days > 31)
                {
                    throw new Exception("Файл должен содержать данные только за один месяц!");
                }

                // открываем соединение к БД с отчетами
                using (var sqlExecutor = new SqlExecutor(this.BilConnectionService.GetConnection(ConnectionType.GisConnStringReports)))
                {
                    this.importLog.AppendLine(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                    this.importLog.AppendLine("Соединение с БД установлено. Старт загрузки данных в БД.");

                    var sqlQuery = string.Format(
                        "SELECT  COUNT(*) AS count " +
                        " FROM gkh.imports" +
                        " WHERE calculation_date = '{1}'::DATE" +
                        " AND district_code = {0} " +
                        " AND load_success != 1 ",
                        this.districtCode,
                        this.startCalculationMonth);

                    if (sqlExecutor.ExecuteScalar<int>(sqlQuery) == 0)
                    {
                        sqlQuery = string.Format(
                            " INSERT INTO gkh.imports " +
                            " (district_code, calculation_month, calculation_year, calculation_date, load_date, load_success) " +
                            "  VALUES ({0}, {1}, {2}, '{3}','{4}', {5})",
                            this.districtCode,
                            this.startCalculationMonth.Month,
                            this.startCalculationMonth.Year,
                            this.startCalculationMonth.ToShortDateString(),
                            DateTime.Now,
                            0);
                        sqlExecutor.ExecuteSql(sqlQuery);
                    }

                    // создаем временные таблицы, которые будут необходимы в процессе работы
                    this.CreateTempTables(sqlExecutor, this.districtCode, this.startCalculationMonth);

                    // расшифровываем и переносим данные во временные таблицы
                    this.LoadDataIntoTempTables(extractionDirectory, sqlExecutor, this.districtCode, this.startCalculationMonth);

                    // заменяем null значения на значения по умолчанию
                    this.UpdateTempTables(sqlExecutor, this.districtCode, this.startCalculationMonth);

                    // удаляем данные, которые были ранее по загружаемому периоду для загружемого района
                    this.DeleteOldData(sqlExecutor, this.districtCode, this.startCalculationMonth, ImportGkhDataForMinstroyReportsHandler.SchemaName);

                    // обработка и перенос данных из временных таблиц в основные
                    // большой, долгий и самый основной метод.
                    // Лучше не смотреть, может плохо повлиять на психику..
                    this.TransferDataToMainTables(sqlExecutor,
                        this.districtCode,
                        this.startCalculationMonth,
                        endCalculationMonth,
                        ImportGkhDataForMinstroyReportsHandler.SchemaName);

                    this.importLog.AppendLine("Успешно завершено!");
                    this.importLog.AppendLine(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

                    sqlQuery = string.Format(
                        " UPDATE gkh.imports  " +
                        " SET load_success = 1  " +
                        " WHERE calculation_date = '{1}'::DATE" +
                        " AND district_code = {0} ",
                        this.districtCode,
                        this.startCalculationMonth.ToShortDateString());

                    sqlExecutor.ExecuteSql(sqlQuery);
                }

                this.resultStatus = TypeStatus.Done;
            }
            catch (Exception ex)
            {
                this.resultStatus = TypeStatus.Error;
                this.importLog.AppendLine($"При загрузке произошла ошибка: {ex.Message}");
                using (var sqlExecutor = new SqlExecutor(this.BilConnectionService.GetConnection(ConnectionType.GisConnStringReports)))
                {
                    // удаляем данные, которые были ранее по загружаемому периоду для загружемого района
                    this.DeleteOldData(sqlExecutor, this.districtCode, this.startCalculationMonth, ImportGkhDataForMinstroyReportsHandler.SchemaName);
                }

                return new ImportDataResult(false, "Ошибка при загрузке!", loadedFileId.Id);

                // TypeStatus = TypeStatus.Error;
            }
            finally
            {
                // очищаем распакованные файлы
                Directory.Delete(extractionDirectory, true);

                using (var sqlExecutor = new SqlExecutor(this.BilConnectionService.GetConnection(ConnectionType.GisConnStringReports)))
                {
                    // удаляем временные таблицы
                    this.DropTempTables(sqlExecutor, this.districtCode, this.startCalculationMonth);
                }

                // сохраняем в файл протокол-загрузки
                var logFileName = $"ПРОТОКОЛ_ЗАГРУЗКИ_{loadedFileId.File.Name}";
                var resultString = this.importLog.ToString();
                var bytes = Encoding.UTF8.GetBytes(resultString);

                // сохраняем лог-файл
                loadedFileId.Log = this.Container.Resolve<IFileManager>().SaveFile(logFileName, "txt", bytes);
                loadedFileId.ImportResult = ImportResult.Success;
                loadedFileId.TypeStatus = this.resultStatus;

                var loadedFileRegisterDomain = this.Container.ResolveDomain<LoadedFileRegister>();
                using (this.Container.Using(loadedFileRegisterDomain))
                {
                    loadedFileRegisterDomain.Update(loadedFileId);
                }
            }

            return new ImportDataResult(true, "Успешно загружено!", loadedFileId.Id);
        }

        /// <summary>
        /// Создание временных таблиц, которые будут необходимы в процессе работы
        /// </summary>
        /// <param name="sqlExecutor">Объект для работы с БД</param>
        /// <param name="districtCode">Код загружаемого района</param>
        /// <param name="calculationMonth">Загружаемый расчетный месяц</param>
        private void CreateTempTables(SqlExecutor sqlExecutor, int districtCode, DateTime calculationMonth)
        {
            // окончание названия временных таблиц (для уникальности)
            var tablePostfix = districtCode + "_" + calculationMonth.ToString("yyyyMM");

            // приборы учета
            var sqlQuery = string.Format(
                " DROP TABLE IF EXISTS {0}; " +
                " CREATE TABLE {0}	" +
                " (                        " +
                "   graj character(100),   " +
                "   year_ integer,         " +
                "   month_ integer,        " +
                "   service character(100)," +
                "   nzp_serv integer,      " +
                "   count_dpu integer,     " +
                "   count_nodpu integer,   " +
                "   count_all integer,     " +
                "   count_all1 integer,    " +
                "   count_ipu integer,     " +
                "   count_noipu integer,   " +
                "   count_kv_all integer,  " +
                "   count_dky integer,     " +
                "   reserve char(1)        " +
                " );                       ",
                " tmp_pu_" + tablePostfix
            );
            sqlExecutor.ExecuteSql(sqlQuery);

            // долги1
            sqlQuery = string.Format(
                " DROP TABLE IF EXISTS {0} ;" +
                " CREATE TABLE {0}	" +
                " (                               " +
                "   service character(100),       " +
                "   name_supp character(100),     " +
                "   nzp_serv integer,             " +
                "   nzp_supp integer,             " +
                "   year_ integer,                " +
                "   month_ integer,               " +
                "   sum_outsaldo numeric(18,4),   " +
                "   sum_money_priv numeric(18,4),    " +
                "   sum_money_nanim numeric(18,4),    " +
                "   sum_real_priv numeric(18,4)," +
                "   sum_real_nanim numeric(18,4)," +
                "   dolg_priv numeric(18,4),         " +
                "   dolg_nanim numeric(18,4),         " +

                // 14.Месячное начисление по собственникам.sum_accrual_priv (добавлено в версии 5.24)
                "   sum_accrual_priv numeric(18,4),         " +

                // 15.Месячное начисление по нанимателям.sum_accrual_nanim (добавлено в версии 5.24)
                "   sum_accrual_nanim numeric(18,4),         " +

                // 16.Сумма перекидки сальдо выполняемая вручную по собственникам.sum_charge_priv (добавлено в версии 5.24)
                "   sum_charge_priv numeric(18,4),         " +

                // 17.Сумма перекидки сальдо выполняемая вручную по нанимателям.sum_charge_nanim (добавлено в версии 5.24)
                "   sum_charge_nanim numeric(18,4),         " +

                // 18.Сумма перерасчета предыдущего периода по собственникам.sum_reval_priv (добавлено в версии 5.24)
                "   sum_reval_priv numeric(18,4),         " +

                // 19.Сумма перерасчета предыдущего периода по нанимателям.sum_reval_nanim (добавлено в версии 5.24)
                "   sum_reval_nanim numeric(18,4),         " +
                "   reserve char(1)               " +
                " );                              ",
                " tmp_dolg1_" + tablePostfix
            );
            sqlExecutor.ExecuteSql(sqlQuery);

            // долги2
            sqlQuery = string.Format(
                " DROP TABLE IF EXISTS {0};" +
                " CREATE TABLE {0} 				" +
                " (                                " +
                "   service          char(100),     " +
                "   name_supp        char(100),     " +
                "   nzp_serv         integer  ,     " +
                "   nzp_supp         integer  ,     " +
                "   year_            integer  ,     " +
                "   month_           integer  ,     " +
                "   pl_kv_p          NUMERIC(18,4), " +
                "   pl_kv_n          NUMERIC(18,4), " +
                "   cntls_all_priv   integer  ,     " +
                "   cntls_all_nanim  integer  ,     " +
                "   pereplata_ls_p   integer  ,     " +
                "   pereplata_ls_n   integer  ,     " +
                "   pereplata_summ_p NUMERIC(14,2), " +
                "   pereplata_summ_n NUMERIC(14,2), " +
                "   cntlsdo_p        integer  ,     " +
                "   cntlsdo_n        integer  ,     " +
                "   summdo_p         NUMERIC(14,2), " +
                "   summdo_n         NUMERIC(14,2), " +
                "   cntls1_p         integer  ,     " +
                "   cntls1_n         integer  ,     " +
                "   summ1_p          NUMERIC(18,4), " +
                "   summ1_n          NUMERIC(18,4), " +
                "   cntls2_p         integer,       " +
                "   cntls2_n         integer,       " +
                "   summ2_p          NUMERIC(18,4), " +
                "   summ2_n          NUMERIC(18,4), " +
                "   cntls3_p         integer,       " +
                "   cntls3_n         integer,       " +
                "   summ3_p          NUMERIC(18,4), " +
                "   summ3_n          NUMERIC(18,4),  " +
                "   reserve char(1)               " +
                " );                               ",
                " tmp_dolg2_" + tablePostfix
            );
            sqlExecutor.ExecuteSql(sqlQuery);

            // ОДН
            sqlQuery = string.Format(
                " DROP TABLE IF EXISTS {0} ; " +
                " CREATE TABLE {0} 		 " +
                " (                              " +
                "   pref character varying(10),  " +
                "   year_ integer,               " +
                "   month_ integer,              " +
                "   town character varying(50),  " +
                "   rajon character varying(50), " +
                "   ulica character varying(50), " +
                "   nzp_dom integer,             " +
                "   ndom character varying(10),  " +
                "   nkor character varying(10),  " +
                "   nzp_serv integer,            " +
                "   count_ls integer,            " +
                "   nzp_type_alg integer,        " +
                "   sum_tarif numeric(14,2),     " +
                " typek integer,                 " +
                "   count_all integer,           " +
                "   rvaldlt numeric(14,7),       " +
                "   tarif numeric(14,3),         " +
                "   c_calc numeric(14,5) ,       " +
                "   reserve char(1)              " +
                " );                             ",
                " tmp_odn_" + tablePostfix
            );
            sqlExecutor.ExecuteSql(sqlQuery);

            // сироты
            sqlQuery = string.Format(
                " DROP TABLE IF EXISTS {0} ; " +
                " CREATE TABLE {0} 	" +
                " (                         " +
                "   ye_ integer,            " +
                "   mn_ integer,            " +
                "   rajon character(100),   " +
                "   num_ls integer, " +
                "   fio character(250),     " +
                "   adress character(50),   " +
                "   kol_gil integer,        " +
                "   pl_kv numeric(14,2),    " +
                "   nzp_serv integer,       " +
                "   dolg_in numeric(14,2),  " +
                "   sum_real numeric(14,2), " +
                "   sum_money numeric(14,2)," +
                "   dolg_out numeric(14,2) ," +
                "   reserve char(1)         " +
                " );                        ",
                " tmp_siroti_" + tablePostfix
            );
            sqlExecutor.ExecuteSql(sqlQuery);

            // оплаты
            sqlQuery = string.Format(
                " DROP TABLE IF EXISTS {0} ; " +
                " CREATE TABLE {0} 	" +
                " (                         " +
                "   pref        char(10),   " +
                "   nzp_graj int,           " +
                " nzp_headbank integer,     " +
                " bank        char(100),    " +
                " nzp_bank    integer,      " +
                " nzp_pack_ls integer,      " +
                " pkod        numeric(10,0)," +
                " kod_sum     integer,      " +
                " paysource   integer,      " +
                " dat_uchet   DATE,         " +
                " dat_vvod    DATE,         " +
                " dat_month   DATE,         " +
                " sum_oplat   numeric(14,2), " +
                " reserve char(1)           " +
                " );                        "
                ,
                " tmp_stat_opl_" + tablePostfix
            );
            sqlExecutor.ExecuteSql(sqlQuery);

            // начисленные суммы
            sqlQuery = string.Format(
                " DROP TABLE IF EXISTS {0} ; " +
                " CREATE TABLE {0} 	" +
                "(												      " +
                "  erc character(100),									" +
                "  nzp_dom integer DEFAULT 0,                        " +
                "  prf character(10),                                 " +
                "  nzp_ul integer DEFAULT 0,                        " +
                "  ulica character(60),                               " +
                "  ndom character(10),                                " +
                "  nkor character(10),                                " +
                "  nzp_raj integer DEFAULT 0,                        " +
                "  rajon character(60),                               " +
                "  nzp_town integer DEFAULT 0,                        " +
                "  town character(30),                                " +
                "  nzp_area integer DEFAULT 0,                        " +
                "  area character(60),                                " +
                "  mn_ integer,                                       " +
                "  yr_ integer,                                       " +
                "  nzp_serv integer DEFAULT 0,                        " +
                "  nzp_supp integer DEFAULT 0,                        " +
                "  name_supp character(100),                          " +
                "  nzp_frm integer DEFAULT 0,                        " +
                "  nzp_measure integer DEFAULT 0,                    " +
                "  typek integer DEFAULT 0,                          " +
                "  is_device integer DEFAULT 0,                       " +
                "  cntls integer DEFAULT 0,                           " +
                "  cntkg integer DEFAULT 0,                           " +
                "  s_ob numeric(14,2) DEFAULT 0.00,                   " +
                "  s_ot numeric(14,2) DEFAULT 0.00,                   " +
                "  s_oba numeric(14,2) DEFAULT 0.00,                  " +
                "  s_ota numeric(14,2) DEFAULT 0.00,                  " +
                "  s_dom numeric(14,2) DEFAULT 0.00,                  " +
                "  s_mop numeric(14,2) DEFAULT 0.00,                  " +
                "  s_mop_hv numeric(14,2) DEFAULT 0.00,               " +
                "  s_mop_gv numeric(14,2) DEFAULT 0.00,               " +
                "  cntls_serv integer DEFAULT 0,                        " +
                "  cntkg_serv integer DEFAULT 0,                        " +
                "  s_ob_serv numeric(14,2),                           " +
                "  tarif numeric(14,5) DEFAULT 0.00,                  " +
                "  rsum_tarif numeric(14,2) DEFAULT 0.00,             " +
                "  sum_nedop numeric(14,2) DEFAULT 0.00,              " +
                "  c_calc numeric(14,2) DEFAULT 0.00,                 " +
                "  sum_tarif_sn_f numeric(14,2) DEFAULT 0.00,         " +
                "  sum_insaldo numeric(14,2) DEFAULT 0.00,            " +
                "  sum_real numeric(14,2) DEFAULT 0.00,               " +
                "  real_charge numeric(14,2) DEFAULT 0.00,            " +
                "  reval numeric(14,2) DEFAULT 0.00,                  " +
                "  sum_money numeric(14,2) DEFAULT 0.00,              " +
                "  sum_outsaldo numeric(14,2) DEFAULT 0.00,           " +
                "  rval_real numeric(14,7) DEFAULT 0.0000000,         " +
                "  rval numeric(14,7) DEFAULT 0.0000000,              " +
                "  rvaldlt numeric(14,7) DEFAULT 0.0000000,           " +
                "  nzp_type_alg integer DEFAULT 0,                    " +
                "  vl210 numeric(15,7) DEFAULT 0.0000000,             " +
                "  dop87 numeric(14,7) DEFAULT 0.0000000,             " +
                "                                                     " +
                "  kf307 numeric(14,7) DEFAULT 0.0000000,             " +
                "  kf307n numeric(14,7) DEFAULT 0.0000000,            " +
                "                                                     " +
                "  is_dpu integer DEFAULT 0,                          " +
                "  tarif_gkal numeric(14,7) DEFAULT 0.0000000,        " +
                "  dpu_val1 numeric(14,7) DEFAULT 0.0000000,          " +
                "  dpu_val2 numeric(14,7) DEFAULT 0.0000000,          " +
                "  dpu_ngp numeric(14,7) DEFAULT 0.0000000,           " +
                "  odn_is_pu integer DEFAULT 0,                       " +
                "  odn_norm numeric(14,7) DEFAULT 0.0000000,          " +
                "  odn_pl_mop numeric(14,7) DEFAULT 0.0000000,        " +
                "  odn_val numeric(14,7) DEFAULT 0.0000000,           " +
                "  odn_prev numeric(14,7) DEFAULT 0.0000000,          " +
                "  cnt_ls_val integer DEFAULT 0,                      " +
                "  sum_ls_val numeric(14,7) DEFAULT 0.0000000,        " +
                "  cnt_ls_norm integer DEFAULT 0,                     " +
                "  sum_ls_norm numeric(14,7) DEFAULT 0.0000000,       " +
                "  cnt_ls_210val integer DEFAULT 0,                   " +
                "  sum_ls_210val numeric(14,7) DEFAULT 0.0000000,     " +
                "  cnt_ls_210norm integer DEFAULT 0,                  " +
                "  sum_ls_210norm numeric(14,7) DEFAULT 0.0000000,    " +
                "  cnt_gils_pu numeric(14,7) DEFAULT 0.0000000,       " +
                "  cnt_gils_norm numeric(14,7) DEFAULT 0.0000000,     " +
                "  cnt_pl_norm numeric(14,7) DEFAULT 0.0000000,       " +
                "  cnt_pl_pu numeric(14,7) DEFAULT 0.0000000,         " +
                "  cnt_ls_a_val integer DEFAULT 0,                    " +
                "  sum_ls_a_val numeric(14,7) DEFAULT 0.0000000,      " +
                "  cnt_pl_a_pu numeric(14,7) DEFAULT 0.0000000,       " +
                "  cnt_ls_a_norm integer DEFAULT 0,                   " +
                "  sum_ls_a_norm numeric(14,7) DEFAULT 0.0000000,     " +
                "  cnt_pl_a_norm numeric(14,7) DEFAULT 0.0000000,     " +
                "                                                     " +
                "                                                     " +
                "  sum_dpu numeric(14,2) DEFAULT 0.00,                " +
                "  dt_begin date,                                     " +
                "  dt_end date,                                       " +
                "  val_begin numeric(14,2),                           " +
                "  val_end numeric(14,2),                             " +
                "  ngp_cnt numeric(14,2) DEFAULT 0.00,                " +
                "                                                     " +
                "  is_prev integer DEFAULT 0,                         " +
                "  etagnost character(10),                            " +

                // Площадь ЛС-разность площадей общей и МОП для дома (для СОИ)
                "  ispp integer DEFAULT 0,                    " +

                // Площадь жилых помещений дома
                "  s_pgp numeric(14,2) DEFAULT 0.0000000,     " +

                // Площадь нежилых помещений дома
                "  s_pnp numeric(14,2) DEFAULT 0.0000000,     " +

                // Коэффициент перевода расхода ГКал в куб.м для ГВС
                "  koef_gv numeric(14,7) DEFAULT 0.0000000,   " +

                // Тариф коммунальной услуги для услуги по содержанию общего имущества (СОИ)
                "  tarif_ku numeric(14,4) DEFAULT 0.0000000,  " +

                // Самостоятельное производство ГВС (бойлер);
                "  p1718 integer DEFAULT 0,  " +

                // Электроплита (на дом)
                "  p28 integer DEFAULT 0,  " +

                // СОИ для ХВС-Домовой расход по данным поставщика
                "  p1871 numeric(15,7) DEFAULT 0.0000000,  " +

                // СОИ для ХВС-Суммарный расход по ЛС для услуги по данным поставщика
                "  p1872 numeric(15,7) DEFAULT 0.0000000,  " +

                // СОИ для ГВС-Домовой расход на СОИ по данным поставщика
                "  p1873 numeric(15,7) DEFAULT 0.0000000,  " +

                // СОИ для ГВС-Суммарный расход по ЛС для услуги по данным поставщика
                "  p1874 numeric(15,7) DEFAULT 0.0000000,  " +

                // СОИ для Эл.Эн.-Домовой расход на СОИ по данным поставщика
                "  p1875 numeric(15,7) DEFAULT 0.0000000,  " +

                // СОИ для Эл.Эн.-Суммарный расход по ЛС для услуги по данным поставщика
                "  p1876 numeric(15,7) DEFAULT 0.0000000,  " +

                "  type_dom integer," +
                
                "  reserve char(1)                      " +
                
                ");                                                  ",
                " tmp_nach_" + tablePostfix);
            sqlExecutor.ExecuteSql(sqlQuery);

            // домовые приборы учета
            sqlQuery = string.Format(
                " DROP TABLE IF EXISTS {0} ; " +
                " CREATE TABLE {0} 	" +
                "(												      " +

                // Наименование расчетного
                "  erc character(100),									" +

                // Код дома
                "  nzp_dom integer DEFAULT 0,                        " +

                // Префикс 
                "  prf character(10),                                 " +

                // Код улицы   
                "  nzp_ul integer DEFAULT 0,                        " +

                // Название улицы
                "  ulica character(60),                               " +

                // Номер дома
                "  ndom character(10),                                " +

                // Номер корпуса
                "  nkor character(10),                                " +

                // Код района
                "  nzp_raj integer DEFAULT 0,                        " +

                // Название района 
                "  rajon character(60),                               " +

                // Код города  
                "  nzp_town integer DEFAULT 0,                        " +

                // Название города 
                "  town character(30),                                " +

                // Код района
                "  nzp_area integer DEFAULT 0,                        " +

                // Название района
                "  area character(60),                                " +

                // Месяц выгрузки 
                "  mn_ integer,                                       " +

                // Год выгрузки   
                "  yr_ integer,                                       " +

                // Код услуги  
                "  nzp_serv integer DEFAULT 0,                        " +

                // Номер счетчика
                "  num_cnt character(60),                                " +

                // Дата след поверки
                "  dat_provnext date,                                " +

                // Дата последнего показания   Дата    dat_uchet
                "  dat_uchet date,                                " +

                // Показание по прибору учета
                "  val_cnt numeric(14, 4) DEFAULT 0.00,           " +
                "  reserve char(1)                         " +
                ");                                                  ",
                " tmp_dom_pu_" + tablePostfix);
            sqlExecutor.ExecuteSql(sqlQuery);
            
            // Оплаты по услугам
            sqlQuery =
                $@" 
                 DROP TABLE IF EXISTS {"tmp_stat_opl_serv_" + tablePostfix};
                 CREATE TABLE {"tmp_stat_opl_serv_" + tablePostfix}
                 (                        
                    pref         char(10),
                    nzp_graj     integer,
                    nzp_headbank integer,
                    bank         char(100),
                    nzp_bank     integer,
                    nzp_pack_ls  integer,
                    pkod         numeric(10,0),
                    kod_sum      integer,
                    paysource    integer,
                    dat_uchet    DATE,
                    dat_vvod     DATE,
                    dat_month    DATE,
                    sum_oplat    numeric(14,2),
                    type_serv    integer,
                    transfer     integer,
                    reserve      char(1)
                 );";
            sqlExecutor.ExecuteSql(sqlQuery);
        }

        /// <summary>
        /// Удаление временных таблиц, которые создаются в процессе работы
        /// </summary>
        /// <param name="sqlExecutor">Объект для работы с БД</param>
        /// <param name="districtCode">Код загружаемого района</param>
        /// <param name="calculationMonth">Загружаемый расчетный месяц</param>
        private void DropTempTables(SqlExecutor sqlExecutor, int districtCode, DateTime calculationMonth)
        {
            // окончание названия временных таблиц (для уникальности)
            var tablePostfix = districtCode + "_" + calculationMonth.ToString("yyyyMM");

            var sqlQuery = string.Format(
                " DROP TABLE IF EXISTS {0}; ",
                " tmp_pu_" + tablePostfix
            );
            sqlExecutor.ExecuteSql(sqlQuery);

            sqlQuery = string.Format(
                " DROP TABLE IF EXISTS {0}; ",
                " tmp_dolg1_" + tablePostfix
            );
            sqlExecutor.ExecuteSql(sqlQuery);

            sqlQuery = string.Format(
                " DROP TABLE IF EXISTS {0}; ",
                " tmp_dolg2_" + tablePostfix
            );
            sqlExecutor.ExecuteSql(sqlQuery);

            sqlQuery = string.Format(
                " DROP TABLE IF EXISTS {0}; ",
                " tmp_nach_" + tablePostfix
            );
            sqlExecutor.ExecuteSql(sqlQuery);

            sqlQuery = string.Format(
                " DROP TABLE IF EXISTS {0}; ",
                " tmp_siroti_" + tablePostfix
            );
            sqlExecutor.ExecuteSql(sqlQuery);

            sqlQuery = string.Format(
                " DROP TABLE IF EXISTS {0}; ",
                " tmp_stat_opl_" + tablePostfix
            );
            sqlExecutor.ExecuteSql(sqlQuery);

            sqlQuery = string.Format(
                " DROP TABLE IF EXISTS {0}; ",
                " tmp_odn_" + tablePostfix
            );
            sqlExecutor.ExecuteSql(sqlQuery);
            
            sqlQuery = string.Format(
                " DROP TABLE IF EXISTS {0}; ",
                " tmp_stat_opl_serv_" + tablePostfix
            );
            sqlExecutor.ExecuteSql(sqlQuery);
        }

        /// <summary>
        /// Расшифровка и перенос данных во временные таблицы
        /// </summary>
        /// <param name="filesDirectory">Директория, где лежат файлы для загрузки</param>
        /// <param name="sqlExecutor">Объект для работы с БД</param>
        /// <param name="districtCode">Код загружаемого района</param>
        /// <param name="calculationMonth">Загружаемый расчетный месяц</param>
        private void LoadDataIntoTempTables(
            string filesDirectory,
            SqlExecutor sqlExecutor,
            int districtCode,
            DateTime calculationMonth)
        {
            var sectionValidationErrorLog = new StringBuilder();

            foreach (var file in Directory.GetFiles(filesDirectory, "*.crp"))
            {
                var decryptedFileFullName = this.CryptoProvider.Decrypt(file);
                var sectionName = Path.GetFileNameWithoutExtension(decryptedFileFullName);

                // формируем названия временных таблиц, которые были созданы ранее
                var tempTableName = $"tmp_{sectionName}_{districtCode}_{calculationMonth:yyyyMM}";

                try
                {
                    using (var stream = File.OpenRead(decryptedFileFullName))
                    {
                        // передача потока напрямую в БД
                        sqlExecutor.CopyIn(
                            $" COPY {tempTableName} FROM stdin WITH DELIMITER AS '|' NULL AS ''"
                            ,
                            stream, Encoding.GetEncoding(1251));

                        // добавляем колонку nzp_graj с заполнением кода района
                        try
                        {
                            var sqlQuery = $" ALTER TABLE {tempTableName} ADD COLUMN nzp_graj INTEGER DEFAULT {districtCode}";
                            sqlExecutor.ExecuteSql(sqlQuery);
                        }
                        catch
                        {
                            // ignored
                        }

                        // Выполняем валидацию загруженной секции
                        this.LoadedSectionValidation(sectionValidationErrorLog, sqlExecutor, sectionName, tempTableName);
                    }
                }
                catch (PostgresException ex)
                {
                    var error = string.Format(
                        " Некорректные данные в файле '{0}'! Сообщение об ошибке:'{1}'.{3}Местоположение некорректных данных: '{2}'",
                        Path.GetFileName(file),
                        ex.Message,
                        ex.Where,
                        Environment.NewLine);
                    throw new Exception(error, ex);
                }
            }

            if (sectionValidationErrorLog.Length > 0)
            {
                throw new Exception("\n" + sectionValidationErrorLog);
            }
        }

        /// <summary>
        /// Вылидация загруженной секции
        /// </summary>
        private void LoadedSectionValidation(StringBuilder sectionValidationErrorLog, SqlExecutor sqlExecutor, string sectionName, string tempTableName)
        {
            if (this.LoadedSectionIsEmpty(sqlExecutor, sectionName, tempTableName))
            {
                sectionValidationErrorLog.AppendLine($"Секция '{sectionName}' пустая, " +
                    "рекомендуем обратиться в выгружающую систему для корректировки данных.");
            }
        }

        /// <summary>
        /// Загруженная секция пуста?
        /// </summary>
        private bool LoadedSectionIsEmpty(SqlExecutor sqlExecutor, string sectionName, string tempTableName)
        {
            var result = false;

            var emptyCheckSectionNames = new[]
            {
                "stat_opl",
                "stat_opl_serv",
                "dolg1",
                "dolg2"
            };

            if (emptyCheckSectionNames.Contains(sectionName.ToLower()))
            {
                var sql = $"SELECT count(*) = 0 FROM {tempTableName};";

                result = sqlExecutor.ExecuteScalar<bool>(sql);
            }

            return result;
        }

        /// <summary>
        /// Заменяем null значения на значения по умолчанию
        /// </summary>
        /// <param name="sqlExecutor"></param>
        /// <param name="nzpGraj"></param>
        /// <param name="calculationMonth"></param>
        private void UpdateTempTables(
            SqlExecutor sqlExecutor,
            int nzpGraj,
            DateTime calculationMonth)
        {
            var tempTableNamePostfix = $"{nzpGraj}_{calculationMonth:yyyyMM}";
            var sqlQuery = string.Format(
                " UPDATE tmp_nach_{0}" +
                " SET " +
                " nzp_dom = COALESCE(nzp_dom,0), " +
                " nzp_ul = COALESCE(nzp_ul,0), " +
                " nzp_raj = COALESCE(nzp_raj,0), " +
                " nzp_town = COALESCE(nzp_town,0), " +
                " nzp_area = COALESCE(nzp_area,0), " +
                " mn_ = COALESCE(mn_,0), " +
                " yr_ = COALESCE(yr_,0), " +
                " nzp_serv = COALESCE(nzp_serv,0), " +
                " nzp_supp = COALESCE(nzp_supp,0), " +
                " nzp_frm = COALESCE(nzp_frm,0), " +
                " nzp_measure = COALESCE(nzp_measure,0), " +
                " typek = COALESCE(typek,0), " +
                " is_device = COALESCE(is_device,0), " +
                " cntls = COALESCE(cntls,0), " +
                " cntkg = COALESCE(cntkg,0), " +
                " s_ob = COALESCE(s_ob,0), " +
                " s_ot = COALESCE(s_ot,0), " +
                " s_oba = COALESCE(s_oba,0), " +
                " s_ota = COALESCE(s_ota,0), " +
                " s_dom = COALESCE(s_dom,0), " +
                " s_mop = COALESCE(s_mop,0), " +
                " s_mop_hv = COALESCE(s_mop_hv,0), " +
                " s_mop_gv = COALESCE(s_mop_gv,0), " +
                " cntls_serv = COALESCE(cntls_serv,0), " +
                " cntkg_serv = COALESCE(cntkg_serv,0), " +
                " s_ob_serv = COALESCE(s_ob_serv,0), " +
                " tarif = COALESCE(tarif,0), " +
                " rsum_tarif = COALESCE(rsum_tarif,0), " +
                " sum_nedop = COALESCE(sum_nedop,0), " +
                " c_calc = COALESCE(c_calc,0), " +
                " sum_tarif_sn_f = COALESCE(sum_tarif_sn_f,0), " +
                " sum_insaldo = COALESCE(sum_insaldo,0), " +
                " sum_real = COALESCE(sum_real,0), " +
                " real_charge = COALESCE(real_charge,0), " +
                " reval = COALESCE(reval,0), " +
                " sum_money = COALESCE(sum_money,0), " +
                " sum_outsaldo = COALESCE(sum_outsaldo,0), " +
                " rval_real = COALESCE(rval_real,0), " +
                " rval = COALESCE(rval,0), " +
                " rvaldlt = COALESCE(rvaldlt,0), " +
                " nzp_type_alg = COALESCE(nzp_type_alg,0), " +
                " vl210 = COALESCE(vl210,0), " +
                " dop87 = COALESCE(dop87,0), " +
                " kf307 = COALESCE(kf307,0), " +
                " kf307n = COALESCE(kf307n,0), " +
                " is_dpu = COALESCE(is_dpu,0), " +
                " tarif_gkal = COALESCE(tarif_gkal,0), " +
                " dpu_val1 = COALESCE(dpu_val1,0), " +
                " dpu_val2 = COALESCE(dpu_val2,0), " +
                " dpu_ngp = COALESCE(dpu_ngp,0), " +
                " odn_is_pu = COALESCE(odn_is_pu,0), " +
                " odn_norm = COALESCE(odn_norm,0), " +
                " odn_pl_mop = COALESCE(odn_pl_mop,0), " +
                " odn_val = COALESCE(odn_val,0), " +
                " odn_prev = COALESCE(odn_prev,0), " +
                " cnt_ls_val = COALESCE(cnt_ls_val,0), " +
                " sum_ls_val = COALESCE(sum_ls_val,0), " +
                " cnt_ls_norm = COALESCE(cnt_ls_norm,0), " +
                " sum_ls_norm = COALESCE(sum_ls_norm,0), " +
                " cnt_ls_210val = COALESCE(cnt_ls_210val,0), " +
                " sum_ls_210val = COALESCE(sum_ls_210val,0), " +
                " cnt_ls_210norm = COALESCE(cnt_ls_210norm,0), " +
                " sum_ls_210norm = COALESCE(sum_ls_210norm,0), " +
                " cnt_gils_pu = COALESCE(cnt_gils_pu,0), " +
                " cnt_gils_norm = COALESCE(cnt_gils_norm,0), " +
                " cnt_pl_norm = COALESCE(cnt_pl_norm,0), " +
                " cnt_pl_pu = COALESCE(cnt_pl_pu,0), " +
                " cnt_ls_a_val = COALESCE(cnt_ls_a_val,0), " +
                " sum_ls_a_val = COALESCE(sum_ls_a_val,0), " +
                " cnt_pl_a_pu = COALESCE(cnt_pl_a_pu,0), " +
                " cnt_ls_a_norm = COALESCE(cnt_ls_a_norm,0), " +
                " sum_ls_a_norm = COALESCE(sum_ls_a_norm,0), " +
                " cnt_pl_a_norm = COALESCE(cnt_pl_a_norm,0), " +
                " sum_dpu = COALESCE(sum_dpu,0), " +
                " val_begin = COALESCE(val_begin,0), " +
                " val_end = COALESCE(val_end,0), " +
                " ngp_cnt = COALESCE(ngp_cnt,0), " +
                " is_prev = COALESCE(is_prev,0)," +
                " ispp = COALESCE(ispp,0)," +
                " s_pgp = COALESCE(s_pgp,0)," +
                " s_pnp = COALESCE(s_pnp,0)," +
                " koef_gv = COALESCE(koef_gv,0)," +
                " tarif_ku = COALESCE(tarif_ku,0), " +
                " p1718 = COALESCE(p1718,0), " +
                " p28 = COALESCE(p28,0), " +
                " p1871 = COALESCE(p1871,0), " +
                " p1872 = COALESCE(p1872,0), " +
                " p1873 = COALESCE(p1873,0), " +
                " p1874 = COALESCE(p1874,0), " +
                " p1875 = COALESCE(p1875,0), " +
                " p1876 = COALESCE(p1876,0) ",
                tempTableNamePostfix);
            sqlExecutor.ExecuteSql(sqlQuery);

            sqlQuery = string.Format(
                " UPDATE tmp_dom_pu_{0}" +
                " SET " +
                " nzp_dom = COALESCE(nzp_dom,0), " +
                " nzp_ul = COALESCE(nzp_ul,0), " +
                " nzp_raj = COALESCE(nzp_raj,0), " +
                " nzp_town = COALESCE(nzp_town,0), " +
                " nzp_area = COALESCE(nzp_area,0), " +
                " mn_ = COALESCE(mn_,0), " +
                " yr_ = COALESCE(yr_,0), " +
                " nzp_serv = COALESCE(nzp_serv,0), " +
                " val_cnt = COALESCE(val_cnt,0) ",
                tempTableNamePostfix);
            sqlExecutor.ExecuteSql(sqlQuery);

            sqlQuery = string.Format(
                " UPDATE tmp_dolg1_{0} " +
                " SET nzp_serv = COALESCE(nzp_serv, 0), " +
                " nzp_supp = COALESCE(nzp_supp, 0), " +
                " year_ = COALESCE(year_, 0), " +
                " month_ = COALESCE(month_, 0), " +
                " sum_outsaldo = COALESCE(sum_outsaldo, 0), " +
                " sum_money_priv = COALESCE(sum_money_priv, 0), " +
                " sum_money_nanim = COALESCE(sum_money_nanim, 0), " +
                " sum_real_priv = COALESCE(sum_real_priv, 0), " +
                " sum_real_nanim = COALESCE(sum_real_nanim, 0), " +
                " dolg_priv = COALESCE(dolg_priv, 0), " +
                " dolg_nanim = COALESCE(dolg_nanim, 0), " +
                " sum_accrual_priv = COALESCE(sum_accrual_priv, 0), " +
                " sum_accrual_nanim = COALESCE(sum_accrual_nanim, 0), " +
                " sum_charge_priv = COALESCE(sum_charge_priv, 0), " +
                " sum_charge_nanim = COALESCE(sum_charge_nanim, 0), " +
                " sum_reval_priv = COALESCE(sum_reval_priv, 0), " +
                " sum_reval_nanim = COALESCE(sum_reval_nanim, 0) ",
                tempTableNamePostfix);
            sqlExecutor.ExecuteSql(sqlQuery);

            sqlQuery = string.Format(
                " UPDATE tmp_dolg2_{0} " +
                " SET nzp_serv = COALESCE(nzp_serv,0), " +
                " nzp_supp = COALESCE(nzp_supp,0)," +
                " pl_kv_p = COALESCE(pl_kv_p , 0)," +
                " pl_kv_n = COALESCE(pl_kv_n , 0)," +
                " cntls_all_priv = COALESCE(cntls_all_priv , 0)," +
                " cntls_all_nanim = COALESCE(cntls_all_nanim , 0)," +
                " pereplata_ls_p = COALESCE(pereplata_ls_p, 0), " +
                " pereplata_ls_n = COALESCE(pereplata_ls_n, 0)," +
                " pereplata_summ_p = COALESCE(pereplata_summ_p, 0)," +
                " pereplata_summ_n = COALESCE(pereplata_summ_n, 0)," +
                " cntlsdo_p = COALESCE(cntlsdo_p, 0), " +
                " cntlsdo_n = COALESCE(cntlsdo_n, 0), " +
                " summdo_p = COALESCE(summdo_p, 0), " +
                " summdo_n = COALESCE(summdo_n, 0), " +
                " cntls1_p = COALESCE(cntls1_p, 0), " +
                " cntls1_n = COALESCE(cntls1_n, 0), " +
                " summ1_p = COALESCE(summ1_p, 0), " +
                " summ1_n = COALESCE(summ1_n, 0), " +
                " cntls2_p = COALESCE(cntls2_p, 0), " +
                " cntls2_n = COALESCE(cntls2_n, 0), " +
                " summ2_p = COALESCE(summ2_p, 0), " +
                " summ2_n = COALESCE(summ2_n, 0), " +
                " cntls3_p = COALESCE(cntls3_p, 0), " +
                " cntls3_n = COALESCE(cntls3_n, 0), " +
                " summ3_p = COALESCE(summ3_p, 0), " +
                " summ3_n = COALESCE(summ3_n, 0); ",
                tempTableNamePostfix);
            sqlExecutor.ExecuteSql(sqlQuery);

            sqlQuery = string.Format(
                " UPDATE tmp_siroti_{0} " +
                " SET pl_kv = COALESCE(pl_kv,0), " +
                "   dolg_in = COALESCE(dolg_in,0), " +
                "   sum_real = COALESCE(sum_real,0), " +
                "   sum_money = COALESCE(sum_money,0), " +
                "   num_ls = COALESCE(num_ls,0), " +

                // косяк при выгрузке в файл, поэтому исправляется при загрузке
                "   dolg_out = COALESCE(dolg_in,0) +  COALESCE(sum_real,0) - COALESCE(sum_money,0); ",
                tempTableNamePostfix);
            sqlExecutor.ExecuteSql(sqlQuery);

            sqlQuery = string.Format(
                " UPDATE tmp_pu_{0}" +
                " SET year_ = COALESCE(year_ , 0),   " +
                " month_ = COALESCE(month_ , 0),   " +
                " nzp_serv = COALESCE(nzp_serv , 0),   " +
                " count_dpu = COALESCE(count_dpu , 0),   " +
                " count_nodpu = COALESCE(count_nodpu , 0),   " +
                " count_all = COALESCE(count_all , 0),   " +
                " count_all1 = COALESCE(count_all1 , 0),   " +
                " count_ipu = COALESCE(count_ipu , 0),   " +
                " count_noipu = COALESCE(count_noipu , 0),   " +
                " count_kv_all = COALESCE(count_kv_all , 0),   " +
                " count_dky = COALESCE(count_dky , 0)   ",
                tempTableNamePostfix);
            sqlExecutor.ExecuteSql(sqlQuery);

            sqlQuery = string.Format(
                " UPDATE tmp_pu_{0}" +
                " SET year_ = COALESCE(year_ , 0),   " +
                " month_ = COALESCE(month_ , 0),   " +
                " nzp_serv = COALESCE(nzp_serv , 0),   " +
                " count_dpu = COALESCE(count_dpu , 0),   " +
                " count_nodpu = COALESCE(count_nodpu , 0),   " +
                " count_all = COALESCE(count_all , 0),   " +
                " count_all1 = COALESCE(count_all1 , 0),   " +
                " count_ipu = COALESCE(count_ipu , 0),   " +
                " count_noipu = COALESCE(count_noipu , 0),   " +
                " count_kv_all = COALESCE(count_kv_all , 0),   " +
                " count_dky = COALESCE(count_dky , 0)   ",
                tempTableNamePostfix);
            sqlExecutor.ExecuteSql(sqlQuery);

            sqlQuery = string.Format(
                " UPDATE tmp_odn_{0}" +
                " SET year_ = COALESCE(year_ , 0),   " +
                " month_ = COALESCE(month_ , 0),   " +
                " nzp_dom = COALESCE(nzp_dom, 0), " +
                " count_ls = COALESCE(count_ls, 0), " +
                " nzp_type_alg = COALESCE(nzp_type_alg, 0), " +
                " sum_tarif = COALESCE(sum_tarif, 0), " +
                " typek = COALESCE(typek, 0), " +
                " count_all = COALESCE(count_all, 0), " +
                " rvaldlt = COALESCE(rvaldlt, 0), " +
                " tarif = COALESCE(tarif, 0), " +
                " c_calc = COALESCE(c_calc, 0) ",
                tempTableNamePostfix);
            sqlExecutor.ExecuteSql(sqlQuery);
        }

        private void DeleteOldData(
            SqlExecutor sqlExecutor,
            int nzpGraj,
            DateTime calculationMonth,
            string schemaName)
        {
            // в случае, если была попытка загрузить файл, который не соответствует формату
            if (nzpGraj == 0)
                return;
            var sqlQuery = string.Format(
                " UPDATE {0}.imports  " +
                " SET load_success = -1  " +
                " WHERE district_code = {1}  " +
                " AND calculation_date = '{2}'::DATE ",
                schemaName,
                this.districtCode,
                this.startCalculationMonth.ToShortDateString()
            );
            sqlExecutor.ExecuteSql(sqlQuery);

            try
            {
                sqlQuery = string.Format(
                    " DELETE FROM {0}.resp_siroti_{2}_{1}" +
                    " WHERE ye_ = {2} " +
                    " AND mn_ = {3}; ",
                    schemaName,
                    nzpGraj,
                    calculationMonth.Year,
                    calculationMonth.Month);
                sqlExecutor.ExecuteSql(sqlQuery);
            }
            catch (Exception e)
            {
                this.CheckException(e);
            }

            sqlQuery = string.Format(
                " DELETE FROM {0}.resp_opl_source" +
                " WHERE nzp_graj = {1} " +
                " AND dat_uchet >= '{2}' " +
                " AND dat_uchet < '{3}'; ",
                schemaName,
                nzpGraj,
                calculationMonth,
                calculationMonth.AddMonths(1));
            sqlExecutor.ExecuteSql(sqlQuery);

            sqlQuery = string.Format(
                " DELETE FROM {0}.resp_opl" +
                " WHERE nzp_graj = {1} " +
                " AND year_ = {2} " +
                " AND month_ = {3}; ",
                schemaName,
                nzpGraj,
                calculationMonth.Year,
                calculationMonth.Month);
            sqlExecutor.ExecuteSql(sqlQuery);

            sqlQuery = string.Format(
                " DELETE FROM {0}.resp_odn_source" +
                " WHERE nzp_graj = {1} " +
                " AND year_ = {2} " +
                " AND month_ = {3}; ",
                schemaName,
                nzpGraj,
                calculationMonth.Year,
                calculationMonth.Month);
            sqlExecutor.ExecuteSql(sqlQuery);

            sqlQuery = string.Format(
                " DELETE FROM {0}.resp_pu" +
                " WHERE nzp_graj = {1} " +
                " AND year_ = {2} " +
                " AND month_ = {3}; ",
                schemaName,
                nzpGraj,
                calculationMonth.Year,
                calculationMonth.Month);
            sqlExecutor.ExecuteSql(sqlQuery);

            sqlQuery = string.Format(
                " DELETE FROM {0}.resp_odn" +
                " WHERE nzp_graj = {1} " +
                " AND year_ = {2} " +
                " AND month_ = {3}; ",
                schemaName,
                nzpGraj,
                calculationMonth.Year,
                calculationMonth.Month);
            sqlExecutor.ExecuteSql(sqlQuery);

            sqlQuery = string.Format(
                " DELETE FROM {0}.resp_odn2" +
                " WHERE nzp_graj = {1} " +
                " AND year_ = {2} " +
                " AND month_ = {3}; ",
                schemaName,
                nzpGraj,
                calculationMonth.Year,
                calculationMonth.Month);
            sqlExecutor.ExecuteSql(sqlQuery);

            sqlQuery = string.Format(
                " DELETE FROM {0}.resp_odn3" +
                " WHERE nzp_graj = {1} " +
                " AND year_ = {2} " +
                " AND month_ = {3}; ",
                schemaName,
                nzpGraj,
                calculationMonth.Year,
                calculationMonth.Month);
            sqlExecutor.ExecuteSql(sqlQuery);

            sqlQuery = string.Format(
                " DELETE FROM {0}.resp_odn4" +
                " WHERE nzp_graj = {1} " +
                " AND year_ = {2} " +
                " AND month_ = {3}; ",
                schemaName,
                nzpGraj,
                calculationMonth.Year,
                calculationMonth.Month);
            sqlExecutor.ExecuteSql(sqlQuery);

            sqlQuery = string.Format(
                " DELETE FROM {0}.resp_nach" +
                " WHERE nzp_graj = {1} " +
                " AND year_ = {2} " +
                " AND month_ = {3}; ",
                schemaName,
                nzpGraj,
                calculationMonth.Year,
                calculationMonth.Month);
            sqlExecutor.ExecuteSql(sqlQuery);

            try
            {
                sqlQuery = string.Format(
                    " DELETE FROM {0}.raj{1}_odn" +
                    " WHERE nzp_graj = {1} " +
                    " AND year_ = {2} " +
                    " AND month_ = {3}; ",
                    schemaName,
                    nzpGraj,
                    calculationMonth.Year,
                    calculationMonth.Month);
                sqlExecutor.ExecuteSql(sqlQuery);
            }
            catch (Exception e)
            {
                this.CheckException(e);
            }

            try
            {
                sqlQuery = string.Format(
                    " DELETE FROM {0}.raj{1}_pu" +
                    " WHERE nzp_graj = {1} " +
                    " AND year_ = {2} " +
                    " AND month_ = {3}; ",
                    schemaName,
                    nzpGraj,
                    calculationMonth.Year,
                    calculationMonth.Month);
                sqlExecutor.ExecuteSql(sqlQuery);
            }
            catch (Exception e)
            {
                this.CheckException(e);
            }

            try
            {
                sqlQuery = string.Format(
                    " DELETE FROM {0}.raj{1}_dom_pu" +
                    " WHERE yr_ = {2} " +
                    " AND mn_ = {3}; ",
                    schemaName,
                    nzpGraj,
                    calculationMonth.Year,
                    calculationMonth.Month);
                sqlExecutor.ExecuteSql(sqlQuery);
            }
            catch (Exception e)
            {
                this.CheckException(e);
            }

            try
            {
                sqlQuery = string.Format(
                    " DELETE FROM {0}.raj{1}_dolg1" +
                    " WHERE nzp_graj = {1} " +
                    " AND year_ = {2} " +
                    " AND month_ = {3}; ",
                    schemaName,
                    nzpGraj,
                    calculationMonth.Year,
                    calculationMonth.Month);
                sqlExecutor.ExecuteSql(sqlQuery);
            }
            catch (Exception e)
            {
                this.CheckException(e);
            }

            try
            {
                sqlQuery = string.Format(
                    " DELETE FROM {0}.raj{1}_dolg1_typ" +
                    " WHERE nzp_graj = {1} " +
                    " AND year_ = {2} " +
                    " AND month_ = {3}; ",
                    schemaName,
                    nzpGraj,
                    calculationMonth.Year,
                    calculationMonth.Month);
                sqlExecutor.ExecuteSql(sqlQuery);
            }
            catch (Exception e)
            {
                this.CheckException(e);
            }

            try
            {
                sqlQuery = string.Format(
                    " DELETE FROM {0}.raj{1}_dolg2" +
                    " WHERE nzp_graj = {1} " +
                    " AND year_ = {2} " +
                    " AND month_ = {3}; ",
                    schemaName,
                    nzpGraj,
                    calculationMonth.Year,
                    calculationMonth.Month);
                sqlExecutor.ExecuteSql(sqlQuery);
            }
            catch (Exception e)
            {
                this.CheckException(e);
            }

            try
            {
                sqlQuery = string.Format(
                    " DELETE FROM {0}.raj{1}_dolg2_typ" +
                    " WHERE nzp_graj = {1} " +
                    " AND year_ = {2} " +
                    " AND month_ = {3}; ",
                    schemaName,
                    nzpGraj,
                    calculationMonth.Year,
                    calculationMonth.Month);
                sqlExecutor.ExecuteSql(sqlQuery);
            }
            catch (Exception e)
            {
                this.CheckException(e);
            }

            try
            {
                sqlQuery = string.Format(
                    " DELETE FROM {0}.raj{1}_nach" +
                    " WHERE nzp_graj = {1} " +
                    " AND yr_ = {2} " +
                    " AND mn_ = {3}; ",
                    schemaName,
                    nzpGraj,
                    calculationMonth.Year,
                    calculationMonth.Month);
                sqlExecutor.ExecuteSql(sqlQuery);
            }
            catch (Exception e)
            {
                this.CheckException(e);
            }

            try
            {
                sqlQuery = string.Format(
                    " DELETE FROM {0}.raj{1}_opl" +
                    " WHERE nzp_graj = {1} " +
                    " AND dat_uchet >= '{2}' " +
                    " AND dat_uchet < '{3}'; ",
                    schemaName,
                    nzpGraj,
                    calculationMonth,
                    calculationMonth.AddMonths(1));
                sqlExecutor.ExecuteSql(sqlQuery);
            }
            catch (Exception e)
            {
                this.CheckException(e);
            }
            
            try
            {
                sqlQuery = string.Format(
                    " DELETE FROM {0}.raj{1}_opl_serv" +
                    " WHERE nzp_graj = {1} " +
                    " AND dat_uchet >= '{2}' " +
                    " AND dat_uchet < '{3}'; ",
                    schemaName,
                    nzpGraj,
                    calculationMonth,
                    calculationMonth.AddMonths(1));
                sqlExecutor.ExecuteSql(sqlQuery);
            }
            catch (Exception e)
            {
                this.CheckException(e);
            }
        }

        /// <summary>
        /// Обработка и перенос данных из временных таблиц в основные
        /// </summary>
        /// <param name="sqlExecutor">Объект для работы с БД</param>
        /// <param name="nzpGraj">Код загружаемого района</param>
        /// <param name="startCalculationMonth">Начальный загружаемый расчетный месяц</param>
        /// <param name="endCalculationMonth">Конечный загружаемый расчетный месяц</param>
        /// <param name="schemaName">Наименование схемы, куда загружаются данные</param>
        private void TransferDataToMainTables(
            SqlExecutor sqlExecutor,
            int nzpGraj,
            DateTime startCalculationMonth,
            DateTime endCalculationMonth,
            string schemaName)
        {
            // берегите глаза, заходя в эти методы
            this.TransferStatOpl(sqlExecutor, nzpGraj, startCalculationMonth, endCalculationMonth, schemaName);
            this.importLog.AppendLine("Файл 'stat_opl.crp' успешно загружен.");
            
            this.TransferStatOpl(sqlExecutor, nzpGraj, startCalculationMonth, endCalculationMonth, schemaName, true);
            this.importLog.AppendLine("Файл 'stat_opl_serv.crp' успешно загружен.");

            this.TransferOdn(sqlExecutor, nzpGraj, startCalculationMonth, endCalculationMonth, schemaName);
            this.importLog.AppendLine("Файл 'odn.crp' успешно загружен.");

            this.TransferDomPu(sqlExecutor, nzpGraj, startCalculationMonth, schemaName);
            this.importLog.AppendLine("Файл 'dom_pu.crp' успешно загружен.");

            this.TransferPu(sqlExecutor, nzpGraj, startCalculationMonth, schemaName);
            this.importLog.AppendLine("Файл 'pu.crp' успешно загружен.");

            this.TransferDolg1(sqlExecutor, nzpGraj, startCalculationMonth, endCalculationMonth, schemaName);
            this.importLog.AppendLine("Файл 'dolg1.crp' успешно загружен.");

            this.TransferDolg2(sqlExecutor, nzpGraj, startCalculationMonth, endCalculationMonth, schemaName);
            this.importLog.AppendLine("Файл 'dolg2.crp' успешно загружен.");

            this.TransferSiroti(sqlExecutor, nzpGraj, startCalculationMonth, endCalculationMonth, schemaName);
            this.importLog.AppendLine("Файл 'siroti.crp' успешно загружен.");

            // жесть
            this.TransferNach(sqlExecutor, nzpGraj, startCalculationMonth, endCalculationMonth, schemaName);
            this.importLog.AppendLine("Файл 'nach.crp' успешно загружен.");

            // самый беспощадный метод..
            // берегись
            // не для слабонервных
            // здесь подготавливаются данные для отчетов
            this.Zhest(sqlExecutor, nzpGraj, startCalculationMonth, schemaName);
            this.importLog.AppendLine("Данные для отчетов успешно подготовлены.");
        }

        /// <summary>
        /// Обработка и перенос данных (пришедших из файла stat_opl.crp) из временных таблиц в основные
        /// </summary>
        /// <param name="sqlExecutor">Объект для работы с БД</param>
        /// <param name="nzpGraj">Код загружаемого района</param>
        /// <param name="startCalculationMonth">Начальный загружаемый расчетный месяц</param>
        /// <param name="endCalculationMonth">Конечный загружаемый расчетный месяц</param>
        /// <param name="schemaName">Наименование схемы, куда загружаются данные</param>
        /// <param name="loadServ">Загрузка оплат по услугам</param>
        private void TransferStatOpl(
            SqlExecutor sqlExecutor,
            int nzpGraj,
            DateTime startCalculationMonth,
            DateTime endCalculationMonth,
            string schemaName,
            bool loadServ = false)
        {
            // временная таблица, в которую ранее положили данные из файла stat_opl.crp
            var tempTableName = string.Format("tmp_{0}_{1}_{2}",
                $"stat_opl{(loadServ ? "_serv" : "")}",
                nzpGraj,
                startCalculationMonth.ToString("yyyyMM")
            );

            // полное название таблицы, в которую будут перенесены данные
            string tableFullName;

            string sqlQuery;
            {
                // Старт обработки данных
                tableFullName = $"{schemaName}.raj{nzpGraj}_opl{(loadServ ? "_serv" : "")}";

                // создаем партиционированную таблицу, унаследовавшись от таблицы raj_opl
                sqlQuery = string.Format(
                    " CREATE TABLE IF NOT EXISTS " +
                    " {1} " +
                    " (LIKE {0}.{2} INCLUDING ALL) " +
                    " INHERITS({0}.{2}) WITH (OIDS=TRUE) ",
                    ImportGkhDataForMinstroyReportsHandler.SchemaName,
                    tableFullName,
                    $"raj_opl{(loadServ ? "_serv" : "")}"
                );
                sqlExecutor.ExecuteSql(sqlQuery);

                // удаляем данные, которые были ранее по загружаемому периоду
                sqlQuery = string.Format(
                    " DELETE FROM {0} " +
                    " WHERE dat_uchet >= '{1}' " +
                    " AND dat_uchet <='{2}'",
                    tableFullName,
                    startCalculationMonth,
                    endCalculationMonth);

                // sqlExecutor.ExecuteSql(sqlQuery);
                sqlQuery = string.Format(
                    " INSERT INTO {0} " +
                    " (pref, nzp_graj, nzp_headbank, bank, nzp_bank, nzp_pack_ls, pkod, kod_sum, paysource, " +
                    $"  dat_uchet, dat_vvod, dat_month, sum_oplat{(loadServ ? ", type_serv, transfer" : "")})                                           " +
                    "  SELECT pref, nzp_graj, nzp_headbank, bank, nzp_bank, nzp_pack_ls, pkod, kod_sum,     " +
                    "         (CASE when (kod_sum = 41) AND (COALESCE(paysource,0)<2) " +
                    "               THEN 4                                       " +
                    "               ELSE COALESCE(paysource,0) end) as paysource,     " +
                    $"         dat_uchet, dat_vvod, dat_month, sum_oplat{(loadServ ? ", type_serv, transfer" : "")}" +
                    " FROM {1} ",
                    tableFullName,
                    tempTableName);
                sqlExecutor.ExecuteSql(sqlQuery);
            }

            if (!loadServ)
            {
                // переходим к следующей таблице
                tableFullName = string.Format("{0}.resp_opl_source", schemaName);

                // удаляем данные, которые были ранее по загружаемому периоду
                sqlQuery = string.Format(
                    " DELETE FROM {0} " +
                    " WHERE dat_uchet >= '{1}' " +
                    " AND dat_uchet <='{2}'" +
                    " AND nzp_graj={3}",
                    tableFullName,
                    startCalculationMonth,
                    endCalculationMonth,
                    nzpGraj);

                // sqlExecutor.ExecuteSql(sqlQuery);
                sqlQuery = string.Format(
                    " INSERT INTO {0} " +
                    " (nzp_graj, nzp_headbank, bank, nzp_bank, kod_sum, paysource, " +
                    "                        dat_uchet, dat_vvod, dat_month, count_kvit, sum_oplat)      " +
                    " SELECT nzp_graj, nzp_headbank, bank, nzp_bank, kod_sum, " +
                    "        (CASE when (kod_sum = 41) AND (COALESCE(paysource,0)<2) " +
                    "              THEN 4 " +
                    "              ELSE COALESCE(paysource,0) end) as paysource, " +
                    "        dat_uchet, dat_vvod, dat_month, count(nzp_pack_ls), sum(sum_oplat) " +
                    " FROM   {1} " +
                    " GROUP BY 1,2,3,4,5,6,7,8,9",
                    tableFullName,
                    tempTableName);
                sqlExecutor.ExecuteSql(sqlQuery);
            }

            if (!loadServ)
            {
                // переходим к следующей таблице
                tableFullName = string.Format("{0}.resp_opl", schemaName);

                // удаляем данные, которые были ранее по загружаемому периоду
                sqlQuery = string.Format(
                    " DELETE FROM {0} " +
                    " WHERE month_ + year_*12 >= " + (startCalculationMonth.Month + startCalculationMonth.Year * 12) +
                    " AND month_ + year_*12 <= " + (endCalculationMonth.Month + endCalculationMonth.Year * 12) +
                    " AND nzp_graj = {1}",
                    tableFullName,
                    nzpGraj);

                // sqlExecutor.ExecuteSql(sqlQuery);
                sqlQuery = string.Format(
                    " INSERT INTO {0} " +
                    " (nzp_graj, nzp_headbank, paysource, month_, year_, count_kvit, sum_oplat) " +
                    " SELECT nzp_graj, nzp_headbank, " +
                    "      (CASE when (kod_sum = 41) AND (COALESCE(paysource,0)<2) " +
                    "            THEN 4 " +
                    "            ELSE COALESCE(paysource,0) end) as paysource ," +
                    "       EXTRACT('MONTH' FROM dat_uchet),  EXTRACT('YEAR' FROM dat_uchet), count(nzp_pack_ls), sum(sum_oplat) " +
                    " FROM {1} " +
                    " GROUP BY 1,2,3,4,5",
                    tableFullName,
                    tempTableName);

                sqlExecutor.ExecuteSql(sqlQuery);
            }
        }

        /// <summary>
        /// Обработка и перенос данных (пришедших из файла odn.crp) из временных таблиц в основные
        /// </summary>
        /// <param name="sqlExecutor">Объект для работы с БД</param>
        /// <param name="nzpGraj">Код загружаемого района</param>
        /// <param name="startCalculationMonth">Начальный загружаемый расчетный месяц</param>
        /// <param name="endCalculationMonth">Конечный загружаемый расчетный месяц</param>
        /// <param name="schemaName">Наименование схемы, куда загружаются данные</param>
        private void TransferOdn(
            SqlExecutor sqlExecutor,
            int nzpGraj,
            DateTime startCalculationMonth,
            DateTime endCalculationMonth,
            string schemaName)
        {
            // временная таблица, в которую ранее положили данные из файла odn.crp
            var tempTableName = string.Format("tmp_{0}_{1}_{2}",
                "odn",
                nzpGraj,
                startCalculationMonth.ToString("yyyyMM")
            );

            // полное название таблицы, в которую будут перенесены данные
            string tableFullName;
            string sqlQuery;
            {
                tableFullName = string.Format("{0}.raj{1}_odn", schemaName, nzpGraj);

                // создаем партиционированную таблицу, унаследовавшись от таблицы raj_odn
                sqlQuery = string.Format(
                    " CREATE TABLE IF NOT EXISTS " +
                    " {1} " +
                    " (LIKE {0}.{2} INCLUDING ALL) " +
                    " INHERITS({0}.{2}) WITH (OIDS=TRUE) ",
                    ImportGkhDataForMinstroyReportsHandler.SchemaName,
                    tableFullName,
                    "raj_odn"
                );
                sqlExecutor.ExecuteSql(sqlQuery);

                // удаляем данные, которые были ранее по загружаемому периоду
                sqlQuery = string.Format(
                    " DELETE FROM {0} " +
                    " WHERE 0 < (" +
                    "   SELECT count(*) " +
                    "    FROM {1} " +
                    "   WHERE {1}.year_={0}.year_  " +
                    "    AND {1}.month_={0}.month_ " +
                    "    AND {1}.nzp_graj={0}.nzp_graj " +
                    "    AND {1}.pref={0}.pref ) ",
                    tableFullName,
                    tempTableName);

                // sqlExecutor.ExecuteSql(sqlQuery);
                sqlQuery = string.Format(
                    " INSERT INTO {0} " +
                    " (pref, nzp_graj, nzp_dom, year_, month_,town, rajon, " +
                    "   ulica, ndom, nkor, nzp_serv, count_ls, count_all, nzp_type_alg,  " +
                    "   sum_tarif, rvaldlt, tarif, c_calc) " +
                    " SELECT pref, nzp_graj, nzp_dom, year_, month_, town, rajon, " +
                    "   ulica, ndom, nkor, nzp_serv, count_ls, count_all, nzp_type_alg,  " +
                    "   sum_tarif, rvaldlt, tarif, c_calc " +
                    " FROM {1} ",
                    tableFullName,
                    tempTableName);
                sqlExecutor.ExecuteSql(sqlQuery);

                // Устанавливает ночное как дневное для 21 и 31 алгоритмов
                sqlQuery = string.Format(
                    " SELECT nzp_dom " +
                    " INTO TEMP t1 " +
                    " FROM {0} WHERE nzp_type_alg in (21,31) " +
                    " AND nzp_serv=25 ",
                    tempTableName);
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = string.Format(
                    " update {0} set nzp_serv = 25, count_ls = 0 " +
                    " WHERE nzp_serv=210 AND nzp_dom in (SELECT nzp_dom FROM t1)",
                    tempTableName);
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = string.Format(" DROP TABLE IF EXISTS  t1 ");
                sqlExecutor.ExecuteSql(sqlQuery);
            }
            {
                // переходим к следующей таблице
                tableFullName = string.Format("{0}.resp_odn_source", schemaName);

                sqlQuery = string.Format(
                    " SELECT nzp_dom, max(count_all) as max_ls " +
                    " INTO TEMP t_max " +
                    " FROM {0} WHERE typek=1 " +
                    " GROUP BY 1"
                    ,
                    tempTableName);

                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = string.Format(
                    " SELECT pref, nzp_graj, year_, month_, " +
                    " (CASE when nzp_serv = 510 THEN 6 " +
                    "       when nzp_serv = 511 THEN 7 " +
                    "       when nzp_serv = 512 THEN 8 " +
                    "       when nzp_serv = 513 THEN 9 " +
                    "       when nzp_serv = 514 THEN 14 " +
                    "       when nzp_serv = 515 THEN 25 " +
                    "       when nzp_serv = 516 THEN 210 ELSE nzp_serv end) as nzp_serv, a.nzp_dom, " +
                    " sum(CASE when nzp_serv < 500 THEN count_ls ELSE 0 end) as count_ls, " +
                    " sum(sum_tarif) as sum_tarif, " +
                    " sum(CASE when nzp_serv >500 THEN sum_tarif ELSE 0 end) as sum_odn, " +
                    " sum(rvaldlt) as rvaldlt, " +
                    " sum(tarif)   as tarif,   " +
                    " sum(c_calc)  as c_calc,  " +
                    " sum(CASE when c_calc > 0 AND rvaldlt > c_calc " +
                    "          THEN ((sum_tarif/c_calc)*(rvaldlt-c_calc)) ELSE 0 end ) as nodn " +
                    " INTO TEMP t2   " +
                    " FROM  {0} a, t_max b  " +
                    " WHERE typek = 1 AND a.nzp_dom = b.nzp_dom AND max_ls >= 2 " +
                    " GROUP BY 1,2,3,4,5,6 ",
                    tempTableName);
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = string.Format(" DROP TABLE IF EXISTS t_max ");
                sqlExecutor.ExecuteSql(sqlQuery);

                // удаляем данные, которые были ранее по загружаемому периоду
                sqlQuery = string.Format(
                    " DELETE FROM {0} " +
                    " WHERE 0 < (" +
                    "   SELECT count(*) " +
                    "    FROM {1} " +
                    "   WHERE {1}.year_={0}.year_  " +
                    "    AND {1}.month_={0}.month_ " +
                    "    AND {1}.nzp_graj={0}.nzp_graj " +
                    "    AND {1}.pref={0}.pref ) ",
                    tableFullName,
                    "t2");

                // sqlExecutor.ExecuteSql(sqlQuery);
                sqlQuery = string.Format(
                    " INSERT INTO {0} " +
                    "  (pref, nzp_graj, year_, month_, nzp_serv, nzp_dom, count_ls, " +
                    "  sum_tarif, sum_odn, rvaldlt, tarif, c_calc )                " +
                    " SELECT pref, nzp_graj, year_, month_, nzp_serv, nzp_dom, count_ls, sum_tarif, sum_odn, " +
                    "  rvaldlt, tarif, c_calc " +
                    " FROM   {1} ",
                    tableFullName,
                    "t2");
                sqlExecutor.ExecuteSql(sqlQuery);
            }
            {
                // переходим к следующей таблице
                tableFullName = string.Format("{0}.resp_odn", schemaName);

                // удаляем данные, которые были ранее по загружаемому периоду
                sqlQuery = string.Format(
                    " DELETE FROM {0} " +
                    " WHERE month_ + year_*12 >= " + (startCalculationMonth.Month + startCalculationMonth.Year * 12) +
                    " AND month_ + year_*12 <= " + (endCalculationMonth.Month + endCalculationMonth.Year * 12) +
                    " AND nzp_graj = {1}",
                    tableFullName,
                    nzpGraj);

                // sqlExecutor.ExecuteSql(sqlQuery);
                sqlQuery = string.Format(
                    " INSERT INTO {0} " +
                    " (nzp_graj, year_, month_, nzp_serv, kod_, count_dom, count_ls," +
                    "                        sum_tarif, sum_odn, rvaldlt, tarif, c_calc)                  " +
                    " SELECT nzp_graj, year_, month_, nzp_serv, " +
                    " (CASE when sum_odn=0 THEN 1 " +
                    "       when sum_odn/sum_tarif >0 AND sum_odn/sum_tarif <0.05 THEN 2 " +
                    "       when sum_odn/sum_tarif >=0.05 AND sum_odn/sum_tarif <0.10 THEN 3" +
                    "       when sum_odn/sum_tarif >=0.10 AND sum_odn/sum_tarif <0.15 THEN 4" +
                    "       when sum_odn/sum_tarif >=0.15 AND sum_odn/sum_tarif <0.20 THEN 5" +
                    "       when sum_odn/sum_tarif >=0.20 AND sum_odn/sum_tarif <0.25 THEN 6" +
                    "       when sum_odn/sum_tarif >=0.25 THEN 7 ELSE 0 end) as kod, " +
                    " count(nzp_dom) as count_dom , " +
                    " sum(count_ls) as count_ls, " +
                    " sum(sum_tarif) as sum_tarif, " +
                    " sum(sum_odn) as sum_odn, " +
                    " sum(rvaldlt) as rvaldlt, " +
                    " sum(tarif) as tarif,    " +
                    " sum(c_calc) as c_calc   " +
                    " FROM  {1} " +
                    " WHERE sum_tarif>0.0001 GROUP BY 1,2,3,4,5",
                    tableFullName,
                    "t2");
                sqlExecutor.ExecuteSql(sqlQuery);
            }
            {
                // переходим к следующей таблице
                tableFullName = string.Format("{0}.resp_odn2", schemaName);

                // удаляем данные, которые были ранее по загружаемому периоду
                sqlQuery = string.Format(
                    " DELETE FROM {0} " +
                    " WHERE month_ + year_*12 >= " + (startCalculationMonth.Month + startCalculationMonth.Year * 12) +
                    " AND month_ + year_*12 <= " + (endCalculationMonth.Month + endCalculationMonth.Year * 12) +
                    " AND nzp_graj = {1}",
                    tableFullName,
                    nzpGraj);

                // sqlExecutor.ExecuteSql(sqlQuery);
                sqlQuery = string.Format(
                    " INSERT INTO {0} " +
                    " (nzp_graj, year_, month_,  nzp_serv, kod_, count_dom, count_ls, " +
                    "                        sum_tarif, sum_odn, rvaldlt, tarif, c_calc, nodn)              " +
                    " SELECT nzp_graj, year_, month_, nzp_serv, " +
                    " (CASE when sum_odn=0 THEN 1 " +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn) > 0 AND       " +
                    "            (sum_odn+nodn)/(sum_tarif+nodn) < 0.05 THEN 2 " +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn) >=0.05 AND    " +
                    "            (sum_odn+nodn)/(sum_tarif+nodn) < 0.10 THEN 3 " +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn) >=0.10 AND    " +
                    "            (sum_odn+nodn)/(sum_tarif+nodn) < 0.15 THEN 4 " +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn) >=0.15 AND    " +
                    "            (sum_odn+nodn)/(sum_tarif+nodn) < 0.20 THEN 5 " +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn) >=0.20 AND    " +
                    "            (sum_odn+nodn)/(sum_tarif+nodn) < 0.25 THEN 6 " +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn)>=0.25 THEN 7 ELSE 0 end) as kod, " +
                    " count(nzp_dom) as count_dom,  " +
                    " sum(count_ls) as count_ls,    " +
                    " sum(sum_tarif) as sum_tarif,  " +
                    " sum(sum_odn) as sum_odn, " +
                    " sum(rvaldlt) as rvaldlt, " +
                    " sum(tarif) as tarif,     " +
                    " sum(c_calc) as c_calc,   " +
                    " sum(nodn) as nodn        " +
                    " FROM {1}                  " +
                    " WHERE sum_tarif>0.0001   " +
                    " GROUP BY 1,2,3,4,5",
                    tableFullName,
                    "t2");
                sqlExecutor.ExecuteSql(sqlQuery);
            }
            {
                // переходим к следующей таблице
                tableFullName = string.Format("{0}.resp_odn3", schemaName);

                // удаляем данные, которые были ранее по загружаемому периоду
                sqlQuery = string.Format(
                    " DELETE FROM {0} " +
                    " WHERE month_ + year_*12 >= " + (startCalculationMonth.Month + startCalculationMonth.Year * 12) +
                    " AND month_ + year_*12 <= " + (endCalculationMonth.Month + endCalculationMonth.Year * 12) +
                    " AND nzp_graj = {1}",
                    tableFullName,
                    nzpGraj);

                // sqlExecutor.ExecuteSql(sqlQuery);
                sqlQuery = string.Format(
                    " INSERT INTO {0} " +
                    " (nzp_graj, year_, month_,  nzp_serv, kod_, count_dom, count_ls, " +
                    "                        sum_tarif, sum_odn, rvaldlt, tarif, c_calc, nodn)              " +
                    " SELECT nzp_graj, year_, month_, nzp_serv, " +
                    " (CASE when sum_odn=0 THEN 1 " +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn) > 0 AND       " +
                    "            (sum_odn+nodn)/(sum_tarif+nodn) < 0.05 THEN 2 " +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn) >=0.05 AND " +
                    "            (sum_odn+nodn)/(sum_tarif+nodn) < 0.10 THEN 3" +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn) >=0.10 AND " +
                    "            (sum_odn+nodn)/(sum_tarif+nodn) < 0.15 THEN 4" +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn) >=0.15 AND " +
                    "            (sum_odn+nodn)/(sum_tarif+nodn) < 0.20 THEN 5" +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn) >=0.20 AND " +
                    "            (sum_odn+nodn)/(sum_tarif+nodn) < 0.25 THEN 6" +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn)>=0.25 THEN 7 ELSE 0 end) as kod, " +
                    " count(nzp_dom) as count_dom , " +
                    " sum(count_ls) as count_ls,    " +
                    " sum(sum_tarif) as sum_tarif,  " +
                    " sum(sum_odn) as sum_odn, " +
                    " sum(rvaldlt) as rvaldlt, " +
                    " sum(tarif) as tarif,     " +
                    " sum(c_calc) as c_calc,   " +
                    " sum(nodn) as nodn        " +
                    " FROM {1} " +
                    " WHERE sum_tarif>0.0001 AND rvaldlt > 0 " +
                    " GROUP BY 1,2,3,4,5 ",
                    tableFullName,
                    "t2");
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlExecutor.ExecuteSql(" DROP TABLE  IF EXISTS t2 ");
            }
        }

        /// <summary>
        /// Обработка и перенос данных (пришедших из файла pu.crp) из временных таблиц в основные
        /// </summary>
        /// <param name="sqlExecutor">Объект для работы с БД</param>
        /// <param name="nzpGraj">Код загружаемого района</param>
        /// <param name="startCalculationMonth">Начальный загружаемый расчетный месяц</param>
        /// <param name="schemaName">Наименование схемы, куда загружаются данные</param>
        private void TransferPu(SqlExecutor sqlExecutor, int nzpGraj, DateTime startCalculationMonth, string schemaName)
        {
            // временная таблица, в которую ранее положили данные из файла pu.crp
            var tempTableName = string.Format("tmp_{0}_{1}_{2}",
                "pu",
                nzpGraj,
                startCalculationMonth.ToString("yyyyMM")
            );

            // полное название таблицы, в которую будут перенесены данные
            string tableFullName;
            string sqlQuery;
            {
                tableFullName = string.Format("{0}.raj{1}_pu", schemaName, nzpGraj);

                // создаем партиционированную таблицу, унаследовавшись от таблицы raj_pu
                sqlQuery = string.Format(
                    " CREATE TABLE IF NOT EXISTS " +
                    " {1} " +
                    " (LIKE {0}.{2} INCLUDING ALL) " +
                    " INHERITS({0}.{2}) WITH (OIDS=TRUE) ",
                    schemaName,
                    tableFullName,
                    "raj_pu"
                );
                sqlExecutor.ExecuteSql(sqlQuery);

                // удаляем данные, которые были ранее по загружаемому периоду
                sqlQuery = string.Format(
                    " DELETE FROM {0} " +
                    " WHERE 0 < (" +
                    "   SELECT count(*) " +
                    "    FROM {1} " +
                    "   WHERE {1}.year_={0}.year_  " +
                    "    AND {1}.month_={0}.month_ " +
                    "    AND {1}.nzp_graj={0}.nzp_graj " +
                    " ) ",
                    tableFullName,
                    tempTableName);

                // sqlExecutor.ExecuteSql(sqlQuery);
                sqlQuery = string.Format(
                    " INSERT INTO {0} " +
                    " (graj, nzp_graj, year_, month_, service, nzp_serv, " +
                    "  count_dpu, count_nodpu, count_all, count_all1,    " +
                    "  count_ipu, count_noipu, count_kv_all, count_dky)  " +
                    " SELECT graj, nzp_graj, year_, month_, service, nzp_serv,      " +
                    "  count_dpu, count_nodpu, count_all, count_all1,         " +
                    "  count_ipu, count_noipu, count_kv_all, count_dky        " +
                    " FROM {1} ",
                    tableFullName,
                    tempTableName);
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = string.Format(
                    " update {1}                                     " +
                    " set graj = (                                                        " +
                    "             SELECT max(point)                                       " +
                    "             FROM   {0}.s_point a                    " +
                    "             WHERE  a.nzp_graj = {1}.nzp_graj ) " +
                    " WHERE graj is null ",
                    schemaName,
                    tableFullName);
                sqlExecutor.ExecuteSql(sqlQuery);
            }
        }

        /// <summary>
        /// Обработка и перенос данных (пришедших из файла dom_pu.crp) из временных таблиц в основные
        /// </summary>
        /// <param name="sqlExecutor">Объект для работы с БД</param>
        /// <param name="nzpGraj">Код загружаемого района</param>
        /// <param name="startCalculationMonth">Загружаемый расчетный месяц</param>
        /// <param name="schemaName">Наименование схемы, куда загружаются данные</param>
        private void TransferDomPu(SqlExecutor sqlExecutor, int nzpGraj, DateTime startCalculationMonth, string schemaName)
        {
            // временная таблица, в которую ранее положили данные из файла pu.crp
            var tempTableName = $"tmp_dom_pu_{nzpGraj}_{startCalculationMonth.ToString("yyyyMM")}";
            {
                // полное название таблицы, в которую будут перенесены данные
                string tableFullName = $"{schemaName}.raj{nzpGraj}_dom_pu";

                // создаем партиционированную таблицу, унаследовавшись от таблицы raj_dom_pu
                var sqlQuery = string.Format(
                    " CREATE TABLE IF NOT EXISTS " +
                    " {1} " +
                    " (LIKE {0}.{2} INCLUDING ALL) " +
                    " INHERITS({0}.{2}) WITH (OIDS=TRUE) ",
                    schemaName,
                    tableFullName,
                    "raj_dom_pu"
                );
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery =
                    $@" INSERT INTO {tableFullName} 
                            (erc, nzp_dom, prf, nzp_ul, ulica, ndom, nkor, nzp_raj, rajon, nzp_town, 
                            town, nzp_area, area, mn_, yr_, nzp_serv, num_cnt, dat_provnext, dat_uchet, val_cnt, nzp_graj) 
                    SELECT  erc, nzp_dom, prf, nzp_ul, ulica, ndom, nkor, nzp_raj, rajon, nzp_town,  
                            town, nzp_area, area, mn_, yr_, nzp_serv, num_cnt, dat_provnext, dat_uchet, val_cnt, nzp_graj 
                    FROM {tempTableName} ";
                sqlExecutor.ExecuteSql(sqlQuery);
            }
        }

        /// <summary>
        /// Обработка и перенос данных (пришедших из файла dolg1.crp) из временных таблиц в основные
        /// </summary>
        /// <param name="sqlExecutor">Объект для работы с БД</param>
        /// <param name="nzpGraj">Код загружаемого района</param>
        /// <param name="startCalculationMonth">Начальный загружаемый расчетный месяц</param>
        /// <param name="endCalculationMonth">Конечный загружаемый расчетный месяц</param>
        /// <param name="schemaName">Наименование схемы, куда загружаются данные</param>
        private void TransferDolg1(
            SqlExecutor sqlExecutor,
            int nzpGraj,
            DateTime startCalculationMonth,
            DateTime endCalculationMonth,
            string schemaName)
        {
            // временная таблица, в которую ранее положили данные из файла dolg1.crp
            var tempTableName = string.Format("tmp_{0}_{1}_{2}",
                "dolg1",
                nzpGraj,
                startCalculationMonth.ToString("yyyyMM")
            );

            // полное название таблицы, в которую будут перенесены данные
            string tableFullName;
            string sqlQuery;
            {
                tableFullName = string.Format("{0}.raj{1}_dolg1", schemaName, nzpGraj);

                // создаем партиционированную таблицу, унаследовавшись от таблицы raj_dolg1
                sqlQuery = string.Format(
                    " CREATE TABLE IF NOT EXISTS " +
                    " {1} " +
                    " (LIKE {0}.{2} INCLUDING ALL) " +
                    " INHERITS({0}.{2}) WITH (OIDS=TRUE) ",
                    ImportGkhDataForMinstroyReportsHandler.SchemaName,
                    tableFullName,
                    "raj_dolg1"
                );
                sqlExecutor.ExecuteSql(sqlQuery);

                // удаляем данные, которые были ранее по загружаемому периоду
                sqlQuery = string.Format(
                    " DELETE FROM {0} " +
                    " WHERE 0 < (" +
                    "   SELECT count(*) " +
                    "    FROM {1} " +
                    "   WHERE {1}.year_={0}.year_  " +
                    "    AND {1}.month_={0}.month_ " +
                    "    AND {1}.nzp_graj={0}.nzp_graj " +
                    " ) ",
                    tableFullName,
                    tempTableName);

                // sqlExecutor.ExecuteSql(sqlQuery);

                // sum_accrual_priv, sum_charge_priv, sum_reval_priv
                sqlQuery =
                    $@" INSERT INTO {tableFullName} 
                    (nzp_graj, service, name_supp, nzp_serv, nzp_supp, year_, month_, 
                    sum_outsaldo, sum_money, sum_real_prev, dolg, 
                    sum_accrual, sum_charge, sum_reval) 
                    SELECT nzp_graj, service, name_supp, nzp_serv, nzp_supp, year_, month_, 
                    sum_outsaldo, sum_money_priv + sum_money_nanim, sum_real_priv + sum_real_nanim, dolg_priv + dolg_nanim,
                    sum_accrual_priv + sum_accrual_nanim, sum_charge_priv + sum_charge_nanim, sum_reval_priv + sum_reval_nanim
                    FROM {tempTableName}";
                sqlExecutor.ExecuteSql(sqlQuery);
            }
            {
                // переходим к следующей таблице
                tableFullName = string.Format("{0}.raj{1}_dolg1_typ", schemaName, nzpGraj);

                // создаем партиционированную таблицу, унаследовавшись от таблицы raj_opl
                sqlQuery = string.Format(
                    " CREATE TABLE IF NOT EXISTS " +
                    " {1} " +
                    " (LIKE {0}.{2} INCLUDING ALL) " +
                    " INHERITS({0}.{2}) WITH (OIDS=TRUE) ",
                    ImportGkhDataForMinstroyReportsHandler.SchemaName,
                    tableFullName,
                    "raj_dolg1_typ"
                );
                sqlExecutor.ExecuteSql(sqlQuery);

                // удаляем данные, которые были ранее по загружаемому периоду
                sqlQuery = string.Format(
                    " DELETE FROM {0} " +
                    " WHERE 0 < (" +
                    "   SELECT count(*) " +
                    "    FROM {1} " +
                    "   WHERE {1}.year_={0}.year_  " +
                    "    AND {1}.month_={0}.month_ " +
                    "    AND {1}.nzp_graj={0}.nzp_graj " +
                    " ) ",
                    tableFullName,
                    tempTableName);

                // sqlExecutor.ExecuteSql(sqlQuery);
                sqlQuery =
                    $@" INSERT INTO {tableFullName}
                    (nzp_graj, service, name_supp, nzp_serv, nzp_supp, year_, month_,
                    sum_outsaldo, sum_money_p, sum_money_n, sum_real_prev_p,
                    sum_real_prev_n, dolg_p, dolg_n,
                    sum_accrual_p, sum_accrual_n, sum_charge_p, sum_charge_n, sum_reval_p, sum_reval_n)
                    SELECT nzp_graj, service, name_supp, nzp_serv, nzp_supp, year_, month_,
                    sum_outsaldo, sum_money_priv, sum_money_nanim, sum_real_priv, 
                    sum_real_nanim, dolg_priv, dolg_nanim,
                    sum_accrual_priv, sum_accrual_nanim, sum_charge_priv, sum_charge_nanim, sum_reval_priv, sum_reval_nanim
                    FROM {tempTableName}";
                sqlExecutor.ExecuteSql(sqlQuery);
            }
        }

        /// <summary>
        /// Обработка и перенос данных (пришедших из файла dolg2.crp) из временных таблиц в основные
        /// </summary>
        /// <param name="sqlExecutor">Объект для работы с БД</param>
        /// <param name="nzpGraj">Код загружаемого района</param>
        /// <param name="startCalculationMonth">Начальный загружаемый расчетный месяц</param>
        /// <param name="endCalculationMonth">Конечный загружаемый расчетный месяц</param>
        /// <param name="schemaName">Наименование схемы, куда загружаются данные</param>
        private void TransferDolg2(
            SqlExecutor sqlExecutor,
            int nzpGraj,
            DateTime startCalculationMonth,
            DateTime endCalculationMonth,
            string schemaName)
        {
            // временная таблица, в которую ранее положили данные из файла dolg1.crp
            var tempTableName = string.Format("tmp_{0}_{1}_{2}",
                "dolg2",
                nzpGraj,
                startCalculationMonth.ToString("yyyyMM")
            );

            // полное название таблицы, в которую будут перенесены данные
            string tableFullName;
            string sqlQuery;
            {
                tableFullName = string.Format("{0}.raj{1}_dolg2", schemaName, nzpGraj);

                // создаем партиционированную таблицу, унаследовавшись от таблицы raj_dolg2
                sqlQuery = string.Format(
                    " CREATE TABLE IF NOT EXISTS " +
                    " {1} " +
                    " (LIKE {0}.{2} INCLUDING ALL) " +
                    " INHERITS({0}.{2}) WITH (OIDS=TRUE) ",
                    ImportGkhDataForMinstroyReportsHandler.SchemaName,
                    tableFullName,
                    "raj_dolg2"
                );
                sqlExecutor.ExecuteSql(sqlQuery);

                // удаляем данные, которые были ранее по загружаемому периоду
                sqlQuery = string.Format(
                    " DELETE FROM {0} " +
                    " WHERE 0 < (" +
                    "   SELECT count(*) " +
                    "    FROM {1} " +
                    "   WHERE {1}.year_={0}.year_  " +
                    "    AND {1}.month_={0}.month_ " +
                    "    AND {1}.nzp_graj={0}.nzp_graj " +
                    " ) ",
                    tableFullName,
                    tempTableName);

                // sqlExecutor.ExecuteSql(sqlQuery);
                sqlQuery = string.Format(
                    " INSERT INTO {0} " +
                    " (nzp_graj, service, name_supp, nzp_serv, nzp_supp, year_, month_, " +
                    "                        cntls_all,  pereplata_ls, pereplata_summ, cntlsdo, summdo, " +
                    "                        cntls1, summ1, cntls2, summ2, cntls3, summ3) " +
                    " SELECT nzp_graj, service, name_supp, nzp_serv, nzp_supp, year_, month_,        " +
                    "        cntls_all_priv+cntls_all_nanim, pereplata_ls_p+pereplata_ls_n,          " +
                    "        pereplata_summ_p+pereplata_summ_n, cntlsdo_p+cntlsdo_n, summdo_p+summdo_n,  " +
                    "        cntls1_p+cntls1_n, summ1_p+summ1_n, cntls2_p+cntls2_n, summ2_p+summ2_n, " +
                    "        cntls3_p+cntls3_n, summ3_p+summ3_n                                      " +
                    " FROM {1} ",
                    tableFullName,
                    tempTableName);
                sqlExecutor.ExecuteSql(sqlQuery);
            }
            {
                // переходим к следующей таблице
                tableFullName = string.Format("{0}.raj{1}_dolg2_typ", schemaName, nzpGraj);

                // создаем партиционированную таблицу, унаследовавшись от таблицы raj_opl
                sqlQuery = string.Format(
                    " CREATE TABLE IF NOT EXISTS " +
                    " {1} " +
                    " (LIKE {0}.{2} INCLUDING ALL) " +
                    " INHERITS({0}.{2}) WITH (OIDS=TRUE) ",
                    ImportGkhDataForMinstroyReportsHandler.SchemaName,
                    tableFullName,
                    "raj_dolg2_typ"
                );
                sqlExecutor.ExecuteSql(sqlQuery);

                // удаляем данные, которые были ранее по загружаемому периоду
                sqlQuery = string.Format(
                    " DELETE FROM {0} " +
                    " WHERE 0 < (" +
                    "   SELECT count(*) " +
                    "    FROM {1} " +
                    "   WHERE {1}.year_={0}.year_  " +
                    "    AND {1}.month_={0}.month_ " +
                    "    AND {1}.nzp_graj={0}.nzp_graj " +
                    " ) ",
                    tableFullName,
                    tempTableName);

                // sqlExecutor.ExecuteSql(sqlQuery);
                sqlQuery = string.Format(
                    " INSERT INTO {0} " +
                    " (nzp_graj, service, name_supp, nzp_serv, nzp_supp, year_, month_, " +
                    "                        pl_kv_p, pl_kv_n, cntls_all_p, cntls_all_n, pereplata_ls_p, " +
                    "                        pereplata_ls_n, " +
                    "                        pereplata_summ_p, pereplata_summ_n, cntlsdo_p, cntlsdo_n, " +
                    "                        summdo_p, summdo_n, cntls1_p, cntls1_n, summ1_p, summ1_n, " +
                    "                        cntls2_p, cntls2_n, summ2_p, summ2_n, cntls3_p, cntls3_n, " +
                    "                        summ3_p, summ3_n) " +
                    " SELECT nzp_graj, service, name_supp, nzp_serv, nzp_supp, year_, month_, pl_kv_p, pl_kv_n, " +
                    "        cntls_all_priv, cntls_all_nanim, pereplata_ls_p, pereplata_ls_n, " +
                    "        pereplata_summ_p, pereplata_summ_n, cntlsdo_p, cntlsdo_n, summdo_p, summdo_n, " +
                    "        cntls1_p, cntls1_n, summ1_p, summ1_n,  " +
                    "        cntls2_p, cntls2_n, summ2_p, summ2_n, cntls3_p, cntls3_n, summ3_p, summ3_n       " +
                    " FROM {1} ",
                    tableFullName,
                    tempTableName);
                sqlExecutor.ExecuteSql(sqlQuery);
            }
        }

        /// <summary>
        /// Обработка и перенос данных (пришедших из файла siroti.crp) из временных таблиц в основные
        /// </summary>
        /// <param name="sqlExecutor">Объект для работы с БД</param>
        /// <param name="nzpGraj">Код загружаемого района</param>
        /// <param name="startCalculationMonth">Начальный загружаемый расчетный месяц</param>
        /// <param name="endCalculationMonth">Конечный загружаемый расчетный месяц</param>
        /// <param name="schemaName">Наименование схемы, куда загружаются данные</param>
        private void TransferSiroti(
            SqlExecutor sqlExecutor,
            int nzpGraj,
            DateTime startCalculationMonth,
            DateTime endCalculationMonth,
            string schemaName)
        {
            // временная таблица, в которую ранее положили данные из файла siroti.crp
            var tempTableName = string.Format("tmp_{0}_{1}_{2}",
                "siroti",
                nzpGraj,
                startCalculationMonth.ToString("yyyyMM")
            );

            // полное название таблицы, в которую будут перенесены данные
            string tableFullName;
            string sqlQuery;
            {
                var parentTableFullName = string.Format("{0}.resp_siroti_{1}", schemaName, startCalculationMonth.Year);

                // создаем таблицу для загружаемого года
                sqlQuery = string.Format(
                    " CREATE TABLE IF NOT EXISTS {0}" +
                    " (                                 " +
                    "   ye_ integer,                    " +
                    "   mn_ integer,                    " +
                    "   rajon character(100),           " +
                    "   fio character(250),             " +
                    "   adress character(50),           " +
                    "   kol_gil integer,                " +
                    "   pl_kv numeric(14,2),            " +
                    "   nzp_serv integer,               " +
                    "   dolg_in numeric(14,2),          " +
                    "   sum_real numeric(14,2),         " +
                    "   sum_money numeric(14,2),        " +
                    "   dolg_out numeric(14,2),          " +
                    "   num_ls integer          " +
                    " )",
                    parentTableFullName
                );
                sqlExecutor.ExecuteSql(sqlQuery);

                tableFullName = string.Format("{0}_{1}", parentTableFullName, nzpGraj);

                // создаем партиционированную таблицу, унаследовавшись от таблицы resp_siroti_YYYY
                sqlQuery = string.Format(
                    " CREATE TABLE IF NOT EXISTS " +
                    " {0} " +
                    " (LIKE {1} INCLUDING ALL) " +
                    " INHERITS({1}) WITH (OIDS=TRUE) ",
                    tableFullName,
                    parentTableFullName
                );
                sqlExecutor.ExecuteSql(sqlQuery);

                // удаляем данные, которые были ранее по загружаемому периоду
                sqlQuery = string.Format(
                    " DELETE FROM {0} " +
                    " WHERE 0 < (" +
                    "   SELECT count(*) " +
                    "    FROM {1} " +
                    "   WHERE {1}.ye_={0}.ye_  " +
                    "    AND {1}.mn_={0}.mn_ " +
                    "    AND {1}.rajon={0}.rajon " +
                    " ) ",
                    tableFullName,
                    tempTableName);

                // sqlExecutor.ExecuteSql(sqlQuery);
                sqlQuery = string.Format(
                    " INSERT INTO {0} " +
                    " (ye_, mn_, rajon, fio, adress, kol_gil, pl_kv,       " +
                    "                        nzp_serv,  dolg_in, sum_real, sum_money, dolg_out, num_ls)  " +
                    " SELECT ye_, mn_, rajon, fio, adress, kol_gil, pl_kv, " +
                    "        nzp_serv,  dolg_in, sum_real, sum_money, dolg_out, num_ls " +
                    " FROM {1} ",
                    tableFullName,
                    tempTableName);
                sqlExecutor.ExecuteSql(sqlQuery);
            }
        }

        /// <summary>
        /// Обработка и перенос данных (пришедших из файла nach.crp) из временных таблиц в основные
        /// </summary>
        /// <param name="sqlExecutor">Объект для работы с БД</param>
        /// <param name="nzpGraj">Код загружаемого района</param>
        /// <param name="startCalculationMonth">Начальный загружаемый расчетный месяц</param>
        /// <param name="endCalculationMonth">Конечный загружаемый расчетный месяц</param>
        /// <param name="schemaName">Наименование схемы, куда загружаются данные</param>
        private void TransferNach(
            SqlExecutor sqlExecutor,
            int nzpGraj,
            DateTime startCalculationMonth,
            DateTime endCalculationMonth,
            string schemaName)
        {
            // временная таблица, в которую ранее положили данные из файла nach.crp
            var tempTableName = string.Format("tmp_{0}_{1}_{2}",
                "nach",
                nzpGraj,
                startCalculationMonth.ToString("yyyyMM")
            );

            // полное название таблицы, в которую будут перенесены данные
            string tableFullName;
            string sqlQuery;
            {
                tableFullName = string.Format("{0}.raj{1}_nach", schemaName, nzpGraj);

                // создаем партиционированную таблицу, унаследовавшись от таблицы raj_nach
                sqlQuery = string.Format(
                    " CREATE TABLE IF NOT EXISTS " +
                    " {1} " +
                    " (LIKE {0}.{2} INCLUDING ALL) " +
                    " INHERITS({0}.{2}) WITH (OIDS=TRUE) ",
                    ImportGkhDataForMinstroyReportsHandler.SchemaName,
                    tableFullName,
                    "raj_nach"
                );
                sqlExecutor.ExecuteSql(sqlQuery);

                // удаляем данные, которые были ранее по загружаемому периоду
                sqlQuery = string.Format(
                    " DELETE FROM {0} " +
                    " WHERE 0 < (" +
                    "   SELECT count(*) " +
                    "    FROM {1} " +
                    "   WHERE {1}.yr_={0}.yr_  " +
                    "    AND {1}.mn_={0}.mn_ " +
                    "    AND {1}.nzp_graj={0}.nzp_graj " +
                    "    AND {1}.prf={0}.prf " +
                    " ) ",
                    tableFullName,
                    tempTableName);

                // sqlExecutor.ExecuteSql(sqlQuery);
                sqlQuery = string.Format(
                    " INSERT INTO {0} " +
                    " (erc, nzp_dom, prf, nzp_ul, ulica, ndom, nkor, nzp_raj, rajon, nzp_town, " +
                    "  town, nzp_area, area, mn_, yr_, nzp_serv, nzp_supp, name_supp, nzp_frm, " +
                    "  nzp_measure, typek, is_device, cntls, cntkg, s_ob, s_ot, s_oba, s_ota,  " +
                    "  s_dom, s_mop, s_mop_hv, s_mop_gv, cntls_serv, cntkg_serv, s_ob_serv,    " +
                    "  tarif, rsum_tarif, sum_nedop, c_calc, sum_tarif_sn_f, sum_insaldo,      " +
                    "  sum_real, real_charge, reval, sum_money, sum_outsaldo, rval_real, rval, " +
                    "  rvaldlt, nzp_type_alg, vl210, dop87, kf307, kf307n, is_dpu, tarif_gkal, " +
                    "  dpu_val1, dpu_val2, dpu_ngp, odn_is_pu, odn_norm, odn_pl_mop, odn_val,  " +
                    "  odn_prev, cnt_ls_val, sum_ls_val, cnt_ls_norm, sum_ls_norm, cnt_ls_210val," +
                    "  sum_ls_210val, cnt_ls_210norm, sum_ls_210norm, cnt_gils_pu,             " +
                    "  cnt_gils_norm, cnt_pl_norm, cnt_pl_pu, cnt_ls_a_val, sum_ls_a_val,      " +
                    "  cnt_pl_a_pu, cnt_ls_a_norm, sum_ls_a_norm, cnt_pl_a_norm, sum_dpu,      " +
                    "  dt_begin, dt_end, val_begin, val_end, ngp_cnt, is_prev, etagnost, nzp_graj," +
                    "  ispp, s_pgp, s_pnp, koef_gv, tarif_ku," +
                    "  p1718, p28, p1871, p1872, p1873, p1874, p1875, p1876, type_dom) " +
                    " SELECT                                                                   " +
                    "  erc, COALESCE(nzp_dom, 0), prf, COALESCE(nzp_ul,0), ulica, ndom, nkor, COALESCE(nzp_raj,0), rajon, COALESCE(nzp_town,0), " +
                    "  town, COALESCE(nzp_area,0), area, COALESCE(mn_,0), COALESCE(yr_,0), COALESCE(nzp_serv,0), COALESCE(nzp_supp,0), name_supp, COALESCE(nzp_frm,0), "
                    +
                    "  COALESCE(nzp_measure,0), COALESCE(typek,0), COALESCE(is_device,0), COALESCE(cntls,0), COALESCE(cntkg,0), s_ob, s_ot, s_oba, s_ota,  "
                    +
                    "  s_dom, s_mop, s_mop_hv, s_mop_gv, cntls_serv, cntkg_serv, s_ob_serv,    " +
                    "  tarif, rsum_tarif, sum_nedop, c_calc, sum_tarif_sn_f, sum_insaldo,      " +
                    "  sum_real, real_charge, reval, sum_money, sum_outsaldo, rval_real, rval, " +
                    "  rvaldlt, COALESCE(nzp_type_alg,0), vl210, dop87, kf307, kf307n, is_dpu, tarif_gkal, " +
                    "  dpu_val1, dpu_val2, dpu_ngp, odn_is_pu, odn_norm, odn_pl_mop, odn_val,  " +
                    "  odn_prev, cnt_ls_val, sum_ls_val, cnt_ls_norm, sum_ls_norm, cnt_ls_210val," +
                    "  sum_ls_210val, cnt_ls_210norm, sum_ls_210norm, cnt_gils_pu,             " +
                    "  cnt_gils_norm, cnt_pl_norm, cnt_pl_pu, cnt_ls_a_val, sum_ls_a_val,      " +
                    "  cnt_pl_a_pu, cnt_ls_a_norm, sum_ls_a_norm, cnt_pl_a_norm, sum_dpu,      " +
                    "  dt_begin, dt_end, val_begin, val_end, ngp_cnt, is_prev, etagnost, COALESCE(nzp_graj,0)," +
                    "  ispp, s_pgp, s_pnp, koef_gv, tarif_ku, " +
                    "  p1718, p28, p1871, p1872, p1873, p1874, p1875, p1876, type_dom" +
                    " FROM {1} ",
                    tableFullName,
                    tempTableName);
                sqlExecutor.ExecuteSql(sqlQuery);
            }
        }

        /// <summary>
        /// Жесть метод, один в один переписан с дельфи
        /// идет жесткая корректировка и подготовка данных для отчета
        /// имеются корректирвоаки данных для определенных домов по коду дома.. ((
        /// </summary>
        /// <param name="sqlExecutor"></param>
        /// <param name="nzpGraj"></param>
        /// <param name="startCalculationMonth"></param>
        /// <param name="schemaName"></param>
        private void Zhest(
            SqlExecutor sqlExecutor,
            int nzpGraj,
            DateTime startCalculationMonth,
            string schemaName)
        {
            // данная загрузка переписана из delphi :'( идет жесткая корректировка данных, переписал код один в один

            // берегите глаза
            // можно рыдать, громко

            // временная таблица, в которую ранее положили данные из файла nach.crp
            var tempTableName = $"tmp_{"nach"}_{nzpGraj}_{startCalculationMonth.ToString("yyyyMM")}";

            var tableFullName = $"{schemaName}.raj{nzpGraj}_nach";

            var sqlQuery = $" SELECT yr_ AS year, mn_ AS month FROM {tempTableName} GROUP BY 1,2 ";

            var chargeYearMonth = sqlExecutor.ExecuteSql<ChargeYearMonth>(sqlQuery).FirstOrDefault();
            {
                // удаление временных таблиц, которые создаются в процессе работы как грибочки после дождя
                sqlQuery =
                    " DROP TABLE IF EXISTS todn;" +
                    " DROP TABLE IF EXISTS tnach;" +
                    " DROP TABLE IF EXISTS t36d;" +
                    " DROP TABLE IF EXISTS t36l;" +
                    " DROP TABLE IF EXISTS trf;" +
                    " DROP TABLE IF EXISTS tall;" +
                    " DROP TABLE IF EXISTS t25;" +
                    " DROP TABLE IF EXISTS t210;" +
                    " DROP TABLE IF EXISTS ta;" +
                    " DROP TABLE IF EXISTS tall1;" +
                    " DROP TABLE IF EXISTS tall2;" +
                    " DROP TABLE IF EXISTS tr_all;" +
                    " DROP TABLE IF EXISTS tr_el;" +
                    " DROP TABLE IF EXISTS tr_210;" +
                    " DROP TABLE IF EXISTS tr_el2;" +
                    " DROP TABLE IF EXISTS tr_el3;" +
                    " DROP TABLE IF EXISTS tls;";

                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = string.Format(
                    " SELECT " +
                    " a.nzp_graj nzp_graj1,a.nzp_area nzp_area1,a.nzp_dom nzp_dom1, " +
                    " a.nzp_serv nzp_serv1,max(a.tarif) tarif1,max(s.nzp_serv_link) nzp_serv_link, " +
                    " sum(CASE when a.is_device in (1,3,5,7) THEN a.rsum_tarif ELSE 0 end) sum_ipu1, " +
                    " sum(CASE when a.is_device not in (1,3,5,7) THEN a.rsum_tarif ELSE 0 end) sum_norm1, " +
                    " sum(CASE when a.is_device in (1,3,5,7) and a.tarif > 0 THEN a.rsum_tarif/a.tarif ELSE 0 end) r_ipu1, " +
                    " sum(CASE when a.is_device not in (1,3,5,7) and a.tarif > 0 THEN a.rsum_tarif/a.tarif ELSE 0 end) r_norm1, " +
                    " sum(CASE when a.nzp_serv > 500 THEN a.rsum_tarif-a.sum_nedop ELSE 0 end)sum_odn," +
                    " max(CASE when c_calc> 0 THEN a.sum_real/a.c_calc ELSE 0 end) as tarif2, max(dt_end) dt_end " +
                    " INTO TEMP todn   " +
                    " FROM   {1} a, {0}.serv_odn s " +
                    " WHERE  a.nzp_serv=s.nzp_serv AND " +
                    "        a.nzp_serv in (510, 511, 512, 513, 514, 515, 516) AND " +
                    "        mn_ = {2} AND " +
                    "        yr_ = {3}" +
                    " GROUP BY 1,2,3,4 ",
                    schemaName,
                    tableFullName,
                    chargeYearMonth.month,
                    chargeYearMonth.year);
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = " create index ix_todn on todn(nzp_graj1,nzp_area1,nzp_dom1,nzp_serv_link); ANALYZE  todn; ";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = string.Format(
                    " SELECT nzp_graj,nzp_area,nzp_dom,nzp_serv,nzp_measure,tarif,max(tarif_gkal) tarif_gkal," +
                    "        max(cntls) cntls,max(s_ob) s_ob," +
                    "        sum(cntls_serv) cntls_serv,sum(s_ob_serv) s_ob_serv," +
                    "        sum(CASE when is_device in (1,3,5,7) THEN s_ob_serv ELSE 0 end) s_ipu_serv," +
                    "        sum(CASE when is_device not in (1,3,5,7) THEN s_ob_serv ELSE 0 end) s_norm_serv," +
                    "        sum(rsum_tarif) rsum_tarif," +
                    "        sum(rsum_tarif-sum_nedop) sum_tarif," +
                    "        sum(CASE when is_device in (1,3,5,7) THEN rsum_tarif ELSE 0 end) sum_ipu," +
                    "        sum(CASE when is_device not in (1,3,5,7) THEN rsum_tarif ELSE 0 end) sum_norm," +
                    "        sum(vl210) vl210,sum(dop87) dop87," +
                    "        max(rval_real) rval_real,max(rval) rval,max(rvaldlt) rvaldlt,max(nzp_type_alg) nzp_type_alg," +
                    "        max(sum_dpu) sum_dpu,max(is_prev) is_prev," +
                    "        max(is_dpu) is_dpu,max(odn_is_pu) odn_is_pu," +
                    "        max(odn_norm) odn_norm,max(odn_pl_mop) odn_pl_mop,max(odn_val) odn_val,max(odn_prev) odn_prev," +
                    "        max(cnt_ls_val) cnt_ls_val,max(sum_ls_val) sum_ls_val,max(sum_ls_210val) sum_ls_210val," +
                    "        max(cnt_ls_norm) cnt_ls_norm,max(sum_ls_norm) sum_ls_norm,max(sum_ls_210norm) sum_ls_210norm," +
                    "        max(cnt_pl_pu) cnt_pl_pu,max(cnt_pl_norm) cnt_pl_norm, " +
                    "        max(CASE when c_calc> 0 THEN a.sum_real/a.c_calc ELSE 0 end) as tarif2, max(dt_end) as dt_end " +
                    " INTO TEMP tnach   " +
                    " FROM   {0} a" +
                    " WHERE nzp_serv in (6,7,8,9,14,25,210) AND " +
                    "        mn_ = {1} AND " +
                    "        yr_ = {2}" +
                    " GROUP BY 1,2,3,4,5,6 ",
                    tableFullName,
                    chargeYearMonth.month,
                    chargeYearMonth.year);
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery =
                    " create index ix_tnach on tnach(nzp_graj,nzp_area,nzp_dom,nzp_serv,nzp_measure); ANALYZE  tnach; ";
                sqlExecutor.ExecuteSql(sqlQuery);

                // для определенных домов определенного района подменяются данные.. :'(
                // во благо корректного формирования отчета
                sqlQuery = string.Format(
                    " SELECT nzp_graj,nzp_dom,6 nzp_serv,max(tarif) tarif,sum(rsum_tarif) rsum_tarif, " +
                    "        sum(CASE when tarif>0 THEN rsum_tarif/tarif ELSE 0 end) c_calc " +
                    " INTO TEMP t36d   " +
                    " FROM   {0} a" +
                    " WHERE  nzp_graj=36 AND           " +
                    "        nzp_dom in (11,15,45) AND " +
                    "        nzp_serv in (6,510) AND   " +
                    "        mn_ = {1} AND " +
                    "        yr_ = {2}" +
                    " GROUP BY 1,2,3 " +
                    " union all      " +
                    " SELECT nzp_graj,nzp_dom,9 nzp_serv,max(tarif) tarif,sum(rsum_tarif) rsum_tarif," +
                    "        sum(CASE when tarif>0 THEN rsum_tarif/tarif ELSE 0 end) c_calc " +
                    " FROM   {0}               " +
                    " WHERE  nzp_graj=36 AND         " +
                    "        nzp_dom in (10659) AND  " +
                    "        nzp_serv in (9,513) AND " +
                    "        mn_ = {1} AND " +
                    "        yr_ = {2}" +
                    " GROUP BY 1,2,3 "
                    ,
                    tableFullName,
                    chargeYearMonth.month,
                    chargeYearMonth.year);
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = string.Format(
                    " SELECT nzp_graj,nzp_dom,6 nzp_serv,max(tarif) tarif,sum(rsum_tarif) rsum_tarif," +
                    "        sum(CASE when tarif>0 then rsum_tarif/tarif else 0 end) c_calc " +
                    " INTO TEMP t36l   " +
                    " FROM   {0} a" +
                    " WHERE  nzp_graj=36 AND nzp_dom in (11,15,45) AND nzp_serv in (6) AND " +
                    "        mn_ = {1} AND " +
                    "        yr_ = {2}" +
                    " GROUP BY 1,2,3 " +
                    " union all      " +
                    " SELECT nzp_graj,nzp_dom,9 nzp_serv,max(tarif) tarif,sum(rsum_tarif) rsum_tarif," +
                    "        sum(CASE when tarif>0 then rsum_tarif/tarif else 0 end) c_calc " +
                    " FROM   {0}               " +
                    " where  nzp_graj=36 and nzp_dom in (10659) and nzp_serv in (9) and         " +
                    "        mn_ = {1} AND " +
                    "        yr_ = {2}" +
                    " GROUP BY 1,2,3 "
                    ,
                    tableFullName,
                    chargeYearMonth.month,
                    chargeYearMonth.year);
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery =
                    " UPDATE tnach set rvaldlt=(            " +
                    " SELECT c_calc                         " +
                    " FROM   t36d h                         " +
                    " WHERE  tnach.nzp_graj=h.nzp_graj AND  " +
                    "        tnach.nzp_dom=h.nzp_dom AND    " +
                    "        tnach.nzp_serv=h.nzp_serv)     " +
                    " WHERE  0<(SELECT count(*)             " +
                    "           FROM   t36d h               " +
                    "           WHERE  tnach.nzp_graj=h.nzp_graj AND " +
                    "                  tnach.nzp_dom=h.nzp_dom AND   " +
                    "                  tnach.nzp_serv=h.nzp_serv)    ";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery =
                    " UPDATE tnach set sum_ls_val=(SELECT c_calc " +
                    "                              FROM t36l h   " +
                    "                              WHERE tnach.nzp_graj=h.nzp_graj AND " +
                    "                                    tnach.nzp_dom=h.nzp_dom AND   " +
                    "                                    tnach.nzp_serv=h.nzp_serv)    " +
                    " WHERE 0<(SELECT count(*) " +
                    "          FROM   t36l h   " +
                    "          WHERE  tnach.nzp_graj=h.nzp_graj AND " +
                    "                 tnach.nzp_dom=h.nzp_dom AND   " +
                    "                 tnach.nzp_serv=h.nzp_serv)    ";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery =
                    " SELECT nzp_graj nzp_graj2,nzp_area nzp_area2,nzp_dom nzp_dom2,nzp_serv nzp_serv2, " +
                    "        max(cntls_serv) cntls_serv2 " +
                    " INTO TEMP tls       " +
                    " FROM   tnach                  " +
                    " GROUP BY 1,2,3,4              "
                    ;
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = " create unique index ix_tls on tls(nzp_graj2,nzp_area2,nzp_dom2,nzp_serv2) ; ANALYZE tls ";
                
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery =
                    " SELECT nzp_graj2, nzp_area2, nzp_dom2, nzp_serv2, cntls_serv2, max(a.tarif) tarif2 " +
                    " INTO TEMP trf  " +
                    " FROM   tnach a,tls t " +
                    " WHERE  a.nzp_graj=t.nzp_graj2 AND " +
                    "        a.nzp_area=t.nzp_area2 AND " +
                    "        a.nzp_dom=t.nzp_dom2 AND   " +
                    "        a.nzp_serv=t.nzp_serv2 AND " +
                    "        a.cntls_serv=t.cntls_serv2 " +
                    " GROUP BY 1,2,3,4,5 "
                    ;
                sqlExecutor.ExecuteSql(sqlQuery);
                sqlQuery =
                    " create unique index ix_trf1 on trf(nzp_graj2,nzp_area2,nzp_dom2,nzp_serv2) ";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery =
                    " create index ix_trf2 on trf(nzp_graj2,nzp_area2,nzp_dom2,nzp_serv2,tarif2) ";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery =
                    " ANALYZE trf ";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = string.Format(
                    " SELECT nzp_graj,nzp_area,nzp_dom,nzp_serv,max(is_device) is_device,max(nzp_measure) nzp_measure, " +
                    "        max(tarif)tarif, max(CASE when c_calc> 0 THEN sum_real/c_calc ELSE 0 end) as tarif2, " +
                    "        max(tarif_gkal) tarif_gkal,max(cntls) cntls,max(s_ob) s_ob, sum(cntls_serv) cntls_serv, " +
                    "        sum(s_ob_serv) s_ob_serv, " +
                    "        sum(CASE when is_device in (1,3,5,7) THEN cntls_serv ELSE 0 end) cntls_ipu_serv, " +
                    "        sum(CASE when is_device not in (1,3,5,7) THEN cntls_serv ELSE 0 end) cntls_norm_serv, " +
                    "        sum(CASE when is_device in (1,3,5,7) THEN s_ob_serv ELSE 0 end) s_ipu_serv, " +
                    "        sum(CASE when is_device not in (1,3,5,7) THEN s_ob_serv ELSE 0 end) s_norm_serv, " +
                    "        sum(rsum_tarif) rsum_tarif, sum(rsum_tarif-sum_nedop) sum_tarif," +
                    "        sum(CASE when is_device in (1,3,5,7) THEN rsum_tarif ELSE 0 end) sum_ipu," +
                    "        sum(CASE when is_device not in (1,3,5,7) THEN rsum_tarif ELSE 0 end) sum_norm," +
                    "        sum(CASE when is_device in (1,3,5,7) and tarif > 0 THEN rsum_tarif/tarif ELSE 0 end) val_ipu," +
                    "        sum(CASE when is_device not in (1,3,5,7) and tarif > 0 THEN rsum_tarif/tarif ELSE 0 end) val_norm," +
                    "        max(rsum_tarif) sum_ipud, max(rsum_tarif) sum_normd,max(rsum_tarif) r_ipud, " +
                    "        max(rsum_tarif) r_normd, sum(vl210) vl210,sum(dop87) dop87, " +
                    "        max(rval_real) rval_real, max(rval) rval, max(rvaldlt) rvaldlt, " +
                    "        max(nzp_type_alg) nzp_type_alg, max(sum_dpu) sum_dpu,max(is_prev) is_prev," +
                    "        max(is_dpu) is_dpu,max(odn_is_pu) odn_is_pu, max(odn_norm) odn_norm,      " +
                    "        max(odn_pl_mop) odn_pl_mop,max(odn_val) odn_val,max(odn_prev) odn_prev,   " +
                    "        max(cnt_ls_val) cnt_ls_val,max(sum_ls_val) sum_ls_val,                    " +
                    "        max(sum_ls_210val) sum_ls_210val,max(sum_ls_210val) sum_ls_210vals,       " +
                    "        max(cnt_ls_norm) cnt_ls_norm,max(sum_ls_norm) sum_ls_norm,                " +
                    "        max(sum_ls_210norm) sum_ls_210norm,max(sum_ls_210norm) sum_ls_210norms,   " +
                    "        max(cnt_pl_pu) cnt_pl_pu,max(cnt_pl_norm) cnt_pl_norm,  sum(cast (0 AS NUMERIC (14,2))) sum_odn, max(dt_end) dt_end " +
                    " INTO TEMP tall   " +
                    " FROM  {0}" +
                    " WHERE  nzp_serv in (6, 7, 8, 9, 14, 25, 210) AND nzp_measure<>4 AND tarif > 0 AND " +
                    "        mn_ = {1} AND " +
                    "        yr_ = {2} " +
                    " GROUP BY 1,2,3,4 "
                    ,
                    tableFullName,
                    chargeYearMonth.month,
                    chargeYearMonth.year);
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery =
                    " create unique index ix_tall on tall(nzp_graj,nzp_area,nzp_dom,nzp_serv)";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = " ANALYZE tall";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery =
                    " UPDATE tall set rvaldlt=(SELECT c_calc " +
                    "                          FROM   t36d h " +
                    "                          WHERE  tall.nzp_graj=h.nzp_graj AND " +
                    "                                 tall.nzp_dom=h.nzp_dom AND   " +
                    "                                 tall.nzp_serv=h.nzp_serv)    " +
                    " WHERE 0<(SELECT count(*) " +
                    "          FROM   t36d h   " +
                    "          WHERE  tall.nzp_graj=h.nzp_graj AND " +
                    "                 tall.nzp_dom=h.nzp_dom AND   " +
                    "                 tall.nzp_serv=h.nzp_serv)    ";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery =
                    " UPDATE tall set sum_ls_val=(SELECT c_calc " +
                    "                             FROM t36l h   " +
                    "                             WHERE tall.nzp_graj=h.nzp_graj AND " +
                    "                                   tall.nzp_dom=h.nzp_dom AND   " +
                    "                                   tall.nzp_serv=h.nzp_serv)    " +
                    " WHERE 0<(SELECT count(*)                    " +
                    "          FROM t36l h                        " +
                    "          WHERE tall.nzp_graj=h.nzp_graj AND " +
                    "                tall.nzp_dom=h.nzp_dom AND   " +
                    "                tall.nzp_serv=h.nzp_serv)    ";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery =
                    " UPDATE tall set nzp_type_alg=0 WHERE nzp_type_alg is null ";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery =
                    " UPDATE tall set odn_prev=0 WHERE odn_prev is null ";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery =
                    " UPDATE tall set rvaldlt=0 WHERE rvaldlt is null ";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery =
                    " UPDATE tall set sum_ipud=0, sum_normd=0,r_ipud=0, r_normd=0, tarif=0 WHERE 1=1 ";
                sqlExecutor.ExecuteSql(sqlQuery);
                {
                    sqlQuery =
                        " UPDATE tall set sum_ipud= " +
                        " (SELECT max(sum_ipu1) FROM todn b " +
                        " WHERE tall.nzp_graj=b.nzp_graj1 AND tall.nzp_area=b.nzp_area1 " +
                        "       AND tall.nzp_dom=b.nzp_dom1 AND tall.nzp_serv=b.nzp_serv_link) " +
                        " WHERE 0< " +
                        " (SELECT count(*) FROM todn b " +
                        " WHERE tall.nzp_graj=b.nzp_graj1 AND tall.nzp_area=b.nzp_area1 " +
                        "       AND tall.nzp_dom=b.nzp_dom1 AND tall.nzp_serv=b.nzp_serv_link)";
                    sqlExecutor.ExecuteSql(sqlQuery);
                    sqlQuery =
                        " UPDATE tall set sum_normd= " +
                        " (SELECT max(sum_norm1) FROM todn b " +
                        " WHERE tall.nzp_graj=b.nzp_graj1 AND tall.nzp_area=b.nzp_area1 " +
                        "       AND tall.nzp_dom=b.nzp_dom1 AND tall.nzp_serv=b.nzp_serv_link) " +
                        " WHERE 0< " +
                        " (SELECT count(*) FROM todn b " +
                        " WHERE tall.nzp_graj=b.nzp_graj1 AND tall.nzp_area=b.nzp_area1 " +
                        "       AND tall.nzp_dom=b.nzp_dom1 AND tall.nzp_serv=b.nzp_serv_link)";
                    sqlExecutor.ExecuteSql(sqlQuery);
                    sqlQuery =
                        " UPDATE tall set r_ipud= " +
                        " (SELECT max(r_ipu1) FROM todn b " +
                        " WHERE tall.nzp_graj=b.nzp_graj1 AND tall.nzp_area=b.nzp_area1 " +
                        "       AND tall.nzp_dom=b.nzp_dom1 AND tall.nzp_serv=b.nzp_serv_link) " +
                        " WHERE 0< " +
                        " (SELECT count(*) FROM todn b " +
                        " WHERE tall.nzp_graj=b.nzp_graj1 AND tall.nzp_area=b.nzp_area1 " +
                        "       AND tall.nzp_dom=b.nzp_dom1 AND tall.nzp_serv=b.nzp_serv_link)";
                    sqlExecutor.ExecuteSql(sqlQuery);
                    sqlQuery =
                        " UPDATE tall set r_normd= " +
                        " (SELECT max(r_norm1) FROM todn b " +
                        " WHERE tall.nzp_graj=b.nzp_graj1 AND tall.nzp_area=b.nzp_area1 " +
                        "       AND tall.nzp_dom=b.nzp_dom1 AND tall.nzp_serv=b.nzp_serv_link) " +
                        " WHERE 0< " +
                        " (SELECT count(*) FROM todn b " +
                        " WHERE tall.nzp_graj=b.nzp_graj1 AND tall.nzp_area=b.nzp_area1 " +
                        "       AND tall.nzp_dom=b.nzp_dom1 AND tall.nzp_serv=b.nzp_serv_link)";
                    sqlExecutor.ExecuteSql(sqlQuery);
                }

                sqlQuery =
                    " UPDATE tall set (sum_odn)=" +
                    " ((SELECT sum(sum_odn) FROM todn b" +
                    " WHERE tall.nzp_graj=b.nzp_graj1 AND tall.nzp_area=b.nzp_area1 AND " +
                    "       tall.nzp_dom=b.nzp_dom1 AND tall.nzp_serv=b.nzp_serv_link))" +
                    " WHERE 0<" +
                    " (SELECT count(*) FROM todn b" +
                    " WHERE tall.nzp_graj=b.nzp_graj1 AND tall.nzp_area=b.nzp_area1 AND " +
                    "       tall.nzp_dom=b.nzp_dom1 AND tall.nzp_serv=b.nzp_serv_link)";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery =
                    " UPDATE tall set tarif= " +
                    " (SELECT max(tarif2) FROM trf b " +
                    " WHERE tall.nzp_graj=b.nzp_graj2 AND tall.nzp_area=b.nzp_area2 AND " +
                    "       tall.nzp_dom=b.nzp_dom2 AND tall.nzp_serv=b.nzp_serv2) " +
                    " WHERE 0< " +
                    " (SELECT count(*) FROM trf b " +
                    " WHERE tall.nzp_graj=b.nzp_graj2 AND tall.nzp_area=b.nzp_area2 AND " +
                    "       tall.nzp_dom=b.nzp_dom2 AND tall.nzp_serv=b.nzp_serv2) ";
                sqlExecutor.ExecuteSql(sqlQuery);
                sqlQuery =
                    " SELECT nzp_graj,nzp_dom " +
                    " INTO TEMP t25   " +
                    " FROM   tall " +
                    " WHERE  nzp_serv=25 AND nzp_type_alg=21 " +
                    " GROUP BY 1,2 "
                    ;
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = " create unique index ix_t25 on t25(nzp_graj,nzp_dom) ";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = " ANALYZE t25 ";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery =
                    " SELECT nzp_graj,nzp_dom,sum(val_ipu) val_ipu210,sum(sum_ipu) sum_ipu210, " +
                    " sum(val_norm) val_norm210,sum(sum_norm) sum_norm210 " +
                    " INTO TEMP t210  " +
                    " FROM tall WHERE nzp_serv=210 AND 0<(SELECT count(*) " +
                    "                                     FROM t25        " +
                    "                                     WHERE t25.nzp_graj=tall.nzp_graj AND " +
                    "                                           t25.nzp_dom=tall.nzp_dom)      " +
                    " GROUP BY 1,2 "
                    ;
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = " create unique index ix_t210 on t210(nzp_graj,nzp_dom) ";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = " ANALYZE t210 ";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery =
                    " UPDATE tall set sum_ls_210val=0, sum_ls_210norm=0,sum_ls_210vals=0, sum_ls_210norms=0 ";
                sqlExecutor.ExecuteSql(sqlQuery);
                {
                    sqlQuery =
                        " UPDATE tall set sum_ls_210val =" +
                        " (SELECT val_ipu210 FROM t210 b" +
                        " WHERE tall.nzp_graj=b.nzp_graj AND tall.nzp_dom=b.nzp_dom)" +
                        " WHERE nzp_serv=25 AND 0<" +
                        " (SELECT count(*) FROM t210 b" +
                        " WHERE tall.nzp_graj=b.nzp_graj AND tall.nzp_dom=b.nzp_dom)";
                    sqlExecutor.ExecuteSql(sqlQuery);

                    sqlQuery =
                        " UPDATE tall set sum_ls_210norm=" +
                        " ((SELECT val_norm210 FROM t210 b" +
                        " WHERE tall.nzp_graj=b.nzp_graj AND tall.nzp_dom=b.nzp_dom))" +
                        " WHERE nzp_serv=25 AND 0<" +
                        " (SELECT count(*) FROM t210 b" +
                        " WHERE tall.nzp_graj=b.nzp_graj AND tall.nzp_dom=b.nzp_dom)";
                    sqlExecutor.ExecuteSql(sqlQuery);

                    sqlQuery =
                        " UPDATE tall set sum_ls_210vals=" +
                        " ((SELECT sum_ipu210 FROM t210 b" +
                        " WHERE tall.nzp_graj=b.nzp_graj AND tall.nzp_dom=b.nzp_dom))" +
                        " WHERE nzp_serv=25 AND 0<" +
                        " (SELECT count(*) FROM t210 b" +
                        " WHERE tall.nzp_graj=b.nzp_graj AND tall.nzp_dom=b.nzp_dom)";
                    sqlExecutor.ExecuteSql(sqlQuery);

                    sqlQuery =
                        " UPDATE tall set sum_ls_210norms=" +
                        " ((SELECT sum_norm210 FROM t210 b" +
                        " WHERE tall.nzp_graj=b.nzp_graj AND tall.nzp_dom=b.nzp_dom))" +
                        " WHERE nzp_serv=25 AND 0<" +
                        " (SELECT count(*) FROM t210 b" +
                        " WHERE tall.nzp_graj=b.nzp_graj AND tall.nzp_dom=b.nzp_dom)";
                    sqlExecutor.ExecuteSql(sqlQuery);
                }

                sqlQuery = string.Format(
                    " SELECT nzp_graj,nzp_area,max(area) area " +
                    " INTO TEMP ta " +
                    " FROM  {0}" +
                    " WHERE  mn_ = {1} AND " +
                    "        yr_ = {2} " +
                    " GROUP BY 1,2    ",
                    tableFullName,
                    chargeYearMonth.month,
                    chargeYearMonth.year);

                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = " create unique index ix_ta on ta(nzp_graj,nzp_area)";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = " ANALYZE ta ";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery =
                    " SELECT nzp_graj, min(nzp_area)nzp_area, nzp_dom, nzp_serv, max(is_device) is_device, max(nzp_measure)nzp_measure," +
                    "        max(tarif)tarif, max(tarif2)tarif2, max(tarif_gkal)tarif_gkal, max(cntls)cntls, " +
                    "        max(s_ob)s_ob, sum(cntls_serv)cntls_serv, sum(s_ob_serv) s_ob_serv,             " +
                    "        sum(cntls_serv) cntls_ipu_serv, sum(cntls_serv) cntls_norm_serv,                " +
                    "        sum(s_ob_serv) s_ipu_serv, sum(s_ob_serv) s_norm_serv,                          " +
                    "        sum(rsum_tarif) rsum_tarif, sum(sum_tarif) sum_tarif, sum(sum_odn) sum_odn,     " +
                    "        sum(sum_ipu) sum_ipu, sum(sum_norm) sum_norm, sum(val_ipu)val_ipu,              " +
                    "        sum(val_norm)val_norm, max(sum_ipud) sum_ipud, max(sum_normd) sum_normd,        " +
                    "        max(r_ipud) r_ipud, max(r_normd) r_normd, sum(vl210) vl210,sum(dop87) dop87,    " +
                    "        max(rval_real) rval_real,max(rval) rval,max(rvaldlt) rvaldlt,                   " +
                    "        max(nzp_type_alg) nzp_type_alg, max(sum_dpu) sum_dpu, max(is_prev) is_prev,     " +
                    "        max(is_dpu) is_dpu, max(odn_is_pu) odn_is_pu, max(odn_norm) odn_norm,           " +
                    "        max(odn_pl_mop) odn_pl_mop,max(odn_val) odn_val,max(odn_prev) odn_prev,         " +
                    "        max(cnt_ls_val) cnt_ls_val,max(sum_ls_val) sum_ls_val,                          " +
                    "        max(sum_ls_210val) sum_ls_210val, max(sum_ls_210val) sum_ls_210vals,            " +
                    "        max(cnt_ls_norm) cnt_ls_norm,max(sum_ls_norm) sum_ls_norm,                      " +
                    "        max(sum_ls_210norm) sum_ls_210norm,max(sum_ls_210norm) sum_ls_210norms,         " +
                    "        max(cnt_pl_pu) cnt_pl_pu,max(cnt_pl_norm) cnt_pl_norm, max(dt_end) dt_end       " +
                    " INTO TEMP tall1                                                              " +
                    " FROM   tall WHERE nzp_measure<>4 AND nzp_serv in (6, 7, 8, 9, 14, 25, 210)             " +
                    " GROUP BY 1,3,4                                                                         "
                    ;
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery =
                    " create unique index ix_tall1 on tall1(nzp_graj,nzp_area,nzp_dom,nzp_serv) ";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery =
                    " ANALYZE tall1 ";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery =
                    string.Format(
                        " INSERT INTO {0}.resp_pu " +
                        " SELECT {1}, {2}, " +
                        "        raj, t.nzp_graj, s.nzp_serv, s.service, count(nzp_dom),     " +
                        "        sum(CASE when is_dpu=0               THEN 1 ELSE 0 end) a2, " +
                        "        sum(CASE when rvaldlt>0 AND is_dpu=0 THEN 1 ELSE 0 end) a3, " +

                        // есть ДПУ
                        "        sum(CASE when is_dpu=1               THEN 1 ELSE 0 end) a4, " +
                        "        sum(case when ((rvaldlt>0 and is_dpu=1 and dt_end is not null) or (is_device>=4)) then 1 else 0 end) a5, " +
                        "        sum(CASE when is_dpu = 1 AND rvaldlt> 0 " +
                        "                      AND abs (rvaldlt  - sum_ls_val - sum_ls_210val - sum_ls_norm - sum_ls_210norm ) <0.01 THEN 1 ELSE 0 end) as dky "
                        +
                        " FROM   tall1 t, ta, {0}.services s, {0}.subs_rajon sr, " +
                        "        {0}.subs_rajon_gor srg " +
                        " WHERE  t.nzp_serv in(6,9,8,25) AND t.nzp_graj=ta.nzp_graj AND t.nzp_area=ta.nzp_area        " +
                        "        AND t.cntls_serv>1  AND t.sum_tarif > 0.0001 AND s.nzp_serv = t.nzp_serv             " +
                        "        AND sr.nzp_graj = t.nzp_graj AND srg.nzp_raj = sr.nzp_raj                            " +
                        " GROUP BY 1,2,3,4,5,6 ",
                        schemaName,
                        chargeYearMonth.year,
                        chargeYearMonth.month);
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = " SELECT *, 0 nzp_graj1 INTO TEMP tall2 FROM tall1    ";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery =
                    " create unique index ix_tall2 on tall2(nzp_graj,nzp_area,nzp_dom,nzp_serv); ANALYZE tall2 ";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = " UPDATE tall2 set nzp_graj1 = nzp_graj ";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = " UPDATE ta set nzp_graj = 106 WHERE nzp_graj = 270";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = " UPDATE ta set nzp_graj = 110 WHERE nzp_graj = 273";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = " UPDATE ta set nzp_graj = 107 WHERE nzp_graj = 274";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = " UPDATE ta set nzp_graj = 111 WHERE nzp_graj = 275";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = " UPDATE ta set nzp_graj = 109 WHERE nzp_graj = 278";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = " UPDATE ta set nzp_graj = 108 WHERE nzp_graj = 279";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = " UPDATE ta set nzp_graj = 112 WHERE nzp_graj in (272,277,283,284,286,287)";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = " UPDATE tall2 set nzp_graj = 106 WHERE nzp_graj = 270";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = " UPDATE tall2 set nzp_graj = 110 WHERE nzp_graj = 273";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = " UPDATE tall2 set nzp_graj = 107 WHERE nzp_graj = 274";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = " UPDATE tall2 set nzp_graj = 111 WHERE nzp_graj = 275";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = " UPDATE tall2 set nzp_graj = 109 WHERE nzp_graj = 278";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = " UPDATE tall2 set nzp_graj = 108 WHERE nzp_graj = 279";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = " UPDATE tall2 set nzp_graj = 112 WHERE nzp_graj in (272,277,283,284,286,287)";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = " UPDATE tall1 set nzp_graj = 106 WHERE nzp_graj = 270";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = " UPDATE tall1 set nzp_graj = 110 WHERE nzp_graj = 273";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = " UPDATE tall1 set nzp_graj = 107 WHERE nzp_graj = 274";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = " UPDATE tall1 set nzp_graj = 111 WHERE nzp_graj = 275";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = " UPDATE tall1 set nzp_graj = 109 WHERE nzp_graj = 278";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = " UPDATE tall1 set nzp_graj = 108 WHERE nzp_graj = 279";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = " UPDATE tall1 set nzp_graj = 112 WHERE nzp_graj  in (272, 277,283,284,286,287)";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery =
                    " SELECT nzp_graj, nzp_serv, " +
                    "        sum(CASE when (rvaldlt>0 AND is_dpu=1) THEN 1 ELSE 0 end) as is_dpu,   " +
                    "        nzp_dom, sum(cntls) as count_ls, sum(sum_tarif) as sum_tarif,          " +
                    "        sum(sum_odn) as sum_odn, sum(rvaldlt) as rvaldlt, max(tarif) as tarif, " +
                    "        sum(val_ipu+val_norm) as c_calc, " +
                    "        sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND " +
                    "                      COALESCE(odn_prev,0)>0.000001 AND rvaldlt-val_ipu-val_norm>0 " +
                    "                 THEN tarif2*(rvaldlt-val_ipu-val_norm-r_ipud-r_normd) ELSE 0 end) nodn " +
                    " INTO TEMP tr_all   " +
                    " FROM  tall2                " +
                    " WHERE sum_tarif>0.0001 AND " +
                    "       cntls_serv > 1 AND   " +
                    "       nzp_serv <> 25 AND   " +
                    "       nzp_serv <> 210      " +
                    " GROUP BY 1,2,4             "
                    ;
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery =
                    " SELECT nzp_graj, nzp_serv, " +
                    "        sum(CASE when (rvaldlt>0 AND is_dpu=1) THEN 1 ELSE 0 end) as is_dpu, nzp_dom,  " +
                    "        sum(cntls) as count_ls, sum(sum_tarif) as sum_tarif, sum(sum_odn) as sum_odn,  " +
                    "        sum(rvaldlt) as rvaldlt, max(tarif) as tarif, sum(val_ipu+val_norm) as c_calc, " +
                    "        sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND " +
                    "                 COALESCE(odn_prev,0)>0.000001 " +
                    "                 AND rvaldlt-val_ipu-val_norm-sum_ls_210val-sum_ls_210norm>0 " +
                    "        THEN tarif2*(rvaldlt-val_ipu-val_norm-r_ipud-r_normd-sum_ls_210val-sum_ls_210norm) ELSE 0 end) nodn" +
                    " INTO TEMP tr_el   " +
                    " FROM   tall2 " +
                    " WHERE  sum_tarif>0.0001 AND " +
                    "        cntls_serv > 1 AND   " +
                    "        nzp_serv in( 25,210) " +
                    " GROUP BY 1,2,4              "
                    ;
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery =
                    " SELECT nzp_graj, nzp_serv, " +
                    "        sum(CASE when (rvaldlt>0 AND is_dpu=1) THEN 1 ELSE 0 end) as is_dpu,   " +
                    "        nzp_dom, sum(cntls) as count_ls, sum(sum_tarif) as sum_tarif,          " +
                    "        sum(sum_odn) as sum_odn, sum(rvaldlt) as rvaldlt, max(tarif) as tarif, " +
                    "        sum(val_ipu+val_norm) as c_calc, " +
                    "        sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND " +
                    "                      COALESCE(odn_prev,0)>0.000001 AND rvaldlt-val_ipu-val_norm>0 " +
                    "                 THEN tarif2*(rvaldlt-val_ipu-val_norm-r_ipud-r_normd) ELSE 0 end) nodn " +
                    " INTO TEMP tr_210     " +
                    " FROM  tall2                " +
                    " WHERE sum_tarif>0.0001 AND " +
                    "       cntls_serv > 1 AND   " +
                    "       nzp_serv = 210 AND   " +
                    "       nzp_type_alg in (22,122) " +
                    " GROUP BY 1,2,4                 ";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery =
                    " UPDATE tr_el set is_dpu = 1 WHERE nzp_serv = 210 AND COALESCE(is_dpu,0) = 0 ";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery =
                    " SELECT nzp_graj, nzp_dom, count_ls,sum_tarif,sum_odn, rvaldlt, tarif,c_calc, nodn, max(is_dpu)is_dpu " +
                    " INTO TEMP tr_el2   " +
                    " FROM   tr_el WHERE sum_tarif>0.0001 " +
                    " GROUP BY 1,2,3,4,5,6,7,8,9 "
                    ;
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery =
                    " SELECT nzp_graj, nzp_dom, count_ls, is_dpu, sum(sum_tarif) as sum_tarif, sum(sum_odn) as sum_odn, " +
                    " sum(rvaldlt) as rvaldlt, max(tarif) as tarif, sum(c_calc) as c_calc, sum(nodn) as nodn " +
                    " INTO TEMP tr_el3    " +
                    " FROM tr_el2 " +
                    " GROUP BY 1,2,3,4 ";
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = string.Format(
                    " DELETE FROM {0}.resp_odn4 " +
                    " WHERE 0 < (SELECT count(*) " +
                    "            FROM {1} " +
                    "            WHERE {1}.yr_={0}.resp_odn4.year_              " +
                    "                  AND {1}.mn_={0}.resp_odn4.month_         " +
                    "                  AND {1}.nzp_graj={0}.resp_odn4.nzp_graj ) ",
                    schemaName,
                    tempTableName);

                // sqlExecutor.ExecuteSql(sqlQuery);

                // загружаем данные по отчету 3-63-2
                sqlQuery = string.Format(
                    " INSERT INTO {0}.resp_odn4 " +
                    " (nzp_graj, year_, month_,nzp_serv, is_dpu,kod_, count_dom, count_ls, sum_tarif, " +
                    "  sum_odn, rvaldlt, tarif, c_calc, nodn) " +
                    " SELECT nzp_graj, {1}, {2}, nzp_serv,is_dpu, " +
                    " (CASE when sum_odn=0 AND COALESCE(is_dpu,0) = 0 THEN 2 " +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn+sum_odn) > 0    and(sum_odn+nodn)/(sum_tarif+nodn+sum_odn) < 0.05 AND COALESCE(is_dpu,0) = 0 THEN 3 "
                    +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn+sum_odn) >=0.05 and(sum_odn+nodn)/(sum_tarif+nodn+sum_odn) < 0.10 AND COALESCE(is_dpu,0) = 0 THEN 4 "
                    +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn+sum_odn) >=0.10 and(sum_odn+nodn)/(sum_tarif+nodn+sum_odn) < 0.15 AND COALESCE(is_dpu,0) = 0 THEN 5 "
                    +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn+sum_odn) >=0.15 and(sum_odn+nodn)/(sum_tarif+nodn+sum_odn) < 0.20 AND COALESCE(is_dpu,0) = 0 THEN 6 "
                    +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn+sum_odn) >=0.20 and(sum_odn+nodn)/(sum_tarif+nodn+sum_odn) < 0.25 AND COALESCE(is_dpu,0) = 0 THEN 7 "
                    +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn+sum_odn)>=0.25  AND COALESCE(is_dpu,0) = 0 THEN 8     " +
                    "       when sum_odn=0 AND COALESCE(is_dpu,0) = 1 THEN 10 " +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn+sum_odn) > 0    and(sum_odn+nodn)/(sum_tarif+nodn+sum_odn) < 0.05 AND COALESCE(is_dpu,0) = 1 THEN 11 "
                    +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn+sum_odn) >=0.05 and(sum_odn+nodn)/(sum_tarif+nodn+sum_odn) < 0.10 AND COALESCE(is_dpu,0) = 1 THEN 12 "
                    +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn+sum_odn) >=0.10 and(sum_odn+nodn)/(sum_tarif+nodn+sum_odn) < 0.15 AND COALESCE(is_dpu,0) = 1 THEN 13 "
                    +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn+sum_odn) >=0.15 and(sum_odn+nodn)/(sum_tarif+nodn+sum_odn) < 0.20 AND COALESCE(is_dpu,0) = 1 THEN 14 "
                    +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn+sum_odn) >=0.20 and(sum_odn+nodn)/(sum_tarif+nodn+sum_odn) < 0.25 AND COALESCE(is_dpu,0) = 1 THEN 15 "
                    +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn+sum_odn)>=0.25  AND COALESCE(is_dpu,0) = 1 THEN 16 ELSE 0 end " +
                    "  ) as kod, " +
                    "  count(nzp_dom) as count_dom, sum(count_ls) as count_ls, sum(sum_tarif) as sum_tarif, " +
                    "  sum(sum_odn) as sum_odn, sum(rvaldlt) as rvaldlt, max(tarif) as tarif, " +
                    "  sum(c_calc) as c_calc, sum(nodn) as nodn " +
                    " FROM  tr_all " +
                    " WHERE sum_tarif>0.0001 GROUP BY 1,2,3,4,5,6 ",
                    schemaName,
                    chargeYearMonth.year,
                    chargeYearMonth.month);
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = string.Format(
                    " INSERT INTO {0}.resp_odn4 " +
                    " (nzp_graj, year_, month_,nzp_serv, is_dpu,kod_, count_dom, count_ls, sum_tarif, " +
                    "  sum_odn, rvaldlt, tarif, c_calc, nodn) " +
                    " SELECT nzp_graj, {1}, {2}, nzp_serv,is_dpu, " +
                    " (CASE when sum_odn=0 AND COALESCE(is_dpu,0) = 0 THEN 2 " +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn+sum_odn) > 0    and(sum_odn+nodn)/(sum_tarif+nodn+sum_odn) < 0.05 AND COALESCE(is_dpu,0) = 0 THEN 3 "
                    +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn+sum_odn) >=0.05 and(sum_odn+nodn)/(sum_tarif+nodn+sum_odn) < 0.10 AND COALESCE(is_dpu,0) = 0 THEN 4 "
                    +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn+sum_odn) >=0.10 and(sum_odn+nodn)/(sum_tarif+nodn+sum_odn) < 0.15 AND COALESCE(is_dpu,0) = 0 THEN 5 "
                    +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn+sum_odn) >=0.15 and(sum_odn+nodn)/(sum_tarif+nodn+sum_odn) < 0.20 AND COALESCE(is_dpu,0) = 0 THEN 6 "
                    +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn+sum_odn) >=0.20 and(sum_odn+nodn)/(sum_tarif+nodn+sum_odn) < 0.25 AND COALESCE(is_dpu,0) = 0 THEN 7 "
                    +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn+sum_odn)>=0.25  AND COALESCE(is_dpu,0) = 0 THEN 8     " +
                    "       when sum_odn=0 AND COALESCE(is_dpu,0) = 1 THEN 10 " +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn+sum_odn) > 0    and(sum_odn+nodn)/(sum_tarif+nodn+sum_odn) < 0.05 AND COALESCE(is_dpu,0) = 1 THEN 11 "
                    +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn+sum_odn) >=0.05 and(sum_odn+nodn)/(sum_tarif+nodn+sum_odn) < 0.10 AND COALESCE(is_dpu,0) = 1 THEN 12 "
                    +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn+sum_odn) >=0.10 and(sum_odn+nodn)/(sum_tarif+nodn+sum_odn) < 0.15 AND COALESCE(is_dpu,0) = 1 THEN 13 "
                    +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn+sum_odn) >=0.15 and(sum_odn+nodn)/(sum_tarif+nodn+sum_odn) < 0.20 AND COALESCE(is_dpu,0) = 1 THEN 14 "
                    +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn+sum_odn) >=0.20 and(sum_odn+nodn)/(sum_tarif+nodn+sum_odn) < 0.25 AND COALESCE(is_dpu,0) = 1 THEN 15 "
                    +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn+sum_odn)>=0.25  AND COALESCE(is_dpu,0) = 1 THEN 16 ELSE 0 end " +
                    "  ) as kod, " +
                    "  count(nzp_dom) as count_dom, sum(count_ls) as count_ls, sum(sum_tarif) as sum_tarif, " +
                    "  sum(sum_odn) as sum_odn, sum(rvaldlt) as rvaldlt, max(tarif) as tarif, " +
                    "  sum(c_calc) as c_calc, sum(nodn) as nodn " +
                    " FROM  tr_210 " +
                    " WHERE sum_tarif>0.0001 GROUP BY 1,2,3,4,5,6 ",
                    schemaName,
                    chargeYearMonth.year,
                    chargeYearMonth.month);
                sqlExecutor.ExecuteSql(sqlQuery);

                sqlQuery = string.Format(
                    " INSERT INTO {0}.resp_odn4 " +
                    "  (nzp_graj, year_, month_,nzp_serv, is_dpu,kod_,    " +
                    "                                count_dom, count_ls, sum_tarif, sum_odn, rvaldlt, " +
                    "                                tarif, c_calc, nodn)                              " +
                    " SELECT nzp_graj, {1}, {2}, 25,is_dpu, " +
                    " (CASE when sum_odn=0 AND COALESCE(is_dpu,0) = 0 THEN 2 " +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn+sum_odn) > 0    and(sum_odn+nodn)/(sum_tarif+nodn+sum_odn) < 0.05 AND COALESCE(is_dpu,0) = 0 THEN 3 "
                    +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn+sum_odn) >=0.05 and(sum_odn+nodn)/(sum_tarif+nodn+sum_odn) < 0.10 AND COALESCE(is_dpu,0) = 0 THEN 4 "
                    +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn+sum_odn) >=0.10 and(sum_odn+nodn)/(sum_tarif+nodn+sum_odn) < 0.15 AND COALESCE(is_dpu,0) = 0 THEN 5 "
                    +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn+sum_odn) >=0.15 and(sum_odn+nodn)/(sum_tarif+nodn+sum_odn) < 0.20 AND COALESCE(is_dpu,0) = 0 THEN 6 "
                    +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn+sum_odn) >=0.20 and(sum_odn+nodn)/(sum_tarif+nodn+sum_odn) < 0.25 AND COALESCE(is_dpu,0) = 0 THEN 7 "
                    +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn+sum_odn)>=0.25  AND COALESCE(is_dpu,0) = 0 THEN 8     " +
                    "       when sum_odn=0 AND COALESCE(is_dpu,0) = 1 THEN 10 " +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn+sum_odn) > 0    and(sum_odn+nodn)/(sum_tarif+nodn+sum_odn) < 0.05 AND COALESCE(is_dpu,0) = 1 THEN 11 "
                    +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn+sum_odn) >=0.05 and(sum_odn+nodn)/(sum_tarif+nodn+sum_odn) < 0.10 AND COALESCE(is_dpu,0) = 1 THEN 12 "
                    +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn+sum_odn) >=0.10 and(sum_odn+nodn)/(sum_tarif+nodn+sum_odn) < 0.15 AND COALESCE(is_dpu,0) = 1 THEN 13 "
                    +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn+sum_odn) >=0.15 and(sum_odn+nodn)/(sum_tarif+nodn+sum_odn) < 0.20 AND COALESCE(is_dpu,0) = 1 THEN 14 "
                    +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn+sum_odn) >=0.20 and(sum_odn+nodn)/(sum_tarif+nodn+sum_odn) < 0.25 AND COALESCE(is_dpu,0) = 1 THEN 15 "
                    +
                    "       when (sum_odn+nodn)/(sum_tarif+nodn+sum_odn)>=0.25  AND COALESCE(is_dpu,0) = 1 THEN 16 ELSE 0 end " +
                    "  ) as kod, " +
                    " count(nzp_dom) as count_dom ,sum(count_ls) as count_ls, sum(sum_tarif) as sum_tarif, " +
                    " sum(sum_odn) as sum_odn, sum(rvaldlt) as rvaldlt, max(tarif) as tarif, " +
                    " sum(c_calc) as c_calc, sum(nodn) as nodn " +
                    " FROM tr_el3 WHERE sum_tarif>0.0001 GROUP BY 1,2,3,4,5,6 ",
                    schemaName,
                    chargeYearMonth.year,
                    chargeYearMonth.month);
                sqlExecutor.ExecuteSql(sqlQuery);

                // загружаем данные для отчета Анализ начислений
                sqlQuery = string.Format(
                    " DELETE FROM {0}.resp_nach " +
                    " WHERE 0 < (SELECT count(*) " +
                    "            FROM {1} " +
                    "            WHERE {1}.yr_={0}.resp_nach.year_               " +
                    "                  AND {1}.mn_={0}.resp_nach.month_          " +
                    "                  AND {1}.nzp_graj={0}.resp_nach.nzp_graj ) ",
                    schemaName,
                    tempTableName);

                // sqlExecutor.ExecuteSql(sqlQuery);
            }

            this.ExecSELECTAnalizOdn(sqlExecutor, schemaName, 210, chargeYearMonth.year, chargeYearMonth.month);
            this.ExecSELECTAnalizOdn(sqlExecutor, schemaName, 6, chargeYearMonth.year, chargeYearMonth.month);
            this.ExecSELECTAnalizOdn(sqlExecutor, schemaName, 9, chargeYearMonth.year, chargeYearMonth.month);
            this.ExecSELECTAnalizOdn(sqlExecutor, schemaName, 7, chargeYearMonth.year, chargeYearMonth.month);
            this.ExecSELECTAnalizOdn(sqlExecutor, schemaName, 25, chargeYearMonth.year, chargeYearMonth.month);
        }

        /// <summary>
        /// Жесть метод
        /// не для слабонервных
        /// </summary>
        /// <param name="sqlExecutor">Объект для работы с БД</param>
        /// <param name="service">Код услуги</param>
        private void ExecSELECTAnalizOdn(
            SqlExecutor sqlExecutor,
            string schemaName,
            int service,
            int chargeYear,
            int chargeMont)
        {
            var sqlQuery = " DROP TABLE IF EXISTS titog; DROP TABLE IF EXISTS tsum ";
            sqlExecutor.ExecuteSql(sqlQuery);

            string str = string.Empty;
            string str1 = string.Empty;

            if (service != 25)
            {
                // результирующий запрос для услуг ХВС, ГВС, Водоотведение, Ночное электроснабжение
                str1 = string.Format(

                    // исправлено в ГИС в связи с отсутствием схемы subs_kernel                    
                    " SELECT t.nzp_graj,TRIM(g.raj) point, ta.area, count(nzp_dom) cnt_doms, max(tarif) tarif,             " +

                    // " SELECT t.nzp_graj,p.point, ta.area, count(nzp_dom) cnt_doms, max(tarif) tarif,             " +
                    "        sum(CASE when (rvaldlt>0 AND is_dpu=1) THEN 1       ELSE 0 end) cnt_doms354pu,      " +
                    "        sum(CASE when (rvaldlt>0 AND is_dpu=1) THEN rvaldlt ELSE 0 end) rvaldlt,            " +
                    "        sum(CASE when (rvaldlt>0 AND is_dpu=1) THEN                                         " +
                    "        sum_ipu+sum_norm+sum_ipud+sum_normd+tarif*(rvaldlt-val_ipu-val_norm-r_ipud-r_normd) " +
                    " ELSE 0 end) nach_dpu,                                                                      " +

                    // 354a
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev=0 THEN 1 ELSE 0 end) cnt_doms354,                                                       "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev=0 THEN cnt_ls_val ELSE 0 end) cnt_ls_val354, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev=0 THEN cnt_pl_pu  ELSE 0 end) cnt_pl_pu354, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev=0 THEN val_ipu    ELSE 0 end) val_ipu354, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev=0 THEN sum_ipu    ELSE 0 end) sum_ipu354, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev=0 THEN r_ipud     ELSE 0 end) r_ipud354, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev=0 THEN sum_ipud   ELSE 0 end) sum_ipud354, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev=0 THEN cnt_ls_norm ELSE 0 end) cnt_ls_norm354, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev=0 THEN cnt_pl_norm ELSE 0 end) cnt_pl_norm354, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev=0 THEN val_norm    ELSE 0 end) val_norm354, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev=0 THEN sum_norm    ELSE 0 end) sum_norm354, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev=0 THEN r_normd     ELSE 0 end) r_normd354, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev=0 THEN sum_normd   ELSE 0 end) sum_normd354, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev=0 THEN val_ipu+val_norm ELSE 0 end) sum_ls354, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev=0 THEN r_ipud+r_normd         ELSE 0 end) r_ls354, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev=0 THEN sum_ipu+sum_ipud+sum_norm+sum_normd ELSE 0 end) nach_ls354, "
                    +

                    // 354b
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev<>0 THEN 1          ELSE 0 end) cnt_doms354b, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev<>0 THEN cnt_ls_val ELSE 0 end) cnt_ls_val354b, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev<>0 THEN cnt_pl_pu  ELSE 0 end) cnt_pl_pu354b, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev<>0 THEN val_ipu    ELSE 0 end) val_ipu354b, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev<>0 THEN sum_ipu    ELSE 0 end) sum_ipu354b, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev<>0 THEN r_ipud     ELSE 0 end) r_ipud354b, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev<>0 THEN sum_ipud   ELSE 0 end) sum_ipud354b, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev<>0 THEN cnt_ls_norm ELSE 0 end) cnt_ls_norm354b, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev<>0 THEN cnt_pl_norm ELSE 0 end) cnt_pl_norm354b, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev<>0 THEN val_norm    ELSE 0 end) val_norm354b, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev<>0 THEN sum_norm    ELSE 0 end) sum_norm354b, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev<>0 THEN r_normd     ELSE 0 end) r_normd354b, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev<>0 THEN sum_normd   ELSE 0 end) sum_normd354b, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev<>0 THEN val_ipu+val_norm ELSE 0 end) sum_ls354b, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev<>0 THEN r_ipud+r_normd         ELSE 0 end) r_ls354b, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev<>0 THEN sum_ipu+sum_ipud+sum_norm+sum_normd ELSE 0 end) nach_ls354b, "
                    +

                    // 344
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)>0.000001 THEN 1          ELSE 0 end) cnt_doms354p, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)>0.000001 THEN cnt_ls_val ELSE 0 end) cnt_ls_val354p, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)>0.000001 THEN cnt_pl_pu  ELSE 0 end) cnt_pl_pu354p, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)>0.000001 THEN val_ipu    ELSE 0 end) val_ipu354p, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)>0.000001 THEN sum_ipu    ELSE 0 end) sum_ipu354p, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)>0.000001 THEN r_ipud     ELSE 0 end) r_ipud354p, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)>0.000001 THEN sum_ipud   ELSE 0 end) sum_ipud354p, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)>0.000001 THEN cnt_ls_norm ELSE 0 end) cnt_ls_norm354p, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)>0.000001 THEN cnt_pl_norm ELSE 0 end) cnt_pl_norm354p, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)>0.000001 THEN val_norm    ELSE 0 end) val_norm354p, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)>0.000001 THEN sum_norm    ELSE 0 end) sum_norm354p, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)>0.000001 THEN r_normd     ELSE 0 end) r_normd354p, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)>0.000001 THEN sum_normd   ELSE 0 end) sum_normd354p, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)>0.000001 THEN val_ipu+val_norm ELSE 0 end) sum_ls354p, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)>0.000001 AND rvaldlt-val_ipu-val_norm>0 THEN rvaldlt-val_ipu-val_norm ELSE 0 end) sum_ls354odnp, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)>0.000001 THEN r_ipud+r_normd         ELSE 0 end) r_ls354p, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)>0.000001 AND rvaldlt-val_ipu-val_norm>0 THEN rvaldlt-val_ipu-val_norm-r_ipud-r_normd ELSE 0 end) sum_ls354dltp, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)>0.000001 AND rvaldlt-val_ipu-val_norm>0 THEN tarif2*(rvaldlt-val_ipu-val_norm-r_ipud-r_normd) ELSE 0 end) sum_ls354dltsp, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)>0.000001 THEN rvaldlt    ELSE 0 end) rvaldlt344, "
                    +

                    // 344(rt)
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg = 28 THEN 1          ELSE 0 end) cnt_doms354rt, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg = 28 THEN cnt_ls_val ELSE 0 end) cnt_ls_val354rt, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg = 28 THEN cnt_pl_pu  ELSE 0 end) cnt_pl_pu354rt, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg = 28 THEN val_ipu    ELSE 0 end) val_ipu354rt, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg = 28 THEN sum_ipu    ELSE 0 end) sum_ipu354rt, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg = 28 THEN r_ipud     ELSE 0 end) r_ipud354rt, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg = 28 THEN sum_ipud   ELSE 0 end) sum_ipud354rt, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg = 28 THEN cnt_ls_norm ELSE 0 end) cnt_ls_norm354rt, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg = 28 THEN cnt_pl_norm ELSE 0 end) cnt_pl_norm354rt, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg = 28 THEN val_norm    ELSE 0 end) val_norm354rt, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg = 28 THEN sum_norm    ELSE 0 end) sum_norm354rt, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg = 28 THEN r_normd     ELSE 0 end) r_normd354rt, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg = 28 THEN sum_normd   ELSE 0 end) sum_normd354rt, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg = 28 THEN val_ipu+val_norm ELSE 0 end) sum_ls354rt, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg = 28 THEN r_ipud+r_normd         ELSE 0 end) r_ls354rt, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg = 28 THEN sum_ipu+sum_ipud+sum_norm+sum_normd ELSE 0 end) nach_ls354rt, " +

                    // norma
                    // " sum(CASE when not is_dpu=1 THEN 1          ELSE 0 end) cnt_doms354n,      "+
                    " sum(CASE when not(rvaldlt>0 AND is_dpu=1) THEN 1          ELSE 0 end) cnt_doms354n,      " +
                    " sum(CASE when not(rvaldlt>0 AND is_dpu=1) THEN cntls_ipu_serv ELSE 0 end) cnt_ls_val354n," +
                    " sum(CASE when not(rvaldlt>0 AND is_dpu=1) THEN s_ipu_serv  ELSE 0 end) cnt_pl_pu354n,    " +
                    " sum(CASE when not(rvaldlt>0 AND is_dpu=1) THEN val_ipu    ELSE 0 end) val_ipu354n,       " +
                    " sum(CASE when not(rvaldlt>0 AND is_dpu=1) THEN sum_ipu    ELSE 0 end) sum_ipu354n,       " +
                    " sum(CASE when not(rvaldlt>0 AND is_dpu=1) THEN r_ipud     ELSE 0 end) r_ipud354n,        " +
                    " sum(CASE when not(rvaldlt>0 AND is_dpu=1) THEN sum_ipud   ELSE 0 end) sum_ipud354n,      " +
                    " sum(CASE when not(rvaldlt>0 AND is_dpu=1) THEN cntls_norm_serv ELSE 0 end) cnt_ls_norm354n," +
                    " sum(CASE when not(rvaldlt>0 AND is_dpu=1) THEN s_norm_serv ELSE 0 end) cnt_pl_norm354n,    " +
                    " sum(CASE when not(rvaldlt>0 AND is_dpu=1) THEN val_norm    ELSE 0 end) val_norm354n,       " +
                    " sum(CASE when not(rvaldlt>0 AND is_dpu=1) THEN sum_norm    ELSE 0 end) sum_norm354n,       " +
                    " sum(CASE when not(rvaldlt>0 AND is_dpu=1) THEN r_normd     ELSE 0 end) r_normd354n,        " +
                    " sum(CASE when not(rvaldlt>0 AND is_dpu=1) THEN sum_normd   ELSE 0 end) sum_normd354n,      " +
                    " sum(CASE when not(rvaldlt>0 AND is_dpu=1) THEN sum_ls_val+sum_ls_norm ELSE 0 end) sum_ls354n, " +
                    " sum(CASE when not(rvaldlt>0 AND is_dpu=1) THEN r_ipud+r_normd         ELSE 0 end) r_ls354n, " +
                    " sum(CASE when not(rvaldlt>0 AND is_dpu=1) THEN sum_ipu+sum_ipud+sum_norm+sum_normd ELSE 0 end) nach_ls354n " +
                    " INTO TEMP titog   " +
                    " FROM tall1 t, ta,                  " +

                    // исправлено в ГИС в связи с отсутствием схемы subs_kernel : добавлен inner join 
                    // "      {0}.s_point p,     " +
                    " {0}.subs_rajon p  " +
                    " INNER JOIN {0}.subs_rajon_gor g ON (g.nzp_raj = p.nzp_raj)",
                    schemaName);
            }

            if (service == 25)
            {
                // результирующий запрос для услуги электроснабжение
                sqlQuery = string.Format(

                    // исправлено в ГИС в связи с отсутствием схемы subs_kernel                    
                    " SELECT t.nzp_graj,TRIM(g.raj) point, ta.area, count(nzp_dom) cnt_doms, max(tarif) tarif,             " +

                    // " SELECT t.nzp_graj,p.point, ta.area,count(*) cnt_doms, max(tarif) tarif, " +
                    // " sum(CASE when is_dpu=1 THEN 1       ELSE 0 end) cnt_doms354pu, "+
                    " sum(CASE when (rvaldlt>0 AND is_dpu=1) THEN 1       ELSE 0 end) cnt_doms354pu, " +
                    " sum(CASE when (rvaldlt>0 AND is_dpu=1) THEN rvaldlt ELSE 0 end) rvaldlt, " +
                    " sum(CASE when (rvaldlt>0 AND is_dpu=1) THEN " +
                    " sum_ipu+sum_norm+sum_ipud+sum_normd+sum_ls_210vals+sum_ls_210norms+tarif*(rvaldlt-val_ipu-val_norm-r_ipud-r_normd-sum_ls_210val-sum_ls_210norm) "
                    +
                    " ELSE 0 end) nach_dpu, " +

                    // 354a
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 " +
                    " AND is_prev=0 THEN 1          ELSE 0 end) cnt_doms354, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 " +
                    " AND is_prev=0 THEN cnt_ls_val ELSE 0 end) cnt_ls_val354, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 " +
                    " AND is_prev=0 THEN cnt_pl_pu  ELSE 0 end) cnt_pl_pu354, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 " +
                    " AND is_prev=0 THEN val_ipu+sum_ls_210val    ELSE 0 end) val_ipu354, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 " +
                    " AND is_prev=0 THEN sum_ipu+sum_ls_210vals   ELSE 0 end) sum_ipu354, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 " +
                    " AND is_prev=0 THEN r_ipud     ELSE 0 end) r_ipud354, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 " +
                    " AND is_prev=0 THEN sum_ipud   ELSE 0 end) sum_ipud354, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 " +
                    " AND is_prev=0 THEN cnt_ls_norm ELSE 0 end) cnt_ls_norm354, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 " +
                    " AND is_prev=0 THEN cnt_pl_norm ELSE 0 end) cnt_pl_norm354, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 " +
                    " AND is_prev=0 THEN val_norm+sum_ls_210norm    ELSE 0 end) val_norm354, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 " +
                    " AND is_prev=0 THEN sum_norm+sum_ls_210norms   ELSE 0 end) sum_norm354, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 " +
                    " AND is_prev=0 THEN r_normd     ELSE 0 end) r_normd354, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 " +
                    " AND is_prev=0 THEN sum_normd   ELSE 0 end) sum_normd354, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 " +
                    " AND is_prev=0 THEN val_ipu+val_norm+sum_ls_210val+sum_ls_210norm ELSE 0 end) sum_ls354, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 " +
                    " AND is_prev=0 THEN r_ipud+r_normd ELSE 0 end) r_ls354, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 " +
                    " AND is_prev=0 THEN sum_ipu+sum_ipud+sum_norm+sum_normd+sum_ls_210vals+sum_ls_210norms ELSE 0 end) nach_ls354, " +

                    // 354b
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev<>0 THEN 1          ELSE 0 end) cnt_doms354b, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev<>0 THEN cnt_ls_val ELSE 0 end) cnt_ls_val354b, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev<>0 THEN cnt_pl_pu  ELSE 0 end) cnt_pl_pu354b, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev<>0 THEN val_ipu+sum_ls_210val     ELSE 0 end) val_ipu354b, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev<>0 THEN sum_ipu+sum_ls_210vals    ELSE 0 end) sum_ipu354b, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev<>0 THEN r_ipud     ELSE 0 end) r_ipud354b, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev<>0 THEN sum_ipud   ELSE 0 end) sum_ipud354b, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev<>0 THEN cnt_ls_norm ELSE 0 end) cnt_ls_norm354b, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev<>0 THEN cnt_pl_norm ELSE 0 end) cnt_pl_norm354b, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev<>0 THEN val_norm+sum_ls_210norm     ELSE 0 end) val_norm354b, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev<>0 THEN sum_norm+sum_ls_210norms    ELSE 0 end) sum_norm354b, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev<>0 THEN r_normd     ELSE 0 end) r_normd354b, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev<>0 THEN sum_normd   ELSE 0 end) sum_normd354b, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev<>0 THEN val_ipu+val_norm+sum_ls_210val+sum_ls_210norm ELSE 0 end) sum_ls354b, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev<>0 THEN r_ipud+r_normd         ELSE 0 end) r_ls354b, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)<=0.000001 AND is_prev<>0 THEN sum_ipu+sum_ipud+sum_norm+sum_normd+sum_ls_210vals+sum_ls_210norms ELSE 0 end) nach_ls354b, "
                    +

                    // 344
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)>0.000001 THEN 1          ELSE 0 end) cnt_doms354p, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)>0.000001 THEN cnt_ls_val ELSE 0 end) cnt_ls_val354p, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)>0.000001 THEN cnt_pl_pu  ELSE 0 end) cnt_pl_pu354p, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)>0.000001 THEN val_ipu+sum_ls_210val     ELSE 0 end) val_ipu354p, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)>0.000001 THEN sum_ipu+sum_ls_210vals    ELSE 0 end) sum_ipu354p, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)>0.000001 THEN r_ipud     ELSE 0 end) r_ipud354p, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)>0.000001 THEN sum_ipud   ELSE 0 end) sum_ipud354p, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)>0.000001 THEN cnt_ls_norm ELSE 0 end) cnt_ls_norm354p, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)>0.000001 THEN cnt_pl_norm ELSE 0 end) cnt_pl_norm354p, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)>0.000001 THEN val_norm+sum_ls_210norm     ELSE 0 end) val_norm354p, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)>0.000001 THEN sum_norm+sum_ls_210norms    ELSE 0 end) sum_norm354p, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)>0.000001 THEN r_normd     ELSE 0 end) r_normd354p, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)>0.000001 THEN sum_normd   ELSE 0 end) sum_normd354p, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)>0.000001 THEN val_ipu+val_norm+sum_ls_210val+sum_ls_210norm ELSE 0 end) sum_ls354p, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)>0.000001 AND rvaldlt-val_ipu-val_norm-sum_ls_210val-sum_ls_210norm>0 "
                    +
                    " THEN rvaldlt-val_ipu-val_norm-sum_ls_210val-sum_ls_210norm ELSE 0 end) sum_ls354odnp, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28  AND COALESCE(odn_prev,0)>0.000001 THEN r_ipud+r_normd         ELSE 0 end) r_ls354p, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)>0.000001 AND rvaldlt-val_ipu-val_norm-sum_ls_210val-sum_ls_210norm>0 "
                    +
                    " THEN rvaldlt-val_ipu-val_norm-r_ipud-r_normd-sum_ls_210val-sum_ls_210norm ELSE 0 end) sum_ls354dltp, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND nzp_type_alg <> 28 AND COALESCE(odn_prev,0)>0.000001 " +
                    "               AND rvaldlt-val_ipu-val_norm-sum_ls_210val-sum_ls_210norm>0 THEN tarif2*(rvaldlt-val_ipu-val_norm-r_ipud-r_normd-sum_ls_210val-sum_ls_210norm) ELSE 0 end) sum_ls354dltsp, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg<100 AND COALESCE(odn_prev,0)>0.000001 THEN rvaldlt    ELSE 0 end) rvaldlt344, "
                    +

                    // 344(rt)
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg = 28 THEN 1          ELSE 0 end) cnt_doms354rt, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg = 28 THEN cnt_ls_val ELSE 0 end) cnt_ls_val354rt, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg = 28 THEN cnt_pl_pu  ELSE 0 end) cnt_pl_pu354rt, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg = 28 THEN val_ipu+sum_ls_210val     ELSE 0 end) val_ipu354rt, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg = 28 THEN sum_ipu+sum_ls_210vals    ELSE 0 end) sum_ipu354rt, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg = 28 THEN r_ipud     ELSE 0 end) r_ipud354rt, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg = 28 THEN sum_ipud   ELSE 0 end) sum_ipud354rt, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg = 28 THEN cnt_ls_norm ELSE 0 end) cnt_ls_norm354rt, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg = 28 THEN cnt_pl_norm ELSE 0 end) cnt_pl_norm354rt, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg = 28 THEN val_norm+sum_ls_210norm     ELSE 0 end) val_norm354rt, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg = 28 THEN sum_norm+sum_ls_210norms    ELSE 0 end) sum_norm354rt, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg = 28 THEN r_normd     ELSE 0 end) r_normd354rt, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg = 28 THEN sum_normd   ELSE 0 end) sum_normd354rt, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg = 28 THEN val_ipu+val_norm+sum_ls_210val+sum_ls_210norm ELSE 0 end) sum_ls354rt, "
                    +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg = 28 THEN r_ipud+r_normd         ELSE 0 end) r_ls354rt, " +
                    " sum(CASE when rvaldlt>0 AND is_dpu=1 AND nzp_type_alg = 28 THEN sum_ipu+sum_ipud+sum_norm+sum_normd+sum_ls_210vals+sum_ls_210norms ELSE 0 end) nach_ls354rt, "
                    +

                    // norma
                    // " sum(CASE when not is_dpu=1 THEN 1              ELSE 0 end) cnt_doms354n, "+
                    " sum(CASE when not(rvaldlt>0 AND is_dpu=1) THEN 1              ELSE 0 end) cnt_doms354n, " +
                    " sum(CASE when not(rvaldlt>0 AND is_dpu=1) THEN cntls_ipu_serv ELSE 0 end) cnt_ls_val354n, " +
                    " sum(CASE when not(rvaldlt>0 AND is_dpu=1) THEN s_ipu_serv     ELSE 0 end) cnt_pl_pu354n, " +
                    " sum(CASE when not(rvaldlt>0 AND is_dpu=1) THEN val_ipu        ELSE 0 end) val_ipu354n, " +
                    " sum(CASE when not(rvaldlt>0 AND is_dpu=1) THEN sum_ipu        ELSE 0 end) sum_ipu354n, " +
                    " sum(CASE when not(rvaldlt>0 AND is_dpu=1) THEN r_ipud         ELSE 0 end) r_ipud354n, " +
                    " sum(CASE when not(rvaldlt>0 AND is_dpu=1) THEN sum_ipud       ELSE 0 end) sum_ipud354n, " +
                    " sum(CASE when not(rvaldlt>0 AND is_dpu=1) THEN cntls_norm_serv ELSE 0 end) cnt_ls_norm354n, " +
                    " sum(CASE when not(rvaldlt>0 AND is_dpu=1) THEN s_norm_serv ELSE 0 end) cnt_pl_norm354n, " +
                    " sum(CASE when not(rvaldlt>0 AND is_dpu=1) THEN val_norm    ELSE 0 end) val_norm354n, " +
                    " sum(CASE when not(rvaldlt>0 AND is_dpu=1) THEN sum_norm    ELSE 0 end) sum_norm354n, " +
                    " sum(CASE when not(rvaldlt>0 AND is_dpu=1) THEN r_normd     ELSE 0 end) r_normd354n, " +
                    " sum(CASE when not(rvaldlt>0 AND is_dpu=1) THEN sum_normd   ELSE 0 end) sum_normd354n, " +
                    " sum(CASE when not(rvaldlt>0 AND is_dpu=1) THEN sum_ls_val+sum_ls_norm ELSE 0 end) sum_ls354n, " +
                    " sum(CASE when not(rvaldlt>0 AND is_dpu=1) THEN r_ipud+r_normd         ELSE 0 end) r_ls354n, " +
                    " sum(CASE when not(rvaldlt>0 AND is_dpu=1) THEN sum_ipu+sum_ipud+sum_norm+sum_normd ELSE 0 end) nach_ls354n " +
                    " INTO TEMP titog   " +
                    " FROM tall1 t, ta,                  " +

                    // исправлено в ГИС в связи с отсутствием схемы subs_kernel : добавлен inner join 
                    // "      {0}.s_point p,     " +
                    " {0}.subs_rajon p  " +
                    " INNER JOIN {0}.subs_rajon_gor g ON (g.nzp_raj = p.nzp_raj) " +
                    " WHERE t.nzp_serv=25 AND t.nzp_graj=p.nzp_graj AND t.nzp_graj=ta.nzp_graj AND t.nzp_area=ta.nzp_area AND t.cntls_serv>1 AND t.sum_tarif > 0.0001 "
                    +
                    " GROUP BY 1,2,3 ",
                    schemaName);

                sqlExecutor.ExecuteSql(sqlQuery);
            }

            if (service == 210)
            {
                str = " WHERE t.nzp_serv=210 AND t.nzp_graj=p.nzp_graj AND t.nzp_graj=ta.nzp_graj AND " +
                    "       t.nzp_area=ta.nzp_area AND t.nzp_type_alg in (22,122) AND " +
                    "       t.cntls_serv>1 AND t.sum_tarif > 0.0001 " +
                    " GROUP BY 1,2,3                                ";
            }

            if (service == 6)
            {
                str = " WHERE t.nzp_serv=6 AND t.nzp_graj=p.nzp_graj AND t.nzp_graj=ta.nzp_graj AND " +
                    " t.nzp_area=ta.nzp_area AND t.cntls_serv>1  AND t.sum_tarif > 0.0001  " +
                    " GROUP BY 1,2,3 ";
            }

            if (service == 9)
            {
                str = " WHERE t.nzp_serv=9 AND t.nzp_graj=p.nzp_graj AND t.nzp_graj=ta.nzp_graj AND " +
                    " t.nzp_area=ta.nzp_area AND t.cntls_serv>1 AND t.sum_tarif > 0.0001  " +
                    " GROUP BY 1,2,3 ";
            }

            if (service == 7)
            {
                str = " WHERE t.nzp_serv=7 AND t.nzp_graj=p.nzp_graj AND t.nzp_graj=ta.nzp_graj AND " +
                    " t.nzp_area=ta.nzp_area AND t.cntls_serv>1 AND t.sum_tarif > 0.0001 " +
                    " GROUP BY 1,2,3 ";
            }

            if (service != 25)
            {
                sqlQuery = str1 + str;
                sqlExecutor.ExecuteSql(sqlQuery);
            }

            sqlQuery =
                " SELECT distinct nzp_graj,point, 'Всего'::text area, sum(cnt_doms) cnt_doms, 0.00 tarif, " +
                " sum(cnt_doms354pu) cnt_doms354pu, sum(rvaldlt) rvaldlt, sum(nach_dpu) nach_dpu, " +

                // 354a
                " sum(cnt_doms354  ) cnt_doms354  , sum(cnt_ls_val354) cnt_ls_val354, " +
                " sum(cnt_pl_pu354 ) cnt_pl_pu354 , sum(val_ipu354   ) val_ipu354   , " +
                " sum(sum_ipu354   ) sum_ipu354   , sum(r_ipud354    ) r_ipud354    , " +
                " sum(sum_ipud354  ) sum_ipud354  , sum(cnt_ls_norm354) cnt_ls_norm354,   " +
                " sum(cnt_pl_norm354) cnt_pl_norm354, sum(val_norm354   ) val_norm354   , " +
                " sum(sum_norm354   ) sum_norm354   , sum(r_normd354    ) r_normd354    , " +
                " sum(sum_normd354  ) sum_normd354  , sum(sum_ls354)  sum_ls354, sum(r_ls354) r_ls354, " +
                " sum(nach_ls354) nach_ls354, " +

                // 354b
                " sum(cnt_doms354b  ) cnt_doms354b  , sum(cnt_ls_val354b) cnt_ls_val354b, " +
                " sum(cnt_pl_pu354b ) cnt_pl_pu354b , sum(val_ipu354b   ) val_ipu354b   , " +
                " sum(sum_ipu354b   ) sum_ipu354b   , sum(r_ipud354b    ) r_ipud354b    , " +
                " sum(sum_ipud354b  ) sum_ipud354b  , sum(cnt_ls_norm354b) cnt_ls_norm354b, " +
                " sum(cnt_pl_norm354b) cnt_pl_norm354b, sum(val_norm354b   ) val_norm354b   , " +
                " sum(sum_norm354b   ) sum_norm354b   , sum(r_normd354b    ) r_normd354b    , " +
                " sum(sum_normd354b  ) sum_normd354b  , sum(sum_ls354b) sum_ls354b, " +
                " sum(r_ls354b) r_ls354b, sum(nach_ls354b) nach_ls354b, " +
                " sum(cnt_doms354p  ) cnt_doms354p  , sum(cnt_ls_val354p) cnt_ls_val354p, " +
                " sum(cnt_pl_pu354p ) cnt_pl_pu354p , sum(val_ipu354p   ) val_ipu354p   , " +
                " sum(sum_ipu354p   ) sum_ipu354p   , sum(r_ipud354p    ) r_ipud354p    , " +
                " sum(sum_ipud354p  ) sum_ipud354p  , sum(cnt_ls_norm354p) cnt_ls_norm354p, " +
                " sum(cnt_pl_norm354p) cnt_pl_norm354p, sum(val_norm354p   ) val_norm354p   , " +
                " sum(sum_norm354p   ) sum_norm354p   , sum(r_normd354p    ) r_normd354p    , " +
                " sum(sum_normd354p  ) sum_normd354p  , sum(sum_ls354p) sum_ls354p, " +
                " sum(sum_ls354odnp) sum_ls354odnp, sum(r_ls354p) r_ls354p, " +
                " sum(sum_ls354dltp) sum_ls354dltp, sum(sum_ls354dltsp) sum_ls354dltsp, " +
                " sum(rvaldlt344) rvaldlt344, " +

                // 344(rt)
                " sum(cnt_doms354rt  ) cnt_doms354rt  , sum(cnt_ls_val354rt) cnt_ls_val354rt, " +
                " sum(cnt_pl_pu354rt ) cnt_pl_pu354rt , sum(val_ipu354rt   ) val_ipu354rt   , " +
                " sum(sum_ipu354rt   ) sum_ipu354rt   , sum(r_ipud354rt    ) r_ipud354rt    , " +
                " sum(sum_ipud354rt  ) sum_ipud354rt  , sum(cnt_ls_norm354rt) cnt_ls_norm354rt, " +
                " sum(cnt_pl_norm354rt) cnt_pl_norm354rt, sum(val_norm354rt   ) val_norm354rt   , " +
                " sum(sum_norm354rt   ) sum_norm354rt   , sum(r_normd354rt    ) r_normd354rt    , " +
                " sum(sum_normd354rt  ) sum_normd354rt  , sum(sum_ls354rt ) sum_ls354rt , " +
                " sum(r_ls354rt   ) r_ls354rt   , sum(nach_ls354rt) nach_ls354rt, " +

                // norma
                " sum(cnt_doms354n  ) cnt_doms354n  , sum(cnt_ls_val354n) cnt_ls_val354n, " +
                " sum(cnt_pl_pu354n ) cnt_pl_pu354n , sum(val_ipu354n   ) val_ipu354n   , " +
                " sum(sum_ipu354n   ) sum_ipu354n   , sum(r_ipud354n    ) r_ipud354n    , " +
                " sum(sum_ipud354n  ) sum_ipud354n  , sum(cnt_ls_norm354n) cnt_ls_norm354n, " +
                " sum(cnt_pl_norm354n) cnt_pl_norm354n, sum(val_norm354n   ) val_norm354n   , " +
                " sum(sum_norm354n   ) sum_norm354n   , sum(r_normd354n    ) r_normd354n    , " +
                " sum(sum_normd354n  ) sum_normd354n  , sum(sum_ls354n ) sum_ls354n , " +
                " sum(r_ls354n   ) r_ls354n   , sum(nach_ls354n) nach_ls354n " +
                " INTO TEMP tsum  " +
                " FROM titog " +
                " GROUP BY 1,2 ";
            sqlExecutor.ExecuteSql(sqlQuery);

            sqlQuery = " DROP TABLE IF EXISTS temp_titog ";
            sqlExecutor.ExecuteSql(sqlQuery);

            sqlQuery =
                @" SELECT nzp_graj, point, area, cnt_doms, tarif, cnt_doms354pu, rvaldlt, nach_dpu, cnt_doms354, cnt_ls_val354, cnt_pl_pu354, val_ipu354, sum_ipu354, r_ipud354, sum_ipud354, cnt_ls_norm354,"
                +
                "        cnt_pl_norm354, val_norm354, sum_norm354, r_normd354, sum_normd354, sum_ls354, r_ls354, nach_ls354, cnt_doms354b, cnt_ls_val354b, cnt_pl_pu354b, val_ipu354b, sum_ipu354b, r_ipud354b, sum_ipud354b,"
                +
                "        cnt_ls_norm354b, cnt_pl_norm354b, val_norm354b, sum_norm354b, r_normd354b, sum_normd354b, sum_ls354b, r_ls354b, nach_ls354b, cnt_doms354p, cnt_ls_val354p, cnt_pl_pu354p, val_ipu354p, sum_ipu354p,"
                +
                "        r_ipud354p, sum_ipud354p, cnt_ls_norm354p, cnt_pl_norm354p, val_norm354p, sum_norm354p, r_normd354p, sum_normd354p, sum_ls354p, sum_ls354odnp, r_ls354p, sum_ls354dltp, sum_ls354dltsp, rvaldlt344,"
                +
                "        cnt_doms354rt, cnt_ls_val354rt, cnt_pl_pu354rt, val_ipu354rt, sum_ipu354rt, r_ipud354rt, sum_ipud354rt, cnt_ls_norm354rt, cnt_pl_norm354rt, val_norm354rt, sum_norm354rt, r_normd354rt, sum_normd354rt,"
                +
                "        sum_ls354rt, r_ls354rt, nach_ls354rt, cnt_doms354n, cnt_ls_val354n, cnt_pl_pu354n, val_ipu354n, sum_ipu354n, r_ipud354n, sum_ipud354n, cnt_ls_norm354n, cnt_pl_norm354n, val_norm354n, sum_norm354n,"
                +
                "        r_normd354n, sum_normd354n, sum_ls354n, r_ls354n, nach_ls354n " +
                " INTO TEMP temp_titog   " +
                "  FROM titog " +
                "  union all  " +
                "  SELECT nzp_graj, point, area, cnt_doms, tarif, cnt_doms354pu, rvaldlt, nach_dpu, cnt_doms354, cnt_ls_val354, cnt_pl_pu354, val_ipu354, sum_ipu354, r_ipud354, sum_ipud354, cnt_ls_norm354,"
                +
                "         cnt_pl_norm354, val_norm354, sum_norm354, r_normd354, sum_normd354, sum_ls354, r_ls354, nach_ls354, cnt_doms354b, cnt_ls_val354b, cnt_pl_pu354b, val_ipu354b, sum_ipu354b,r_ipud354b, sum_ipud354b,"
                +
                "         cnt_ls_norm354b, cnt_pl_norm354b, val_norm354b, sum_norm354b, r_normd354b, sum_normd354b, sum_ls354b, r_ls354b, nach_ls354b, cnt_doms354p, cnt_ls_val354p, cnt_pl_pu354p, val_ipu354p, sum_ipu354p,"
                +
                "         r_ipud354p, sum_ipud354p, cnt_ls_norm354p, cnt_pl_norm354p, val_norm354p, sum_norm354p, r_normd354p, sum_normd354p, sum_ls354p, sum_ls354odnp, r_ls354p, sum_ls354dltp, sum_ls354dltsp, rvaldlt344,"
                +
                "         cnt_doms354rt, cnt_ls_val354rt, cnt_pl_pu354rt, val_ipu354rt, sum_ipu354rt, r_ipud354rt, sum_ipud354rt, cnt_ls_norm354rt, cnt_pl_norm354rt, val_norm354rt, sum_norm354rt, r_normd354rt, sum_normd354rt,"
                +
                "         sum_ls354rt, r_ls354rt, nach_ls354rt, cnt_doms354n, cnt_ls_val354n, cnt_pl_pu354n, val_ipu354n, sum_ipu354n, r_ipud354n, sum_ipud354n, cnt_ls_norm354n, cnt_pl_norm354n, val_norm354n, sum_norm354n,"
                +
                "         r_normd354n, sum_normd354n, sum_ls354n, r_ls354n, nach_ls354n " +
                "  FROM tsum " +
                "  order by 2,3 ";
            sqlExecutor.ExecuteSql(sqlQuery);

            sqlQuery = string.Format(
                " INSERT INTO {0}.resp_nach " +
                " SELECT *, {1},{2}, {3} " +
                " FROM temp_titog  ",
                schemaName,
                chargeYear,
                chargeMont,
                service);
            sqlExecutor.ExecuteSql(sqlQuery);
        }

        /// <summary>
        /// Вспомогательная структура для переноса начислений
        /// </summary>
        private struct ChargeYearMonth
        {
            public int month;
            public int year;
        }

        /// <summary>
        /// Проверка исключения
        /// </summary>
        private void CheckException(Exception e)
        {
            // Если ошибка PostgresSql с кодом "42P01" (нужная таблица ещё не создана), то пропускаем
            var exception = e.InnerException as PostgresException;
            if (exception == null || exception.SqlState != "42P01")
            {
                throw e;
            }
        }
    }
}