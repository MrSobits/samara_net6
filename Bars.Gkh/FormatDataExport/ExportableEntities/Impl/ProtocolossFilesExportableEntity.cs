namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// Файлы протокола общего собрания собственников
    /// </summary>
    public class ProtocolossFilesExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "PROTOCOLOSSFILES";

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            var entities = this.ProxySelectorFactory.GetSelector<ProtocolossProxy>()
                .ExtProxyListCache;

            return this.AddFilesToExport(entities, x => x.AttachmentFile)
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.AttachmentFile.Id.ToStr(),
                        x.Id.ToStr()
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
                "Уникальный идентификатор файла",
                "Уникальный идентификатор протокола голосования",
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(ProtocolossExportableEntity));
        }
    }
}