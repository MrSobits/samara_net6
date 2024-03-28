namespace Bars.GkhGji.Regions.Tatarstan.Quartz
{
    using Bars.B4.Application;
    using Bars.B4.Modules.Quartz;
    using Bars.B4.Utils;

    using Castle.MicroKernel.Lifestyle;

    /// <summary>
    /// Задача на создание напоминалок по Постановлениям 
    /// </summary>
    public class ResolutionQuartzTask : BaseTask
    {
        public override void Execute(DynamicDictionary @params)
        {
            var service = this.Container.Resolve<IReminderResolution>();

            try
            {
                using (this.Container.BeginScope())
                {
                    string msg;
                    if (!service.CreateReminders(@params, out msg))
                    {
                        // просто не получилось
                    }
                }
            }
            finally
            {
                Container.Release(service);
            }
            
        }
    }
}
