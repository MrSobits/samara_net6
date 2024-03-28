namespace Bars.GkhEdoInteg.Quartz
{
    using System.Linq;

    using Bars.B4.Application;
    using Bars.B4.Config;
    using Bars.B4.Modules.Quartz;
    using Bars.B4.Utils;

    using Castle.MicroKernel.Lifestyle;

    using global::Quartz;

    public class EdoLoadAppealCitsTask : BaseTask
    {
        public override void Execute(DynamicDictionary @params)
        {
            if (!IntegrationEmsedServiceSyncContext.IsReady())
            {
                return;
            }
            try
            {
                var config = this.Container.Resolve<IConfigProvider>().GetConfig();
                if (config == null || !config.AppSettings.ContainsKey("AutomaticIntegEdo"))
                {
                    return;
                }

                if (!config.AppSettings["AutomaticIntegEdo"].ToBool())
                {
                    return;
                }

                var scheduler = this.Container.Resolve<IScheduler>();
                var jobKey = new JobKey("IntegEdo", "EdoGroup");
                
                // Проверка на текущее выполнения Job Должно быть выше, тк при вхождение в этот метод Job текущий будет уже в scheduler
                var isCompleteLog = scheduler.GetCurrentlyExecutingJobs().GetAwaiter().GetResult().Count(x => x.JobDetail?.Key.Name == jobKey.Name && x.JobDetail?.Key.Group == jobKey.Group) == 1;

                if (!isCompleteLog)
                {
                    return;
                }
                
                
               
                using (this.Container.BeginScope())
                {
                    string msg;
                    var emsedService = this.Container.Resolve<IEmsedService>();
                    if (!emsedService.LoadData(@params, out msg))
                    {
                        //LogManager.GetLogger("B4 Application").Info("Загрузка обращений из ЭДО", new Exception(msg));
                    }
                }
            }
            finally
            {
                IntegrationEmsedServiceSyncContext.Ready();
            }
        }
    }
}
