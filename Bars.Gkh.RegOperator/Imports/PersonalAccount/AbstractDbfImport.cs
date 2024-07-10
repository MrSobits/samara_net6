namespace Bars.Gkh.RegOperator.Imports
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using B4;
    
    using Bars.Gkh.RegOperator.Utils;

    using DbfDataReader;
    using Import;
    using Castle.Windsor;
    using Import.Impl;

     
    // TODO: Проверить работу после смены библиотеки
    
    public abstract class AbstractDbfImport<T> : GkhImportBase
        where T : new()
    {
        #region IGkhImport properties
        public override string PossibleFileExtensions { get { return "dbf"; } }
        
        /// <summary>
        /// Список колонок таблицы DBF
        /// </summary>
        private List<string> columnNames;
        #endregion

        #region IGkhImport methods

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

            var fileData = baseParams.Files["FileImport"].Data;

            try
            {
                using (var stream = new MemoryStream(fileData))
                {
                    using (var table = new DbfTable(stream, Encoding))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                message = "Файл не является корректным .dbf файлом";
                return false;
            }
        }
        #endregion

        protected abstract Encoding Encoding { get; }

        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected readonly Dictionary<string, int> HeadersIndexes = new Dictionary<string, int>();

        /// <summary>
        /// 
        /// </summary>
        protected HashSet<string> FieldsNames = new HashSet<string>();

        protected AbstractDbfImport(ILogImportManager logImportManager, ILogImport logImport)
        {
            this.LogImportManager = logImportManager;
            this.LogImport = logImport;
        }

        protected virtual void ProcessData(byte[] fileData)
        {
            using (var stream = new MemoryStream(fileData))
            {
                using (var table = new DbfTable(stream, Encoding))
                {
                    columnNames = table.Columns.Select(x => x.ColumnName).ToList();
                    FillHeader(table);

                    var index = 0;

                    var dbfRecord = new DbfRecord(table);
                    while (table.Read(dbfRecord))
                    {
                        var row = dbfRecord.ReadToObject<T>(table);
                        ProcessLine(row, index++);
                    }
                }
            }
        }

        protected virtual void FillHeader(DbfTable table)
        {
            var index = 0;
            foreach (var header in this.columnNames)
            {
                if (FieldsNames.Contains(header))
                {
                    HeadersIndexes[header] = index;
                }
                else
                {
                    LogImport.Error("Неизвестное название колонки", string.Format("Данные в колонке '{0}' не будут импортированы", header));
                }

                index++;
            }
        }

        protected abstract void ProcessLine(T row, int rowNumber);

        protected virtual void InitLog(string fileName)
        {
            if (!this.Container.Kernel.HasComponent(typeof(ILogImportManager)))
            {
                throw new ImportException("Не найдена реализация интерфейса ILogImportManager");
            }

            this.LogImportManager.FileNameWithoutExtention = fileName;
            this.LogImportManager.UploadDate = DateTime.Now;

            if (this.LogImport == null)
            {
                throw new ImportException("Не найдена реализация интерфейса ILogImport");
            }

            this.LogImport.SetFileName(fileName);
            this.LogImport.ImportKey = Key;
        }

        protected virtual void ReleaseLog(FileData file)
        {
            this.LogImportManager.FileNameWithoutExtention = file.FileName;
            this.LogImportManager.UploadDate = DateTime.Now;

            LogImport.SetFileName(file.FileName);
            LogImport.ImportKey = Key;

            this.LogImportManager.Add(file, LogImport);
            this.LogImportManager.Save();
        }
    }
}
