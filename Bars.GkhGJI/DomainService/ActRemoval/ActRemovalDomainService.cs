namespace Bars.GkhGji.DomainService
{
    using Bars.B4;
    using Bars.GkhGji.Entities;

    // такую пустышку делаю чтобы в регионах заменять , но для этог онад очтобы именно она была зарегистрирована в основном модуле
    public class ActRemovalDomainService : ActRemovalDomainService<ActRemoval>
    {
        
    }

    // Такую фигню делаю чтобы в модулях регионов расширять сущность ActSurvey 
    public class ActRemovalDomainService<T> : BaseDomainService<T>
        where T : ActRemoval
    {
        
    }
}