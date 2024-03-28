namespace Bars.Gkh.Services.ServiceContracts
{
    using System.ServiceModel;

    using Bars.Gkh.Services.DataContracts.HouseSearch;

    using CoreWCF.Web;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetRaionList")]
        RaionListResponse GetRaionList();

        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetCities/{munId}")]
        CitiesResponse GetCities(string munId);

        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetStreets/{cityId}")]
        StreetsResponse GetStreets(string cityId);

        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetHouses/{streetId}/{filter=null}")]
        HousesResponse GetHouses(string streetId, string filter);

        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetHousesByMu/{munId}")]
        HousesResponse GetHousesByMu(string munId);

        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetFlats/{houseId}")]
        FlatsResponse GetFlats(string houseId);
    }
}