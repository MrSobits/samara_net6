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
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.Gkh.RegOperator.Domain.ImportExport;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;
    using Castle.Windsor;
    using Import.Impl;

    internal class PersonalAccountChargeImportCsv : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        #region IGkhImport Properties
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
            get { return "Импорт начислений csv"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "csv"; }
        }

        public override string PermissionName
        {
            get { return "GkhRegOp.PersonalAccount.Import.PersonalAccountChargeImportCsv"; }
        }
        #endregion

        private readonly IWindsorContainer container;
        private Dictionary<string, long> personalAccountsDict;

        // private AutoResetEvent _cacheEvent;


        // public IAppCache AppCache { get; set; }

        public IDomainService<UnacceptedChargePacket> PacketDomain { get; set; }

        public IDomainService<BasePersonalAccount> BasePersonalAccountDomainService { get; set; } 

        public PersonalAccountChargeImportCsv(IWindsorContainer container, ILogImportManager logManager, ILogImport logImport)
        {
            this.LogImportManager = logManager;
            this.LogImport = logImport;
            this.container = container;
        }

        private void WarmCache()
        {
            personalAccountsDict = BasePersonalAccountDomainService.GetAll().Select(x => new { x.Id, x.PersonalAccountNum }).ToDictionary(x => x.PersonalAccountNum, x => x.Id);

            /*AppCache.RegisterInvalidator<PersonalAccountDto>(new CustomPersistentInvalidationStrategy<BasePersonalAccount, long>(_container, AppCache));
            var holder = _container.Resolve<IKeyGeneratorHolder>();
            holder.Add(new SimpleKeyGenerator<BasePersonalAccount>(x => x.PersonalAccountNum));*/

            // _cacheEvent = new AutoResetEvent(false);

            // AppCache.FlushAll();

            // AppCache.InvalidationComplete += () => _cacheEvent.Set();

            // AppCache.Invalidate();
        }

        public override ImportResult Import(BaseParams baseParams)
        {
            var fileData = baseParams.Files["FileImport"];

            InitLog(fileData.FileName);
            WarmCache();

            // _cacheEvent.WaitOne();

            var importRows = new List<ImportRow<ChargeProxy>>();
            using (var reader = new StreamReader(new MemoryStream(fileData.Data)))
            {
                while (reader.Peek() >= 0)
                {
                    importRows.Add(ChargeProxy.FromDataString(reader.ReadLine()));
                }
            }

            var result = new ImportResult(StatusImport.CompletedWithoutError);
            var unacceptedCharges = new List<UnacceptedCharge>();
            var packet = new UnacceptedChargePacket { CreateDate = DateTime.Now, PacketState = PaymentOrChargePacketState.Pending, UserName = FindOutUserName() };

            // var accountClient = AppCache.GetClient<BasePersonalAccount>();

            foreach (var row in importRows)
            {
                if (!string.IsNullOrEmpty(row.Warning))
                {
                    result.StatusImport = result.StatusImport < StatusImport.CompletedWithWarning ?
                        StatusImport.CompletedWithWarning : result.StatusImport;
                    this.LogImport.Warn(row.Value.DataString, row.Warning);
                }

                if (!string.IsNullOrEmpty(row.Error))
                {
                    result.StatusImport = StatusImport.CompletedWithError;
                    this.LogImport.Error(row.Value.DataString, row.Error);
                    continue;
                }

                var accountId = personalAccountsDict.Get(row.Value.AccontNumber);
                if (accountId == 0)
                {
                    result.StatusImport = StatusImport.CompletedWithError;
                    this.LogImport.Error(row.Value.DataString, string.Format("Не найден ЛС с номером {0}", row.Value.AccontNumber));
                    continue;
                }

                unacceptedCharges.Add(row.Value.ToUnacceptedCharge(new BasePersonalAccount { Id = accountId }, packet));
            }

            try
            {
                if (unacceptedCharges.Count > 0)
                {
                    PacketDomain.Save(packet);
                    TransactionHelper.InsertInManyTransactions(this.container, unacceptedCharges, 10000);
                }
            }
            catch
            {
                if (packet.Id > 0)
                {
                    PacketDomain.Delete(packet.Id);
                }

                throw;
            }

            ReleaseLog(fileData);

            return result;
        }

        public override bool Validate(BaseParams baseParams, out string message)
        {
            message = string.Empty;
            if (!baseParams.Files.ContainsKey("FileImport"))
            {
                message = "Не выбран файл для импорта";
                return false;
            }

            return true;
        }

        private string FindOutUserName()
        {
            var userName = AnonymousUserIdentity.Identity.Name;
            var currentUserId = 0L;
            var currentUser = this.container.Resolve<IUserIdentity>();
            if (!(currentUser is AnonymousUserIdentity))
            {
                currentUserId = currentUser.UserId;
            }

            if (currentUserId > 0)
            {
                var curOp =
                    this.container.ResolveDomain<Operator>().GetAll()
                        .FirstOrDefault(x => x.User.Id == currentUserId);

                if (curOp != null && curOp.User != null)
                {
                    userName = curOp.User.Name;
                }
            }
            return userName;
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

        private void ReleaseLog(FileData file)
        {
            this.LogImportManager.FileNameWithoutExtention = file.FileName;
            this.LogImportManager.UploadDate = DateTime.Now;

            this.LogImport.SetFileName(file.FileName);
            this.LogImport.ImportKey = Key;

            this.LogImportManager.Add(file, this.LogImport);
            this.LogImportManager.Save();
        }

        private void SaveCharge(ChargeProxy chargeProxy)
        {
            // TODO сохранение начислений
            this.LogImport.CountAddedRows++;
            this.LogImport.Info(chargeProxy.DataString, "Успешно импортировано.");
        }

        private class ChargeProxy
        {
            private ChargeProxy()
            {

            }

            private readonly Dictionary<string, int> _monthIndexByName = new Dictionary<string, int>
            {
                {"январь", 1},
                {"февраль", 2},
                {"март", 3},
                {"апрель", 4},
                {"май", 5},
                {"июнь", 6},
                {"июль", 7},
                {"август", 8},
                {"сентябрь", 9},
                {"октябрь", 10},
                {"ноябрь", 11},
                {"декабрь", 12}
            };

            public static ImportRow<ChargeProxy> FromDataString(string dataString)
            {
                ArgumentChecker.NotNull(dataString, "dataString");
                var data = dataString.Split(';');
                var row = new ImportRow<ChargeProxy>();

                var proxy = new ChargeProxy
                {
                    DataString = dataString,
                    AccountOwnerNumber = data[0].Replace("\"", string.Empty),
                    AccontNumber = data[1].Replace("\"", string.Empty),
                    Order = data[2].Replace("\"", string.Empty),
                    MonthYear = data[3].Replace("\"", string.Empty),
                    ZipCode = data[4].Replace("\"", string.Empty),
                    Address = data[5].Replace("\"", string.Empty),
                    Tariff = data[6].ToDecimal(),
                    Charge = data[7].ToDecimal(),
                    Paid = data[8].ToDecimal(),
                    Discount = data[9].ToDecimal(),
                    Recalc = data[10].ToDecimal(),
                    Penalty = data[11].ToDecimal(),
                    Payment = data[12].ToDecimal(),
                    NormativPaymentDate = data[13].ToDateTime(),
                    RecalcPeriodStart = data[14].ToDateTime(),
                    RecalcPeriodEnd = data[15].ToDateTime(),
                    Area = data[16].ToDecimal(),
                    SaldoIn = data[17].ToDecimal(),
                    Accounted = data[18].ToDecimal(),
                    SaldoOut = data[19].ToDecimal(),
                    PenaltyOut = data[20].ToDecimal()
                };

                row.Value = proxy;

                Validate(row);
                return row;
            }

            private static void Validate(ImportRow<ChargeProxy> importRow)
            {
                var proxy = importRow.Value;
                var error = new StringBuilder();

                if (string.IsNullOrEmpty(proxy.AccontNumber))
                {
                    error.Append("Отсутсвует номер ЛС; ");
                }

                if (string.IsNullOrEmpty(proxy.MonthYear) || proxy.Month < 0 || proxy.Year < 0)
                {
                    error.Append("Некорректное данные в поле Месяц и год; ");
                }

                if (proxy.Charge <= 0)
                {
                    error.Append("Некорректная сумма начисления; ");
                }

                importRow.Error = error.ToString();
            }

            public UnacceptedCharge ToUnacceptedCharge(BasePersonalAccount account, UnacceptedChargePacket packet)
            {
                return new UnacceptedCharge(account, packet)
                {
                    ChargeTariff = Charge,
                    RecalcByBaseTariff = Recalc,
                    Penalty = Penalty
                };
            }

            /// <summary>
            /// Имортируемая строка
            /// </summary>
            public string DataString { get; protected set; }

            /// <summary>
            /// Номер абонента в ИСУСЭ БП
            /// </summary>
            public string AccountOwnerNumber { get; protected set; }

            /// <summary>
            /// Номер лицевого счета ФКР (обязательное)
            /// </summary>
            public string AccontNumber { get; protected set; }

            /// <summary>
            /// Порядковый номер счета
            /// </summary>
            public string Order { get; protected set; }


            /// <summary>
            /// Месяц и год (обязательное) в формате "Ноябрь 2014"
            /// </summary>
            public string MonthYear { get; set; }

            /// <summary>
            /// Порядковый номер месяца (нумерация с 1) или -1, если неизвестно
            /// </summary>
            public int Month
            {
                get
                {
                    var monthName = MonthYear.Substring(0, MonthYear.IndexOf(' '));
                    return _monthIndexByName.ContainsKey(monthName.ToLowerInvariant())
                        ? _monthIndexByName[monthName.ToLowerInvariant()]
                        : -1;
                }
            }

            /// <summary>
            /// Год
            /// </summary>
            public int Year
            {
                get
                {
                    var yearStr = MonthYear.Substring(MonthYear.IndexOf(' '));
                    int year;
                    return int.TryParse(yearStr, out year) ? year : -1;
                }
            }

            /// <summary>
            /// Индекс
            /// </summary>
            public string ZipCode { get; protected set; }

            /// <summary>
            /// Адрес
            /// </summary>
            public string Address { get; protected set; }

            /// <summary>
            /// Тариф
            /// </summary>
            public decimal Tariff { get; protected set; }

            /// <summary>
            /// Начислено (обязательное)
            /// </summary>
            public decimal Charge { get; protected set; }

            /// <summary>
            /// Оплачено ранее
            /// </summary>
            public decimal Paid { get; protected set; }

            /// <summary>
            /// Скидка по льготе
            /// </summary>
            public decimal Discount { get; protected set; }

            /// <summary>
            /// Перерасчет
            /// </summary>
            public decimal Recalc { get; protected set; }

            /// <summary>
            /// Пени
            /// </summary>
            public decimal Penalty { get; protected set; }

            /// <summary>
            /// Сумма к оплате
            /// </summary>
            public decimal Payment { get; protected set; }

            /// <summary>
            /// Нормативная дата оплаты
            /// </summary>
            public DateTime NormativPaymentDate { get; protected set; }

            /// <summary>
            /// Период перерасчета от
            /// </summary>
            public DateTime RecalcPeriodStart { get; protected set; }

            /// <summary>
            /// Период перерасчета до
            /// </summary>
            public DateTime RecalcPeriodEnd { get; protected set; }

            /// <summary>
            /// Площадь
            /// </summary>
            public decimal Area { get; protected set; }

            /// <summary>
            /// Не оплачено на начало периода
            /// </summary>
            public decimal SaldoIn { get; protected set; }

            /// <summary>
            /// Учтено
            /// </summary>
            public decimal Accounted { get; protected set; }

            /// <summary>
            /// Долг на конец периода
            /// </summary>
            public decimal SaldoOut { get; protected set; }

            /// <summary>
            /// Долг по пени
            /// </summary>
            public decimal PenaltyOut { get; protected set; }
        }

        private class PersonalAccountDto : IEntity
        {
            object IEntity.Id { get { return Id; } set { Id = (long)value; } }

            public long Id { get; set; }

            public string AccountNumber { get; set; }
        }

        //private class PersonalAccountStrategy : InvalidationStrategy<BasePersonalAccount, PersonalAccountDto, long>
        //{
        //    public PersonalAccountStrategy(IWindsorContainer container, IAppCache appCache) : base(container, appCache)
        //    {
        //    }

        //    public override IQueryable GetQuery(IQueryable<BasePersonalAccount> query)
        //    {
        //        return query.Select(x => new PersonalAccountDto()
        //        {
        //            Id = x.Id,
        //            AccountNumber = x.PersonalAccountNum
        //        });
        //    }
        //}
    }

    //public abstract class InvalidationStrategy<TEntity, TDto, TKey> : IInvalidationStrategy
    //    where TEntity : IEntity
    //    where TDto : class, IEntity, new()
    //{
    //    private readonly IWindsorContainer _container;
    //    private ICacheClient<TDto> _cache;

    //    public InvalidationStrategy(IWindsorContainer container, IAppCache appCache, Expression<Func<TEntity, TDto>> select)
    //    {
    //        _container = container;
    //        _cache = appCache.GetInvalidationClient<TDto>();
    //    }

    //    public bool Invalidate()
    //    {
    //        var scope = ExplicitSessionScope.EnterScope(new DefaultLifetimeScope());

    //        var sw = Stopwatch.StartNew();
    //        //var name = typeof(TValue).Name;
    //        //var fName = typeof(TValue).FullName;

    //        //_logger.LogInformation("Кэш:({0}) Прогрев кэша для {1}".FormatUsing(name, fName));
    //        var repo = _container.ResolveRepository<TEntity>();
    //        var sessions = _container.Resolve<ISessionProvider>();

    //        try
    //        {
    //            //var cachedKeys = _cache.GetAllKeys().ToList().Select(x => x.To<TKey>()).ToList();
    //            var persistentKeys = repo.GetAll().Select(x => x.Id).ToList().Cast<TKey>().ToList();

    //            //var cacheExceptDb = cachedKeys.Except(persistentKeys).ToList();
    //            //var dbExceptCache = persistentKeys.Except(cachedKeys.ToList());

    //            //// 1) Delete all excess _cache
    //            //_cache.RemoveAll(_cache.GetAll(cacheExceptDb.Cast<object>()));

    //            //// 2) Get new data from db and put to _cache
    //            var objectKeys = persistentKeys.ToList();

    //            //_logger.LogInformation("Кэш:({0}) К заполнению {1}".FormatUsing(name, objectKeys.Count));
    //            var counter = 0;
    //            foreach (var partKeys in objectKeys.Split(1000))
    //            {
    //                var fromDb = GetQuery(repo.GetAll().Where(x => partKeys.Contains((TKey)x.Id))).ToList().Select();
    //                _cache.AddMany(fromDb, true);

    //                counter += partKeys.Count();

    //                //_logger.LogInformation("Кэш:({0}) Заполнено {1} из {2}".FormatUsing(name, counter, objectKeys.Count));

    //                if (counter % 10000 == 0)
    //                {
    //                    sessions.CloseCurrentSession();
    //                }
    //            }

    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            //_logger.ErrorFormat(ex, "Кэш:({0}). Ошибка при обновлении кэша.", name);
    //            throw;
    //        }
    //        finally
    //        {
    //            sw.Stop();
    //            //_logger.LogInformation("Кэш:({0}) Кэш прогрет за {1}".FormatUsing(name, sw.Elapsed));
    //            _container.Release(repo);

    //            sessions.CloseCurrentSession();
    //            _container.Release(sessions);

    //            ExplicitSessionScope.LeaveScope(scope);
    //        }
    //    }

    //    public abstract IQueryable GetQuery(IQueryable<TEntity> query);

    //    public bool CanInvalidate(object item)
    //    {
    //        return false;
    //    }

    //    public void InvalidateEntry(object item, EntryUpdateType operation)
    //    {
    //    }
    //}
}
