namespace Bars.GkhGji.Regions.Tatarstan.DomainService.ActionIsolated
{
    using Bars.B4;

    public interface ITaskActionIsolatedArticleLawService
    {
        /// <summary>
        /// Добавить статьи закона задания профилактического мероприятия
        /// </summary>
        IDataResult AddArticles(BaseParams baseParams);
    }
}
