namespace Bars.GkhGji.DomainService
{
    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class ActCheckRealityObjectDomainService : ActCheckRealityObjectDomainService<ActCheckRealityObject>
    {

    }

    //делаю чтобы в модулях регионов расширять сущность ActCheckRealityObject 
    public class ActCheckRealityObjectDomainService<T> : BaseDomainService<T>
        where T : ActCheckRealityObject
    {

    }
}
