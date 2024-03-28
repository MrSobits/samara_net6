namespace Bars.Gkh.Overhaul.Hmao.ExecutionAction
{
    using System;

    using Bars.B4;
    using Bars.Gkh.Config;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Действие "увеличивает" значение года Периода корректировки (Субсидирование)
    /// </summary>
    /// <remarks>
    /// на самом деле просто устанавливает в поле текущий год
    /// </remarks>
    public class CorrectionPeriodAction : BaseExecutionAction
    {
        public override string Name => "Увеличение года корректировки";

        /// <inheritdoc />
        public override string Description => @"Ежегодное увеличение значения года в поле ""Период корректировки года с""";

        /// <inheritdoc />
        public override Func<IDataResult> Action => this.CorrectionPeriod;
        
        private BaseDataResult CorrectionPeriod()
        {
            var provider = this.Container.Resolve<IGkhConfigProvider>();
            
            var config = Container.GetGkhConfig<OverhaulHmaoConfig>().SubsidyConfig;
            config.CorrectionPeriodStart = DateTime.Today.Year;
            
            provider.SaveChanges();

            return new BaseDataResult();
        }
    }
}