using Bars.GisIntegration.Base.Exporters;
using Bars.GisIntegration.Smev.Tasks.PrepareData;
using Bars.GisIntegration.Smev.Tasks.SendData;
using Bars.GkhGji.Regions.Tatarstan.Entities;
using System;

namespace Bars.GisIntegration.Smev.Exporters
{
    /// <summary>
    /// Корректировка <see cref="TatarstanDisposal"/>
    /// </summary>
    class DisposalCorrectionExporter : BaseDataExporter
    {
        /// <inheritdoc />
        public override bool NeedSign => false;

        /// <inheritdoc />
        public override string Name => "Корректировка распоряжения";

        /// <inheritdoc />
        public override int Order => 10;

        /// <inheritdoc />
        public override bool DataSupplierIsRequired => false;

        /// <inheritdoc />
        public override int MaxRepeatCount => 1440; // 5 суток с интервалом в 300 секунд

        /// <inheritdoc />
        public override int Interval => 300;

        /// <inheritdoc />
        public override Type PrepareDataTaskType => typeof(DisposalPrepareDataTask);

        /// <inheritdoc />
        public override Type SendDataTaskType => typeof(DisposalSendDataTask);
    }
}
