namespace Bars.Gkh.Gis.DomainService.ImportData.Impl.ImportIncremetalData.LoadFromOtherSystems
{
    using System.Data;
    using System.IO;
    using System.Text;
    using B4;
    using Castle.Windsor;
    using Entities.ImportIncrementalData.LoadFromOtherSystems;
    using Intf;
    using Ionic.Zip;
    using Npgsql;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;

    using Bars.Gkh.Gis.DomainService.BilConnection;
    using Bars.Gkh.Gis.Enum;

    using Dapper;
    using Entities.Register.LoadedFileRegister;

    using Utils;
    using Component = Castle.MicroKernel.Registration.Component;

    public class LoadFileFromOtherSystemInstance : IDisposable
    {
        public IWindsorContainer Container { get; set; }
        private IDbConnection Connection { get; set; }
        public ZipFile MainArchive { get; set; }
        public StringBuilder LogFile { get; set; }
        public DateTime StartUpload { get; set; }
        public DateTime EndUpload { get; set; }
        public string FileName { get; set; }
        public IBilConnectionService BilConnectionService { get; set; }
        public LoadFileFromOtherSystemInstance(ZipFile mainArchive, IncrementalParams parameters, LoadedFileRegister file, string mgfConnectionString, IBilConnectionService bilConnectionService)
        {
            BilConnectionService = bilConnectionService;
            var storage = GetDataBankStogare();
            Connection = new NpgsqlConnection(storage.ConnectionString);
            Container = new WindsorContainer();
            MainArchive = mainArchive;
            FileName = file.File.Name;
            StartUpload = DateTime.Now;

            Connection.Open();
            Container.Register(Component.For<IDbConnection>().UsingFactoryMethod(() => Connection));
            Container.Register(Component.For<IWindsorContainer>().UsingFactoryMethod(() => Container));
            Container.Register(Component.For<IncrementalParams>().UsingFactoryMethod(() => parameters));
            Container.Register(Component.For<IValidateFile>().ImplementedBy<ValidateFile>());
            Container.Register(Component.For<LoadLog>().UsingFactoryMethod(() => new LoadLog
            {
                ErrorMessages = new StringBuilder(),
                InfoMessages = new StringBuilder(),
                WarningMessages = new StringBuilder(),
                Status = UploadStatuses.Upload
            }));
            Container.Register(Component.For<FormatTemplate>().UsingFactoryMethod(() => new FormatTemplate { SupplierAndBank = new SupplierAndBank { MgfFileId = parameters.MgfFileId } }));
            Container.Register(Component.For<DataBankStogare>().UsingFactoryMethod(() => storage));
            Container.Register(Component.For<FileHeader>().UsingFactoryMethod(() => new FileHeader()));
            var supplier = new SupplierAndBank { MgfFileId = parameters.MgfFileId };
            Container.Register(Component.For<SupplierAndBank>().UsingFactoryMethod(() => supplier));
            Container.Register(Component.For<IRegisterFormatForOtherSystems>().ImplementedBy<RegisterFormatForOtherSystems>());
            Container.Register(Component.For<ILoadFileFromOtherSystem>().ImplementedBy<LoadFileFromOtherSystem>());
            Container.Register(Component.For<UploadErrors>().ImplementedBy<UploadErrors>());
            Container.Register(Component.For<IDeterminateStorage>().ImplementedBy<DeterminateStorageService>());
            var connectionParameters = GetConnectionParameters(mgfConnectionString);
            Container.Register(Component.For<ConnectionParameters>().UsingFactoryMethod(() => connectionParameters));
        }

        public IDataResult Run(bool checkDataSupplier = true)
        {
            SetCulture();
            var loadService = Container.Resolve<ILoadFileFromOtherSystem>();
            var uploadErrors = Container.Resolve<UploadErrors>();
            var formatService = Container.Resolve<FormatTemplate>();
            var systemService = Container.Resolve<IRegisterFormatForOtherSystems>();
            var deletePartition = true;
            try
            {
                var validateService = Container.Resolve<IValidateFile>();
                //Если заголовочный файл валиден
                var header = validateService.ValidateHeader(MainArchive, FileName, checkDataSupplier);
                if (header.Success)
                {
                    var registerService = Container.Resolve<IRegisterFormatForOtherSystems>();
                    //Получение заголовков формата
                    formatService.HeaderList = registerService.GetFormatHeaders();
                    registerService.SetOrdering();
                    //Получение формата загрузки 
                    formatService.Format = registerService.GetFormat();
                    //Загрузка данных
                    loadService.LoadData(MainArchive);
                    //Проверить загруженные данные
                    validateService.Validate();
                    //Создание схемы

                    if (systemService.GetLoadResult())
                    {
                        Container.Resolve<IDeterminateStorage>().DeterminateStorage();
                        //Перенос данных
                        loadService.TransferData(ref deletePartition);
                        //Сохранение данных по домам
                        loadService.TransferHouseData(ref deletePartition);
                    }
                }
                else throw new Exception(header.Message);
            }
            catch (PostgresException ex)
            {
                uploadErrors.GetErrorMessage("", null, 998, String.Format(
                    "Архив не прошел валидацию! Ошибка при загрузке данных из файла.Детали ошибки: {0} ",
                    ex.Where
                    ));
            }
            catch (Exception ex)
            {
                uploadErrors.GetErrorMessage("_info.csv", null, 999, string.Format("Ошибка:{0}", ex.Message));
                return new BaseDataResult { Success = false, Data = GetUploadResult(false) };
            }
            finally
            {
                if (deletePartition)
                    loadService.TrancateTables();
            }
            return new BaseDataResult { Success = true, Data = GetUploadResult(true) };
        }

