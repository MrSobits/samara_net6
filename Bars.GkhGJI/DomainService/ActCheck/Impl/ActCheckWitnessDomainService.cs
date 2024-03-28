namespace Bars.GkhGji.DomainService
{
    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class ActCheckWitnessDomainService : ActCheckWitnessDomainService<ActCheckWitness>
    {
    }

    //делаю чтобы в модулях регионов расширять сущность ActCheckWitness 
    public class ActCheckWitnessDomainService<T> : BaseDomainService<T>
        where T : ActCheckWitness
    {

    }
}
