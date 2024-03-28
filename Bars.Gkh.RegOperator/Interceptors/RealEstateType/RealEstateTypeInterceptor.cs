namespace Bars.Gkh.RegOperator.Interceptors
{
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using Entities;
    using Gkh.Entities.Dicts;
    using Overhaul.Entities;

    public class RealEstateTypeInterceptor : EmptyDomainInterceptor<RealEstateType>
    {
        public override IDataResult AfterCreateAction(IDomainService<RealEstateType> service, RealEstateType entity)
        {
            var payrecDomain = Container.ResolveDomain<PaysizeRecord>();
            var payRetDomain = Container.ResolveDomain<PaysizeRealEstateType>();

            foreach (var recId in payrecDomain.GetAll().Select(x => x.Id))
            {
                payRetDomain.Save(new PaysizeRealEstateType
                {
                    RealEstateType = entity,
                    Record = payrecDomain.Load(recId)
                });
            }

            return Success();
        }
    }
}
