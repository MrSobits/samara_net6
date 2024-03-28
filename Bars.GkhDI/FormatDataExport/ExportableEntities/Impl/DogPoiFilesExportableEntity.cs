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
    /// Файлы договора на пользование общим имуществом
    /// </summary>
    public class DogPoiFilesExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "DOGPOIFILES";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            var exportData = this.ProxySelectorFactory.GetSelector<DogPoiProxy>()
                .ExtProxyListCache
                .Where(x => x.ContractFile != null);

            return this.AddFilesToExport(exportData, x => x.ContractFile)
                .Select(x => new ExportableRow(x.ContractFile,
                    new List<string>
                    {
                        this.GetStrId(x.ContractFile),
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
                "Уникальный идентификатор договора"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "DOGPOI"
            };
        }
    }
}