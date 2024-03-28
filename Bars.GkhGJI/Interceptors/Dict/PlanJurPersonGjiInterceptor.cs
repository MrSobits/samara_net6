namespace Bars.GkhGji.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class PlanJurPersonGjiInterceptor : EmptyDomainInterceptor<PlanJurPersonGji>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<PlanJurPersonGji> service, PlanJurPersonGji entity)
        {
            if (Container.Resolve<IDomainService<BaseJurPerson>>().GetAll().Any(x => x.Plan.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах:  Плановые проверки юр. лиц;");
            }

            return this.Success();
        }
    }
}