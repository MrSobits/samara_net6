namespace Bars.Gkh.Gis.Controllers.Olap
{
    using System;
    using System.IO;
    using System.Net;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.Config;
    using B4.Utils;

    public class CubeController: BaseController
    {
        /// <summary>
        /// Загрузить кубы от Альфы
        /// </summary>        
        public ActionResult LoadAlphaBI(BaseParams baseParams)
        {
            var config = Container.Resolve<IConfigProvider>().GetConfig();
            const string prefix = "AlphaBi";
            var hostKey = string.Format("{0}-Host", prefix);
            var authTokenKey = string.Format("{0}-AuthToken", prefix);
            var loginKey = string.Format("{0}-Login", prefix);
            var passwordKey = string.Format("{0}-Password", prefix);
            var paramsKey = string.Format("{0}-Params", prefix);

            var host = "localhost";
            var authToken = string.Empty;
            var login = string.Empty;
            var password = string.Empty;
            var paramsString = string.Empty;

            if (config.AppSettings.ContainsKey(hostKey))
            {
                host = config.AppSettings.GetAs<string>(hostKey);
            }
            if (config.AppSettings.ContainsKey(authTokenKey))
            {
                authToken = config.AppSettings.GetAs<string>(authTokenKey);
            }
            if (config.AppSettings.ContainsKey(loginKey))
            {
                login = config.AppSettings.GetAs<string>(loginKey);
            }
            if (config.AppSettings.ContainsKey(passwordKey))
            {
                password = config.AppSettings.GetAs<string>(passwordKey);
            }
            if (config.AppSettings.ContainsKey(paramsKey))
            {
                paramsString = config.AppSettings.GetAs<string>(paramsKey);
            }            
            var json = string.Format(@"{{""Login"": ""{0}"", ""Password"": ""{1}"", ""AuthToken"": ""{2}""}}", login, password, authToken);

            var httpWebRequest = WebRequest.Create("http://" + host + "/login/login") as HttpWebRequest;
            httpWebRequest.Proxy = null;
            httpWebRequest.Timeout = 100000;

            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            System.Net.ServicePointManager.Expect100Continue = false;
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(json);
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var responseText = streamReader.ReadToEnd();
                var js = DynamicDictionary.FromJson(responseText);
                return
                    Redirect(string.Format("http://{0}/?OpenAction={1}&authToken={2}&{3}", host, 14209, js["TokenValue"],
                        paramsString));
            }            
        }
    }
}
