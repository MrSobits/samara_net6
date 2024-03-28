namespace Bars.Gkh.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;

    public class ResettlementProgramSourceInterceptor : EmptyDomainInterceptor<ResettlementProgramSource>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<ResettlementProgramSource> service, ResettlementProgramSource entity)
        {
            if (Container.Resolve<IDomainService<EmerObjResettlementProgram>>().GetAll().Any(x => x.ResettlementProgramSource.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Программы переселения аварийного дома;");
            }

            return Success();
        }
    }
}
