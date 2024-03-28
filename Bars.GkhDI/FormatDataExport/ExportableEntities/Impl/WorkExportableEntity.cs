namespace Bars.GkhDi.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Работы/услуги перечня
    /// </summary>
    public class WorkExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "WORK";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.Ogv |
            FormatDataExportProviderFlags.Oms;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<WorkListProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.Id.ToStr(),
                        this.GetDecimal(x.Cost),
                        this.GetDecimal(x.Volume),
                        x.Count.ToStr().Cut(3),
                        this.GetDecimal(x.Summary),
                        x.DictUslugaId.ToStr()
                    }))
                .ToList();
        }

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код",
                "Перечень работ/услуг на период",
                "Цена",
                "Объём",
                "Количество",
                "Общая стоимость",
                "Работа/услуга организации"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "WORKLIST",
                "WORKUSLUGA"
            };
        }

        /// <inheritdoc />
        protected override Func<KeyValuePair<int, string>, ExportableRow, bool> EmptyFieldPredicate { get; } = (cell, row) =>
        {
            switch (cell.Key)
            {
                case 0:
                case 1:
                case 6:
                    return row.Cells[cell.Key].IsEmpty();
                case 2:
                case 3:
                case 4:
                    return row.Cells[cell.Key].IsEmpty() && row.Cells[5].IsEmpty();
                case 5:
                    return row.Cells[2].IsEmpty() && row.Cells[3].IsEmpty() && row.Cells[4].IsEmpty() && row.Cells[5].IsEmpty();
            }
            return false;
        };
    }
}