namespace Bars.GkhGji.Regions.BaseChelyabinsk.DomainService.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Repositories;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.ConfigSections;
    using Bars.GkhGji.Contracts.Enums;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;
    using Entities.Reminder;
    public class ChelyabinskReminderService : IExtReminderService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult ListAppealCitsReminder(BaseParams baseParams)
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var servInsSub = this.Container.Resolve<IDomainService<InspectorSubscription>>();
            var servReminder = this.Container.Resolve<IDomainService<ChelyabinskReminder>>();

            try
            {
                var loadParams = baseParams.GetLoadParam();
                var activeOperator = userManager.GetActiveOperator();

                if (activeOperator?.Inspector == null)
                {
                    return new ListDataResult(null, 0);
                }

                var answersInWork = this.GetInWorkAnswerQuery();

                var data = servReminder.GetAll()
                    .Where(x => x.Actuality && x.TypeReminder == TypeReminder.Statement)
                    .Where(x => x.AppealCitsExecutant != null)
                    .Where(
                        x => servInsSub.GetAll()
                            .Any(y => y.SignedInspector.Id == activeOperator.Inspector.Id)
                            ? servInsSub.GetAll()
                                .Any(
                                    y => y.SignedInspector.Id == activeOperator.Inspector.Id
                                        && ((y.Inspector.Id == x.AppealCitsExecutant.Executant.Id || y.Inspector.Id == x.AppealCitsExecutant.Author.Id
                                            || y.Inspector.Id == x.AppealCitsExecutant.Controller.Id))
                                        || y.SignedInspector.Id == x.AppealCitsExecutant.Executant.Id
                                        || y.SignedInspector.Id == x.AppealCitsExecutant.Author.Id
                                        || y.SignedInspector.Id == x.AppealCitsExecutant.Controller.Id)
                            : x.AppealCitsExecutant.Executant.Id == activeOperator.Inspector.Id
                                || x.AppealCitsExecutant.Author.Id == activeOperator.Inspector.Id
                                || x.AppealCitsExecutant.Controller.Id == activeOperator.Inspector.Id
                    )

                    // Вообщем если у инспектора есть Подписки на других инспеткоров то показываем тех инспекторов на которых он подписал, Иначе показывам тольк опо своему Инспектору
                    .Select(
                        x => new
                        {
                            x.Id,
                            AppealCits = x.AppealCits != null ? x.AppealCits.Id : 0,
                            x.Num,
                            x.CheckDate,
                            Guarantor = x.AppealCitsExecutant.Author != null ? x.AppealCitsExecutant.Author.Fio : null,
                            Inspector = x.AppealCitsExecutant.Executant != null ? x.AppealCitsExecutant.Executant.Fio : null,
                            CheckingInspector = x.AppealCitsExecutant.Controller != null ? x.AppealCitsExecutant.Controller.Fio : null,
                            AppealCorr = x.AppealCits != null ? x.AppealCits.Correspondent : null,
                            AppealCorrAddress = x.AppealCits != null ? x.AppealCits.CorrespondentAddress : null,
                            AppealDescription = x.AppealCits != null ? x.AppealCits.Description : null,
                            x.AppealCitsExecutant.State,
                            AppealCitsExecutant = x.AppealCitsExecutant.Id,
                            x.AppealCits.DateFrom,
                            HasAppealCitizensInWorkState = answersInWork
                                                            .Where(y => y.AppealCits == x.AppealCits)
                                                            .Any(y => y.Executor.Id == x.AppealCitsExecutant.Controller.Id
                                                                || y.Executor.Id == x.AppealCitsExecutant.Executant.Id)
                        })
                    .OrderIf(loadParams.Order.Length == 0, true, x => x.CheckDate)
                    .Filter(loadParams, this.Container);

                var totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
            finally
            {
                this.Container.Release(userManager);
                this.Container.Release(servInsSub);
                this.Container.Release(servReminder);
            }
        }

        /// <summary>
        /// Вернуть список ответов в работе
        /// </summary>
        /// <returns>Подзапрос</returns>
        protected IQueryable<AppealCitsAnswer> GetInWorkAnswerQuery()
        {
            var appealCits = this.Container.ResolveDomain<AppealCitsAnswer>();
            var stateRepo = this.Container.Resolve<IStateRepository>();
            var config = this.Container.GetGkhConfig<HousingInspection>();

            using (this.Container.Using(appealCits, stateRepo, config))
            {
                var availableStates = stateRepo.GetAllStates<AppealCitsAnswer>()
                    .Where(x => x.Name.In("В работе", "Готов ответ"))
                    .Select(x => x.Id)
                    .ToArray();

                return appealCits.GetAll()
                    .Where(x => !config.GeneralConfig.ShowStatementsWithAnswerAsOverdue)
                    .Where(x => availableStates.Contains(x.State.Id));
            }
        }
    }
}