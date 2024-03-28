namespace Bars.GkhDi.Import.CommunalPay
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.Gkh.Import.Impl;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Import.Data;
    using Bars.GkhDi.Import.Sections;

    using Castle.Windsor;

    using Ionic.Zip;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Импорт из комплат
    /// </summary>
    public class CommunalPayImport : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType?.FullName;

        public new virtual IWindsorContainer Container { get; set; }

        public override string Key => CommunalPayImport.Id;

        public override string CodeImport => "CommunalPay";

        public override string Name => "Импорт из комплат";

        public override string PossibleFileExtensions => "zip,csv";

        public override string PermissionName => "GkhDi.Import.CommunalPay.View";

        public Dictionary<string, SectionsData> SectionsDataDict { get; set; }

        private string ZipName { get; set; }

        public CommunalPayImport(ILogImportManager logImportManager, ILogImport logImport)
        {
            this.LogImportManager = logImportManager;
            this.LogImport = logImport;
        }

        public override ImportResult Import(BaseParams baseParams)
        {
            var message = string.Empty;

            var fileData = baseParams.Files["FileImport"];
            var periodDiId = baseParams.Params.GetAs<long>("PeriodDiId");

            this.InitLog(fileData.FileName);

            FileData csvFile;

            using (var strFile = new StreamReader(this.PrepareFiles(fileData), Encoding.GetEncoding(1251)))
            {
                csvFile = new FileData(fileData.FileName, "csv", strFile.CurrentEncoding.GetBytes(strFile.ReadToEnd()));
                strFile.BaseStream.Seek(0, 0);
                this.SectionsDataDict = this.PrepareData(strFile);
            }

            var roDomain = this.Container.Resolve<IDomainService<RealityObject>>();
            var sectionImportDomain = this.Container.ResolveAll<ISectionImport>();

            using (this.Container.Using(roDomain, sectionImportDomain))
            {
                var realityObjectsInfoDict = roDomain.GetAll()
                    .Where(x => x.CodeErc != null)
                    .Select(x => new { x.Id, x.FiasAddress.AddressName, x.CodeErc })
                    .AsEnumerable()
                    .GroupBy(x => x.CodeErc)
                    .ToDictionary(x => x.Key, y => y.Select(x => new RealObjImportInfo { Id = x.Id, FiasAddressName = x.AddressName }).First());

                var sectionImportList = sectionImportDomain.ToList();

                StatusImport status;

                try
                {
                    if (this.SectionsDataDict != null)
                    {
                        foreach (var kvp in this.SectionsDataDict)
                        {
                            var realObjIds = this.GetRealityObjects(periodDiId, kvp.Key);

                            foreach (var sectionImport in sectionImportList)
                            {
                                sectionImport.ImportSection(
                                    new ImportParams
                                    {
                                        Inn = kvp.Key,
                                        SectionData = kvp.Value,
                                        PeriodDiId = periodDiId,
                                        LogImport = this.LogImport,
                                        zipName = this.ZipName,
                                        RealityObjectIds = realObjIds,
                                        RealObjsImportInfo = realityObjectsInfoDict
                                    });
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    this.LogImport.Error(this.Name, $"Во время импорта произошла ошибка: {e.Message}, InnerException: {e.InnerException?.Message}");
                    throw;
                }
                finally
                {
                    this.Container.UsingForResolved<ISessionProvider>(
                        (container, sessionProvider) => sessionProvider.CloseCurrentSession());

                    foreach (var sectionImport in sectionImportList)
                    {
                        this.Container.Release(sectionImport);
                    }

                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    if (this.LogImport.IsImported && this.LogImport.CountImportedRows == 0)
                    {
                        this.LogImport.IsImported = false;
                        this.LogImport.Error(this.Name, "Не удалось обнаружить записи для импорта");
                        message = "Не удалось обнаружить записи для импорта";
                    }

                    this.LogImportManager.Add(csvFile, this.LogImport);
                    message += this.LogImportManager.GetInfo();
                    this.LogImportManager.Save();

                    status = !this.LogImport.IsImported
                        ? StatusImport.CompletedWithError
                        : (this.LogImport.CountWarning > 0
                            ? StatusImport.CompletedWithWarning
                            : StatusImport.CompletedWithoutError);
                }

                return new ImportResult(status, message, string.Empty, this.LogImportManager.LogFileId);
            }
        }

        public override bool Validate(BaseParams baseParams, out string message)
        {
            try
            {
                message = null;
                if (!baseParams.Files.ContainsKey("FileImport"))
                {
                    message = "Не выбран файл для импорта";
                    return false;
                }

                var extention = baseParams.Files["FileImport"].Extention;

                var fileExtentions = this.PossibleFileExtensions.Contains(",") ? this.PossibleFileExtensions.Split(',') : new[] {this.PossibleFileExtensions };
                if (fileExtentions.All(x => x != extention))
                {
                    message = $"Необходимо выбрать файл с допустимым расширением: {this.PossibleFileExtensions}";
                    return false;
                }

                return true;
            }
            catch (Exception exp)
            {
                this.Container.Resolve<ILogger>().LogError(exp, "Валидация файла импорта");
                message = "Произошла неизвестная ошибка при проверки формата файла";
                return false;
            }
        }

        protected Dictionary<string, SectionsData> PrepareData(StreamReader sr)
        {
            var sectionDataDict = new Dictionary<string, SectionsData>();

            string lineCode = null;
            string line;
            string inn = string.Empty;

            while ((line = sr.ReadLine()) != null && !string.IsNullOrEmpty(line))
            {
                if (line.StartsWith("###"))
                {
                    lineCode = line.Split(';')[0].Split(' ')[0].Substring(3);
                    continue;
                }

                switch (lineCode)
                {
                    case "0":
                        {
                            var section0 = new Section0();
                            section0.Parse(line);
                            if (section0.VersionFormat != "1.7")
                            {
                                LogImport.Warn(this.Name, "Секция ###0, версия (поле VERS_FORMAT) должна быть 1.7");
                                return null;
                            }
                        }

                        break;
                    case "1":
                        {
                            var section1 = new Section1();
                            section1.Parse(line);

                            inn = section1.Inn;

                            if (string.IsNullOrEmpty(inn))
                            {
                                //ToDo добавить сообщение в лог 
                                return null;
                            }

                            if (sectionDataDict.ContainsKey(inn))
                            {
                                sectionDataDict[inn].Section1.Add(section1);
                            }
                            else
                            {
                                var sections = new SectionsData();
                                sections.Section1.Add(section1);
                                sectionDataDict.Add(inn, sections);
                            }

                            break;
                        }

                    case "3":
                        {
                            var section3 = new Section3();
                            section3.Parse(line);
                            if (sectionDataDict.ContainsKey(inn))
                            {
                                sectionDataDict[inn].Section3.Add(section3);
                            }
                            else
                            {
                                var sections = new SectionsData();
                                sections.Section3.Add(section3);
                                sectionDataDict.Add(inn, sections);
                            }

                            break;
                        }

                    case "4":
                        {
                            var section4 = new Section4();
                            section4.Parse(line);
                            if (sectionDataDict.ContainsKey(inn))
                            {
                                sectionDataDict[inn].Section4.Add(section4);
                            }
                            else
                            {
                                var sections = new SectionsData();
                                sections.Section4.Add(section4);
                                sectionDataDict.Add(inn, sections);
                            }

                            break;
                        }

                    case "5":
                        {
                            var section5 = new Section5();
                            section5.Parse(line);
                            if (sectionDataDict.ContainsKey(inn))
                            {
                                sectionDataDict[inn].Section5.Add(section5);
                            }
                            else
                            {
                                var sections = new SectionsData();
                                sections.Section5.Add(section5);
                                sectionDataDict.Add(inn, sections);
                            }

                            break;
                        }

                    case "6":
                        {
                            var section6 = new Section6();
                            section6.Parse(line);
                            if (sectionDataDict.ContainsKey(inn))
                            {
                                sectionDataDict[inn].Section6.Add(section6);
                            }
                            else
                            {
                                var sections = new SectionsData();
                                sections.Section6.Add(section6);
                                sectionDataDict.Add(inn, sections);
                            }

                            break;
                        }

                    case "7":
                        {
                            var section7 = new Section7();
                            section7.Parse(line);
                            if (sectionDataDict.ContainsKey(inn))
                            {
                                sectionDataDict[inn].Section7.Add(section7);
                            }
                            else
                            {
                                var sections = new SectionsData();
                                sections.Section7.Add(section7);
                                sectionDataDict.Add(inn, sections);
                            }

                            break;
                        }

                    case "8":
                        {
                            var section8 = new Section8();
                            section8.Parse(line);
                            if (sectionDataDict.ContainsKey(inn))
                            {
                                sectionDataDict[inn].Section8.Add(section8);
                            }
                            else
                            {
                                var sections = new SectionsData();
                                sections.Section8.Add(section8);
                                sectionDataDict.Add(inn, sections);
                            }

                            break;
                        }

                    case "9":
                        {
                            var section9 = new Section9();
                            section9.Parse(line);
                            if (sectionDataDict.ContainsKey(inn))
                            {
                                sectionDataDict[inn].Section9.Add(section9);
                            }
                            else
                            {
                                var sections = new SectionsData();
                                sections.Section9.Add(section9);
                                sectionDataDict.Add(inn, sections);
                            }

                            break;
                        }

                    case "10":
                        {
                            var section10 = new Section10();
                            section10.Parse(line);
                            if (sectionDataDict.ContainsKey(inn))
                            {
                                sectionDataDict[inn].Section10.Add(section10);
                            }
                            else
                            {
                                var sections = new SectionsData();
                                sections.Section10.Add(section10);
                                sectionDataDict.Add(inn, sections);
                            }

                            break;
                        }

                    case "11":
                        {
                            var section11 = new Section11();
                            section11.Parse(line);
                            if (sectionDataDict.ContainsKey(inn))
                            {
                                sectionDataDict[inn].Section11.Add(section11);
                            }
                            else
                            {
                                var sections = new SectionsData();
                                sections.Section11.Add(section11);
                                sectionDataDict.Add(inn, sections);
                            }

                            break;
                        }

                    case "12":
                        {
                            var section12 = new Section12();
                            section12.Parse(line);
                            if (sectionDataDict.ContainsKey(inn))
                            {
                                sectionDataDict[inn].Section12.Add(section12);
                            }
                            else
                            {
                                var sections = new SectionsData();
                                sections.Section12.Add(section12);
                                sectionDataDict.Add(inn, sections);
                            }

                            break;
                        }

                    case "13":
                        {
                            var section13 = new Section13();
                            section13.Parse(line);
                            if (sectionDataDict.ContainsKey(inn))
                            {
                                sectionDataDict[inn].Section13.Add(section13);
                            }
                            else
                            {
                                var sections = new SectionsData();
                                sections.Section13.Add(section13);
                                sectionDataDict.Add(inn, sections);
                            }

                            break;
                        }

                    case "14":
                        {
                            var section14 = new Section14();
                            section14.Parse(line);
                            if (sectionDataDict.ContainsKey(inn))
                            {
                                sectionDataDict[inn].Section14.Add(section14);
                            }
                            else
                            {
                                var sections = new SectionsData();
                                sections.Section14.Add(section14);
                                sectionDataDict.Add(inn, sections);
                            }

                            break;
                        }

                    case "15":
                        {
                            var section15 = new Section15();
                            section15.Parse(line);
                            if (sectionDataDict.ContainsKey(inn))
                            {
                                sectionDataDict[inn].Section15.Add(section15);
                            }
                            else
                            {
                                var sections = new SectionsData();
                                sections.Section15.Add(section15);
                                sectionDataDict.Add(inn, sections);
                            }

                            break;
                        }

                    case "17":
                        {
                            var section17 = new Section17();
                            section17.Parse(line);
                            if (sectionDataDict.ContainsKey(inn))
                            {
                                sectionDataDict[inn].Section17.Add(section17);
                            }
                            else
                            {
                                var sections = new SectionsData();
                                sections.Section17.Add(section17);
                                sectionDataDict.Add(inn, sections);
                            }

                            break;
                        }

                    case "18":
                        {
                            var section18 = new Section18();
                            section18.Parse(line);
                            if (sectionDataDict.ContainsKey(inn))
                            {
                                sectionDataDict[inn].Section18.Add(section18);
                            }
                            else
                            {
                                var sections = new SectionsData();
                                sections.Section18.Add(section18);
                                sectionDataDict.Add(inn, sections);
                            }

                            break;
                        }

                    case "19":
                        {
                            var section19 = new Section19();
                            section19.Parse(line);
                            if (sectionDataDict.ContainsKey(inn))
                            {
                                sectionDataDict[inn].Section19.Add(section19);
                            }
                            else
                            {
                                var sections = new SectionsData();
                                sections.Section19.Add(section19);
                                sectionDataDict.Add(inn, sections);
                            }

                            break;
                        }
                }
            }

            return sectionDataDict;
        }

        /// <summary>
        /// Подготавливаем файлы
        /// </summary>
        /// <param name="fileData">файл</param>
        /// <returns>поток</returns>
        protected MemoryStream PrepareFiles(FileData fileData)
        {
            if (fileData.Extention == "csv")
            {
                return new MemoryStream(fileData.Data);
            }

            var result = new MemoryStream();

            if (fileData.Extention == "zip")
            {
                this.ZipName = fileData.FileName;

                using (var zipFile = ZipFile.Read(new MemoryStream(fileData.Data)))
                {
                    var csvZipEntry = zipFile.FirstOrDefault(x => x.FileName.EndsWith(".csv"));
                    if (csvZipEntry != null)
                    {                        

                        var path = ApplicationContext.Current.MapPath("~/Import/");

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        var fileName = csvZipEntry.FileName;
                        csvZipEntry.Extract(path, ExtractExistingFileAction.OverwriteSilently);

                        using (var filestream = File.Open(path + fileName, FileMode.Open))
                        {
                            filestream.CopyTo(result);
                            result.Seek(0, SeekOrigin.Begin);
                        }

#warning убрать выполнение в потоке
                        var thread = new Thread(
                            () =>
                            {
                                try
                                {
                                    this.CopyDocuments(zipFile, path);
                                }
                                catch (Exception ex)
                                {
                                    this.LogImport.IsImported = false;
                                    this.LogImport.Error(this.Name, "Не удалось скопировать документы для импорта");
                                }
                            });

                        thread.Start();
                    }
                    else
                    {
                        this.LogImport.IsImported = false;
                        this.LogImport.Error(this.Name, "Не удалось обнаружить csv файл для импорта");
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Копируем zip на сервер
        /// </summary>
        /// <param name="zipFile">zip файл</param>
        /// <param name="path">путь на сервере</param>
        protected void CopyDocuments(ZipFile zipFile, string path)
        {
            var filePathWithName = string.Format("{0}{1}", path, this.ZipName);

            if (Directory.Exists(path))
            {
                if (File.Exists(filePathWithName))
                {
                    File.Delete(filePathWithName);
                }
            }
            else
            {
                Directory.CreateDirectory(path);
            }

            zipFile.Save(filePathWithName);
        }

        /// <summary>
        /// Метод возвращающий id домов по периоду и инн организации
        /// </summary>
        /// <param name="periodId">период</param>
        /// <param name="inn">инн</param>
        /// <returns>список id домов</returns>
        protected List<long> GetRealityObjects(long periodId, string inn)
        {
            var periodDi = this.Container.Resolve<IDomainService<PeriodDi>>().Get(periodId);

            return this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>().GetAll()
                .Where(x => x.ManOrgContract.ManagingOrganization.Contragent.Inn == inn)

                // нач2>=нач1 и кон1>=нач2
                // или
                // нач1>=нач2 и кон2>=нач1
                // кон1 или кон2 == null считаем что null соответ + бесконечность
                .Where(x =>
                    ((
                        (x.ManOrgContract.StartDate.HasValue
                            && periodDi.DateStart.HasValue
                            && x.ManOrgContract.StartDate.Value >= periodDi.DateStart.Value)
                        || !periodDi.DateStart.HasValue)
                     && (
                        (x.ManOrgContract.StartDate.HasValue
                            && periodDi.DateEnd.HasValue
                            && periodDi.DateEnd.Value >= x.ManOrgContract.StartDate.Value)
                        || !periodDi.DateEnd.HasValue))
                    ||
                    ((
                        (x.ManOrgContract.StartDate.HasValue
                            && periodDi.DateStart.HasValue
                            && periodDi.DateStart.Value >= x.ManOrgContract.StartDate.Value)
                        || !x.ManOrgContract.StartDate.HasValue)
                     && (
                        (x.ManOrgContract.StartDate.HasValue
                            && periodDi.DateEnd.HasValue
                            && x.ManOrgContract.EndDate.Value >= periodDi.DateStart.Value)
                        || !x.ManOrgContract.EndDate.HasValue)))
                .Select(x => x.RealityObject.Id)
                .Distinct()
                .ToList();
        }

        private new void InitLog(string fileName)
        {
            if (this.LogImportManager == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImportManager");
            }

            this.LogImportManager.FileNameWithoutExtention = fileName;
            this.LogImportManager.UploadDate = DateTime.Now;

            if (this.LogImport == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImport");
            }

            this.LogImport.SetFileName(fileName);
            this.LogImport.ImportKey = this.Key;
        }
    }
}
