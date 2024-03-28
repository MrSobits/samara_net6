namespace Bars.GkhCr.Interceptors
{
    using B4;
    using B4.Modules.States;
    using Entities;

    public class DefectListServiceInterceptor : EmptyDomainInterceptor<DefectList>
    {
        public override IDataResult BeforeCreateAction(IDomainService<DefectList> service, DefectList entity)
        {
            var stateProvider = Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            if (entity.TypeWork != null)
            {
                entity.Work = entity.TypeWork.Work;
            }

            return Success();
        }
    }
}