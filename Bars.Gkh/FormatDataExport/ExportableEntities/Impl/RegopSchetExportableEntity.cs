namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// Расчетные счета фонда капитального ремонта
    /// </summary>
    public class RegopSchetExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "REGOPSCHET";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.RegOpCr;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<RegopSchetProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.TypeAccount.ToStr(),
                        x.RegopContragentId.ToStr(),
                        x.SpecialContragentId.ToStr(),
                        x.ContragentRschetId.ToStr(),
                        x.Status.ToStr()
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
                case 4:
                case 5:
                    return row.Cells[cell.Key].IsEmpty();
                case 2:
                case 3:
                    return row.Cells[2].IsEmpty() && row.Cells[3].IsEmpty();
            }
            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код записи",
                "Тип счета ",
                "Региональный оператор капитального ремонта",
                "Контрагент",
                "Расчетный счет ",
                "Статус"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(RegopExportableEntity),
                typeof(ContragentRschetExportableEntity));
        }
    }
}