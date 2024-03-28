namespace Bars.Gkh.Overhaul.Hmao.ExecutionAction
{
    using System;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.ConfigSections.Overhaul.Enums;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;

    /// <summary>
    /// Действие копирования настроек из config в БД
    /// </summary>
    public class OverhaulHmaoConfigMigrationAction : BaseExecutionAction
    {
        /// <summary>
        /// IoC Контейнер
        /// </summary>
        /// <summary>
        /// Код действия
        /// </summary>
        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => "ДПКР (ХМАО) - Копирование параметров в единые настройки";

        /// <summary>
        /// Описание
        /// </summary>
        public override string Description => @"ДПКР (ХМАО) - Копирование параметров в единые настройки";

        /// <summary>
        /// Код действия
        /// </summary>
        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var dpkrParamsService = this.Container.Resolve<IDpkrParamsService>();
            var provider = this.Container.Resolve<IGkhConfigProvider>();
            try
            {
                var config = provider.Get<OverhaulHmaoConfig>();
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
                config.HouseTypesConfig.UseManyApartments = parameters.Get<bool>("UseManyApartments");
                config.HouseTypesConfig.UseBlockedBuilding = parameters.Get<bool>("UseBlockedBuilding");
                config.HouseTypesConfig.UseIndividual = parameters.Get<bool>("UseIndividual");
                config.HouseTypesConfig.UseSocialBehavior = parameters.Get<bool>("UseSocialBehavior");
                config.HouseAddInProgramConfig.MinimumCountApartments = parameters.Get<int>("MinimumCountApartments");
                config.HouseAddInProgramConfig.TypeUseWearMainCeo = parameters.Get<TypeUseWearMainCeo>("TypeUseWearMainCeo");
                config.HouseAddInProgramConfig.WearMainCeo = parameters.Get<decimal>("WearMainCeo");
                config.HouseAddInProgramConfig.UsePhysicalWearout = parameters.Get<bool>("UsePhysicalWearout") ? TypeUsage.Used : TypeUsage.NoUsed;
                config.HouseAddInProgramConfig.PhysicalWear = parameters.Get<decimal>("PhysicalWear");
                config.HouseAddInProgramConfig.UseLimitCost = parameters.Get<bool>("UseLimitCost") ? TypeUsage.Used : TypeUsage.NoUsed;
                config.SubsidyConfig.UseFixationPublishedYears = parameters.Get<bool>("UseFixationPublishedYears") ? TypeUsage.Used : TypeUsage.NoUsed;
                config.SubsidyConfig.UseRealObjCollection = parameters.Get<bool>("UseRealObjCollection") ? TypeUsage.Used : TypeUsage.NoUsed;
                config.SubsidyConfig.TypeCorrection = parameters.Get<TypeCorrection>("TypeCorrection");
                config.SubsidyConfig.TypeCorrectionActualizeRecs = parameters.Get<TypeCorrectionActualizeRecs>("TypeCorrectionActualizeRecs");
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
                this.Container.Release(provider);
                this.Container.Release(dpkrParamsService);
            }
        }
    }
}