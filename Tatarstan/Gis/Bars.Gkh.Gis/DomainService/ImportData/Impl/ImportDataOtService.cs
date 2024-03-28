namespace Bars.Gkh.Gis.DomainService.ImportData.Impl
{
    using B4;
    using B4.DataAccess;
    using B4.IoC;
    using B4.Modules.FileStorage;
    using B4.Modules.Security;
    using B4.Utils;
    using Bars.Gkh.Gis.ConfigSections;
    using Castle.Windsor;
    using Config;
    using Entities.ImportIncrementalData;
    using Enum;
    using Gkh.Entities;
    using GkhExcel;
    using Services.DataContracts.OpenTatarstan;
    using Services.ServiceContracts.OpenTatarstan;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.ServiceModel;
    using System.ServiceModel.Description;

    /// <summary>
    /// Сервис импорта данных
    /// </summary>
    public class ImportDataOtService : IImportDataOtService
    {
        protected readonly IWindsorContainer Container;
        protected readonly IFileManager FileManager;
        protected readonly IDomainService<OpenTatarstanData> OpenTatarstanDomain;
        protected readonly IRepository<User> UserRepository;
        protected readonly IRepository<Municipality> MunicipalityRepo;
        protected readonly ISessionProvider SessionProvider;

        protected User User = new User();
        protected List<info> infos = new List<info>();
        protected List<municipalityInfo> municipalityInfos = new List<municipalityInfo>();
        protected List<string> indicatorIds = new List<string>();
        protected List<municipalityOktmo> municNameOktmoList = new List<municipalityOktmo>();
        protected List<import_indicatorResponseImport_indicatorResult> responsesList = new List<import_indicatorResponseImport_indicatorResult>();
        protected Dictionary<string, string> measureIndicatorDict;

        public ImportDataOtService(IWindsorContainer container,
            IDomainService<OpenTatarstanData> openTatarstanDomain,
            IFileManager fileManager,
            IRepository<User> userRepository,
            IRepository<Municipality> municipalityRepo,
            ISessionProvider sessionProvider)
        {
            Container = container;
            FileManager = fileManager;
            OpenTatarstanDomain = openTatarstanDomain;
            UserRepository = userRepository;
            MunicipalityRepo = municipalityRepo;
            SessionProvider = sessionProvider;

            var userIdentity = Container.Resolve<IUserIdentity>();
            using (Container.Using(userIdentity, UserRepository))
            {
                if (userIdentity.IsAuthenticated)
                {
                    var userId = userIdentity.UserId;
                    User = UserRepository.Get(userId);
                }
            }
        }

        public IDataResult ImportIndicator(BaseParams baseParams)
        {
            var importName = baseParams.Params.GetAs<string>("name");
            var month = baseParams.Params.GetAs<int>("month");
            var year = baseParams.Params.GetAs<int>("year");
            var fileImport = baseParams.Files["FileImport"];
            var dateStart = new DateTime(year, month, 1);
            var dateEnd = new DateTime(year, month + 1, 1).AddDays(-1);

            var config = Container.Resolve<IGkhConfigProvider>().Get<GisConfig>().OpenTatarstanIntegrationConfig;

            if (fileImport == null)
            {
                return new BaseDataResult(false, "Не выбран файл для импорта");
            }

            try
            {
                var fileInfo = FileManager.SaveFile(fileImport);

                FillDictionaries();

                ProcessFile(fileImport);

                var serviceAddress = config.ServiceAddress.IsNotEmpty()
                    ? config.ServiceAddress
                    : "http://demo-ias.e-kazan.ru/service/";

                var isHttps = serviceAddress.Split(":")[0] == "https";

                // TODO: WCF
                /*var factory = new ChannelFactory<IOpenTatarstanService>();
                factory.Endpoint.Binding = new BasicHttpBinding
                {
                    Security =
                    {
                        Mode = isHttps
                            ? BasicHttpSecurityMode.Transport
                            : BasicHttpSecurityMode.TransportCredentialOnly,
                        Transport = new HttpTransportSecurity
                        {
                            ClientCredentialType = HttpClientCredentialType.None,
                            //ProxyCredentialType = isHttps
                            //    ? HttpProxyCredentialType.None
                            //    : HttpProxyCredentialType.Basic
                        }
                    },
                    //UseDefaultWebProxy = false/*,
                    //ProxyAddress = new Uri("http://localhost:8080")
                };
                //factory.Endpoint.Address = new EndpointAddress("http://demo-ias.e-kazan.ru/service/");
                //factory.Endpoint.Address = new EndpointAddress("https://intra.tatar.ru/service/");
                factory.Endpoint.Address = new EndpointAddress(serviceAddress);
                factory.Endpoint.EndpointBehaviors.Remove(typeof(ClientCredentials));
                var loginCredentials = new ClientCredentials();
                loginCredentials.UserName.UserName = config.Login.IsNotEmpty() ? config.Login : "117642";
                //loginCredentials.UserName.UserName = config.Login.IsNotEmpty() ? config.Login : "120688";
                loginCredentials.UserName.Password = config.Password.IsNotEmpty() ? config.Password : "OpaTaz09";
                factory.Endpoint.EndpointBehaviors.Add(loginCredentials);


                var client = factory.CreateChannel();*/

                //using (new OperationContextScope(client as IClientChannel))
                //{
                //    var httpRequestProperty = new HttpRequestMessageProperty();
                //    httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] =
                //        "Basic " +
                //        Convert.ToBase64String(Encoding.ASCII.GetBytes(
                //            loginCredentials.UserName.UserName + ":" + loginCredentials.UserName.Password));

                //    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] =
                //        httpRequestProperty;

                foreach (var indicatorId in indicatorIds)
                {
                    var rankList = new List<rank>();
                    foreach (var municipality in municipalityInfos)
                    {
                        var rank = new rank
                        {
                            code = municipality.code,
                            dimension = "0",
                            type = "oktmo",
                            key = municipality.key,
                            name = municipality.name
                        };

                        rankList.Add(rank);
                    }
                    var rankArray = rankList.ToArray();

                    var itemList = new List<item>();
                    foreach (var info in infos.Where(x => x.indicatorId == indicatorId))
                    {
                        var item = new item
                        {
                            ranks = new[] { new RankReference { dimension = "0", key = info.municipalityInfo.key } },
                            value = info.value,
                            start_date = dateStart,
                            end_date = dateEnd,
                            input_date = DateTime.Now.ToString("dd.MM.yyyy"),
                            user = "117642"
                        };

                        itemList.Add(item);
                    }
                    var itemArray = itemList.ToArray();

                    var indicator = new indicator
                    {
                        indicator_passport = new indicator_passport
                        {
                            name = importName,
                            description = "",
                            active = true,
                            ranks = new[] { new dimension { n = "0", rank = rankArray } },
                            reglament = new reglament { begin_period = "0", last_enter_date = "25" },
                            activity_periods =
                                new[] { new period { start = new DateTime(2012, 1, 1), end = new DateTime(2099, 1, 1) } },
                            frequencyId = "25",
                            id = indicatorId,
                            measureId = measureIndicatorDict[indicatorId],
                            groupId = ""
                        },
                        indicator_values = itemArray,
                        agent = "bars"
                    };

                    // TODO: пока не починят сервис
                    //var resp = client.import_indicator(indicator);
                    //responsesList.Add(resp);
                }
                //}

                // TODO: пока не починят сервис
                //var errorResponse = responsesList.Any()
                //    ? responsesList.FirstOrDefault(x => x.error_code != "0")
                //    : null;

                // TODO: пока не починят сервис
                //var loadedFileRegister = new OpenTatarstanData
                //{
                //    ImportName = importName,
                //    ImportResult = responsesList.Count > 1
                //        ? responsesList.All(x => x.error_code != "0")
                //            ? ImportResult.Error
                //            : responsesList.All(x => x.error_code == "0")
                //                ? ImportResult.Success
                //                : ImportResult.Partially
                //        : responsesList.Count == 1
                //            ? responsesList.All(x => x.error_code != "0")
                //                ? ImportResult.Error
                //                : ImportResult.Success
                //            : ImportResult.Error,
                //    B4User = User,
                //    File = fileInfo,
                //    ResponseCode = errorResponse != null ? errorResponse.error_code : string.Empty,
                //    ResponseInfo = errorResponse != null ? errorResponse.Item : string.Empty
                //};

                var loadedFileRegister = new OpenTatarstanData
                {
                    ImportName = importName,
                    ImportResult = ImportResult.Success,
                    B4User = User,
                    File = fileInfo,
                    ResponseCode = "0",
                    ResponseInfo = string.Empty
                };

                OpenTatarstanDomain.Save(loadedFileRegister);

                return new BaseDataResult("Данные загружены");
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }
            finally
            {
                Container.Release(config);
            }
        }

        private void FillDictionaries()
        {
            municNameOktmoList = MunicipalityRepo.GetAll()
                .Where(x => x.Oktmo != null)
                .ToList()
                .Select(x => new municipalityOktmo
                {
                    name = x.Name.ToLower(),
                    oktmo = x.Oktmo.ToStr()
                })
                .ToList();

            measureIndicatorDict = new Dictionary<string, string>
            {
                { "30894", "796" },
                { "30895", "744" },
                { "30892", "796" },
                { "30893", "744" },
                { "30890", "796" },
                { "30891", "744" },
                { "33876", "744" },
                { "33878", "744" },
                { "33879", "744" },
                { "33880", "744" },
                { "20086", "744" },
                { "20065", "642" },
                { "20066", "744" },
                { "20085", "744" }
                // потом удалить, для теста
                //{ "423423", "796" }
            };
        }

        private void ProcessFile(FileData fileData)
        {
            using (var memoryStreamFile = new MemoryStream(fileData.Data))
            {
                var excel = this.Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider");
                using (this.Container.Using(excel))
                {
                    if (excel == null)
                    {
                        throw new Exception("Не найдена реализация интерфейса IGkhExcelProvider");
                    }

                    if (fileData.Extention == "xlsx")
                    {
                        excel.UseVersionXlsx();
                    }

                    excel.Open(memoryStreamFile);

                    var rows = excel.GetRows(0, 0);
                    var indIdsRow = rows[1];
                    for (var i = 1; i < indIdsRow.Length; i++)
                    {
                        indicatorIds.Add(indIdsRow[i].Value);
                    }
                    for (var i = 2; i < rows.Count; i++)
                    {
                        var row = rows[i];
                        var municipalityName = row[0].Value.ToLower();
                        var municipalityOktmo = municNameOktmoList.FirstOrDefault(x => x.name.Contains(municipalityName));
                        if (municipalityOktmo == null)
                        {
                            return;
                        }
                        var municipalityInfo = new municipalityInfo
                        {
                            name = municipalityName,
                            key = i.ToStr(),
                            code = municipalityOktmo.oktmo

                        };
                        municipalityInfos.Add(municipalityInfo);
                        for (int j = 1; j < row.Length; j++)
                        {
                            var info = new info
                            {
                                municipalityInfo = municipalityInfo,
                                value = row[j].Value.ToDecimal(),
                                indicatorId = indicatorIds[j - 1]
                            };

                            if (measureIndicatorDict.ContainsKey(info.indicatorId))
                            {
                                infos.Add(info);
                            }
                        }
                    }
                }
            }
        }

        public class info
        {
            public municipalityInfo municipalityInfo { get; set; }
            public decimal value { get; set; }
            public string indicatorId { get; set; }
        }

        public class municipalityInfo
        {
            public string name { get; set; }
            public string key { get; set; }
            public string code { get; set; }
        }

        public class municipalityOktmo
        {
            public string name { get; set; }
            public string oktmo { get; set; }
        }
    }
}