namespace Bars.Gkh.Services.ServiceContracts
{
    using Bars.Gkh.Services.DataContracts.GetAllHouses;
    using System.ServiceModel;

    using CoreWCF.Web;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetAllHouses/{afterDate}")]
        AllHousesResponse GetAllHouses(string afterDate);

        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetHouseProp/{houseId}")]
        HouseElemPropResponse GetHouseProp(string houseId);
    }
}