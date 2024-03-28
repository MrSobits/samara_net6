namespace Bars.Gkh.InspectorMobile.Api.Version1.Services
{
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Decision;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Decision;

    /// <summary>
    /// Интерфейс сервиса для взаимодействия с <see cref="Decision"/>
    /// </summary>
    public interface IDocumentDecisionService : IDocumentWithParentService<DocumentDecision, object, object, DecisionQueryParams>
    {
    }
}