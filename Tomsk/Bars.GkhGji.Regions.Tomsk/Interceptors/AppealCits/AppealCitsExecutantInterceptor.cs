namespace Bars.GkhGji.Regions.Tomsk.Interceptors.AppealCits
{
	using System.Linq;
	using Bars.B4;
	using Bars.B4.DataAccess;
	using Bars.B4.Modules.States;
	using Bars.B4.Utils;
	using Bars.Gkh.Authentification;
	using Bars.Gkh.Gji.DomainService;
	using Bars.GkhGji.Contracts.Reminder;
	using Bars.GkhGji.Regions.Tomsk.Entities.AppealCits;

	public class AppealCitsExecutantInterceptor : EmptyDomainInterceptor<AppealCitsExecutant>
    {
        public override IDataResult BeforeCreateAction(IDomainService<AppealCitsExecutant> service,
                                                       AppealCitsExecutant entity)
        {
            var userManager = Container.Resolve<IGkhUserManager>();
            var stateProvider = Container.Resolve<IStateProvider>();
            var paramService = Container.Resolve<IGjiParamsService>();
            try
            {
                if (paramService.GetParamByKey("AutoSetSurety").ToBool())
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
                Container.Release(paramService);
            }
        }

        public override IDataResult BeforeUpdateAction(IDomainService<AppealCitsExecutant> service,
                                                       AppealCitsExecutant entity)
        {
            if (entity.IsResponsible
                &&
                service.GetAll()
                       .Where(x => x.AppealCits.Id == entity.AppealCits.Id && x.Id != entity.Id)
                       .Any(x => x.IsResponsible))
            {
                return Failure("У данного обращения уже имеется ответственный исполнитель");
            }

            return Success();
        }

        public override IDataResult AfterCreateAction(IDomainService<AppealCitsExecutant> service,
                                                      AppealCitsExecutant entity)
        {
            var acService = Container.ResolveDomain<GkhGji.Entities.AppealCits>();
            try
            {
                CreateReminders(acService.Get(entity.AppealCits.Id));
            }
            finally
            {
                Container.Release(acService);
            }

            return base.AfterCreateAction(service, entity);
        }

        public override IDataResult AfterUpdateAction(IDomainService<AppealCitsExecutant> service,
                                                      AppealCitsExecutant entity)
        {
            CreateReminders(entity.AppealCits);
            return base.AfterUpdateAction(service, entity);
        }

        public override IDataResult AfterDeleteAction(IDomainService<AppealCitsExecutant> service, AppealCitsExecutant entity)
        {
            CreateReminders(entity.AppealCits);
            return base.AfterDeleteAction(service, entity);
        }

        private void CreateReminders<T>(T entity) where T : GkhGji.Entities.AppealCits
        {
            var servReminderRule = Container.ResolveAll<IReminderRule>();
            try
            {
                var rule = servReminderRule.FirstOrDefault(x => x.Id == "AppealCitsReminderRule");
                if (rule != null)
                {
                    rule.Create(entity);
                }
            }
            finally
            {
                Container.Release(servReminderRule);
            }
        }
    }
}