namespace Bars.GkhGji.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Impl;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// Места проведения проверки
    /// </summary>
    public class AuditPlaceExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "AUDITPLACE";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Gji |
            FormatDataExportProviderFlags.Omjk;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<AuditPlaceProxy>()
                .ExtProxyListCache
                .Select((x, i) => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.AuditId.ToStr(),
                        x.Number.ToStr(),
                        x.HouseId.ToStr(),
                        x.AdditionalInfo
                    }))
                .ToList();
        }

        /// <inheritdoc />
        protected override IList<int> MandatoryFields { get; } = new List<int> { 0, 1, 2 };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Проверка",
                "Порядковый номер",
                "Дом",
                "Дополнительная информация"
           };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(AuditExportableEntity), typeof(HouseExportableEntity));
        }
    }
}