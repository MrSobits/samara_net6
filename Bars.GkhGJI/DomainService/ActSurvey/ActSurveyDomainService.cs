namespace Bars.GkhGji.DomainService
{
    using Bars.B4;
    using Bars.GkhGji.Entities;

    // такую пустышку делаю чтобы в регионах заменять , но для этог онад очтобы именно она была зарегистрирована в основном модуле
    public class ActSurveyDomainService : ActSurveyDomainService<ActSurvey>
    {
        
    }

    // Такую фигню делаю чтобы в модулях регионов расширять сущность ActSurvey 
    public class ActSurveyDomainService<T> : BaseDomainService<T>
        where T: ActSurvey
    {
        
    }
}