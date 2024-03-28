namespace Bars.Gkh.RegOperator.Imports.SocialSupport
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Bars.B4.IoC;
    using Castle.Windsor;
    using Gkh.Enums.Import;
    using Import;
    using Import.Impl;

    /// <summary>
    /// Импорт данных социальной поддержки
    /// </summary>
    public class SocialSupportImport : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IWindsorContainer Container { get; set; }

        public override string Key
        {
            get { return Id; }
        }

        public override string CodeImport
        {
            get { return "BankDocument"; }
        }

        public override string Name
        {
            get { return "Импорт социальной поддержки"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "csv,txt"; }
        }

        public override string PermissionName
        {
            get { return "Import.SocialSupportImport"; }
        }

        private bool isRunning;

        private Stopwatch sw = new Stopwatch();

        public override ImportResult Import(B4.BaseParams baseParams)
        {
            if (isRunning)
            {
                var elapsed = sw.ElapsedMilliseconds / 1000;
                var text = string.Format("Процесс импорта запущен {0} сек. назад и пока занят, попробуйте позже", elapsed);
                return new ImportResult(StatusImport.CompletedWithError, text);
            }

            sw.Reset();
            sw.Start();

            var file = baseParams.Files["FileImport"];

            var importer = Container.Resolve<IBillingFileImporter>("SocialSupport");

            if (importer == null)
            {
                sw.Stop();

                throw new NotImplementedException("Отсутствует реализация импорта лицевых счетов");
            }

            var stream = new MemoryStream(file.Data) { Position = 0 };
            StatusImport statusImport;

            var logImport = Container.Resolve<ILogImport>();
            var logImportManager = Container.Resolve<ILogImportManager>();

            using (Container.Using(logImport, logImportManager))
            {
                logImport.Info("Старт импорта лицевых счетов", string.Format("Старт импорта лицевых счетов: {0}", file.FileName));

                try
                {
                    isRunning = true;
                    importer.Import(stream, file.FileName, logImport);
                }
                finally
                {
                    isRunning = false;
                    Container.Release(importer);

                    logImport.SetFileName(file.FileName);
                    logImport.ImportKey = Key;

                    logImportManager.FileNameWithoutExtention = file.FileName;
                    logImportManager.Add(file, logImport);
                    logImportManager.Save();

                    statusImport = logImport.CountError > 0
                        ? StatusImport.CompletedWithError
                        : logImport.CountWarning > 0
                            ? StatusImport.CompletedWithWarning
                            : StatusImport.CompletedWithoutError;

                    sw.Stop();
                }

                return new ImportResult(statusImport, string.Empty, string.Empty, logImportManager.LogFileId);
            }
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
    }
}