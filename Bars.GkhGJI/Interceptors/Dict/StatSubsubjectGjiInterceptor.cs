namespace Bars.GkhGji.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class StatSubsubjectGjiInterceptor : EmptyDomainInterceptor<StatSubsubjectGji>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<StatSubsubjectGji> service, StatSubsubjectGji entity)
        {
            if (Container.Resolve<IDomainService<AppealCitsStatSubject>>().GetAll().Any(x => x.Subsubject.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Подтематики реестра обращений;");
            }

            return this.Success();
        }
    }
}