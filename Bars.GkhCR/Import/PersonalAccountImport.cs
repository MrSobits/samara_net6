namespace Bars.GkhCr.Import
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;
    using Bars.GkhExcel;

    using Castle.Windsor;
    using Gkh.Import.Impl;

    using Microsoft.Extensions.Logging;

    public class PersonalAccountImport : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public virtual IWindsorContainer Container { get; set; }

        public override string Key
        {
            get { return Id; }
        }

        public override string CodeImport
        {
            get { return "ObjectCr"; }
        }

        public override string Name
        {
            get { return "Импорт лицевых счетов"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "xls"; }
        }

        public override string PermissionName
        {
            get { return "Import.PersonalAccount.View"; }
        }

        public PersonalAccountImport(ILogImportManager logImportManager, ILogImport logImport)
        {
            this.LogImportManager = logImportManager;
            this.LogImport = logImport;
        }

        public override ImportResult Import(BaseParams baseParams)
        {
           var message = string.Empty;

            var fileData = baseParams.Files["FileImport"];
            var programId = baseParams.Params["ProgramCr"].ToInt();

            InitLog(fileData.FileName);

            var transaction = this.Container.Resolve<IDataTransaction>();
            using (this.Container.Using(transaction))
            {
                try
                {
                    var excel = this.Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider");
                    using (this.Container.Using(excel))
                    {
                        using (var memoryStreamFile = new MemoryStream(fileData.Data))
                        {
                            if (excel == null)
                            {
                                throw new Exception("Не найдена реализация интерфейса IGkhExcelProvider");
                            }

                            excel.Open(memoryStreamFile);

                            foreach (var row in excel.GetRows(0, 0)
                                .Where(row => row.Length >= 4 && !row[0].FontBold && !string.IsNullOrEmpty(row[1].Value)))
                            {
                                var serPersonalAccount = Container.Resolve<IDomainService<PersonalAccount>>();
                                var serObjectCr = Container.Resolve<IDomainService<ObjectCr>>();
                                ImportRow(row, programId, serPersonalAccount, serObjectCr);
                            }

                            transaction.Commit();
                            LogImport.IsImported = true;
                        }
                    }
                }
                catch (Exception exp)
                {
                    try
                    {
                        LogImport.IsImported = false;
                        Container.Resolve<ILogger>().LogError(exp, "Импорт");
                        message = "Произошла неизвестная ошибка";
                        LogImport.Error(Name, "Произошла неизвестная ошибка. Обратитесь к администратору");

                        transaction.Rollback();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message, exp);
                    }
                }
            }

            // добавляем лог в logManager для последующего сохранения в системе
            LogImportManager.Add(fileData, LogImport);
            message += LogImportManager.GetInfo();
            LogImportManager.Save();

            var status = !LogImport.IsImported ? StatusImport.CompletedWithError : (LogImport.CountWarning > 0 ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError);
            return new ImportResult(status, message, string.Empty, LogImportManager.LogFileId);
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

                var bytes = baseParams.Files["FileImport"].Data;
                var extention = baseParams.Files["FileImport"].Extention;

                var fileExtentions = PossibleFileExtensions.Contains(",") ? PossibleFileExtensions.Split(',') : new[] { PossibleFileExtensions };
                if (fileExtentions.All(x => x != extention))
                {
                    message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", PossibleFileExtensions);
                    return false;
                }

                var excel = this.Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider");
                using (this.Container.Using(excel))
                {
                    if (excel == null)
                    {
                        throw new Exception("Не найдена реализация интерфейса IGkhExcelProvider");
                    }

                    using (var memoryStreamFile = new MemoryStream(bytes))
                    {
                        excel.Open(memoryStreamFile);

                        if (excel.IsEmpty(0, 0))
                        {
                            message = string.Format("Не удалось обнаружить записи в файле: {0}", PossibleFileExtensions);
                            return false;
                        }

                        var title = excel.GetRows(0, 0)[0];

                        if (title[0].Value.Trim() != "Муниципальное образование" || title[1].Value.Trim() != "Адрес объекта"
                            || title[2].Value.Trim() != "Лицевой счет объекта по 185 ФЗ"
                            || title[3].Value.Trim() != "Лицевой счет на другие разрезы финансирования")
                        {
                            message = "Заголовки столбцов не соответствуют шаблону";
                            return false;
                        }

                        return true;
                    }
                }
            }
            catch (Exception exp)
            {
                Container.Resolve<ILogger>().LogError(exp, "Валидация файла импорта");
                message = "Произошла неизвестная ошибка при проверки формата файла";
                return false;
            }
        }

        private void InitLog(string fileName)
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
            this.LogImport.ImportKey = Key;
        }

        private void ImportRow(GkhExcelCell[] row, int programId, IDomainService<PersonalAccount> serPersonalAccount, IDomainService<ObjectCr> serObjectCr)
        {
            var region = row[0].Value;
            var address = row[1].Value;

            var for185F3 = row[2].Value;
            var forOtherResurcesFin = row[3].Value;

            var objectCr = serObjectCr.GetAll().FirstOrDefault(x => x.ProgramCr.Id == programId && x.RealityObject.Address == address && x.RealityObject.Municipality.Name == region);
            if (objectCr != null)
            {
                var realtyObjName = objectCr.RealityObject.Address;

                if (!string.IsNullOrWhiteSpace(for185F3))
                {
                    var perAccountFor185F3 = serPersonalAccount.GetAll()
                        .FirstOrDefault(x => x.ObjectCr.Id == objectCr.Id && x.Account == for185F3);

                    if (perAccountFor185F3 == null)
                    {
                        perAccountFor185F3 = new PersonalAccount
                            {
                                Closed = false,
                                Account = for185F3,
                                FinanceGroup = TypeFinanceGroup.ProgramCr,
                                ObjectCr = objectCr
                            };

                        serPersonalAccount.Save(perAccountFor185F3);

                        var msg = string.Format("Добавлен лицевой счет {0} в объект КР {1} с группой финансирования Программа КР", for185F3, realtyObjName);
                        LogImport.Info(Name, msg, LogTypeChanged.Added);
                    }
                    else
                    {
                        var msg = string.Format("Указанный лицевой счет {0} с группой финансирования Программа КР, уже внесен в объект КР {1}", for185F3, realtyObjName);
                        LogImport.Info(Name, msg);
                    }
                }

                if (!string.IsNullOrWhiteSpace(forOtherResurcesFin))
                {
                    var perAccountForOtherResurcesFin = serPersonalAccount.GetAll()
                        .FirstOrDefault(x => x.ObjectCr.Id == objectCr.Id && x.Account == forOtherResurcesFin);
                    
                    if (perAccountForOtherResurcesFin == null)
                    {
                        perAccountForOtherResurcesFin = new PersonalAccount
                            {
                                Closed = false,
                                Account = forOtherResurcesFin,
                                FinanceGroup = TypeFinanceGroup.Other,
                                ObjectCr = objectCr
                            };

                        serPersonalAccount.Save(perAccountForOtherResurcesFin);

                        var msg = string.Format("Добавлен лицевой счет {0} в объект КР {1} с группой финансирования Другие", forOtherResurcesFin, realtyObjName);
                        LogImport.Info(Name, msg, LogTypeChanged.Added);
                    }
                    else
                    {
                        var msg = string.Format("Указанный лицевой счет {0} с группой финансирования Другие, уже внесен в объект КР {1}", forOtherResurcesFin, realtyObjName);
                        LogImport.Info(Name, msg);
                    }
                }
            }
            else
            {
                var msg = string.Format("Адрес дома {0} или район {1}, введен неправильно или дом не попадает в выбранную программу капитального ремонта", address, region);
                LogImport.Warn(Name, msg);
            }
        }
    }
}
