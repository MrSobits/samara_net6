namespace Bars.Gkh.InspectorMobile.Api.Version1.Services
{
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.VisitSheet;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    /// <summary>
    /// Интерфейс API сервиса для <see cref="VisitSheet"/>
    /// </summary>
    public interface IDocumentVisitSheetService : IDocumentWithParentService<DocumentVisitSheetGet, DocumentVisitSheetCreate, DocumentVisitSheetUpdate, BaseDocQueryParams>
    {
    }
}