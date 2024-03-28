namespace Bars.Gkh.Reforma.Domain.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Config;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Quartz;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.Reforma.Entities;
    using Bars.Gkh.Reforma.Entities.Dict;
    using Bars.Gkh.Reforma.Enums;
    using Bars.Gkh.Reforma.Interface;
    using Bars.Gkh.Reforma.PerformerActions.GetReportingPeriodList;
    using Bars.Gkh.Reforma.Tasks;

    using Castle.MicroKernel;
    using Castle.MicroKernel.Lifestyle;
    using Castle.Windsor;

    using global::Quartz;

    /// <summary>
    ///     Сервис управления интеграцией с Реформой ЖКХ
    /// </summary>
    public class SyncService : ISyncService
    {
        #region Fields

        private readonly IConfigProvider configProvider;

        private readonly IWindsorContainer container;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        /// <param name="configProvider">Провайдер конфигурации</param>
        public SyncService(IWindsorContainer container, IConfigProvider configProvider)
        {
            this.container = container;
            this.configProvider = configProvider;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Получение параметров
        /// </summary>
        /// <returns>Параметры</returns>
        public IDataResult GetParams()
        {
            var appSettings = this.configProvider.GetConfig().AppSettings;
            return
                new BaseDataResult(
                    new Dictionary<string, object>
                        {
                            { "User", appSettings.Get("Reforma.User").ToStr() }, 
                            { "Password", appSettings.Get("Reforma.Password").ToStr() }, 
                            { "RemoteAddress", appSettings.Get("Reforma.RemoteAddress").ToStr() }, 
                            { "Enabled", appSettings.Get("Reforma.Enabled").ToBool() }, 
                            { "IntegrationTime", appSettings.Get("Reforma.IntegrationTime").ToDateTime().ToShortTimeString() },
                            { "NullIsNotData", appSettings.Get("Reforma.NullIsNotData").ToBool() }
                        });
        }

        /// <summary>
        ///     Запустить интеграцию немедля
        /// </summary>
        /// <returns>Результат</returns>
        public IDataResult RunNow()
        {
            var key = new JobKey("Integration", "Reforma");
            var scheduler = this.container.Resolve<IScheduler>();
            if (scheduler.CheckExists(key).GetResultWithoutContext())
            {
                if (ReformaIntegrationTask.IsRunning)
                {
                    return new BaseDataResult(false, "Интеграция уже запущена. Дождитесь окончания процесса и повторите попытку");
                }

                scheduler.TriggerJob(key, new JobDataMap
                {
                    { "typeIntegration", TypeIntegration.Manual }
                });
            }
            else
            {
                return new BaseDataResult(false, "Интеграция отключена. Включите её в настройках и повторите попытку");
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// Получение информации о жилом доме
        /// </summary>
        /// <param name="id">Идентификатор жилого дома</param>
        /// <returns>Информация</returns>
        public IDataResult GetRobjectDetails(long id)
        {
            var service = container.ResolveDomain<RefRealityObject>();
            try
            {
                return new BaseDataResult(new { ExternalIds = string.Join(", ", service.GetAll().Where(x => x.RealityObject.Id == id).Select(x => x.ExternalId)) });
            }
            finally
            {
                container.Release(service);
            }
        }

        /// <summary>
        /// Получение информации об УО
        /// </summary>
        /// <param name="id">Идентификатор УО</param>
        /// <returns>Информация</returns>
        public IDataResult GetManOrgDetails(long id)
        {
            var manOrgService = this.container.ResolveDomain<ManagingOrganization>();
            var refManOrgService = container.ResolveDomain<RefManagingOrganization>();
            try
            {
                var inn = manOrgService.Get(id).Return(x => x.Contragent.Inn);
                return !string.IsNullOrEmpty(inn) ? new BaseDataResult(refManOrgService.GetAll().FirstOrDefault(x => x.Inn == inn)) : new BaseDataResult();
            }
            finally
            {
                this.container.Release(manOrgService);
                container.Release(refManOrgService);
            }
        }

        /// <summary>
        ///     Сохранение параметров
        /// </summary>
        /// <param name="baseParams">Новый параметры</param>
        /// <returns>Результат сохранения</returns>
        public IDataResult SaveParams(BaseParams baseParams)
        {
            var parameters = baseParams.Params.GetAs<DynamicDictionary>("parameters");
            if (parameters == null)
            {
                return new BaseDataResult(false, "Не удалось прочитать параметры");
            }

            var appConfig = this.configProvider.GetConfig();
            var appSettings = appConfig.AppSettings;

            appSettings["Reforma.User"] = parameters.Get("User").ToStr();
            string password;
            if (parameters.ContainsKey("Password") && !(password = parameters.Get("Password").ToStr()).IsEmpty())
            {
                appSettings["Reforma.Password"] = password;
            }

            appSettings["Reforma.RemoteAddress"] = parameters.Get("RemoteAddress").ToStr();
            appSettings["Reforma.Enabled"] = parameters.Get("Enabled").ToStr();
            appSettings["Reforma.IntegrationTime"] = parameters.Get("IntegrationTime").ToDateTime().ToShortTimeString();
            appSettings["Reforma.NullIsNotData"] = parameters.Get("NullIsNotData").ToStr();

            this.configProvider.SaveConfig(appConfig);

            this.UpdateScheduledJob(appSettings["Reforma.Enabled"].ToBool(), appSettings["Reforma.IntegrationTime"].ToDateTime());

            if (baseParams.Params.Get("cleanup").ToBool())
            {
                this.Cleanup();
            }

            return new BaseDataResult();
        }

        #endregion

        #region Methods

        private void Cleanup()
        {
            var moService = this.container.ResolveDomain<RefManagingOrganization>();
            var roService = this.container.ResolveDomain<RefRealityObject>();
            var perService = this.container.ResolveDomain<ReportingPeriodDict>();
            try
            {
                this.container.InTransaction(
                    () =>
                        {
                            roService.GetAll().Select(x => x.Id).ToArray().ForEach(x => roService.Delete(x));
                            moService.GetAll().Select(x => x.Id).ToArray().ForEach(x => moService.Delete(x));
                            perService.GetAll().Select(x => x.Id).ToArray().ForEach(x => perService.Delete(x));
                        });
                
                using (this.container.BeginScope())
                {
                    var argument = new Arguments { { "silentMode", true } };
                    var provider = this.container.Resolve<ISyncProvider>(argument);
                    try
                    {
                        provider.Performer.AddToQueue<GetReportingPeriodListAction>();
                        provider.Performer.Perform();
                    }
                    finally
                    {
                        provider.Close();
                        this.container.Release(provider);
                    }
                }
            }
            finally
            {
                this.container.Release(moService);
                this.container.Release(roService);
                this.container.Release(perService);
            }
        }

        private void UpdateScheduledJob(bool enabled, DateTime integrationTime)
        {
            var scheduler = this.container.Resolve<IScheduler>();

            var key = new TriggerKey("Integration", "Reforma");
            if (enabled)
            {
                var trigger =
                    TriggerBuilder.Create()
                        .WithDailyTimeIntervalSchedule(
                            x => x.StartingDailyAt(TimeOfDay.HourMinuteAndSecondOfDay(integrationTime.Hour, integrationTime.Minute, integrationTime.Second)).WithIntervalInHours(24))
                        .StartNow()
                        .WithIdentity(key)
                        .Build();
                if (scheduler.CheckExists(key).GetResultWithoutContext())
                {
                    scheduler.RescheduleJob(key, trigger);
                }
                else
                {
                    scheduler.ScheduleJob(JobBuilder.Create<TaskJob<ReformaIntegrationTask>>().WithIdentity("Integration", "Reforma").Build(), trigger);
                }
            }
            else
            {
                scheduler.UnscheduleJob(key);
            }
        }

        #endregion
    }
}