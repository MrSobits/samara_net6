namespace Bars.GkhGji.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class InspectionGjiViolInterceptor : EmptyDomainInterceptor<InspectionGjiViol>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<InspectionGjiViol> service, InspectionGjiViol entity)
        {
            if (Container.Resolve<IDomainService<InspectionGjiViolStage>>().GetAll().Any(x => x.InspectionViolation.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Этапы нарушения проверки ГЖИ;");
            }

            return this.Success();
        }
    }
}