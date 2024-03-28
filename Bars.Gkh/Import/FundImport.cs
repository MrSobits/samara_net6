namespace Bars.Gkh.Import
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Enums.Import;

    using Castle.Windsor;
    using Gkh.Import.Impl;

    public class FundImport : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public virtual IWindsorContainer Container { get; set; }

        public override string Key
        {
            get { return Id; }
        }

        public override string CodeImport
        {
            get { return "FundImport"; }
        }

        public override string Name
        {
            get { return "Импорт по форме фонда"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "zip"; }
        }

        public override string PermissionName
        {
            get { return "Import.FundImport.View"; }
        }

        public FundImport(ILogImportManager logImportManager, ILogImport logImport)
        {
            this.LogImportManager = logImportManager;
            this.LogImport = logImport;
        }

        public override ImportResult Import(BaseParams baseParams)
        {
            var message = string.Empty;
            var fileData = baseParams.Files["FileImport"];

            InitLog(fileData.FileName);

            using (var memoryStreamFile = new MemoryStream(fileData.Data))
            {
                memoryStreamFile.Seek(0, SeekOrigin.Begin);
            }

            LogImportManager.Add(fileData, LogImport);
            LogImportManager.Save();

            message += LogImportManager.GetInfo();
            var status = LogImportManager.CountError > 0 ? StatusImport.CompletedWithError : (LogImportManager.CountWarning > 0 ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError);
            return new ImportResult(status, message, string.Empty, LogImportManager.LogFileId);
        }

        public override bool Validate(BaseParams baseParams, out string message)
        {
            message = null;
            if (!baseParams.Files.ContainsKey("FileImport"))
            {
                message = "Не выбран файл для импорта";
                return false;
            }

            var extention = baseParams.Files["FileImport"].Extention;

            var fileExtentions = PossibleFileExtensions.Contains(",") ? PossibleFileExtensions.Split(',') : new[] { PossibleFileExtensions };
            if (fileExtentions.All(x => x != extention))
            {
                message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", PossibleFileExtensions);
                return false;
            }

            return true;
        }

        private void InitLog(string fileName)
        {
            if (this.LogImportManager == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImportManager");
            }

            this.LogImportManager.FileNameWithoutExtention = fileName;
            this.LogImportManager.UploadDate = DateTime.Now;
        }
    }
}
