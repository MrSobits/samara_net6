namespace Bars.GkhGji.Regions.Zabaykalye.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4.Modules.States;
    using Bars.Gkh.Authentification;

    using Entities;
    using B4;

    using Bars.GkhGji.Regions.Zabaykalye.Entities.AppealCits;

    public class AppealCitsExecutantInterceptor : EmptyDomainInterceptor<AppealCitsExecutant>
    {
        public override IDataResult BeforeCreateAction(IDomainService<AppealCitsExecutant> service, AppealCitsExecutant entity)
        {
            var userManager = Container.Resolve<IGkhUserManager>();
            var stateProvider = Container.Resolve<IStateProvider>();
            try
            {
                var userOperator = userManager.GetActiveOperator();

                if (userOperator != null)
                {
                    if (userOperator.Inspector != null)
                    {
                        entity.Author = userOperator.Inspector;
                    }
                    else
                    {
                        return Failure("Не указан инспектор текущего оператора!");
                    }

                }
                else
                {
                    // Для администратора ничего не делаем
                }

                if (entity.State == null)
                {
                    stateProvider.SetDefaultState(entity);
                    if (entity.State == null)
                    {
                        return Failure("Не задан начальный статус для Исполнителя обращения");
                    }
                }

                return Success();
            }
            finally
            {
                Container.Release(userManager);
                Container.Release(stateProvider);
            }

        }

        public override IDataResult BeforeUpdateAction(IDomainService<AppealCitsExecutant> service, AppealCitsExecutant entity)
        {
            if (entity.IsResponsible
                && service.GetAll().Where(x => x.AppealCits.Id == entity.AppealCits.Id && x.Id != entity.Id).Any(x => x.IsResponsible))
            {
                return Failure("У данного обращения уже имеется ответственный исполнитель");
            }

            return Success();
        }
    }
}
