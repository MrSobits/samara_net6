namespace Bars.Gkh.Overhaul.Nso.ExecutionAction
{
    using System;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.ConfigSections.Overhaul.Enums;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.Overhaul.Nso.ConfigSections;

    public class OverhaulNsoConfigMigrationAction : BaseExecutionAction
    {
        public override string Name => "ДПКР (НСО) - Копирование параметров в единые настройки";

        public override string Description => @"ДПКР (НСО) - Копирование параметров в единые настройки";

        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var dpkrParamsService = this.Container.Resolve<IDpkrParamsService>();
            var provider = this.Container.Resolve<IGkhConfigProvider>();
            try
            {
                var config = provider.Get<OverhaulNsoConfig>();
                var parameters = dpkrParamsService.GetParams();

                config.ProgrammPeriodStart = parameters.Get<int>("ProgrammPeriodStart");
                config.ProgrammPeriodEnd = parameters.Get<int>("ProgrammPeriodEnd");
                config.GroupByCeoPeriod = parameters.Get<int>("GroupByCeoPeriod");
                config.WorkPriceCalcYear = parameters.Get<WorkPriceCalcYear>("WorkPriceCalcYear");
                config.WorkPriceDetermineType = parameters.Get<WorkPriceDetermineType>("WorkPriceDetermineType");
                config.MethodOfCalculation = parameters.Get<TypePriority>("MethodOfCalculation");
                config.RateCalcTypeArea = parameters.Get<RateCalcTypeArea>("RateCalcTypeArea");
                config.GroupByRoPeriod = parameters.Get<int>("GroupByRoPeriod");
                config.ServiceCost = parameters.Get<decimal>("ServiceCost");
                config.YearPercent = parameters.Get<decimal>("YearPercent");
                config.ShortTermProgPeriod = parameters.Get<int>("ShortTermProgPeriod");
                config.HouseAddInProgramConfig.MinimumCountApartments = parameters.Get<int>("MinimumCountApartments");
                config.HouseAddInProgramConfig.TypeUseWearMainCeo = parameters.Get<TypeUseWearMainCeo>("TypeUseWearMainCeo");
                config.HouseAddInProgramConfig.WearMainCeo = parameters.Get<decimal>("WearMainCeo");
                config.HouseAddInProgramConfig.UsePhysicalWearout = parameters.Get<bool>("UsePhysicalWearout") ? TypeUsage.Used : TypeUsage.NoUsed;
                config.HouseAddInProgramConfig.PhysicalWear = parameters.Get<decimal>("PhysicalWear");
                config.HouseAddInProgramConfig.UseLimitCost = parameters.Get<bool>("UseLimitCost") ? TypeUsage.Used : TypeUsage.NoUsed;
                config.SubsidyConfig.UseFixationPublishedYears = parameters.Get<bool>("UseFixationPublishedYears") ? TypeUsage.Used : TypeUsage.NoUsed;
                config.SubsidyConfig.TypeCorrection = parameters.Get<TypeCorrection>("TypeCorrection");
                config.SubsidyConfig.UsePlanOwnerCollectionType = parameters.Get<UsePlanOwnerCollectionType>("UsePlanOwnerCollectionType");
                config.SubsidyConfig.UseLifetime = parameters.Get<bool>("UseLifetime") ? TypeUsage.Used : TypeUsage.NoUsed;
                config.PublishProgramConfig.PublicationPeriod = parameters.Get<int>("PublicationPeriod");
                config.PublishProgramConfig.UseShortProgramPeriod = parameters.Get<TypeUseShortProgramPeriod>("UseShortProgramPeriod");
                config.ActualizeConfig.ActualizeUseValidShortProgram = parameters.Get<bool>("ActualizeUseValidShortProgram")
                    ? TypeUsage.Used
                    : TypeUsage.NoUsed;

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