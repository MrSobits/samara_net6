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
    /// Файлы актов выполненных работ по  договору на выполнение работ по капитальному ремонту(actworkdogovfiles.csv)
    /// </summary>
    public class ActWorkDogovFilesExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "ACTWORKDOGOVFILES";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.RegOpCr;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            var exportData = this.ProxySelectorFactory.GetSelector<ActWorkDogovProxy>()
                .ExtProxyListCache
                .Where(x => x.File != null)
                .DistinctBy(x => x.Id);

            return this.AddFilesToExport(exportData, x => x.File)
                .Select(x => new ExportableRow(x.File.Id,
                    new List<string>
                    {
                        this.GetStrId(x.File),
                        x.Id.ToStr(),
                        x.Type.ToStr()
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
                "Уникальный идентификатор акта",
                "Тип файла"
            };
        }
    }
}