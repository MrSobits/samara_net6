namespace Bars.Gkh.Decisions.Nso.Interceptors
{
    using System.Linq;

    using B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;

    using Entities;

    using Castle.Core.Internal;

    public class RealityObjectDecisionProtocolInterceptor : EmptyDomainInterceptor<RealityObjectDecisionProtocol>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<RealityObjectDecisionProtocol> service, RealityObjectDecisionProtocol entity)
        {
            var ultimateDecisionService = Container.Resolve<IDomainService<UltimateDecision>>();
            var monthlyFeeDecisionService = Container.Resolve<IDomainService<MonthlyFeeAmountDecHistory>>();
            var decisionNotificationService = Container.Resolve<IDomainService<DecisionNotification>>();
            var genericDecisionService = Container.Resolve<IDomainService<GenericDecision>>();

            using (Container.Using(ultimateDecisionService, monthlyFeeDecisionService, decisionNotificationService, genericDecisionService))
            {
                ultimateDecisionService.GetAll().Where(x => x.Protocol.Id == entity.Id).Select(x => x.Id).ForEach(x => ultimateDecisionService.Delete(x));
                monthlyFeeDecisionService.GetAll().Where(x => x.Protocol.Id == entity.Id).Select(x => x.Id).ForEach(x => monthlyFeeDecisionService.Delete(x));
                decisionNotificationService.GetAll().Where(x => x.Protocol.Id == entity.Id).Select(x => x.Id).ForEach(x => decisionNotificationService.Delete(x));
                genericDecisionService.GetAll().Where(x => x.Protocol.Id == entity.Id).Select(x => x.Id).ForEach(x => genericDecisionService.Delete(x));
            }

            return base.BeforeDeleteAction(service, entity);
        }
    }
}