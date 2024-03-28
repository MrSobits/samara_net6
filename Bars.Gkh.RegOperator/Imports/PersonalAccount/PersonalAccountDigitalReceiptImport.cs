namespace Bars.Gkh.RegOperator.Imports
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.NH.Extentions;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.Gkh.Import.Impl;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Импорт ПИР
    /// </summary>
    public partial class PersonalAccountDigitalReceiptImport : GkhImportBase
    {
        /// <summary>
        /// Id
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        /// <summary>
        /// Ключ импорта
        /// </summary>
        public override string Key => Id;

        /// <summary>
        /// Код группировки импорта (например группировка в меню)
        /// </summary>
        public override string CodeImport => nameof(PersonalAccountDigitalReceiptImport);

        /// <summary>
        /// Наименование импорта
        /// </summary>
        public override string Name => "Импорт признака выдачи электронных квитанций";

        /// <summary>
        /// Разрешенные расширения файлов
        /// </summary>
        public override string PossibleFileExtensions => "csv";

        /// <summary>
        /// Права
        /// </summary>
        public override string PermissionName => "Import.PersonalAccountDigitalReceiptImport.View";

        public new ILogImport LogImport { get; set; }

        /// <summary>
        /// Менеджер управляющий логами
        /// </summary>
        public ILogImportManager LogManager { get; set; }

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
            var extention = baseParams.Files["FileImport"].Extention;

            var fileExtentions = PossibleFileExtensions.Contains(",") ? PossibleFileExtensions.Split(',') : new[] { PossibleFileExtensions };
            if (fileExtentions.All(x => x != extention))
            {
                message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", PossibleFileExtensions);
                return false;
            }

            using (var memoryStreamFile = new MemoryStream(fileData.Data))
            {
                memoryStreamFile.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(memoryStreamFile, Encoding.GetEncoding(1251));
                var headers = reader.ReadLine().ToStr().Split(';');
                if (headers.Length != 4)
                {
                    message = "Содержание файла не соответствует формату импорта";
                    return false;
                }
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
            var message = string.Empty;
            var fileData = baseParams.Files["FileImport"];

            InitLog(fileData.FileName);

            using (var memoryStreamFile = new MemoryStream(fileData.Data))
            {
                memoryStreamFile.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(memoryStreamFile, Encoding.GetEncoding(1251));

                var markDict = new Dictionary<string, YesNo>();
                try
                {
                    var strNum = 1;
                    while (!reader.EndOfStream)
                    {
                        var dataFromRow = reader.ReadLine().ToStr().Split(';');

                        if (dataFromRow.Length != 4)
                        {
                            LogImport.Warn(Name, $"Содержимое строки {strNum} не соответствует формату импорта");
                            continue;
                        }

                        var accNum = dataFromRow[0].Remove(0, 5);
                        var mark = dataFromRow[1].ToInt() == 0 ? YesNo.No : YesNo.Yes;

                        if (markDict.ContainsKey(accNum))
                        {
                            markDict.Remove(accNum);
                            LogImport.Warn(Name, $"Обнаружен дубль для ЛС {accNum}");
                        }
                        markDict.Add(accNum, mark);
                        strNum++;
                    }
                }
                catch (Exception e)
                {
                    try
                    {
                        LogImport.IsImported = false;
                        Container.Resolve<ILogger>().LogError(e, "Импорт");

                        LogImport.Error(Name, "Произошла неизвестная ошибка. Обратитесь к администратору.");
                        message = "Произошла неизвестная ошибка. Обратитесь к администратору";
                    }
                    catch
                    {
                        throw new Exception(e.Message, e);
                    }
                }
                finally
                {
                    memoryStreamFile.Close();
                }

                try
                {
                    if (markDict != null && markDict.Count > 0)
                    {
                        Container.InTransaction(() =>
                        {
                            var persAccDomain = Container.ResolveDomain<BasePersonalAccount>();
                            var persAccs = persAccDomain.GetAll()
                                .Where(x => markDict.Keys.Contains(x.PersonalAccountNum));

                            persAccs.ForEach(x => x.DigitalReceipt = markDict[x.PersonalAccountNum]);
                            persAccs.ForEach(x => persAccDomain.Update(x));
                        });

                        LogImport.IsImported = true;
                        LogImport.CountAddedRows = markDict.Count;
                    }
                }
                catch (Exception e)
                {
                    try
                    {
                        LogImport.IsImported = false;
                        Container.Resolve<ILogger>().LogError(e, "Импорт");

                        LogImport.Error(Name, "Произошла неизвестная ошибка. Обратитесь к администратору.");
                        message = "Произошла неизвестная ошибка. Обратитесь к администратору";
                    }
                    catch
                    {
                        throw new Exception(e.Message, e);
                    }
                }
            }

            var massPersAccService = Container.Resolve<IMassPersonalAccountDtoService>();
            var updateResult = massPersAccService.MassCreatePersonalAccountDto(true);

            LogImport.SetFileName(fileData.FileName);
            LogImport.ImportKey = this.CodeImport;

            LogManager.FileNameWithoutExtention = fileData.FileName;
            LogManager.Add(fileData, LogImport);
            LogManager.Save();

            message += LogManager.GetInfo() + " ";
            message += updateResult.Message;

            var status = !LogImport.IsImported ? StatusImport.CompletedWithError : (LogImport.CountWarning > 0 ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError);
            return new ImportResult(status, message, string.Empty, LogManager.LogFileId);
        }
    }
}