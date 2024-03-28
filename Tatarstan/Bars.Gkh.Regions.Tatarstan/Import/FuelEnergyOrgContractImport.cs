namespace Bars.Gkh.Regions.Tatarstan.Import
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.Gkh.Import.Impl;
    using Bars.Gkh.Modules.Gkh1468.Entities;
    using Bars.Gkh.Regions.Tatarstan.DomainService;
    using Bars.Gkh.Regions.Tatarstan.Entities;
    using Bars.Gkh.Utils;

    using NHibernate.Linq;

    /// <summary>
    /// Загрузка данных по договорам ресурсоснабжения
    /// </summary>
    public class FuelEnergyOrgContractImport : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        private readonly IList<FuelEnergyOrgContractInfo> listToSave = new List<FuelEnergyOrgContractInfo>();
        private readonly IList<string> splits = new List<string>();

        private ImportParams importParams;
        private Contragent currentContragent;
        private ContractPeriod period;

        private PublicServiceOrg publicServiceOrg;

        private IDictionary<string, Dictionary<string, FuelEnergyOrgContractInfo>> dictData;
       
        /// <inheritdoc />
        public override string Key => FuelEnergyOrgContractImport.Id;

        /// <inheritdoc />
        public override string CodeImport => "TatarstanImport";

        /// <inheritdoc />
        public override string Name => "Импорт данных по договорам ресурсоснабжения (ТЭР)";

        /// <inheritdoc />
        public override string PossibleFileExtensions => "csv";

        /// <inheritdoc />
        public override string PermissionName => string.Empty;

        /// <summary>
        /// Менеджер пользователей
        /// </summary>
        public IGkhUserManager UserManager { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="Contragent"/>
        /// </summary>
        public IDomainService<Contragent> ContragentDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="ContractPeriod"/>
        /// </summary>
        public IDomainService<ContractPeriod> ContractPeriodDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="PublicServiceOrg"/>
        /// </summary>
        public IDomainService<PublicServiceOrg> PublicServiceOrgDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="BudgetOrgContractPeriodSumm"/>
        /// </summary>
        public IDomainService<FuelEnergyOrgContractInfo> FuelEnergyOrgContractInfoDomain { get; set; }

        /// <summary>
        /// Интерфейс работы с расщеплением платежей
        /// </summary>
        public IChargeSplittingService ChargeSplittingService { get; set; }

        /// <inheritdoc />
        protected override ImportResult ImportUsingGkhApi(BaseParams baseParams)
        {
            string errorMessage;
            if (!this.Validate(baseParams, out errorMessage))
            {
                return new ImportResult(StatusImport.CompletedWithError, errorMessage);
            }

            using (var memoryStream = new MemoryStream(baseParams.Files.First().Value.Data))
            using (var streamReader = new StreamReader(memoryStream, Encoding.GetEncoding(1251)))
            {
                var validateResult = this.ValidateAndInitInternal(streamReader);
                if (!validateResult.Success)
                {
                    return new ImportResult(StatusImport.CompletedWithError, validateResult.Message);
                }

                this.Indicate(5, "Инициализация кэша");
                this.InitCache();

                this.Indicate(10, "Чтение файла");
                this.ImportInternal(streamReader);

                this.Indicate(90, "Сохранение данных");
                this.Container.InTransaction(() =>
                {
                    TransactionHelper.InsertInManyTransactions(this.Container, this.listToSave);
                });
            }

            return new ImportResult();
        }

        private IDataResult ValidateAndInitInternal(StreamReader streamReader)
        {
            // Читаем первую строку
            streamReader.ReadLine();

            try
            {
                // Читаем вторую строку о поставщике информации
                var inputString = streamReader.ReadLine().Split(';');
                this.importParams = new ImportParams
                {
                    Inn = inputString[1],
                    Kpp = inputString[2],
                    Ogrn = inputString[3],
                    Year = inputString[4].ToInt(),
                    Month = inputString[5].ToInt(),
                    ProviderCode = (ProviderCode)inputString[9].ToInt()
                };

                return this.InitParams();
            }
            catch (Exception)
            {
                return BaseDataResult.Error("Отсутствует строка заголовка");
            }
        }
        
        private IDataResult InitParams()
        {
            var userContragentIds = this.UserManager.GetContragentIds();
            this.currentContragent = this.ContragentDomain.GetAll().FirstOrDefault(x => x.Inn == this.importParams.Inn && x.Kpp == this.importParams.Kpp);

            if (this.currentContragent.IsNull())
            {
                return BaseDataResult.Error("Не найден контрагент по указанным ИНН и КПП");
            }

            // если к оператору привязан контрагент, то проверяем, что ему можно грузить
            if (userContragentIds.Any() && !userContragentIds.Contains(this.currentContragent.Id))
            {
                return BaseDataResult.Error("Текущий оператор не имеет прав доступа на импорт указанного файла");
            }

            switch (this.importParams.ProviderCode)
            {
                case ProviderCode.Rso:
                    this.publicServiceOrg = this.PublicServiceOrgDomain.GetAll().FirstOrDefault(x => x.Contragent.Id == this.currentContragent.Id);
                    break;
                default:
                    return BaseDataResult.Error("Ошибка при определении типа поставщика информации");
            }

            if (this.publicServiceOrg.IsNull())
            {
                return BaseDataResult.Error("Не найден указанный поставщик информации");
            }

            this.period = this.ContractPeriodDomain.GetAll()
                    .FirstOrDefault(x => x.StartDate.Year == this.importParams.Year && x.StartDate.Month == this.importParams.Month);

            if (this.period.IsNull())
            {
                return BaseDataResult.Error("Не сформирован указанный период");
            }

            return new BaseDataResult();
        }

        private void InitCache()
        {
            var contractQuery = this.FuelEnergyOrgContractInfoDomain.GetAll()
                .Fetch(x => x.FuelEnergyResourceContract)
                .ThenFetch(x => x.FuelEnergyResourceOrg)
                .ThenFetch(x => x.Contragent)
                .Where(x => x.PeriodSummary.ContractPeriod.Id == this.period.Id)
                .Where(x => x.FuelEnergyResourceContract.FuelEnergyResourceOrg == this.publicServiceOrg);

            Func<FuelEnergyOrgContractInfo, string> keyFunc =
                x =>
                    $"{x.FuelEnergyResourceContract.FuelEnergyResourceOrg.Contragent.Inn}_" +
                    $"{x.FuelEnergyResourceContract.FuelEnergyResourceOrg.Contragent.Kpp}_" +
                    $"{x.PeriodSummary.PublicServiceOrg.Contragent.Inn}_" +
                    $"{x.PeriodSummary.PublicServiceOrg.Contragent.Kpp}_";

            this.dictData = contractQuery.AsEnumerable()
                .GroupBy(keyFunc)
                .ToDictionary(x => x.Key, y => y.ToDictionary(x => x.Resource.Code));
        }

        private void ImportInternal(StreamReader streamReader)
        {
            streamReader.ReadLine(); // читаем строку с шапкой

            while (!streamReader.EndOfStream)
            {
                this.splits.Add(streamReader.ReadLine());
            }

            var rowNum = 3; // 3-я строка, т.к. первые 2 - это шапка
            var splitsCount = (double)this.splits.Count + 2; // количество строк
            foreach (var split in this.splits)
            {
                var percent = (Math.Min((rowNum - 1) / splitsCount * 100, 100) * 0.8) + 10;

                if (split.IsNotEmpty())
                {
                    this.Indicate((int)percent, $"Обработка строки {rowNum} из {splitsCount}");
                    this.ProcessLine(rowNum++, split.Split(';'));
                }
            }
        }

        private void ProcessLine(int rowNum, string[] splits)
        {
            var dataExtractor = new DataExtractor();
            var record = Record.Parse(splits, dataExtractor);

            if (!record.IsValid)
            {
                var rowIdtext = $"rowNum = {rowNum}";
                this.LogImport.Warn(rowIdtext, record.Errors.AggregateWithSeparator(","));
                return;
            }

            var processResult = this.ProcessRecord(record, dataExtractor);
            if (!processResult.Success)
            {
                var rowIdtext = $"rowNum = {rowNum}";
                this.LogImport.Warn(rowIdtext, processResult.Message);
            }
        }

        private IDataResult ProcessRecord(Record record, DataExtractor dataExtractor)
        {
            if (record.FuelEnergyResourceOrgInn != this.publicServiceOrg.Contragent.Inn || record.FuelEnergyResourceOrgKpp != this.publicServiceOrg.Contragent.Kpp)
            {
                return BaseDataResult.Error("Импортируемая информация принадлежит другому поставщику ТЭР");
            }

            var entity = this.GetEntity(record);

            if (entity.IsNull())
            {
                return BaseDataResult.Error("Не удалось определить договор по связке РСО и Бюджетная организация");
            }

            if (dataExtractor.MergeData(record, entity))
            {
                this.listToSave.Add(entity);
                this.LogImport.CountChangedRows++;
            }

            return new BaseDataResult();
        }

        private FuelEnergyOrgContractInfo GetEntity(Record record)
        {
            var key = $"{record.FuelEnergyResourceOrgInn}_{record.FuelEnergyResourceOrgKpp}_{record.InnRso}_{record.KppRso}";
            return this.dictData.Get(key)?.Get(record.ResourceCode);
        }

        private class DataExtractor
        {
            private readonly Dictionary<string, PropertyInfo> propertyInfos = new Dictionary<string, PropertyInfo>();
            private readonly Dictionary<string, Tuple<int, string, bool>> propertyMapping = new Dictionary<string, Tuple<int, string, bool>>
            {
                { "FuelEnergyResourceOrgInn", new Tuple<int, string, bool>(0, "ИНН поставщика ТЭР", false) },
                { "FuelEnergyResourceOrgKpp", new Tuple<int, string, bool>(1, "КПП поставщика ТЭР", false) },
                { "InnRso", new Tuple<int, string, bool>(2, "ИНН РСО", false) },
                { "KppRso", new Tuple<int, string, bool>(3, "КПП РСО", false) },
                { "ResourceCode", new Tuple<int, string, bool>(4, "Код услуги", false) },
                { "SaldoIn", new Tuple<int, string, bool>(5, "Задолженность на начало месяца", true) },
                { "DebtIn", new Tuple<int, string, bool>(6, "Входящая просроченная задолженность", true) },
                { "Charged", new Tuple<int, string, bool>(7, "Начислено за месяц", true) },
                { "Paid", new Tuple<int, string, bool>(8, "Оплачено за месяц", true) },
                { "SaldoOut", new Tuple<int, string, bool>(9, "Задолженность на конец месяца", true) },
                { "DebtOut", new Tuple<int, string, bool>(10, "Исходящая просроченная задолженность", true) }
            };

            public TObject GetObject<TObject>(string[] splits) where TObject : class, IRecord, new()
            {
                var obj = new TObject();
                foreach (var mapping in this.propertyMapping)
                {
                    try
                    {
                        this.TryExtractProperty(obj, mapping.Key, splits[mapping.Value.Item1]);
                    }
                    catch
                    {
                        obj.LogError($"Значение столбца {mapping.Value.Item2} пусто или имеет неверный формат");
                    }
                }

                return obj;
            }

            public bool MergeData<TIn, TOut>(TIn obj, TOut result)
            {
                var infos = this.propertyMapping.Where(x => x.Value.Item3).Select(x => this.GetProperty<TIn>(x.Key)).ToDictionary(x => x.Name);
                var resultProperties = typeof(TOut)
                    .GetProperties(BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.Instance)
                    .Where(x => infos.Keys.Contains(x.Name))
                    .ToList();

                var changes = false;
                foreach (var propertyInfo in resultProperties)
                {
                    var sourceProperty = infos.Get(propertyInfo.Name);
                    propertyInfo.SetValue(result, sourceProperty.GetValue(obj));
                    changes = true;
                }

                return changes;
            }

            private void TryExtractProperty<TObject>(TObject obj, string propertyName, string value)
            {
                var propertyInfo = this.GetProperty<TObject>(propertyName);
                var convertedValue = ConvertHelper.ConvertTo(value, propertyInfo.PropertyType);
                propertyInfo.SetValue(obj, convertedValue);
            }

            private PropertyInfo GetProperty<TObject>(string propertyName)
            {
                PropertyInfo propertyInfo;
                if (!this.propertyInfos.TryGetValue(typeof(TObject).Name + propertyName, out propertyInfo))
                {
                    propertyInfo = typeof(TObject).GetProperty(propertyName);
                    this.propertyInfos.Add(typeof(TObject).Name + propertyName, propertyInfo);
                }
                return propertyInfo;
            }
        }

        private interface IRecord
        {
            /// <summary>
            /// Добавить лог ошибки объекта
            /// </summary>
            /// <param name="errorMessage">Сообщение об ошибке</param>
            void LogError(string errorMessage);
        }

        private class Record : IRecord
        {
            public string FuelEnergyResourceOrgInn { get; set; }
            public string FuelEnergyResourceOrgKpp { get; set; }
            public string InnRso { get; set; }
            public string KppRso { get; set; }
            public string ResourceCode { get; set; }
            public decimal SaldoIn { get; set; }
            public decimal DebtIn { get; set; }
            public decimal Charged { get; set; }
            public decimal Paid { get; set; }
            public decimal SaldoOut { get; set; }
            public decimal DebtOut { get; set; }

            public bool IsValid { get; private set; }
            public IList<string> Errors { get; }

            public Record()
            {
                this.IsValid = true;
                this.Errors = new List<string>();
            }

            public static Record Parse(string[] splits, DataExtractor extractor)
            {
                return extractor.GetObject<Record>(splits);
            }

            /// <summary>
            /// Добавить лог ошибки объекта
            /// </summary>
            /// <param name="errorMessage">Сообщение об ошибке</param>
            public void LogError(string errorMessage)
            {
                this.IsValid = false;
                this.Errors.Add(errorMessage);
            }
        }

        private class ImportParams
        {
            public string Inn { get; set; }
            public string Kpp { get; set; }
            public string Ogrn { get; set; }
            public int Year { get; set; }
            public int Month { get; set; }
            public ProviderCode ProviderCode { get; set; }
        }

        private enum ProviderCode
        {
            Rso = 1,
            Uo = 2
        }
    }
}