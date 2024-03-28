namespace Bars.GkhGji.Regions.Tatarstan.DomainService.ActionIsolated
{
    using Bars.B4;

    public interface ITaskActionIsolatedService
    {
        /// <summary>
        /// Список мероприятий для карточки обращения гражданина
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult ListForCitizenAppeal(BaseParams baseParams);
    }
}