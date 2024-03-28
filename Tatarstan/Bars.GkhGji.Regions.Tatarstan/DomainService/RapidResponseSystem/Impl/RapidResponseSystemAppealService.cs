namespace Bars.GkhGji.Regions.Tatarstan.DomainService
{
    using System;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Models;
    using Bars.Gkh.Modules.Gkh1468.Entities;
    using Bars.Gkh.Utils;
    using Bars.GkhCalendar.Entities;
    using Bars.GkhCalendar.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.ConfigSections.Appeal;
    using Bars.GkhGji.Regions.Tatarstan.Dto;
    using Bars.GkhGji.Regions.Tatarstan.Entities.RapidResponseSystem;
    using Bars.GkhGji.Regions.Tatarstan.Services;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для <see cref="RapidResponseSystemAppeal"/>
    /// </summary>
    public class RapidResponseSystemAppealService : IRapidResponseSystemAppealService
    {
        #region Dependency Injection
        private readonly IWindsorContainer container;
        private readonly IGkhUserManager userManager;
        private readonly IDomainService<RapidResponseSystemAppeal> rapidResponseSystemAppealDomain;
        private readonly IDomainService<RapidResponseSystemAppealDetails> rapidResponseSystemAppealDetailsDomain;
        private readonly IRapidResponseSystemAppealMailProvider rapidResponseSystemAppealMailProvider;
        private readonly IDomainService<Day> dayDomain;
        
        public RapidResponseSystemAppealService(
            IWindsorContainer container,
            IGkhUserManager userManager,
            IDomainService<RapidResponseSystemAppeal> rapidResponseSystemAppealDomain,
            IDomainService<RapidResponseSystemAppealDetails> rapidResponseSystemAppealDetailsDomain,
            IRapidResponseSystemAppealMailProvider rapidResponseSystemAppealMailProvider,
            IDomainService<Day> dayDomain)
        {
            this.container = container;
            this.userManager = userManager;
            this.rapidResponseSystemAppealDomain = rapidResponseSystemAppealDomain;
            this.rapidResponseSystemAppealDetailsDomain = rapidResponseSystemAppealDetailsDomain;
            this.rapidResponseSystemAppealMailProvider = rapidResponseSystemAppealMailProvider;
            this.dayDomain = dayDomain;
        }
        #endregion

        /// <inheritdoc />
        public IDataResult ChangeState(BaseParams baseParams)
        {
            var stateCode = baseParams.Params.GetAs<string>("stateCode");
            var entityId = baseParams.Params.GetAs<long>("entityId");

            if (string.IsNullOrEmpty(stateCode) || !(entityId > 0))
            {
                return new BaseDataResult(false, "Обязательные параметры отсутствуют, невозможно сменить статус");
            }

            var stateProvider = this.container.Resolve<IStateProvider>();
            var stateDomain = this.container.ResolveDomain<State>();

            using (this.container.Using(stateDomain))
            {
                var stateTypeId = stateProvider.GetStatefulEntityInfo(typeof(RapidResponseSystemAppealDetails)).TypeId;
                var state = stateDomain.FirstOrDefault(x => x.TypeId == stateTypeId && x.Code == stateCode);

                if (state is null)
                {
                    return new BaseDataResult(false, "Статус не найден");
                }

                stateProvider.ChangeState(entityId, state.TypeId, state);

                return new BaseDataResult();
            }
        }

        /// <inheritdoc />
        public IDataResult GetAvailableContragents(BaseParams baseParams)
        {
            var appealCitsId = baseParams.Params.GetAsId("appealCitizensId");

            var appealCitsRealityObjectDomain = this.container.ResolveDomain<AppealCitsRealityObject>();
            var manOrgContractRealityObjectDomain = this.container.ResolveDomain<ManOrgContractRealityObject>();
            var publicServiceOrgContractRealObjDomain = this.container.ResolveDomain<PublicServiceOrgContractRealObj>();

            using (this.container.Using(appealCitsRealityObjectDomain,
                manOrgContractRealityObjectDomain, publicServiceOrgContractRealObjDomain))
            {
                var rapidResponseSystemAppealContragentIdsQuery = this.rapidResponseSystemAppealDomain.GetAll()
                    .Where(x => x.AppealCits.Id == appealCitsId)
                    .Select(x => x.Contragent.Id);

                var manOrgContragentsQuery = appealCitsRealityObjectDomain.GetAll()
                    .Where(x => x.AppealCits.Id == appealCitsId)
                    .Join(manOrgContractRealityObjectDomain.GetAll(),
                        x => x.RealityObject.Id,
                        y => y.RealityObject.Id,
                        (x, y) => y)
                    .Where(x =>
                        x.ManOrgContract.EndDate == null &&
                        x.ManOrgContract.ManagingOrganization.Contragent != null &&
                        x.ManOrgContract.ManagingOrganization.Contragent.IncludeInSopr &&
                        !rapidResponseSystemAppealContragentIdsQuery.Any(y => x.ManOrgContract.ManagingOrganization.Contragent.Id == y))
                    .Select(x => x.ManOrgContract.ManagingOrganization.Contragent);

                return appealCitsRealityObjectDomain.GetAll()
                    .Where(x => x.AppealCits.Id == appealCitsId)
                    .Join(publicServiceOrgContractRealObjDomain.GetAll(),
                        x => x.RealityObject.Id,
                        y => y.RealityObject.Id,
                        (x, y) => y)
                    .Where(x =>
                        x.RsoContract.DateEnd == null &&
                        x.RsoContract.PublicServiceOrg.Contragent != null &&
                        x.RsoContract.PublicServiceOrg.Contragent.IncludeInSopr &&
                        !rapidResponseSystemAppealContragentIdsQuery.Any(z => x.RsoContract.PublicServiceOrg.Contragent.Id == z))
                    .Select(x => x.RsoContract.PublicServiceOrg.Contragent)
                    .AsEnumerable()
                    .Union(manOrgContragentsQuery)
                    .Select(x => new
                    {
                        x.Id,
                        x.ShortName,
                        Municipality = x.Municipality?.Name,
                        x.Inn
                    })
                    .ToListDataResult(baseParams.GetLoadParam());
            }
        }

        /// <inheritdoc />
        public IDataResult GetAppealsStatistic()
        {
            var operatorInspector = this.userManager.GetActiveOperator()?.Inspector;

            var baseQuery = this.rapidResponseSystemAppealDomain.GetAll()
                .WhereIf(operatorInspector != null, x => x.AppealCits.Executant != null && operatorInspector.Id == x.AppealCits.Executant.Id)
                .Select(x => x.AppealCits)
                .AsEnumerable()
                .DistinctBy(x => x.Id);

            var data = new
            {
                NewAppealsCount = baseQuery.Count(x => x.State.StartState),
                AppealsInWorkCount = baseQuery.Count(x => !x.State.StartState && !x.State.FinalState),
                ClosedAppealsCount = baseQuery.Count(x => x.State.FinalState)
            };

            return new BaseDataResult(data);
        }

        /// <inheritdoc />
        public IDataResult GetAppealDetailsStatistic()
        {
            var baseQuery = this.GetSoprAppeals();

            var data = new
            {
                NewAppealsCount = baseQuery.Count(x => x.AppealDetails.State.StartState),
                AppealsInWorkCount = baseQuery.Count(x => !x.AppealDetails.State.StartState && !x.AppealDetails.State.FinalState),
                ProcessedAppealsCount = baseQuery.Count(x => x.AppealDetails.State.Code == "3"), // Статус "Обработано"
                NotProcessedAppealsCount = baseQuery.Count(x => x.AppealDetails.State.Code == "4") // Статус "Не обработано"
            };

            return new BaseDataResult(data);
        }

        /// <inheritdoc />
        public IQueryable<RapidResponseSystemAppealLink> GetSoprAppeals()
        {
            var authorizeService = this.container.Resolve<IAuthorizationService>();
            var userIdentityProvider = this.container.Resolve<IUserIdentityProvider>();
            var operatorContragentIds = this.userManager.GetContragentIds();

            return (from appealDetails in this.rapidResponseSystemAppealDetailsDomain.GetAll()
                join appeal in this.rapidResponseSystemAppealDomain.GetAll() on appealDetails.RapidResponseSystemAppeal.Id equals appeal.Id 
                select new RapidResponseSystemAppealLink { Appeal = appeal, AppealDetails = appealDetails })
            .WhereIf(!authorizeService.Grant(userIdentityProvider.GetCurrentUserIdentity(), "CitizenAppealModule.RapidResponseSystem.ViewAll"),
                x => x.Appeal.Contragent != null && operatorContragentIds.Contains(x.Appeal.Contragent.Id) ||
                    x.AppealDetails.User != null && this.userManager.GetActiveUser().Id == x.AppealDetails.User.Id);
        }

        /// <inheritdoc />
        public IDataResult UpdateSoprControlPeriod(BaseParams baseParams)
        {
            //Получаем значения параметров из конфигурации
            var config = this.container.GetGkhConfig<AppealConfig>();
            // Параметер 'Учитывать закрытые обращения'
            var considerClosedAppeals = config.RapidResponseSystemConfig.ConsiderClosedAppeals;
            // Новое значение параметра 'Контрольного срока'
            var controlPeriodParameter = config.RapidResponseSystemConfig.ControlPeriodParameter;
            
            // Данные парметры приходят только из интерцептора 'DayInterceptor.BeforeUpdateAction'
            var fromCalendar = baseParams.Params.GetAs<bool>("fromCalendar");
            var newDate = baseParams.Params.GetAs<DateTime>("newDate");

            // В транзакции получаем по фильтрам записи и расчитываем новый контрольный срок, после обновляем запись
            this.container.InTransaction(() =>
            {
                this.rapidResponseSystemAppealDetailsDomain.GetAll()
                    .WhereIf(!considerClosedAppeals, x => !x.State.FinalState)
                    .WhereIf(fromCalendar,
                        x => x.ReceiptDate >= newDate.AddDays(-controlPeriodParameter) &&
                            x.ReceiptDate <= newDate.AddDays(controlPeriodParameter))
                    .ForEach(x =>
                    {
                        x.ControlPeriod = this.GetControlPeriodMaxDay(controlPeriodParameter, x.ReceiptDate);
                        this.rapidResponseSystemAppealDetailsDomain.Update(x);
                    });
            });

            return new BaseDataResult();
        }

        /// <inheritdoc />
        public DateTime GetControlPeriodMaxDay(int additionalDaysCount, DateTime fromDate)
        {
            return this.dayDomain.GetAll()
                .Where(x => x.DayType == DayType.Workday && x.DayDate.Date > fromDate)
                .OrderBy(x => x.DayDate)
                .Take(additionalDaysCount)
                .AsEnumerable()
                .Last().DayDate;
        }

        /// <inheritdoc />
        public IDataResult SendNotificationMail(BaseParams baseParams)
        {
            var appealDetailsId = baseParams.Params.GetAsId("appealDetailsId");
            var contragent = this.rapidResponseSystemAppealDetailsDomain.Get(appealDetailsId).RapidResponseSystemAppeal.Contragent;
            
            var errorStringBuilder = new StringBuilder();
            var config = this.container.GetGkhConfig<AppealConfig>().RapidResponseSystemConfig;

            if (!config.EnableSendingNotifications)
            {
                errorStringBuilder.AppendLine("Не активирована конфигурация \"Включить отправку уведомлений о направленных обращениях в СОПР\" в настройках приложения");
            }

            if (contragent.ReceiveNotifications == YesNo.No)
            {
                errorStringBuilder.AppendLine("У организации отключены уведомления на E-mail.");
            }

            if (string.IsNullOrEmpty(contragent.Email))
            {
                errorStringBuilder.AppendLine("У организации не заполнен E-mail.");
            }

            if (errorStringBuilder.Length != 0)
            {
                errorStringBuilder.AppendLine("Уведомление не будет направлено.");

                return new BaseDataResult(false, errorStringBuilder.ToString());
            }

            var data = this.rapidResponseSystemAppealMailProvider.PrepareData(baseParams);
            var messageBody = this.rapidResponseSystemAppealMailProvider.PrepareMessage(data);

            var mailInfo = new MailInfo
            {
                MessageBody = messageBody, 
                RecieverMailAddress = contragent.Email,
                MailTheme = "Уведомление от системы оперативного реагирования в ГИС МЖФ РТ"
            };
            this.rapidResponseSystemAppealMailProvider.SendMessage(mailInfo);

            return new BaseDataResult();
        }
    }
}