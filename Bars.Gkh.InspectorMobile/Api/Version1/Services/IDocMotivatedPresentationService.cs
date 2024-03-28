namespace Bars.Gkh.InspectorMobile.Api.Version1.Services
{
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.MotivationPresentation;

    /// <summary>
    /// Сервис для МП документа "Мотивированное представление"
    /// </summary>
    public interface IDocMotivatedPresentationService : IDocumentWithParentService<MotivationPresentationGet, MotivationPresentationCreate, MotivationPresentationUpdate, BaseDocQueryParams>
    {
    }
}