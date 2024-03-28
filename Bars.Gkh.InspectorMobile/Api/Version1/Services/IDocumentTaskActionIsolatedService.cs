namespace Bars.Gkh.InspectorMobile.Api.Version1.Services
{
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.TaskActionIsolated;

    /// <summary>
    /// Сервис для работы с документов <see cref="DocumentTaskActionIsolated"/>
    /// </summary>
    public interface IDocumentTaskActionIsolatedService : IDocumentWithParentService<DocumentTaskActionIsolated, object, object, TaskActionIsolatedQueryParams>
    {
    }
}