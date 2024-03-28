namespace Bars.Gkh.Services.ServiceContracts
{
    using System.ServiceModel;

    using DataContracts.GetOperationTime;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        GetCreateHouseResult GetCreateHouse(GetCreateHouseResponse requestData);
    }
}