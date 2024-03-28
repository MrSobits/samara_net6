namespace Bars.GkhGji.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.GkhGji.Entities;

    public class ActivityTsjMemberInterceptor : EmptyDomainInterceptor<ActivityTsjMember>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ActivityTsjMember> service, ActivityTsjMember entity)
        {
            // Перед сохранением присваиваем начальный статус
            var stateProvider = Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            return this.Success();
        }        
    }
}