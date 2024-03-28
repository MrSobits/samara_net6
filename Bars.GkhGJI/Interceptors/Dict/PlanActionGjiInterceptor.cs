namespace Bars.GkhGji.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class PlanActionGjiInterceptor : EmptyDomainInterceptor<PlanActionGji>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<PlanActionGji> service, PlanActionGji entity)
        {
            if (Container.Resolve<IDomainService<BasePlanAction>>().GetAll().Any(x => x.Plan.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Проверка по плану мероприятия;");
            }

            return this.Success();
        }

        public override IDataResult BeforeCreateAction(IDomainService<PlanActionGji> service, PlanActionGji entity)
        {
            return CheckFields(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<PlanActionGji> service, PlanActionGji entity)
        {
            return CheckFields(entity);
        }

        private IDataResult CheckFields(PlanActionGji entity)
        {
            if (entity.Name.IsEmpty())
            {
                return Failure("Не заполнены обязательные поля: Наименование");
            }

            if (!entity.DateStart.HasValue)
            {
                return Failure("Не заполнены обязательные поля: Дата начала");
            }

            if (!entity.DateEnd.HasValue)
            {
                return Failure("Не заполнены обязательные поля: Дата окончания");
            }

            return Success();
        }
    }
}