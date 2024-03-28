using System.Text;
using System.Xml.Serialization;
using Bars.Gkh.RegOperator.Imports.ImportRkc;

namespace Bars.Gkh.RegOperator.Imports
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Bars.B4;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Castle.Windsor;
    using Import.Impl;

    /// <summary>
    /// Импорт данных из РКЦ
    /// </summary>
    public class RkcImport : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IWindsorContainer Container { get; set; }

        public ILogImportManager LogImportManager { get; set; }

        public IRkcImportService RkcImportService { get; set; }

        public override string Key
        {
            get { return Id; }
        }

        public override string CodeImport
        {
            get { return "PersonalAccountImport"; }
        }

        public override string Name
        {
            get { return "Импорт ЛС из XML"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "xml"; }
        }

        public override string PermissionName
        {
            get { return "Import.RkcImport"; }
        }

        private bool _isRunning;

        private ImportRkcRecord record;

        public override ImportResult Import(B4.BaseParams baseParams)
        {
            var file = baseParams.Files["FileImport"];

            var stream = new MemoryStream(file.Data) { Position = 0 };
            try
            {
                using (var reader = new StreamReader(stream, Encoding.GetEncoding(1251)))
                {
                    var content = reader.ReadToEnd();

                    var openIndex = content.IndexOf("<?xml", StringComparison.OrdinalIgnoreCase) + 5;
                    var closeIndex = content.IndexOf("?>", openIndex);
 
                    var header = content.Substring(openIndex, closeIndex - openIndex);

 
                    header = header.Replace("version=\"1.1\"", "version=\"1.0\"");

                    content = string.Concat(content.Substring(0, openIndex), header, content.Substring(closeIndex));
                   
                    byte[] byteArray = Encoding.ASCII.GetBytes(content);
                    using (var newStream = new MemoryStream(byteArray))
                    {
                        var ser = new XmlSerializer(typeof(ImportRkcRecord));
                        record = (ImportRkcRecord)ser.Deserialize(newStream);
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception(string.Format("Выбранный файл не соответствует формату выгрузки"));
            }

            var logImport = RkcImportService.Import(record);

            LogImportManager.FileNameWithoutExtention = file.FileName;
            LogImportManager.UploadDate = DateTime.Now;

            logImport.SetFileName(file.FileName);
            logImport.ImportKey = Key;

            LogImportManager.Add(file, logImport);
            LogImportManager.Save();

            var message = LogImportManager.GetInfo();
            var status = LogImportManager.CountError > 0 ? StatusImport.CompletedWithError : (LogImportManager.CountWarning > 0 ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError);
            return new ImportResult(status, message, string.Empty, LogImportManager.LogFileId);
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
