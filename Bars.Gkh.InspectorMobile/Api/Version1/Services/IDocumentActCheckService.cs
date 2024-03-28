namespace Bars.Gkh.InspectorMobile.Api.Version1.Services
{
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.ActCheck;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;

    /// <summary>
    /// Интерфейс сервиса документа "Акт проверки"
    /// </summary>
    public interface IDocumentActCheckService : IDocumentWithParentService<DocumentActCheckGet, DocumentActCheckCreate, DocumentActCheckUpdate, BaseDocQueryParams>
    {
    }
}