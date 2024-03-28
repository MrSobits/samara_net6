namespace Bars.Gkh.Overhaul.Nso.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Entities;

    public class CreditOrganizationDecisionInterceptor : EmptyDomainInterceptor<CreditOrganizationDecision>
    {
        public override IDataResult BeforeUpdateAction(IDomainService<CreditOrganizationDecision> service, CreditOrganizationDecision entity)
        {
            var anotherAvtualDecision = service.GetAll()
                .Where(x => x.RealityObject.Id == entity.RealityObject.Id)
                .Where(x => x.PropertyOwnerDecisionType == entity.PropertyOwnerDecisionType)
                .Where(x => x.Id != entity.Id)
                .FirstOrDefault(x => x.DateEnd == null);

            if (anotherAvtualDecision != null && entity.DateStart.HasValue)
            {
                anotherAvtualDecision.DateEnd = entity.DateStart.Value.AddDays(-1);
                service.Save(anotherAvtualDecision);
            }

            return this.Success();
        }
    }
}
