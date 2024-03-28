namespace Bars.Gkh.Gis.DomainService.ImportData.Impl.ImportIncremetalData.LoadFromOtherSystems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Data;
    using System.IO;
    using System.Text;
    using B4;
    using Dapper;
    using Entities.ImportIncrementalData.LoadFromOtherSystems;
    using Intf;
    using Ionic.Zip;
    using Npgsql;

    using ServiceStack.Common;

    public class LoadFileFromOtherSystem : ILoadFileFromOtherSystem
    {
        public LoadLog Log { get; set; }
        public FormatTemplate FormatTemplate { get; set; }
        public IDbConnection Connection { get; set; }
        public DataBankStogare DataBankStogare { get; set; }
        public UploadErrors UploadErrors { get; set; }
        public IRegisterFormatForOtherSystems RegisterFormatForOtherSystems { get; set; }
        public ConnectionParameters ConnectionParameters { get; set; }

        public IDataResult LoadData(ZipFile mainArchive)
        {
            //Секции файла
            var fileSections = FormatTemplate.SectionList;
            //Создаем индексы на лог файл
            Connection.Execute(string.Format(" select upload.create_table_part_index('file_log', {0});", FormatTemplate.UploadId));
            foreach (var file in fileSections.OrderBy(x => x.Ordering))
            {
                var section =
                     FormatTemplate.HeaderList.Where(
                         x =>
                             file.SectionName.ToUpper() == (x.SectionFile + ".csv").ToUpper()).Select(z => new TemplateHeader { SectionName = z.SectionName, SectionFile = z.SectionFile, SectionNumber = z.SectionNumber, SectionTable = z.SectionTable }).FirstOrDefault();
                if (section == null)
                {
                    UploadErrors.GetErrorMessage(file.SectionName, null, 1501, "");
                    continue;
                }
                var data = mainArchive.FirstOrDefault(x => x.FileName.ToUpper() == (file.SectionName).ToUpper());
                if (data == null)
                {
                    UploadErrors.GetErrorMessage(file.SectionName, null, 1501, string.Format("Ошибка получения данных из архива, файл {0}", section.SectionFile));
                    continue;
                }
                using (var dataMs = (Stream)data.OpenReader())
                {
                    var connectionToCopy = new NpgsqlConnection(DataBankStogare.ConnectionString);
                    connectionToCopy.Open();
                    try
                    {
                        var fields = string.Join(",", FormatTemplate.Format[section.SectionTable].OrderBy(x => x.Place).Select(x => x.ColumnName));
                        //var tempTable = string.Format("_temp_{0}", DateTime.Now.Ticks);
                        //CreateTempTable(connectionToCopy, tempTable, section.SectionTable, section.SectionName, file.SectionName);
                        var sqlQuery = String.Format(
                            " COPY upload_part.{1}_{0} ({2}) " +
                            " FROM stdin " +
                            " WITH (DELIMITER ';', " +
                            " NULL ''," +
                            " FORMAT CSV," +
                            " HEADER true)",
                            FormatTemplate.UploadId, section.SectionTable, fields);

                        using (var copier = connectionToCopy.BeginTextImport(sqlQuery))
                        {
                            using (var sr = new StreamReader(dataMs, Encoding.GetEncoding(1251)))
                            {
                                while (!sr.EndOfStream)
                                {
                                    copier.WriteLine(sr.ReadLine());
                                }
                            }
                        }
                       
                        //TransferTempTable(connectionToCopy, tempTable, section.SectionTable, section.SectionName, file.SectionName);
                        FormatTemplate.HeaderList.Find(x => x.SectionNumber == section.SectionNumber).IsUpload = true;
                        Connection.Execute(string.Format(" select upload.create_table_part_index('{1}', {0});", FormatTemplate.UploadId, section.SectionTable));
                        Connection.Execute(string.Format("analyze upload_part.{1}_{0}", FormatTemplate.UploadId, section.SectionTable));
                    }
                    catch (PostgresException ex)
                    {
                        UploadErrors.GetErrorMessage(file.SectionName, null, 998, String.Format(
                            "Архив не прошел валидацию! Ошибка при загрузке данных из файла '{0}' в таблицу '{1}': {2} {3}Детали ошибки: {4} ",
                            Path.GetFileNameWithoutExtension(data.FileName),
                            section.SectionTable,
                            ex.Message,
                            Environment.NewLine,
                            ex.Where
                            ));

                        CheckFile(data, section.SectionTable, section.SectionName);
                    }
                    catch (Exception ex)
                    {
                        UploadErrors.GetErrorMessage(file.SectionName, null, 999, "Ошибка при копировании:" + ex.Message);
                        CheckFile(data, section.SectionTable, section.SectionName);
                    }
                    finally
                    {
                        connectionToCopy.Close();
                    }
                }
            }
            RegisterFormatForOtherSystems.UpdateFileProgress(0.3m);
            return new BaseDataResult();
        }

        private void CreateTempTable(IDbConnection connectionToCopy, string tempTable, string sectionTable, string sectionName, string fileName)
        {
            if (!FormatTemplate.Format.ContainsKey(sectionTable))
            {
                UploadErrors.GetErrorMessage(fileName, null, 999, string.Format("Секция {0} не найдена. Проверьте формат загрузчика", sectionName));
                return;
            }
            var fields = FormatTemplate.Format[sectionTable];
            //Дропаем темповую таблицу
            var sql = string.Format("drop table if exists {0}", tempTable);
            connectionToCopy.Execute(sql);
            //Создаем темповую таблицу
            sql = string.Format("create temp table {0}(", tempTable);
            for (var i = 0; i < fields.Count(); i++)
            {
                var type = "";
                switch (fields[i].Type)
                {
                    case Types.DateTime:
                        {
                            type = " timestamp ";
                        } break;
                    case Types.Date:
                        {
                            type = " date ";
                        } break;
                    case Types.Int:
                        {
                            type = " integer ";
                        } break;
                    case Types.NumericMoney:
                        {
                            type = " real ";
                        } break;
                    case Types.Numeric:
                        {
                            type = string.Format(" numeric ({0},{1})", fields[i].NumericPrecission, fields[i].NumericScale);
                        } break;
                    case Types.String:
                        {
                            type = string.Format(" varchar({0})", fields[i].MaxLength);
                        } break;
                }
                sql += string.Format("{0} {1}", fields[i].ColumnName, type);
                if (i != fields.Count() - 1)
                    sql += ",";
            }
            sql += ");";
            connectionToCopy.Execute(sql);
        }

        private void TransferTempTable(IDbConnection connectionToCopy, string tempTable, string sectionTable, string sectionName, string fileName)
        {
            if (!FormatTemplate.Format.ContainsKey(sectionTable))
            {
                UploadErrors.GetErrorMessage(fileName, null, 999, string.Format("Секция {0} не найдена. Проверьте формат загрузчика", sectionName));
                return;
            }
            var fields = FormatTemplate.Format[sectionTable];
            var joinFields = string.Join(",", fields.Select(x => x.ColumnName));
            var sql = string.Format("insert into upload_part.{0}(file_id,{1}) (select {2},{1} from {3})", sectionTable + "_" + FormatTemplate.UploadId, joinFields, FormatTemplate.UploadId, tempTable);
            connectionToCopy.Execute(sql);
        }

        private void CheckFile(ZipEntry verificationInfo, string sectionTable, string sectionName)
        {
            using (var ms = new MemoryStream())
            {
                verificationInfo.Extract(ms);
                ms.Seek(0, SeekOrigin.Begin);
                var list = new List<string[]>();
                using (var streamReader = new StreamReader(ms, Encoding.GetEncoding(1251)))
                {
                    //пропускаем первую строку
                    streamReader.ReadLine();
                    while (!streamReader.EndOfStream)
                    {
                        var readLine = streamReader.ReadLine();
                        if (readLine != null)
                        {
                            var split = readLine.Split(';');
                            list.Add(split);
                        }
                    }
                }
                //Проверка на дублирование строк
                //var hashset = new HashSet<string>();
                var fields = FormatTemplate.Format[sectionTable];
                var k = 0;
                foreach (var items in list)
                {
                    k++;
                    if (items.Count() != fields.Count)
                    {
                        UploadErrors.GetErrorMessage(verificationInfo.FileName, k, 2001, string.Format("Ошибка, Неправильный формат файла загрузки,{4}: {2}, количество полей = {0} вместо {1}, строка {3}", items.Count(), fields.Count, sectionName, k, UploadErrors.Types[2001]));
                        continue;
                    }
                    for (var i = 0; i < items.Length; i++)
                    {
                        fields[i].CheckValues(UploadErrors, items[i], k, sectionName, verificationInfo.FileName);
                    }
                }
            }
        }

        public IDataResult TrancateTables()
        {
            if (FormatTemplate.UploadId > 0)
            {
                var sql = string.Format("select upload.drop_part({0}); ", FormatTemplate.UploadId);
                Connection.Execute(sql);
            }
            return new BaseDataResult();
        }


        public IDataResult TransferData(ref bool deletePartition)
        {
            try
            {
                Connection.Execute(string.Format("select master.transfer_upload_in_datastore({0})", FormatTemplate.UploadId));
            }
            catch (PostgresException ex)
            {
                deletePartition = false;
                UploadErrors.GetErrorMessage("", null, 997, String.Format(
                 "Архив не прошел валидацию! Ошибка при сохранении данных на склад.Детали ошибки: {0},{1} ",
                 ex.Message, ex.Where
                 ));
            }
            return new BaseDataResult();
        }

        public IDataResult TransferHouseData(ref bool deletePartition)
        {
            try
            {
                var sql =
                    string.Format(
                        "select master.transfer_dom_to_mgf({0},'{1}'::varchar,'{2}'::integer,'{3}'::varchar,'{4}'::varchar,'{5}'::varchar)"
                        , FormatTemplate.UploadId
                        , ConnectionParameters.Server, ConnectionParameters.Port, ConnectionParameters.UserName
                        , ConnectionParameters.Password, ConnectionParameters.DbName);
                Connection.Execute(sql);
            }
            catch (PostgresException ex)
            {
                deletePartition = false;
                UploadErrors.GetErrorMessage("", null, 997, String.Format(
                 "Архив не прошел валидацию! Ошибка при сохранении данных по домам.Детали ошибки: {0},{1} ",
                 ex.Message, ex.Where
                 ));
            }
            return new BaseDataResult();
        }
    }
}
