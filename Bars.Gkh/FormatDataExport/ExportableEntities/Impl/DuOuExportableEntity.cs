namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// Объекты управления договора управления
    /// </summary>
    public class DuOuExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "DUOU";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            var cache = this.ProxySelectorFactory.GetSelector<DuProxy>()
                .ExtProxyListCache;
            // Раскомментировать как только будут передаваться
            //this.AddFilesToExport(cache, x => x.AttachmentFile);
            //this.AddFilesToExport(cache, x => x.TerminationFile);

            return cache
                .Select(x => new ExportableRow(x,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.Id.ToStr(),
                        x.RealityObjectId.ToStr(),
                        x.Id.ToStr(),
                        this.GetDate(x.StartDate),
                        this.GetDate(x.EndDate),
                        x.IsContractReason.ToStr(),
                        this.GetStrId(x.AttachmentFile),
                        x.StatusDu.ToStr(),
                        x.ReasonTermination.ToStr(),
                        this.GetStrId(x.TerminationFile),
                        this.GetDate(x.ExceptionManagementDate)
                    }))
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
                case 4:
                case 6:
                case 8:
                    return row.Cells[cell.Key].IsEmpty();
                case 7:
                    return row.Cells[6] == "2" && row.Cells[cell.Key].IsEmpty();
                case 9:
                case 11:
                    return row.Cells[8] == "2" && row.Cells[cell.Key].IsEmpty();
                case 10:
                    return row.Cells[6] == "2" && row.Cells[8] == "2" && row.Cells[cell.Key].IsEmpty();
            }
            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код",
                "Договор управления",
                "Дом",
                "Номер лота",
                "Дата начала предоставления услуг дому",
                "Дата окончания предоставления услуг дому",
                "Основанием является",
                "Файл с дополнительным соглашением",
                "Статус объекта управления ДУ",
                "Основание исключения",
                "Файл с дополнительным соглашением",
                "Дата исключения объекта управления из ДУ"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(DuExportableEntity),
                typeof(HouseExportableEntity));
        }
    }
}