namespace Bars.Gkh.Overhaul.Hmao.Interceptors
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    public class SpecialAccountDecisionInterceptor : EmptyDomainInterceptor<SpecialAccountDecision>
    {
        public override IDataResult BeforeCreateAction(IDomainService<SpecialAccountDecision> service, SpecialAccountDecision entity)
        {
            var serv = Container.Resolve<IDomainService<SpecialAccountDecisionNotice>>();
            serv.Save(new SpecialAccountDecisionNotice
                          {
                              SpecialAccountDecision = entity
                          });

            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<SpecialAccountDecision> service, SpecialAccountDecision entity)
        {
            var serv = Container.Resolve<IDomainService<SpecialAccountDecisionNotice>>();

            var ids = serv.GetAll()
                .Where(x => x.SpecialAccountDecision.Id == entity.Id)
                .Select(x => x.Id)
                .ToArray();

            foreach (var id in ids)
            {
                serv.Delete(id);
            }

            return Success();
        }
    }
}