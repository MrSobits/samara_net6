namespace Bars.GisIntegration.Base.Tasks.SendData.HouseManagement
{
    using System;
    using HouseManagementAsync;

    /// <summary>
    /// Задача отправки данных по домам для УО
    /// </summary>
    public class HouseUOExportDataTask: BaseExportHouseDataTask<importHouseUORequest>
    {
        /// <summary>
        /// Выполнить запрос к асинхронному сервису ГИС
        /// </summary>
        /// <param name="header">Заголовок запроса</param>
        /// <param name="request">Объект запроса</param>
        /// <returns>Идентификатор сообщения для получения результата</returns>
        protected override string ExecuteRequest(RequestHeader header, importHouseUORequest request)
        {
            AckRequest result;
            var soapClient = this.ServiceProvider.GetSoapClient();

            if (soapClient == null)
            {
                throw new Exception("Не удалось получить SOAP клиент");
            }

            soapClient.importHouseUOData(header, request, out result);

            if (result?.Ack == null)
            {
                throw new Exception("Пустой результат выполенния запроса");
            }

            return result.Ack.MessageGUID;
        }
    }
}