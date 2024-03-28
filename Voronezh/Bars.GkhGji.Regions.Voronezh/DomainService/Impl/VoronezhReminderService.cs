namespace Bars.GkhGji.Regions.Voronezh.DomainService.Impl
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
    using Bars.GkhGji.Regions.BaseChelyabinsk.DomainService;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Reminder;

    using Castle.Windsor;

    public class VoronezhReminderService : IExtReminderService
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
                var activeInspectorId = userManager.GetActiveOperator()?.Inspector.Id ?? 0;

                if (activeInspectorId == 0)
                {
                    return BaseDataResult.Error("Не найден оператор");
                }

                var answersInWork = this.GetInWorkAnswerQuery();

                var listDataResult = servReminder.GetAll()
                     .Where(x => x.Actuality && x.TypeReminder == TypeReminder.Statement)
                     .WhereNotNull(x => x.AppealCitsExecutant)
                     .WhereNotNull(x => x.AppealCitsExecutant.Executant)
                     .WhereNotNull(x => x.AppealCits)
                     .WhereNotNull(x => x.AppealCitsExecutant.State)
                     .Where(x => !(x.AppealCitsExecutant.Executant.Id == activeInspectorId && x.AppealCitsExecutant.State.FinalState))
                     .Where(x => servInsSub.GetAll()
                             .Select(y => y.SignedInspector.Id)
                             .Any(id => id == activeInspectorId)
                             ? servInsSub.GetAll()
                                 .Select(y => new
                                 {
                                     SignedInspectorId = y.SignedInspector.Id,
                                     InspectorId = y.Inspector.Id
                                 })
                                 .Any(y => (y.SignedInspectorId == activeInspectorId
                                         && (y.InspectorId == x.AppealCitsExecutant.Executant.Id
                                             || y.InspectorId == x.AppealCitsExecutant.Author.Id
                                             || y.InspectorId == x.AppealCitsExecutant.Controller.Id))
                                     || activeInspectorId == x.AppealCitsExecutant.Executant.Id
                                     || activeInspectorId == x.AppealCitsExecutant.Author.Id
                                     || activeInspectorId == x.AppealCitsExecutant.Controller.Id)
                             : x.AppealCitsExecutant.Executant.Id == activeInspectorId
                             || x.AppealCitsExecutant.Author.Id == activeInspectorId
                             || x.AppealCitsExecutant.Controller.Id == activeInspectorId
                     )

                     // Вообщем если у инспектора есть Подписки на других инспеткоров то показываем тех инспекторов на которых он подписал, Иначе показывам тольк опо своему Инспектору 
                     .Select(x => new
                     {
                         x.Id,
                         AppealCits = x.AppealCits.Id,
                         NumberGji = x.AppealCits.NumberGji,
                         AppealState = x.AppealCits.State,
                         x.AppealCits.CheckTime,
                         Num = x.AppealCits.DocumentNumber,
                         CheckDate = x.AppealCitsExecutant.PerformanceDate,
                         SoprDate = x.AppealCits.CaseDate,
                         Contragent = x.AppealCits.OrderContragent != null? x.AppealCits.OrderContragent.Name:"",
                         Guarantor = x.AppealCitsExecutant.Author.Fio,
                         Inspector = x.AppealCitsExecutant.Executant.Fio,
                         CheckingInspector = x.AppealCitsExecutant.Controller.Fio,
                         AppealCorr = x.AppealCits.Correspondent,
                         x.AppealCits.StatementSubjects,
                         AppealCorrAddress = x.AppealCits.CorrespondentAddress,
                         AppealDescription = x.AppealCits.Description,
                         x.AppealCitsExecutant.State,
                         x.AppealCits.ExtensTime,
                         AppealCitsExecutant = x.AppealCitsExecutant.Id,
                         x.AppealCits.DateFrom,
                         InspectorId = x.AppealCitsExecutant.Executant.Id,
                         IncomingSources = x.AppealCits.IncomingSourcesName,                         
                         HasAppealCitizensInWorkState = answersInWork
                             .Select(y => new
                             {
                                 AppealCitsId = y.AppealCits.Id,
                                 ExecutorId = y.Executor.Id
                             })
                             .Where(y => y.AppealCitsId == x.AppealCits.Id)
                             .Any(y => y.ExecutorId == x.AppealCitsExecutant.Controller.Id
                                 || y.ExecutorId == x.AppealCitsExecutant.Executant.Id)
                     })
                     .OrderIf(loadParams.Order.Length == 0, true, x => x.CheckDate)
                     .ToListDataResult(loadParams);

                return listDataResult;
            }
            catch (ValidationException exc)
            {
                return BaseDataResult.Error(exc.Message);
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