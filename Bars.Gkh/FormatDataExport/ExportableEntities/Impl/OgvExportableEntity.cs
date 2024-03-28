namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;

    /// <summary>
    /// Органы государственной власти
    /// </summary>
    public class OgvExportableEntity : BaseExportableEntity<PoliticAuthority>
    {
        /// <inheritdoc />
        public override string Code => "OGV";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Ogv;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.GetFiltred(x => x.Contragent)
                .Select(x => new ExportableRow(x.Contragent,
                    new List<string>
                    {
                        this.GetStrId(x.Contragent),
                        string.Empty,
                        string.Empty
                    }))
                .ToList();
        }

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Контрагент",
                "Номер основания наделения полномочиями",
                "Дата основания наделения полномочиями"
            };
        }
    }
}