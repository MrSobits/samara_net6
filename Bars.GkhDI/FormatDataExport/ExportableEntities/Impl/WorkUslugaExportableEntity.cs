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
    /// Справочник работ/услуг
    /// </summary>
    public class WorkUslugaExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "WORKUSLUGA";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.Ogv |
            FormatDataExportProviderFlags.Oms;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<WorkUslugaProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.Name.Cut(150),
                        x.BaseService.ToStr(),
                        x.Type.ToStr(),
                        x.OkeiCode.Cut(50),
                        x.AnotherUnit.Cut(100),
                        x.ParentServiceId.ToStr()
                    }))
                .ToList();
        }

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код работы/услуги",
                "Наименование работы/услуги",
                "Базовая работа/услуга организации",
                "Вид работ",
                "Код ОКЕИ",
                "Другая единица измерения",
                "Родительская работа/услуга"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "DICTMEASURE"
            };
        }

        /// <inheritdoc />
        protected override Func<KeyValuePair<int, string>, ExportableRow, bool> EmptyFieldPredicate { get; } = (cell, row) =>
        {
            switch (cell.Key)
            {
                case 0:
                case 1:
                case 3:
                    return row.Cells[cell.Key].IsEmpty();
                case 4:
                case 5:
                    return row.Cells[4].IsEmpty() && row.Cells[5].IsEmpty();
            }
            return false;
        };
    }
}