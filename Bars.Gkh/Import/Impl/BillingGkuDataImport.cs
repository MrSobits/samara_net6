namespace Bars.Gkh.Import
{
    using System.Linq;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using B4.IoC;
    using Gkh.Domain.ArchieveReader;
    using Bars.Gkh.Enums.Import;
    using Castle.Windsor;
    using System;
    using Impl;

    /// <summary>
    /// Импорт данных из Биллинга
    /// </summary>
    public class BillingGkuDataImport : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public ILogImportManager LogManager { get; set; }
        public ILogImportManager LogImportManager { get; set; }

        #region IGkhImport Members

        public override string Key
        {
            get { return Id; }
        }

        public override string CodeImport
        {
            get { return "BillingDataImport"; }
        }

        public override string Name
        {
            get { return "Импорт сведений по ЖКУ из Биллинга"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "zip,rar"; }
        }

        public override string PermissionName
        {
            get { return "Import.BillingData"; }
        }

        public override ImportResult Import(B4.BaseParams baseParams)
        {
            var sw = new Stopwatch();
            sw.Start();

            var file = baseParams.Files["FileImport"];

            using (var archiveMemoryStream = new MemoryStream(file.Data))
            {
                var reader = ArchiveReaderFactory.Create(file.Extention);

                var archParts =
                    reader.GetArchiveParts(archiveMemoryStream, file.FileName)
                        .Where(x => x.FileName.EndsWith(".csv") || x.FileName.EndsWith(".txt"))
                        .ToArray();

                if (archParts.Length < 1)
                {
                    LogImport.Error("Ошибка", "Отсутствуют файлы для импорта");
                    return new ImportResult(StatusImport.CompletedWithError, "Отсутствуют файлы для импорта");
                }
                try
                {
                    ImportInternal(archParts, file.FileName);
                }
                catch (Exception exc)
                {
                    LogImport.Error("Ошибка", exc.Message);
                    return new ImportResult(StatusImport.CompletedWithError, exc.Message);
                }
            }

            LogImport.SetFileName(file.FileName);
            LogImport.ImportKey = this.CodeImport;

            LogImportManager.FileNameWithoutExtention = file.FileName;
            LogImportManager.Add(file, LogImport);
            LogImportManager.Save();

            var statusImport = LogImport.CountError > 0
                ? StatusImport.CompletedWithError
                : LogImport.CountWarning > 0
                    ? StatusImport.CompletedWithWarning
                    : StatusImport.CompletedWithoutError;

            sw.Stop();

            return new ImportResult(statusImport, string.Format("time elapsed: {0} ms;", sw.ElapsedMilliseconds));
        }

        public override bool Validate(B4.BaseParams baseParams, out string message)
        {
            message = null;
            if (!baseParams.Files.ContainsKey("FileImport"))
            {
                message = "Не выбран файл для импорта";
                return false;
            }

            var fileData = baseParams.Files["FileImport"];
            var extention = fileData.Extention;

            var fileExtentions = PossibleFileExtensions.Contains(",")
                ? PossibleFileExtensions.Split(',')
                : new[] { PossibleFileExtensions };
            if (fileExtentions.All(x => x != extention))
            {
                message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", PossibleFileExtensions);
                return false;
            }

            return true;
        }

        #endregion

        private void ImportInternal(ArchivePart[] entries, string archiveName)
        {
            var billingImporters = Container.ResolveAll<IBillingFileImporter>().OrderBy(x => x.Order);

            using (Container.Using(billingImporters))
            {
                foreach (var importer in billingImporters)
                {
                    var file =
                        entries.FirstOrDefault(
                            x =>
                                // костыль
                                String.Compare(
                                    x.FileName.Replace(archiveName, "").Replace("\\", ""),
                                    string.Format("{0}.csv", importer.FileName),
                                    StringComparison.CurrentCultureIgnoreCase) == 0 ||
                                String.Compare(
                                    x.FileName.Replace(archiveName, "").Replace("\\", ""),
                                    string.Format("{0}.txt", importer.FileName),
                                    StringComparison.CurrentCultureIgnoreCase) == 0);

                    if (file != null)
                    {
                        var stream = file.StreamProvider.OpenStream();
                        if (stream.CanSeek)
                        {
                            stream.Position = 0;
                        }

                        try
                        {
                            importer.Import(stream, archiveName);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(String.Format("{0}:{1}", importer.FileName, ex.Message));
                        }

                    }
                }
            }
        }
    }
}