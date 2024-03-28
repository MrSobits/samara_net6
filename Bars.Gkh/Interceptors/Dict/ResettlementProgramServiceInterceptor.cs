namespace Bars.Gkh.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;

    public class ResettlementProgramServiceInterceptor : EmptyDomainInterceptor<ResettlementProgram>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<ResettlementProgram> service, ResettlementProgram entity)
        {
            if (Container.Resolve<IDomainService<EmergencyObject>>().GetAll().Any(x => x.ResettlementProgram.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Реестр аварийных домов;");
            }

            return Success();
        }
    }
}
