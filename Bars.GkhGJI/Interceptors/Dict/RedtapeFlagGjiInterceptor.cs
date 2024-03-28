namespace Bars.GkhGji.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class RedtapeFlagGjiInterceptor : EmptyDomainInterceptor<RedtapeFlagGji>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<RedtapeFlagGji> service, RedtapeFlagGji entity)
        {
            if (Container.Resolve<IDomainService<AppealCits>>().GetAll().Any(x => x.RedtapeFlag.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Реестр обращений;");
            }

            return this.Success();
        }
    }
}