namespace Bars.Gkh.InspectorMobile.Api.Version1.Services
{
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.TaskPreventiveAction;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    /// <summary>
    /// Интейрейс сервиса документа <see cref="PreventiveActionTask"/>
    /// </summary>
    public interface IDocumentTaskPreventiveActionService : IDocumentWithParentService<DocumentTaskPreventiveActionGet, object, DocumentTaskPreventiveActionUpdate, TaskPreventiveActionQueryParams>
    {
    }
}