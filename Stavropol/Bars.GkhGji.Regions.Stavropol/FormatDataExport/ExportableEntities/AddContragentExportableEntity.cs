namespace Bars.GkhGji.Regions.Stavropol.FormatDataExport.ExportableEntities
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.Overhaul.Hmao.FormatDataExport.ProxyEntities;

    /// <summary>
    /// Дополнительный поставщик информации (addcontragent.csv)
    /// </summary>
    public class AddContragentExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "ADDCONTRAGENT";

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory
                .GetSelector<AddContragentProxy>()
                .ExtProxyListCache
                .Select(s => new ExportableRow(s.Id,
                    new List<string>
                    {
                        s.Id.ToStr(),
                        s.InformationProviderType.ToStr()
                    }))
                .ToList();
        }

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код контрагента",
                "Тип поставщика информации"
            };
        }
    }
}