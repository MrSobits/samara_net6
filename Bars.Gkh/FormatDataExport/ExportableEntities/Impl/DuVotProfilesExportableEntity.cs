namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// Файлы к уставу
    /// </summary>
    public class DuvotproFilesExportableEntity : BaseExportableEntity
    {
        private int id = 1;

        /// <inheritdoc />
        public override string Code => "DUVOTPROFILES";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            var withoutAttachment = this.SelectParams.GetAs("WithoutAttachment", false);

            if (withoutAttachment)
            {
                return new List<ExportableRow>();
            }

            return this.GetFiles(x => x.ProtocolOwnersMeetingFile, 1)
                .Union(this.GetFiles(x => x.ProtocolCompetitionFile, 2))
                .ToList();
        }

        /// <inheritdoc />
        protected override IList<int> MandatoryFields => this.GetAllFieldIds();

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Договор управления",
                "Идентификатор файла",
                "Тип документа решения"
            };
        }

        private IEnumerable<ExportableRow> GetFiles(Func<DuProxy, FileInfo> fileSelector, int type)
        {
            var ustavs = this.ProxySelectorFactory.GetSelector<DuProxy>().ExtProxyListCache;

            return this.AddFilesToExport(ustavs.Where(x => fileSelector(x) != null), fileSelector)
                .Select(x => new ExportableRow(this.id++,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        this.GetStrId(fileSelector(x)),
                        type.ToString()
                    }));
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(DuExportableEntity));
        }
    }
}