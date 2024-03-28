namespace Bars.Gkh.Overhaul.Nso.Interceptors
{
    using Bars.B4;
    using Bars.Gkh.Overhaul.Nso.Entities;

    public class SpecialAccountDecisionInterceptor : EmptyDomainInterceptor<SpecialAccountDecision>
    {
        public override IDataResult BeforeCreateAction(IDomainService<SpecialAccountDecision> service, SpecialAccountDecision entity)
        {

            var serv = this.Container.Resolve<IDomainService<SpecialAccountDecisionNotice>>();
            serv.Save(new SpecialAccountDecisionNotice
                          {
                              SpecialAccountDecision = entity
                          });

            return this.Success();
        }
    }
}