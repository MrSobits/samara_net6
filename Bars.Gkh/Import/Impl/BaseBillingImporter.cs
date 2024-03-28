namespace Bars.Gkh.Import
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;

    using Castle.Windsor;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    /// <summary>
    /// Базовый импортер из биллинга
    /// </summary>
    public abstract class BaseBillingImporter<T> : IBillingFileImporter
        where T : BaseEntity
    {
        protected List<T> Items;
        private List<T> cache;

        /// <summary>
        /// Кэш
        /// </summary>
        protected List<T> Cache => this.cache ?? (this.cache = this.Domain.GetAll().ToList());

        /// <summary>
        /// Название архива
        /// </summary>
        protected string ArchiveName { get; set; }

        /// <summary>
        /// Интерфейс идентификатора пользователя
        /// </summary>
        protected IUserIdentity UserIdentity { get; set; }

        /// <summary>
        ///     Количество частей в строке
        /// </summary>
        public abstract int SplitCount { get; }

        /// <summary>
        /// Индикатор прогресса
        /// </summary>
        protected IProgressIndicator Indicator { get; set; }

        /// <summary>
        /// Логгер
        /// </summary>
        protected ILogImport Logger { get; set; }

        /// <summary>
        /// Название файла
        /// </summary>
        public abstract string FileName { get; }
        
        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File  { get; set; }

        /// <summary>
        /// Приоритет
        /// </summary>
        public abstract int Order { get; }

        /// <summary>
        /// Импорт
        /// <para>Закрывает поток после завершения.</para>
        /// </summary>
        public void Import(Stream fileStream, string archiveName, ILogImport logger = null, IProgressIndicator indicator = null, long userId = 0, object param = null)
        {
            using (fileStream)
            {
                using (var streamReader = new StreamReader(fileStream, Encoding.GetEncoding(1251)))
                {
                    var validateResult = this.Validate(streamReader);

                    if (!validateResult.Success)
                    {
                        throw new ValidationException(validateResult.Message);
                    }

                    this.UserIdentity
                        = userId > 0 ? new ProxyUserIdentity(userId) : null;
                    this.Indicator = indicator;
                    this.Logger = logger;
                    this.ArchiveName = archiveName;
                    this.InitDictionaries();
                    this.Items = new List<T>();

                    string line;

                    while (!streamReader.EndOfStream && (line = streamReader.ReadLine()) != null)
                    {
                        if (line.IsEmpty())
                        {
                            continue;
                        }

                        var splits = line.Split(new[] {"|"}, StringSplitOptions.None);

                        if (splits.Length < this.SplitCount)
                        {
                            throw new ArgumentException(
                                string.Format(
                                    "Invalid line detected. Expected part count: {3}. File: {0}, Archive: {1}, Line: {2}",
                                    this.FileName,
                                    archiveName,
                                    string.Join("|", splits),
                                    this.SplitCount));
                        }

                        this.ProcessLine(archiveName, splits);
                    }

                    var importedFile = param as FileInfo;

                    var fileManager = this.Container.Resolve<IFileManager>();
                    using (this.Container.Using(fileManager))
                    {
                        this.File = importedFile ?? fileManager.SaveFile(fileStream, archiveName);
                    }
                }
            }

            this.SaveRecords(this.Items);
        }

        /// <summary>
        /// Метод фильтрации при добавлении и перед сохранением
        /// </summary>
        /// <param name="other">Сравниваемая сущность</param>
        public abstract Func<T, bool> Predicate(T other);

        /// <summary>
        /// Инициализация словарей
        /// </summary>
        protected virtual void InitDictionaries()
        {
        }

        /// <summary>
        /// Валидация
        /// </summary>
        protected virtual ValidateResult Validate(StreamReader streamReader) => new ValidateResult(true);

        /// <summary>
        /// Обработать строку
        /// </summary>
        public virtual void ProcessLine(string archiveName, string[] splits)
        {
        }

        /// <summary>
        /// Обработать строку
        /// </summary>
        public virtual void ProcessLine(string archiveName, string[] splits, ILogImport logger)
        {
            this.ProcessLine(archiveName, splits);
        }

        /// <summary>
        /// Добавить
        /// </summary>
        protected virtual void Add(T item)
        {
            this.Items.Add(item);
        }

        /// <summary>
        /// Действие перед сохранением
        /// </summary>
        public virtual void BeforeSave(List<T> records)
        {
            var now = DateTime.Now;
            records.ForEach(
                x =>
                {
                    x.ObjectCreateDate = now;
                    x.ObjectEditDate = now;
                });
        }

        /// <summary>
        /// Сохранить записи
        /// </summary>
        protected virtual void SaveRecords(List<T> records)
        {
            if (records != null)
            {
                this.BeforeSave(records);

                TransactionHelper.InsertInManyTransactions(this.Container, records);

                records.Clear();
            }
        }

        private FiasAddress GetByBillingIdAndUid(string billingId, string uid, int length)
        {
            if (billingId.IsEmpty() || uid.Length < length)
            {
                return null;
            }

            // <billingId, uid>
            var key = Tuple.Create(billingId, uid.Substring(0, length));

            return this.AddressByBillingAndUid.Get(key);
        }

        /// <summary>
        /// Результат валидации
        /// </summary>
        protected class ValidateResult
        {
            public ValidateResult(bool success, string message = null)
            {
                this.Success = success;
                this.Message = message;
            }

            public bool Success { get; set; }

            public string Message { get; set; }
        }

        #region injection
        /// <summary>
        /// Домен-сервис
        /// </summary>
        public IDomainService<T> Domain { get; set; }

        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="FiasAddressUid"/>
        /// </summary>
        public IDomainService<FiasAddressUid> FiasUids { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="Municipality"/>
        /// </summary>
        public IDomainService<Municipality> MunicipalityDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="Contragent"/>
        /// </summary>
        public IDomainService<Contragent> ContrService { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="FiasAddress"/>
        /// </summary>
        public IDomainService<FiasAddress> FiasAddressDomain { get; set; }

        /// <summary>
        /// Репозиторий <see cref="RealityObject"/>
        /// </summary>
        public IRepository<RealityObject> RealityObjectDomain { get; set; }

        /// <summary>
        /// Сервис авторизации
        /// </summary>
        public IAuthorizationService AuthService { get; set; }

        /// <summary>
        /// Текущий пользователь
        /// </summary>
        public IUserIdentity CurrentUser { get; set; }
        #endregion injection

        /// <summary>
        /// Домен-сервис <see cref="UnitMeasure"/>
        /// </summary>
        public IDomainService<UnitMeasure> UnitMeasureService { get; set; }

        #region properties
        private readonly Dictionary<string, string> gkhUnitsCompare = new Dictionary<string, string>
        {
            {"кв.метр", "кв.м"},
            {"куб.м", "куб.метр"},
            {"кВт*час", "Квт/час"},
            {"неопределенная единица измерения", "-"},
            {string.Empty, "-"}
        };

        private IDictionary<string, long> unitMeasures;

        /// <summary>
        /// Единицы измерения
        /// </summary>
        public IDictionary<string, long> UnitMeasures
        {
            get
            {
                return this.unitMeasures ??
                    (this.unitMeasures = this.UnitMeasureService.GetAll()
                        .Where(x => x.Description != null && x.Description.Length > 0)
                        .Select(x => new {x.Description, x.Id})
                        .ToList()
                        .GroupBy(x => x.Description)
                        .ToDictionary(x => x.Key, x => x.FirstOrDefault().Return(y => y.Id)));
            }
        }

        private IDictionary<string, long> contragents;

        /// <summary>
        /// Контрагенты
        /// </summary>
        public IDictionary<string, long> Contragents
        {
            get
            {
                return this.contragents ??
                    (this.contragents = this.ContrService.GetAll()
                        .Where(x => x.Name != null && x.Name.Length > 0)
                        .Select(x => new {x.Name, x.Id})
                        .ToList()
                        .GroupBy(x => x.Name)
                        .ToDictionary(x => x.Key, x => x.FirstOrDefault().Return(y => y.Id)));
            }
        }

        private IDictionary<string, FiasAddress> fiasShortUids;

        /// <summary>
        /// Краткие идентификаторы ФИАС
        /// </summary>
        public IDictionary<string, FiasAddress> FiasShortuids
        {
            get
            {
                return this.fiasShortUids ??
                    (this.fiasShortUids = this.Fiasuids.Select(
                        x => new
                        {
                            Uid = x.Key.Substring(0, 20),
                            x.Value
                        }).ToDictionary(x => x.Uid, arg => arg.Value));
            }
        }

        private IDictionary<string, FiasAddress> fiasUids;

        private IDictionary<string, List<FiasAddress>> fiasBillingIds;

        /// <summary>
        /// Идентификаторы ФИАС
        /// </summary>
        public IDictionary<string, FiasAddress> Fiasuids
        {
            get
            {
                return this.fiasUids ??
                    (this.fiasUids = this.FiasUids.GetAll()
                        .Where(x => x.Uid != null && x.Uid.Length > 0)
                        .Select(x => new {x.Uid, x.Address})
                        .ToList()
                        .GroupBy(x => x.Uid)
                        .ToDictionary(x => x.Key, x => x.FirstOrDefault().Return(y => y.Address)));
            }
        }

        /// <summary>
        /// Словарь ФИАС
        /// </summary>
        public IDictionary<string, List<FiasAddress>> FiasBillingIds
        {
            get
            {
                return this.fiasBillingIds ??
                    (this.fiasBillingIds = this.FiasUids.GetAll()
                        .Where(x => x.BillingId != null && x.BillingId.Length > 0)
                        .Where(x => this.RealityObjectDomain.GetAll().Select(y => y.FiasAddress.Id).Contains(x.Address.Id))
                        .Select(x => new {x.BillingId, x.Address})
                        .ToList()
                        .GroupBy(x => x.BillingId)
                        .ToDictionary(x => x.Key, x => x.Select(y => y.Address).ToList()));
            }
        }

        private IDictionary<Tuple<string, string>, FiasAddress> byBillingAndUid;

        /// <summary>
        /// Словарь ФИАС
        /// </summary>
        public IDictionary<Tuple<string, string>, FiasAddress> AddressByBillingAndUid
        {
            get
            {
                return this.byBillingAndUid ?? (this.byBillingAndUid = this.FiasUids.GetAll()
                    .Where(x => x.BillingId != null && x.BillingId.Length > 0)
                    .Where(x => x.Uid != null && x.Uid.Length > 0)
                    .Where(x => this.RealityObjectDomain.GetAll().Select(y => y.FiasAddress.Id).Contains(x.Address.Id))
                    .Select(x => new {x.BillingId, x.Uid, x.Address})
                    .ToList()
                    .GroupBy(
                        x => new
                        {
                            x.BillingId,
                            x.Uid
                        })
                    .ToDictionary(
                        x => Tuple.Create(x.Key.BillingId, x.Key.Uid),
                        x => x.FirstOrDefault().Return(y => y.Address)));
            }
        }

        private IDictionary<string, long> municipalities;

        /// <summary>
        /// Муниципалитеты
        /// </summary>
        public IDictionary<string, long> Municipalitites
        {
            get
            {
                return this.municipalities ??
                    (this.municipalities = this.MunicipalityDomain.GetAll()
                        .Where(x => x.FiasId != null && x.FiasId.Length > 0)
                        .Select(x => new {x.FiasId, x.Id})
                        .ToList()
                        .GroupBy(x => x.FiasId)
                        .ToDictionary(x => x.Key, x => x.FirstOrDefault().Return(y => y.Id)));
            }
        }

        private IDictionary<long, string> fas;

        /// <summary>
        /// Словарь адресов ФИАС
        /// </summary>
        public IDictionary<long, string> FiasAddress
        {
            get
            {
                return this.fas ??
                    (this.fas = this.FiasAddressDomain.GetAll()
                        .Where(x => x.PlaceName != null && x.PlaceName != string.Empty)
                        .Select(x => new {x.AddressName, x.Id})
                        .ToList()
                        .GroupBy(x => x.Id)
                        .ToDictionary(x => x.Key, x => x.FirstOrDefault().ReturnSafe(y => y.AddressName)));
            }
        }

        private IDictionary<long, Tuple<long, long>> ros;

        /// <summary>
        /// Дома
        /// </summary>
        public IDictionary<long, Tuple<long, long>> RealityObjects
        {
            get
            {
                return this.ros ??
                    (this.ros = this.RealityObjectDomain.GetAll()
                        .Where(x => x.FiasAddress != null)
                        .Where(x => x.FiasAddress.PlaceName != null && x.FiasAddress.PlaceName != string.Empty)
                        .Select(x => new {FiasAddressId = x.FiasAddress.Id, x.Id, MunId = x.Municipality.Id})
                        .ToList()
                        .GroupBy(x => x.FiasAddressId)
                        .ToDictionary(
                            x => x.Key,
                            x => new Tuple<long, long>(x.FirstOrDefault().Return(y => y.Id), x.FirstOrDefault().Return(y => y.MunId))));
            }
        }
        #endregion properties

        #region Helpers
        /// <summary>
        /// Получить идентификатор импорта
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="archiveName"></param>
        /// <returns></returns>
        protected string GetImportId(string fileId, string archiveName)
        {
            return $"{archiveName}.{fileId}";
        }

        /// <summary>
        /// Получить муниципалитет
        /// </summary>
        /// <param name="addresUid"></param>
        /// <returns></returns>
        protected Municipality GetMunicipality(string addresUid)
        {
            bool isWrongErcBinding = false;
            return this.GetRealityObject(addresUid, out isWrongErcBinding).Return(x => x.Municipality);
        }

        /// <summary>
        /// Получить дом
        /// </summary>
        /// <param name="addressUid"></param>
        /// <param name="bilingId"></param>
        /// <returns></returns>
        protected RealityObject GetRealityObject(string addressUid, out bool isWrongErcBinding, string bilingId = "")
        {
            isWrongErcBinding = false;
            addressUid = addressUid.Trim();

            FiasAddress address = this.GetByBillingIdAndUid(bilingId, addressUid, 20) ?? 
                                  this.GetByBillingIdAndUid(bilingId, addressUid, 36) ??
                                  (addressUid.Length == 20 ? 
                                      this.FiasShortuids.Get(addressUid) : this.Fiasuids.Get(addressUid));

            if (address == null && !string.IsNullOrEmpty(bilingId))
            {
                var addressList = this.FiasBillingIds.Get(bilingId);

                if (addressList == null)
                {
                    return null;
                }
                
                if (addressList.Count > 1)
                {
                    isWrongErcBinding = true;
                    return null;
                }

                address = addressList[0];
            }

            if (address == null && addressUid.Length > 0 && addressUid.Length != 36 && addressUid.Length != 20)
            {
                return null;
            }

            if (address == null)
            {
                return null;
            }

            /* RealityObjects - словарь, который в виде ключа содержит RealityObject и Municipality */
            var ro = this.RealityObjects.Get(address.Id);

            var id = ro?.Item1 ?? 0;

            return id > 0 ? new RealityObject { Id = id, Municipality = new Municipality { Id = ro.Item2 } } : null;
        }

        /// <summary>
        /// Получить единицу измерения
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected UnitMeasure GetUnitMeasure(string name)
        {
            if (name == null)
            {
                name = string.Empty;
            }

            if (this.gkhUnitsCompare.ContainsKey(name))
            {
                name = this.gkhUnitsCompare[name.Trim()];
            }

            var umName = name.ToLower();

            var unitId = this.UnitMeasures.ContainsKey(umName) ? this.UnitMeasures[umName] : 0;

            if (unitId == 0)
            {
                var unit = new UnitMeasure
                {
                    Description = umName,
                    Name = umName,
                    ShortName = umName
                };

                this.UnitMeasureService.Save(unit);

                this.UnitMeasures.Add(unit.Name, unit.Id);

                return unit;
            }

            return new UnitMeasure { Id = unitId };
        }
        #endregion Helpers
    }

    /// <summary>
    /// Расширения
    /// </summary>
    public static class BillingExtensions
    {
        /// <summary>
        /// Преобразовать в bool
        /// </summary>
        public static bool ToSillyBool(this string val)
        {
            if (val.IsEmpty())
            {
                return false;
            }

            if (val.ToLowerInvariant() == "t")
            {
                return true;
            }

            return val.ToBool();
        }

        /// <summary>
        /// Преобразовать в int без проверки переполнения
        /// </summary>
        public static int ToUncheckedInt(this object val)
        {
            return unchecked((int)val.ToDecimal());
        }
    }

    /// <summary>
    /// Прокси класс пользователя
    /// </summary>
    public class ProxyUserIdentity : IUserIdentity
    {
        public ProxyUserIdentity(long userId)
        {
            this.UserId = userId;
        }

        public string Name { get; private set; }

        public string AuthenticationType { get; private set; }

        public bool IsAuthenticated { get; private set; }

        public long UserId { get; private set; }

        public string TrackId { get; private set; }
    }
}