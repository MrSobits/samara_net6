using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Bars.B4;
using Bars.B4.Modules.FileStorage;
using Bars.Gkh.Gis.DomainService.JExtractor;
using Bars.Gkh.Gis.DomainService.JExtractor.Impl;
using Bars.Gkh.Gis.Entities.Register.LoadedFileRegister;
using Bars.Gkh.Gis.Enum;
using Bars.Gkh.Gis.Utils;
using Npgsql;

namespace Bars.Gkh.Gis.DomainService.ImportData.Impl
{
    using System.Collections.Generic;

    using Bars.Gkh.Gis.DomainService.BilConnection;

    using DataResult;

    /// <summary>
    /// Класс загрузки данных СЗ для отчетов для Минстроя
    /// </summary>
    public class ImportSzDataForMinstroyReportsHandler : BaseImportDataHandler
    {
        private TypeStatus _resultStatus;

        private readonly StringBuilder _importLog;

        public ImportSzDataForMinstroyReportsHandler()
        {
            //загружаемые файлы приходят в формате ".J", 
            //поэтому создаем инстанс разархиватора
            JExtractor = new JExtractorService();
            _importLog = new StringBuilder();
        }


        public IFileManager FileManager { get; set; }

        public IBilConnectionService BilConnectionService { get; set; }
        protected IJExtractorService JExtractor;

        public override IEnumerable<ImportDataResult> ImportData(IEnumerable<LoadedFileRegister> loadedFile)
        {
            return loadedFile.Select(ImportData);
        }

