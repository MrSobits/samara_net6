namespace Bars.Gkh.RegOperator.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.RegOperator.Entities;

    public class RealityObjectSupplierAccountInterceptor : EmptyDomainInterceptor<RealityObjectSupplierAccount>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<RealityObjectSupplierAccount> service, RealityObjectSupplierAccount entity)
        {
            var realityObjectSupplierAccountOperationDomain = Container.Resolve<IDomainService<RealityObjectSupplierAccountOperation>>();

            try
            {
                if (realityObjectSupplierAccountOperationDomain.GetAll().Any(x => x.Account == entity))
                {
                    return Failure("Существуют зависимые записи: Операции счета по рассчету с поставщиками");
                }
            }
            finally
            {
                Container.Release(realityObjectSupplierAccountOperationDomain);
            }

            return this.Success();
        }
    }
}