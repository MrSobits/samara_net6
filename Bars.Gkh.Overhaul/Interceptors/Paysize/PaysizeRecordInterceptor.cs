namespace Bars.Gkh.Overhaul.Interceptors
{
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using Entities;
    using Gkh.Entities.Dicts;

    public class PaysizeRecordInterceptor : EmptyDomainInterceptor<PaysizeRecord>
    {
        public override IDataResult AfterCreateAction(IDomainService<PaysizeRecord> service, PaysizeRecord entity)
        {
            var retDomain = Container.ResolveRepository<RealEstateType>();
            var recordDomain = Container.ResolveDomain<PaysizeRealEstateType>();

            try
            {
                var retIds = retDomain.GetAll().Select(x => x.Id).ToList();

                foreach (var retId in retIds)
                {
                    recordDomain.Save(new PaysizeRealEstateType
                    {
                        Record = entity,
                        RealEstateType = new RealEstateType {Id = retId}
                    });
                }

                return Success();
            }
            finally
            {
                Container.Release(retDomain);
                Container.Release(recordDomain);
            }
        }
    }
}