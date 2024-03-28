namespace Bars.GkhGji.Regions.Voronezh.StateChange
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Text;
    using System.Threading.Tasks;
    using B4;
    using B4.Modules.States;
    using Bars.B4.Config;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Services.DataContracts.Suggestion;
    using Bars.GkhGji.Contracts.Reminder;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.PosAppeal;
    using Castle.Core.Internal;
    using Castle.Windsor;
    using Entities;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using NHibernate.Linq.Functions;

    public class AppealCitsPosAnswerRule : IRuleChangeStatus
    {
        public IWindsorContainer Container { get; set; }

        public string Id
        {
            get { return "gji_appeal_citizens_posanswer_validation_rule"; }
        }

        public string Name
        {
            get { return "Отправка ответа в ПОС"; }
        }

        public string TypeId
        {
            get { return "gji_appeal_citizens"; }
        }

        public string Description
        {
            get { return "Данное правило отправляет ответ в ПОС при закрытии обращения если обращение получено из ПОС"; }
        }

        public virtual ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            if (statefulEntity is AppealCits)
            {

                var AnswerDomain = this.Container.Resolve<IDomainService<AppealCitsAnswer>>();
                var AppealCitsSourceDomain = this.Container.Resolve<IDomainService<AppealCitsSource>>();
                var AppealAnswerLongTextDomain = this.Container.Resolve<IDomainService<AppealAnswerLongText>>();
                try
                {
                    var appealCits = statefulEntity as AppealCits;
                    if (string.IsNullOrEmpty(appealCits.ArchiveNumber))
                    {
                        return ValidateResult.Yes();
                    }
                    var source = AppealCitsSourceDomain.GetAll().Where(x => x.AppealCits.Id == appealCits.Id && x.RevenueSource != null && x.RevenueSource.Code == "12").FirstOrDefault();
                    if (source == null)
                    {
                        return ValidateResult.Yes();
                    }
                    var answ = AnswerDomain.GetAll()
                        .Where(x => x.AppealCits.Id == appealCits.Id && x.Addressee != null && x.Addressee.Name == "Заявитель").FirstOrDefault();

                    if (answ == null || answ.File == null)
                    {
                        return ValidateResult.No("Данное обращения принято из ПОС. Перед закрытием необходимо дать ответ заявителю с прикреплением файла ответа");
                    }
                    var lt = AppealAnswerLongTextDomain.GetAll().FirstOrDefault(x => x.AppealCitsAnswer.Id == answ.Id);
                    if (answ.AnswerContent == null)
                    {
                        return ValidateResult.No("Данное обращения принято из ПОС. Перед закрытием необходимо указать содержание ответа");
                    }
                    if (lt == null)
                    {
                        return ValidateResult.No("Данное обращения принято из ПОС. Перед закрытием необходимо заполнить текст ответа");
                    }
                    if (answ.Executor == null)
                    {
                        return ValidateResult.No("Данное обращения принято из ПОС. Перед закрытием необходимо указать исполнителя ответа");
                    }


                    //var json = JsonConvert.SerializeObject(msg);
                    Task<string> task = Task.Run<string>(async () => await GetTokenAsync());
                    var res = task.Result;
                    if (string.IsNullOrEmpty(res))
                    {
                        return ValidateResult.No("не удалось отправить сообщение в ПОС. Ошибка авторизации");
                    }
                    Task<SendFileResult> acc = Task.Run<SendFileResult>(async () => await SendFile(answ.File, res));

                    var result = acc.Result;
                    if (!result.result)
                    {
                        return ValidateResult.No(result.fileId);
                    }
                    else
                    {
                        PosAnswer msg = new PosAnswer
                        {
                            answer = true,
                            answerText = System.Text.Encoding.UTF8.GetString(lt.Description),
                            appealAnswerType = GetAppealAnswerType(answ.AnswerContent.Code),
                            attachmentIds = new string[] { result.fileId },
                            comment = answ.Description,
                            executorInfo = new Executorinfo
                            {
                                email = answ.Executor.Email,
                                phone = answ.Executor.Phone,
                                surname = answ.Executor.Fio.Split()[0].Trim(),
                                name = answ.Executor.Fio.Split()[1].Trim(),
                                patronymic = answ.Executor.Fio.Split()[2].Trim()
                            },
                            id = Convert.ToInt64(appealCits.ArchiveNumber),
                            managerInfo = new Managerinfo
                            {
                                email = answ.Signer.Email,
                                phone = answ.Signer.Phone,
                                surname = answ.Signer.Fio.Split()[0].Trim(),
                                name = answ.Signer.Fio.Split()[1].Trim(),
                                patronymic = answ.Signer.Fio.Split()[2].Trim()
                            },
                            regNumber = appealCits.NumberGji,
                            status = "Закрыто",
                            statusText = "Обращение рассмотрено",
                            regNumberSetAt = appealCits.ObjectCreateDate.ToString("yyyy-MM-ddTHH:mm:ss") + "Z",
                            opaId = Convert.ToInt64(220511),
                            opaName = "ГОСУДАРСТВЕННАЯ ЖИЛИЩНАЯ ИНСПЕКЦИЯ ВОРОНЕЖСКОЙ ОБЛАСТИ",
                            notifyApplicant = true,
                            outgoingNumber = appealCits.NumberGji,
                            outgoingNumberSetAt = appealCits.ObjectCreateDate.ToString("yyyy-MM-ddTHH:mm:ss") + "Z",


                        };
                        string json = JsonConvert.SerializeObject(msg,
                                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, Formatting = Formatting.Indented });

                        Task<TaskResult> answerResult = Task.Run<TaskResult>(async () => await SendAccepted(appealCits.ArchiveNumber, res, json));
                        var tr = answerResult.Result;
                        if (!tr.result)
                        {
                            return ValidateResult.No(tr.answer);
                        }
                    }
                }
                catch (Exception e)
                {
                    var str = e.ToString();
                }
                finally
                {
                    this.Container.Release(AnswerDomain);

                }
            }

            return ValidateResult.Yes();
        }

        private string GetAppealAnswerType(string code)
        {
            switch (code)
            {
                case "1": return "SOLVED";
                case "2": return "SOLVED";
                case "3": return "FORWARDED";
                case "4": return "REJECTED";
                case "5": return "SOLVED";
                case "6": return "POSTPONED";
                default: return "SOLVED";
            }
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

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        private async Task<SendFileResult> SendFile(B4.Modules.FileStorage.FileInfo fileInfo, string token)
        {
            string url = "https://pos.gosuslugi.ru/file-storage-service/filestorage";
            var fm = Container.Resolve<IFileManager>();
            byte[] file_bytes = ReadFully(fm.GetFile(fileInfo));
            using (var myclient = new HttpClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                myclient.DefaultRequestHeaders.Add("User-Agent", "CBS Brightcove API Service");
                myclient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new ByteArrayContent(file_bytes, 0, file_bytes.Length), "file", fileInfo.FullName);
                HttpResponseMessage response = await myclient.PostAsync(url, form);
                if (!response.IsSuccessStatusCode)
                {
                    return null;

                }
                HttpContent content = response.Content;
                string result = await content.ReadAsStringAsync();
                var dataresponce = JsonConvert.DeserializeObject<PosFileAnswer>(result);
                return new SendFileResult
                {
                    result = true,
                    fileId = dataresponce.id,
                };

            }
            return new SendFileResult
            {
                result = true,
                fileId = "",
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

        private class SendFileResult
        {
            public bool result { get; set; }
            public string fileId { get; set; }

        }

        private class PosFileAnswer
        {
            public string id { get; set; }
            public string name { get; set; }
            public int size { get; set; }
            public string contentType { get; set; }
            public bool temporary { get; set; }
            public bool avatar { get; set; }
            public object previewUrl { get; set; }
            public string url { get; set; }
        }
    }
}