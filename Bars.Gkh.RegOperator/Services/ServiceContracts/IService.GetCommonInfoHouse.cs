namespace Bars.Gkh.RegOperator.Services.ServiceContracts
{
    using System.ServiceModel;
    using CoreWCF.Web;

    using DataContracts;

    /// <summary>
    /// Интерфейс для сервиса регопа метода <see cref="GetCommonInfoHouse"/>
    /// </summary>
    public partial interface IService
    {
        /// <summary>
        /// Метод используется для построения графиков в аналитике по домам, 
        /// жителям в них, о накопленных и оплаченных взносах на капремонт, 
        /// о вхождении домов в краткосрочную программу капитального ремонта. 
        /// </summary>
        /// <param name="year">Год</param>
        /// <returns>Результат запроса</returns>
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetCommonInfoHouse/{year}")]
        CommonInfoMkd GetCommonInfoHouse(string year);
    }
}
