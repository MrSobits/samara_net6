using System.ServiceModel;

using Bars.GkhGji.Services.DataContracts;

namespace Bars.GkhGji.Regions.BaseChelyabinsk.Services.ServiceContracts
{
    using System;

    /// <summary>
    /// Сервис работы с АМИРС
    /// </summary>
    [ServiceContract]
    public interface IAmirsService
    {
        /// <summary>
        /// Импорт сведений из АМИРС
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [XmlSerializerFormat]
        AmirsResult[] ImportAmirs(AmirsData[] records, string token);
    }
}
