namespace Bars.Gkh.Overhaul.Tat.ExecutionAction
{
    using System;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.Overhaul.Tat.ConfigSections;

    public class OverhaulTatConfigMigrationAction : BaseExecutionAction
    {
        public override Func<IDataResult> Action => this.Execute;

        public override string Description => @"ДПКР (РТ) - Копирование параметров в единые настройки";

        public override string Name => "ДПКР (РТ) - Копирование параметров в единые настройки";

        private BaseDataResult Execute()
        {
            var dpkrParamsService = this.Container.Resolve<IDpkrParamsService>();
            var provider = this.Container.Resolve<IGkhConfigProvider>();
            try
            {
                var config = provider.Get<OverhaulTatConfig>();
                var parameters = dpkrParamsService.GetParams();

                config.ProgrammPeriodStart = parameters.Get<int>("ProgrammPeriodStart");
                config.ProgrammPeriodEnd = parameters.Get<int>("ProgrammPeriodEnd");
                config.GroupByCeoPeriod = parameters.Get<int>("GroupByCeoPeriod");
                config.GroupByRoPeriod = parameters.Get<int>("GroupByRoPeriod");
                config.ServiceCost = parameters.Get<decimal>("ServiceCost");
                config.YearPercent = parameters.Get<decimal>("YearPercent");
                config.ShortTermProgPeriod = parameters.Get<int>("ShortTermProgPeriod");
                config.PublicationPeriod = parameters.Get<int>("PublicationPeriod");
                config.ActualizePeriodStart = parameters.Get<int>("ActualizePeriodStart");
                config.ActualizePeriodEnd = parameters.Get<int>("ActualizePeriodEnd");

                var exception = provider.SaveChanges();
                return exception == null ? new BaseDataResult() : new BaseDataResult(false, exception.Message);
            }
            finally
            {
                this.Container.Release(dpkrParamsService);
            }
        }
    }
}