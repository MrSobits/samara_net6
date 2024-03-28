using Bars.B4.IoC;
using Bars.Gkh1468.Entities.Passport;

namespace Bars.Gkh1468.Interceptors.Passport
{
    using System.Linq;

    using B4;
    using Entities;

    public class HouseProviderPassportInterceptor : EmptyDomainInterceptor<HouseProviderPassport>
    {
        public IDomainService<HouseProviderPassportRow> rowDomain { get; set; }

        public override IDataResult BeforeUpdateAction(IDomainService<HouseProviderPassport> service, HouseProviderPassport entity)
        {
            ReCalculatePasportPercent(service, entity);
            return this.Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<HouseProviderPassport> service, HouseProviderPassport entity)
        {
            rowDomain.GetAll().Where(x => x.ProviderPassport.Id == entity.Id).Select(x => x.Id).ToList().ForEach(x => rowDomain.Delete(x));

            return base.BeforeDeleteAction(service, entity);
        }

        public override IDataResult AfterDeleteAction(IDomainService<HouseProviderPassport> service, HouseProviderPassport entity)
        {
            ReCalculatePasportPercent(service, entity);

            if (!service.GetAll().Any(x => x.HousePassport.Id == entity.HousePassport.Id && x.Id != entity.Id))
            {
                Container.UsingForResolved<IDomainService<HousePassport>>((ct, domain) => domain.Delete(entity.HousePassport.Id));
            }

            return base.AfterDeleteAction(service, entity);
        }

        private void ReCalculatePasportPercent(IDomainService<HouseProviderPassport> service,
            HouseProviderPassport entity)
        {
            var providerFillCount = service.GetAll()
                .Count(x => x.HousePassport.Id == entity.HousePassport.Id && x.Percent == 100);
            var providerCount = service.GetAll()
                .Count(x => x.HousePassport.Id == entity.HousePassport.Id);

            entity.HousePassport.Percent = providerCount != 0 ? ((decimal)providerFillCount * 100 / providerCount) : 0; 
        }
    }
}