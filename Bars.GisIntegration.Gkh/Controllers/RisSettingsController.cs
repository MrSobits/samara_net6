namespace Bars.GisIntegration.Gkh.Controllers
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Security.Web.Service;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Gkh.Security;
    using Bars.GisIntegration.UI.Entities;

    using Newtonsoft.Json;

    public class RisSettingsController : BaseController
    {
        public ActionResult SaveParams(BaseParams baseParams)
        {
            var serviceNameValue = baseParams.Params.GetAs<string>("ServiceName");
            var loginValue = baseParams.Params.GetAs<string>("Login");
            var passwordValue = baseParams.Params.GetAs<string>("Password");

            var settingsDomain = this.Container.ResolveDomain<RisSettings>();

            try
            {
                var settingsList = settingsDomain.GetAll().ToList();

                if (serviceNameValue.IsNotEmpty())
                {
                    var serviceNameExists = settingsList.FirstOrDefault(x => x.Name == "ServiceName");
                    if (serviceNameExists != null)
                    {
                        serviceNameExists.Value = serviceNameValue;
                        settingsDomain.Save(serviceNameExists);
                    }
                    else
                    {
                        var newServiceName = new RisSettings
                        {
                            Code = "gisIntegration",
                            Name = "ServiceName",
                            Value = serviceNameValue
                        };
                        settingsDomain.Save(newServiceName);
                    }
                }

                if (loginValue.IsNotEmpty())
                {
                    var loginExists = settingsList.FirstOrDefault(x => x.Name == "Login");
                    if (loginExists != null)
                    {
                        loginExists.Value = loginValue;
                        settingsDomain.Save(loginExists);
                    }
                    else
                    {
                        var newLogin = new RisSettings
                        {
                            Code = "gisIntegration",
                            Name = "Login",
                            Value = loginValue
                        };
                        settingsDomain.Save(newLogin);
                    }
                }

                if (passwordValue.IsNotEmpty())
                {
                    var passwordExists = settingsList.FirstOrDefault(x => x.Name == "Password");
                    if (passwordExists != null)
                    {
                        passwordExists.Value = passwordValue;
                        settingsDomain.Save(passwordExists);
                    }
                    else
                    {
                        var newPassword = new RisSettings
                        {
                            Code = "gisIntegration",
                            Name = "Password",
                            Value = passwordValue
                        };
                        settingsDomain.Save(newPassword);
                    }
                }

                return this.JsSuccess();
            }
            finally
            {
                this.Container.Release(settingsDomain);
            }
        }

        public ActionResult GetParams(BaseParams baseParams)
        {
            var settingsCode = baseParams.Params.GetAs<string>("settingsCode");

            var settingsDomain = this.Container.ResolveDomain<RisSettings>();

            try
            {
                var settings = settingsDomain.GetAll()
                    .Where(x => x.Code == settingsCode)
                    .GroupBy(x => x.Name)
                    .ToDictionary(x => x.Key, x => x.First().Value);

                var data = new
                {
                    ServiceName = settings.ContainsKey("ServiceName") ? settings["ServiceName"] : string.Empty,
                    Login = settings.ContainsKey("Login") ? settings["Login"] : string.Empty,
                    Password = settings.ContainsKey("Password") ? settings["Password"] : string.Empty
                };

                return new JsonGetResult(data);
            }
            finally
            {
                this.Container.Release(settingsDomain);
            }
        }

        public ActionResult GetRisUrl(string redirect)
        {
            var service = this.Container.Resolve<RisManager>();

            using (this.Container.Using(service))
            {
                var risUrl = service.GetRisUrl(redirect);
                if (risUrl == null)
                {
                    return this.JsFailure("Пользователь не аутентифицирован");
                }
                return new JsonNetResult(risUrl);
            }
        }
    }
}