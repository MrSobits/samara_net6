namespace Bars.GkhDi.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhDi.Entities;

    public class WorkToInterceptor : EmptyDomainInterceptor<WorkTo>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<WorkTo> service, WorkTo entity)
        {
            if (Container.Resolve<IDomainService<WorkRepairTechServ>>().GetAll().Any(x => x.WorkTo.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Работы по ТО услуги ремонт;");
            }

            return Success();
        }
    }
}
