namespace Bars.GkhDi.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhDi.Entities;

    public class WorkRepairDetailInterceptor : EmptyDomainInterceptor<WorkRepairDetail>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<WorkRepairDetail> service, WorkRepairDetail entity)
        {
            if (Container.Resolve<IDomainService<WorkRepairDetail>>().GetAll().Any(x => x.WorkPpr.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Детализация ППР;");
            }

            return Success();
        }
    }
}
