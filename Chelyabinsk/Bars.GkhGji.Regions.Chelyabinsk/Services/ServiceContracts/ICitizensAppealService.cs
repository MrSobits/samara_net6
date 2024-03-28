using System.ServiceModel;

using Bars.GkhGji.Services.DataContracts;

namespace Bars.GkhGji.Regions.Chelyabinsk.Services.ServiceContracts
{
    using System;

    /// <summary>
    /// Сервис сведений об обращениях граждан
    /// </summary>
    [ServiceContract]
    public interface ICitizensAppealService
    {
        /// <summary>
        /// Импорт сведений об обращении граждан
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [XmlSerializerFormat]
        AppealTransferResult[] ImportInfoCitizensAppealRequest(CitizenAppeal[] appeals);

        /// <summary>
        /// Импорт сведений об отмене обращения граждан
        /// </summary>
        [OperationContract]
        [XmlSerializerFormat]
        CitizensAppealCancelResult[] ImportInfoCitizensAppealCancelRequest(Guid[] appealUids);
    }
}
