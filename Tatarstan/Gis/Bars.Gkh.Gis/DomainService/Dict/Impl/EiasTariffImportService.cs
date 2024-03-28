namespace Bars.Gkh.Gis.DomainService.Dict.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Gis.ConfigSections;
    using Bars.Gkh.Gis.Entities.Dict;
    using Bars.Gkh.Import;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    using Newtonsoft.Json;

    public partial class EiasTariffImportService : IEiasTariffImportService
    {
        private Uri serverAddress;
        private string login;
        private string password;
        private string objectName;

        private const int BarchSize = 7000;
        private static readonly IEnumerable<int> AllowedCodes = new[] { 6, 7, 8, 9, 10, 25, 30 };

        private readonly Stream importDataStream = new MemoryStream();
        private TextWriter importDataWriter;

        private IDictionary<string, long> municipalityDict;
        private IDictionary<int, long> serviceDict;
        private IDictionary<string, Dictionary<string, long>> contragentDict;
        private IDictionary<string, long> tariffDict;

        private readonly List<GisTariffDict> tariffToSave = new List<GisTariffDict>();
        private readonly List<GisTariffDict> tariffToUpdate = new List<GisTariffDict>();
        private readonly HashSet<string> importedRecords = new HashSet<string>();

        public IWindsorContainer Container { get; set; }
        public ILogImportManager LogImportManager { get; set; }
        public ILogImport LogImport { get; set; }
        public IDomainService<GisTariffDict> TariffDomain { get; set; }

        /// <inheritdoc />
        public IDataResult Import(BaseParams baseParams)
        {
            var timeStamp = baseParams.Params.GetAs<DateTime?>("TimeStamp");
            try
            {
                this.Initialize();
                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    this.SetAuthHeader(client.Headers);

                    var describeData = this.GetDescribe(client);

                    var rowCount = describeData.RowsCount;
                    var page = 0;
                    var downloadRowCount = 0;
                    while (rowCount > downloadRowCount)
                    {
                        var tariffData = this.GetTariffPage(client, page++, timeStamp);
                        if (tariffData == null || tariffData.Count == 0)
                        {
                            break;
                        }
                        downloadRowCount += tariffData.Count;

                        this.ImportTariffData(tariffData);
                    }

                    var resultData = new Dictionary<string, string>
                    {
                        { "Добавлено строк", this.LogImport?.CountAddedRows.ToStr() },
                        { "Изменено строк", this.LogImport?.CountChangedRows.ToStr() },
                        { "Общее количество импортированных строк", this.LogImport?.CountImportedRows.ToStr() },
                        { "Количество строк с ошибками", this.LogImport?.CountWarning.ToStr() },
                        { "Количество строк пришедших из ЕИАС", downloadRowCount.ToStr() },
                        { "Всего количество строк в ЕИАС", describeData?.RowsCount.ToStr() }
                    };
                    if (timeStamp.HasValue)
                    {
                        resultData.Add("Дата загрузки c", timeStamp.Value.ToShortDateString());
                    }

                    return new BaseDataResult(resultData);
                }
            }
            catch (WebException ex)
            {
                var messages = ex.GetInnerExceptions().Select(x => x.Message).ToArray();
                this.LogImport.Error("Ошибка при передаче данных", ex.Response.ResponseUri.ToString(), messages);
                throw;
            }
            catch (Exception ex)
            {
                this.LogImport.Error(string.Empty, ex.Message);
                throw;
            }
            finally
            {
                this.LogImportManager.Add(this.importDataStream, "eias_data.json", this.LogImport);
                this.LogImportManager.Save();
            }
        }

        private void Update()
        {
            var query = this.TariffDomain.GetAll()
                .WhereNotEmptyString(x => x.ExternalId)
                .OrderBy(x => x.Id);

            var allCount = query.Count();

            for (int i = 0; i < allCount; i += EiasTariffImportService.BarchSize)
            {
                this.Container.InTransaction(() =>
                {
                    var skip = i;
                    var portion = query.Skip(skip).Take(EiasTariffImportService.BarchSize).ToList();
                    var toUpdateList = portion.Join(this.tariffToUpdate,
                            x => x.ExternalId,
                            x => x.ExternalId,
                            (oldData, newData) =>
                            {
                                oldData.EiasUploadDate = newData.EiasUploadDate;
                                oldData.EiasEditDate = newData.EiasEditDate;
                                oldData.Municipality = newData.Municipality;
                                oldData.Service = newData.Service;
                                oldData.Contragent = newData.Contragent;
                                oldData.ActivityKind = newData.ActivityKind;
                                oldData.ContragentName = newData.ContragentName;
                                oldData.StartDate = newData.StartDate;
                                oldData.EndDate = newData.EndDate;
                                oldData.TariffKind = newData.TariffKind;
                                oldData.ZoneCount = newData.ZoneCount;
                                oldData.TariffValue = newData.TariffValue;
                                oldData.TariffValue1 = newData.TariffValue1;
                                oldData.TariffValue2 = newData.TariffValue2;
                                oldData.TariffValue3 = newData.TariffValue3;
                                oldData.IsNdsInclude = newData.IsNdsInclude;
                                oldData.IsSocialNorm = newData.IsSocialNorm;
                                oldData.IsMeterExists = newData.IsMeterExists;
                                oldData.IsElectricStoveExists = newData.IsElectricStoveExists;
                                oldData.Floor = newData.Floor;
                                oldData.ConsumerType = newData.ConsumerType;
                                oldData.SettelmentType = newData.SettelmentType;
                                oldData.ConsumerByElectricEnergyType = newData.ConsumerByElectricEnergyType;
                                oldData.RegulatedPeriodAttribute = newData.RegulatedPeriodAttribute;
                                oldData.BasePeriodAttribute = newData.BasePeriodAttribute;

                                return oldData;
                            })
                            .ToList();
                    toUpdateList.ForEach(this.TariffDomain.Update);
                });

            }
        }
        
        private void ImportTariffData(List<TariffData> tariffData)
        {
            this.tariffToSave.Clear();
            this.tariffToUpdate.Clear();

            tariffData.ForEach(x =>
            {
                var tariff = this.GetGisTariffEntity(x);
                if (tariff != null)
                {
                    if (tariff.Id > 0)
                    {
                        this.tariffToUpdate.Add(tariff);
                    }
                    else
                    {
                        this.tariffToSave.Add(tariff);
                    }
                }
            });

            TransactionHelper.InsertInManyTransactions(this.Container, this.tariffToSave, EiasTariffImportService.BarchSize, false);
            this.Update();

            this.LogImport.CountAddedRows += this.tariffToSave.Count;
            this.LogImport.CountChangedRows += this.tariffToUpdate.Count;
        }

        private void Initialize()
        {
            var config = this.Container.GetGkhConfig<GisConfig>()?.EiasIntegrationConfig;

            this.serverAddress = new Uri(config.ServiceAddress);
            this.login = config.Login;
            this.password = config.Password;
            this.objectName = config.ObjectName;

            this.LogImportManager.FileNameWithoutExtention = "Импорт данных по тарифам из ЕИАС";
            this.LogImport.SetFileName("Импорт данных по тарифам из ЕИАС");
            this.LogImport.ImportKey = nameof(EiasTariffImportService);

            this.importDataWriter = new StreamWriter(this.importDataStream, Encoding.UTF8);

            this.InitCache();
        }

        private void InitCache()
        {
            var municipalityDomain = this.Container.ResolveDomain<Municipality>();
            var serviceDomain = this.Container.ResolveDomain<ServiceDictionary>();
            var contragentDomain = this.Container.ResolveDomain<Contragent>();

            using (this.Container.Using(municipalityDomain, serviceDomain, contragentDomain))
            {
                this.municipalityDict = municipalityDomain.GetAll()
                    .Select(x => new
                    {
                        x.Id,
                        x.FiasId
                    })
                    .ToList()
                    .Join(EiasTariffImportService.MoGuidDict,
                        o => o.FiasId,
                        i => i.Value,
                        (o, i) => new
                        {
                            o.Id,
                            i.Key
                        })
                    .ToDictionary(x => x.Key, x => x.Id);

                this.serviceDict = serviceDomain.GetAll()
                    .WhereContains(x => x.Code, EiasTariffImportService.AllowedCodes)
                    .Select(x => new
                    {
                        x.Id,
                        x.Code
                    })
                    .ToDictionary(x => x.Code, x => x.Id);

                this.contragentDict = contragentDomain.GetAll()
                    .WhereNotEmptyString(x => x.Inn)
                    .Select(x => new
                    {
                        x.Id,
                        x.Inn,
                        Kpp = x.Kpp ?? string.Empty
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Kpp)
                    .ToDictionary(x => x.Key,
                        x => x.GroupBy(y => y.Inn, y => y.Id)
                            .ToDictionary(y => y.Key, y => y.FirstOrDefault()));

                this.tariffDict = this.TariffDomain.GetAll()
                    .WhereNotEmptyString(x => x.ExternalId)
                    .Select(x => new
                    {
                        x.ExternalId,
                        x.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.ExternalId, x => x.Id)
                    .ToDictionary(x => x.Key, x => x.Single());
            }
        }

        private DescribeData GetDescribe(WebClient client)
        {
            var response = JsonConvert.DeserializeObject<DescribeResponse>(client.DownloadString(this.GetDescribeRequestUri(this.objectName)));

            return response?.Data;
        }

        private List<TariffData> GetTariffPage(WebClient client, int page, DateTime? timeStamp = null, List<string> fields = null)
        {
            this.importDataWriter.Write($@"{{""page"":{page},""response:""");
            var responseText = client.DownloadString(this.GetDataRequestUri(this.objectName, page, timeStamp, fields));

            this.importDataWriter.Write(responseText);
            this.importDataWriter.Write("},");
            var response = JsonConvert.DeserializeObject<TariffResponse>(responseText);

            return response?.Data;
        }

        private void SetAuthHeader(WebHeaderCollection headerCollection)
        {
            var auth = $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes($"{this.login}:{this.password}"))}";
            headerCollection.Add(HttpRequestHeader.Authorization, auth);
        }

        /// <summary>
        /// Запрос возвращает список доступных объектов с краткими сведениями о них
        /// </summary>
        private Uri GetListRequestUri()
        {
            return new Uri(this.serverAddress, "api/list");
        }

        /// <summary>
        /// Запрос возвращает подробное описание объекта
        /// </summary>
        private Uri GetDescribeRequestUri(string code)
        {
            return new Uri(this.serverAddress, $"api/describe?code={code}");
        }

        /// <summary>
        /// Запрос возвращает данные соответствующего объекта (постраничная выборка по 500 объектов)
        /// </summary>
        private Uri GetDataRequestUri(string code, int? pageNumber = null, DateTime? timeStamp = null, List<string> fields = null)
        {
            var uriBuilder = new UriBuilder(new Uri(this.serverAddress, "api/data"));
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            query["code"] = code;

            if (pageNumber.HasValue)
            {
                query["pageNumber"] = pageNumber.ToString();
            }

            if (timeStamp.HasValue)
            {
                query["timeStamp"] = timeStamp.Value.ToString("yyyy-MM-dd HH:mm");
            }

            fields?.ForEach(x =>
            {
                query.Add("fields[]", x);
            });
            uriBuilder.Query = query.ToString();
            return uriBuilder.Uri;
        }
    }
} 