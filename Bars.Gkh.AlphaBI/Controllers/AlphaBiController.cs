namespace Bars.Gkh.AlphaBI.Controllers
{
    using System;
    using System.IO;
    using System.Net;
    using System.Web.Mvc;
    using Bars.B4;
    using Bars.B4.Config;
    using Bars.B4.Utils;

    /// <summary>
    /// 
    /// </summary>
    public class AlphaBiController : BaseController
    {
        public ActionResult ToAlphaBI()
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

            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

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
                    Redirect("http://" + host + "/?authToken={0}&{1}".FormatUsing(js["TokenValue"], paramsString));
            }
        }
    }
}
