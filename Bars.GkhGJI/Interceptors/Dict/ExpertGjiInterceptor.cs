namespace Bars.GkhGji.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class ExpertGjiInterceptor : EmptyDomainInterceptor<ExpertGji>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<ExpertGji> service, ExpertGji entity)
        {
            if (Container.Resolve<IDomainService<DisposalExpert>>().GetAll().Any(x => x.Expert.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Эксперты рапоряжения ГЖИ;");
            }

            return this.Success();
        }
    }
}