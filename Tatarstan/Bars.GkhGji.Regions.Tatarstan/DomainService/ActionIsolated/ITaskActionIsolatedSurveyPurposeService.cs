namespace Bars.GkhGji.Regions.Tatarstan.DomainService.ActionIsolated
{
    using Bars.B4;

    public interface ITaskActionIsolatedSurveyPurposeService
    {
        /// <summary>
        /// Добавить цели
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult AddPurposes(BaseParams baseParams);
    }
}