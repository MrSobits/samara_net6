namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Файлы договора на выполнение работ по капитальному ремонту
    /// </summary>
    public class DogovorPkrFilesExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "DOGOVORPKRFILES";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.RegOpCr;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            var exportData = this.ProxySelectorFactory.GetSelector<DogovorPkrProxy>()
                .ExtProxyListCache
                .Where(x => x.File != null)
                .DistinctBy(x => x.File.Id);

            return this.AddFilesToExport(exportData, x => x.File)
                .Select(x => new ExportableRow(x.File.Id,
                    new List<string>
                    {
                        this.GetStrId(x.File),
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
                "Уникальный идентификатор договора на выполнение работ по капитальному ремонту",
                "Тип файла"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(DogovorPkrExportableEntity));
        }
    }
}