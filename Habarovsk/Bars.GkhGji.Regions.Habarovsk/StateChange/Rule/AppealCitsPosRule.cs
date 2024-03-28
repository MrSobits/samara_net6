namespace Bars.GkhGji.Regions.Habarovsk.StateChange
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Text;
    using System.Threading.Tasks;
    using B4;
    using B4.Modules.States;
    using Bars.GkhGji.Contracts.Reminder;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.PosAppeal;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;
    using Castle.Windsor;
    using Entities;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Bars.B4.Config;

    public class AppealCitsPosRule : IRuleChangeStatus
    {
        public IWindsorContainer Container { get; set; }

        public string Id
        {
            get { return "gji_appeal_citizens_pos_validation_rule"; }
        }

        public string Name
        {
            get { return "Назначение исполнителя в ПОС"; }
        }

        public string TypeId
        {
            get { return "gji_appeal_citizens"; }
        }

        public string Description
        {
            get { return "Данное правило отправляет исполнителя в ПОС если обращение получено из ПОС"; }
        }

        public virtual ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            if (statefulEntity is AppealCits)
            {

                var ExecutantDomain = this.Container.Resolve<IDomainService<AppealCitsExecutant>>();
                try
                {
                    var appealCits = statefulEntity as AppealCits;
                    if (string.IsNullOrEmpty(appealCits.ArchiveNumber))
                    {
                        return ValidateResult.Yes();
                    }
                    var exec = ExecutantDomain.GetAll()
                        .Where(x => x.AppealCits.Id == appealCits.Id && x.IsResponsible).FirstOrDefault();
                    if (exec == null || exec.Executant == null)
                    {
                        return ValidateResult.No("Данное обращения принято из ПОС. Перед переводом в работу необходимо назначить ответственного исполнителя");
                    }
                 
                    PosAnswer msg = new PosAnswer
                    {
                        answer = false,
                        executorInfo = new Executorinfo
                        {
                            email = exec.Executant.Email,
                            phone = exec.Executant.Phone,
                            surname = exec.Executant.Fio.Split()[0].Trim(),
                            name = exec.Executant.Fio.Split()[1].Trim(),
                            patronymic = exec.Executant.Fio.Split()[2].Trim()
                        },
                        id = Convert.ToInt64(appealCits.ArchiveNumber),
                        managerInfo = new Managerinfo
                        {
                            email = exec.Author.Email,
                            phone = exec.Author.Phone,
                            surname = exec.Author.Fio.Split()[0].Trim(),
                            name = exec.Author.Fio.Split()[1].Trim(),
                            patronymic = exec.Author.Fio.Split()[2].Trim()
                        },
                        regNumber = appealCits.NumberGji,
                        status = "В работе",
                        statusText = "Обращение принято в работу",
                        regNumberSetAt = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "Z",
                        opaId = Convert.ToInt64(220511),
                        opaName = "ГОСУДАРСТВЕННАЯ ЖИЛИЩНАЯ ИНСПЕКЦИЯ ВОРОНЕЖСКОЙ ОБЛАСТИ",
                        notifyApplicant = true,
                        outgoingNumber = appealCits.NumberGji+"/1",
                        outgoingNumberSetAt = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "Z",


                    };
                    string json = JsonConvert.SerializeObject(msg,
                            new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, Formatting = Formatting.Indented });
                    //var json = JsonConvert.SerializeObject(msg);
                    Task<string> task = Task.Run<string>(async () => await GetTokenAsync());
                    var res = task.Result;
                    if (string.IsNullOrEmpty(res))
                    {
                        return ValidateResult.No("не удалось отправить сообщение в ПОС. Ошибка авторизации");
                    }
                    Task<TaskResult> acc = Task.Run<TaskResult>(async () => await SendAccepted(appealCits.ArchiveNumber, res, json));
                    var result = acc.Result;
                    if (!result.result)
                    {
                        return ValidateResult.No(result.answer);
                    }
                    else
                    {
                        return ValidateResult.Yes();
                    }

                }
                finally
                {
                    this.Container.Release(ExecutantDomain);
                }
            }

            return ValidateResult.Yes();
        }
        private async Task<TaskResult> SendAccepted(string id, string token, string jsonObject)
        {
            string url = "https://pos.gosuslugi.ru/appeal-service/edms/" + id;
            using (var myclient = new HttpClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                myclient.DefaultRequestHeaders.Add("User-Agent", "CBS Brightcove API Service");
                myclient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await myclient.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    var responces = response.Content.ReadFromJsonAsync<Dictionary<string,object>>().Result;
                    foreach (KeyValuePair<string, object> m in responces)
                    {
                        if (m.Key == "messages")
                        {
                            var msg = m.Value as JToken;
                            var v1 = msg.First.ToString();
                            if (!string.IsNullOrEmpty(v1))
                            {
                                return new TaskResult
                                {
                                    result = false,
                                    answer = v1
                                };
                            }
                            var v3 = msg.ToString();
                            if (!string.IsNullOrEmpty(v3))
                            {
                                return new TaskResult
                                {
                                    result = false,
                                    answer = v3
                                };
                            }
                        }
                    }

                }
                else
                {
                    return new TaskResult
                    {
                        result = true,
                        answer = ""
                    };
                }
            }
            return new TaskResult
            {
                result = true,
                answer = ""
            };
        }

        private async Task<string> GetTokenAsync()
        {
            var configProvider = Container.Resolve<IConfigProvider>();
            var config = configProvider.GetConfig().GetModuleConfig("Bars.GkhGji.Regions.Voronezh");
            var ClientID = config.GetAs("ClientID", (string)null, true);
            var SecretKey = config.GetAs("SecretKey", (string)null, true);
            var username = "fdppca-user";
            var password = "fdppca-password";
            var byteArray = new UTF8Encoding().GetBytes($"{username}:{password}");
            var formData = new List<KeyValuePair<string, string>>();
            formData.Add(new KeyValuePair<string, string>("username", ClientID));
            formData.Add(new KeyValuePair<string, string>("password", SecretKey));

            formData.Add(new KeyValuePair<string, string>("scope", "any"));
            formData.Add(new KeyValuePair<string, string>("grant_type", "password"));
            var client = new HttpClient();
            //var client = new HttpClient(new HttpClientHandler
            //{
            //    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
            //    Credentials = new Cre
            //},
            // disposeHandler: false);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.DefaultRequestHeaders
              .Accept
              .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var url = "https://pos.gosuslugi.ru/user-service/oauth/token";
            client.BaseAddress = new Uri("http://example.com/");
            var req = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(formData) };
            req.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            HttpResponseMessage response = await client.SendAsync(req);
            HttpContent content = response.Content;
            string result = await content.ReadAsStringAsync();
            var dataresponce = JsonConvert.DeserializeObject<ResponceJson>(result);
            if (dataresponce != null)
            {
                return dataresponce.access_token;
            }
            return "";
        }


        private class ResponceJson
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public string expires_in { get; set; }
            public string scope { get; set; }
            public string patronymic { get; set; }
            public string system { get; set; }
            public string surname { get; set; }
            public string name { get; set; }
            public string rsId { get; set; }
            public string userId { get; set; }
            public string email { get; set; }
            public string superUser { get; set; }
            public string jti { get; set; }
        }
        private class TaskResult
        {
            public bool result { get; set; }
            public string answer { get; set; }

        }
    }
}