namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// Файлы кредитного договора/договора займа (creditcontractfiles.csv)
    /// </summary>
    public class CreditContractFilesExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "CREDITCONTRACTFILES";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Ogv
            | FormatDataExportProviderFlags.RegOpCr
            | FormatDataExportProviderFlags.Uo;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.AddFilesToExport(this.ProxySelectorFactory.GetSelector<CreditContractProxy>()
                        .ExtProxyListCache,
                    x => x.File)
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
                "Идентификатор файла",
                "Идентификатор кредитного договора/договора займа",
                "Тип файла"
            };
        }
    }
}