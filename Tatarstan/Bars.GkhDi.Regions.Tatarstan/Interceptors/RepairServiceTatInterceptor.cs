namespace Bars.GkhDi.Regions.Tatarstan.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Enums;

    public class RepairServiceTatInterceptor : EmptyDomainInterceptor<RepairService>
    {
        public override IDataResult BeforeCreateAction(IDomainService<RepairService> service, RepairService entity)
        {
            var serviceHasBeenAdded = Container.Resolve<IDomainService<BaseService>>().GetAll()
                .Any(x => x.DisclosureInfoRealityObj.Id == entity.DisclosureInfoRealityObj.Id && x.TemplateService.Id == entity.TemplateService.Id);

            if (serviceHasBeenAdded)
            {
                return Failure("Данная услуга уже добавлена");
            }

            return Success();
        }
        
        public override IDataResult BeforeUpdateAction(IDomainService<RepairService> service, RepairService entity)
        {
            if (entity.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceProvidedMo && entity.Provider != null)
            {
                var haveActiveProviderService = Container.Resolve<IDomainService<ProviderService>>().GetAll()
                    .Any(x => x.BaseService.Id == entity.Id && x.IsActive);

                if (!haveActiveProviderService)
                {
                    return Failure("Отсутствуют активные поставщики");
                }
            }
            
            return Success();
        }
    }
}
