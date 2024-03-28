namespace Bars.GkhGji.Regions.Tatarstan.Quartz.Impl
{
    using System;
    using B4.Modules.Quartz;
    using B4.Utils;

    using Bars.B4.Application;

    using Castle.MicroKernel.Lifestyle;

    using Integration;

    using Microsoft.Extensions.Logging;

    public class GisPaymentTask : BaseTask
    {
        public override void Execute(DynamicDictionary @params)
        {
            using (this.Container.BeginScope())
            {
                var logger = Container.Resolve<ILogger>();

                try
                {
                    var result = Container.Resolve<IGisGmpIntegration>().LoadPayments();

                    if (result.Success)
                    {
                        logger.LogInformation("Загрузка оплат из ГИС ГМП успешно выполнена");
                    }
                    else
                    {
                        logger.LogInformation("При загрузке оплат из ГИС ГМП произошла ошибка: " + result.Message);
                    }
                }
                catch (Exception e)
                {
                    logger.LogInformation("При загрузке оплат из ГИС ГМП произошла ошибка: " + e.Message);
                }
            }
        }
    }
}