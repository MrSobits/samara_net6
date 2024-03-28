namespace Bars.Gkh.InspectorMobile.Api.Version1.Services
{
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Protocol;

    /// <summary>
    /// Интерфейс сервиса документа "Акт проверки"
    /// </summary>
    public interface IDocumentProtocolService : IDocumentWithParentService<DocumentProtocolGet, DocumentProtocolCreate, DocumentProtocolUpdate, BaseDocQueryParams>
    {
    }
}