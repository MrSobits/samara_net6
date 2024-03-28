using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;

namespace Sobits.GisGkh.DomainService
{
    /// <summary>
    /// Сервис работы с ГИС ЖКХ
    /// </summary>
    public interface IGISGKHService
    {
        /// <summary>
        /// Получение неподписанной XML
        /// </summary>
        /// <param name="reqId">Id запроса в ГИС ЖКХ</param>
        /// <returns>строка с XML</returns>
        string GetXML(long reqId);

        /// <summary>
        /// Сохранение подписанной XML
        /// </summary>
        /// <param name="reqId">Id запроса в ГИС ЖКХ</param>
        /// <param name="signedData">Подписанный запрос</param>
        void SaveSignedXML(long reqId, string signedData);

        /// <summary>
        /// Отправка запроса
        /// </summary>
        /// <param name="reqId">Id запроса в ГИС ЖКХ</param>
        /// <returns>true, если успешно</returns>
        bool SendRequest(long reqId);


        IDataResult ListDisposalAndDecisionForGisGkh(BaseParams baseParams);
        IDataResult ListResolutionForGisGkh(BaseParams baseParams);
        IDataResult ListROForGisGkhExport(BaseParams baseParams);
        IDataResult ListMOForGisGkhExport(BaseParams baseParams);
        IDataResult ListProgramForGisGkhExport(BaseParams baseParams);
        IDataResult ListContragentForGisGkhExport(BaseParams baseParams);
        IDataResult ListRooms(BaseParams baseParams);
        IDataResult ListPremises(BaseParams baseParams);
        IDataResult ListTasksForMassSign(BaseParams baseParams);
        IDataResult ListDownloads(BaseParams baseParams);
        IDataResult ListProgramImport(BaseParams baseParams);
        IDataResult ListObjectCrImport(BaseParams baseParams);
        IDataResult ListBuildContractImport(BaseParams baseParams);
        IDataResult ListBuildContractForActImport(BaseParams baseParams);
        IDataResult ListPerfWorkActImport(BaseParams baseParams);
        bool MatchRoom(BaseParams baseParams, long roomId, long premisesId);
        bool UnMatchRoom(BaseParams baseParams, long roomId);
        bool DownloadFiles(BaseParams baseParams);
    }
}
