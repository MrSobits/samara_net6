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
    using Bars.Gkh.Utils;

    /// <summary>
    /// Размещение размера платы за жилое помещение по договору управления
    /// </summary>
    public class DuChargeExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "DUCHARGE";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.Oms;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<DuProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.ChargeStatus.ToStr(),
                        x.Id.ToStr(),
                        this.GetDate(x.StartDatePaymentPeriod),
                        this.GetDate(x.EndDatePaymentPeriod),
                        this.GetDecimal(x.PaymentAmount),
                        x.SetPaymentsFoundation.ToStr(),
                        x.ChargeRevocationReason.Cut(1000),
                        x.Id.ToStr(),
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
                case 3:
                case 4:
                case 5:
                case 6:
                    return row.Cells[cell.Key].IsEmpty();
                case 7:
                    if (row.Cells[1] == "2")
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
                "Уникальный код записи",
                "Статус",
                "Договор управления",
                "Дата начала периода",
                "Дата окончания периода",
                "Цена за услуги, работы по управлению МКД",
                "Тип размера платы",
                "Причина аннулирования",
                "Объект управления ДУ"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "DU",
                "DICTUSLUGA"
            };
        }
    }
}