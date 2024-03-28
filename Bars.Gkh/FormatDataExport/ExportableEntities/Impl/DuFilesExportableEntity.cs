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
    /// Файлы к договору управления
    /// </summary>
    public class DuFilesExportableEntity : BaseExportableEntity
    {
        private int id = 1;

        /// <inheritdoc />
        public override string Code => "DUFILES";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.Oms;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            var withoutAttachment = this.SelectParams.GetAs("WithoutAttachment", false);

            if (withoutAttachment)
            {
                return new List<ExportableRow>();
            }

            return this.GetFiles(x => x.DuFile, 1)
                .Union(this.GetFiles(x => x.TerminationFile, 3))
                .Union(this.GetFiles(x => x.OwnerFile, 4))
                .ToList();
        }

        /// <inheritdoc />
        protected override Func<KeyValuePair<int, string>, ExportableRow, bool> EmptyFieldPredicate { get; } = (cell, row) =>
        {
            switch (cell.Key)
            {
                case 0:
                case 1:
                case 2:
                    return row.Cells[cell.Key].IsEmpty();
                case 3:
                case 4:
                    if (row.Cells[2] == "2")
                    {
                        return row.Cells[cell.Key].IsEmpty();
                    }
                    break;
            }
            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный идентификатор файла",
                "Уникальный идентификатор договора управления",
                "Тип файла",
                "Номер дополнительного соглашения",
                "Дата дополнительного соглашения"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "DU"
            };
        }

        private IEnumerable<ExportableRow> GetFiles(Func<DuProxy, FileInfo> fileSelector, int type)
        {
            var contracts = this.ProxySelectorFactory.GetSelector<DuProxy>().ExtProxyListCache;
            return this.AddFilesToExport(contracts.Where(x => fileSelector(x) != null), fileSelector)
                .Select(x => new ExportableRow(this.id++,
                    new List<string>
                    {
                        this.GetStrId(fileSelector(x)),
                        x.Id.ToStr(),
                        type.ToString(),
                        string.Empty,
                        string.Empty
                    }));
        }
    }
}