namespace Bars.GkhGji.DomainService
{
    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class ActCheckViolationDomainService : ActCheckViolationDomainService<ActCheckViolation>
    {
    }


    //делаю чтобы в модулях регионов расширять сущность ActCheckWitness 
    public class ActCheckViolationDomainService<T> : BaseDomainService<T>
        where T : ActCheckViolation
    {

    }
}
