namespace Bars.Gkh.Gis.ViewModel.KpSettings
{
    using System.Linq;
    using B4;
    using Enum;
    using Entities.Kp50;

    public class BilConnectionViewModel : BaseViewModel<BilConnection>
    {
        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат получения списка
        /// </returns>
        public override IDataResult List(IDomainService<BilConnection> domainService, BaseParams baseParams)
        {
            string appUrl = baseParams.Params["SERVER_NAME"].ToString();
            int dataCount = domainService.GetAll().Count(x => x.AppUrl == appUrl);
            if (dataCount == 0)
            {
                this.CreateBaseConnection(domainService, appUrl);
            }

            var result = domainService.GetAll().Where(x => x.AppUrl == appUrl).Select(
                x => new
                {
                    x.Id,
                    x.Connection,
                    x.ConnectionType
                }).ToList();

            return new ListDataResult(result, result.Count());
        }

        /// <summary>
        /// Создание 
        /// </summary>
        /// <param name="domainService">Домен</param>
        /// <param name="appUrl">Базовые параметры</param>
        private void CreateBaseConnection(IDomainService<BilConnection> domainService, string appUrl)
        {
            domainService.Save(
                new BilConnection()
                {
                    AppUrl = appUrl,
                    ConnectionType = ConnectionType.GisConnStringInc,
                    Connection = ""
                });
            domainService.Save(
                new BilConnection()
                {
                    AppUrl = appUrl,
                    ConnectionType = ConnectionType.GisConnStringIncKzn,
                    Connection = ""
                });
            domainService.Save(
                new BilConnection()
                {
                    AppUrl = appUrl,
                    ConnectionType = ConnectionType.GisConnStringIncRt,
                    Connection = ""
                });
            domainService.Save(
                new BilConnection()
                {
                    AppUrl = appUrl,
                    ConnectionType = ConnectionType.GisConnStringReports,
                    Connection = ""
                });
            domainService.Save(
                new BilConnection()
                {
                    AppUrl = appUrl,
                    ConnectionType = ConnectionType.GisConnStringPgu,
                    Connection = ""
                });
        }
    }
}