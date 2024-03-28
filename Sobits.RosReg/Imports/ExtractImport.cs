namespace Sobits.RosReg.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.Gkh.Import.Impl;

    using Castle.Windsor;

    using Ionic.Zip;

    using Sobits.RosReg.Entities;
    using Sobits.RosReg.Enums;
    using Sobits.RosReg.Tasks.ExtractParse;

    public class ExtractImport : GkhImportBase
    {
        public override ImportResult Import(BaseParams baseParams)
        {
            this.globalStopwatch.Start();
            var file = baseParams.Files["FileImport"];
            this.InitLog(file.FileName);

            using (var zipfileMemoryStream = new MemoryStream(file.Data))
            {
                using (var zipFile = ZipFile.Read(zipfileMemoryStream))
                {
                    var allEntries = zipFile.ToArray();
                    var xmlEntries = zipFile.Where(x => x.FileName.EndsWith(".xml") 
                                                || x.FileName.EndsWith(".xml.original")
                                                || x.FileName.EndsWith(".original")).ToArray();

                    if (xmlEntries.Length < 1)
                    {
                        this.LogImport.Error("Ошибка", "Отсутствуют файлы для импорта");
                        return new ImportResult(StatusImport.CompletedWithError, "Отсутствуют файлы для импорта");
                    }

                    this.ImportInternal(xmlEntries, allEntries);
                }
            }

            this.LogImport.SetFileName(file.FileName);
            this.LogImport.ImportKey = this.CodeImport;

            this.LogImportManager.FileNameWithoutExtention = file.FileName;
            this.LogImportManager.Add(file, this.LogImport);
            this.LogImportManager.Save();

            var statusImport = this.LogImport.CountError > 0
                ?StatusImport.CompletedWithError
                :this.LogImport.CountWarning > 0
                    ?StatusImport.CompletedWithWarning
                    :StatusImport.CompletedWithoutError;
            this.globalStopwatch.Stop();

            var taskManager = this.Container.Resolve<ITaskManager>();

            try
            {
                taskManager.CreateTasks(new ExtractParseTaskProvider(), new BaseParams());
            }
            finally
            {
                this.Container.Release(taskManager);
            }

            return new ImportResult(statusImport, $"time elapsed: {this.globalStopwatch.ElapsedMilliseconds} ms;");
        }

        private void ImportInternal(IReadOnlyCollection<ZipEntry> xmlEntries, IEnumerable<ZipEntry> allEntries)
        {
            var indicator = 0;

            var service = this.Container.ResolveDomain<Extract>();
            var fileManager = this.Container.Resolve<IFileManager>();

            // var readOptions = new ReadOptions();
            // readOptions.Encoding = Encoding.GetEncoding(866);
            try
            {
                foreach (var xmlEntry in xmlEntries)
                {
                    var extract = ExtractImport.GetExtract(xmlEntry);
                    
                    ZipEntry extractPdf = null;
                    // Поиск пдф-файла для расширений .xml и .xml.original
                    var lastIndex = xmlEntry.FileName.LastIndexOf(".xml", StringComparison.InvariantCulture);
                    // Поиск пдф-файла для расширения .original
                        if (lastIndex==-1) lastIndex = xmlEntry.FileName.LastIndexOf(".original", StringComparison.InvariantCulture);

                    var name = "";
                    if ( lastIndex > 0)
                    {
                        name = $"{xmlEntry.FileName.Substring(0,lastIndex)}.pdf";
                        extractPdf = allEntries.FirstOrDefault(
                            x => x.FileName == name);
                    }

                    if (extractPdf != null)
                    {
                        var ms = new MemoryStream();
                        extractPdf.Extract(ms);
                        var filename = Encoding.GetEncoding(866).GetString(Encoding.GetEncoding(437).GetBytes(extractPdf.FileName));
                        var file = fileManager.SaveFile(ms, $"{filename}");
                        extract.File = file.Id;
                    }

                    service.Save(extract);
                    indicator++;
                    this.Indicate(80 * indicator / xmlEntries.Count, $"{indicator} выписок из {xmlEntries.Count} загружено");
                }
            }
            finally
            {
                this.Container.Release(service);
                this.Container.Release(fileManager);
            }

            this.Indicate(95, "Завершение импорта");

            //this.AssignTypes();
        }

        //Обработка построчная, медленнее и сильнее нагружает базу, но надежнее
        private static Extract GetExtract(ZipEntry zipEntry)
        {
            //this.localStopwatch.Restart();

            //Извлекаем xml в строку
            string xml;
            using (Stream s = new MemoryStream())
            {
                zipEntry.Extract(s);
                s.Seek(0, SeekOrigin.Begin);
                using (var sr = new StreamReader(s))
                {
                    xml = sr.ReadToEnd();
                }
            }

            return new Extract
            {
                Type = ExtractType.NotSet,
                CreateDate = DateTime.Now,
                IsActive = false,
                IsParsed = false,
                Xml = xml
            };

            //Вставка прямым запросом, т.к. нхибернейт не желает работать с типом данных xml
            // using (IStatelessSession statelessSession = this.Container.Resolve<ISessionProvider>().OpenStatelessSession())
            // {
            //     using (IDbConnection connection = statelessSession.Connection)
            //     {
            //         string sqlinsert = $@"insert into {ExtractMap.SchemaName}.{ExtractMap.TableName}
            //                     ({nameof(Extract.Type).ToLower()},
            //                     {nameof(Extract.CreateDate).ToLower()},
            //                     {nameof(Extract.IsParsed).ToLower()},
            //                     {nameof(Extract.IsActive).ToLower()},
            //                     xml)
            //                     values (null,now(),false,false,'{xml}')";
            //         connection.Execute(sqlinsert);
            //         this.localStopwatch.Stop();
            //         Debug.WriteLine($@"IMPORT SQL INSERT: {this.localStopwatch.ElapsedMilliseconds} ms");
            //     }
            // }
        }

        #region Utility
        public static readonly string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public new IWindsorContainer Container { get; set; }

        public ILogImportManager LogManager { get; set; }

        public new ILogImport LogImport { get; set; }

        public new ILogImportManager LogImportManager { get; set; }

        public override string Key => ExtractImport.Id;

        public override string CodeImport => "ExtractImport";

        public override string Name => "Импорт выписок из Росреестра";

        public override string PossibleFileExtensions => "zip";

        public override string PermissionName => "Import.RosRegExtract";

        private new void InitLog(string fileName)
        {
            this.LogManager = this.Container.Resolve<ILogImportManager>();
            if (this.LogManager == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImportManager");
            }

            this.LogImport.ImportKey = this.Name;
            this.LogManager.FileNameWithoutExtention = fileName;
            this.LogManager.UploadDate = DateTime.Now;
        }

        //public ISessionProvider SessionProvider { get; private set; }

        //Таймеры глобальные, чтобы не пересоздавать их каждый раз заново
        private readonly Stopwatch globalStopwatch = new Stopwatch();
        #endregion
    }
}