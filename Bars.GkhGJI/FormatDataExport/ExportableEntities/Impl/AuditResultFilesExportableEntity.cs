namespace Bars.GkhGji.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// Файл результата проверки
    /// </summary>
    public class AuditResultFilesExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "AUDITRESULTFILES";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Gji |
            FormatDataExportProviderFlags.Omjk;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            var exportData = this.ProxySelectorFactory.GetSelector<AuditResultFilesProxy>()
                .ExtProxyListCache
                .Where(x => x.File != null);

            return this.AddFilesToExport(exportData, x => x.File)
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(), // 1. Уникальный код
                        x.AuditId.ToStr(), // 2. Уникальный идентификатор проверки
                        this.Yes // 3. Тип файла (всегда передаем 1)
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
                "Уникальный идентификатор проверки",
                "Тип файла"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(AuditExportableEntity));
        }
    }
}