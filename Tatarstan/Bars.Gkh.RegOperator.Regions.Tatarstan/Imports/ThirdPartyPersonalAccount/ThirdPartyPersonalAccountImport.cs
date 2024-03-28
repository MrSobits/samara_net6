/*
 * общая схема такая:
 * 1) читаем данные из файла
 * 2) в зависимости от данных, которые есть в файле, формируем кэш
 * 3) получаем/создаем объекты (помещения, счета)
 * 4) агрегируем деньги (начисления, оплаты, перерасчет)
 * 5) применяем эти цифры для итогов по периодам, счета начислений и счета оплат
 * 6) сохраняем
 */

namespace Bars.Gkh.RegOperator.Regions.Tatarstan.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Domain.ParameterVersioning;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Import;
    using Bars.Gkh.Import.Impl;
    using Bars.Gkh.RegOperator.AccountNumberGenerator;
    using Bars.Gkh.RegOperator.Domain;
    using Bars.Gkh.RegOperator.Domain.ParametersVersioning;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Imports;
    using Bars.GkhExcel;

    using Castle.Windsor;

    using NHibernate;
    using NHibernate.Linq;

    using ExecutionContext = Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    /// <summary>
    /// Импорт лс из сторонних систем
    /// </summary>
    public partial class ThirdPartyPersonalAccountImport : GkhImportBase
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        protected HashSet<BasePersonalAccount> AccountsToSave = new HashSet<BasePersonalAccount>();
        protected HashSet<BasePersonalAccount> AccountsToGenerateNumber = new HashSet<BasePersonalAccount>();
        protected HashSet<Room> RoomsToSave = new HashSet<Room>();
        protected HashSet<IndividualAccountOwner> OwnersToSave = new HashSet<IndividualAccountOwner>();
        protected HashSet<PersonalAccountPeriodSummary> SummariesToSave = new HashSet<PersonalAccountPeriodSummary>();

        protected Regex TypeServiceRegex = new Regex(@"^.\d{1,2}$");

        protected Dictionary<string, Dictionary<long, List<MoneyAggregation>>> MoneyAggregationDict =
            new Dictionary<string, Dictionary<long, List<MoneyAggregation>>>();

        private List<long> periods = new List<long>();

        private Dictionary<string, State> accountStatuses = new Dictionary<string, State>();
        private readonly List<ChargePeriod> chargePeriods = new List<ChargePeriod>();

        private bool onlyOpenPeriod = true;
        private long userId;

        private bool replaceData;

        private CultureInfo culture;

        /// <summary>
        /// Сессия 
        /// </summary>
        private ISessionProvider sessionProvider;

        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Сервис получения номера Лс 
        /// </summary>
        public IAccountNumberGenerator AccountNumberGenerator { get; set; }

        /// <summary>
        /// Ключ импорта
        /// </summary>
        public override string Key
        {
            get { return ThirdPartyPersonalAccountImport.Id; }
        }

        /// <summary>
        /// Код группировки импорта (например группировка в меню)
        /// </summary>
        public override string CodeImport
        {
            get { return "PersonalAccountImport"; }
        }

        /// <summary>
        /// Наименование импорта
        /// </summary>
        public override string Name
        {
            get { return "Импорт лс из сторонних систем"; }
        }

        /// <summary>
        /// Разрешенные расширения файлов
        /// </summary>
        public override string PossibleFileExtensions
        {
            get { return "xls,xlsx"; }
        }

        /// <summary>
        /// Права
        /// </summary>
        public override string PermissionName
        {
            get { return "Import.PersonalAccountImport"; }
        }
        private const string RecruitmentCode = "15";
        private const string OverhaulPenaltyCode = "506";
        private const string RecruitmentPenaltyCode = "507";

        /// <summary>
        /// Сессия 
        /// </summary>
        public ISessionProvider SessionProvider
        {
            get { return this.sessionProvider ?? (this.sessionProvider = this.Container.Resolve<ISessionProvider>()); }
        }

        public ThirdPartyPersonalAccountImport(ILogImportManager logManager, ILogImport logImport)
        {
            this.LogImportManager = logManager;
            this.LogImport = logImport;
        }

        /// <summary>
        /// Инициализация лога
        /// </summary>
        private void InitLog(FileData file)
        {
            if (this.LogImportManager == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImportManager");
            }

            this.LogImportManager.FileNameWithoutExtention = file.FileName;
            this.LogImportManager.UploadDate = DateTime.Now;

            if (this.LogImport == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImport");
            }

            this.LogImport.SetFileName(file.FileName);
            this.LogImport.ImportKey = Key;

            this.LogImportManager.LogImportPreviewMode = true;
            this.LogImportManager.Add(file, this.LogImport);
        }

        /// <summary>
        /// Общий метод для импорта 
        /// </summary>
        /// <param name="params">Базовые параметры</param>
        /// <param name="ctx">Индикатор импорта</param>
        /// <param name="indicator"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        protected override ImportResult Import(BaseParams @params, ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            this.userId = ctx.UserId;
            return base.Import(@params, ctx, indicator, ct);
        }

        /// <summary>
        /// Импорт
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public override ImportResult Import(BaseParams baseParams)
        {
            var message = string.Empty;
            var file = baseParams.Files["FileImport"];
            var isChanged = false;

            try
            {
                this.culture = CultureInfo.CreateSpecificCulture("ru-RU");
                this.replaceData = baseParams.Params.GetAs<bool>("replaceData");

                var login = this.UserRepo.Get(this.Identity.UserId).Return(u => u.Login);

                if (login.IsEmpty())
                {
                    login = "anonymous";
                }

                this.InitLog(file);

                var session = this.SessionProvider.GetCurrentSession();
                session.FlushMode = FlushMode.Never;

                //1) читаем данные из файла
                var rows = this.ReadFile(file);

                // Проверка корректности даты загрузки
                var dateLoad = this.ParseStringPeriodToStartDate(new DataRecord(1) { Period = rows[1][17].Value });
                var cDate = DateTime.Now.Date;
                var conditionDate = cDate.Month == 1
                    ? dateLoad.Year >= cDate.Date.Year - 1 && dateLoad <= cDate
                    : dateLoad.Year == cDate.Year && dateLoad <= cDate;

                if (!conditionDate)
                {
                    throw new Exception("Импорт лицевых счетов из сторонних систем разрешен с указанием периода текущего года");
                }

                //проверка, может ли пользователь грузить данные в закрытый период
                var authorization = this.Container.ResolveAll<IAuthorizationService>().FirstOrDefault();
                if (authorization != null)
                {
                    this.onlyOpenPeriod = !authorization.Grant(
                        new ProxyUserIdentity(this.Identity.UserId),
                        "Import.ThirdPartyPersonalAccountImportClosed");
                }

                this.RobjectCache = this.RobjectRepo.GetAll()
                    .Fetch(x => x.Municipality)
                    .Fetch(x => x.MoSettlement)
                    .Select(
                        x => new
                        {
                            Street = x.FiasAddress.StreetName.ToLower().Trim(),
                            Place = x.Municipality.Group.Trim() == "" ? x.FiasAddress.PlaceName.ToLower().Trim() : x.Municipality.Group.ToLower().Trim(),
                            House = x.FiasAddress.House.ToLower().Trim(),
                            Housing = x.FiasAddress.Housing.ToLower().Trim(),
                            Letter = x.FiasAddress.Letter.ToLower().Trim(),
                            RealityObject = x
                        })
                    .AsEnumerable()

                    //трим делается из-за того, что в татарстане в FiasAddress в полях строение или литер хранится символ '-',
                    //но в строковом адресе в доме он не отображается
                    .GroupBy(x => $"{x.Place}#{x.Street}#{x.House}#{x.Housing.ToStr().Trim('-')}#{x.Letter}".Trim('-'))
                    .ToDictionary(x => x.Key, y => y.Select(x => x.RealityObject).First());

                var data = this.ReadData(rows);
                
                //2) в зависимости от данных, которые есть в файле, формируем кэш
                this.InitCache(data);

                //3) получаем/создаем объекты (помещения, счета)
                //4) агрегируем деньги (начисления, оплаты, перерасчет)
                foreach (var item in data)
                {
                    this.ProcessLine(item);
                }

                TransactionHelper.InsertInManyTransactions(this.Container, this.chargePeriods, useStatelessSession: true);
                session.FlushMode = FlushMode.Commit;
                this.CorrectPeriods();
                session.FlushMode = FlushMode.Never;
                this.CreatePartitions();

                //5) применяем эти цифры для итогов по периодам, счета начислений и счета оплат
                foreach (var aggrByAccount in this.MoneyAggregationDict)
                {
                    foreach (var aggrByPeriod in aggrByAccount.Value)
                    {
                        foreach (var aggr in aggrByPeriod.Value.GroupBy(x => x.OperationType))
                        {
                            switch (aggr.Key)
                            {
                                case PaymentOperationType.IncomeRent:
                                    //если тип IncomeRent, то каждая оплата уникальная
                                    foreach (var item in aggr)
                                    {
                                        this.ApplyToPersonalAccount(item);
                                    }
                                    break;
                                case PaymentOperationType.IncomeCr:
                                case PaymentOperationType.IncomePenalty:
                                    //если тип IncomeCr или IncomePenalty, то оплаты за один период с этим типом должны сесть как одна
                                    var first = aggr.First();
                                    var last = aggr.Last();
                                    //формируем одну общую оплату
                                    var incomeCrItem = new MoneyAggregation
                                    {
                                        Robject = first.Robject,
                                        Account = first.Account,
                                        Period = first.Period,
                                        Charge = aggr.Sum(x => x.Charge),
                                        Payment = aggr.Sum(x => x.Payment),
                                        Recalc = aggr.Sum(x => x.Recalc),
                                        OperationType = aggr.Key,
                                        RowNumber = first.RowNumber,
                                        SaldoIn = first.SaldoIn,
                                        SaldoOut = last.SaldoOut
                                    };
                                    this.ApplyToPersonalAccount(incomeCrItem);
                                    break;
                            }
                        }
                    }
                }

                var entityLogLightList = this.PrepareEntityLogToSave(login);

                //6) сохраняем

                TransactionHelper.InsertInManyTransactions(this.Container, this.AccountsToSave.Select(x => x.BaseTariffWallet), 1000, false, true);
                TransactionHelper.InsertInManyTransactions(this.Container, this.AccountsToSave.Select(x => x.DecisionTariffWallet), 1000, false, true);
                TransactionHelper.InsertInManyTransactions(this.Container, this.AccountsToSave.Select(x => x.RentWallet), 1000, false, true);
                TransactionHelper.InsertInManyTransactions(this.Container, this.AccountsToSave.Select(x => x.PenaltyWallet), 1000, false, true);
                TransactionHelper.InsertInManyTransactions(this.Container, this.AccountsToSave.Select(x => x.SocialSupportWallet), 1000, false, true);
                TransactionHelper.InsertInManyTransactions(this.Container, this.AccountsToSave.Select(x => x.PreviosWorkPaymentWallet), 1000, false, true);
                TransactionHelper.InsertInManyTransactions(this.Container, this.AccountsToSave.Select(x => x.AccumulatedFundWallet), 1000, false, true);
                TransactionHelper.InsertInManyTransactions(
                    this.Container,
                    this.AccountsToSave.Select(x => x.RestructAmicableAgreementWallet),
                    1000,
                    false,
                    true);

                this.AccountNumberGenerator.Generate(this.AccountsToGenerateNumber);
                TransactionHelper.InsertInManyTransactions(this.Container, this.OwnersToSave, 10000, true, true);
                TransactionHelper.InsertInManyTransactions(this.Container, this.RoomsToSave, 10000, true, true);
                TransactionHelper.InsertInManyTransactions(this.Container, this.AccountsToSave, 10000, true, true);
                isChanged = true;
                TransactionHelper.InsertInManyTransactions(this.Container, this.SummariesToSave, 10000, true, true);
                TransactionHelper.InsertInManyTransactions(this.Container, entityLogLightList, 10000, true, true);

                this.SaveChargeAccountOperations();
                this.AccountDtoService.ApplyChanges();
            }
            catch (Exception exception)
            {
                if (!isChanged)
                {
                    this.LogImport.CountAddedRows = 0;
                    this.LogImport.CountChangedRows = 0;
                }

                message = "Message: " + exception.Message + "\nStacktrace: " + exception.StackTrace;
                this.LogImport.Error("Ошибка", exception.Message);
            }

            var currSession = this.SessionProvider.GetCurrentSession();
            currSession.FlushMode = FlushMode.Commit;

            this.LogImportManager.Update(this.LogImport);
            this.LogImportManager.Save();

            return new ImportResult(this.LogImportManager.GetStatus(), message, "", this.LogImportManager.LogFileId);
        }

        /// <summary>
        /// Чтение файла
        /// </summary>
        /// <param name="file">Файл</param>
        /// <returns>Список строк файла</returns>
        protected List<GkhExcelCell[]> ReadFile(FileData file)
        {
            var excel = this.Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider");
            using (this.Container.Using(excel))
            {
                if (excel == null)
                {
                    throw new Exception("Не найдена реализация интерфейса IGkhExcelProvider");
                }

                if (file.Extention == "xlsx")
                {
                    excel.UseVersionXlsx();
                }

                using (var memoryStreamFile = new MemoryStream(file.Data))
                {
                    memoryStreamFile.Seek(0, SeekOrigin.Begin);

                    excel.Open(memoryStreamFile);

                   return excel.GetRows(0, 0).Where(x => x.Any(y => y.Value != string.Empty)).ToList();

                }
            }
        }

        /// <summary>
        /// Получение данных DataRecord
        /// </summary>
        /// <param name="rows">Записи из файла импорта</param>
        /// <returns></returns>
        protected List<DataRecord> ReadData(List<GkhExcelCell[]> rows)
        {
            var result = new List<DataRecord>();

            for (var i = 1; i < rows.Count; ++i)
            {
                result.Add(this.ExtractRowData(rows[i], i + 1));
            }

            return result;
        }

        /// <summary>
        /// Извлечение данных
        /// </summary>
        /// <param name="data">Данные</param>
        /// <param name="rowNumber">Номер</param>
        /// <returns></returns>
        protected DataRecord ExtractRowData(GkhExcelCell[] data, int rowNumber)
        {
            var record = new DataRecord(rowNumber)
            {
                AccountNumberExternalSystems = this.GetValue(data, 0),
                Tariff = this.GetValue(data, 1).ToDecimal(),
                AreaShare = this.GetValue(data, 2).ToDecimal(),
                Area = this.GetValue(data, 3).ToDecimal(),
                OpenDate = this.GetValue(data, 4).ToDateTime(),
                CloseDate = this.GetValue(data, 5).To<DateTime?>(),
                Municipality = this.GetValue(data, 6),
                TypeSettlement = this.GetValue(data, 7),
                Settlement = this.GetValue(data, 8).ToLower(),
                TypeStreet = this.GetValue(data, 9),
                Street = this.GetValue(data, 10).ToLower(),
                House = this.GetValue(data, 11).ToLower(),
                Letter = this.GetValue(data, 12).ToLower(),
                Housing = this.GetValue(data, 13).ToLower(),
                Building = this.GetValue(data, 14).ToLower(),
                RoomNum = this.GetValue(data, 15).ToLower(),
                AccountState = this.GetValue(data, 16),
                Period = this.GetValue(data, 17),
                SaldoIn = this.GetValue(data, 18).ToDecimal(),
                SaldoOut = this.GetValue(data, 19).ToDecimal(),
                TypeService = this.GetValue(data, 20),
                Charged = this.GetValue(data, 21).ToDecimal(),
                Paid = this.GetValue(data, 22).ToDecimal(),
                Recalc = this.GetValue(data, 23).ToDecimal()
            };

            record.Robject = this.GetRobject(record);

            if (record.Robject != null)
            {
                this.roIds.Add(record.Robject.Id);
            }

            return record;
        }

        /// <summary>
        /// Проверка записи
        /// </summary>
        /// <param name="record">Запись</param>
        protected void ProcessLine(DataRecord record)
        {
            var period = this.GetPeriod(record);

            if (!this.periods.Contains(period.Id))
            {
                // Загружаемый период оказался будущим
                if (period.StartDate.Date > DateTime.Now.Date)
                {
                    this.LogImport.Warn("Строка " + record.RowNumber,
                        "Лицевой счет и начисления по нему не загружены, " +
                        "так как указан некорректный период начисления.");
                    return;
                }

                // Загружаемый период отсутствует в БД
                if (!this.PeriodCache.ContainsKey(period.StartDate))
                {
                    // Список периодов для вставки в БД
                    this.chargePeriods.Add(period);

                    this.PeriodCache.Add(period.StartDate, period);
                }

                periods.Add(period.Id);
            }

            if (record.Robject == null)
            {
                this.LogImport.Warn("Строка " + record.RowNumber, "Не удалось получить жилой дом");
                return;
            }

            if (record.RoomNum.IsEmpty())
            {
                this.LogImport.Warn("Строка " + record.RowNumber, "Не указан номер помещения");
                return;
            }

            if (record.AreaShare <= 0)
            {
                this.LogImport.Warn("Строка " + record.RowNumber, "Не указана доля собственности");
            }

            if (record.OpenDate == DateTime.MinValue)
            {
                this.LogImport.Warn("Строка " + record.RowNumber, "Не указана дата открытия счета");
                return;
            }

            var room = this.GetOrCreateRoom(record.Robject, record, period);

            if (record.AccountNumberExternalSystems.IsEmpty())
            {
                this.LogImport.Warn("Строка " + record.RowNumber, "Не указан номер лицевого счета во внешних системах");
                return;
            }

            if (!string.IsNullOrWhiteSpace(record.AccountState)
                && !this.accountStatuses.ContainsKey(record.AccountState.ToLower()))
            {
                this.LogImport.Warn("Строка " + record.RowNumber, $"Лицевой счет по умолчанию загружен со статусом \"{PersonalAccountStartState.Name}\"." +
                    $" Значение поля \"Состояние счета\" может принимать строгое значение {string.Join("/", this.accountStatuses.Keys)}.");
            }

            var account = this.GetOrCreateAccount(room, record, period);

            if (account == null)
            {
                return;
            }

            if (account.Room.Id != room.Room.Id)
            {
                this.LogImport.Warn("Строка " + record.RowNumber, "Лицевой счет связан с другим помещением. Лицевой счет не загружен");
                return;
            }

            if (!this.AccountsToSave.Contains(account))
            {
                this.AccountDtoService.AddPersonalAccount(account);
                this.AccountsToSave.Add(account);
            }

            if (this.onlyOpenPeriod && period.IsClosed)
            {
                this.LogImport.Warn(
                    "Строка " + record.RowNumber,
                    "У пользователя нет прав на импорт в закрытый период. Информация по начислениям и оплатам не загружена");
                return;
            }

            this.AggregateMoney(record.Robject, account, period, record);
        }

        /// <summary>
        /// Получение жилого дома 
        /// </summary>
        /// <param name="record">Запись</param>
        /// <returns></returns>
        protected RealityObject GetRobject(DataRecord record)
        {
            var key = $"{record.TypeSettlement + ". " + record.Settlement}#{record.TypeStreet + ". " + record.Street}#{record.House}#{record.Housing}#{record.Letter}";

            if (this.RobjectCache.ContainsKey(key))
            {
                return this.RobjectCache[key];
            }

            return null;
        }

        /// <summary>
        /// Получение периода 
        /// </summary>
        /// <param name="record">Запись</param>
        /// <returns></returns>
        protected ChargePeriod GetPeriod(DataRecord record)
        {
            var periodStartDate = this.ParseStringPeriodToStartDate(record);

            return this.PeriodCache.ContainsKey(periodStartDate)
                ? this.PeriodCache.Get(periodStartDate)
                : new ChargePeriod(periodStartDate.ToString("yyyy MMM", this.culture),
                periodStartDate, periodStartDate.AddMonths(1).AddDays(-1));
        }

        /// <summary>
        /// Корректирование периодов
        /// </summary>
        private void CorrectPeriods()
        {
            var periods = this.ChargePeriod.GetAll().Where(x => !x.IsClosed).ToList();

            foreach (var period in periods)
            {
                if (!periods.Any(x => x.StartDate > period.StartDate))
                {
                    if (period.EndDate.HasValue)
                    {
                        period.EndDate = null;
                        this.ChargePeriod.Update(period);
                    }
                    continue;
                }

                if (!period.EndDate.HasValue)
                {
                    period.EndDate = period.GetEndDate();
                }

                period.IsClosed = true;
                this.ChargePeriod.Update(period);
            }
        }

        /// <summary>
        /// Создание необходимых партиций на основании периодов
        /// </summary>
        private void CreatePartitions()
        {
            var commonQueriesList = PersonalAccountImportHelper.GetSqlCommandsNeedToExecute(this.SessionProvider.GetCurrentSession());

            foreach (var sql in commonQueriesList)
            {
                this.SessionProvider.GetCurrentSession().CreateSQLQuery(sql).ExecuteUpdate();
            }
        }

        /// <summary>
        /// Получить Или создать помещения 
        /// </summary>
        /// <param name="robject">Жилой дом</param>
        /// <param name="record">Запись</param>
        /// <param name="period">Период</param>
        /// <returns></returns>
        protected RoomInfo GetOrCreateRoom(RealityObject robject, DataRecord record, ChargePeriod period)
        {
            if (!this.RoomCache.ContainsKey(robject.Id))
            {
                this.RoomCache[robject.Id] = new Dictionary<string, RoomInfo>();
            }

            //не используется метод Get из EnumerableExtension, поскольку при его использовании учитывается регистр
            var roominfo = this.RoomCache[robject.Id].Values
                .FirstOrDefault(x => string.Equals(record.RoomNum, x.Room?.RoomNum, StringComparison.OrdinalIgnoreCase));

            if (roominfo == null)
            {
                var room = new Room
                {
                    RoomNum = record.RoomNum,
                    OwnershipType = RoomOwnershipType.NotSet,
                    RealityObject = robject,
                    Type = RoomType.Living,
                    Area = record.Area,
                    LivingArea = record.Area
                };

                roominfo = new RoomInfo
                {
                    Room = room
                };

                this.RoomCache[robject.Id][record.RoomNum] = roominfo;

                this.LogImport.Info("Строка " + record.RowNumber, "Добавлена информация о помещении");

                this.RoomsToSave.Add(roominfo.Room);
            }

            if (this.replaceData)
            {
                if (roominfo.Room.Area != record.Area.ToDecimal())
                {
                    this.roomLogToCreateDict[roominfo.Room] = period.StartDate;
                }

                roominfo.Room.Area = record.Area;
                roominfo.Room.LivingArea = record.Area;
            }

            return roominfo;
        }

        private List<EntityLogLight> PrepareEntityLogToSave(string login)
        {
            var result = new List<EntityLogLight>();
            var emptyPersonalAccount = new BasePersonalAccount();

            if (VersionedEntityHelper.IsUnderVersioning(emptyPersonalAccount))
            {
                var parameters = VersionedEntityHelper.GetCreator(emptyPersonalAccount)
                    .CreateParameters()
                    .Where(x => !VersionedEntityHelper.ShouldSkip(emptyPersonalAccount, x.ParameterName))
                    .ToList();

                var areaShareParameter = parameters.FirstOrDefault(x => x.ParameterName == VersionedParameters.AreaShare);
                var accountOpenDateParameter = parameters.FirstOrDefault(x => x.ParameterName == VersionedParameters.PersonalAccountOpenDate);

                var now = DateTime.UtcNow;

                if (areaShareParameter != null)
                {
                    foreach (var pair in this.accountAreaShareLogToCreateDict)
                    {
                        var log = new EntityLogLight
                        {
                            EntityId = pair.Key.Id,
                            ClassName = areaShareParameter.ClassName,
                            PropertyName = areaShareParameter.PropertyName,
                            PropertyValue = pair.Key.AreaShare.ToString("G29", this.culture),
                            ParameterName = areaShareParameter.ParameterName,
                            DateApplied = now,
                            DateActualChange = pair.Value,
                            User = login,
                            ObjectCreateDate = now,
                            ObjectEditDate = now
                        };

                        result.Add(log);
                    }
                }

                if (accountOpenDateParameter != null)
                {
                    foreach (var pair in this.accountOpenDateLogToCreateDict)
                    {
                        var log = new EntityLogLight
                        {
                            EntityId = pair.Key.Id,
                            ClassName = accountOpenDateParameter.ClassName,
                            PropertyName = accountOpenDateParameter.PropertyName,
                            PropertyValue = pair.Key.OpenDate.ToString("dd.MM.yyyy"),
                            ParameterName = accountOpenDateParameter.ParameterName,
                            DateApplied = now,
                            DateActualChange = pair.Value,
                            User = login,
                            ObjectCreateDate = now,
                            ObjectEditDate = now
                        };

                        result.Add(log);
                    }
                }
            }

            var emptyRoom = new Room();

            if (VersionedEntityHelper.IsUnderVersioning(emptyRoom))
            {
                var parameters = VersionedEntityHelper.GetCreator(emptyRoom)
                    .CreateParameters()
                    .Where(x => !VersionedEntityHelper.ShouldSkip(emptyRoom, x.ParameterName))
                    .ToList();

                var roomAreaParameter = parameters.FirstOrDefault(x => x.ParameterName == VersionedParameters.RoomArea);

                var now = DateTime.UtcNow;

                if (roomAreaParameter != null)
                {
                    foreach (var pair in this.roomLogToCreateDict)
                    {
                        var log = new EntityLogLight
                        {
                            EntityId = pair.Key.Id,
                            ClassName = roomAreaParameter.ClassName,
                            PropertyName = roomAreaParameter.PropertyName,
                            PropertyValue = pair.Key.Area.ToString("G29", this.culture),
                            ParameterName = roomAreaParameter.ParameterName,
                            DateApplied = now,
                            DateActualChange = pair.Value,
                            User = login,
                            ObjectCreateDate = now,
                            ObjectEditDate = now
                        };

                        result.Add(log);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Получить или Создать ЛС
        /// </summary>
        /// <param name="room">Помещение</param>
        /// <param name="record">Запись</param>
        /// <param name="period">Период</param>
        /// <returns></returns>
        protected BasePersonalAccount GetOrCreateAccount(RoomInfo room, DataRecord record, ChargePeriod period)
        {
            var account = this.AccountCache.Get(record.AccountNumberExternalSystems);

            var serviceType = record.TypeService == RecruitmentCode ? PersAccServiceType.Recruitment : PersAccServiceType.Overhaul;

            if (account == null)
            {
                if (room.AreaShare + record.AreaShare > 1)
                {
                    this.LogImport.Warn("Строка " + record.RowNumber, "Суммарная доля собственности по помещению больше 1");
                    return null;
                }

                room.AreaShare += record.AreaShare;

                var owner = new IndividualAccountOwner
                {
                    FirstName = "Тестовый",
                    SecondName = "Тестовый",
                    Surname = "Тестовый",
                    IdentityNumber = "Тестовый",
                    IdentitySerial = "Тестовый",
                    Inn = "Тестовый",
                    Kpp = "Тестовый",
                    IdentityType = IdentityType.Passport,
                    BirthDate = DateTime.Today,
                    OwnerType = PersonalAccountOwnerType.Individual
                };

                this.OwnersToSave.Add(owner);

                account = new BasePersonalAccount
                {
                    Area = record.Area,
                    AreaShare = record.AreaShare,
                    LivingArea = record.Area * record.AreaShare,
                    Tariff = record.Tariff,
                    Room = room.Room,
                    PersAccNumExternalSystems = record.AccountNumberExternalSystems,
                    OpenDate = record.OpenDate,
                    State = this.PersonalAccountStartState,
                    AccountOwner = owner,
                    ServiceType = serviceType
                };

                account.SetCloseDate(record.CloseDate ?? DateTime.MinValue, false);

                this.LogImport.Info("Строка " + record.RowNumber, "Добавлена информация о лицевом счете");

                this.AccountCache[record.AccountNumberExternalSystems] = account;

                this.AccountsToSave.Add(account);

                this.AccountsToGenerateNumber.Add(account);

                this.CreateWalletIfNeed(account);

                this.LogImport.CountAddedRows++;

                return account;
            }

            if (this.replaceData)
            {
                if (account.AreaShare != record.AreaShare)
                {
                    this.accountAreaShareLogToCreateDict[account] = period.StartDate;
                }

                if (account.OpenDate != record.OpenDate)
                {
                    this.accountOpenDateLogToCreateDict[account] = period.StartDate;
                }

                account.AreaShare = record.AreaShare;
                account.LivingArea = record.Area * record.AreaShare;
                account.Area = record.Area;
                account.OpenDate = record.OpenDate;
                account.SetCloseDate(record.CloseDate ?? DateTime.MinValue, false);
                account.ServiceType = serviceType;

                if (!string.Equals(account.State.Name, record.AccountState, StringComparison.CurrentCultureIgnoreCase))
                {
                    account.State = this.accountStatuses.TryGetValue(record.AccountState.ToLower(), out var newStatus)
                        ? newStatus
                        : this.PersonalAccountStartState;
                }

                this.LogImport.CountChangedRows++;
            }

            this.CreateWalletIfNeed(account);

            return account;
        }
        /// <summary>
        /// Парсинга записи и получение даты для периода 
        /// </summary>
        /// <param name="record">Запись</param>
        /// <returns></returns>
        protected DateTime ParseStringPeriodToStartDate(DataRecord record)
        {
            var period = record.Period;

            if (string.IsNullOrEmpty(period))
            {
                return default(DateTime);
            }

            var splits = period.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (splits.Length != 2)
            {
                return default(DateTime);
            }

            var year = splits[0].ToInt();

            if (year < 1 || year > 9999)
            {
                return default(DateTime);
            }

            int month = 0;

            switch (splits[1].ToLower())
            {
                case "январь":
                    month = 1;
                    break;
                case "февраль":
                    month = 2;
                    break;
                case "март":
                    month = 3;
                    break;
                case "апрель":
                    month = 4;
                    break;
                case "май":
                    month = 5;
                    break;
                case "июнь":
                    month = 6;
                    break;
                case "июль":
                    month = 7;
                    break;
                case "август":
                    month = 8;
                    break;
                case "сентябрь":
                    month = 9;
                    break;
                case "октябрь":
                    month = 10;
                    break;
                case "ноябрь":
                    month = 11;
                    break;
                case "декабрь":
                    month = 12;
                    break;
            }

            if (month < 0 || month > 12)
            {
                return default(DateTime);
            }

            return new DateTime(year, month, 1);
        }

        /// <summary>
        /// Агригация денег
        /// </summary>
        /// <param name="robject">Жилой дом</param>
        /// <param name="account">ЛС</param>
        /// <param name="period">Период</param>
        /// <param name="record">Запись</param>
        protected void AggregateMoney(RealityObject robject, BasePersonalAccount account, ChargePeriod period, DataRecord record)
        {
            //агрегируем информацию по оплатам, начислениям, перерасчету
            //потом положим их на счета как одну операцию
            if (record.TypeService.IsEmpty() || !this.TypeServiceRegex.IsMatch(record.TypeService))
            {
                this.LogImport.Warn("Строка " + record.RowNumber, "Неверный тип услуги");
                return;
            }

            if (!this.MoneyAggregationDict.ContainsKey(record.AccountNumberExternalSystems))
            {
                this.MoneyAggregationDict[record.AccountNumberExternalSystems] = new Dictionary<long, List<MoneyAggregation>>();
            }

            if (!this.MoneyAggregationDict[record.AccountNumberExternalSystems].ContainsKey(period.Id))
            {
                this.MoneyAggregationDict[record.AccountNumberExternalSystems][period.Id] = new List<MoneyAggregation>();
            }

            var aggregation = new MoneyAggregation
            {
                Period = period,
                Robject = robject,
                Account = account,
                RowNumber = record.RowNumber,
                Charge = record.Charged,
                Payment = record.Paid,
                Recalc = record.Recalc,
                SaldoOut = record.SaldoOut,
                SaldoIn = record.SaldoIn
            };

            switch (record.TypeService)
            {
                case ThirdPartyPersonalAccountImport.RecruitmentCode:
                    {
                        aggregation.OperationType = PaymentOperationType.IncomeRent;
                        break;
                    }
                case ThirdPartyPersonalAccountImport.OverhaulPenaltyCode:
                case ThirdPartyPersonalAccountImport.RecruitmentPenaltyCode:
                    {
                        aggregation.OperationType = PaymentOperationType.IncomePenalty;
                        break;
                    }
                default:
                    {
                        aggregation.OperationType = PaymentOperationType.IncomeCr;
                        break;
                    }
            }

            this.MoneyAggregationDict[record.AccountNumberExternalSystems][period.Id].Add(aggregation);
        }

        /// <summary>
        /// Применить на личный счет
        /// </summary>
        /// <param name="aggregation">Запись</param>
        protected void ApplyToPersonalAccount(MoneyAggregation aggregation)
        {
            var account = aggregation.Account;
            var period = aggregation.Period;

            var byPeriods = this.AccountSummaryCache.Get(account.PersAccNumExternalSystems);

            if (byPeriods == null)
            {
                byPeriods = new Dictionary<long, PersonalAccountPeriodSummary>();

                this.AccountSummaryCache.Add(account.PersAccNumExternalSystems, byPeriods);
            }

            var summary = byPeriods.Get(period.Id);

            if (summary == null)
            {
                summary = new PersonalAccountPeriodSummary
                {
                    PersonalAccount = account,
                    Period = period
                };
                byPeriods[period.Id] = summary;
            }

            decimal balance = 0;

            this.FillPeriodSummary(aggregation, summary, ref balance);

            this.PeriodCache
                .OrderByDescending(x => x.Key)
                .ForEach(
                    x =>
                    {
                        //если нет информации по периоду
                        //или период является тем, в который кладутся деньги
                        // то ничего не делаем, деньги уже учтены парой строк выше
                        if (!byPeriods.ContainsKey(x.Value.Id)
                            || x.Value.Id == period.Id)
                        {
                            return;
                        }

                        byPeriods[x.Value.Id].SaldoIn += balance;
                        byPeriods[x.Value.Id].SaldoOut += balance;
                    });

            if (!this.SummariesToSave.Contains(summary))
            {
                this.SummariesToSave.Add(summary);
            }
        }

        private void FillPeriodSummary(MoneyAggregation aggregation, PersonalAccountPeriodSummary summary, ref decimal balance)
        {
            //сумма по оплате и начислению, садится на исходящее сальдо
            balance = aggregation.Charge + aggregation.Recalc - aggregation.Payment;
            summary.SaldoOut += balance;

            if (aggregation.OperationType == PaymentOperationType.IncomePenalty)
            {
                if (this.replaceData)
                {
                    summary.Penalty = 0;
                    summary.PenaltyPayment = 0;
                    summary.RecalcByPenalty = 0;
                }

                summary.Penalty += aggregation.Charge;
                summary.PenaltyPayment += aggregation.Payment;
                summary.RecalcByPenalty += aggregation.Recalc;
            }
            else
            {
                if (this.replaceData)
                {
                    summary.SaldoIn = 0;
                    summary.ChargeTariff = 0;
                    summary.ChargedByBaseTariff = 0;
                    summary.TariffPayment = 0;
                    summary.RecalcByBaseTariff = 0;
                    summary.SaldoOut = 0;

                    if (aggregation.OperationType == PaymentOperationType.IncomeRent)
                    {
                        summary.RecruitmentPayment = 0;
                    }

                    if (aggregation.OperationType == PaymentOperationType.IncomeCr)
                    {
                        summary.OverhaulPayment = 0;
                    }

                }

                summary.SaldoIn += aggregation.SaldoIn;
                summary.ChargeTariff += aggregation.Charge;
                summary.ChargedByBaseTariff += aggregation.Charge;
                summary.TariffPayment += aggregation.Payment;
                summary.RecalcByBaseTariff += aggregation.Recalc;
                summary.SaldoOut += aggregation.SaldoOut;

                if (aggregation.OperationType == PaymentOperationType.IncomeRent)
                {
                    summary.RecruitmentPayment += aggregation.Payment;
                }

                if (aggregation.OperationType == PaymentOperationType.IncomeCr)
                {
                    summary.OverhaulPayment += aggregation.Payment;
                }
            }
        }

        /// <summary>
        /// Просуммировать начислено и оплачено из лс в счета начислений (по периодам)
        /// </summary>
        protected void SaveChargeAccountOperations()
        {
            var unProxy = this.Container.Resolve<IUnProxy>();

            var chargeAccItems = new List<RealityObjectChargeAccount>();
            var chargeAccOperationsItems = new List<RealityObjectChargeAccountOperation>();

            var roIdsDict = this.roIds.Distinct().ToDictionary(x => x);

            var persAccs = this.AccountCache.Values.Where(x => roIdsDict.ContainsKey(x.Room.RealityObject.Id)).ToList();

            var persAccsIds = persAccs.Select(x => x.Id).Distinct().ToDictionary(x => x);

            // Их счета начислений
            var chargeAccounts = this.RoChargeAccountRepo.GetAll()
                .Where(x => this.roIds.Contains(x.RealityObject.Id))
                .ToList();

            var chargeAccountsIds = chargeAccounts.Select(x => x.Id).Distinct().ToDictionary(x => x);

            // Операции счета начислений
            var chargeAccOpers = this.RoChargeAccountSummaryRepo.GetAll()
                .Where(x => this.roIds.Contains(x.Account.RealityObject.Id))
                .AsEnumerable()
                .Where(x => chargeAccountsIds.ContainsKey(x.Account.Id))
                .GroupBy(x => x.Account.Id)
                .ToDictionary(x => x.Key, arg => arg.ToList());

            // операции лс по дому, периоду
            var periodSummaries = this.AccountSummaryRepo.GetAll()
                .Where(x => this.roIds.Contains(x.PersonalAccount.Room.RealityObject.Id) && this.periods.Contains(x.Period.Id))
                .Select(x => new { ro_Id = x.PersonalAccount.Room.RealityObject.Id, PeriodSummary = x })
                .AsEnumerable()
                .GroupBy(x => x.ro_Id)
                .ToDictionary(
                    x => x.Key,
                    arg => arg.GroupBy(x => x.PeriodSummary.Period)
                        .ToDictionary(y => y.Key, arg2 => arg2.Select(z => z.PeriodSummary).ToList()));

            // берем счет начислений
            foreach (var charAcc in chargeAccounts)
            {
                Dictionary<ChargePeriod, List<PersonalAccountPeriodSummary>> periodSummariesByRo;
                if (!periodSummaries.TryGetValue(charAcc.RealityObject.Id, out periodSummariesByRo))
                {
                    continue;
                }

                // операции лс по этому дому
                // по всем периодам операций лс
                foreach (var chargePeriod in periodSummariesByRo.Keys)
                {
                    RealityObjectChargeAccountOperation chargeAccOper = null;
                    var period = chargePeriod;

                    // ищем операцию за тот же период в счете начислений иначе создаем
                    List<RealityObjectChargeAccountOperation> chargeOperations;
                    if (chargeAccOpers.TryGetValue(charAcc.Id, out chargeOperations))
                    {
                        chargeAccOper = chargeOperations.FirstOrDefault(x => x.Period.Id == period.Id);
                    }

                    if (chargeAccOper == null)
                    {
                        chargeAccOper = new RealityObjectChargeAccountOperation
                        {
                            Account = charAcc,
                            Period = period,
                            Date = period.StartDate
                        };
                    }

                    // агрегируем
                    var summariesPeriod = periodSummariesByRo[period];
                    chargeAccOper.ChargedTotal = summariesPeriod.SafeSum(x => x.ChargedByBaseTariff + x.RecalcByBaseTariff + x.Penalty + x.RecalcByPenalty);
                    chargeAccOper.ChargedPenalty = summariesPeriod.SafeSum(x => x.Penalty + x.RecalcByPenalty);
                    chargeAccOper.PaidTotal = summariesPeriod.SafeSum(x => x.TariffPayment);
                    chargeAccOper.PaidPenalty = summariesPeriod.SafeSum(x => x.PenaltyPayment);
                    chargeAccOper.SaldoIn = summariesPeriod.SafeSum(x => x.SaldoIn);
                    chargeAccOper.SaldoOut = summariesPeriod.SafeSum(x => x.SaldoOut);

                    chargeAccOperationsItems.Add(chargeAccOper);
                }

                if (chargeAccOpers.ContainsKey(charAcc.Id))
                {
                    // Все оплаты
                    charAcc.PaidTotal = chargeAccOpers[charAcc.Id].SafeSum(x => x.PaidTotal + x.PaidPenalty);
                }

                chargeAccItems.Add(charAcc);
            }

            // Намеренно закрываю сессию, что orm забыл про измененные записи, полученные объектами
            this.SessionProvider.CloseCurrentSession();

            var session = this.SessionProvider.OpenStatelessSession();

            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    chargeAccItems.ForEach(
                        x =>
                        {
                            if (x.Id > 0)
                            {
                                session.Update(unProxy.GetUnProxyObject(x));
                            }
                            else
                            {
                                session.Insert(unProxy.GetUnProxyObject(x));
                            }
                        });

                    chargeAccOperationsItems.ForEach(
                        x =>
                        {
                            if (x.Id > 0)
                            {
                                session.Update(x);
                            }
                            else
                            {
                                session.Insert(x);
                            }
                        });

                    transaction.Commit();
                }
                catch (Exception)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch
                    {
                        // ignored
                    }

                    throw;
                }
            }
        }

        /// <summary>
        /// Проверка значения 
        /// </summary>
        /// <param name="data">Колонка в excel</param>
        /// <param name="index">Индекс</param>
        /// <returns></returns>
        protected string GetValue(GkhExcelCell[] data, int index)
        {
            var result = string.Empty;

            if (index > -1 && data.Length >= index)
            {
                result = data[index].Value.ToStr();
            }

            return result.Trim();
        }

        /// <summary>
        /// Создать кошелек в случае необходимости
        /// </summary>
        /// <param name="account">Лицевой счет</param>
        protected void CreateWalletIfNeed(BasePersonalAccount account)
        {
            var btw = account.BaseTariffWallet;
            var dtw = account.DecisionTariffWallet;
            var rw = account.RentWallet;
            var pw = account.PenaltyWallet;
            var ssw = account.SocialSupportWallet;
            var pww = account.PreviosWorkPaymentWallet;
            var afw = account.AccumulatedFundWallet;
            var raaw = account.RestructAmicableAgreementWallet;
        }

        /// <summary>
        /// Проверка 
        /// </summary>
        /// <param name="baseParams">Базовый параметр</param>
        /// <param name="message">Сообщение</param>
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

            var fileExtentions = this.PossibleFileExtensions.Contains(",")
                ? this.PossibleFileExtensions.Split(',')
                : new[] { this.PossibleFileExtensions };

            if (fileExtentions.All(x => x != extention))
            {
                message = $"Необходимо выбрать файл с допустимым расширением: {this.PossibleFileExtensions}";
                return false;
            }

            return true;
        }

        protected class MoneyAggregation
        {
            /// <summary>
            /// Пени
            /// </summary>
            public decimal Payment { get; set; }

            /// <summary>
            /// Начисление 
            /// </summary>
            public decimal Charge { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public decimal Recalc { get; set; }

            /// <summary>
            /// Входящее сальдо
            /// </summary>
            public decimal SaldoIn { get; set; }

            /// <summary>
            /// Исходящее сальдо
            /// </summary>
            public decimal SaldoOut { get; set; }

            /// <summary>
            /// Номер
            /// </summary>
            public int RowNumber { get; set; }

            /// <summary>
            ///  Период начислений
            /// </summary>
            public ChargePeriod Period { get; set; }

            /// <summary>
            /// Жилой дом
            /// </summary>
            public RealityObject Robject { get; set; }

            /// <summary>
            /// Лицевой счет
            /// </summary>
            public BasePersonalAccount Account { get; set; }

            /// <summary>
            /// Тип операции по счету оплат дома
            /// </summary>
            public PaymentOperationType OperationType { get; set; }
        }
    }
}