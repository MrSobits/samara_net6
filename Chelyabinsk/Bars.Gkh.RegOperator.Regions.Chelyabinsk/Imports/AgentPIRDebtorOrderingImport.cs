namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.Imports
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Import;
    using Bars.Gkh.Import.Impl;
    using Bars.Gkh.RegOperator.Regions.Chelyabinsk.Entities;
    using Ionic.Zip;

    /// <summary>
    /// Импорт ПИР
    /// </summary>
    public partial class AgentPIRDebtorOrderingImport : GkhImportBase
    {
        /// <summary>
        /// Id
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        /// <summary>
        /// Ключ импорта
        /// </summary>
        public override string Key => AgentPIRDebtorOrderingImport.Id;

        /// <summary>
        /// Код группировки импорта (например группировка в меню)
        /// </summary>
        public override string CodeImport => nameof(AgentPIRDebtorOrderingImport);

        /// <summary>
        /// Наименование импорта
        /// </summary>
        public override string Name => "Импорт выписок должников агента ПИР";

        /// <summary>
        /// Разрешенные расширения файлов
        /// </summary>
        public override string PossibleFileExtensions => "zip";

        /// <summary>
        /// Права
        /// </summary>
        public override string PermissionName => "Import.AgentPIRDebtorOrderingImport";

        public new ILogImport LogImport { get; set; }

        public new ILogImportManager LogImportManager { get; set; }

        public IDomainService<AgentPIRDebtor> debtorDomain { get; set; }

        private Dictionary<string, byte[]> fileDict;

        /// <summary>
        /// Первоночальная валидация файла перед импортом
        /// </summary>
        /// <param name="baseParams"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public override bool Validate(BaseParams baseParams, out string message)
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

            if ((fileData.Data.LongLength * 8) > 1073741824)
            {
                message = string.Format("Необходимо выбрать файл размером менее 128МБ, размер вашего файла: {0}МБ", fileData.Data.LongLength / 1048576);
                return false;
            }
                
            return true;
        }

        /// <summary>
        /// Импорт
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат</returns>
        public override ImportResult Import(BaseParams baseParams)
        {
            var file = baseParams.Files["FileImport"];

            this.LogImportManager.FileNameWithoutExtention = file.FileName;
            this.LogImportManager.UploadDate = DateTime.Now;

            this.LogImport.SetFileName(file.FileName);
            this.LogImport.ImportKey = this.Key;

            this.ProcessData(file.Data);

            this.LogImportManager.Add(file, this.LogImport);
            this.LogImportManager.Save();

            return new ImportResult();
        }

        private void ProcessData(byte[] fileData)
        {
            using (var zipfileMemoryStream = new MemoryStream(fileData))
            {
                using (var zipFile = ZipFile.Read(zipfileMemoryStream))
                {
                    var zipEntries = zipFile.Where(x => x.FileName.EndsWith(".pdf")).ToArray();

                    if (zipEntries.Length < 1)
                        LogImport.Error("Ошибка", "Отсутствуют файлы для импорта");

                    this.CreateDocument(zipEntries);
                }
            }
        }

        private void CreateDocument(ZipEntry[] zipEntries)
        {
          
            var fileManager = this.Container.Resolve<IFileManager>();
            this.fileDict = new Dictionary<string, byte[]>();

            foreach (var file in zipEntries)
            {
                Stream ms = new MemoryStream();
                file.Extract(ms);
              
                var fileinfo = fileManager.SaveFile(ms, $"{file.FileName}");
                if (!string.IsNullOrEmpty(file.FileName) && file.FileName.Contains("."))
                {
                    var docNum = file.FileName.Split('.')[0];
                    try
                    {
                        var debtor = debtorDomain.GetAll().FirstOrDefault(x => x.BasePersonalAccount.PersonalAccountNum == docNum);
                        if (debtor != null)
                        {
                            debtor.Ordering = fileinfo;
                            debtorDomain.Update(debtor);
                        }
                    }
                    catch
                    {
                        
                    }
                    
                }
                
            }          

        }
    }
}