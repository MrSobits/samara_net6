namespace Bars.GkhGji.Regions.Stavropol.FormatDataExport.ExportableEntities
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.Overhaul.Hmao.FormatDataExport.ProxyEntities;

    /// <summary>
    /// 2.22.9 Файлы документов ПКР (pkrdocfiles.csv)
    /// Для Ставропольского края
    /// </summary>
    public class PkrDocFilesExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "PKRDOCFILES";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Ogv;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            var data = this.ProxySelectorFactory
                .GetSelector<PkrDocProxy>()
                .ExtProxyListCache;

            return this.AddFilesToExport(data, x => x.File)
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        this.GetStrId(x.File)
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
                "Идентификатор документа КПР",
                "Идентификатор файла"
            };
        }
    }
}