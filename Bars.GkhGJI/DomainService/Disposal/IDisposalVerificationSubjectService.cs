namespace Bars.GkhGji.DomainService
{
    using Bars.B4;

    public interface IDisposalVerificationSubjectService
    {
        IDataResult AddSurveySubjects(BaseParams baseParams);
        IDataResult AddSurveySubjects(long documentId, long[] ids);
        /// <summary>
        /// Добавить Требования НПА проверки
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        IDataResult AddNormDocItems(BaseParams baseParams);
    }
}
