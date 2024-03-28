namespace Bars.GkhCr.Services.ServiceContracts
{
    using System.ServiceModel;
    using CoreWCF.Web;

    /// <summary>
    /// Информация о работах ДПКР (РТ)
    /// </summary>
    public partial interface IService
    {
        /// <summary>
        /// Информация о работах ДПКР (РТ)
        /// </summary>
        /// <param name="municipalityId">МО</param>
        /// <param name="housesId">Уникальный номер дома</param>
        /// <returns>Информация о работе ДПКР и результат</returns>
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetDpkrWork/{municipalityId}/{housesId}")]
        GetDpkrWorkResponse GetDpkrWork(string municipalityId, string housesId);
    }
}