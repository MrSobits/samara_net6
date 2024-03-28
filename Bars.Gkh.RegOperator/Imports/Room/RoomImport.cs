namespace Bars.Gkh.RegOperator.Imports.Room
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using B4.Modules.Tasks.Common.Service;
    using B4.Modules.Tasks.Common.Utils;
    using Bars.B4;
    using Bars.B4.Config;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Microsoft.Extensions.Logging;
    using Bars.B4.Modules.Caching.Impl;
    using Bars.B4.Modules.Caching.Interfaces;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Modules.Security;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Domain;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Domain.ParameterVersioning;
    using Bars.Gkh.Domain.TableLocker;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.Gkh.RegOperator.Caching;
    using Bars.Gkh.RegOperator.Domain.ParametersVersioning;
    using Bars.Gkh.RegOperator.Domain.Repository;
    using Bars.Gkh.RegOperator.DomainService.Period;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Wallet;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Extenstions;
    using Bars.Gkh.Repositories;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.Gkh.Utils;
    using Bars.GkhExcel;
    using FastMember;
    using Import.Impl;
    using NHibernate;
    using ServiceStack.Redis;
    using ServiceStack.Text;
    using ExecutionContext = B4.Modules.Tasks.Common.Contracts.ExecutionContext;
    /// <summary>
    /// Room import
    /// </summary>
    public partial class RoomImport : GkhImportBase, IDisposable
    {
        private CultureInfo culture;
        private Dictionary<string, int> headersDict;
        private bool replaceExistRooms;
        private ChargePeriod firstPeriod;
        private ChargePeriod curPeriod;
        private bool roomNumNormalizationDisabled;
        private bool discriminateAccountsByNum;

        /// <summary>
        /// Id
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        #region Property injections
        /// <summary>
        /// RoomsDomain
        /// </summary>
        public IDomainService<Room> RoomsDomain { get; set; }

        /// <summary>
        /// SessionProvider
        /// </summary>
        public ISessionProvider SessionProvider { get; set; }

        /// <summary>
        /// RealityObjectDomain
        /// </summary>
        public IRepository<RealityObject> RealityObjectDomain { get; set; }

        /// <summary>
        /// BasePersonalAccountDomain
        /// </summary>
        public IDomainService<BasePersonalAccount> BasePersonalAccountDomain { get; set; }

        /// <summary>
        /// IndividualAccountOwnerDomain
        /// </summary>
        public IDomainService<IndividualAccountOwner> IndividualAccountOwnerDomain { get; set; }

        /// <summary>
        /// LegalAccountOwnerDomain
        /// </summary>
        public IDomainService<LegalAccountOwner> LegalAccountOwnerDomain { get; set; }

        /// <summary>
        /// ContragentDomain
        /// </summary>
        public IDomainService<Contragent> ContragentDomain { get; set; }

        /// <summary>
        /// FiasDomain
        /// </summary>
        public IRepository<Fias> FiasDomain { get; set; }

        /// <summary>
        /// FiasAddressDomain
        /// </summary>
        public IDomainService<FiasAddress> FiasAddressDomain { get; set; }

        /// <summary>
        /// CashPaymentCenterPersAccDomain
        /// </summary>
        public IDomainService<CashPaymentCenterPersAcc> CashPaymentCenterPersAccDomain { get; set; }

        /// <summary>
        /// CashPaymentCenterDomain
        /// </summary>
        public IDomainService<CashPaymentCenter> CashPaymentCenterDomain { get; set; }

        /// <summary>
        /// EntityLogLightDomain
        /// </summary>
        public IDomainService<EntityLogLight> EntityLogLightDomain { get; set; }

        /// <summary>
        /// UserRepo
        /// </summary>
        public IRepository<User> UserRepo { get; set; }

        /// <summary>
        /// Identity
        /// </summary>
        public IUserIdentity Identity { get; set; }

        /// <summary>
        /// SubPersonalAccountService
        /// </summary>
        public ISubPersonalAccountService SubPersonalAccountService { get; set; }

        /// <summary>
        /// ChargePeriodService
        /// </summary>
        public IChargePeriodService ChargePeriodService { get; set; }

        /// <summary>
        /// ChargePeriodRepo
        /// </summary>
        public IChargePeriodRepository ChargePeriodRepo { get; set; }

        /// <summary>
        /// RoDecisionsService
        /// </summary>
        public IRealityObjectDecisionsService RoDecisionsService { get; set; }

        #endregion

        /// <summary>
        /// listRoomToSave
        /// </summary>
        public Dictionary<long, HashSet<string>> ListRoomToSave = new Dictionary<long, HashSet<string>>();

        /// <summary>
        /// listContragentToSave
        /// </summary>
        public List<Contragent> ListContragentToSave = new List<Contragent>();

        /// <summary>
        /// listBasePersonalAccountToSave
        /// </summary>
        public HashSet<BasePersonalAccount> ListBasePersonalAccountToSave = new HashSet<BasePersonalAccount>();

        /// <summary>
        /// roomLogToCreateDict
        /// </summary>
        public Dictionary<Room, ProxyLogValue> RoomLogToCreateDict = new Dictionary<Room, ProxyLogValue>();

        /// <summary>
        /// accountAreaShareLogToCreateDict
        /// </summary>
        public Dictionary<BasePersonalAccount, ProxyLogValue> AccountAreaShareLogToCreateDict = new Dictionary<BasePersonalAccount, ProxyLogValue>();

        /// <summary>
        /// accountOpenDateLogToCreateDict
        /// </summary>
        public Dictionary<BasePersonalAccount, ProxyLogValue> AccountOpenDateLogToCreateDict = new Dictionary<BasePersonalAccount, ProxyLogValue>();

        /// <summary>
        /// accountNumberService
        /// </summary>
        public IAccountNumberService AccountNumberService { get; set; }

        /// <summary>
        /// Сервис массовой работы с DTO KC
        /// </summary>
        public IMassPersonalAccountDtoService AccountDtoService { get; set; }

        /// <summary>
        /// Сервис абонентов
        /// </summary>
        public IPersonalAccountOwnerService PersonalAccountOwnerService { get; set; }

        #region Properties
        /// <summary>
        /// Key
        /// </summary>
        public override string Key { get { return RoomImport.Id; } }

        /// <summary>
        /// CodeImport
        /// </summary>
        public override string CodeImport { get { return "RoomImport"; } }

        /// <summary>
        /// Name
        /// </summary>
        public override string Name { get { return "Импорт абонентов"; } }

        /// <summary>
        /// PossibleFileExtensions
        /// </summary>
        public override string PossibleFileExtensions { get { return "xls,xlsx"; } }

        /// <summary>
        /// PermissionName
        /// </summary>
        public override string PermissionName { get { return "Import.RoomImport.View"; } }

        #endregion Properties

        #region Cache

        private State accountDefaultState;
        private State accountStartState;
        private HashSet<long> existRobjectIds;
        private List<FiasProxy> fiasRecords;
        private List<FiasAddressProxy> fiasAddresList;

        /// <summary>
        /// Сет содержит ключи вида адрес_помещения#имя собственника, для проверки существования счета
        /// </summary>
        private Dictionary<string, long> existAccount;

        #endregion Cache

        #region Dictionaries

        /// <summary>
        /// Служебный класс RealtyObjectInStreet
        /// </summary>
        private class RealtyObjectInStreet : IEquatable<RealtyObjectInStreet>
        {
            public long RoId { get; set; }

            public string House { get; set; }

            public string Letter { get; set; }

            public string Housing { get; set; }

            public string Building { get; set; }

            public string HouseLetter { get; set; }

            public bool Equals(RealtyObjectInStreet other)
            {
                if (Object.ReferenceEquals(other, null))
                {
                    return false;
                }

                if (Object.ReferenceEquals(this, other))
                {
                    return true;
                }

                return 
                    this.RoId == other.RoId && this.House == other.House && this.Letter == other.Letter && this.Housing == other.Housing && this.Building == other.Building && this.HouseLetter == other.HouseLetter;
            }

            public override int GetHashCode()
            {
                int hashRoId = this.RoId.GetHashCode();
                int hashHouse = this.House == null ? 0 : this.House.GetHashCode();
                int hashLetter = string.IsNullOrEmpty(this.Letter) ? 0 : this.Letter.GetHashCode();
                int hashHousing = string.IsNullOrEmpty(this.Housing) ? 0 : this.Housing.GetHashCode();
                int hashBuilding = string.IsNullOrEmpty(this.Building) ? 0 : this.Building.GetHashCode();
                int hashHouseLetter = string.IsNullOrEmpty(this.HouseLetter) ? 0 : this.HouseLetter.GetHashCode();
                return hashRoId ^ hashHouse ^ hashLetter ^ hashHousing ^ hashBuilding ^ hashHouseLetter;
            }
        }

        private readonly Dictionary<int, Dictionary<bool, List<string>>> logDict = new Dictionary<int, Dictionary<bool, List<string>>>();

        // 3 - уровневневый словарь
        // Список существующих домов сгруппированных по улице
        // => сгруппированных по населенному пункту
        // => сгруппированных по муниципальному образованию (первого уровня)
        private Dictionary<string, Dictionary<string, Dictionary<string, List<RealtyObjectInStreet>>>> realtyObjectsByAddressDict;
        private Dictionary<long, Dictionary<string, object>> realtyObjectRoomsDict;
        private Dictionary<string, object> individualOwnersDict;
        private Dictionary<long, object> legalOwnersDict;
        private Dictionary<string, Dictionary<string, object>> accountByRoomAndOwnerDict;
        private IQueryable<string> allExistingAccNumbers;
        private Dictionary<string, Dictionary<string, object>> accountByExternalNumDict;
        private Dictionary<string, ContragentInfoProxy> contragentsDict;
        private Dictionary<string, long> cashPaymentCenterIdDict;
        private Dictionary<long, List<CashPaymentCenterPersAccProxy>> cashPaymentCenterPersAccDict;
        private Dictionary<long, DateTime> roomAreaLastLogDict;
        private Dictionary<long, DateTime> accountOpenDateLastLogDict;
        private Dictionary<long, DateTime> accountAreaShareLastLogDict;
        private Dictionary<long, string> ownerKeysByAccountId;

        #endregion Dictionaries

        private List<Record> records = new List<Record>();

        private readonly List<EntityLogLight> entityLogLightListToCreate = new List<EntityLogLight>();
        private readonly List<CashPaymentCenterPersAcc> cashPayCenterPersAccToSave = new List<CashPaymentCenterPersAcc>();
        private readonly HashSet<long> roWithProtocolsInFinalState = new HashSet<long>();
        private readonly HashSet<long> roWithNoProtocolsInFinalState = new HashSet<long>(); 

        private TypeAccessor Accessor { get; set; }

        private PropertyInfo[] Props { get; set; }

        /// <summary>
        /// transaction
        /// </summary>
        public ITransaction Transaction { get; set; }

        /// <summary>
        /// AppCache
        /// </summary>
        public IAppCache AppCache { get; set; }

        /// <summary>
        /// StatelessSession
        /// </summary>
        public IStatelessSession StatelessSession { get; set; }

        private readonly HashSet<long> newPersAccIds = new HashSet<long>();
        private readonly HashSet<long> newContragensIds = new HashSet<long>();
        private readonly HashSet<long> newRoomIds = new HashSet<long>();
        private readonly HashSet<PersonalAccountOwner> ownersToRecalcAccounts = new HashSet<PersonalAccountOwner>();

        private ICacheClient<Contragent> contragentCacheClient;
        private ICacheClient<BasePersonalAccount> persAccCacheClient;
        private ICacheClient<Room> roomCacheClient;
        private ICacheClient<RealityObject> realityObjectCacheClient;

        private readonly IEnumerable<RoomOwnershipType> ownerShipTypes = Enum.GetValues(typeof(RoomOwnershipType))
            .Cast<RoomOwnershipType>();

        /// <summary>
        /// Logger
        /// </summary>
        public ILogger Logger { get; set; }

        public IRealityObjectDecisionsService RealityObjectDecisionsService { get; set; }

        public IPersonalAccountService PersonalAccountService { get; set; }

        /// <summary>
        /// Не обновлять кэш Redis
        /// </summary>
        public bool NoCasheRefresh { get; set; }

        /// <summary>
        /// Служебный класс RoomLog
        /// </summary>
        private class RoomLog
        {
            public Room Room;
            public HashSet<string> Changes;
        }

        public RoomImport(ILogImportManager logImportManager, ILogImport logImport)
        {
            this.LogImportManager = logImportManager;
            this.LogImport = logImport;
        }

        /// <summary>
        /// Валидация
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <param name="message">Сообщение</param>
        /// <returns></returns>
        public override bool Validate(BaseParams baseParams, out string message)
        {
            message = null;

            var tableLocker = this.Container.Resolve<ITableLocker>();
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
                this.Container.Release(tableLocker);
            }

            if (!baseParams.Files.ContainsKey("FileImport"))
            {
                message = "Не выбран файл для импорта";
                return false;
            }

            var extention = baseParams.Files["FileImport"].Extention;

            var fileExtentions = this.PossibleFileExtensions.Contains(",") ? this.PossibleFileExtensions.Split(',') : new[] { this.PossibleFileExtensions };
            if (fileExtentions.All(x => x != extention))
            {
                message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", this.PossibleFileExtensions);
                return false;
            }

            return true;
        }

        private void KillCache()
        {
            //var gkhparams = Container.Resolve<IGkhParams>().GetParams();

            //if (gkhparams.ContainsKey("DontKillRedisCache") && gkhparams["DontKillRedisCache"].ToBool())
            //{
            //    return; // живи
            //}
            this.Logger.LogDebug("KillCache(): AppCache._useCount = {0}", this.GetAppCacheInUse());
            this.AppCache.FlushDb(); // умри

        }

        private void TryInitAndTestCache()
        {
            var forceRebuild = false;

            do
            {
                this.InitCache(forceRebuild);

                try
                {
                    this.TestCache(); // надо проверить, что кеш реально собран
                    break;
                }
                catch (RedisException) when (!forceRebuild && this.NoCasheRefresh)
                {
                    // если у нас возникла ошибка тестирования в первый раз, то принудительно чистим кэш
                    // скорее всего его просто нет
                    // либо уже пробовали собрать -> this.NoCasheRefresh
                    forceRebuild = true;
                }
            }
            while (true);
        }

        private void TestCache()
        {
            var testRo = this.RealityObjectDomain.GetAll().OrderBy(x => x.Id).FirstOrDefault();

            // тестим на кошках
            // точнее на рандомном доме
            if (testRo != null)
            {
                this.Logger.LogDebug("TestCache(): AppCache._useCount = {0}", this.GetAppCacheInUse());
                var testCacheRo = this.realityObjectCacheClient.Get(testRo.Id);

                this.Logger.LogDebug(
                   "TestCache(): объект в бд {0}, объект в кэше {1}",
                   testRo.Return(x => x.Id).ToStr(),
                   testCacheRo.Return(x => x.Id).ToStr());

                if (testCacheRo == null)
                {
                    throw new RedisException("Тест связи с кешем Redis не пройден. Перезапустите сервер Redis и убедитесь, что он работает");
                }
            }
        }

        private void InitCache(bool force = false)
        {
            var holder = this.Container.Resolve<IKeyGeneratorHolder>();

            // LS
            holder.Add(new SimpleKeyGenerator<BasePersonalAccount>(x => x.PersonalAccountNum));
            if (!this.AppCache.IsCached(typeof(BasePersonalAccount)))
            {
                this.AppCache.RegisterInvalidator<BasePersonalAccount>(new CustomPersistentInvalidationStrategy<BasePersonalAccount, long>(this.Container, this.AppCache));
            }
           
            // Contragent
            holder.Add(new SimpleKeyGenerator<Contragent>(x => string.Format("{0}#{1}#{2}",
                x.Name.ToStr().Trim(),
                x.Inn.ToStr().Trim(),
                x.Kpp.ToStr().Trim())));
            
            if (!this.AppCache.IsCached(typeof(Contragent)))
            {
                this.AppCache.RegisterInvalidator<Contragent>(new CustomPersistentInvalidationStrategy<Contragent, long>(this.Container, this.AppCache));
            }

            //Room
            if (!this.AppCache.IsCached(typeof(Room)))
            {
                this.AppCache.RegisterInvalidator<Room>(new CustomPersistentInvalidationStrategy<Room, long>(this.Container, this.AppCache));
            }

            //RealityObject
            holder.Add(new SimpleKeyGenerator<RealityObject>(x => "fiasadr" + x.FiasAddress.Id.ToString()));
            if (!this.AppCache.IsCached(typeof(RealityObject)))
            {
                this.AppCache.RegisterInvalidator<RealityObject>(new CustomPersistentInvalidationStrategy<RealityObject, long>(this.Container, this.AppCache));
            }

            JsConfig.AssumeUtc = true;

            this.Logger.LogDebug("noCasheRefresh == {0}", this.NoCasheRefresh);

            if (this.NoCasheRefresh == false || force)
            {
                this.KillCache();

                this.Logger.LogDebug("AppCache.Invalidating = {0}", this.AppCache.Invalidating);
                
                var invalidateResult = this.AppCache.Invalidate();
                this.Logger.LogDebug("InitCache() var invalidateResult = AppCache.Invalidate();: AppCache._useCount = {0}", this.GetAppCacheInUse());

                // грязный хак, ибо иногда счётчик использования AppCache не равен нулю на данном этапе
                // из-за чего инвалидация кэша не происходит
                var maxCount = 100;
                var counter = 0;
                while (!invalidateResult)
                {
                    counter++;
                    if (counter > maxCount)
                    {
                        var exception = new RedisException("Не удалось обновить кэш Redis");
                        this.Logger.LogError(exception.Message, exception);
                        throw exception;
                    }

                    if (this.GetAppCacheInUse() > 0)
                    {
                        this.AppCache.Release();
                        this.Logger.LogDebug("InitCache() this.AppCache.Release(): AppCache._useCount = {0}", this.GetAppCacheInUse());
                    }
                    else if (this.GetAppCacheInUse() < 0)
                    {
                        this.AppCache.Use();
                        this.Logger.LogDebug("InitCache() this.AppCache.Use();: AppCache._useCount = {0}", this.GetAppCacheInUse());
                    }

                    invalidateResult = this.AppCache.Invalidate();
                }

                this.Logger.LogDebug("var invalidateResult = {0}", invalidateResult);
                this.Logger.LogDebug("InitCache() AppCache.Invalidate(): AppCache._useCount = {0}", this.GetAppCacheInUse());

                while (this.AppCache.Invalidating)
                {
                    Thread.Sleep(1000);
                }
            }

            this.contragentCacheClient = this.AppCache.GetClient<Contragent>();
            this.Logger.LogDebug("InitCache() AppCache.GetClient<Contragent>();: AppCache._useCount = {0}", this.GetAppCacheInUse());
            this.persAccCacheClient = this.AppCache.GetClient<BasePersonalAccount>();
            this.Logger.LogDebug("InitCache() AppCache.GetClient<BasePersonalAccount>();: AppCache._useCount = {0}", this.GetAppCacheInUse());
            this.roomCacheClient = this.AppCache.GetClient<Room>();
            this.Logger.LogDebug("InitCache() AppCache.GetClient<Room>();: AppCache._useCount = {0}", this.GetAppCacheInUse());
            this.realityObjectCacheClient = this.AppCache.GetClient<RealityObject>();
            this.Logger.LogDebug("InitCache() AppCache.GetClient<RealityObject>();: AppCache._useCount = {0}", this.GetAppCacheInUse());

            this.SessionProvider.CloseCurrentSession(null);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private int GetAppCacheInUse()
        {
            return typeof(AppCache).GetField("_useCount", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(this.AppCache).As<int>();
        }

        private void InitHeader(GkhExcelCell[] data)
        {
            this.headersDict = new Dictionary<string, int>();

            this.headersDict["ID_DOMA"] = -1;
            this.headersDict["MU"] = -1;
            this.headersDict["TYPE_CITY"] = -1;
            this.headersDict["CITY"] = -1;
            this.headersDict["TYPE_STREET"] = -1;
            this.headersDict["STREET"] = -1;
            this.headersDict["HOUSE_NUM"] = -1;
            this.headersDict["LITER"] = -1;
            this.headersDict["KORPUS"] = -1;
            this.headersDict["BUILDING"] = -1;
            this.headersDict["FLAT_PLACE_NUM"] = -1;
            this.headersDict["TOTAL_AREA"] = -1;
            this.headersDict["LIVE_AREA"] = -1;
            this.headersDict["FLAT_PLACE_TYPE"] = -1;
            this.headersDict["PROPERTY_TYPE"] = -1;
            this.headersDict["BILL_TYPE"] = -1;
            this.headersDict["SURNAME"] = -1;
            this.headersDict["NAME"] = -1;
            this.headersDict["LASTNAME"] = -1;
            this.headersDict["INN"] = -1;
            this.headersDict["KPP"] = -1;
            this.headersDict["RENTER_NAME"] = -1;
            this.headersDict["SHARE"] = -1;
            this.headersDict["DATE"] = -1;
            this.headersDict["LS_NUM"] = -1;
            this.headersDict["LS_DATE"] = -1;
            this.headersDict["KLADRCODE"] = -1;
            this.headersDict["CADASTRAL_NUM"] = -1;
            this.headersDict["RKC_LS_NUM"] = -1;
            this.headersDict["RKC_NUM"] = -1;
            this.headersDict["RKC_START_DATE"] = -1;
            this.headersDict["RKC_END_DATE"] = -1;

            for (var index = 0; index < data.Length; ++index)
            {
                var header = data[index].Value.ToUpper();
                if (this.headersDict.ContainsKey(header))
                {
                    this.headersDict[header] = index;
                }
            }
        }

        /// <summary>
        /// InitLog
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        public new void InitLog(string fileName)
        {
            if (!this.Container.Kernel.HasComponent(typeof(ILogImportManager)))
            {
                throw new ImportException("Не найдена реализация интерфейса ILogImportManager");
            }
            if (this.LogImportManager == null)
            {
                throw new ImportException("Не найдена реализация интерфейса ILogImport");
            }

            this.LogImportManager.FileNameWithoutExtention = fileName;
            this.LogImportManager.UploadDate = DateTime.Now;

            if (this.LogImport == null)
            {
                throw new ImportException("Не найдена реализация интерфейса ILogImport");
            }

            this.LogImport.SetFileName(fileName);
            this.LogImport.ImportKey = this.Key;
        }

        private string GetValue(GkhExcelCell[] data, string field)
        {
            var result = string.Empty;

            if (this.headersDict.ContainsKey(field))
            {
                var index = this.headersDict[field];
                if (data.Length > index && index > -1)
                {
                    result = data[index].Value;
                }
            }

            return result?.Trim() ?? string.Empty;
        }

        #region Overrides of GkhImportBase
        /// <summary>
        /// Импорт
        /// </summary>
        /// <param name="params">Параметры</param>
        /// <param name="ctx">Контекст</param>
        /// <param name="indicator">Индикатор прогресса</param>
        /// <param name="ct">Уведомление об отмене</param>
        /// <returns></returns>
        protected override ImportResult Import(BaseParams @params, ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            if (!this.AccountHasDefaultState())
            {
                return new ImportResult(StatusImport.CompletedWithError, "Не создан начальный статус для сущности 'Лицевой счет(модуль Регоператора)'!");
            }

            this.replaceExistRooms = @params.Params.GetAs<bool>("replaceExistRooms");
            this.NoCasheRefresh = @params.Params.GetAs<bool>("NoRefreshCash");
            this.culture = CultureInfo.CreateSpecificCulture("ru-RU");

            var file = @params.Files["FileImport"];
            var fileExtention = file.Extention;
            var appSettings = this.Container.Resolve<IConfigProvider>().GetConfig().AppSettings;
            this.roomNumNormalizationDisabled = appSettings.GetAs<bool>("RoomImport.RoomNumNormalizationDisabled");
            this.discriminateAccountsByNum = appSettings.GetAs<bool>("RoomImport.DiscriminateAccountsByNum");

            var message = "";

            try
            {
                this.InitLog(file.FileName);

                indicator.Indicate(null, 0, "Инициализация кеша");

                this.TryInitAndTestCache();

                indicator.Indicate(null, 10, "Подготовка данных для импорта");

                this.InitDictForExtract();
                this.ProcessData(file.Data, fileExtention);
                this.KillExtractDicts();

                this.InitDictForImport();
                this.PrepareData(indicator);
                this.KillImportDicts();

                indicator.Indicate(null, 90, "Сохранение истории изменений");
                this.InTransaction();

                // после импорта дешевле весь кэш пересобрать, чем отслеживать все изменения
                indicator.Indicate(null, 95, "Обновление реестра лицевых счетов");
                this.UpdateCountersAndRegistry();

                this.WriteLogs();

                this.Dispose();
            }
            catch (Exception e)
            {
                this.LogImport.Error(e.Message, string.Format("Произошла непредвиденная ошибка.\n{0} {1}", e.Message, e));
                message = string.Format("Произошла непредвиденная ошибка.\n {0} {1}", e.Message, e);
            }
            finally
            {
                this.logDict.Clear();
            }

            this.SubPersonalAccountService?.AddSubPersonalAccount();

            // Намеренно закрываем текущую сессию, иначе при каждом коммите транзакции
            // ранее измененные дома вызывают каскадирование ФИАС
            this.Container.Resolve<ISessionProvider>().CloseCurrentSession();

            this.LogImportManager.Add(file, this.LogImport);
            this.LogImportManager.Save();

            message = string.IsNullOrEmpty(message) ? this.LogImportManager.GetInfo() : message;

            var status = this.LogImportManager.CountError > 0 ? StatusImport.CompletedWithError : StatusImport.CompletedWithoutError;

            var logField = this.LogImportManager.LogFileId;

            return new ImportResult(status, message, string.Empty, logField);
        }

        private void UpdateCountersAndRegistry()
        {
            var updateResult = new List<PersistentObject>();
            foreach (var ownersToRecalcAccount in this.ownersToRecalcAccounts)
            {
                if (this.PersonalAccountOwnerService.OnUpdateOwner(ownersToRecalcAccount))
                {
                    updateResult.Add(ownersToRecalcAccount);
                }
            }

            TransactionHelper.InsertInManyTransactions(this.Container, updateResult, useStatelessSession: true);
            this.AccountDtoService.MassCreatePersonalAccountDto(true);
        }
        #endregion

        private void KillImportDicts()
        {
            this.legalOwnersDict = null;
            this.existAccount = null;
            this.ownerKeysByAccountId = null;
            this.individualOwnersDict = null;
            this.accountByRoomAndOwnerDict = null;
            this.accountByExternalNumDict = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void InitDictForImport()
        {
            this.legalOwnersDict = this.LegalAccountOwnerDomain.GetAll()
                .Select(x => new { contragentId = x.Contragent.Id, accountId = x.Id })
                .AsEnumerable()
                .GroupBy(x => x.contragentId)
                .ToDictionary(x => x.Key, x => (object)x.First().accountId);

            var existOwners = this.BasePersonalAccountDomain.GetAll()
                .Select(x => new
                {
                    AccountOwner = x.AccountOwner.Id,
                    RealityObject = x.Room.RealityObject.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.AccountOwner)
                .ToDictionary(
                    x => x.Key,
                    y => y.Select(z => z.RealityObject).FirstOrDefault());

            var tmpLegalownerDict = this.LegalAccountOwnerDomain.GetAll()
                .Select(x => new { x.Contragent.Name, x.Contragent.Inn, x.Contragent.Kpp, x.Id })
                .AsEnumerable()
                .ToDictionary(x => x.Id, y => string.Format("{0}_{1}_{2}", y.Name, y.Inn, y.Kpp).Replace(" ", string.Empty).Trim().ToLower());

            var tmpIndivownerDict = this.IndividualAccountOwnerDomain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    ownerSurname = x.Surname ?? string.Empty,
                    ownerFirstName = x.FirstName ?? string.Empty,
                    ownerSecondName = x.SecondName ?? string.Empty
                })
                .AsEnumerable()
                .ToDictionary(x => x.Id,
                    y => string.Format("{0}_{1}_{2}",
                        y.ownerSurname,
                        y.ownerFirstName,
                        y.ownerSecondName).Replace(" ", string.Empty).Trim().ToLower());

            this.existAccount = this.BasePersonalAccountDomain.GetAll()
                    .Select(
                        x =>
                            new
                            {
                                RoId = x.Room.RealityObject.Id,
                                x.Room.RoomNum,
                                x.AccountOwner.Id,
                                Account = x.Id,
                                x.PersonalAccountNum,
                                x.PersAccNumExternalSystems
                            })
                    .AsEnumerable()
                    .GroupBy(
                        x =>
                            (this.discriminateAccountsByNum
                                ? string.Format(
                                    "{0}#{1}#{2}#{3}#{4}",
                                    x.RoId, this.NormalizeRoomNum(x.RoomNum),
                                    (tmpIndivownerDict.Get(x.Id) ?? tmpLegalownerDict.Get(x.Id) ?? string.Empty).ToLower
                                        (),
                                    x.PersonalAccountNum,
                                    x.PersAccNumExternalSystems)
                                : string.Format(
                                    "{0}#{1}#{2}",
                                    x.RoId, this.NormalizeRoomNum(x.RoomNum),
                                    ((tmpIndivownerDict.Get(x.Id) ?? tmpLegalownerDict.Get(x.Id) ?? string.Empty)
                                        .ToLower()) + string.Format("#{0}", x.PersonalAccountNum))).Trim())
                    .ToDictionary(x => x.Key, y => y.Select(x => x.Account).First());

            this.ownerKeysByAccountId = this.existAccount.ToDictionary(x => x.Value, x => x.Key);

            this.individualOwnersDict = this.IndividualAccountOwnerDomain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    ownerSurname = x.Surname ?? string.Empty,
                    ownerFirstName = x.FirstName ?? string.Empty,
                    ownerSecondName = x.SecondName ?? string.Empty
                })
                .AsEnumerable()
                .GroupBy(y => string.Format(
                    "{0}_{1}_{2}_{3}",
                    existOwners.ContainsKey(y.Id) ? existOwners[y.Id].ToString() : string.Empty,
                    y.ownerSurname.Trim(),
                    y.ownerFirstName.Trim(),
                    y.ownerSecondName.Trim()).Replace(" ", string.Empty).Trim().ToLower())
                .ToDictionary(y => y.Key, y => (object)y.First().Id);

            this.allExistingAccNumbers = this.BasePersonalAccountDomain.GetAll().Select(x => x.PersonalAccountNum);

            var accountsGroupingInfo = this.BasePersonalAccountDomain.GetAll()
                .Where(x => !x.State.FinalState)
                .Select(x => new
                {
                    accountId = x.Id,
                    roomNum = x.Room.RoomNum,
                    roId = x.Room.RealityObject.Id,
                    x.PersonalAccountNum,
                    x.PersAccNumExternalSystems
                })
                .AsEnumerable()
                .GroupBy(x => string.Format("{0}_{1}", x.roId, this.NormalizeRoomNum(x.roomNum)));

            this.accountByRoomAndOwnerDict = accountsGroupingInfo
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(y => y.PersonalAccountNum)
                        .ToDictionary(y => y.Key, y => (object)y.First().accountId));

            this.accountByExternalNumDict = accountsGroupingInfo
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(y => y.PersAccNumExternalSystems ?? string.Empty)
                        .ToDictionary(y => y.Key, y => (object)y.First().accountId));

            this.roomAreaLastLogDict = this.EntityLogLightDomain.GetAll()
                .Where(x => x.ClassName == typeof(Room).Name)
                .Where(x => x.ParameterName == VersionedParameters.RoomArea)
                .Select(x => new { x.EntityId, x.Id, x.DateActualChange })
                .AsEnumerable()
                .GroupBy(x => x.EntityId)
                .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.Id).First().DateActualChange);

            this.accountOpenDateLastLogDict = this.EntityLogLightDomain.GetAll()
                .Where(x => x.ClassName == typeof(BasePersonalAccount).Name)
                .Where(x => x.ParameterName == VersionedParameters.PersonalAccountOpenDate)
                .Select(x => new { x.EntityId, x.Id, x.DateActualChange })
                .AsEnumerable()
                .GroupBy(x => x.EntityId)
                .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.Id).First().DateActualChange);

            this.accountAreaShareLastLogDict = this.EntityLogLightDomain.GetAll()
                .Where(x => x.ClassName == typeof(BasePersonalAccount).Name)
                .Where(x => x.ParameterName == VersionedParameters.AreaShare)
                .Select(x => new { x.EntityId, x.Id, x.DateActualChange })
                .AsEnumerable()
                .GroupBy(x => x.EntityId)
                .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.Id).First().DateActualChange);

            this.Props = typeof(BasePersonalAccount).GetProperties().Where(x => x.PropertyType == typeof(Wallet)).ToArray();
            this.Accessor = TypeAccessor.Create(typeof(BasePersonalAccount), true);

            this.curPeriod = this.ChargePeriodRepo.GetCurrentPeriod();
        }

        private void CloseTransaction()
        {
            if (this.Transaction != null && this.Transaction.IsActive)
            {
                try
                {
                    this.Transaction.Commit();

                    this.newPersAccIds.Clear();
                    this.newContragensIds.Clear();
                    this.newRoomIds.Clear();
                }
                catch (Exception)
                {
                    this.Transaction.Rollback();
                    this.KillNewObjectsOnFail();
                    this.InTransaction();

                    throw;
                }
            }
        }

        private void KillExtractDicts()
        {
            this.existRobjectIds = null;
            this.realtyObjectsByAddressDict = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void InitDictForExtract()
        {
            this.firstPeriod = this.ChargePeriodRepo.GetFirstPeriod();

            this.realtyObjectRoomsDict = this.RoomsDomain.GetAll()
                .Select(x => new
                {
                    roId = x.RealityObject.Id,
                    x.RoomNum,
                    x.Id
                })
                .ToArray()
                .GroupBy(x => x.roId)
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(y => this.NormalizeRoomNum(y.RoomNum))
                        .ToDictionary(y => y.Key, y => (object)y.First().Id));

            this.existRobjectIds = this.RealityObjectDomain.GetAll().Select(x => x.Id).ToHashSet();

            // Получаем дома, адрес которых имеет улицу
            // делаем join записей, у которых равны AOGuid и ParentGuid
            var housesWithStreetsByAoGuid = this.FiasDomain.GetAll()
                .Join(this.FiasDomain.GetAll(),
                    x => x.AOGuid,
                    x => x.ParentGuid,
                    (a, b) => new { parent = a, child = b })
                .Join(this.RealityObjectDomain.GetAll(),
                    x => x.child.AOGuid,
                    y => y.FiasAddress.StreetGuidId,
                    (c, d) => new { c.parent, c.child, realityObject = d })
                .Where(x => x.parent.ActStatus == FiasActualStatusEnum.Actual)
                .Where(x => x.child.ActStatus == FiasActualStatusEnum.Actual)
                .Where(x => x.child.AOLevel == FiasLevelEnum.Street)
                .Select(x => new
                {
                    localityName = x.parent.OffName,
                    localityShortname = x.parent.ShortName,
                    streetName = x.child.OffName,
                    streetShortname = x.child.ShortName,
                    x.realityObject.FiasAddress.House,
                    x.realityObject.FiasAddress.Letter,
                    x.realityObject.FiasAddress.Housing,
                    x.realityObject.FiasAddress.Building,
                    RealityObjectId = x.realityObject.Id,
                    MunicipalityName = x.realityObject.Municipality.Name,
                    MirrorGuid = string.Empty,
                    ParentGuid = string.Empty
                })
                .AsEnumerable()
                .Where(x => !string.IsNullOrWhiteSpace(x.localityName))
                .Where(x => !string.IsNullOrWhiteSpace(x.streetName))
                .ToList();

            // Получаем дома, адрес которых имеет улицу
            // делаем join записей, у которых равны MirrorGuid и ParentGuid
            var housesWithStreetsByMirrorGuid = this.FiasDomain.GetAll()
                .Join(this.FiasDomain.GetAll(),
                    x => x.MirrorGuid,
                    x => x.ParentGuid,
                    (a, b) => new { parent = a, child = b })
                .Join(this.RealityObjectDomain.GetAll(),
                    x => x.child.AOGuid,
                    y => y.FiasAddress.StreetGuidId,
                    (c, d) => new { c.parent, c.child, realityObject = d })
                .Where(x => x.parent.ActStatus == FiasActualStatusEnum.Actual)
                .Where(x => x.child.ActStatus == FiasActualStatusEnum.Actual)
                .Where(x => x.child.AOLevel == FiasLevelEnum.Street)
                .Select(x => new
                {
                    localityName = x.parent.OffName,
                    localityShortname = x.parent.ShortName,
                    streetName = x.child.OffName,
                    streetShortname = x.child.ShortName,
                    x.realityObject.FiasAddress.House,
                    x.realityObject.FiasAddress.Letter,
                    x.realityObject.FiasAddress.Housing,
                    x.realityObject.FiasAddress.Building,
                    RealityObjectId = x.realityObject.Id,
                    MunicipalityName = x.realityObject.Municipality.Name,
                    x.parent.MirrorGuid,
                    x.child.ParentGuid
                })
                .AsEnumerable()
                .Where(x => !string.IsNullOrWhiteSpace(x.MirrorGuid))
                .Where(x => !string.IsNullOrWhiteSpace(x.ParentGuid))
                .Where(x => !string.IsNullOrWhiteSpace(x.localityName))
                .Where(x => !string.IsNullOrWhiteSpace(x.streetName))
                .ToList();

            // Получаем дома, адрес которых НЕ имеет улицу
            var housesWithoutStreets = this.FiasDomain.GetAll()
                .Join(this.RealityObjectDomain.GetAll(),
                    x => x.AOGuid,
                    y => y.FiasAddress.PlaceGuidId,
                    (c, d) => new { fias = c, realityObject = d })
                .Where(x => x.fias.ActStatus == FiasActualStatusEnum.Actual)
                .Where(x => x.fias.AOLevel != FiasLevelEnum.Street)
                .Select(x => new
                {
                    localityName = x.fias.OffName,
                    localityShortname = x.fias.ShortName,
                    x.realityObject.FiasAddress.House,
                    x.realityObject.FiasAddress.Letter,
                    x.realityObject.FiasAddress.Housing,
                    x.realityObject.FiasAddress.Building,
                    RealityObjectId = x.realityObject.Id,
                    MunicipalityName = x.realityObject.Municipality.Name,
                    x.realityObject.FiasAddress.StreetGuidId
                })
                .AsEnumerable()
                // Такой замут с AsEnumerable() из-за особенностей обработки строк ораклом
                .Where(x => string.IsNullOrWhiteSpace(x.StreetGuidId))
                .Select(x => new
                {
                    x.localityName,
                    x.localityShortname,
                    streetName = string.Empty,
                    streetShortname = string.Empty,
                    x.House,
                    x.Letter,
                    x.Housing,
                    x.Building,
                    x.RealityObjectId,
                    x.MunicipalityName,
                    MirrorGuid = string.Empty,
                    ParentGuid = string.Empty
                })
                .Where(x => !string.IsNullOrWhiteSpace(x.localityName))
                .Where(x => string.IsNullOrWhiteSpace(x.streetName))
                .ToList();

            // 3 - уровневневый словарь
            // Список существующих домов сгруппированных по улице
            // => сгруппированных по населенному пункту
            // => сгруппированных по муниципальному образованию (первого уровня)
            this.realtyObjectsByAddressDict = housesWithStreetsByAoGuid
                .Union(housesWithoutStreets)
                .Union(housesWithStreetsByMirrorGuid)
                .GroupBy(x => x.MunicipalityName.ToStr().Trim().ToLower())
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(z => (z.localityName + " " + (z.localityShortname ?? string.Empty)).Trim().ToLower())
                        .ToDictionary(
                            z => z.Key,
                            z => z.GroupBy(v => (v.streetName + " " + (v.streetShortname ?? string.Empty)).Trim().ToLower())
                                .ToDictionary(
                                    v => v.Key,
                                    v => v.Select(u => new RealtyObjectInStreet
                                    {
                                        RoId = u.RealityObjectId,
                                        House = u.House,
                                        Letter = u.Letter,
                                        Housing = u.Housing,
                                        Building = u.Building,
                                        HouseLetter = String.Format("{0}{1}{2}{3}", u.House, u.Housing, u.Letter, u.Building).ToLower()
                                    }).Distinct()
                                        .ToList())));

            this.contragentsDict = this.ContragentDomain.GetAll()
                .Select(x => new { x.Id, x.Name, x.Inn, x.Kpp })
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    mixedkey = string.Format("{0}#{1}#{2}",
                        x.Name.ToStr().Trim(),
                        x.Inn.ToStr().Trim(),
                        x.Kpp.ToStr().Trim()).ToLower()
                })
                .GroupBy(x => x.mixedkey)
                .ToDictionary(x => x.Key, x => x.Select(y => new ContragentInfoProxy { Id = y.Id, Name = y.Name }).First());

            this.cashPaymentCenterIdDict = this.CashPaymentCenterDomain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Identifier
                })
                .ToList()
                .GroupBy(x => x.Identifier)
                .ToDictionary(x => x.Key, y => y.First().Id);

            this.cashPaymentCenterPersAccDict = this.CashPaymentCenterPersAccDomain.GetAll()
                .Select(x => new CashPaymentCenterPersAccProxy
                {
                    Id = x.Id,
                    CashPaymentCenterId = x.CashPaymentCenter.Id,
                    PersAccId = x.PersonalAccount.Id,
                    DateStart = x.DateStart,
                    DateEnd = x.DateEnd,
                    AccNum = x.PersonalAccount.PersonalAccountNum
                })
                .ToList()
                .GroupBy(x => x.PersAccId)
                .ToDictionary(x => x.Key, y => y.ToList());

            this.fiasRecords = this.FiasDomain.GetAll()
                .Where(x => x.ActStatus == FiasActualStatusEnum.Actual)
                .Select(x => new FiasProxy
                {
                    CodeRecord = x.CodeRecord,
                    AOLevel = x.AOLevel,
                    AOGuid = x.AOGuid,
                    ParentGuid = x.ParentGuid
                })
                .ToList();

            this.fiasAddresList = this.FiasAddressDomain.GetAll()
                .Join(this.RealityObjectDomain.GetAll(),
                    a => a.Id,
                    b => b.FiasAddress.Id,
                    (a, b) => new FiasAddressProxy
                    {
                        Id = a.Id,
                        PlaceGuidId = a.PlaceGuidId,
                        StreetGuidId = a.StreetGuidId,
                        House = a.House,
                        Housing = a.Housing,
                        Letter = a.Letter,
                        Building = a.Building
                    })
                .AsEnumerable()
                .Select(x => new FiasAddressProxy
                {
                    Id = x.Id,
                    PlaceGuidId = x.PlaceGuidId,
                    StreetGuidId = x.StreetGuidId,
                    House = x.House,
                    Housing = x.Housing,
                    Letter = x.Letter,
                    Building = x.Building,
                    HouseLetter = String.Format("{0}{1}{2}{3}", x.House, x.Housing, x.Letter, x.Building)
                }).ToList();
        }

        // Метод подготовки данных
        private void PrepareData(IProgressIndicator indicator)
        {
            var recDict = this.records.Select(
                x => new
                         {
                             Record = x, // импортируемая запись
                             Room = this.GetOrCreateRoom(x), // помещение
                             Key = // ключ определяющий собственника
                         (x.OwnerType == PersonalAccountOwnerType.Legal
                              ? string.Format("{0}_{1}_{2}#{3}", x.OwnerJurName, x.OwnerJurInn, x.OwnerJurKpp, x.AccountNumber)
                              : string.Format("{0}_{1}_{2}#{3}", x.OwnerPhysSurname, x.OwnerPhysFirstName, x.OwnerPhysSecondName, x.AccountNumber)).Replace(" ", string.Empty).Trim().ToLower(),
                             x.AccountNumber,
                             x.PersAccNumExternalSystems
                         })
                .GroupBy(
                    x =>
                    new
                        {
                            x.Record.RealtyObjectId,
                            RoomNum = this.NormalizeRoomNum(x.Room.RoomNum),
                            x.Key,
                            AccNum = this.discriminateAccountsByNum ? x.AccountNumber : string.Empty,
                            ExtAccNum = this.discriminateAccountsByNum ? x.PersAccNumExternalSystems : string.Empty
                        })
                .Select(x => x.First())
                .Select(
                    x =>
                    new
                        {
                            x.Record,
                            x.Room,
                            Key = this.discriminateAccountsByNum
                            ? string.Format("{0}#{1}#{2}#{3}#{4}", x.Record.RealtyObjectId, this.NormalizeRoomNum(x.Room.RoomNum), x.Key, x.AccountNumber, x.PersAccNumExternalSystems)
                            : string.Format("{0}#{1}#{2}", x.Record.RealtyObjectId, this.NormalizeRoomNum(x.Room.RoomNum), x.Key)
                            /* ключ, определяющий собственника в помещении */
                        }).GroupBy(x => string.Format("{0}_{1}", x.Record.RealtyObjectId, this.NormalizeRoomNum(x.Room.RoomNum)))
                // получаем словарь помещение -> собственники
                .ToDictionary(x => x.Key, x => x.ToDictionary(y => y.Key, y => y));


            var count = recDict.Count();
            var cur = 0;

            foreach (var itemDict in recDict.Section(1000))
            {
                this.CloseTransaction();

                foreach (var item in itemDict)
                {
                    cur++;
                    var percent = 10 + (((float)cur * 80) / count);

                    indicator.Indicate(null,
                        (ushort)(percent > 90 ? 90 : percent),
                        "Обработка строки {0} из {1}".FormatUsing(cur, count));

                    // ищем существующих аккаунты
                    Dictionary<string, object> accountInRoom;
                    List<decimal> areaShares;
                    if (this.accountByRoomAndOwnerDict.TryGetValue(item.Key, out accountInRoom))
                    {
                        // если нашли, то получаем для каждого ключ собственника в помещении
                        var accounts =
                            accountInRoom.Values
                                .Select(x =>
                                    x is long
                                        ? this.persAccCacheClient.Get((long)x)
                                        : (x is BasePersonalAccount ? (BasePersonalAccount)x : null))
                                .Where(x => x != null)
                                .Select(x => new { Account = x, Key = this.ownerKeysByAccountId.Get(x.Id) })
                                .Where(x => !string.IsNullOrEmpty(x.Key))
                                .ToDictionary(x => x.Key, x => x.Account);

                        // по ключу собственника в помещении пытаемся найти все аккаунты, существующие и импортируемые
                        // если включена замена данных, то берем импортируемые доли собственности, а затем добавляем существующие из тех, которые не импортируются в данный момент
                        // иначе берем существующие аккаунты и добавляем к ним те, который новые

                        if (this.replaceExistRooms)
                        {
                            areaShares = item.Value.Values.Select(x => x.Record.AreaShare).ToList();
                            areaShares.AddRange(accounts.Where(x => !item.Value.ContainsKey(x.Key)).Select(x => x.Value.AreaShare));
                        }
                        else
                        {
                            areaShares = accounts.Select(x => x.Value.AreaShare).ToList();
                            areaShares.AddRange(item.Value.Where(x => !accounts.ContainsKey(x.Key))
                                .Select(x => x.Value.Record.AreaShare));
                        }
                    }
                    else
                    {
                        // если существующих аккаунтов нет, просто складываем доли собственности по импортируемым строкам
                        areaShares = item.Value.Values.Select(x => x.Record.AreaShare).ToList();
                    }

                    var totalAreaShare = areaShares.SafeSum(x => x);
                    // почему 1.05? это питер. cry baby cry
                    if (totalAreaShare > 1.05M)
                    {
                        // если доля собственности оказалась больше 1, выбрасываем из импорта все строки по этому помещению
                        item.Value.Values.ForEach(
                            x =>
                            {
                                x.Record.CreateOwner = false;
                                this.AddLog(
                                    x.Record.RowNumber,
                                    "Суммарная доля собственности у жилища больше 1. Лицевой счет не создан!", 
                                    false);
                            });
                    }
                    else
                    {
                        // иначе создаем/обновляем лицевые счета
                        item.Value.Values
                            .ForEach(x =>
                            {
                                if (x.Record.CreateOwner)
                                {
                                    var owner = x.Record.OwnerType == PersonalAccountOwnerType.Individual
                                        ? this.GetIndividualOwner(x.Record)
                                        : this.GetLegalOwner(x.Record);

                                    this.CreateOrUpdateAccount(x.Record, x.Room, owner);
                                }

                                this.AddLog(x.Record.RowNumber, "Успешно");
                            });
                    }
                }

                this.CloseTransaction();
            }
        }

        private void KillNewObjectsOnFail()
        {
            foreach (var key in this.newContragensIds)
            {
                var obj = this.contragentCacheClient.Get(key);
                this.contragentCacheClient.Remove(obj);
            }
            foreach (var key in this.newRoomIds)
            {
                var obj = this.roomCacheClient.Get(key);
                this.roomCacheClient.Remove(obj);
            }
            foreach (var key in this.newPersAccIds)
            {
                var obj = this.persAccCacheClient.Get(key);
                this.persAccCacheClient.Remove(obj);
            }

            this.newPersAccIds.Clear();
            this.newContragensIds.Clear();
            this.newRoomIds.Clear();
        }

        private void WriteLogs()
        {
            foreach (var log in this.logDict.OrderBy(x => x.Key))
            {
                var rowNumber = string.Format("Строка {0}", log.Key);

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
        }

        private void AddLog(int rowNum, string message, bool success = true, Exception ex = null)
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

            if (ex.IsNotNull())
            {
                this.Logger.LogError(ex, "Ошибка в импорте абонентов");
            }
        }

        private Room NewApartment(Record record)
        {
            var room = new Room
            {
                RealityObject = this.realityObjectCacheClient.Get(record.RealtyObjectId),
                RoomNum = this.NormalizeRoomNum(record.Apartment),
                Area = record.Area,
                LivingArea = record.LivingArea,
                Type = record.RoomType,
                OwnershipType = record.OwnershipType,
                CadastralNumber = record.CadastralNumber
            };

            this.InsertOrUpdateObject(room);
            this.newRoomIds.Add(room.Id);
            this.roomCacheClient.Add(room);

            this.AddLog(record.RowNumber,
                string.Format(
                    "Добавлено новое помещение по адресу {0}, кв. {1}; площадь {2}; жилая площадь {3};тип помещения {4}; форма собственности {5}; кадастровый номер {6}",
                    room.RealityObject.Address,
                    room.RoomNum,
                    room.Area.RegopRoundDecimal(2),
                    room.LivingArea.HasValue ? room.LivingArea.Value.RegopRoundDecimal(2).ToString() : "",
                    room.Type.GetEnumMeta().Display,
                    room.OwnershipType.GetEnumMeta().Display,
                    room.CadastralNumber));

            return room;
        }

        private void InsertOrUpdateAccount(BasePersonalAccount account)
        {
            this.UpdatePersonalAccountState(account);
            this.InsertOrUpdateObject(account);
        }

        /// <summary>
        /// Добавить или обновить
        /// </summary>
        /// <typeparam name="T">Тип</typeparam>
        /// <param name="obj">Объект</param>
        public void InsertOrUpdateObject<T>(T obj) where T : BaseEntity
        {
            if (this.StatelessSession == null || !this.StatelessSession.IsOpen)
            {
                this.StatelessSession = this.SessionProvider.OpenStatelessSession();
            }

            if (this.Transaction == null || !this.Transaction.IsActive)
            {
                this.Transaction = this.StatelessSession.BeginTransaction();
            }

            var now = DateTime.Now;
            obj.ObjectEditDate = now;

            if (obj.Id == 0)
            {
                obj.ObjectCreateDate = now;

                if (typeof(T) == typeof(BasePersonalAccount) && !this.AccountNumIsUnique(obj as BasePersonalAccount))
                {
                    throw new Exception("В системе уже существует лицевой счет с таким номером.");
                }

                this.StatelessSession.Insert(obj);

                if(typeof(T) == typeof(BasePersonalAccount))
                {
                    this.LogImport.CountAddedRows++;
                }
            }
            else
            {
                if (obj.ObjectCreateDate.Year == DateTime.MinValue.Year)
                {
                    obj.ObjectCreateDate = now;
                }
                this.StatelessSession.Update(obj);

                if (typeof(T) == typeof(BasePersonalAccount))
                {
                    this.LogImport.CountChangedRows++;
                }
            }
        }
        
        /// <summary>
        /// Установить начальный статус ЛС в зависимости от наличия активных протоколов жилого дома
        /// </summary>
        private void UpdatePersonalAccountState(BasePersonalAccount account)
        {
            if (account == null || account.Room == null || account.Room.RealityObject == null)
            {
                return;
            }

            var roId = account.Room.RealityObject.Id;

            if (this.roWithProtocolsInFinalState.Contains(roId))
            {
                account.State = this.accountStartState;
            }
            else if (!this.roWithNoProtocolsInFinalState.Contains(roId))
            {
                var protocols = this.RoDecisionsService.GetRobjectsFundFormation(new[] {roId});

                if (protocols.ContainsKey(roId) && protocols[roId].Any(x => x.Item1 < DateTime.Today))
                {
                    this.roWithProtocolsInFinalState.Add(roId);
                    account.State = this.accountStartState;
                }
                else
                {
                    this.roWithNoProtocolsInFinalState.Add(roId);
                }
            }
        }

        /// <summary>
        /// Проверка номера создаваемого ЛС на уникальность в системе.
        /// </summary>
        /// <param name="account">ЛС для создания</param>
        /// <returns>true - если проверка на уникальность пройдена; в противном случае - false</returns>
        private bool AccountNumIsUnique(BasePersonalAccount account)
        {
            bool result = true;

            if (this.allExistingAccNumbers != null && account != null)
            {
                result = !this.allExistingAccNumbers.Any(x => x == account.PersonalAccountNum);
            }

            return result;
        }

        private PersonalAccountOwner GetIndividualOwner(Record record)
        {
            var key = string.Format(
                "{0}_{1}_{2}_{3}",
                record.RealtyObjectId,
                record.OwnerPhysSurname,
                record.OwnerPhysFirstName,
                record.OwnerPhysSecondName)
                .Replace(" ", string.Empty)
                .Trim()
                .ToLower();

            var owner = new IndividualAccountOwner
            {
                OwnerType = PersonalAccountOwnerType.Individual,
                Surname = record.OwnerPhysSurname ?? string.Empty,
                FirstName = record.OwnerPhysFirstName ?? string.Empty,
                SecondName = record.OwnerPhysSecondName ?? string.Empty,
                IdentityNumber = string.Empty,
                IdentitySerial = string.Empty,
                IdentityType = 0
            };

            this.AddLog(record.RowNumber, "Добавлен новый владелец счета " + owner.Name);

            this.InsertOrUpdateObject(owner);

            this.individualOwnersDict[key] = owner;
            this.ownersToRecalcAccounts.Add(owner);

            return owner;
        }

        private PersonalAccountOwner GetLegalOwner(Record record)
        {
            Contragent contragent;

            var mixedkey = string.Format("{0}#{1}#{2}", record.OwnerJurName, record.OwnerJurInn, record.OwnerJurKpp).ToLower();

            if (this.contragentsDict.ContainsKey(mixedkey))
            {
                var contragentInfo = this.contragentsDict[mixedkey];
                contragent = new Contragent {Id = contragentInfo.Id, Name = contragentInfo.Name, ContragentState = ContragentState.Active};
            }
            else
            {
                this.AddLog(record.RowNumber, "Добавлен новый контрагент " + record.OwnerJurName);

                contragent = new Contragent
                {
                    Name = record.OwnerJurName,
                    Inn = record.OwnerJurInn,
                    Kpp = record.OwnerJurKpp,
                    ContragentState = ContragentState.Active
                };

                if (contragent.Name == null)
                {
                    contragent.Name = "  ";
                }

                this.ListContragentToSave.Add(contragent);
                this.InsertOrUpdateObject(contragent);
                this.contragentCacheClient.Add(contragent);

                this.contragentsDict[mixedkey] = new ContragentInfoProxy { Id = contragent.Id, Name = contragent.Name };
            }

            if (this.legalOwnersDict.ContainsKey(contragent.Id))
            {
                if (this.legalOwnersDict[contragent.Id] is long)
                {
                    return new LegalAccountOwner { Id = (long)this.legalOwnersDict[contragent.Id] };
                }

                if (this.legalOwnersDict[contragent.Id] is LegalAccountOwner)
                {
                    return (LegalAccountOwner)this.legalOwnersDict[contragent.Id];
                }
            }

            var owner = new LegalAccountOwner
            {
                OwnerType = PersonalAccountOwnerType.Legal,
                Contragent = contragent
            };

            this.AddLog(record.RowNumber, "Добавлен новый владелец счета " + owner.Name);

            this.InsertOrUpdateObject(owner);

            this.legalOwnersDict[contragent.Id] = owner;
            this.ownersToRecalcAccounts.Add(owner);

            return owner;
        }

        private BasePersonalAccount NewAccount(Record record, Room apartment, PersonalAccountOwner owner)
        {
            var accountKey = this.discriminateAccountsByNum
                ? string.Format(
                    "{0}#{1}#{2}#{3}#{4}",
                    record.RealtyObjectId, this.NormalizeRoomNum(apartment.RoomNum),
                    (record.OwnerType == PersonalAccountOwnerType.Individual
                        ? string.Format("{0}_{1}_{2}", record.OwnerPhysSurname, record.OwnerPhysFirstName, record.OwnerPhysSecondName)
                        : string.Format("{0}_{1}_{2}", record.OwnerJurName, record.OwnerJurInn, record.OwnerJurKpp)).Replace(" ", string.Empty).ToLower(),
                    record.AccountNumber,
                    record.PersAccNumExternalSystems)
                : string.Format(
                    "{0}#{1}#{2}",
                    record.RealtyObjectId, this.NormalizeRoomNum(apartment.RoomNum),
                    (record.OwnerType == PersonalAccountOwnerType.Individual
                        ? string.Format("{0}_{1}_{2}#{3}", record.OwnerPhysSurname, record.OwnerPhysFirstName, record.OwnerPhysSecondName, record.AccountNumber)
                        : string.Format("{0}_{1}_{2}#{3}", record.OwnerJurName, record.OwnerJurInn, record.OwnerJurKpp, record.AccountNumber)).Replace(" ", string.Empty).ToLower());

            if (this.existAccount.ContainsKey(accountKey))
            {
                var acc = this.persAccCacheClient.Get(this.existAccount[accountKey]);
                if (acc != null)
                {
                    if (record.AccountNumber.IsEmpty())
                    {
                        record.AccountNumber = acc.PersonalAccountNum;
                    }

                    return acc;
                }
            }

            var account = new BasePersonalAccount
            {
                Room = apartment,
                AccountOwner = owner,
                AreaShare = record.AreaShare,
                OpenDate = record.AccountCreateDate,
                PersonalAccountNum = record.AccountNumber,
                State = this.accountDefaultState,
                PersAccNumExternalSystems = record.PersAccNumExternalSystems
            };

            account.SetCloseDate(DateTime.MinValue, false);

            if (account.OpenDate.Year < 2000)
            {
                account.OpenDate = DateTime.Now.Date;
            }

            foreach (var walletProp in this.Props)
            {
                var wallet = (Wallet)this.Accessor[account, walletProp.Name];

                if (wallet == null)
                {
                    wallet = (Wallet)Activator.CreateInstance(typeof(Wallet), Guid.NewGuid().ToString(), account, WalletHelper.GetWalletTypeByPropertyName(walletProp.Name));

                    this.Accessor[account, walletProp.Name] = wallet;
                }

                if (wallet.Id == 0)
                {
                    this.InsertOrUpdateObject(wallet);
                }
            }

            if (string.IsNullOrWhiteSpace(account.PersonalAccountNum))
            {
                this.AccountNumberService.Generate(account);
                record.AccountNumber = account.PersonalAccountNum;
                if (this.discriminateAccountsByNum)
                {
                    accountKey = string.Format(
                        "{0}#{1}#{2}#{3}#{4}",
                        record.RealtyObjectId, this.NormalizeRoomNum(apartment.RoomNum),
                        (record.OwnerType == PersonalAccountOwnerType.Individual
                            ? string.Format("{0}_{1}_{2}#{3}", record.OwnerPhysSurname, record.OwnerPhysFirstName, record.OwnerPhysSecondName, record.AccountNumber)
                            : string.Format("{0}_{1}_{2}#{3}", record.OwnerJurName, record.OwnerJurInn, record.OwnerJurKpp, record.AccountNumber)).Replace(" ", string.Empty).ToLower(),
                        record.AccountNumber,
                        record.PersAccNumExternalSystems);
                }
            }

            this.InsertOrUpdateAccount(account);
            this.ListBasePersonalAccountToSave.Add(account);
            this.newPersAccIds.Add(account.Id);
            this.persAccCacheClient.Add(account);
            this.existAccount[accountKey] = account.Id;

            var accSummary = new PersonalAccountPeriodSummary(account, this.curPeriod, 0m);
            this.InsertOrUpdateObject(accSummary);

            this.AddLog(record.RowNumber,
                string.Format("Добавлен новый лицевой счет с номером {0}; доля собственности {1}; дата открытия {2};",
                    account.PersonalAccountNum,
                    account.AreaShare.RegopRoundDecimal(2),
                    account.OpenDate.ToShortDateString()));

            return account;
        }

        private Room GetOrCreateRoom(Record record)
        {
            Room apartment = null;
            var apartmentNum = this.NormalizeRoomNum(record.Apartment);
            if (this.realtyObjectRoomsDict.ContainsKey(record.RealtyObjectId))
            {
                var realtyObjectRooms = this.realtyObjectRoomsDict[record.RealtyObjectId];
                if (realtyObjectRooms.ContainsKey(apartmentNum))
                {
                    // Необходимо рассматривать только те записи которые в списке как Id а не как объект
                    // Если запись как объект значит она уже учтена в рачете и ее редактирвоать ненадо
                    if (realtyObjectRooms[apartmentNum] is long)
                    {
                        apartment = this.roomCacheClient.Get((long)realtyObjectRooms[apartmentNum]);
                    }
                    else if (realtyObjectRooms[apartmentNum] is Room)
                    {
                        apartment = (Room)realtyObjectRooms[apartmentNum];
                    }

                    if (apartment == null)
                    {
                        throw new RedisException("Ошибка инициализации кеша - отсутствуют необходимые значения");
                    }

                    if (this.replaceExistRooms)
                    {
                        DateTime lastActualSinceLog = DateTime.MinValue;

                        if (!this.roomAreaLastLogDict.ContainsKey(apartment.Id))
                        {
                            this.RoomLogToCreateDict[apartment] = new ProxyLogValue
                            {
                                DateActualChange = record.AccountCreateDate,
                                Value = record.Area.ToString("G29", this.culture)
                            };
                        }
                        else
                        {
                            lastActualSinceLog = this.roomAreaLastLogDict[apartment.Id];

                            if (apartment.Area != record.Area || lastActualSinceLog != record.AccountCreateDate)
                            {
                                this.RoomLogToCreateDict[apartment] = new ProxyLogValue
                                {
                                    DateActualChange = record.AccountCreateDate,
                                    Value = record.Area.ToString("G29", this.culture)
                                };
                            }
                        }

                        if (apartment.Area != record.Area
                            || apartment.LivingArea != record.LivingArea
                            || apartment.Type != record.RoomType
                            || apartment.OwnershipType != record.OwnershipType
                            || apartment.CadastralNumber != record.CadastralNumber)
                        {
                            this.AddLog(record.RowNumber,
                                string.Format(
                                    "Изменение информации о помещении." +
                                    "Старые значения: площадь {0}; жилая площадь {1}; тип помещения {2}; тип собственности {3}; кадастровый номер {4}." +
                                    "Новые значения: площадь {5}; жилая площадь {6}; тип помещения {7}; тип собственности {8}; кадастровый номер {9}",
                                    apartment.Area,
                                    apartment.LivingArea,
                                    apartment.Type.GetEnumMeta().Display,
                                    apartment.OwnershipType.GetEnumMeta().Display,
                                    apartment.CadastralNumber,
                                    record.Area,
                                    record.LivingArea,
                                    record.RoomType.GetEnumMeta().Display,
                                    record.OwnershipType.GetEnumMeta().Display,
                                    record.CadastralNumber));

                            HashSet<string> changes = null;

                            if (apartment.Id != 0)
                            {
                                if (this.ListRoomToSave.ContainsKey(apartment.Id))
                                {
                                    changes = this.ListRoomToSave[apartment.Id];
                                }
                            }

                            if (changes == null)
                            {
                                changes = new HashSet<string>();
                            }

                            if (apartment.Area != record.Area)
                            {
                                apartment.Area = record.Area;
                                changes.Add("room_area");
                            }

                            if (record.LivingArea > 0)
                            {
                                apartment.LivingArea = record.LivingArea;
                            }

                            apartment.Type = record.RoomType;

                            if (apartment.OwnershipType != record.OwnershipType)
                            {
                                apartment.OwnershipType = record.OwnershipType;
                                changes.Add("room_ownership_type");
                            }

                            if (apartment.CadastralNumber != record.CadastralNumber)
                            {
                                apartment.CadastralNumber = record.CadastralNumber;
                                changes.Add("room_cadastral_num");
                            }

                            if (this.ListRoomToSave.ContainsKey(apartment.Id))
                            {
                                this.ListRoomToSave[apartment.Id] = changes;
                            }

                            this.InsertOrUpdateObject(apartment);
                            this.roomCacheClient.Add(apartment);
                        }
                    }
                }
                else
                {
                    apartment = this.NewApartment(record);
                    this.ListRoomToSave.Add(apartment.Id, null);
                    realtyObjectRooms[apartmentNum] = apartment;
                }
            }
            else
            {
                apartment = this.NewApartment(record);
                var realtyObjectRooms = new Dictionary<string, object>();
                this.ListRoomToSave.Add(apartment.Id, null);
                realtyObjectRooms[apartmentNum] = apartment;
                this.realtyObjectRoomsDict[record.RealtyObjectId] = realtyObjectRooms;
            }

            return apartment;
        }

        private void GetRkcRelation(Record record, BasePersonalAccount persAcc)
        {
            if (record.RkcIdentifier == null || record.RkcIdentifier.IsEmpty())
            {
                return;
            }

            if (persAcc == null)
            {
                this.AddLog(record.RowNumber, "Не создана привязка для РКЦ, лицевой счет не был создан или найден из за ошибки");
            }

            if (!record.RkcDateStart.HasValue)
            {
                this.AddLog(record.RowNumber, "Не указана дата начала действия договора с РКЦ");
                return;
            }

            if (!this.cashPaymentCenterIdDict.ContainsKey(record.RkcIdentifier))
            {
                this.AddLog(record.RowNumber, "Не найден РКЦ с данным идентификатором: {0}".FormatUsing(record.RkcIdentifier));
                return;
            }

            if (record.RkcDateEnd.HasValue && record.RkcDateStart > record.RkcDateEnd)
            {
                this.AddLog(record.RowNumber, "Дата окончания действия договора с РКЦ меньше чем дата начала ");
                return;
            }

            var cashCenterPayId = this.cashPaymentCenterIdDict.Get(record.RkcIdentifier);
            var contracts = new List<CashPaymentCenterPersAccProxy>();
            if (persAcc != null)
            {
                contracts = this.cashPaymentCenterPersAccDict.Get(persAcc.Id) ?? new List<CashPaymentCenterPersAccProxy>();
            }

            var existContract = contracts.FirstOrDefault(x => (x.DateStart <= record.RkcDateStart && (!x.DateEnd.HasValue || x.DateEnd >= record.RkcDateStart))
                                                              || (!record.RkcDateEnd.HasValue && x.DateStart >= record.RkcDateStart)
                                                              || (record.RkcDateEnd.HasValue && x.DateStart <= record.RkcDateEnd && (!x.DateEnd.HasValue || x.DateEnd >= record.RkcDateEnd)));

            if (existContract == null)
            {
                this.cashPayCenterPersAccToSave.Add(new CashPaymentCenterPersAcc
                {
                    CashPaymentCenter = this.CashPaymentCenterDomain.Load(cashCenterPayId),
                    PersonalAccount = persAcc,
                    DateStart = record.RkcDateStart.ToDateTime(),
                    DateEnd = record.RkcDateEnd
                });
            }
            else
            {
                if (existContract.CashPaymentCenterId != cashCenterPayId)
                {
                    this.AddLog(record.RowNumber, "Данный счет имеет договор с другим РКЦ");
                    return;
                }

                if (existContract.DateStart != record.RkcDateStart)
                {
                    this.AddLog(record.RowNumber, "Не совпадает дата начала действия договора с РКЦ. По файлу: {0}. В системе: {1}"
                        .FormatUsing(record.RkcDateStart.ToDateTime().ToShortDateString(), existContract.DateStart.ToShortDateString()));
                    return;
                }

                if (!existContract.DateEnd.HasValue && record.RkcDateEnd.HasValue)
                {
                    var contract = this.CashPaymentCenterPersAccDomain.Load(existContract.Id);
                    contract.DateEnd = record.RkcDateEnd;
                    this.cashPayCenterPersAccToSave.Add(contract);
                }
            }
        }

        private void CreateOrUpdateAccount(Record record, Room apartment, PersonalAccountOwner owner)
        {
            BasePersonalAccount account = null;

            var key = string.Format("{0}_{1}", record.RealtyObjectId, this.NormalizeRoomNum(apartment.RoomNum));
            if (this.accountByRoomAndOwnerDict.ContainsKey(key))
            {
                // лс по номеру
                var accountsInRoom = this.accountByRoomAndOwnerDict[key];

                // лс по внешнему номеру
                var accountsInRoomExternal = this.accountByExternalNumDict[key];

                try
                {
                    account = this.NewAccount(record, apartment, owner);
                }
                catch (Exception e)
                {
                    this.AddLog(record.RowNumber, e.Message, false, e);
                    return;
                }

                object accountInfo = accountsInRoom.Get(record.AccountNumber);

                if (accountInfo == null && !string.IsNullOrWhiteSpace(record.PersAccNumExternalSystems))
                {
                    accountInfo = accountsInRoomExternal.Get(record.PersAccNumExternalSystems);
                }

                if (accountInfo != null)
                {
                    if (accountInfo is long)
                    {
                        object roomAccount;

                        if (accountsInRoom.TryGetValue(record.AccountNumber, out roomAccount))
                        {
                            BasePersonalAccount basePersonalAccount = this.persAccCacheClient.Get((long)roomAccount);

                            if (basePersonalAccount != null)
                            {
                                account = basePersonalAccount;
                            }
                        }
                    }
                    else if (accountInfo is BasePersonalAccount)
                    {
                        account = (BasePersonalAccount)accountInfo;
                    }

                    if (account != null)
                    {
                        if (this.accountAreaShareLastLogDict.ContainsKey(account.Id))
                        {
                            var lastActualSinceLog = this.accountAreaShareLastLogDict[account.Id];

                            if (account.AreaShare != record.AreaShare
                                || lastActualSinceLog != record.AccountCreateDate)
                            {
                                this.AccountAreaShareLogToCreateDict[account] = new ProxyLogValue
                                {
                                    DateActualChange = record.AccountCreateDate,
                                    Value = record.AreaShare.ToString("G29", this.culture)
                                };
                            }

                            if (account.AreaShare != record.AreaShare && lastActualSinceLog < record.AccountCreateDate)
                            {
                                account.AreaShare = record.AreaShare;
                            }
                        }

                        if (!this.accountOpenDateLastLogDict.ContainsKey(account.Id))
                        {
                            this.AccountAreaShareLogToCreateDict[account] = new ProxyLogValue
                            {
                                DateActualChange = record.AccountCreateDate,
                                Value = record.AccountCreateDate.ToString("dd.MM.yyyy")
                            };
                        }
                        else
                        {
                            var lastActualSinceLog = this.accountOpenDateLastLogDict[account.Id];

                            if (account.OpenDate != record.AccountCreateDate
                                || lastActualSinceLog != record.AccountCreateDate)
                            {
                                this.AccountOpenDateLogToCreateDict[account] = new ProxyLogValue
                                {
                                    DateActualChange = record.AccountCreateDate,
                                    Value = record.AccountCreateDate.ToString("dd.MM.yyyy")
                                };
                            }

                            if (account.OpenDate != record.AccountCreateDate && lastActualSinceLog < record.AccountCreateDate)
                            {
                                account.OpenDate = record.AccountCreateDate;
                            }
                        }

                        if (this.replaceExistRooms)
                        {
                            account.PersAccNumExternalSystems = record.PersAccNumExternalSystems;
                        }

                        this.ListBasePersonalAccountToSave.Add(account);

#warning костыль от Redis'а
                        //сей костыль нужен для случая, когда инфа по лс грузится второй раз,
                        //т.к. в этом случае для значения, равного DateTime.MinValue, добавляется TimeZone.
                        //из-за этого ломается фильтрация ЛС
                        if (account.CloseDate.Date == DateTime.MinValue)
                            account.SetCloseDate(DateTime.MinValue, false);

                        this.InsertOrUpdateAccount(account);
                    }
                }
                else
                {
                    this.ListBasePersonalAccountToSave.Add(account);
                    accountsInRoom[record.AccountNumber] = account;
                    this.InsertOrUpdateAccount(account);
                }
            }
            else
            {
                try
                {
                    account = this.NewAccount(record, apartment, owner);
                    this.ListBasePersonalAccountToSave.Add(account);

                    var accountsInRoom = new Dictionary<string, object>();
                    accountsInRoom[record.AccountNumber] = account;
                    this.accountByRoomAndOwnerDict[key] = accountsInRoom;

                    var accountsInRoomExternal = new Dictionary<string, object>();
                    if (!string.IsNullOrWhiteSpace(record.PersAccNumExternalSystems))
                    {
                        accountsInRoomExternal[record.PersAccNumExternalSystems] = account;
                    }
                    this.accountByExternalNumDict[key] = accountsInRoomExternal;

                    this.InsertOrUpdateAccount(account);
                }
                catch (Exception e)
                {
                    var errorMessage = e.InnerException != null
                        ? e.InnerException.Message + " (" + e.Message + ")"
                        : e.Message;

                    this.AddLog(record.RowNumber, errorMessage, false, e);
                }
            }

           this.GetRkcRelation(record, account);
        }

        private bool AccountHasDefaultState()
        {
            var stateProvider = this.Container.Resolve<IStateProvider>();
            var stateRepo = this.Container.Resolve<IStateRepository>();

            try
            {
                var account = new BasePersonalAccount();
                stateProvider.SetDefaultState(account);
                this.accountDefaultState = account.State;

                this.accountStartState = stateRepo.GetAllStates<BasePersonalAccount>().Where(x => x.Name == "Открыт").FirstOrDefault();

                return account.State != null && this.accountStartState != null;
            }
            finally
            {
                this.Container.Release(stateProvider);
                this.Container.Release(stateRepo);
            }
        }

        private void InTransaction()
        {
            var login = this.UserRepo.Get(this.Identity.UserId).Return(u => u.Login);

            if (login.IsEmpty())
            {
                login = "anonymous";
            }

            this.PrepareEntityLogToSave(login);

            foreach (var item in this.ListRoomToSave)
            {
                var room = this.roomCacheClient.Get(item.Key);
                this.LogEntity(room, login, item.Value);
            }
            foreach (var item in this.ListBasePersonalAccountToSave)
            {
                this.LogEntity(item, login, null);
            }

            this.ListRoomToSave.Clear();
            this.ListBasePersonalAccountToSave.Clear();

            this.CloseTransaction();

            TransactionHelper.InsertInManyTransactions(this.Container, this.entityLogLightListToCreate, 1000, true, true);
            TransactionHelper.InsertInManyTransactions(this.Container, this.cashPayCenterPersAccToSave, 1000, true, true);
        }

        private void LogEntity(IEntity entity, string login, HashSet<string> changes)
        {
            if (VersionedEntityHelper.IsUnderVersioning(entity))
            {
                var parameters =
                    VersionedEntityHelper.GetCreator(entity)
                        .CreateParameters()
                        .Where(x => !VersionedEntityHelper.ShouldSkip(entity, x.ParameterName))
                        .ToList();

                var hasDate = entity as IHasDateActualChange;
                DateTime? dateActualChange = null;
                var now = DateTime.UtcNow;

                if (entity is Room)
                {
                    dateActualChange = this.firstPeriod != null ? this.firstPeriod.StartDate : now;
                }

                if (entity is BasePersonalAccount)
                {
                    var persAcc = entity as BasePersonalAccount;
                    dateActualChange = persAcc.OpenDate;
                }

                parameters
                    .WhereIf(changes != null && changes.Any(), x => changes.Contains(x.ParameterName))
                    .ForEach(x =>
                    {
                        var save = new EntityLogLight
                        {
                            EntityId = entity.Id.To<long>(),
                            ClassName = x.ClassName,
                            PropertyName = x.PropertyName,
                            PropertyValue = x.PropertyValue,
                            ParameterName = x.ParameterName,
                            DateApplied = now,
                            DateActualChange = dateActualChange.HasValue
                                ? dateActualChange.ToDateTime()
                                : hasDate.Return(y => y.ActualChangeDate,
                                    new DateTime(now.Year, now.Month, 1, now.Hour, now.Minute, now.Second,
                                        now.Millisecond)),
                            User = login,
                            ObjectCreateDate = now,
                            ObjectEditDate = now
                        };

                        if (save.PropertyName == "OwnershipType")
                        {
                            var value = this.ownerShipTypes.FirstOrDefault(p => p.ToStr() == save.PropertyValue);

                            save.PropertyValue = value.GetEnumMeta().Display;
                        }

                        this.InsertOrUpdateObject(save);
                    });
            }
        }

        private void PrepareEntityLogToSave(string login)
        {
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
                    foreach (var pair in this.AccountAreaShareLogToCreateDict)
                    {
                        var log = new EntityLogLight
                        {
                            EntityId = pair.Key.Id,
                            ClassName = areaShareParameter.ClassName,
                            PropertyName = areaShareParameter.PropertyName,
                            PropertyValue = pair.Value.Value,
                            ParameterName = areaShareParameter.ParameterName,
                            DateApplied = now,
                            DateActualChange = pair.Value.DateActualChange,
                            User = login,
                            ObjectCreateDate = now,
                            ObjectEditDate = now
                        };

                        if (log.PropertyName == "OwnershipType")
                        {
                            var value = this.ownerShipTypes.FirstOrDefault(p => p.ToStr() == log.PropertyValue);

                            log.PropertyValue = value.GetEnumMeta().Display;
                        }

                        this.entityLogLightListToCreate.Add(log);
                    }
                }

                if (accountOpenDateParameter != null)
                {
                    foreach (var pair in this.AccountOpenDateLogToCreateDict)
                    {
                        var log = new EntityLogLight
                        {
                            EntityId = pair.Key.Id,
                            ClassName = accountOpenDateParameter.ClassName,
                            PropertyName = accountOpenDateParameter.PropertyName,
                            PropertyValue = pair.Value.Value,
                            ParameterName = accountOpenDateParameter.ParameterName,
                            DateApplied = now,
                            DateActualChange = pair.Value.DateActualChange,
                            User = login,
                            ObjectCreateDate = now,
                            ObjectEditDate = now
                        };

                        this.entityLogLightListToCreate.Add(log);
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
                    foreach (var pair in this.RoomLogToCreateDict)
                    {
                        var log = new EntityLogLight
                        {
                            EntityId = pair.Key.Id,
                            ClassName = roomAreaParameter.ClassName,
                            PropertyName = roomAreaParameter.PropertyName,
                            PropertyValue = pair.Value.Value,
                            ParameterName = roomAreaParameter.ParameterName,
                            DateApplied = now,
                            DateActualChange = pair.Value.DateActualChange,
                            User = login,
                            ObjectCreateDate = now,
                            ObjectEditDate = now
                        };

                        this.entityLogLightListToCreate.Add(log);
                    }
                }
            }
        }

        /// <summary>
        ///     Returns a string that has space removed from the start and the end, and that has each sequence of internal space
        ///     replaced with a single space.
        /// </summary>
        /// <param name="initialString"></param>
        /// <returns></returns>
        private static string Simplified(string initialString)
        {
            if (string.IsNullOrEmpty(initialString))
            {
                return initialString;
            }

            var trimmed = initialString.Trim();

            if (!trimmed.Contains(" "))
            {
                return trimmed;
            }

            var result = string.Join(" ", trimmed.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)));

            return result;
        }

        private string NormalizeRoomNum(string roomNum)
        {
            roomNum = roomNum ?? string.Empty;

            if (!this.roomNumNormalizationDisabled)
            {
                roomNum = roomNum.Replace(" ", string.Empty).ToLower();
            }

            return roomNum;
        }

        private class FiasProxy
        {
            public string CodeRecord { get; set; }

            public FiasLevelEnum AOLevel { get; set; }

            public string AOGuid { get; set; }

            public string ParentGuid { get; set; }
        }

        private class FiasAddressProxy
        {
            public long Id { get; set; }

            public string PlaceGuidId { get; set; }

            public string StreetGuidId { get; set; }

            public string House { get; set; }

            public string Housing { get; set; }

            public string Letter { get; set; }

            public string Building { get; set; }

            public string HouseLetter { get; set; }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (this.contragentCacheClient != null)
            {
                this.contragentCacheClient.Dispose();
                this.Logger.LogDebug("Dispose() ContragentCacheClient.Dispose();: AppCache._useCount = {0}", this.GetAppCacheInUse());
            }

            if (this.persAccCacheClient != null)
            {
                this.persAccCacheClient.Dispose();
                this.Logger.LogDebug("Dispose() PersAccCacheClient.Dispose();: AppCache._useCount = {0}", this.GetAppCacheInUse());
            }

            if (this.roomCacheClient != null)
            {
                this.roomCacheClient.Dispose();
                this.Logger.LogDebug("Dispose() RoomCacheClient.Dispose();: AppCache._useCount = {0}", this.GetAppCacheInUse());
            }

            if (this.realityObjectCacheClient != null)
            {
                this.realityObjectCacheClient.Dispose();
                this.Logger.LogDebug("Dispose() RealityObjectCacheClient.Dispose(): AppCache._useCount = {0}", this.GetAppCacheInUse());
            }
        }
    }

     struct ContragentInfoProxy
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// Служебный класс CashPaymentCenterPersAccProxy
    /// </summary>
    internal class CashPaymentCenterPersAccProxy
    {
        public long Id { get; set; }
        public long CashPaymentCenterId { get; set; }
        public long PersAccId { get; set; }
        public string AccNum { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
    }

    /// <summary>
    /// Служебный класс ProxyLogValue
    /// </summary>
    public class ProxyLogValue
    {
        /// <summary>
        /// DateActualChange
        /// </summary>
        public DateTime DateActualChange { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        public string Value { get; set; }
    }
}