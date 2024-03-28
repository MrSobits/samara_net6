namespace Bars.Gkh.Gasu.DomainService
{
    using System.Net;
    using System.Text;
    using Bars.B4;
    using Bars.B4.Config;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Gasu.ImportExport;
    using Castle.Windsor;
    using System;
    using System.Linq;

    public class GasuImportExportService : IGasuImportExportService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult GetServiceData(BaseParams baseParams)
        {
            var gasuAddress = baseParams.Params.GetAs<string>("gasuAddress");
            var config = Container.Resolve<IConfigProvider>().GetConfig();
            var data = config.AppSettings["GasuServiceData"] as string ?? gasuAddress;

            if (!System.IO.File.Exists(data))
            {
                return new BaseDataResult(new GasuServiceConfig());
            }

            var serviceConfig =
                    JsonNetConvert.DeserializeObject<GasuServiceConfig>(System.IO.File.ReadAllText(data));

            var authorization = Container.ResolveAll<IAuthorizationService>().FirstOrDefault();

            // если нет права на изменение сервиса, то пароль от сервиса на клиент не отправляем
            // адрес и имя пользователя нужно отправлять чтобы была возможность проверить что данные отправляются к нужному сервису
            if (authorization != null &&
                !authorization.Grant(Container.Resolve<IUserIdentity>(), "Export.GasuService_Edit"))
            {
                serviceConfig.UserPassword = new string(' ', serviceConfig.UserPassword.Length);
            }

            return new BaseDataResult(serviceConfig);
        }

		public IDataResult SetServiceData(BaseParams baseParams)
		{
			var serviceUrl = baseParams.Params.GetAs<string>("serviceUrl");
			var userName = baseParams.Params.GetAs<string>("userName");
			var userPassword = baseParams.Params.GetAs<string>("userPassword");
			var gasuAddress = baseParams.Params.GetAs<string>("gasuAddress");

			var config = Container.Resolve<IConfigProvider>().GetConfig();
			var data = config.AppSettings["GasuServiceData"] as string ?? gasuAddress;
			var authorization = Container.ResolveAll<IAuthorizationService>().FirstOrDefault();

			// есть ли право модифицировать адрес сервиса
			if (authorization == null ||
			   authorization.Grant(Container.Resolve<IUserIdentity>(), "Export.GasuService_Edit") && !string.IsNullOrEmpty(serviceUrl))
			{
				// если есть, то записать его
				System.IO.File.WriteAllText(data, JsonNetConvert.SerializeObject(Container, new GasuServiceConfig
				{
					ServiceUrl = serviceUrl,
					UserName = userName,
					UserPassword = userPassword
				}));

				return new BaseDataResult(true, "Данные сервиса ГАСУ сохранены");
			}

			return new BaseDataResult(false, "Недостаточно прав для сохранения данных сервиса ГАСУ");
		}

        public IDataResult SendGasu(BaseParams baseParams)
        {
            // вытаскиваем параметры запроса
            var serviceUrl = baseParams.Params.GetAs<string>("serviceUrl");
            var userName = baseParams.Params.GetAs<string>("userName");
            var userPassword = baseParams.Params.GetAs<string>("userPassword");
            var exportName = baseParams.Params.GetAs<string>("exportName");
            var gasuAddress = baseParams.Params.GetAs<string>("gasuAddress");

            var dateStart = baseParams.Params.GetAs<DateTime>("dateStart");
            var exporter = Container.Resolve<IDataExportService>(exportName);

            var config = Container.Resolve<IConfigProvider>().GetConfig();
            var data = config.AppSettings["GasuServiceData"] as string ?? gasuAddress;
            var authorization = Container.ResolveAll<IAuthorizationService>().FirstOrDefault();

            // есть ли право модифицировать адрес сервиса
            if (authorization == null ||
               authorization.Grant(Container.Resolve<IUserIdentity>(), "Export.GasuService_Edit") && !string.IsNullOrEmpty(serviceUrl))
            {
                // если есть, то записать его
                System.IO.File.WriteAllText(data, JsonNetConvert.SerializeObject(Container, new GasuServiceConfig
                {
                    ServiceUrl = serviceUrl,
                    UserName = userName,
                    UserPassword = userPassword
                }));
            }
            else
            {
                // а если нет, то заменить текущие параметры на уже сконфигурированные
                if (!System.IO.File.Exists(data))
                {
                    return new BaseDataResult(false, "Произошла ошибка при запросе сервиса ГАСУ. Ошибка: данные сервиса не сконфигурированы");
                }

                var serviceConfig =
                    JsonNetConvert.DeserializeObject<GasuServiceConfig>(System.IO.File.ReadAllText(data));
                serviceUrl = serviceConfig.ServiceUrl;
                userName = serviceConfig.UserName;
                userPassword = serviceConfig.UserPassword;
            }

            var exportParams = new BaseParams();
            exportParams.Params = baseParams.Params;
            exportParams.Params.Add("periodStart", dateStart);

            // сформировать данные для отправки
            var exportedData = exporter.ExportData(exportParams);

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(serviceUrl + "/data");
                request.Method = "PUT";
                request.ContentType = "application/xml";
                request.MediaType = "application/xml";

                // создаем заголовок авторизации "BASIC base64(USERNAME:PASSWORD)"
                var authorizationHeader = userName + ":" + userPassword;
                var encodedHeader = Convert.ToBase64String(
                    Encoding.UTF8.GetBytes(authorizationHeader),
                    Base64FormattingOptions.InsertLineBreaks);
                request.Headers.Add("Authorization", "Basic " + encodedHeader);

                using (var writer = request.GetRequestStream())
                {
                    exportedData.FileStream.CopyTo(writer);
                    writer.Flush();
                    writer.Close();
                }

                var response = request.GetResponse();

                response.Close();
                return new BaseDataResult();
            }
            catch (Exception exception)
            {
                return new BaseDataResult(false, "Произошла ошибка при запросе сервиса ГАСУ. Ошибка: " + exception.Message);
            }
        }
	}
}
