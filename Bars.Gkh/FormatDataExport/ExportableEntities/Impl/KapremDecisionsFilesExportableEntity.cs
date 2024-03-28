namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// 2.22.3 Файлы решений по капитальному ремонту (kapremdecisionsfiles.csv)
    /// </summary>
    public class KapremDecisionsFilesExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "KAPREMDECISIONSFILES";

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            var exportData = this.ProxySelectorFactory.GetSelector<KapremDecisionsProxy>()
                .ExtProxyListCache
                .Where(x => x.File != null)
                .DistinctBy(x => x.File.Id);

            return this.AddFilesToExport(exportData, x => x.File)
                .Select(x => new ExportableRow(x,
                    new List<string>
                    {
                        x.File.Id.ToStr(),
                        x.Id.ToStr(),
                        x.FileType.ToStr()
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
                "Идентификатор решения",
                "Тип файла"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(KapremDecisionsExportableEntity));
        }
    }
}