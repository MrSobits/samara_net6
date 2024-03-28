namespace Bars.GkhGji.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class KindStatementGjiInterceptor : EmptyDomainInterceptor<KindStatementGji>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<KindStatementGji> service, KindStatementGji entity)
        {
            if (Container.Resolve<IDomainService<AppealCits>>().GetAll().Any(x => x.KindStatement.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Реестр обращений;");
            }

            return this.Success();
        }
    }
}