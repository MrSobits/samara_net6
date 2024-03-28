namespace Bars.Gkh.Overhaul.Interceptors
{
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;

    using Bars.B4.IoC;

    using Entities;
    using Gkh.Entities;

    public class MunicipalityInterceptor : EmptyDomainInterceptor<Municipality>
    {
        public override IDataResult BeforeCreateAction(IDomainService<Municipality> service, Municipality entity)
        {
            return CheckMunicipality(entity, ServiceOperationType.Save);
        }
        
        public override IDataResult AfterCreateAction(IDomainService<Municipality> service, Municipality entity)
        {
            var paysizeDomain = Container.ResolveDomain<Paysize>();
            var paysizeRecordDomain = Container.ResolveDomain<PaysizeRecord>();

            using(Container.Using(paysizeDomain, paysizeRecordDomain))
            {
                var paysizeIds = paysizeDomain.GetAll()
                .Select(x => x.Id);

                foreach (var paysizeId in paysizeIds)
                {
                    paysizeRecordDomain.Save(new PaysizeRecord
                    {
                        Municipality = entity,
                        Paysize = paysizeDomain.Load(paysizeId)
                    });
                }
            }

            return Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<Municipality> service, Municipality entity)
        {
            return CheckMunicipality(entity, ServiceOperationType.Update);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<Municipality> service, Municipality entity)
        {
            var paysizeRecordDomain = Container.ResolveDomain<PaysizeRecord>();
            var paysizeRecRetDomain = Container.ResolveDomain<PaysizeRealEstateType>();

            using(Container.Using(paysizeRecordDomain, paysizeRecRetDomain))
            {
                paysizeRecordDomain.GetAll()
                .Where(x => x.Municipality.Id == entity.Id)
                .Select(x => x.Id)
                .ForEach(id =>
                {
                    paysizeRecRetDomain.GetAll()
                        .Where(x => x.Record.Id == id)
                        .Select(x => x.Id)
                        .ForEach(x => paysizeRecRetDomain.Delete(x));

                    paysizeRecordDomain.Delete(id);
                });
            }

            return Success();
        }

        private IDataResult CheckMunicipality(Municipality entity, ServiceOperationType operationType)
        {
            if (entity.Name.Length > 300)
            {
                return Failure("Количество знаков в поле Наименование не должно превышать 300 символов");
            }

            return Success();
        }
    }
}