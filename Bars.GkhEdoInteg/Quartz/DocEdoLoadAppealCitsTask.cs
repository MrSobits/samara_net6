namespace Bars.GkhEdoInteg.Quartz
{
    using Bars.B4.Application;
    using Bars.B4.Config;
    using Bars.B4.Modules.Quartz;
    using Bars.B4.Utils;

    using Castle.MicroKernel.Lifestyle;

    public class DocEdoLoadAppealCitsTask : BaseTask
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

                using (this.Container.BeginScope())
                {
                    string msg;
                    var emsedService = this.Container.Resolve<IEmsedService>();
                    if (!emsedService.LoadDocuments(@params, out msg))
                    {
                        //LogManager.GetLogger("B4 Application").Info("Загрузка документов обращений из ЭДО", new Exception(msg));
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
