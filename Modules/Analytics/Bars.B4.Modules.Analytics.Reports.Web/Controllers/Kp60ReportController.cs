namespace Bars.B4.Modules.Analytics.Reports.Web.Controllers
{
    using System;
    using System.Web;

    using Bars.B4;
    using Bars.B4.Config;
    using Bars.B4.Modules.Analytics.Reports.Web.Utils;

    using Microsoft.AspNetCore.Mvc;

    public class Kp60ReportController : BaseController
    {
        public ActionResult ToKp60Report()
        {
            var config = this.Container.Resolve<IConfigProvider>().GetConfig();
            var hostKey = "Kp60-Host";
            var loginKey = "Kp60-Login";
            var passwordKey = "Kp60-Password";

            if (!config.AppSettings.ContainsKey(hostKey))
            {
                throw new Exception("В файле конфигурации не указан адрес приложения КП60");
            }
            if (!config.AppSettings.ContainsKey(loginKey))
            {
                throw new Exception("В файле конфигурации не указан логин в КП60");
            }
            if (!config.AppSettings.ContainsKey(passwordKey))
            {
                throw new Exception("В файле конфигурации не указан пароль в КП60");
            }

            var host = config.AppSettings.GetAs<string>(hostKey);
            var login = config.AppSettings.GetAs<string>(loginKey);
            var password = config.AppSettings.GetAs<string>(passwordKey);
            var key = Convert.FromBase64String("2NQeBS0lpayLsraZYCnwdwWNYvgKvbxynxo04Kb2O8o=");
            var iv = Convert.FromBase64String("hkpx0ON9A07XbcTIrUFYvA==");

            var json =
                $@"{{""Login"": ""{login}"", ""Password"": ""{password}"", ""CurrentTime"": ""{DateTime.Now}"", ""SourceSystem"": ""Gkh"", ""Route"": ""kpreports"", ""IframeMode"": true}}";

            var aes = new AesSerializer(key, iv);

            var encryptedToken = aes.Encrypt(json);

            var token = HttpUtility.UrlEncode(Convert.ToBase64String(encryptedToken));

            return this.Redirect($"{host}login/loginbytoken?token={token}");
        }
    }
}
