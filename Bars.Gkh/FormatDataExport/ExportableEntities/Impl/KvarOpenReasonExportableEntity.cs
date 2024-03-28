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
    /// Конкретизация оснований открытия лицевого счета
    /// </summary>
    public class KvarOpenReasonExportableEntity : BaseExportableEntity
    {
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.Rso |
            FormatDataExportProviderFlags.RegOpCr |
            FormatDataExportProviderFlags.Ogv |
            FormatDataExportProviderFlags.Rc;

        /// <inheritdoc />
        public override string Code => "KVAROPENREASON";

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<KvarProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.ReasonType.ToStr(),
                        x.DuId.ToStr(),
                        x.UstavId.ToStr(),
                        x.KapremDecisionId.ToStr(),
                        x.Param6,
                        x.Param7,
                        x.Param8,
                        x.Param9,
                        x.Param10
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
                    return row.Cells[cell.Key].IsEmpty();
                case 4:
                    return (row.Cells[1] == "5" || row.Cells[1] == "6") && row.Cells[cell.Key].IsEmpty(); // Тип основания открытия ЛС = 5 или 6
            }

            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Лицевой счет",
                "Тип основания открытия ЛС",
                "Договор управления",
                "Устав",
                "Решение о выбранном способе формирования фонда КР",
                "Договор найма отсутствует в системе отправителя",
                "Договор найма жилого помещения",
                "Номер договора найма жилого помещения",
                "Дата заключения договора найма жилого помещения",
                "Тип договора найма жилого помещения"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(DuExportableEntity),
                typeof(UstavExportableEntity),
                typeof(KapremDecisionsExportableEntity));
        }
    }
}