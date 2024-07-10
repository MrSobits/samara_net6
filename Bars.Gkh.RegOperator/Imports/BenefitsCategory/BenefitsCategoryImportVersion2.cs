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
    using Bars.Gkh.RegOperator.Entities.Dict;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Imports.BenefitsCategory;
    using Bars.Gkh.RegOperator.Utils;

    using Castle.Windsor;

    using DbfDataReader;
    
    // TODO: Проверить работу после смены библиотеки

    public class BenefitsCategoryImportVersion2 : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        /// <summary>
        /// The fields.
        /// </summary>
        private readonly HashSet<string> fields = new HashSet<string>
        {
            "LSA",
            "FAMIL",
            "IMJA",
            "OTCH",
            "KOD_POST",
            "KOD_NNASP",
            "KOD_NYLIC",
            "NDOM",
            "NKORP",
            "NKW",
            "NKOMN",
            "NKOD",
            "DATLGTS1",
            "DATLGTPO1"
        };

        /// <summary>
        /// The headers dictionary.
        /// </summary>
        private readonly Dictionary<string, int> headersDict = new Dictionary<string, int>();

        /// <summary>
        /// Импортируемые записи.
        /// </summary>
        private readonly List<RecordVersion2> records = new List<RecordVersion2>();

        /// <summary>
        /// Записи для сохранения
        /// </summary>
        private readonly List<PersonalAccountPrivilegedCategory> persAccPrivCategsToSave = new List<PersonalAccountPrivilegedCategory>();

        /// <summary>
        /// Заменить данные.
        /// </summary>
        private bool isOverwriteData;

        /// <summary>
        /// Идентификация по номеру лс или по Адресу + ФИО.
        /// </summary>
        private BenefitsCategoryImportIdentificationType identificationType;

        /// <summary>
        /// Словарь существующих записей в таблице льготных категорий лицевых счетов 
        /// сгруппированных по ЛС
        /// </summary>
        private Dictionary<long, List<RecordVersion2>> privelegedCategoriesByAccIdDict;

        /// <summary>
        /// Словарь существующих записей в таблице льготных категорий лицевых счетов 
        /// сгруппированных по адресу и ФИО
        /// </summary>
        private Dictionary<string, List<RecordVersion2>> privelegedCategoriesByAddrCodeDict;

        /// <summary>
        /// Словарь существующих льготных категорий сгруппированных по коду
        /// </summary>
        private Dictionary<string, PrivilegedCategory> privelegedCategoriesByCodeDict;

        /// <summary>
        /// Словарь id лицевых счетов по номеру
        /// </summary>
        private Dictionary<string, long> accountIdByAccountNumberDict;

        /// <summary>
        /// Список колонок таблицы DBF
        /// </summary>
        private List<string> columnNames;

        /// <summary>
        /// Словарь id лицевых счетов по коду адреса + имени абонента
        /// </summary>
        private Dictionary<string, long> accountIdByAddressFio;

        public BenefitsCategoryImportVersion2(ILogImportManager logImportManager, ILogImport logImport)
        {
            this.LogImportManager = logImportManager;
            this.LogImport = logImport;
        }

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
            get { return "PersonalAccountImport"; }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public override string Name
        {
            get { return "Импорт сведений по льготным категориям граждан 2"; }
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
            get { return "Import.BenefitsCategoryImportVersion2"; }
        }

        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Gets or sets the privileged category domain.
        /// </summary>
        public IDomainService<PrivilegedCategory> PrivilegedCategoryDomain { get; set; }

        /// <summary>
        /// Gets or sets the base per acc domain.
        /// </summary>
        public IRepository<BasePersonalAccount> BasePerAccRepo { get; set; }

        /// <summary>
        /// Gets or sets the per acc owner domain.
        /// </summary>
        public IDomainService<PersonalAccountOwner> PerAccOwnerDomain { get; set; }

        /// <summary>
        /// Gets or sets the pers acc priveleged category domain.
        /// </summary>
        public IRepository<PersonalAccountPrivilegedCategory> PersonalAccountPrivilegedCategoryRepo { get; set; }

        /// <summary>
        /// Gets or sets the reality object repository.
        /// </summary>
        public IRepository<RealityObject> RealityObjectRepository { get; set; }

        /// <summary>
        /// Gets or sets the user repo.
        /// </summary>
        public IRepository<User> UserRepo { get; set; }

        /// <summary>
        /// Gets or sets the identity.
        /// </summary>
        public IUserIdentity Identity { get; set; }

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

            try
            {
                using (var stream = new MemoryStream(fileData))
                {
                    using (new DbfTable(stream, Encoding.GetEncoding(866)))
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
            isOverwriteData = baseParams.Params.GetAs<bool>("replaceData");
            identificationType = baseParams.Params.GetAs<BenefitsCategoryImportIdentificationType>("IdentificationType");

            try
            {
                InitLog(file.FileName);

                InitDictionaries();

                ProcessData(file.Data);

                this.LogImportManager.FileNameWithoutExtention = file.FileName;
                this.LogImportManager.UploadDate = DateTime.Now;

                LogImport.SetFileName(file.FileName);
                LogImport.ImportKey = this.Key;

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

        /// <summary>
        /// Инициализировать переменные.
        /// </summary>
        private void InitDictionaries()
        {
            // заполняю словарь получения id льготной категории по коду
            privelegedCategoriesByCodeDict = PrivilegedCategoryDomain.GetAll()
                .GroupBy(x => x.Code)
                .ToDictionary(x => x.Key, x => x.First());

            if (identificationType == BenefitsCategoryImportIdentificationType.AccNumIdentification)
            {
                // заполняю словарь получения лицевого счета по номеру
                accountIdByAccountNumberDict = BasePerAccRepo.GetAll()
                    .Where(x => x.PersonalAccountNum != null)
                    .Select(x => new
                    {
                        x.PersonalAccountNum,
                        x.Id
                    })
                    .ToList()
                    .GroupBy(x => x.PersonalAccountNum)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.Id).First());
            }
            else
            {
                accountIdByAddressFio = BasePerAccRepo.GetAll()
                    .Where(x => x.Room != null && x.Room.RealityObject != null)
                    .Select(x => new
                    {
                        x.Id,
                        x.Room.RoomNum,
                        x.Room.RealityObject.AddressCode,
                        x.AccountOwner.Name
                    })
                    .ToArray()
                    .GroupBy(x => string.Format("{0}{1}#{2}", x.AddressCode, x.RoomNum, x.Name))
                    .ToDictionary(x => x.Key, x => x.First().Id);
            }

            // если выбрана идентификация по номеру ЛС, то заполняю словарь
            // существующими записями в гриде льготных категорий по ЛС
            if (identificationType == BenefitsCategoryImportIdentificationType.AccNumIdentification)
            {
                privelegedCategoriesByAccIdDict = PersonalAccountPrivilegedCategoryRepo.GetAll()
                    .Where(x => x.PersonalAccount != null && x.PrivilegedCategory != null)
                    .Select(x => new
                    {
                        x.Id,
                        PersAccId = x.PersonalAccount.Id,
                        x.DateFrom,
                        x.DateTo
                    })
                    .ToList()
                    .GroupBy(x => x.PersAccId)
                    .ToDictionary(x => x.Key, x => x.Select(y => new RecordVersion2
                    {
                        PersAccPrivilegedCategoryId = y.Id,
                        DateFrom = y.DateFrom,
                        DateTo = y.DateTo
                    }).ToList());
            }

            // если выбрана идентификация по Адресу + ФИО, то заполняю словарь
            // существующими записями в гриде льготных категорий по ЛС
            else
            {
                privelegedCategoriesByAddrCodeDict = PersonalAccountPrivilegedCategoryRepo.GetAll()
                    .Where(x => x.PersonalAccount != null && x.PersonalAccount.AccountOwner != null
                        && x.PersonalAccount.Room != null && x.PersonalAccount.Room.RealityObject != null
                        && x.PrivilegedCategory != null)
                    .ToList()
                    .Select(x => new
                    {
                        x.Id,
                        Key = string.Format(
                            "{0}{1}#{2}", 
                            x.PersonalAccount.Room.RealityObject.AddressCode,
                            x.PersonalAccount.Room.RoomNum,
                            x.PersonalAccount.AccountOwner.Name),
                        x.DateFrom,
                        x.DateTo
                    })
                    .ToList()
                    .GroupBy(x => x.Key)
                    .ToDictionary(
                        x => x.Key, 
                        x => x.Select(y => new RecordVersion2
                        {
                            PersAccPrivilegedCategoryId = y.Id,
                            DateFrom = y.DateFrom,
                            DateTo = y.DateTo
                        }).ToList());
            }
        }

        /// <summary>
        /// Обработать данные.
        /// </summary>
        /// <param name="fileData">
        /// Массив байтов данных.
        /// </param>
        private void ProcessData(byte[] fileData)
        {
            using (var stream = new MemoryStream(fileData))
            {
                try
                {
                    using (var table = new DbfTable(stream, Encoding.GetEncoding(866)))
                    {
                        FillHeader(table);

                        var dbfRecord = new DbfRecord(table);
                        columnNames = table.Columns.Select(x => x.ColumnName).ToList();
                        var index = 0;
                        while (table.Read(dbfRecord))
                        {
                            var record = ReadLine(dbfRecord, index);

                            if (record.IsValidRecord)
                            {
                                records.Add(record);
                            }

                            index++;
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
                        
                        LogImport.Error("Открытие файла", msg);
                        return;
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // если выбрана идентификация по номеру ЛС
            if (identificationType == BenefitsCategoryImportIdentificationType.AccNumIdentification)
            {
                foreach (var record in records)
                {
                    var existedRecords = privelegedCategoriesByAccIdDict.ContainsKey(record.PersAcc.Id)
                        ? privelegedCategoriesByAccIdDict[record.PersAcc.Id]
                        : null;

                    var persAccCategoryForSamePeriod = existedRecords != null
                        ? GetCategoryForSamePeriod(existedRecords, record)
                        : null;

                    if (existedRecords == null || persAccCategoryForSamePeriod == null)
                    {
                        var persAccPrivCateg = new PersonalAccountPrivilegedCategory
                        {
                            PersonalAccount = record.PersAcc,
                            PrivilegedCategory = record.PrivilegedCategory,
                            DateFrom = record.DateFrom,
                            DateTo = record.DateTo
                        };

                        persAccPrivCategsToSave.Add(persAccPrivCateg);
                        LogImport.CountAddedRows++;
                    }
                    else{
                        if (isOverwriteData)
                        {
                            var persAccPrivCateg = new PersonalAccountPrivilegedCategory
                                                       {
                                                           Id =
                                                               persAccCategoryForSamePeriod
                                                               .PersAccPrivilegedCategoryId,
                                                           PersonalAccount =
                                                               record.PersAcc,
                                                           PrivilegedCategory =
                                                               record
                                                               .PrivilegedCategory,
                                                           DateFrom = record.DateFrom,
                                                           DateTo = record.DateTo
                                                       };
                            
                            persAccPrivCategsToSave.Add(persAccPrivCateg);
                            LogImport.CountChangedRows++;
                        }
                        else
                        {
                            LogImport.Error(record.RowNumber.ToString(), "Льготная категория для данного ЛС за этот период уже есть");
                        }
                    }
                }
            }

            // если выбрана идентификация по Адресу + ФИО
            else
            {
                foreach (var record in records)
                {
                    var key = string.Format("{0}#{1}", record.AddressCode, record.OwnerFullName);

                    var existedRecords = privelegedCategoriesByAddrCodeDict.ContainsKey(key)
                        ? privelegedCategoriesByAddrCodeDict[key]
                        : null;

                    var persAccCategoryForSamePeriod = existedRecords != null
                        ? GetCategoryForSamePeriod(existedRecords, record)
                        : null;

                    if (existedRecords == null || persAccCategoryForSamePeriod == null)
                    {
                        var persAccPrivCateg = new PersonalAccountPrivilegedCategory
                        {
                            PersonalAccount = record.PersAcc,
                            PrivilegedCategory = record.PrivilegedCategory,
                            DateFrom = record.DateFrom,
                            DateTo = record.DateTo
                        };

                        persAccPrivCategsToSave.Add(persAccPrivCateg);
                        LogImport.CountAddedRows++;
                    }
                    else
                    {
                        if (isOverwriteData)
                        {
                            var persAccPrivCateg = new PersonalAccountPrivilegedCategory
                            {
                                Id = persAccCategoryForSamePeriod.PersAccPrivilegedCategoryId,
                                PersonalAccount = record.PersAcc,
                                PrivilegedCategory = record.PrivilegedCategory,
                                DateFrom = record.DateFrom,
                                DateTo = record.DateTo
                            };

                            persAccPrivCategsToSave.Add(persAccPrivCateg);
                            LogImport.CountChangedRows++;
                        }
                        else
                        {
                            LogImport.Error(record.RowNumber.ToString(), "Льготная категория для данного ЛС за этот период уже есть");
                        }
                    }
                }
            }

            TransactionHelper.InsertInManyTransactions(Container, persAccPrivCategsToSave, 1000, true, true);
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
            foreach (var header in table.Columns.Select(col => col.ColumnName))
            {
                if (fields.Contains(header))
                {
                    headersDict[header] = index;
                }
                else
                {
                    LogImport.Warn("Неизвестное название колонки", string.Format("Данные в колонке '{0}' не будут импортированы", header));
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
        private string GetValueOrDefault(DbfRecord row, string column)
        {
            return headersDict.ContainsKey(column) ? row.GetValueOrDefault(column, columnNames).ToString() : string.Empty;
        }

        /// <summary>
        /// Прочитать данные из строки dbf
        /// </summary>
        /// <param name="row">
        /// The row.
        /// </param>
        /// <returns>
        /// The <see cref="RecordVersion2"/>.
        /// </returns>
        private RecordVersion2 ReadLine(DbfRecord row, int rowNumber)
        {
            var record = new RecordVersion2
            {
                IsValidRecord = false,
                RowNumber = rowNumber,
                AccountNum = GetValueOrDefault(row, "LSA"),
                OwnerFullName = string.Format(
                    "{0} {1} {2}",
                    GetValueOrDefault(row, "FAMIL"),
                    GetValueOrDefault(row, "IMJA"),
                    GetValueOrDefault(row, "OTCH"))
            };

            if (identificationType == BenefitsCategoryImportIdentificationType.AccNumIdentification)
            {
                if (record.AccountNum.IsEmpty())
                {
                    LogImport.Error(record.RowNumber.ToString(), "Не указан номер ЛС");
                    return record;
                }

                if (!accountIdByAccountNumberDict.ContainsKey(record.AccountNum))
                {
                    LogImport.Error(record.RowNumber.ToString(), "Данный ЛС не найден в базе");
                    return record;
                }

                record.PersAcc = new BasePersonalAccount
                {
                    Id = accountIdByAccountNumberDict[record.AccountNum]
                };
            }
            else
            {
                if (record.OwnerFullName.IsEmpty())
                {
                    LogImport.Error(record.RowNumber.ToString(), "Не указано ФИО абонента");
                    return record;
                }

                if (!CheckAddressCodeInfo(record, row))
                {
                    return record;
                }

                record.AddressCode = string.Format(
                    "{0}{1}{2}{3}{4}{5}{6}",
                    record.CodePost.PadLeft(3, '0'),
                    record.CodeNnasp.PadLeft(5, '0'),
                    record.CodeNylic.PadLeft(5, '0'),
                    record.Ndom.PadLeft(4, '0'),
                    record.Nkorp.PadLeft(3, '0'),
                    record.Nkw,
                    record.Nkomn);

                if (!accountIdByAddressFio.ContainsKey(string.Format("{0}#{1}", record.AddressCode, record.OwnerFullName)))
                {
                    LogImport.Error(record.RowNumber.ToString(), "Не найден ЛС с указанным кодом адреса и ФИО абонента");
                    return record;
                }

                record.PersAcc = new BasePersonalAccount
                {
                    Id = accountIdByAddressFio[string.Format("{0}#{1}", record.AddressCode, record.OwnerFullName)]
                };
            }

            if (GetValueOrDefault(row, "NKOD").IsEmpty())
            {
                LogImport.Error(record.RowNumber.ToString(), "Не указан код льготной категории");
                return record;
            }
            record.Ncode = GetValueOrDefault(row, "NKOD");

            if (GetValueOrDefault(row, "DATLGTS1").IsEmpty()
                || GetValueOrDefault(row, "DATLGTS1").ToDateTime() == DateTime.MinValue)
            {
                LogImport.Error(record.RowNumber.ToString(),
                    "Не указана дата начала действия значения льготной категории или значение не является датой");
                return record;
            }
            record.DateFrom = GetValueOrDefault(row, "DATLGTS1").ToDateTime();

            if (!GetValueOrDefault(row, "DATLGTPO1").IsEmpty())
            {
	            if (GetValueOrDefault(row, "DATLGTPO1").ToDateTime() == DateTime.MinValue)
	            {
		            LogImport.Warn(record.RowNumber.ToString(),
			            "Значение даты окончания действия значения льготной категории не является датой");
	            }
	            else
	            {
					record.DateTo = GetValueOrDefault(row, "DATLGTPO1").ToDateTime();
	            }
            }

            if (!privelegedCategoriesByCodeDict.ContainsKey(record.Ncode))
            {
                LogImport.Error(record.RowNumber.ToString(),
                    "Не найдена льготная категория с таким кодом в справочнике льготных категорий");
                return record;
            }

            record.PrivilegedCategory = privelegedCategoriesByCodeDict[record.Ncode];
	        if (record.DateFrom < record.PrivilegedCategory.DateFrom)
	        {
				LogImport.Error(record.RowNumber.ToString(),
					"В выбранные даты не заведена льготная категория в системе");
				return record;
	        }
			
            record.IsValidRecord = true;

            return record;
        }

        /// <summary>
        /// Прочитать данные из строки dbf
        /// связанные с кодом адреса помещения
        /// </summary>
        /// <param name="record">
        /// Запись
        /// </param>
        /// <param name="row">
        /// Строка dbf-файла
        /// </param>
        private bool CheckAddressCodeInfo(RecordVersion2 record, DbfRecord row)
        {
            if (!GetValueOrDefault(row, "KOD_POST").IsEmpty())
            {
                if (GetValueOrDefault(row, "KOD_POST").Length <= 3)
                {
                    record.CodePost = GetValueOrDefault(row, "KOD_POST");
                }
                else
                {
                    LogImport.Error(record.RowNumber.ToString(), "Длина кода МР не может быть больше 3 символов");
                    return false;
                }
            }
            else
            {
                LogImport.Error(record.RowNumber.ToString(), "Не указан код МР");
                return false;
            }

            if (!GetValueOrDefault(row, "KOD_NNASP").IsEmpty())
            {
                if (GetValueOrDefault(row, "KOD_NNASP").Length <= 5)
                {
                    record.CodeNnasp = GetValueOrDefault(row, "KOD_NNASP");
                }
                else
                {
                    LogImport.Error(record.RowNumber.ToString(), "Длина кода нас. пункта не может быть больше 5 символов");
                    return false;
                }
            }
            else
            {
                LogImport.Error(record.RowNumber.ToString(), "Не указан код нас. пункта");
                return false;
            }


            var nylic = GetValueOrDefault(row, "KOD_NYLIC");
            if (!nylic.IsEmpty())
            {
                if (nylic.Length <= 5)
                {
                    record.CodeNylic = nylic;
                }
                else
                {
                    LogImport.Error(record.RowNumber.ToString(), "Длина кода улицы не может быть больше 5 символов");
                    return false;
                }
            }
            else
            {
                LogImport.Warn(record.RowNumber.ToString(), "Не указан код улицы");
                return false;
            }

            var ndom = GetValueOrDefault(row, "NDOM");
            if (!ndom.IsEmpty())
            {
                if (ndom.Length <= 4)
                {
                    record.Ndom = ndom;
                }
                else
                {
                    LogImport.Error(record.RowNumber.ToString(), "Длина номера дома не может быть больше 4 символов");
                    return false;
                }
            }
            else
            {
                LogImport.Warn(record.RowNumber.ToString(), "Не указан номер дома");
                return false;
            }

            var nkorp = GetValueOrDefault(row, "NKORP");
            if (nkorp.Length <= 3)
            {
                record.Nkorp = nkorp;
            }
            else
            {
                LogImport.Error(record.RowNumber.ToString(), "Длина номера корпуса не может быть больше 3 символов");
                return false;
            }

            var nkw = GetValueOrDefault(row, "NKW");
            if (nkw.Length <= 4)
            {
                record.Nkw = GetValueOrDefault(row, "NKW");
            }
            else
            {
                LogImport.Error(record.RowNumber.ToString(), "Длина номера квартиры не может быть больше 4 символов");
                return false;
            }

            var nkomn = GetValueOrDefault(row, "NKOMN");
            if (nkomn.Length <= 2)
            {
                record.Nkomn = nkomn;
            }
            else
            {
                LogImport.Error(record.RowNumber.ToString(), "Длина номера квартиры не может быть больше 2 символов");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Прочитать данные из строки dbf
        /// связанные с кодом адреса помещения
        /// </summary>
        /// <param name="existedRecords">
        /// Существующие записи
        /// </param>
        /// <param name="record">
        /// Запись
        /// </param>
        /// <returns>
        /// The <see cref="ImportResult"/>.
        /// </returns>
        private RecordVersion2 GetCategoryForSamePeriod(List<RecordVersion2> existedRecords, RecordVersion2 record)
        {
            return existedRecords
                .FirstOrDefault(x => (!x.DateTo.HasValue && (record.DateFrom >= x.DateFrom || (record.DateTo.HasValue && record.DateTo >= x.DateFrom)))
                          || (x.DateTo.HasValue
                              && ((record.DateFrom <= x.DateFrom && record.DateFrom <= x.DateTo)
                                  || (record.DateTo <= x.DateFrom && record.DateTo >= x.DateTo)
                                  || (record.DateTo >= x.DateFrom && record.DateTo <= x.DateTo)
                                  || (record.DateTo >= x.DateFrom && record.DateTo >= x.DateTo)))
                          || (!record.DateTo.HasValue && x.DateFrom >= record.DateFrom));
        }
    }
}