namespace Bars.Gkh.Overhaul.Hmao.Interceptors
{
    using B4;
    using B4.Utils;
    using Entities;
    using Gkh.Entities.CommonEstateObject;
    using System.Linq;

    public class CommonEstateObjectInterceptor : EmptyDomainInterceptor<CommonEstateObject>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<CommonEstateObject> service, CommonEstateObject entity)
        {
            var shareService = Container.Resolve<IDomainService<ShareFinancingCeo>>();

            shareService.GetAll()
                .Where(x => x.CommonEstateObject.Id == entity.Id)
                .Select(x => x.Id)
                .ForEach(x => shareService.Delete(x));

            return Success();
        }
    }
}