namespace Bars.Gkh.RegOperator.Interceptors
{
    using System.Linq;

    using B4;

    using Bars.Gkh.Repositories.ChargePeriod;

    using Entities;
    using DomainService.RealityObjectAccount;

    /// <summary>
    /// Счет начислений дома
    /// </summary>
    public class RealityObjectChargeAccountInterceptor : EmptyDomainInterceptor<RealityObjectChargeAccount>
    {
        public IRealityObjectAccountGenerator Generator { get; set; }
        public IRealtyObjectRegopOperationService RealtyObjectRegopOperationService { get; set; }
        public IChargePeriodRepository ChargePeriodRepo { get; set; }
            
        public override IDataResult AfterCreateAction(IDomainService<RealityObjectChargeAccount> service, RealityObjectChargeAccount entity)
        {
            var period = this.ChargePeriodRepo.GetCurrentPeriod();

            if (period != null)
            {
                this.RealtyObjectRegopOperationService.CreateRealtyObjectChargeOperations(period, entity.RealityObject);
            }
           
            return base.AfterCreateAction(service, entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<RealityObjectChargeAccount> service, RealityObjectChargeAccount entity)
        {
            var realityObjectChargeAccountOperationDomain = this.Container.Resolve<IDomainService<RealityObjectChargeAccountOperation>>();

            try
            {
                if (realityObjectChargeAccountOperationDomain.GetAll().Count(x => x.Account == entity) > 0)
                {
                    return this.Failure("Существуют зависимые записи: Начисления по счету начисления дома");
                }
            }
            finally
            {
                this.Container.Release(realityObjectChargeAccountOperationDomain);
            }

            return this.Success();
        }
    }
}
