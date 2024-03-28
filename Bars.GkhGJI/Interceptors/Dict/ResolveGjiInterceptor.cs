namespace Bars.GkhGji.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class ResolveGjiInterceptor : EmptyDomainInterceptor<ResolveGji>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<ResolveGji> service, ResolveGji entity)
        {
            if (Container.Resolve<IDomainService<AppealCits>>().GetAll().Any(x => x.SuretyResolve.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Реестр обращений;");
            }

            return this.Success();
        }
    }
}