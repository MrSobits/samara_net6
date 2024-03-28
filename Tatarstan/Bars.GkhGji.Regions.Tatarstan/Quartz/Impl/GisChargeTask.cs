namespace Bars.GkhGji.Regions.Tatarstan.Quartz.Impl
{
    using B4.Modules.Quartz;
    using B4.Utils;

    using Bars.B4.Application;
    using Bars.B4.IoC;

    using Castle.MicroKernel.Lifestyle;

    using Integration;

    public class GisChargeTask : BaseTask
    {
        public override void Execute(DynamicDictionary @params)
        {
            using (this.Container.BeginScope())
            {
                var gisGmpIntegration = this.Container.Resolve<IGisGmpIntegration>();
                using (this.Container.Using(gisGmpIntegration))
                {
                    gisGmpIntegration.UploadCharges();
                }
            }
        }
    }
}