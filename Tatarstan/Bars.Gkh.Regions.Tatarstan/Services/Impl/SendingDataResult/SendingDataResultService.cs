namespace Bars.Gkh.Regions.Tatarstan.Services.Impl.SendingDataResult
{
    using System;
    using System.IO;
    using System.Net;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.ConfigSections.Administration;
    using Bars.Gkh.DomainService.Config;
    using Bars.Gkh.Regions.Tatarstan.Entities.SendingDataResult;
    using Bars.Gkh.Regions.Tatarstan.Services.ServicesContracts.SendingDataResult;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    using Newtonsoft.Json;

    public class SendingDataResultService : ISendingDataResultService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }
        
        /// <inheritdoc />
        public IDataResult GetSendingDataResult(string risToken)
        {
            try
            {
                if (string.IsNullOrEmpty(risToken))
                {
                    throw new SendingDataResultObtainmentException("Произошла ошибка при получении информации о результатах отправки данных с ГИС ЖКХ: отсутствует токен авторизации");
                }

                var configurationService = this.Container.Resolve<IGkhConfigService>();
                string endPointAddress;

                using (this.Container.Using(configurationService))
                {
                    endPointAddress = this.Container.GetGkhConfig<AdministrationConfig>()?
                        .FormatDataExport?
                        .FormatDataExportGeneral?
                        .TransferServiceAddress;

                    if (string.IsNullOrEmpty(endPointAddress))
                    {
                        throw new SendingDataResultObtainmentException(
                            "Произошла ошибка при получении информации о результатах отправки данных с ГИС ЖКХ: не задан адрес удаленного сервера РИС ЖКХ в настройках приложения");
                    }
                }
                
                var webRequest = (HttpWebRequest) WebRequest.Create(endPointAddress + "/intregration/info");
                webRequest.Headers.Add("Authorization", "Token " + risToken);
                webRequest.Timeout = 10000;

                var response = (HttpWebResponse) webRequest.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    var sendingDataResultInfo = JsonConvert.DeserializeObject<SendingDataResult>(responseText);

                    if (!string.IsNullOrEmpty(sendingDataResultInfo.ErrorMessage))
                    {
                        throw new SendingDataResultObtainmentException("Произошла ошибка при получении информации о результатах отправки данных с ГИС ЖКХ: " + sendingDataResultInfo.ErrorMessage);
                    }

                    return new BaseDataResult(sendingDataResultInfo);
                }
            }
            catch (SendingDataResultObtainmentException ex)
            {
                return new BaseDataResult(false, ex.Message);
            }
            catch (Exception ex)
            {
                return new BaseDataResult(false, "Произошла непредвиденная ошибка при получении информации о результатах отправки данных с ГИС ЖКХ");
            }
        }

        public class SendingDataResultObtainmentException : Exception
        {
            public SendingDataResultObtainmentException(string message)
                : base(message) { }
        }
    }
}