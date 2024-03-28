namespace Bars.GisIntegration.Smev.Exporters
{
    using System;

    using Bars.GisIntegration.Base.Exporters;
    using Bars.GisIntegration.Smev.Tasks.PrepareData;
    using Bars.GisIntegration.Smev.Tasks.SendData;

    /// <summary>
    /// Корректировка КНМ
    /// </summary>
    public class KnmCorrectionExporter : BaseDataExporter
    {
        /// <inheritdoc />
        public override bool NeedSign => false;

        /// <inheritdoc />
        public override string Name => "Корректировка КНМ";

        /// <inheritdoc />
        public override int Order => 10;

        /// <inheritdoc />
        public override bool DataSupplierIsRequired => false;

        /// <inheritdoc />
        public override int MaxRepeatCount => 1440; // 5 суток с интервалом в 300 секунд

        /// <inheritdoc />
        public override int Interval
        {
            get
            {
                #if DEBUG
                    return 30;
                #else
                    return 300;
                #endif
            }
        }

        /// <inheritdoc />
        public override Type PrepareDataTaskType => typeof(KnmCorrectionPrepareDataTask);

        /// <inheritdoc />
        public override Type SendDataTaskType => typeof(KnmSendDataTask);
    }
}
