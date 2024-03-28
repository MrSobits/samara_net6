namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// Размещение размера платы за жилое помещение по договору управления
    /// </summary>
    public class DuChargeProtExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "DUCHARGEPROT";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.Oms;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.AddFilesToExport(this.ProxySelectorFactory
                        .GetSelector<DuProxy>()
                        .ExtProxyListCache.Where(x => x.PaymentProtocolFile != null),
                    x => x.PaymentProtocolFile)
                .Select(x => new ExportableRow(x,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.PaymentProtocolFile.Id.ToStr()
                    }))
                .ToList();
        }

        /// <inheritdoc />
        protected override IList<int> MandatoryFields => this.GetAllFieldIds();

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код записи сведений о размещении размера платы",
                "Протокол общего собрания собственников помещений в МКД об установлении размера платы за содержание жилого помещения"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(DuChargeExportableEntity));
        }
    }
}