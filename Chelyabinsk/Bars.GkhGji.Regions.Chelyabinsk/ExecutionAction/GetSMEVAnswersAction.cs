using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.ExecutionAction;
using Bars.GkhGji.Entities.PosAppeal;
using Bars.GkhGji.Regions.Chelyabinsk.DomainService;
using Bars.GkhGji.Regions.Chelyabinsk.Tasks.GetSMEVAnswers;
using EcmaScript.NET;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Bars.GkhGji.Regions.Chelyabinsk.ExecutionAction
{
  

    /// <summary>
    /// Периодическая проверка СМЭВа на результаты
    /// </summary>
    public class GetSMEVAnswersAction : BaseExecutionAction
    {
        public IComplaintsService _cpmplService;

        private string thistoken;

        public GetSMEVAnswersAction(
        IComplaintsService complServ)
        {
            this._cpmplService = complServ;
        }

        public override string Description => "Запрашивает из СМЭВа все ответы и обрабатывает их (пока только те, что добавлены в 2018)";

        public override string Name => "Проверить ответы в СМЭВ";

        public override Func<IDataResult> Action => GetSMEVResponses;

        private async Task<Rootobject> GetPOS()
        {
            string token = await GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                thistoken = token;
                HttpClient client = new HttpClient();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                //GET Method
                client.BaseAddress = new Uri("https://pos.gosuslugi.ru/appeal-service/edms");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage responseRest = await client.GetAsync("");
                if (responseRest.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    var dataObjects = await responseRest.Content.ReadAsStringAsync();  //Make sure to add a reference to System.Net.Http.Formatting.dll
                    if (!string.IsNullOrEmpty(dataObjects))
                    {
                        try
                        {
                            var rootobject = JsonConvert.DeserializeObject<Rootobject>(dataObjects);
                            if (rootobject != null)
                            {
                                return rootobject;
                                //   _cpmplService.CreateAppeals(rootobject);
                            }
                        }
                        catch (Exception e)
                        {

                        }
                        return null;

                    }

                }
            }
            return null;
        }

        private async Task<string> GetTokenAsync()
        {

            var username = "fdppca-user";
            var password = "fdppca-password";
            var byteArray = new UTF8Encoding().GetBytes($"{username}:{password}");
            var formData = new List<KeyValuePair<string, string>>();
            //formData.Add(new KeyValuePair<string, string>("username", "560f7eb4-1a54-4bf7-b7c9-0a952ec2e698"));
            //formData.Add(new KeyValuePair<string, string>("password", "1affd569-9f05-4026-8ea8-8ba9b9913e79"));
            formData.Add(new KeyValuePair<string, string>("username", "5484b7fa-60a2-4f1b-a37d-ad3d72261115"));
            formData.Add(new KeyValuePair<string, string>("password", "f16d2538-c85a-440b-8a44-00984cb1329d"));           
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
            var dataresponce = JsonConvert.DeserializeObject<ResponseJson>(result);
            if (dataresponce != null)
            {
                return dataresponce.access_token;
            }
            return "";
        }

        private IDataResult GetSMEVResponses()
        {
            try
            {
                Task<Rootobject> task = Task.Run<Rootobject>(async () => await GetPOS());
                var res = task.Result;
                if (res != null)
                {
                    _cpmplService.CreateEmailGJI(res, thistoken);
                }
            }
            catch (Exception e)
            {

            }

            var taskManager = Container.Resolve<ITaskManager>();

            try
            {
                taskManager.CreateTasks(new GetSMEVAnswersTaskProvider(Container), new BaseParams());
                return new BaseDataResult(true, "Задача успешно поставлена");
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }
            finally
            {
                Container.Release(taskManager);
            }
        }

        private class ResponseJson
        {
            /// <summary>
            /// Токен
            /// </summary>
            public string access_token { get; set; }

            /// <summary>
            /// Тип токена
            /// </summary>
            public string token_type { get; set; }

            /// <summary>
            /// Окончание действия токена
            /// </summary>
            public string expires_in { get; set; }

            /// <summary>
            /// Выдан в контексте
            /// </summary>
            public string scope { get; set; }

            /// <summary>
            /// Отчество
            /// </summary>
            public string patronymic { get; set; }

            /// <summary>
            /// Система (вид, тип)
            /// </summary>
            public string system { get; set; }

            /// <summary>
            /// Фамилия
            /// </summary>
            public string surname { get; set; }

            /// <summary>
            /// Имя
            /// </summary>
            public string name { get; set; }

            /// <summary>
            /// RS ID 
            /// </summary>
            public string rsId { get; set; }

            /// <summary>
            /// ID пользователя
            /// </summary>
            public string userId { get; set; }

            /// <summary>
            /// Е-мэйл
            /// </summary>
            public string email { get; set; }
            
            /// <summary>
            /// Является суперпользователем
            /// </summary>
            public string superUser { get; set; }

            /// <summary>
            /// JTI (?)
            /// </summary>
            public string jti { get; set; }
        }
    }
}
