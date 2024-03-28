namespace Bars.Gkh.RegOperator.Imports.OwnerRoom
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Import;
    using Bars.Gkh.Import.Impl;
    using Bars.Gkh.RegOperator.Domain.Repository;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.Gkh.Utils;

    using NHibernate.Linq;

    using ExecutionContext = Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    /// <summary>
    /// Импорт абонентов
    /// </summary>
    public partial class OwnerRoomImport : GkhImportBase
    {
        /// <summary>
        /// Id
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        private readonly Dictionary<int, Dictionary<bool, List<string>>> logDict = new Dictionary<int, Dictionary<bool, List<string>>>();
        private Dictionary<string, int> headersDict;
        private readonly List<RoomRecord> records = new List<RoomRecord>();

        private List<IndividualAccountOwner> individAccountsOwnersForUpdate;
        private List<IndividualAccountOwner> individAccountsOwnersForSave;
        private List<LegalAccountOwner> legalAccountsOwnersForSave;
        private List<LegalAccountOwner> legalAccountsOwnersForUpdate;
        private List<BasePersonalAccount> personalAccountsForUpdate;
        private List<Room> roomsForUpdate;

        private Dictionary<string, Contragent[]> existContragentDict;
        private Dictionary<long, LegalAccountOwner[]> existLegalOwnerDict;
        private Dictionary<string, IndividualAccountOwner[]> existIndividOwnerDict;
        private Dictionary<long, decimal> roomAreaSharesDict;

        private Dictionary<string, AccountProxy> accountDict;
        private Dictionary<string, RoomProxy> roomDict;
        private Dictionary<string, IndividOwnerProxy> individOwnerDict;
        private Dictionary<string, LegalOwnerProxy> legalOwnerDict;

        private BasePersonalAccount[] accounts;
        private Room[] rooms;
        private IndividualAccountOwner[] individOwners;
        private LegalAccountOwner[] legalOwners;

        private List<EntityLogLight> entityLogsToSave;
        private List<PersonalAccountChange> accountChangesToSave;
        private List<AccountOwnershipHistory> ownershipHistoryToSave;

        private string login;

        /// <summary>
        /// Ключ импорта
        /// </summary>
        public override string Key => OwnerRoomImport.Id;

        /// <summary>
        /// Код группировки импорта (например группировка в меню)
        /// </summary>
        public override string CodeImport => "OwnerRoomImport";

        /// <summary>
        /// Наименование импорта
        /// </summary>
        public override string Name => "Импорт абонентов (замена данных)";

        /// <summary>
        /// Разрешенные расширения файлов
        /// </summary>
        public override string PossibleFileExtensions => "xls,xlsx";

        /// <summary>
        /// Права
        /// </summary>
        public override string PermissionName => "Import.OwnerRoomImport";

        /// <summary>
        /// Менеджер пользователей
        /// </summary>
        public IGkhUserManager UserManager { get; set; }

        /// <summary>
        /// Менеджер лога
        /// </summary>
        public ILogImportManager LogImportManager { get; set; }

        /// <summary>
        /// Лог
        /// </summary>
        public new ILogImport LogImport { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="BasePersonalAccount" />
        /// </summary>
        public IDomainService<BasePersonalAccount> PersonalAccountDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="PersonalAccountOwner" />
        /// </summary>
        public IDomainService<PersonalAccountOwner> AccountOwnerDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="IndividualAccountOwner" />
        /// </summary>
        public IDomainService<IndividualAccountOwner> IndividualAccountOwnerDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="LegalAccountOwner" />
        /// </summary>
        public IDomainService<LegalAccountOwner> LegalAccountOwnerDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="Room" />
        /// </summary>
        public IDomainService<Room> RoomDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="Contragent" />
        /// </summary>
        public IDomainService<Contragent> ContragentDomain { get; set; }

        /// <summary>
        /// Периоды расчета 
        /// </summary>
        public IChargePeriodRepository PeriodRepository { get; set; }

        /// <summary>
        /// Сервис массовой работы с DTO KC
        /// </summary>
        public IMassPersonalAccountDtoService AccountDtoService { get; set; }

        /// <summary>
        /// Валидация
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <param name="message">Сообщение</param>
        /// <returns>Результат</returns>
        public override bool Validate(BaseParams baseParams, out string message)
        {
            message = null;

            if (!baseParams.Files.ContainsKey("FileImport"))
            {
                message = "Не выбран файл для импорта";
                return false;
            }

            var extention = baseParams.Files["FileImport"].Extention;

            var fileExtentions = this.PossibleFileExtensions.Contains(",") ? this.PossibleFileExtensions.Split(',') : new[] {this.PossibleFileExtensions};
            if (fileExtentions.All(x => x != extention))
            {
                message = $"Необходимо выбрать файл с допустимым расширением: {this.PossibleFileExtensions}";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Импорт
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <param name="ctx">Контекст</param>
        /// <param name="indicator">Индикатор прогресса</param>
        /// <param name="ct">Уведомление об отмене</param>
        /// <returns>Результат</returns>
        protected override ImportResult Import(BaseParams baseParams, ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            var file = baseParams.Files["FileImport"];
            var fileExtention = file.Extention;

            this.InitLog(file.FileName);

            indicator.Report(null, 0, "Чтение из файла");

            this.ProcessData(file.Data, fileExtention);

            indicator.Report(null, 10, "Подготовка данных");

            this.PrepareData(indicator);

            indicator.Report(null, 80, "Сохранение данных");

            this.SaveChanges();
            this.AccountDtoService.ApplyChanges();

            indicator.Report(null, 90, "Запись логов");

            this.WriteLogs();

            this.LogImportManager.Add(file, this.LogImport);
            this.LogImportManager.Save();

            return new ImportResult();
        }

        /// <summary>
        /// Инициализация лога
        /// </summary>
        /// <param name="fileName">Название файла</param>
        public new void InitLog(string fileName)
        {
            this.login = this.UserManager.GetActiveUser()?.Login ?? "anonymous";

            this.LogImportManager.FileNameWithoutExtention = fileName;
            this.LogImportManager.UploadDate = DateTime.Now;

            this.LogImport.SetFileName(fileName);
            this.LogImport.ImportKey = this.Key;
        }

        private void PrepareData(IProgressIndicator indicator)
        {
            this.individAccountsOwnersForUpdate = new List<IndividualAccountOwner>();
            this.individAccountsOwnersForSave = new List<IndividualAccountOwner>();
            this.legalAccountsOwnersForUpdate = new List<LegalAccountOwner>();
            this.legalAccountsOwnersForSave = new List<LegalAccountOwner>();
            this.personalAccountsForUpdate = new List<BasePersonalAccount>();
            this.roomsForUpdate = new List<Room>();
            this.entityLogsToSave = new List<EntityLogLight>();
            this.accountChangesToSave = new List<PersonalAccountChange>();
            this.ownershipHistoryToSave = new List<AccountOwnershipHistory>();

            this.AccountDtoService.InitCache();

            this.accounts = this.PersonalAccountDomain.GetAll()
                .WhereContains(x => x.PersonalAccountNum, this.records.Select(x => x.AccountNumber))
                .ToArray();

            this.rooms = this.RoomDomain.GetAll()
                .WhereContains(x => x.Id, this.accounts.Select(x => x.Room.Id))
                .ToArray();

            this.roomAreaSharesDict = this.PersonalAccountDomain.GetAll()
                .WhereContains(x => x.Room.Id, this.rooms.Select(x => x.Id))
                .Select(
                    x => new
                    {
                        x.Room.Id,
                        x.AreaShare
                    })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.Sum(y => y.AreaShare));

            indicator.Report(null, 15, "Подготовка данных");

            this.individOwners = this.IndividualAccountOwnerDomain.GetAll()
                .WhereContains(x => x.Id, this.accounts.Select(x => x.AccountOwner.Id))
                .ToArray();

            this.legalOwners = this.LegalAccountOwnerDomain.GetAll()
                .WhereContains(x => x.Id, this.accounts.Select(x => x.AccountOwner.Id))
                .Fetch(x => x.Contragent)
                .ToArray();

            this.existContragentDict = this.ContragentDomain.GetAll()
                .Where(x => x.ContragentState == ContragentState.Active)
                .AsEnumerable()
                .GroupBy(x => $"{x.Inn?.Trim()}#{x.Kpp?.Trim()}")
                .ToDictionary(x => x.Key, x => x.ToArray());

            this.existLegalOwnerDict = this.LegalAccountOwnerDomain.GetAll()
                .Where(x => x.Contragent.ContragentState == ContragentState.Active)
                .AsEnumerable()
                .GroupBy(x => x.Contragent.Id)
                .ToDictionary(x => x.Key, x => x.ToArray());

            this.existIndividOwnerDict = this.IndividualAccountOwnerDomain.GetAll()
                .AsEnumerable()
                .GroupBy(x => $"{x.Surname?.Trim().ToLower()}#{x.FirstName?.Trim().ToLower()}#{x.SecondName?.Trim().ToLower()}")
                .ToDictionary(x => x.Key, x => x.ToArray());

            indicator.Report(null, 20, "Подготовка данных");

            this.accountDict = this.records
                .ToDictionary(
                    x => x.AccountNumber,
                    x => new AccountProxy
                    {
                        RowNumber = x.RowNumber,
                        AreaShare = x.AreaShare,
                        AreaShareDate = x.AreaShareDate,
                        PersAccNumExternalSystems = x.PersAccNumExternalSystems,
                        PersAccNumExternalSystemsDate = x.PersAccNumExternalSystemsDate,
                        AccountCreateDate = x.AccountCreateDate,
                        AccountCreateDateStart = x.AccountCreateDateStart
                    });

            this.roomDict = this.records
                .ToDictionary(
                    x => x.AccountNumber,
                    x => new RoomProxy
                    {
                        RowNumber = x.RowNumber,
                        Area = x.Area,
                        AreaDate = x.AreaDate,
                        LivingArea = x.LivingArea,
                        OwnershipType = x.OwnershipType,
                        OwnershipTypeDate = x.OwnershipTypeDate,
                        RoomType = x.RoomType
                    });

            indicator.Report(null, 25, "Подготовка данных");

            this.individOwnerDict = this.records
                .Where(x => x.OwnerType == PersonalAccountOwnerType.Individual)
                .ToDictionary(
                    x => x.AccountNumber,
                    x =>
                        new IndividOwnerProxy
                        {
                            Key = $"{x.OwnerPhysSurname.ToLower()}#{x.OwnerPhysFirstName.ToLower()}#{x.OwnerPhysSecondName.ToLower()}",
                            RowNumber = x.RowNumber,
                            OwnerPhysFirstName = x.OwnerPhysFirstName,
                            OwnerPhysSurname = x.OwnerPhysSurname,
                            OwnerPhysSecondName = x.OwnerPhysSecondName,
                            BirthDate = x.BirthDate
                        });

            this.legalOwnerDict = this.records
                .Where(x => x.OwnerType == PersonalAccountOwnerType.Legal)
                .ToDictionary(
                    x => x.AccountNumber,
                    x =>
                        new LegalOwnerProxy
                        {
                            RowNumber = x.RowNumber,
                            OwnerJurName = x.OwnerJurName,
                            OwnerJurInn = x.OwnerJurInn,
                            OwnerJurKpp = x.OwnerJurKpp
                        });

            uint start = 30;
            uint result = 50;
            uint index = 1;

            indicator.Report(null, start, "Обработка данных");

            var notFoundAccounts = this.records.Where(x => this.accounts.All(y => y.PersonalAccountNum != x.AccountNumber));

            foreach (var record in notFoundAccounts)
            {
                this.AddLog(record.RowNumber, $"ЛС №{record.AccountNumber} не найден", false);
            }

            foreach (var account in this.accounts)
            {
                this.AccountDtoService.AddPersonalAccount(account);

                this.UpdateAccount(account);

                this.UpdateRoom(account);

                if (this.individOwnerDict.ContainsKey(account.PersonalAccountNum))
                {
                    this.ProcessIndividOwner(account);
                }
                else
                {
                    this.ProcessLegalOwner(account);
                }

                var progress = start + result / this.accounts.Length * index;

                indicator.Report(null, (uint) progress, "Обработка данных");

                index++;
            }
        }

        private void SaveChanges()
        {
            TransactionHelper.InsertInManyTransactions(this.Container, this.individAccountsOwnersForSave, useStatelessSession: true);
            TransactionHelper.InsertInManyTransactions(this.Container, this.legalAccountsOwnersForSave, useStatelessSession: true);

            TransactionHelper.InsertInManyTransactions(this.Container, this.personalAccountsForUpdate, useStatelessSession: true);
            TransactionHelper.InsertInManyTransactions(this.Container, this.roomsForUpdate, useStatelessSession: true);
            TransactionHelper.InsertInManyTransactions(this.Container, this.individAccountsOwnersForUpdate, useStatelessSession: true);
            TransactionHelper.InsertInManyTransactions(this.Container, this.legalAccountsOwnersForUpdate, useStatelessSession: true);

            TransactionHelper.InsertInManyTransactions(this.Container, this.entityLogsToSave, useStatelessSession: true);

            this.accountChangesToSave.ForEach(x => x.NewValue = x.PersonalAccount.AccountOwner.Id.ToString());
            TransactionHelper.InsertInManyTransactions(this.Container, this.accountChangesToSave, useStatelessSession: true);

            this.ownershipHistoryToSave.ForEach(x => x.AccountOwner = x.PersonalAccount.AccountOwner);
            TransactionHelper.InsertInManyTransactions(this.Container, this.ownershipHistoryToSave, useStatelessSession: true);
        }

        private void ProcessLegalOwner(BasePersonalAccount account)
        {
            var importedLegalOwner = this.legalOwnerDict[account.PersonalAccountNum];

            if (string.IsNullOrEmpty(importedLegalOwner.OwnerJurInn) || string.IsNullOrEmpty(importedLegalOwner.OwnerJurKpp))
            {
                this.AddLog(importedLegalOwner.RowNumber, "Не задан ИНН или КПП.");
                return;
            }

            var legalOwner = this.legalOwners.FirstOrDefault(x => x.Id == account.AccountOwner.Id);

            if (legalOwner.IsNull() || (importedLegalOwner.OwnerJurInn != legalOwner.Inn || importedLegalOwner.OwnerJurKpp != legalOwner.Kpp))
            {
                this.ChangeToLegalOwner(account, importedLegalOwner);
            }
        }

        private void ChangeToLegalOwner(BasePersonalAccount account, LegalOwnerProxy importedLegalOwner)
        {
            // Сие количество проверок связано с тем, что есть контрагенты с одинаковыми ИНН и КПП (хотя такого быть не должно, но увы),
            // также есть контрагенты, к которым привязаны несколько абонентов (чего тоже не должно быть, но опять же увы).

            var contragentKey = $"{importedLegalOwner.OwnerJurInn}#{importedLegalOwner.OwnerJurKpp}";

            if (this.existContragentDict.ContainsKey(contragentKey))
            {
                if (this.existContragentDict[contragentKey].Length > 1)
                {
                    this.AddLog(importedLegalOwner.RowNumber, "Найдено несколько контрагентов с подходящим ИНН и КПП", false);
                }
                else
                {
                    var contragent = this.existContragentDict[contragentKey].First();

                    if (this.existLegalOwnerDict.ContainsKey(contragent.Id) && this.existLegalOwnerDict[contragent.Id].Length == 1)
                    {
                        var existLegalOwner = this.existLegalOwnerDict[contragent.Id].First();

                        this.SetLegalOwner(account, existLegalOwner);

                        existLegalOwner.TotalAccountsCount++;
                        this.legalAccountsOwnersForUpdate.Add(existLegalOwner);
                    }
                    else
                    {
                        this.SetNewLegalOwner(account, contragent);
                    }
                }
            }
            else
            {
                this.AddLog(importedLegalOwner.RowNumber, "Контрагент с подходящим ИНН и КПП не найден", false);
            }
        }

        private void SetNewLegalOwner(BasePersonalAccount account, Contragent contragent)
        {
            var newLegalOwner = new LegalAccountOwner
            {
                OwnerType = PersonalAccountOwnerType.Legal,
                Contragent = contragent
            };

            this.SetLegalOwner(account, newLegalOwner);

            newLegalOwner.TotalAccountsCount++;
            this.legalAccountsOwnersForSave.Add(newLegalOwner);
        }

        private void SetLegalOwner(BasePersonalAccount account, LegalAccountOwner legalOwner)
        {
            var accountChange = new PersonalAccountChange(
                account,
                $"Смена абонента ЛС с \"{account.AccountOwner.Name}\" на \"{legalOwner.Name}\"",
                PersonalAccountChangeType.OwnerChange,
                DateTime.UtcNow,
                DateTime.UtcNow,
                this.login,
                null,
                account.AccountOwner.Id.ToString(),
                this.PeriodRepository.GetCurrentPeriod())
                {
                    Reason = this.Name
                };

            this.accountChangesToSave.Add(accountChange);

            this.ownershipHistoryToSave.Add(new AccountOwnershipHistory(account, null, DateTime.UtcNow));

            account.AccountOwner = legalOwner;
            this.personalAccountsForUpdate.Add(account);
        }

        private void ProcessIndividOwner(BasePersonalAccount account)
        {
            var importedIndividOwner = this.individOwnerDict[account.PersonalAccountNum];

            if (string.IsNullOrWhiteSpace(importedIndividOwner.OwnerPhysSurname) || string.IsNullOrWhiteSpace(importedIndividOwner.OwnerPhysFirstName))
            {
                this.AddLog(importedIndividOwner.RowNumber, "Не задан собственник-физ.лицо");
                return;
            }

            var individOwner = this.individOwners.FirstOrDefault(x => x.Id == account.AccountOwner.Id);

            if (individOwner.IsNull() || individOwner.IsNotNull() && this.NeedToChange(individOwner, importedIndividOwner))
            {
                this.ChangeToIndividOwner(account, importedIndividOwner);
            }
        }

        private void ChangeToIndividOwner(BasePersonalAccount account, IndividOwnerProxy importedIndividOwner)
        {
            if (this.existIndividOwnerDict.ContainsKey(importedIndividOwner.Key))
            {
                this.SetExistIndividOwner(account, importedIndividOwner);
            }
            else
            {
                this.SetNewIndividOwner(account, importedIndividOwner);
            }
        }

        private void SetExistIndividOwner(BasePersonalAccount account, IndividOwnerProxy importedIndividOwner)
        {
            var existIndividOwners = this.existIndividOwnerDict[importedIndividOwner.Key]
                .WhereIf(importedIndividOwner.BirthDate.HasValue, x => x.BirthDate == importedIndividOwner.BirthDate)
                .ToArray();

            if (existIndividOwners.Length == 1)
            {
                var existIndividOwner = existIndividOwners.First();

                this.SetIndividOwner(account, existIndividOwner);

                existIndividOwner.TotalAccountsCount++;
                this.individAccountsOwnersForUpdate.Add(existIndividOwner);
            }
            else
            {
                this.SetNewIndividOwner(account, importedIndividOwner);
            }
        }

        private void SetNewIndividOwner(BasePersonalAccount account, IndividOwnerProxy importedIndividOwner)
        {
            var newIndividOwner = new IndividualAccountOwner
            {
                OwnerType = PersonalAccountOwnerType.Individual,
                FirstName = importedIndividOwner.OwnerPhysFirstName,
                SecondName = importedIndividOwner.OwnerPhysSecondName,
                Surname = importedIndividOwner.OwnerPhysSurname,
            };

            if (importedIndividOwner.BirthDate.HasValue)
            {
                newIndividOwner.BirthDate = importedIndividOwner.BirthDate.Value;
            }

            this.SetIndividOwner(account, newIndividOwner);

            newIndividOwner.TotalAccountsCount++;
            this.individAccountsOwnersForSave.Add(newIndividOwner);

            this.AddLog(
                importedIndividOwner.RowNumber,
                string.Format(
                    "Создано новое физ. лицо {0} {1} {2}",
                    importedIndividOwner.OwnerPhysSurname,
                    importedIndividOwner.OwnerPhysFirstName,
                    importedIndividOwner.OwnerPhysSecondName));
        }

        private void SetIndividOwner(BasePersonalAccount account, IndividualAccountOwner individOwner)
        {
            var accountChange = new PersonalAccountChange(
                account,
                string.Format(
                    "Смена абонента ЛС с {0} на {1} {2} {3}",
                    account.AccountOwner.Name,
                    individOwner.Surname,
                    individOwner.FirstName,
                    individOwner.SecondName),
                PersonalAccountChangeType.OwnerChange,
                DateTime.UtcNow,
                DateTime.UtcNow,
                this.login,
                null,
                account.AccountOwner.Id.ToString(),
                this.PeriodRepository.GetCurrentPeriod())
                {
                    Reason = this.Name
                };

            this.accountChangesToSave.Add(accountChange);

            this.ownershipHistoryToSave.Add(new AccountOwnershipHistory(account, null, DateTime.UtcNow));

            account.AccountOwner = individOwner;

            this.personalAccountsForUpdate.Add(account);
        }

        private void UpdateRoom(BasePersonalAccount account)
        {
            var hasChanges = false;

            var importedRoom = this.roomDict[account.PersonalAccountNum];

            var room = this.rooms.FirstOrDefault(x => x.Id == account.Room.Id);

            if (room.IsNotNull())
            {
                if (importedRoom.Area.HasValue && room.Area != importedRoom.Area)
                {
                    if (importedRoom.AreaDate.HasValue)
                    {
                        room.Area = importedRoom.Area.Value;
                        hasChanges = true;

                        var entityLog = new EntityLogLight
                        {
                            ClassName = "Room",
                            EntityId = room.Id,
                            PropertyName = "Area",
                            PropertyValue = importedRoom.Area.Value.ToString(),
                            DateActualChange = importedRoom.AreaDate.Value,
                            DateApplied = DateTime.UtcNow,
                            ParameterName = "room_area",
                            User = this.login,
                            Reason = this.Name
                        };

                        this.entityLogsToSave.Add(entityLog);
                    }
                    else
                    {
                        this.AddLog(importedRoom.RowNumber, "Невозможно поменять Площадь помещения: не указана Дата начала действия значения", false);
                    }
                }

                if (importedRoom.OwnershipType.HasValue)
                {
                    if (room.OwnershipType != importedRoom.OwnershipType)
                    {
                        if (importedRoom.OwnershipTypeDate.HasValue)
                        {
                            room.OwnershipType = importedRoom.OwnershipType.Value;
                            hasChanges = true;

                            var entityLog = new EntityLogLight
                            {
                                ClassName = "Room",
                                EntityId = room.Id,
                                PropertyName = "OwnershipType",
                                PropertyValue = importedRoom.OwnershipType.ToString(),
                                DateActualChange = importedRoom.OwnershipTypeDate.Value,
                                DateApplied = DateTime.UtcNow,
                                ParameterName = "room_ownership_type",
                                User = this.login,
                                Reason = this.Name
                            };

                            this.entityLogsToSave.Add(entityLog);
                        }
                        else
                        {
                            this.AddLog(
                                importedRoom.RowNumber,
                                "Невозможно поменять Тип собственности помещения: не указана Дата начала действия значения",
                                false);
                        }
                    }
                }

                if (importedRoom.RoomType.HasValue)
                {
                    if (room.Type != importedRoom.RoomType)
                    {
                        room.Type = importedRoom.RoomType.Value;
                        hasChanges = true;
                    }
                }

                if (importedRoom.LivingArea.HasValue)
                {
                    if (room.LivingArea != importedRoom.LivingArea)
                    {
                        room.LivingArea = importedRoom.LivingArea;
                        hasChanges = true;
                    }
                }

                if (hasChanges)
                {
                    this.roomsForUpdate.Add(room);
                }
            }
        }

        private void UpdateAccount(BasePersonalAccount account)
        {
            var hasChanges = false;

            var importedAccount = this.accountDict[account.PersonalAccountNum];

            if (importedAccount.AreaShare.HasValue && account.AreaShare != importedAccount.AreaShare.Value)
            {
                if (importedAccount.AreaShareDate.HasValue)
                {
                    var areaShareDelta = importedAccount.AreaShare.Value - account.AreaShare;

                    if (this.roomAreaSharesDict[account.Room.Id] + areaShareDelta > 1)
                    {
                        this.AddLog(importedAccount.RowNumber, "Невозможно поменять Долю собственности: Доля собственности в сумме превышает 1", false);
                    }
                    else
                    {
                        account.AreaShare = importedAccount.AreaShare.Value;
                        hasChanges = true;

                        var entityLog = new EntityLogLight
                        {
                            ClassName = "BasePersonalAccount",
                            EntityId = account.Id,
                            PropertyName = "AreaShare",
                            PropertyValue = importedAccount.AreaShare.Value.ToString(),
                            DateActualChange = importedAccount.AreaShareDate.Value,
                            DateApplied = DateTime.UtcNow,
                            ParameterName = "area_share",
                            User = this.login,
                            Reason = this.Name
                        };

                        this.entityLogsToSave.Add(entityLog);

                        //меняем общую долю собственности для этого помещения на эту разницу
                        this.roomAreaSharesDict[account.Room.Id] += areaShareDelta;
                    }
                }
                else
                {
                    this.AddLog(importedAccount.RowNumber, "Невозможно поменять Долю собственности: не указана Дата начала действия значения", false);
                }
            }

            if (importedAccount.AccountCreateDate.HasValue && account.OpenDate != importedAccount.AccountCreateDate.Value)
            {
                if (importedAccount.AccountCreateDateStart.HasValue)
                {
                    account.OpenDate = importedAccount.AccountCreateDate.Value;
                    hasChanges = true;

                    var entityLog = new EntityLogLight
                    {
                        ClassName = "BasePersonalAccount",
                        EntityId = account.Id,
                        PropertyName = "OpenDate",
                        PropertyValue = importedAccount.AccountCreateDate.Value.ToString(),
                        DateActualChange = importedAccount.AccountCreateDateStart.Value,
                        DateApplied = DateTime.UtcNow,
                        ParameterName = "account_open_date",
                        User = this.login,
                        Reason = this.Name
                    };

                    this.entityLogsToSave.Add(entityLog);
                }
                else
                {
                    this.AddLog(importedAccount.RowNumber, "Невозможно поменять Дату открытия ЛС: не указана Дата начала действия значения", false);
                }
            }

            if (importedAccount.PersAccNumExternalSystems.IsNotEmpty() && account.PersAccNumExternalSystems != importedAccount.PersAccNumExternalSystems)
            {
                if (importedAccount.PersAccNumExternalSystemsDate.HasValue)
                {
                    account.PersAccNumExternalSystems = importedAccount.PersAccNumExternalSystems;
                    hasChanges = true;

                    var entityLog = new EntityLogLight
                    {
                        ClassName = "BasePersonalAccount",
                        EntityId = account.Id,
                        PropertyName = "PersAccNumExternalSystems",
                        PropertyValue = importedAccount.PersAccNumExternalSystems,
                        DateActualChange = importedAccount.PersAccNumExternalSystemsDate.Value,
                        DateApplied = DateTime.UtcNow,
                        ParameterName = "account_external_num",
                        User = this.login,
                        Reason = this.Name
                    };

                    this.entityLogsToSave.Add(entityLog);
                }
                else
                {
                    this.AddLog(importedAccount.RowNumber, "Невозможно поменять Внешний номер ЛС: не указана Дата начала действия значения", false);
                }
            }

            if (hasChanges)
            {
                this.personalAccountsForUpdate.Add(account);
            }
        }

        private void AddLog(int rowNum, string message, bool success = true)
        {
            if (!this.logDict.ContainsKey(rowNum))
            {
                this.logDict[rowNum] = new Dictionary<bool, List<string>>();
            }

            var log = this.logDict[rowNum];

            if (!log.ContainsKey(success))
            {
                log[success] = new List<string>();
            }

            log[success].Add(message);
        }

        private void WriteLogs()
        {
            foreach (var log in this.logDict.OrderBy(x => x.Key))
            {
                var rowNumber = $"Строка {log.Key}";

                foreach (var rowLog in log.Value)
                {
                    if (rowLog.Key)
                    {
                        this.LogImport.Info(rowNumber, rowLog.Value.AggregateWithSeparator("."));
                    }
                    else
                    {
                        this.LogImport.Warn(rowNumber, rowLog.Value.AggregateWithSeparator("."));
                    }
                }
            }

            this.LogImport.CountChangedRows = this.individAccountsOwnersForUpdate.Count
                + this.legalAccountsOwnersForUpdate.Count
                + this.personalAccountsForUpdate.Count
                + this.roomsForUpdate.Count;

            this.LogImport.CountAddedRows = this.individAccountsOwnersForSave.Count + this.legalAccountsOwnersForSave.Count;
        }

        private bool NeedToChange(IndividualAccountOwner individOwner, IndividOwnerProxy importedIndividOwner)
        {
            return individOwner.FirstName.ToLower() != importedIndividOwner.OwnerPhysFirstName.ToLower()
                || individOwner.SecondName.ToLower() != importedIndividOwner.OwnerPhysSecondName.ToLower()
                || individOwner.Surname.ToLower() != importedIndividOwner.OwnerPhysSurname.ToLower()
                || importedIndividOwner.BirthDate.HasValue && individOwner.BirthDate != importedIndividOwner.BirthDate;
        }

        private class AccountProxy
        {
            public int RowNumber { get; set; }

            public decimal? AreaShare { get; set; }

            public DateTime? AreaShareDate { get; set; }

            public string PersAccNumExternalSystems { get; set; }

            public DateTime? PersAccNumExternalSystemsDate { get; set; }

            public DateTime? AccountCreateDate { get; set; }

            public DateTime? AccountCreateDateStart { get; set; }
        }

        private class RoomProxy
        {
            public int RowNumber { get; set; }

            public decimal? Area { get; set; }

            public DateTime? AreaDate { get; set; }

            public decimal? LivingArea { get; set; }

            public RoomOwnershipType? OwnershipType { get; set; }

            public DateTime? OwnershipTypeDate { get; set; }

            public RoomType? RoomType { get; set; }
        }

        private class IndividOwnerProxy
        {
            public string Key { get; set; }

            public int RowNumber { get; set; }

            public string OwnerPhysFirstName { get; set; }

            public string OwnerPhysSurname { get; set; }

            public string OwnerPhysSecondName { get; set; }

            public DateTime? BirthDate { get; set; }
        }

        private class LegalOwnerProxy
        {
            public int RowNumber { get; set; }

            public string OwnerJurName { get; set; }

            public string OwnerJurInn { get; set; }

            public string OwnerJurKpp { get; set; }
        }
    }
}