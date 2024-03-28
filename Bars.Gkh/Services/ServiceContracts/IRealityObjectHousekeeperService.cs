using System.ServiceModel;

namespace Bars.Gkh.Services.ServiceContracts
{
    using Bars.Gkh.Services.DataContracts.RealityObjectHousekeeper;

    public partial interface IService
    {
        /// <summary>
        /// Запрос списка операторов
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [XmlSerializerFormat]
        GetRealityObjectHousekeeperResponse GetHousekeepersList(string Token);       

    }
}
