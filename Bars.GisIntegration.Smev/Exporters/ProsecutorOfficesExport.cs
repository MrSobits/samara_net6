namespace Bars.GisIntegration.Smev.Exporters
{
    using System;

    using Bars.GisIntegration.Base.Exporters;
    using Bars.GisIntegration.Smev.Tasks.PrepareData;
    using Bars.GisIntegration.Smev.Tasks.SendData;
    using Bars.GkhGji.Regions.Tatarstan.Entities;

    /// <summary>
    /// Запрос получения справочников прокуратур <see cref="ProsecutorOfficeDict"/>
    /// </summary>
    public class ProsecutorOfficesExport : BaseDataExporter
    {
        /// <inheritdoc />
        public override bool NeedSign => false;

        /// <inheritdoc />
        public override string Name => "Запрос получения справочников прокуратур";

        /// <inheritdoc />
        public override int Order => 10;

        /// <inheritdoc />
        public override bool DataSupplierIsRequired => false;

        /// <inheritdoc />
        public override int MaxRepeatCount => 864; // 3-е суток с интервалом в 300 секунд

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
        public override Type PrepareDataTaskType => typeof(ProsecutorOfficePrepareDataTask);

        /// <inheritdoc />
        public override Type SendDataTaskType => typeof(ProsecutorOfficeSendDataTask);
    }
}
