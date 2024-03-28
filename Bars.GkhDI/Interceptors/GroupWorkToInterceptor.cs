namespace Bars.GkhDi.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhDi.Entities;

    public class GroupWorkToInterceptor : EmptyDomainInterceptor<GroupWorkTo>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<GroupWorkTo> service, GroupWorkTo entity)
        {
            if (Container.Resolve<IDomainService<WorkTo>>().GetAll().Any(x => x.GroupWorkTo.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Работы по ТО;");
            }

            return Success();
        }
    }
}
