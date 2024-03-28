using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.ExecutionAction;
using Bars.GkhGji.Regions.Habarovsk.Tasks.GetSMEVAnswers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel;
using Bars.GkhGji.Entities.PosAppeal;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Bars.GkhGji.Entities;
using Bars.Gkh.Entities;
using Bars.GkhGji.Regions.Habarovsk.DomainService;
using Bars.B4.Config;

namespace Bars.GkhGji.Regions.Habarovsk.ExecutionAction
{
    /// <summary>
    /// Периодическая проверка СМЭВа на результаты
    /// </summary>
    public class GetSMEVAnswersAction : BaseExecutionAction//, IMandatoryExecutionAction
    {
        public IComplaintsService _cpmplService;

        private string thistoken;

        public GetSMEVAnswersAction(
           IComplaintsService complServ)
        {
            this._cpmplService = complServ;
        }
        public override string Description => "Запрашивает из СМЭВа все ответы и обрабатывает их (пока только те, что добавлены после 2018)";

        public override string Name => "Проверить ответы в СМЭВ";

        public override Func<IDataResult> Action => GetSMEVResponses;

        // public bool IsNeedAction() => true;

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
                                return null;
                                return rootobject;
                                  // _cpmplService.CreateAppeals(rootobject);
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


        private IDataResult GetSMEVResponses()
        {
            try
            {
                Task<Rootobject> task = Task.Run<Rootobject>(async () => await GetPOS());
                var res =  task.Result;
                if (res != null)
                {
                    _cpmplService.CreateAppeals(res, thistoken);
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
    }
}
