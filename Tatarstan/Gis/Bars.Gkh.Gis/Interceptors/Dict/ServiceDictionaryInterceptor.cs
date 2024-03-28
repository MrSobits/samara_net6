namespace Bars.Gkh.Gis.Interceptors.Dict
{
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;

    using Bars.Gkh.Entities.Dicts;

    using Entities.Dict;
    using Entities.Kp50;
    using Entities.Register.HouseServiceRegister;

    public class ServiceDictionaryInterceptor : EmptyDomainInterceptor<ServiceDictionary>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ServiceDictionary> service, ServiceDictionary entity)
        {
            if (service.GetAll().Any(x => x.Code == entity.Code && x.Id != entity.Id))
            {
                return Failure("Услуга с таким кодом уже существует");
            }

            return Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<ServiceDictionary> service, ServiceDictionary entity)
        {
            if (service.GetAll().Any(x => x.Code == entity.Code && x.Id != entity.Id))
            {
                return Failure("Услуга с таким кодом уже существует");
            }

            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<ServiceDictionary> service, ServiceDictionary entity)
        {
            var normativDictDomain = Container.ResolveDomain<GisNormativDict>();
            var tarifDictDomain = Container.ResolveDomain<GisTariffDict>();
            var houseServiceRegisterDomain = Container.ResolveDomain<HouseServiceRegister>();
            var bilServiceDictDomain = Container.ResolveDomain<BilServiceDictionary>();

            try
            {
                normativDictDomain.GetAll()
                    .Where(x => x.Service.Id == entity.Id)
                    .Select(x => x.Id)
                    .ForEach(x => normativDictDomain.Delete(x));

                tarifDictDomain.GetAll()
                    .Where(x => x.Service.Id == entity.Id)
                    .Select(x => x.Id)
                    .ForEach(x => tarifDictDomain.Delete(x));

                houseServiceRegisterDomain.GetAll()
                    .Where(x => x.Service.Id == entity.Id)
                    .Select(x => x.Id)
                    .ForEach(x => houseServiceRegisterDomain.Delete(x));

                var bilServiceList = bilServiceDictDomain.GetAll()
                    .Where(x => x.Service.Id == entity.Id)
                    .ToList();

                foreach (var bilService in bilServiceList)
                {
                    bilService.Service = null;
                    bilServiceDictDomain.Update(bilService);
                }

                return Success();

            }
            finally
            {
                Container.Release(normativDictDomain);
                Container.Release(tarifDictDomain);
                Container.Release(houseServiceRegisterDomain);
                Container.Release(bilServiceDictDomain);
            }
        }
    }
}