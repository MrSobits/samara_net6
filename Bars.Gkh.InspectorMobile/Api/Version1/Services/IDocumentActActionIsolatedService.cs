using Bars.Gkh.InspectorMobile.Api.Version1.Models.ActActionIsolated;

namespace Bars.Gkh.InspectorMobile.Api.Version1.Services
{
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;

    /// <summary>
    /// Интерфейс сервиса документа "Акт КНМ без взаимодействия с контролируемыми лицами"
    /// </summary>
    public interface IDocumentActActionIsolatedService : IDocumentWithParentService<DocumentActActionIsolatedGet, DocumentActActionIsolatedCreate, DocumentActActionIsolatedUpdate, BaseDocQueryParams>
    {
    }
}