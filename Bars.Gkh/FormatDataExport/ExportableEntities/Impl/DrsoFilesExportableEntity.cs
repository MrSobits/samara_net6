namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.Modules.Gkh1468.Entities;

    /// <summary>
    /// Файлы договора ресурсоснабжения
    /// </summary>
    public class DrsoFilesExportableEntity : BaseExportableEntity<PublicServiceOrgContract>
    {
        /// <inheritdoc />
        public override string Code => "DRSOFILES";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Rso;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            var entityList = this.GetFiltred(x => x.PublicServiceOrg.Contragent)
                .Where(x => x.FileInfo != null)
                .Select(x => new
                {
                    x.Id,
                    x.FileInfo
                })
                .ToList();

            return this.AddFilesToExport(entityList, x => x.FileInfo)
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.FileInfo.Id.ToStr(),
                        x.Id.ToStr(),
                        this.Yes
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
                "Уникальный идентификатор договора ресурсоснабжения",
                "Тип файла"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "DRSO"
            };
        }
    }
}