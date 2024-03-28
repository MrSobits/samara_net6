namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// Управляющие организации/Протоколы общего собрания собственников
    /// </summary>
    [Obsolete("СА: Не выгружаем", true)]
    public class UoProtocolossExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "UOPROTOCOLOSS";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            var id = 1L;

            return this.ProxySelectorFactory.GetSelector<ProtocolossProxy>()
                .ExtProxyListCache
                .Where(x => x.ContragentId.HasValue)
                .Select(x => new ExportableRow(id++,
                    new List<string>
                    {
                        x.ContragentId.ToStr(),
                        x.Id.ToStr()
                    }))
                .ToList();
        }

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "УО",
                "Протокол ОСС"
            };
        }
    }
}