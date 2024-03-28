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
    /// Протоколы собрания собственников к договорам на пользование общим имуществом
    /// </summary>
    public class DogPoiProtocolossExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "DOGPOIPROTOCOLOSS";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            var exportData = this.ProxySelectorFactory.GetSelector<DogPoiProxy>()
                .ExtProxyListCache
                .Where(x => x.ProtocolFile != null);

            return this.AddFilesToExport(exportData, x => x.ProtocolFile)
                .Select(x => new ExportableRow(x.ProtocolFile,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        this.GetStrId(x.ProtocolFile)
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
                "Договор на пользование общим имуществом",
                "Протокол общего собрания собственников"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "DOGPOI",
                //"PROTOCOLOSS"
            };
        }
    }
}