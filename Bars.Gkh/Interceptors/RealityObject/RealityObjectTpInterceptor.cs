namespace Bars.Gkh.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public class RealityObjectTpInterceptor : EmptyDomainInterceptor<RealityObject>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<RealityObject> service, RealityObject entity)
        {
            var tehPsValueService = Container.Resolve<IDomainService<TehPassportValue>>();
            var tehPsService = Container.Resolve<IDomainService<TehPassport>>();

            var values = tehPsValueService.GetAll().Where(x => x.TehPassport.RealityObject.Id == entity.Id).ToArray();

            var tps = tehPsService.GetAll().Where(x => x.RealityObject.Id == entity.Id).ToArray();

            foreach (var val in values)
            {
                tehPsValueService.Delete(val.Id);
            }

            foreach (var val in tps)
            {
                tehPsService.Delete(val.Id);
            }

            return Success();
        }    
    }
}
