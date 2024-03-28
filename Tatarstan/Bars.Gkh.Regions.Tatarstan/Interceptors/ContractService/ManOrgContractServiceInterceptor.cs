namespace Bars.Gkh.Regions.Tatarstan.Interceptors
{
    using Bars.B4;
    using Bars.Gkh.Regions.Tatarstan.Entities.ContractService;

    public class ManOrgContractServiceInterceptor : ManOrgContractServiceInterceptor<ManOrgContractService>
    {
    }

    public class ManOrgContractServiceInterceptor<T> : EmptyDomainInterceptor<T>
        where T : ManOrgContractService
    {
        public override IDataResult BeforeCreateAction(IDomainService<T> service, T entity)
        {
            entity.ServiceType = entity.Service.ServiceType;

            return this.Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<T> service, T entity)
        {
            if (entity.ServiceType != entity.Service.ServiceType)
            {
                return this.Failure("Неправильно задан тип услуги");
            }

            return this.Success();
        }
    }
}