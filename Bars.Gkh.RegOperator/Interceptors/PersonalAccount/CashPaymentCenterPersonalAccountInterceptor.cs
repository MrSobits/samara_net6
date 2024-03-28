namespace Bars.Gkh.RegOperator.Interceptors
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Интерцептор для удаления связи с РКЦ
    /// </summary>
    class CashPaymentCenterPersonalAccountInterceptor : EmptyDomainInterceptor<BasePersonalAccount>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<BasePersonalAccount> service, BasePersonalAccount entity)
        {            
            Container.UsingForResolved<IDomainService<CashPaymentCenterPersAcc>>(
                (container, domain) => domain.GetAll()
                    .Where(x => x.PersonalAccount.Id == entity.Id)
                    .Select(x => x.Id)
                    .ForEach(x => domain.Delete(x))
                );

            return Success();
        }
    }
}
