namespace Bars.GkhDi.Interceptors
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhDi.Entities;

    public class ProviderServiceInterceptor : EmptyDomainInterceptor<ProviderService>
    {
        public override IDataResult AfterDeleteAction(IDomainService<ProviderService> service, ProviderService entity)
        {
            return ChangeServiceProvider(service, entity);
        }

        public override IDataResult AfterCreateAction(IDomainService<ProviderService> service, ProviderService entity)
        {
            return ChangeServiceProvider(service, entity);
        }

        public override IDataResult AfterUpdateAction(IDomainService<ProviderService> service, ProviderService entity)
        {
            return ChangeServiceProvider(service, entity);
        }

        public override IDataResult BeforeCreateAction(IDomainService<ProviderService> service, ProviderService entity)
        {
            if (entity.BaseService.IsNull())
                return Failure("Необходимо сохранить базовую услугу.");

            return base.BeforeCreateAction(service, entity);
        }

        private IDataResult ChangeServiceProvider(IDomainService<ProviderService> service, ProviderService entity)
        {
            var baseService = this.Container.Resolve<IDomainService<BaseService>>();

            var providerMaxDateStartContract = service.GetAll()
                .Where(x => x.BaseService.Id == entity.BaseService.Id)
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.DateStartContract)
                .Select(x => x.Provider)
                .FirstOrDefault();

            if (providerMaxDateStartContract.IsNotNull())
            {
                var baseServiceEntity = baseService.GetAll()
                    .FirstOrDefault(x => x.Id == entity.BaseService.Id);

                if (baseServiceEntity.IsNotNull())
                {
                    baseServiceEntity.Provider = providerMaxDateStartContract;
                    baseService.Update(baseServiceEntity);
                }
            }

            return Success();
        }
    }
}
