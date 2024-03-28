namespace Bars.Gkh.Gis.ExecutionAction
{
    using System;

    using Bars.B4;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Gis.DomainService.Dict;

    /// <summary>
    /// Импорт данных по тарифам из ЕИАС
    /// </summary>
    public class EiasTariffImportServiceAction : BaseExecutionAction
    {
        /// <inheritdoc />
        public override string Description =>
            "Действие предназначено для импорта сведений о тарифах из ЕИАС посредством веб-сервиса";

        /// <inheritdoc />
        public override string Name => "ГИС - Импорт данных по тарифам из ЕИАС";

        /// <inheritdoc />
        public override Func<IDataResult> Action => this.Execute;

        public IEiasTariffImportService Service { get; set; }

        private IDataResult Execute()
        {
            return this.Service.Import(this.ExecutionParams);
        }
    }
}