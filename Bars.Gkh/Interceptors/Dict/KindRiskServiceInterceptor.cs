namespace Bars.Gkh.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public class KindRiskServiceInterceptor : EmptyDomainInterceptor<KindRisk>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<KindRisk> service, KindRisk entity)
        {
            if (Container.Resolve<IDomainService<BelayPolicyRisk>>().GetAll().Any(x => x.KindRisk.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Застрахованные риски;");
            }

            return Success();
        }
    }
}
