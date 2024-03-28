namespace Bars.Gkh.Decisions.Nso.Interceptors
{
    using System.Linq;
    using B4;
    using Entities;
    using Domain.Decisions;

    public class GenericDecisionInterceptor : EmptyDomainInterceptor<GenericDecision>
    {
        private readonly IDecisionContainer _decisionContainer;

        public GenericDecisionInterceptor(IDecisionContainer decisionContainer)
        {
            _decisionContainer = decisionContainer;
        }

        public override IDataResult BeforeCreateAction(IDomainService<GenericDecision> service, GenericDecision entity)
        {
            entity.IsActual = true;

            var siblings = _decisionContainer.GetSiblings(entity.DecisionCode).Distinct().ToList();

            service.GetAll().Where(x => siblings.Contains(x.DecisionCode) && x.Protocol.Id == entity.Protocol.Id).ToList().ForEach(
                x =>
                {
                    x.IsActual = false;
                    service.Update(x);
                });

            return base.BeforeCreateAction(service, entity);
        }
    }
}