namespace Bars.Gkh.Regions.Tatarstan.Services.Impl.RisDebtInfo
{
    using System;
    using System.IO;
    using System.Net;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.ConfigSections.Administration;
    using Bars.Gkh.DomainService.Config;
    using Bars.Gkh.Regions.Tatarstan.Entities.RisDebtInfo;
    using Bars.Gkh.Regions.Tatarstan.Services.ServicesContracts.RisDebtInfo;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    using Newtonsoft.Json;

    public class DebtSubRequestInformationService : IDebtSubRequestInformationService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public IDataResult GetDebtInfo(string risToken)
        {
            try
            {
                if (string.IsNullOrEmpty(risToken))
                {
                    throw new DebtInfoObtainmentException("Произошла ошибка при получении информации о задолженностях: отсутствует токен авторизации");
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
                        throw new DebtInfoObtainmentException(
                            "Произошла ошибка при получении информации о задолженностях: не задан адрес удаленного сервера РИС ЖКХ в настройках приложения");
                    }
                }
                
                var webRequest = (HttpWebRequest) WebRequest.Create(endPointAddress + "/subrequest/list");
                webRequest.Headers.Add("Authorization", "Token " + risToken);
                webRequest.Timeout = 10000;

                var response = (HttpWebResponse) webRequest.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    var debtInfo = JsonConvert.DeserializeObject<DebtInfo>(responseText);

                    if (!string.IsNullOrEmpty(debtInfo.ErrorMessage))
                    {
                        throw new DebtInfoObtainmentException("Произошла ошибка при получении информации о задолженностях: " + debtInfo.ErrorMessage);
                    }

                    return new BaseDataResult(debtInfo);
                }
            }
            catch (DebtInfoObtainmentException ex)
            {
                return new BaseDataResult(false, ex.Message);
            }
            catch (Exception ex)
            {
                return new BaseDataResult(false, "Произошла непредвиденная ошибка при получении информации о задолженностях");
            }
        }

        public class DebtInfoObtainmentException : Exception
        {
            public DebtInfoObtainmentException(string message)
                : base(message) { }
        }
    }
}