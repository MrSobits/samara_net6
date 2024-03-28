namespace Bars.GkhGji.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class ActivityTsjProtocolInterceptor : EmptyDomainInterceptor<ActivityTsjProtocol>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<ActivityTsjProtocol> service, ActivityTsjProtocol entity)
        {
            if (Container.Resolve<IDomainService<ActivityTsjProtocolRealObj>>().GetAll().Any(x => x.ActivityTsjProtocol.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Дома протокола деятельности ТСЖ;");
            }

            return Success();
        }
    }
}