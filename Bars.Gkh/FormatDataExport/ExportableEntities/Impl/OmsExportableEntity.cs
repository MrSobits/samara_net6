namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;

    /// <summary>
    /// Органы местного самоуправления
    /// </summary>
    public class OmsExportableEntity : BaseExportableEntity<LocalGovernment>
    {
        /// <inheritdoc />
        public override string Code => "OMS";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Oms;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.GetFiltred(x => x.Contragent)
                .Select(x => new ExportableRow(x.Contragent,
                    new List<string>
                    {
                        this.GetStrId(x.Contragent) //1. Контрагент
                    }))
                .ToList();
        }

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Контрагент"
            };
        }
    }
}