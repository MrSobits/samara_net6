namespace Bars.Gkh.RegOperator.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.RegOperator.DomainService;
    using Bars.Gkh.RegOperator.Entities;

    public class DecisionNotificationInterceptor : EmptyDomainInterceptor<DecisionNotification>
    {
        public override IDataResult AfterCreateAction(IDomainService<DecisionNotification> service, DecisionNotification entity)
        {
            this.UpdateSpecialAccountInfo(entity);

            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<DecisionNotification> service, DecisionNotification entity)
        {
            this.UpdateSpecialAccountInfo(entity);

            return this.Success();
        }

        private bool AnyNewerProtocolOrNotification(DecisionNotification notification)
        {
            var protocolDomain = this.Container.ResolveDomain<RealityObjectDecisionProtocol>();
            var decNotifDomain = this.Container.ResolveDomain<DecisionNotification>();

            using (this.Container.Using(protocolDomain, decNotifDomain))
            {
                return protocolDomain.GetAll()
                        .Where(x => x.RealityObject == notification.Protocol.RealityObject)
                        .Where(x => x != notification.Protocol)
                        .Where(x => x.State.FinalState)
                        .Any(x => x.ProtocolDate > notification.Protocol.ProtocolDate)
                    && decNotifDomain.GetAll()
                        .Where(x => x.Protocol == notification.Protocol)
                        .Where(x => x != notification)
                        .Any(x => x.Date > notification.Date);
            }
        }

        private void UpdateSpecialAccountInfo(DecisionNotification entity)
        {
            if (!this.AnyNewerProtocolOrNotification(entity))
            {
                var specaccService = this.Container.Resolve<ISpecialCalcAccountService>();
                var specAccDomain = this.Container.ResolveDomain<SpecialCalcAccount>();

                using (this.Container.Using(specaccService, specAccDomain))
                {
                    var specialAccount = specaccService.GetSpecialAccount(entity.Protocol.RealityObject);

                    if (specialAccount != null)
                    {
                        specialAccount.DateOpen = entity.OpenDate;
                        specialAccount.DateClose =
                            entity.CloseDate != DateTime.MinValue
                                ? (DateTime?) entity.CloseDate
                                : null;
                        specialAccount.AccountNumber = entity.AccountNum;

                        specAccDomain.Update(specialAccount);
                    }
                }
            }
        }
    }
}