        public ConnectionParameters GetConnectionParameters(string connectionString)
        {
            var dbName = "";
            var password = "";
            var port = 0;
            var server = "";
            var userName = "";
            var uploadErrors = Container.Resolve<UploadErrors>();
            try
            {
                dbName = Connection.ExecuteScalar<string>("select setting_value from master.sys_setting where setting_id = 7");
                password = Connection.ExecuteScalar<string>("select setting_value from master.sys_setting where setting_id = 6");
                port = Connection.ExecuteScalar<int>("select setting_value::integer from master.sys_setting where setting_id = 4");
                server = Connection.ExecuteScalar<string>("select setting_value from master.sys_setting where setting_id = 3");
                userName = Connection.ExecuteScalar<string>("select setting_value from master.sys_setting where setting_id = 5");
            }
            catch (Exception ex)
            {
                uploadErrors.GetErrorMessage("_info.csv", null, 999, string.Format("Ошибка:{0}", ex.Message));
            }

            return new ConnectionParameters
            {
                DbName = dbName,
                Password = password,
                Port = port,
                Server = server,
                UserName = userName
            };
        }

        public void SetCulture()
        //----------------------------------------------------------------------
        {
            var culture = new CultureInfo("ru-RU")
            {
                NumberFormat = { NumberDecimalSeparator = "." },
                DateTimeFormat =
                {
                    ShortDatePattern = "dd.MM.yyyy",
                    ShortTimePattern = "HH:mm:ss"
                }
            };
            Thread.CurrentThread.CurrentCulture = culture;
        }


        public void Dispose()
        {
            Connection.Close();
            Connection.Dispose();
            Container.Dispose();
        }

        /// <summary>
        /// По коду банка данных определить хранилище в реестре data_bank
        /// </summary>
        /// <param name="dataBankId"></param>
        private DataBankStogare GetDataBankStogare()
        {
            //TODO: надо реализовать возврат реального хранилища банка данных
            return new DataBankStogare() { ConnectionString = this.BilConnectionService.GetConnection(ConnectionType.GisConnStringInc) };
        }

        private IDataResult GetUploadResult(bool success)
        {
            EndUpload = DateTime.Now;
            var format = Container.Resolve<FormatTemplate>();
            bool result = success;
            byte[] zipbytes;
            var sql = string.Format(" select coalesce(file_name,'')||';'||coalesce(str_number::varchar,'')||';'||coalesce(error_id::varchar,'')||';'||coalesce(error_text,'') " +
                                    " from upload.file_log where file_id = {0} order by error_id", format.UploadId);
            var systemService = Container.Resolve<IRegisterFormatForOtherSystems>();
            var cmd = new NpgsqlCommand(sql, (NpgsqlConnection)Connection);
            var fileName = String.Format("ПРОТОКОЛ_ЗАГРУЗКИ_{0} от {1}", FileName, StartUpload.ToString("dd-MM-yyyy HH-mm-ss"));
            var tempDirectory = Path.GetTempPath();
            var builder = Directory.CreateDirectory(Path.Combine(tempDirectory, "GisUploadForAnotherFormat", "Upload_" + format.UploadId)).FullName;
            var fileStatus = Path.Combine(builder, "_result.csv");
            var fileError = Path.Combine(builder, "_error.csv");
            try
            {
                using (var zip = new ZipFile(fileName, Encoding.UTF8))
                {
                    //Сохраняем файлы
                    using (var memoryStreamError = new FileStream(fileError, FileMode.Create))
                    using (var memoryStreamStatus = new FileStream(fileStatus, FileMode.Create))
                    {
                        result = systemService.GetLoadResult() && success;
                        systemService.UpdateFileStatus(result ? UploadStatuses.Uploaded : UploadStatuses.UploadedWithErrors, result ? (int)UploadStatuses.Uploaded : (int)UploadStatuses.UploadedWithErrors);
                        using (var reader = cmd.ExecuteReader())
                        {
                            using (var writerStatus = new StreamWriter(memoryStreamStatus, Encoding.GetEncoding(1251)))
                            using (var writerError = new StreamWriter(memoryStreamError, Encoding.GetEncoding(1251)))
                            {
                                writerError.WriteLine("Наименование файла;Номер строки файла (не считая заголовка);Код ошибки;Текст ошибки");
                                while (reader.Read())
                                {
                                    writerError.WriteLine(reader.GetString(0));
                                }
                                writerStatus.WriteLine("Наименование загружаемого файла;Дата и время начала загрузки;Дата и время окончания загрузки;Результат загрузки");
                                writerStatus.Write("{0};{1};{2};{3}", FileName, StartUpload.ToString("dd.MM.yyyy HH:mm:ss"), EndUpload.ToString("dd.MM.yyyy HH:mm:ss"), Convert.ToInt32(result));

                                writerStatus.Flush();
                                writerStatus.Close();

                                writerError.Flush();
                                writerError.Close();

                                zip.AddDirectory(builder);
                                var output = new MemoryStream();
                                zip.Save(output);
                                output.Seek(0, SeekOrigin.Begin);
                                output.Flush();
                                zipbytes = output.ToArray();
                            }
                        }
                    }
                }
            }
            finally
            {
                Directory.Delete(builder, true);
            }
            return new BaseDataResult
            {
                Success = result,
                Data = zipbytes
            };
        }
    }
}
