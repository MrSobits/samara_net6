namespace Bars.GkhGji.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.GkhGji.Entities;

    public class ActivityTsjStatuteInterceptor : EmptyDomainInterceptor<ActivityTsjStatute>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ActivityTsjStatute> service, ActivityTsjStatute entity)
        {
            // Перед сохранением присваиваем начальный статус
            var stateProvider = Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            return this.Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<ActivityTsjStatute> service, ActivityTsjStatute entity)
        {
            if (Container.Resolve<IDomainService<ActivityTsjArticle>>().GetAll().Any(x => x.ActivityTsjStatute.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Уставы деятельности ТСЖ;");
            }

            return Success();
        }
    }
}