namespace Bars.Gkh.InspectorMobile.Api.Version1.Services
{
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Prescription;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Интерфейс сервиса для взаимодействия с <see cref="Prescription"/>
    /// </summary>
    public interface IDocumentPrescriptionService : IDocumentWithParentService<DocumentPrescriptionGet, DocumentPrescriptionCreate, DocumentPrescriptionUpdate, BaseDocQueryParams>
    {
    }
}