        public override ImportDataResult ImportData(LoadedFileRegister loadedFileId)
        {
            //директория для распаковки (директория создастся в процессе распаковки)
            var extractionDirectory = "";

            try
            {
                _importLog.Clear();
                _importLog.AppendLine("Дата загрузки: " + DateTime.Now);
                _importLog.AppendLine("Тип загрузки: 'Загрузка данных СЗ для отчетов'");
                _importLog.AppendLine("Наименование файла: " + loadedFileId.File.FullName);

                //получаем полный путь к архиву
                var archive = FileManager.LoadFile(loadedFileId.File.Id);

                extractionDirectory = Path.Combine(Path.GetDirectoryName(archive.Path), "temp" + DateTime.Now.Ticks);

                //распаковываем архив
                var extraction = JExtractor.ExtractToDirectory(archive.Path, extractionDirectory);
                if (!extraction.Success)
                {
                    throw new Exception(String.Format("Не удалось распаковать архив: {0}", extraction.Message));
                }

                //список успешно загруженных файлов
                var loadedFiles = new StringBuilder();

                //схема для загрузки СЗ
                const string schemaName = "sz";

                //дата расчетного  месяца
                DateTime calculationDate;

                //месяц расчета
                int calculationMonth;

                //год расчета
                int calculationYear;

                //код района
                int districtCode;

                //наименование района
                string districtName;

                //уникальный код ресурса данных
                int nzpSource;

                var sqlQuery = "";

                //заголовочный информационный файл
                var headerFile = extractionDirectory + @"\_link.msg";

                if (!File.Exists(headerFile))
                    throw new Exception(String.Format("Не найден заголовочный файл '{0}'", headerFile));

                //Разбираем заголовок
                using (var streamReader = new StreamReader(
                    File.OpenRead(headerFile),
                    Encoding.GetEncoding(1251)))
                {
                    var line = streamReader.ReadLine();
                    if (line == null)
                    {
                        throw new Exception("Файл пуст");
                    }
                    if (!Regex.IsMatch(line, "^beg=Адресная социальная помощь населению|szn|"))
                    {
                        throw new Exception("Неверный заголовок");
                    }

                    //разделитель полей 
                    var fields = line.Split('|');

                    //считываем месяц расчета
                    calculationMonth = Convert.ToInt32(fields[3]);

                    //считываем год расчета
                    calculationYear = Convert.ToInt32(fields[2]);

                    //из полей собираем расчетный месяц
                    calculationDate = new DateTime(calculationYear, calculationMonth, 1);

                    //считываем код района
                    districtCode = Convert.ToInt32(fields[4]);

                    //считываем наименование района
                    districtName = fields[5];
                }


                _importLog.AppendLine(String.Format("Код района: '{0}' ", districtCode));
                _importLog.AppendLine(String.Format("Наименование района: '{0}' ", districtName));
                _importLog.AppendLine(String.Format("Месяц расчета: '{0}' ", calculationMonth));
                _importLog.AppendLine(String.Format("Год расчета: '{0}' ", calculationYear));
                _importLog.AppendLine();



                //открываем соединение к БД с отчетами
                using (var executor = new SqlExecutor.SqlExecutor(this.BilConnectionService.GetConnection(ConnectionType.GisConnStringReports)))
                {

                    //определение nzp_source
                    sqlQuery = String.Format(
                        " SELECT nzp_source AS NzpSource, fl_load AS FlLoad" +
                        " FROM {0}.ods_source " +

                        //изменил на kod_org, правильно ли это? 
                        //" WHERE nzp_point = {1}" +
                        " WHERE kod_org = {1}" +
                        " AND year_= {2}" +
                        " AND month_= {3}",
                        schemaName, districtCode, calculationYear, calculationMonth);

                    var data = executor.ExecuteSql<OdsSourceEntity>(sqlQuery);

                    if (data.Count() != 0)
                    {
                        nzpSource = data.FirstOrDefault().NzpSource;
                        if (data.FirstOrDefault().FlLoad == 1)
                        {
                            throw new Exception("Данные успешно загружены ранее!");
                        }

                        sqlQuery = String.Format("UPDATE {0}.ods_source SET fl_load=-1 WHERE nzp_source={1}", schemaName,
                            nzpSource);
                        executor.ExecuteSql(sqlQuery);

                    }
                    else
                    {
                        sqlQuery = String.Format(
                            " INSERT INTO {2}.ods_source " +
                            "        (source_name, org_name, kod_org, month_, year_, fl_load, nzp_point) " +
                            " SELECT s.system_name, p.point, p.kod, {0},{1},  -1, p.nzp_point " +
                            " FROM {2}.s_point p, " +
                            "      {2}.s_system s " +
                            " WHERE s.nzp_system = p.nzp_system " +

                            //изменил на kod, правильно ли это? 
                            //" AND p.nzp_point = {3}",
                            " AND p.kod = {3}",
                            calculationMonth,
                            calculationYear,
                            schemaName,
                            districtCode
                            );
                        executor.ExecuteSql(sqlQuery);

                        //получаем уникальный код только что вставленной записи
                        nzpSource = executor.ExecuteSql<int>("select lastval() as key").FirstOrDefault();
                    }


                    _importLog.AppendLine("Старт загрузки данных в БД: " + DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss"));
                    foreach (var file in Directory.EnumerateFiles(extractionDirectory, "*.unl"))
                    {
                        //название файла соответствует названию таблицы
                        var originalTableName = Path.GetFileNameWithoutExtension(file);

                        //данные кладутся в партиционированные таблицы
                        //собираем название таблицы
                        var newTableName = String.Format("{0}_{1}_{2}",
                            originalTableName,
                            districtCode,
                            calculationDate.ToString("yyyy_MM"));

                        try
                        {
                            //обновляем запись о загрузке в данную таблицу
                            sqlQuery = String.Format(
                                " DELETE FROM {0}.ods_tables " +
                                " WHERE tab_name = '{1}'" +
                                " AND tab_scheme ='{2}'" +
                                " AND nzp_source = {3}; " +

                                " INSERT INTO {0}.ods_tables (tab_name, tab_scheme, nzp_source) " +
                                " VALUES ('{1}', '{2}', {3}); ",
                                schemaName, newTableName, originalTableName, nzpSource);

                            //удаляем таблицу
                            sqlQuery += String.Format(" DROP TABLE IF EXISTS {0}.{1} CASCADE; ", schemaName,
                                newTableName);
                            executor.ExecuteSql(sqlQuery);

                            //создаем партиционированную таблицу
                            sqlQuery =
                                String.Format(
                                    " CREATE TABLE " +
                                    " {0}.{1} " +
                                    " (LIKE {0}.{2} INCLUDING ALL) " +
                                    " INHERITS({0}.{2}) WITH (OIDS=TRUE) ",
                                    schemaName,
                                    newTableName,
                                    originalTableName
                                    );
                            executor.ExecuteSql(sqlQuery);

                            using (var stream = File.OpenRead(file))
                            {
                                //передача потока напрямую в БД
                                executor.CopyIn(
                                    String.Format(
                                        " COPY {0}.{1} FROM stdin WITH DELIMITER AS '|' NULL AS ''",
                                        schemaName, newTableName)
                                    , stream, Encoding.GetEncoding(1251));

                                //установка nzp_source
                                sqlQuery =
                                    String.Format(
                                        " UPDATE {0}.{1} SET nzp_source = {2}",
                                        schemaName, newTableName, nzpSource
                                        );
                                executor.ExecuteSql(sqlQuery);

                                //пишем список успешно загруженных файлов в отдельный stringBuilder 
                                //для того чтоб добавить его в лог только в случае успешной загрузки
                                loadedFiles.AppendLine(String.Format("Файл '{0}' успешно загружен.",
                                    Path.GetFileName(file)));
                            }
                        }
                        catch (PostgresException ex)
                        {
                            var error = String.Format(
                                " Некорректные данные в файле '{0}'! Сообщение об ошибке:'{1}'.{3}Местоположение некорректных данных: '{2}'",
                                Path.GetFileName(file),
                                ex.Message,
                                ex.Where,
                                Environment.NewLine);

                            //удаляем таблицы в случае ошибки
                            foreach (var oneQuery in Directory.EnumerateFiles(extractionDirectory, "*.unl").
                                Select(oneFile => String.Format("{0}.{1}_{2}_{3}",
                                    schemaName,
                                    Path.GetFileNameWithoutExtension(oneFile),
                                    districtCode,
                                    calculationDate.ToString("yyyy_MM"))).
                                Select(tableName => " DROP TABLE IF EXISTS " + tableName + " CASCADE "))
                            {
                                executor.ExecuteSql(oneQuery);
                            }
                            throw new Exception(error, ex);
                        }
                    }

                    //отметка об успешной загрузке
                    sqlQuery =
                        String.Format(
                            " UPDATE {0}.ods_source SET fl_load = 1 " +
                            " WHERE nzp_source = {1}",
                            schemaName, nzpSource
                            );
                    executor.ExecuteSql(sqlQuery);
                }

                //в лог добавляем список загруженных файлов
                _importLog.AppendLine(loadedFiles.ToString());
                _importLog.AppendLine("Данные полностью успешно загружены!");
                _resultStatus = TypeStatus.Done;
            }
            catch (Exception ex)
            {
                _resultStatus = TypeStatus.Error;
                _importLog.AppendLine(ex.Message);
                return new ImportDataResult(false, "Ошибка при загрузке!", loadedFileId.Id);
            }
            finally
            {
                Directory.Delete(extractionDirectory, true);
                //сохраняем в файл протокол-загрузки
                var logFileName = String.Format("ПРОТОКОЛ_ЗАГРУЗКИ_{0}", loadedFileId.File.Name);
                var resultString = _importLog.ToString();
                var bytes = new byte[resultString.Length * sizeof(char)];
                Buffer.BlockCopy(resultString.ToCharArray(), 0, bytes, 0, bytes.Length);

                //сохраняем лог-файл
                loadedFileId.Log = Container.Resolve<IFileManager>().SaveFile(logFileName, "txt", bytes);
                loadedFileId.ImportResult = ImportResult.Success;
                loadedFileId.TypeStatus = _resultStatus;
                Container.Resolve<IDomainService<LoadedFileRegister>>().Update(loadedFileId);
            }
            return new ImportDataResult(true, "Успешно загружено!", loadedFileId.Id);
        }


        /// <summary>
        /// Вспомогательный объект для считывания из таблицы ods_source
        /// </summary>
        private class OdsSourceEntity
        {
            internal int NzpSource { get; set; }
            internal int FlLoad { get; set; }
        }
    }
}
