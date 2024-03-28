namespace Bars.Gkh.Services.ServiceContracts.GlonassIntegration
{
    using System.ServiceModel;

    using CoreWCF.Web;

    using DataContracts.GlonassIntegration;

    public partial interface IGlonassIntegService
    {
        /// <summary>
        /// Получить информацию по дому
        /// </summary>
        /// <param name="id">Идентификатор дома</param>
        /// <returns>Ответ от сервиса с результатом выполнения запроса</returns>
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "gethouse/{id}")]
        GetHouseResponse GetHouse(string id);
    }
}