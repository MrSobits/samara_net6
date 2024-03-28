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
    /// Документы проверки ГЖИ
    /// </summary>
    public class AuditFilesExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "AUDITFILES";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Gji |
            FormatDataExportProviderFlags.Omjk;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            var exportData = this.ProxySelectorFactory.GetSelector<AuditFilesProxy>()
                .ExtProxyListCache
                .Where(x => x.File != null);

            var id = 1;
            return this.AddFilesToExport(exportData, x => x.File)
                .Select(x => new ExportableRow(id++,
                    new List<string>
                    {
                        x.Id.ToStr(), // 1. Уникальный код
                        x.AuditId.ToStr(), // 2. Проверка
                        x.Type.ToStr() // 3. Тип файла
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
                "Уникальный код",
                "Проверка",
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