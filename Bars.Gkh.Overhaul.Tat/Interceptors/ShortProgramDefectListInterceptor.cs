namespace Bars.Gkh.Overhaul.Tat.Interceptors
{
    using B4;
    using B4.Modules.States;

    using Bars.Gkh.Overhaul.Tat.Entities;

    public class ShortProgramDefectListInterceptor : EmptyDomainInterceptor<ShortProgramDefectList>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ShortProgramDefectList> service, ShortProgramDefectList entity)
        {
            var stateProvider = Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            return Success();
        }
    }
}