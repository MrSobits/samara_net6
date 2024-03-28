namespace Bars.GkhEdoInteg.DomainService
{
    using Bars.B4;

    public interface IAppealCitsEdoIntegService
    {
        /// <summary>
        /// Получение списка документов и ответов обращения граждан
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult ListInspectionDocsAndAnswers(BaseParams baseParams);

        /// <summary>
        /// Получение данных по обращениям
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult ListAppealCitsLog(BaseParams baseParams);

        /// <summary>
        /// Получени лога загрузки обращений
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult ListLogRequests(BaseParams baseParams);

        IDataResult ListRequestsAppealCits(BaseParams baseParams);
    }
}
