namespace Bars.Gkh.Overhaul.Interceptors
{
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Entities;
    using Gkh.Entities;

    public class PaysizeInterceptor : EmptyDomainInterceptor<Paysize>
    {
        public override IDataResult BeforeCreateAction(IDomainService<Paysize> service, Paysize entity)
        {
            if (!CheckPeriod(service, entity))
            {
                return new BaseDataResult(false, "Период пересекается с уже существующим");
            }

            return Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<Paysize> service, Paysize entity)
        {
            if (!CheckPeriod(service, entity))
            {
                return new BaseDataResult(false, "Период пересекается с уже существующим");
            }

            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<Paysize> service, Paysize entity)
        {
            var retDomain = Container.ResolveDomain<PaysizeRealEstateType>();
            var recordDomain = Container.ResolveDomain<PaysizeRecord>();

            retDomain.GetAll()
                .Where(x => x.Record.Paysize.Id == entity.Id)
                .Select(x => x.Id)
                .ForEach(x => retDomain.Delete(x));

            recordDomain.GetAll()
                .Where(x => x.Paysize.Id == entity.Id)
                .Select(x => x.Id)
                .ForEach(x => recordDomain.Delete(x));

            return Success();
        }

        public override IDataResult AfterCreateAction(IDomainService<Paysize> service, Paysize entity)
        {
            var muDomain = Container.ResolveRepository<Municipality>();
            var recordDomain = Container.ResolveDomain<PaysizeRecord>();

            try
            {
                var muIds = muDomain.GetAll().Select(x => x.Id).ToList();

                foreach (var muId in muIds)
                {
                    recordDomain.Save(new PaysizeRecord
                    {
                        Paysize = entity,
                        Municipality = new Municipality { Id = muId }
                    });
                }

                return Success();
            }
            finally
            {
                Container.Release(muDomain);
                Container.Release(recordDomain);
            }
        }

        private bool CheckPeriod(IDomainService<Paysize> service, Paysize entity)
        {
            return !service.GetAll()
                .WhereIf(entity.DateEnd.HasValue, x => x.DateStart <= entity.DateEnd)
                .Where(x => !x.DateEnd.HasValue || x.DateEnd >= entity.DateStart)
                .Any(x => x.Id != entity.Id);
        }
    }
}
