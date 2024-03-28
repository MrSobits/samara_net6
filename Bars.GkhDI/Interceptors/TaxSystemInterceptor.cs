namespace Bars.GkhDi.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhDi.Entities;

    public class TaxSystemInterceptor : EmptyDomainInterceptor<TaxSystem>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<TaxSystem> service, TaxSystem entity)
        {
            if (Container.Resolve<IDomainService<FinActivity>>().GetAll().Any(x => x.TaxSystem.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Финансовая деятельность;");
            }

            return Success();
        }
    }
}
