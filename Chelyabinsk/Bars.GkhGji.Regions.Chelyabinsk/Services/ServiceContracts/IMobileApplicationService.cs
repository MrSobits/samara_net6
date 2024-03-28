using System.ServiceModel;

namespace Bars.GkhGji.Regions.Chelyabinsk.Services.ServiceContracts
{
    using Bars.GkhGji.Regions.Chelyabinsk.Services.DataContracts.SyncOperators;
    using Bars.GkhGji.Regions.Chelyabinsk.Services.DataContracts.SyncDictionaries;
    using System;
    using Bars.GkhGji.Regions.Chelyabinsk.Services.DataContracts.SyncInspections;
    using Bars.GkhGji.Regions.Chelyabinsk.Services.DataContracts;

    /// <summary>
    /// Сервис обмена данными с мобильным приложением ГЖИ
    /// </summary>
    [ServiceContract]
    public interface IMobileApplicationService
    {
        /// <summary>
        /// Запрос списка операторов
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [XmlSerializerFormat]
        OperatorGJIResponse GetOperatorsList(string Token);

        /// <summary>
        /// Запрос справочников
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [XmlSerializerFormat]
        DictionaryGJIResponse GetDictionaries(string Token);

        /// <summary>
        /// Запрос проверок
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [XmlSerializerFormat]
        IncpectionGJIResponse GetInspections(string Token, long operatorId);

        /// <summary>
        /// Импорт результатов проверок
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [XmlSerializerFormat]
        RequestResult ImportInspectionResult(string Token, long inspector, IncpectionGJIRequest request);
        /// <summary>
        /// Импорт результатов проверок
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [XmlSerializerFormat]
        GetReportResponse ActpPintedFormt(string Token, string reportCode, long docId);

    }
}
