namespace Bars.GkhDi.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhDi.Entities;

    public class SupervisoryOrgInterceptor : EmptyDomainInterceptor<SupervisoryOrg>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<SupervisoryOrg> service, SupervisoryOrg entity)
        {
            if (Container.Resolve<IDomainService<AdminResp>>().GetAll().Any(x => x.SupervisoryOrg.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Административная ответственность;");
            }

            return Success();
        }
    }
}
