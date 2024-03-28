namespace Bars.GkhCr.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhCr.Entities;

    public class BankStatementServiceInterceptor : EmptyDomainInterceptor<BankStatement>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<BankStatement> service, BankStatement entity)
        {
            if (Container.Resolve<IDomainService<BasePaymentOrder>>().GetAll().Any(x => x.BankStatement.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Платежные поручения;");
            }

            return Success();
        }
    }
}