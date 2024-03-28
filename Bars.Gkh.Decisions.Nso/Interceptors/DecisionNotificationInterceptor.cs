namespace Bars.Gkh.Decisions.Nso.Interceptors
{
    using System.Linq;

    using B4;
    using B4.Modules.States;
    using Entities;

    public class DecisionNotificationInterceptor : EmptyDomainInterceptor<DecisionNotification>
    {
        public override IDataResult BeforeCreateAction(IDomainService<DecisionNotification> service, DecisionNotification entity)
        {
            if (!string.IsNullOrEmpty(entity.AccountNum)
                && service.GetAll().Any(x => x.AccountNum == entity.AccountNum))
            {
                return Failure("Указанный номер счета уже существует!");
            }

            var stateNotificationProvider = Container.Resolve<IStateProvider>();
            stateNotificationProvider.SetDefaultState(entity);
            return base.BeforeCreateAction(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<DecisionNotification> service, DecisionNotification entity)
        {
            if (!string.IsNullOrEmpty(entity.AccountNum)
                && service.GetAll().Any(x => x.AccountNum == entity.AccountNum && x.Id != entity.Id))
            {
                return Failure("Указанный номер счета уже существует!");
            }

            return base.BeforeUpdateAction(service, entity);
        }
    }
}
