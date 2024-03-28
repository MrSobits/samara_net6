namespace Bars.Gkh.Overhaul.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Entities;
    using Gkh.Entities.CommonEstateObject;

    public class CommonEstateObjectInterceptor : EmptyDomainInterceptor<CommonEstateObject>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<CommonEstateObject> service, CommonEstateObject entity)
        {
            var seGroupService = Container.Resolve<IDomainService<StructuralElementGroup>>();

            seGroupService.GetAll()
                .Where(x => x.CommonEstateObject.Id == entity.Id)
                .Select(x => x.Id)
                .ForEach(x => seGroupService.Delete(x));

            var roMissingCeoService = Container.Resolve<IDomainService<RealityObjectMissingCeo>>();

            roMissingCeoService.GetAll()
                .Where(x => x.MissingCommonEstateObject.Id == entity.Id)
                .Select(x => x.Id)
                .ForEach(x => roMissingCeoService.Delete(x));

            return Success();
        }
    }
}