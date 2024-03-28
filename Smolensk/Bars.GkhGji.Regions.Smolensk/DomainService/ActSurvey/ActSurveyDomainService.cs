using Bars.Gkh.DomainService;
using Bars.GkhGji.Regions.Smolensk.Entities;

namespace Bars.GkhGji.Regions.Smolensk.DomainService
{
    /// <summary>
    /// Данный класс служит заменой домен сервиса IDomainService<ActSurvey> на IDomainService<ActSurveySmol>
    /// </summary>
    public class ActSurveySmolDomainService : ReplacementDomainService<Bars.GkhGji.Entities.ActSurvey, ActSurveySmol>
    {
    }
}
