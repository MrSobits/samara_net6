namespace Bars.GkhDi.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// ОКТМО к документам об утверждении тарифов ЖКУ
    /// </summary>
    public class TarifOktmoExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "TARIFOKTMO";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.Ogv |
            FormatDataExportProviderFlags.Oms;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<TarifProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.Oktmo.ToStr()
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
                "Документ об утверждении тарифов ЖКУ",
                "ОКТМО"
            };
        }
    }
}