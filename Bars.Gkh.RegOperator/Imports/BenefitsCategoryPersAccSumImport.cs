namespace Bars.Gkh.RegOperator.Imports
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Security;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.Gkh.Import.Impl;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;

    using Castle.Windsor;

    using NDbfReaderEx;

    /// <summary>
    /// Импорт информации по начисленным льготам
    /// </summary>
    public class BenefitsCategoryPersAccSumImport : GkhImportBase
    {
        /// <summary>
        /// Идентификатор класса импорта
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        /// <summary>
        /// The fields.
        /// </summary>
        private readonly HashSet<string> fields = new HashSet<string>
        {
            "LSA",
            "SUML1"
        };

        /// <summary>
        /// The headers dictionary.
        /// </summary>
        private readonly Dictionary<string, int> headersDict = new Dictionary<string, int>();

        /// <summary>
        /// Записи для сохранения
        /// </summary>
        private readonly List<PersonalAccountBenefits> benefitsesToSave = new List<PersonalAccountBenefits>();

        /// <summary>
        /// The exist benefits dictionary.
        /// </summary>
        private Dictionary<long, PersonalAccountBenefits> existBenefits = new Dictionary<long, PersonalAccountBenefits>();

        /// <summary>
        /// The exist personal account dictionary.
        /// </summary>
        private Dictionary<string, BasePersonalAccount> persAccByNum = new Dictionary<string, BasePersonalAccount>();

        /// <summary>
        /// Gets the key.
        /// </summary>
        public override string Key
        {
            get { return Id; }
        }

        /// <summary>
        /// Gets the code import.
        /// </summary>
        public override string CodeImport
        {
            get { return "PersonalAccountBenefits"; }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public override string Name
        {
            get { return "Импорт начисленных льгот"; }
        }

        /// <summary>
        /// Gets the possible file extensions.
        /// </summary>
        public override string PossibleFileExtensions
        {
            get { return "dbf"; }
        }

        /// <summary>
        /// Gets the permission name.
        /// </summary>
        public override string PermissionName
        {
            get { return "Import.BenefitsCategoryPersAccSum"; }
        }

        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Gets or sets the charge period domain.
        /// </summary>
        public IDomainService<ChargePeriod> ChargePeriodDomain { get; set; }

        /// <summary>
        /// Gets or sets the personal account domain.
        /// </summary>
        public IDomainService<BasePersonalAccount> PersonalAccountDomain { get; set; }

        /// <summary>
        /// Gets or sets the pers acc benefits domain.
        /// </summary>
        public IDomainService<PersonalAccountBenefits> PersonalAccountBenefitsDomain { get; set; }

        /// <summary>
        /// Gets or sets the user repo.
        /// </summary>
        public IRepository<User> UserRepo { get; set; }

        /// <summary>
        /// Gets or sets the identity.
        /// </summary>
        public IUserIdentity Identity { get; set; }

        /// <summary>
        /// Gets or sets the charge period .
        /// </summary>
        public ChargePeriod Period { get; set; }

        public BenefitsCategoryPersAccSumImport(ILogImportManager logManager, ILogImport logImport)
        {
            this.LogImport = logImport;
            this.LogImportManager = logManager;
        }

        /// <summary>
        /// Валидация импорта.
        /// </summary>
        /// <param name="baseParams">
        /// The base params.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Validate(BaseParams baseParams, out string message)
        {
            message = null;
            if (!baseParams.Files.ContainsKey("FileImport"))
            {
                message = "Не выбран файл для импорта";
                return false;
            }

            var extention = baseParams.Files["FileImport"].Extention.Trim().ToLower();

            var fileExtentions = PossibleFileExtensions.Contains(",")
                ? PossibleFileExtensions.Split(',')
                : new[] { this.PossibleFileExtensions };

            if (fileExtentions.All(x => x != extention))
            {
                message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", this.PossibleFileExtensions);
                return false;
            }

            var fileData = baseParams.Files["FileImport"].Data;

            // в исходном файле заказчика имеется запись в заголовке  "Code Page Mark", на которую DbfTable ругается
            if (fileData.Length > 30)
            {
                fileData[29] = 0;
            }

            try
            {
                using (var stream = new MemoryStream(fileData))
                {
                    using (DbfTable.Open(stream, Encoding.GetEncoding(866)))
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

        /// <summary>
        /// Метод импорта.
        /// </summary>
        /// <param name="baseParams">
        /// The base params.
        /// </param>
        /// <returns>
        /// The <see cref="ImportResult"/>.
        /// </returns>
        public override ImportResult Import(BaseParams baseParams)
        {
            var file = baseParams.Files["FileImport"];
            var periodId = baseParams.Params.GetAsId("periodId");

            Period = ChargePeriodDomain.Get(periodId);

            try
            {
                InitLog(file.FileName);

                InitDictionaries();

                ProcessData(file.Data);

                this.LogImportManager.FileNameWithoutExtention = file.FileName;
                this.LogImportManager.UploadDate = DateTime.Now;

                this.LogImport.SetFileName(file.FileName);
                this.LogImport.ImportKey = this.Key;

                this.LogImportManager.Add(file, this.LogImport);
                this.LogImportManager.Save();

                var message = this.LogImportManager.GetInfo();
                var status = this.LogImportManager.CountError > 0
                    ? StatusImport.CompletedWithError
                    : (this.LogImportManager.CountWarning > 0
                        ? StatusImport.CompletedWithWarning
                        : StatusImport.CompletedWithoutError);

                return new ImportResult(status, message, string.Empty, this.LogImportManager.LogFileId);
            }
            catch (ImportException e)
            {
                return new ImportResult(StatusImport.CompletedWithError, e.Message);
            }
        }

        /// <summary>
        /// Инициализировать лог импорта.
        /// </summary>
        /// <param name="fileName">
        /// Имя файла.
        /// </param>
        /// <exception cref="ImportException">
        /// Исключение импорта
        /// </exception>
        private void InitLog(string fileName)
        {
            if (!Container.Kernel.HasComponent(typeof(ILogImportManager)))
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

        /// <summary>
        /// Инициализировать переменные.
        /// </summary>
        private void InitDictionaries()
        {
            persAccByNum = PersonalAccountDomain.GetAll()
                .AsEnumerable()
                .GroupBy(x => x.PersonalAccountNum)
                .ToDictionary(x => x.Key, y => y.First());
        }

        /// <summary>
        /// Обработать данные.
        /// </summary>
        /// <param name="fileData">
        /// Массив байтов данных.
        /// </param>
        private void ProcessData(byte[] fileData)
        {
            // в исходном файле заказчика имеется запись в заголовке  "Code Page Mark", на которую DbfTable ругается
            if (fileData.Length > 30)
            {
                fileData[29] = 0;
            }

            using (var stream = new MemoryStream(fileData))
            {
                try
                {
                    using (var table = DbfTable.Open(stream, Encoding.GetEncoding(866)))
                    {
                        FillHeader(table);

                        var invalidHeaders = fields.Where(x => !headersDict.ContainsKey(x)).ToList();

                        if (invalidHeaders.Count > 0)
                        {
                            foreach (var invalidHeader in invalidHeaders)
                            {
                                this.LogImport.Error("Ошибка", "Не найден атрибут ({0})".FormatUsing(invalidHeader));
                            }

                            return;
                        }

                        var rowCnt = 0;

                        var importInfo = table
                            .Select(x => new
                            {
                                AccNum = GetValueOrDefault(x, "LSA"),
                                Benefits = GetValueOrDefault(x, "SUML1")
                            }).ToList();


                        if (importInfo.All(x => x.AccNum.IsEmpty()) || importInfo.All(x => x.Benefits.IsEmpty()))
                        {
                            this.LogImport.Error("Ошибка", "Не заполнены обязательные поля. Загрузка невозможна.");
                            return;
                        }

                        if (importInfo.All(x => x.AccNum.ToInt() == 0) || importInfo.All(x => x.Benefits.ToDecimal() == 0))
                        {
                            this.LogImport.Error("Ошибка", "Некорректный тип обязательных атрибутов. Загрузка невозможна");
                            return;
                        }

                        foreach (var row in table)
                        {
                            rowCnt++;

                            var accNum = GetValueOrDefault(row, "LSA");

                            if (accNum.IsEmpty())
                            {
                                this.LogImport.Warn("Предупреждение", "Не заполнено обязательное поле (\"LSA\"). Строка: {0}".FormatUsing(rowCnt));
                                continue;
                            }

                            if (accNum.ToInt() == 0)
                            {
                                this.LogImport.Warn("Предупреждение", "Некорректный тип атрибута (\"LSA\"). Строка: {0}".FormatUsing(rowCnt));
                                continue;
                            }

                            var persAcc = persAccByNum.Get(accNum);

                            if (persAcc == null)
                            {
                                this.LogImport.Warn("Предупреждение", "Не найден лицевой счет. Данные по ЛС({0}) не загружены. Строка: {1}".FormatUsing(accNum, rowCnt));
                                continue;
                            }

                            var benefits = 0.0m;
                            var benefitsStr = GetValueOrDefault(row, "SUML1");

                            if (!string.IsNullOrEmpty(benefitsStr) && !decimal.TryParse(benefitsStr, out benefits))
                            {
                                this.LogImport.Warn("Предупреждение", "Некорректный тип атрибута (\"SUML1\"). Строка: {0}".FormatUsing(rowCnt));
                                continue;
                            }

                            var persAccBenefit = new PersonalAccountBenefits
                            {
                                PersonalAccount = persAcc,
                                Period = Period,
                                Sum = benefits
                            };

                            benefitsesToSave.Add(persAccBenefit);

                            this.LogImport.CountAddedRows++;
                        }
                    }
                }
                catch (IOException e)
                {
                    /* Перехватываем исключения метода ReadDbfHeader общего типа IOException
                     * с сообщением, что файл не является DBF — "Not a DBF file! ..."
                     */
                    if (e.Message.Contains("Not a DBF file"))
                    {
                        var msg = string.Format(
                            "Данный формат DBF файла не поддерживается. При открытии возникло исключение с текстом: {0}",
                            e.Message);

                        this.LogImport.Error("Открытие файла", msg);
                        return;
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            using (var session = Container.Resolve<ISessionProvider>().OpenStatelessSession())
            {
                // Удаляем существующие записи
                session.CreateSQLQuery("DELETE FROM REGOP_PERS_ACC_BENEFITS " +
                                       "WHERE PERIOD_ID = :periodId")
                    .SetInt64("periodId", Period.Id)
                    .ExecuteUpdate();
            }

            TransactionHelper.InsertInManyTransactions(Container, benefitsesToSave, 1000, true, true);
        }

        /// <summary>
        /// Заполнить словарь заголовков столбцов.
        /// </summary>
        /// <param name="table">
        /// The table.
        /// </param>
        private void FillHeader(DbfTable table)
        {
            var index = 0;
            foreach (var header in table.columns.Select(col => col.name))
            {
                if (fields.Contains(header))
                {
                    headersDict[header] = index;
                }

                index++;
            }
        }

        /// <summary>
        /// Получить значение из строки dbf по имени столбца.
        /// </summary>
        /// <param name="row">
        /// Строка dbf.
        /// </param>
        /// <param name="column">
        /// Имя столбца.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string GetValueOrDefault(DbfRow row, string column)
        {
            return headersDict.ContainsKey(column) ? row.GetValue(column).ToStr() : string.Empty;
        }
    }
}