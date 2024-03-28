namespace Bars.Gkh.RegOperator.Imports.DecisionProtocol
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Modules.Security;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Decisions.Nso.Entities.Decisions;
    using Bars.Gkh.Decisions.Nso.Entities.Proxies;
    using Bars.Gkh.Domain.TableLocker;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.GkhExcel;
    using Fasterflect;
    using Import.Impl;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using global::Quartz.Util;

    /// <summary>
    /// Импорт протокола решений
    /// </summary>
    public class DecisionProtocolImport : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        #region Overrides

        /// <summary>
        /// Ключ импорта
        /// </summary>
        public override string Key { get { return Id; } }

        /// <summary>
        /// Код группировки импорта (например группировка в меню)
        /// </summary>
        public override string CodeImport {get { return "DecisionProtocolImport"; } }

        /// <summary>
        /// Наименование импорта
        /// </summary>
        public override string Name { get { return "Импорт протоколов решений"; } }

        /// <summary>
        /// Разрешенные расширения файлов
        /// </summary>
        public override string PossibleFileExtensions { get { return "xlsx"; } }

        /// <summary>
        /// Права
        /// </summary>
        public override string PermissionName { get { return "Import.DecisionProtocolImport"; } }

        #endregion

        #region Properties

        /// <summary>
        /// Контейнер
        /// </summary>
        //new public IWindsorContainer Container { get; set; }

        /// <summary>
        ///     Логгер импорта
        /// </summary>
        new public ILogImport LogImport { get; set; }

        /// <summary>
        /// Репозиторий для <see cref="RealityObjectDecisionProtocol"/>
        /// </summary>
        public IRepository<RealityObjectDecisionProtocol> RoDecisionProtocolRepository { get; set; }

        /// <summary>
        /// Репозиторий для <see cref="GovDecision"/>
        /// </summary>
        public IRepository<GovDecision> RoGovDecisionRepository { get; set; }

        /// <summary>
        /// Репозиторий для <see cref="StateHistory"/>
        /// </summary>
        public IRepository<StateHistory> StateHistoryRepository { get; set; }

        /// <summary>
        /// Репозиторий для <see cref="User"/>
        /// </summary>
        public IRepository<User> UserRepository { get; set; }

        /// <summary>
        /// Интерфейс идентификатора пользователя
        /// </summary>
        public IUserIdentity UserIdentity { get; set; }

        #endregion

        #region Nested classes

        /// <summary>
        /// Строка протокола.
        /// </summary>
        public class DecisionProtocolRow
        {
            /// <summary>
            /// Номер строки
            /// </summary>
            public int RowNumber { get; set; }

            /// <summary>
            /// Номер
            /// </summary>
            public int Number { get; set; }

            /// <summary>
            /// Индекс
            /// </summary>
            public string HouseIndex { get; set; }

            /// <summary>
            /// Адрес КЛАДР
            /// </summary>
            public string HouseKlard { get; set; }

            /// <summary>
            /// Идентификатор жилого дома
            /// </summary>
            public long RoId { get; set; }

            /// <summary>
            /// МО
            /// </summary>
            public string Municipality { get; set; }

            /// <summary>
            /// МУ
            /// </summary>
            public string MU { get; set; }

            /// <summary>
            /// Тип города
            /// </summary>
            public string KindCity { get; set; }

            /// <summary>
            /// Тип города
            /// </summary>
            public string DateFrom { get; set; }

            /// <summary>
            /// Город
            /// </summary>
            public string City { get; set; }

            /// <summary>
            /// Тип улицы
            /// </summary>
            public string KindStreet { get; set; }

            /// <summary>
            /// Улица
            /// </summary>
            public string Street { get; set; }

            /// <summary>
            /// Номер дома
            /// </summary>
            public string HouseNum { get; set; }

            /// <summary>
            /// Корпус
            /// </summary>
            public string CorpNum { get; set; }

            /// <summary>
            /// Литерал
            /// </summary>
            public string Char { get; set; }

            /// <summary>
            /// Дом
            /// </summary>
            public string Building { get; set; }

            /// <summary>
            /// Тип протокола
            /// </summary>
            public string protocoltype { get; set; }

            /// <summary>
            /// Номер протокола
            /// </summary>
            public string protocolnum { get; set; }

            /// <summary>
            /// Дата протокола
            /// </summary>
            public string date_protocol { get; set; }

            /// <summary>
            /// Дата вступления в силу протокола
            /// </summary>
            public string date_entry_protocol { get; set; }

            /// <summary>
            /// Управление домом
            /// </summary>
            public string mkdperson { get; set; }

            /// <summary>
            /// Дата проведения общего собрания
            /// </summary>
            public string meetingdate { get; set; }

            /// <summary>
            /// Уполномоченное лицо
            /// </summary>
            public string FIO { get; set; }

            /// <summary>
            /// Телефон
            /// </summary>
            public string phone { get; set; }

            /// <summary>
            /// Тип счета
            /// </summary>
            public string CrFundFormationType { get; set; }

            /// <summary>
            /// Владелец счета
            /// </summary>
            public string accountowner { get; set; }

            /// <summary>
            /// Ежемесячная оплата
            /// </summary>
            public string monthlypayment { get; set; }

            /// <summary>
            /// Дата начала
            /// </summary>
            public string datefrom { get; set; }

            /// <summary>
            /// Дата окончания
            /// </summary>
            public string dateto { get; set; }

            /// <summary>
            /// Кредитная организация
            /// </summary>
            public string creditorg { get; set; }

            /// <summary>
            /// Минимальный процент
            /// </summary>
            public string minCrFundPercent { get; set; }

            /// <summary>
            /// Сумма перевода
            /// </summary>
            public string transferamount { get; set; }

            /// <summary>
            /// Статус протокола
            /// </summary>
            public string protocolstate { get; set; }

            /// <summary>
            /// Снос МКД
            /// </summary>
            public string destroymkd { get; set; }

            /// <summary>
            /// Дата сноса МКД
            /// </summary>
            public string destroydate { get; set; }

            /// <summary>
            /// Реконструкция МКД
            /// </summary>
            public string reconstructionmkd { get; set; }

            /// <summary>
            /// Дата начала реконструкции
            /// </summary>
            public string govdatefrom { get; set; }

            /// <summary>
            /// Дата окончания реконструкции
            /// </summary>
            public string govdateto { get; set; }

            /// <summary>
            /// Изъятие для государственных или муниципальных нужд земельного участка, на котором расположен МКД
            /// </summary>
            public string withdrawarea { get; set; }

            /// <summary>
            /// Дата изъятия земельного участка
            /// </summary>
            public string withdrawareadate { get; set; }

            /// <summary>
            /// Изъятие каждого жилого помещения в доме
            /// </summary>
            public string withdrawflat { get; set; }

            /// <summary>
            /// Дата изъятия жилых помещений
            /// </summary>
            public string withdrawflatdate { get; set; }

            /// <summary>
            /// Оплата
            /// </summary>
            public string govminCrPayment { get; set; }

            /// <summary>
            /// Максимальный размер фонда
            /// </summary>
            public string govMaxCrFundAmount { get; set; }

            /// <summary>
            /// Работа 1
            /// </summary>
            public string work1 { get; set; }

            /// <summary>
            /// Работа 2
            /// </summary>
            public string work2 { get; set; }

            /// <summary>
            /// Работа 3
            /// </summary>
            public string work3 { get; set; }

            /// <summary>
            /// Работа 4
            /// </summary>
            public string work4 { get; set; }

            /// <summary>
            /// Работа 5
            /// </summary>
            public string work5 { get; set; }

            /// <summary>
            /// Работа 6
            /// </summary>
            public string work6 { get; set; }

            /// <summary>
            /// Работа 7
            /// </summary>
            public string work7 { get; set; }

            /// <summary>
            /// Работа 8
            /// </summary>
            public string work8 { get; set; }

            /// <summary>
            /// Работа 9
            /// </summary>
            public string work9 { get; set; }

            /// <summary>
            /// Работа 10
            /// </summary>
            public string work10 { get; set; }

            /// <summary>
            /// Работа 11
            /// </summary>
            public string work11 { get; set; }
        }

        private class RobjectProxy
        {
            public int Count { get; set; }

            public RealityObject Robject { get; set; }
        }
        #endregion

        #region Public methods

        /// <summary>
        /// Первоначальная проверка файла перед импортом
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <param name="message">Ошибки  валидации</param>
        /// <returns>Результат проверки</returns>
        override public bool Validate(BaseParams baseParams, out string message)
        {
            message = null;

            var tableLocker = Container.Resolve<ITableLocker>();
            try
            {
                if (tableLocker.CheckLocked<BasePersonalAccount>("INSERT"))
                {
                    message = TableLockedException.StandardMessage;
                    return false;
                }
            }
            finally
            {
                Container.Release(tableLocker);
            }

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
                message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", this.PossibleFileExtensions);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Импорт данных
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Итоги импорта</returns>
        public override ImportResult Import(BaseParams baseParams)
        {
            var file = baseParams.Files["FileImport"];
            var transferToDefaultState = baseParams.Params.GetAs<bool>("TransferToDefaultState");
            var user = UserRepository.Get(UserIdentity.UserId);
            
            var parsedRows = new List<DecisionProtocolRow>();

            // Парсинг сущностей
            var excel = this.Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider");
            using (this.Container.Using(excel))
            {
                if (excel == null)
                    throw new Exception("Не найдена реализация интерфейса IGkhExcelProvider");

                excel.UseVersionXlsx();
                
                using (var xlsMemoryStream = new MemoryStream(file.Data))
                {
                    excel.Open(xlsMemoryStream);

                    var dict = new Dictionary<string, string>();
                    var data = excel.GetRows(0, 0);                 
                    var startIndex = 0;                    

                    if (data.Count == 0)
                        return new ImportResult(StatusImport.CompletedWithWarning, "Импортируемый файл пуст");

                    //создаем массив заголовков
                    var rowNum = 0;
                    string[] header = null;
                    while (startIndex == 0 && rowNum < data.Count)
                    {
                        var row = data[rowNum].Where(x => x.Value != null).Select(x => x.Value.ToLower()).ToArray();

                        if (row.Contains("num"))
                        {
                            startIndex = rowNum + 1;
                            header = row;
                        }

                        rowNum++;
                    }

                    if (startIndex == 0)
                    {
                        return new ImportResult(StatusImport.CompletedWithError, "Формат шаблона не соответсвует импорту!\nНе найден столбец \"num\"");
                    }

                    for (var i = startIndex; i < data.Count; i++)
                    {
                        dict.Clear();

                        //заполняем словарь столбиков
                        var row = data[i];
                        for (var j = 0; j < row.Length; j++)
                        {
                            if (header != null && !header[j].IsNullOrWhiteSpace())
                            {
                                dict.Add(header[j], row[j].Value);
                            }
                        }

                        //парсим в класс
                        var entity = new DecisionProtocolRow
                        {
                            RowNumber = i,
                            Number = dict.Get("num").ToInt(),
                            HouseIndex = dict.Get("index"),
                            HouseKlard = dict.Get("kladr"),
                            RoId = dict.Get("id_doma").ToLong(),
                            Municipality = dict.Get("mr"),
                            MU = dict.Get("mu"),
                            KindCity = dict.Get("kindcity"),
                            City = dict.Get("city"),
                            KindStreet = dict.Get("kindstreet"),
                            Street = dict.Get("street"),
                            HouseNum = dict.Get("housenum"),
                            CorpNum = dict.Get("corpnum"),
                            Char = dict.Get("char"),
                            Building = dict.Get("building"),
                            protocoltype = dict.Get("protocoltype"),
                            protocolnum = dict.Get("protocolnum"),
                            date_protocol = dict.Get("date_protocol"),
                            DateFrom = dict.Get("datefrom"),
                            date_entry_protocol = dict.Get("date_entry_protocol"),
                            mkdperson = dict.Get("mkdperson"),
                            meetingdate = dict.Get("meetingdate"),
                            FIO = dict.Get("fio"),
                            phone = dict.Get("phone"),
                            CrFundFormationType = dict.Get("crfundformationtype"),
                            accountowner = dict.Get("accountowner"),
                            monthlypayment = dict.Get("monthlypayment"),
                            datefrom = dict.Get("datefrom"),
                            dateto = dict.Get("dateto"),
                            creditorg = dict.Get("creditorg"),
                            minCrFundPercent = dict.Get("mincrfundpercent"),
                            transferamount = dict.Get("transferamount"),
                            protocolstate = dict.Get("protocolstate"),
                            destroymkd = dict.Get("destroymkd"),
                            destroydate = dict.Get("destroydate"),
                            reconstructionmkd = dict.Get("reconstructionmkd"),
                            govdatefrom = dict.Get("govdatefrom"),
                            govdateto = dict.Get("govdateto"),
                            withdrawarea = dict.Get("withdrawarea"),
                            withdrawareadate = dict.Get("withdrawareadate"),
                            withdrawflat = dict.Get("withdrawflat"),
                            withdrawflatdate = dict.Get("withdrawflatdate"),
                            govminCrPayment = dict.Get("govmincrpayment"),
                            govMaxCrFundAmount = dict.Get("govmaxcrfundamount"),
                            work1 = dict.Get("work1") ?? string.Empty,
                            work2 = dict.Get("work2") ?? string.Empty,
                            work3 = dict.Get("work3") ?? string.Empty,
                            work4 = dict.Get("work4") ?? string.Empty,
                            work5 = dict.Get("work5") ?? string.Empty,
                            work6 = dict.Get("work6") ?? string.Empty,
                            work7 = dict.Get("work7") ?? string.Empty,
                            work8 = dict.Get("work8") ?? string.Empty,
                            work9 = dict.Get("work9") ?? string.Empty,
                            work10 = dict.Get("work10") ?? string.Empty,
                            work11 = dict.Get("work11") ?? string.Empty,
                        };

                        if (entity.Number == 0)
                        {
                            LogImport.Error(
                                "Протокол решения не добавлен",
                                string.Format("Для протокола решения в строке {0} не найден порядковый номер", i + 1));
                            continue;
                        }

                        if (entity.RoId == 0)
                        {
                            if (string.IsNullOrWhiteSpace(entity.HouseIndex) &&
                                string.IsNullOrWhiteSpace(entity.HouseKlard) &&
                                string.IsNullOrWhiteSpace(entity.Municipality))
                            {
                                this.LogImport.Error(
                                    "Протокол решения не добавлен",
                                    string.Format(
                                        "Для протокола решения в строке {0} не найден индекс дома и индекс кладр и наименование муниципального района",
                                        i + 1));
                                continue;
                            }

                            if (string.IsNullOrWhiteSpace(entity.HouseNum))
                            {
                                this.LogImport.Error(
                                    "Протокол решения не добавлен",
                                    string.Format("Для протокола решения в строке {0} не найден номер дома. Добавьте столец \"housenum\"", i + 1));
                                continue;
                            }
                        }

                        if (string.IsNullOrWhiteSpace(entity.date_protocol) || string.IsNullOrWhiteSpace(entity.date_entry_protocol))
                        {
                            if (!string.IsNullOrWhiteSpace(entity.DateFrom))
                            {
                                entity.date_protocol = entity.DateFrom;
                                entity.date_entry_protocol = entity.DateFrom;
                            }
                            else
                            {
                                this.LogImport.Error(
                                    "Протокол решения не добавлен",
                                    string.Format(
                                        "Для протокола решения в строке {0} не найдены дата потокола или дата вступления в силу протокола. Добавьте столец \"date_protocol\" и \"date_entry_protocol\"",
                                        i + 1));
                                continue;
                            }
                        }

                        if (string.IsNullOrWhiteSpace(entity.protocoltype))
                        {
                            this.LogImport.Error(
                                  "Протокол решения не добавлен",
                                  string.Format("Для протокола решения в строке {0} не найден тип протокола. Добавьте столец \"protocoltype\"", i + 1));
                            continue;
                        }

                        switch (entity.protocoltype.Trim())
                        {
                            case "0":
                                entity.protocoltype = ((int)CoreDecisionType.Owners).ToString();
                                break;
                            case "1":
                                entity.protocoltype = ((int)CoreDecisionType.Government).ToString();
                                break;
                            default:
                                entity.protocoltype = entity.protocoltype.Trim();
                                break;
                        }

                        if (entity.protocoltype != ((int)CoreDecisionType.Owners).ToString() && entity.protocoltype != ((int)CoreDecisionType.Government).ToString())
                        {
                            this.LogImport.Error("Протокол решения не добавлен",
                                  string.Format("Для протокола решения в строке {0} не найден известный тип протокола. Тип протокола: {1}", i + 1, entity.protocoltype));
                            continue;
                        }

                        parsedRows.Add(entity);
                    }
                }
            }

            var jobs = this.Container.ResolveDomain<Job>().GetAll()
                .Where(x => x.Work.Code != null)
                .Select(x => new
                {
                    x.Work.Code,
                    Job = x
                })
                .AsEnumerable()
                .GroupBy(x => x.Code)
                .ToDictionary(x => x.Key, y => y.First().Job);

            var stateDomain = this.Container.ResolveDomain<State>();

            var finalState = stateDomain.GetAll().FirstOrDefault(item => item.TypeId == "gkh_real_obj_dec_protocol" && item.FinalState);
            if (finalState == null)
            {
                this.LogImport.Error("Отсутствие статуса для сущности",
                    "Отсутсвие конечного статуса для сущности gkh_real_obj_dec_protocol");
                throw new Exception("Для сущности \"Протокол решений\" отсутсвует конечный статус");
            }
            var defaultState = stateDomain.GetAll().FirstOrDefault(item => item.TypeId == "gkh_real_obj_dec_protocol" && item.StartState);
            if (defaultState == null)
            {
                this.LogImport.Error("Отсутствие статуса для сущности",
                    "Отсутсвие начального статуса для сущности gkh_real_obj_dec_protocol");
                throw new Exception("Для сущности \"Протокол решений\" отсутсвует начальный статус");
            }

            var govFinalState = stateDomain.GetAll().FirstOrDefault(item => item.TypeId == "gkh_real_obj_gov_dec" && item.FinalState);
            if (govFinalState == null)
            {
                this.LogImport.Error("Отсутствие статуса для сущности",
                    "Отсутсвие  конечного статуса для сущности gkh_real_obj_gov_dec");
                throw new Exception("Для сущности \"Протокол решений органа гос. власти\" отсутсвует конечный статус");
            }
            var govDefaultState = stateDomain.GetAll().FirstOrDefault(item => item.TypeId == "gkh_real_obj_gov_dec" && item.StartState);
            if (govDefaultState == null)
            {
                this.LogImport.Error("Отсутствие статуса для сущности",
                    "Отсутсвие начального статуса для сущности gkh_real_obj_gov_dec");
                throw new Exception("Для сущности \"Протокол решений органа гос. власти\" отсутсвует начальный статус");
            }

            this.InitCache();

            if (parsedRows.Count > 0)
            {
                var govDecisionDomain = this.Container.ResolveRepository<GovDecision>();
                var creditOrg = this.Container.ResolveDomain<CreditOrg>();
                var creditOrgs = creditOrg.GetAll().ToArray();
                
                foreach (var partition in Partitioner.Create(0, parsedRows.Count).GetDynamicPartitions())
                {
                    var tr = this.Container.Resolve<IDataTransaction>();
                    try
                    {
                        for (var index = partition.Item1; index < partition.Item2; index++)
                        {
                            var entry = parsedRows[index];
                            if (entry.protocoltype == ((int)CoreDecisionType.Owners).ToString())
                            {
                                var robject = this.TryGetRealityObject(entry);

                                if (robject == null)
                                {
                                    var description = string.Format(
                                        "Не удалось получить жилой дом {0} ул.{1} д.{2}{3} {4} {5}",
                                        entry.City,
                                        entry.Street,
                                        entry.HouseNum,
                                        entry.Char,
                                        entry.CorpNum,
                                        entry.Building)
                                        .TrimEnd();
                                    this.LogImport.Error("Строка " + index, description);
                                    continue;
                                }

                                var protocolDate = entry.date_protocol.ToDateTime();
                                var newProtocol = new RealityObjectDecisionProtocol
                                {
                                    DocumentNum = entry.protocolnum,
                                    ProtocolDate = entry.date_protocol.ToDateTime(),
                                    DateStart = entry.date_entry_protocol.ToDateTime(),
                                    AuthorizedPerson = entry.FIO,
                                    PhoneAuthorizedPerson = entry.phone,
                                    State = finalState,
                                    RealityObject = robject
                                };

                                if (this.ownerDecisions.ContainsKey(robject.Id))
                                {
                                    var roOwnerDecisions = this.ownerDecisions[robject.Id];
                                    if (roOwnerDecisions.Any(item => item.ProtocolDate == protocolDate && newProtocol.DocumentNum == item.Protocol.DocumentNum))
                                    {
                                        var description = "Для объекта {0} протокол решения с датой {1:yyyy-MM-dd} уже существует".FormatUsing(robject.Address, protocolDate);
                                        this.LogImport.Info("Строка " + index, description);
                                        this.LogImport.CountChangedRows++;
                                        continue;
                                    }

                                    if (transferToDefaultState)
                                    {
                                        foreach (var decision in roOwnerDecisions)
                                        {
                                            if (decision.InFinalState)
                                            {
                                                decision.Protocol.State = defaultState;
                                                decision.InFinalState = false;

                                                this.RoDecisionProtocolRepository.Update(decision.Protocol);
                                                this.SaveStateHistory(
                                                    user,
                                                    finalState,
                                                    defaultState,
                                                    decision.Protocol.Id,
                                                    "gkh_real_obj_dec_protocol",
                                                    "Смена статуса в связи с импортом нового протокола");
                                            }
                                        }

                                        roOwnerDecisions.Add(new DecisionProtocolInfo
                                        {
                                            Id = newProtocol.Id,
                                            ProtocolDate = newProtocol.ProtocolDate,
                                            RealityObjectId = newProtocol.RealityObject.Id,
                                            InFinalState = true,
                                            Protocol = newProtocol
                                        });
                                    }
                                }

                                var entity = new UltimateDecisionProxy
                                {
                                    Protocol = newProtocol,
                                    AccountManagementDecision = new AccountManagementDecision
                                    {
                                        Decision = (AccountManagementType)entry.mkdperson.ToInt()
                                    },
                                    CrFundFormationDecision = new CrFundFormationDecision
                                    {
                                        Decision = (CrFundFormationDecisionType)entry.CrFundFormationType.ToInt(),
                                        IsChecked = true
                                    },
                                    AccountOwnerDecision = new AccountOwnerDecision
                                    {
                                        DecisionType = (AccountOwnerDecisionType)entry.accountowner.ToInt(),
                                        IsChecked = (AccountOwnerDecisionType)entry.accountowner.ToInt() == AccountOwnerDecisionType.Custom
                                    },
                                    MonthlyFeeAmountDecision = new MonthlyFeeAmountDecision
                                    {
                                        IsChecked = true,
                                        Decision = new List<PeriodMonthlyFee>
                                    {
                                        new PeriodMonthlyFee
                                        {
                                            Value = entry.monthlypayment.ToDecimal(),
                                            From = entry.datefrom.ToDateTime(),
                                            To = this.toNullableDateTime(entry.dateto)
                                        }
                                    }
                                    },
                                    CreditOrgDecision = new CreditOrgDecision
                                    {
                                        Decision = creditOrgs.FirstOrDefault(item => item.Name == entry.creditorg)
                                    },
                                    MinFundAmountDecision = new MinFundAmountDecision
                                    {
                                        Decision = entry.minCrFundPercent.ToDecimal()
                                    },
                                    AccumulationTransferDecision = new AccumulationTransferDecision
                                    {
                                        Decision = entry.transferamount.ToDecimal()
                                    }
                                };

                                var year = DateTime.Now.Year;
                                entity.JobYearDecision = new JobYearDecision
                                {
                                    JobYears = new List<RealtyJobYear>
                                    {
                                        new RealtyJobYear
                                        {
                                            PlanYear = year,
                                            Job = jobs.Get(entry.work1)
                                        },
                                        new RealtyJobYear
                                        {
                                            PlanYear = year,
                                            Job = jobs.Get(entry.work2)
                                        },
                                        new RealtyJobYear
                                        {
                                            PlanYear = year,
                                            Job = jobs.Get(entry.work3)
                                        },
                                        new RealtyJobYear
                                        {
                                            PlanYear = year,
                                            Job = jobs.Get(entry.work4)
                                        },
                                        new RealtyJobYear
                                        {
                                            PlanYear = year,
                                            Job = jobs.Get(entry.work5)
                                        },
                                        new RealtyJobYear
                                        {
                                            PlanYear = year,
                                            Job = jobs.Get(entry.work6)
                                        },
                                        new RealtyJobYear
                                        {
                                            PlanYear = year,
                                            Job = jobs.Get(entry.work7)
                                        },
                                        new RealtyJobYear
                                        {
                                            PlanYear = year,
                                            Job = jobs.Get(entry.work8)
                                        },
                                        new RealtyJobYear
                                        {
                                            PlanYear = year,
                                            Job = jobs.Get(entry.work9)
                                        },
                                        new RealtyJobYear
                                        {
                                            PlanYear = year,
                                            Job = jobs.Get(entry.work10)
                                        },
                                        new RealtyJobYear
                                        {
                                            PlanYear = year,
                                            Job = jobs.Get(entry.work11)
                                        }
                                    }
                                };

                                this.SaveOwnersDecision(entity);
                                this.SaveStateHistory(
                                    user,
                                    finalState,
                                    defaultState,
                                    newProtocol.Id,
                                    "gkh_real_obj_dec_protocol",
                                    "Статус импортирован из файла");

                                this.LogImport.CountAddedRows++;
                                this.LogImport.Info("Добавление", string.Format("Строка {0}. Добавлен протокол решений собственников", index));
                            }
                            else
                            {
                                var robject = this.TryGetRealityObject(entry);

                                if (robject == null)
                                {
                                    var description = string.Format(
                                        "Не удалось получить жилой дом {0} ул.{1} д.{2}{3} {4} {5}",
                                        entry.City,
                                        entry.Street,
                                        entry.HouseNum,
                                        entry.Char,
                                        entry.CorpNum,
                                        entry.Building)
                                        .TrimEnd();
                                    this.LogImport.Error("Строка " + index, description);
                                    continue;
                                }

                                var protocolDate = entry.date_protocol.ToDateTime();
                                var entity = new GovDecision
                                {
                                    AuthorizedPerson = entry.FIO,
                                    AuthorizedPersonPhone = entry.phone,
                                    Destroy = entry.destroymkd.ToBool(),
                                    DestroyDate = entry.destroydate.ToDateTime(),
                                    FundFormationByRegop = entry.CrFundFormationType.ToInt() == (int)CrFundFormationDecisionType.RegOpAccount,
                                    MaxFund = entry.govMaxCrFundAmount.ToDecimal(),
                                    ProtocolDate = entry.date_protocol.ToDateTime(),
                                    DateStart = entry.date_entry_protocol.ToDateTime(),
                                    ProtocolNumber = entry.protocolnum,
                                    RealityObject = robject,
                                    RealtyManagement = entry.mkdperson,
                                    Reconstruction = entry.reconstructionmkd.ToBool(),
                                    ReconstructionStart = entry.govdatefrom.ToDateTime(),
                                    ReconstructionEnd = entry.govdateto.ToDateTime(),
                                    State = govFinalState,
                                    TakeApartsForGov = entry.withdrawflat.ToBool(),
                                    TakeApartsForGovDate = entry.withdrawflatdate.ToDateTime(),
                                    TakeLandForGov = entry.withdrawarea.ToBool(),
                                    TakeLandForGovDate = entry.withdrawareadate.ToDateTime()
                                };

                                if (this.govDecisions.ContainsKey(robject.Id))
                                {
                                    var roGovDecisions = this.govDecisions[robject.Id];
                                    if (roGovDecisions.Any(item => item.ProtocolDate == protocolDate && entity.ProtocolNumber == item.GovProtocol.ProtocolNumber))
                                    {
                                        var description =
                                            "Для объекта {0} протокол решения с датой {1:yyyy-MM-dd} уже существует".FormatUsing(
                                                robject.Address,
                                                protocolDate);
                                        this.LogImport.Info("Строка " + index, description);

                                        this.LogImport.CountChangedRows++;
                                        continue;
                                    }

                                    if (transferToDefaultState)
                                    {
                                        foreach (var decision in roGovDecisions)
                                        {
                                            if (decision.InFinalState)
                                            {
                                                decision.GovProtocol.State = defaultState;
                                                decision.InFinalState = false;

                                                this.RoGovDecisionRepository.Update(decision.GovProtocol);
                                                this.SaveStateHistory(
                                                    user,
                                                    finalState,
                                                    defaultState,
                                                    decision.GovProtocol.Id,
                                                    "gkh_real_obj_gov_dec",
                                                    "Смена статуса в связи с импортом нового протокола");

                                            }
                                        }

                                        roGovDecisions.Add(new DecisionProtocolInfo
                                        {
                                            Id = entity.Id,
                                            ProtocolDate = entity.ProtocolDate,
                                            RealityObjectId = entity.RealityObject.Id,
                                            InFinalState = true,
                                            GovProtocol = entity
                                        });
                                    }
                                }
                                
                                govDecisionDomain.Save(entity);
                                this.SaveStateHistory(
                                    user,
                                    finalState,
                                    defaultState,
                                    entity.Id,
                                    "gkh_real_obj_gov_dec",
                                    "Статус импортирован из файла");

                                this.LogImport.CountAddedRows++;
                                this.LogImport.Info("Добавление", string.Format("Строка {0}. Добавлен протокол решений органов гос. власти", index));
                            }
                        }

                        tr.Commit();
                    }
                    catch (Exception ex)
                    {
                        tr.Rollback();
                        this.LogImport.Error("Ошибка сохранения. ", ex.Message);
                        throw;
                    }
                    finally
                    {
                        this.Container.Release(tr);
                    }
                }
            }

            this.LogImport.ImportKey = this.Key;
            this.LogImportManager.Add(file, this.LogImport);
            this.LogImportManager.FileNameWithoutExtention = file.FileName;
            this.LogImportManager.Save();

            var statusImport = this.LogImport.CountError > 0
                ? StatusImport.CompletedWithError
                : this.LogImport.CountWarning > 0
                    ? StatusImport.CompletedWithWarning
                    : StatusImport.CompletedWithoutError;

            return new ImportResult(
                statusImport,
                string.Format("Импортировано {0} записей", this.LogImport.CountAddedRows + this.LogImport.CountError + this.LogImport.CountChangedRows),
                string.Empty, this.LogImportManager.LogFileId);
        }

        #endregion

        #region private methods

        private void SaveOwnersDecision(UltimateDecisionProxy proxy)
        {
            var properties = proxy.GetType().GetProperties();

            dynamic protocol = null;
            var protocolProperty = properties.FirstOrDefault(x => typeof(RealityObjectDecisionProtocol) == x.PropertyType);

            if (protocolProperty != null)
            {
                properties = properties.Except(new[] { protocolProperty }).ToArray();
                protocol = protocolProperty.GetValue(proxy, new object[0]);

                if (protocol != null)
                {
                    var domain = this.Container.ResolveRepository<RealityObjectDecisionProtocol>();

                    using (this.Container.Using(domain))
                    {
                        domain.Save(protocol);
                    }
                }
            }

            foreach (var property in properties)
            {
                var value = property.GetValue(proxy, new object[0]);

                if (value == null)
                {
                    continue;
                }

                var protocolProp =
                    value.GetType()
                        .GetProperties()
                        .FirstOrDefault(x => typeof(RealityObjectDecisionProtocol) == x.PropertyType);

                if (protocolProp != null)
                {
                    protocolProp.SetValue(value, protocol, new object[0]);
                }

                if (!typeof(IEntity).IsAssignableFrom(property.PropertyType))
                {
                    continue;
                }

                var domainType = typeof(IDomainService<>).MakeGenericType(property.PropertyType);
                var domain = this.Container.Resolve(domainType);

                using (this.Container.Using(domain))
                {
                    var save = domain.GetType().GetMethod("Save", new[] { property.PropertyType });
                    save.Invoke(domain, new[] { value });
                }
            }
        }

        private void SaveStateHistory(User user, State newState, State oldState, long entityId, string typeId, string desc)
        {
            var stateHistory = new StateHistory
            {
                EntityId = entityId,
                StartState = newState,
                FinalState = oldState,
                UserLogin = user != null ? user.Login : "",
                UserName = user != null ? user.Name : "",
                TypeId = typeId,
                ChangeDate = DateTime.Now,
                Description = desc
            };

            this.StateHistoryRepository.Save(stateHistory);
        }

        private DateTime? toNullableDateTime(string dtString)
        {
            if (dtString.IsNotEmpty())
            {
                return dtString.ToDateTime();
            }
            else
            {
                return null;
            }
        }
        private RealityObject TryGetRealityObject(DecisionProtocolRow row)
        {
            //если в файле указан RoId, то вначале ищем по нему
            if (row.RoId > 0 && this.robjectByRoIdCache.ContainsKey(row.RoId))
            {
                return this.robjectByRoIdCache[row.RoId].Return(x => x.Robject);
            }

            //если в файлике указан индекс, и в системе существует единственная запись с таким кодом - берем его
            if (!row.HouseIndex.IsNullOrWhiteSpace()
                && this.robjectByCodeCache.ContainsKey(row.HouseIndex)
                && this.robjectByCodeCache[row.HouseIndex].Count == 1)
            {
                return this.robjectByCodeCache[row.HouseIndex].Return(x => x.Robject);
            }

            var addressKey = string.Format("{0}#{1}#{2}#{3}#{4}#{5}#{6}#{7}", this.ToLowerTrim(row.Municipality), this.ToLowerTrim(row.MU),
                "{0}_{1}".FormatUsing(this.ToLowerTrim(row.KindCity), this.ToLowerTrim(row.City)),
                "{0}_{1}".FormatUsing(this.ToLowerTrim(row.KindStreet), this.ToLowerTrim(row.Street)), this.ToLowerTrim(row.HouseNum), this.ToLowerTrim(row.CorpNum), this.ToLowerTrim(row.Char), this.ToLowerTrim(row.Building));

            if (this.robjectByAddressCache.ContainsKey(addressKey)
                && this.robjectByAddressCache[addressKey].Count == 1)
            {
                return this.robjectByAddressCache[addressKey].Robject;
            }

            return null;
        }

        #endregion 

        #region cache

        private sealed class DecisionProtocolInfo
        {
            public long Id { get; set; }

            public long RealityObjectId { get; set; }

            public DateTime? ProtocolDate { get; set; }

            public bool InFinalState { get; set; }

            public RealityObjectDecisionProtocol Protocol { get; set; }

            public GovDecision GovProtocol { get; set; }
        }

        private void InitCache()
        {
            var roRep = this.Container.ResolveRepository<RealityObject>();

            this.robjectByCodeCache = roRep.GetAll()
                .Where(x => x.GkhCode != null)
                .AsEnumerable()
                .Where(x => !x.GkhCode.IsNullOrWhiteSpace())
                .GroupBy(x => x.GkhCode)
                .ToDictionary(x => x.Key, y => new RobjectProxy
                {
                    Count = y.Count(),
                    Robject = y.First()
                });

            this.robjectByRoIdCache = roRep.GetAll()
                .Select(
                    x => new
                    {
                        x.Id,
                        x.Address
                    })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(
                    x => x.Key,
                    y => new RobjectProxy
                    {
                        Count = y.Count(),
                        Robject = new RealityObject {Id = y.First().Id, Address = y.First().Address}
                    });

            var fiasCache = this.Container.ResolveRepository<Fias>().GetAll()
                .Select(x => new
                {
                    x.AOGuid,
                    x.ShortName,
                    x.FormalName,
                    x.ActStatus,
                    x.OffName
                })
                .Where(x => x.ActStatus == FiasActualStatusEnum.Actual)
                .ToList()
                .GroupBy(x => x.AOGuid)
                .ToDictionary(x => x.Key, y => y.First());

            this.ownerDecisions = this.Container.ResolveRepository<RealityObjectDecisionProtocol>().GetAll()
                .Select(
                    item => new DecisionProtocolInfo
                    {
                        Id = item.Id,
                        ProtocolDate = item.ProtocolDate,
                        RealityObjectId = item.RealityObject.Id,
                        InFinalState = item.State != null && item.State.FinalState,
                        Protocol = item
                    })
                    .ToList()
                    .GroupBy(item => item.RealityObjectId)
                    .ToDictionary(item => item.Key, item => item.ToList());

            this.govDecisions = this.Container.ResolveRepository<GovDecision>().GetAll()
                .Select(
                    item => new DecisionProtocolInfo
                    {
                        Id = item.Id,
                        ProtocolDate = item.ProtocolDate,
                        RealityObjectId = item.RealityObject.Id,
                        InFinalState = item.State != null && item.State.FinalState,
                        GovProtocol = item
                    })
                .ToList()
                .GroupBy(item => item.RealityObjectId)
                .ToDictionary(item => item.Key, item => item.ToList());

            this.robjectByAddressCache = roRep.GetAll()
                .Where(x => x.FiasAddress != null)
                .Select(x => new
                {
                    RoId = x.Id,
                    Mr = x.Municipality.Name.ToLower().Trim(),
                    Mu = x.MoSettlement.Name.ToLower().Trim(),
                    Address = x.Address,
                    x.FiasAddress.PlaceGuidId,
                    x.FiasAddress.StreetGuidId,
                    House = x.FiasAddress.House.ToLower().Trim(),
                    Housing = x.FiasAddress.Housing.ToLower().Trim(),
                    Letter = x.FiasAddress.Letter.ToLower().Trim(),
                    Building = x.FiasAddress.Building.ToLower().Trim()
                })
                .AsEnumerable()
                .Select(x => new
                {
                    Key = string.Format("{0}#{1}#{2}#{3}#{4}#{5}#{6}#{7}",
                        x.Mr,
                        x.Mu,
                        x.PlaceGuidId.IsEmpty()
                            ? string.Empty
                            : fiasCache.Get(x.PlaceGuidId)
                                .Return(y =>
                                    "{0}_{1}".FormatUsing(this.ToLowerTrim(y.ShortName), this.ToLowerTrim(y.OffName))),
                        x.StreetGuidId.IsEmpty()
                            ? "_"
                            : fiasCache.Get(x.StreetGuidId)
                                .Return(y =>
                                    "{0}_{1}".FormatUsing(this.ToLowerTrim(y.ShortName), this.ToLowerTrim(y.OffName))),
                        x.House,
                        x.Housing,
                        x.Letter,
                        x.Building),
                    x.RoId,
                    x.Address
                })
                .GroupBy(x => x.Key)
                .ToDictionary(x => x.Key, y => new RobjectProxy
                {
                    Count = y.Count(),
                    Robject = new RealityObject {Id = y.First().RoId, Address = y.First().Address}
                });
        }

        private Dictionary<string, RobjectProxy> robjectByCodeCache;
        private Dictionary<long, RobjectProxy> robjectByRoIdCache;
        private Dictionary<string,  RobjectProxy> robjectByAddressCache;

        private Dictionary<long, List<DecisionProtocolInfo>> ownerDecisions;

        private Dictionary<long, List<DecisionProtocolInfo>> govDecisions;
        
        private string ToLowerTrim(string value)
        {
            return (value ?? "").ToLower().Trim(' ', '.');
        }

        #endregion cache

    }
}