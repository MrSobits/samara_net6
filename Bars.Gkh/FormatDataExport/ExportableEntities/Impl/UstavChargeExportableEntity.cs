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
    /// Размещение сведений о размера платы за жилое помещение по уставу
    /// </summary>
    public class UstavChargeExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "USTAVCHARGE";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<UstavProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.Id.ToStr(),
                        x.ChargeStatus.ToStr(),
                        this.GetDate(x.StartDatePaymentPeriod),
                        this.GetDate(x.EndDatePaymentPeriod),
                        this.GetDecimal(x.CompanyReqiredPaymentAmount),
                        this.GetDecimal(x.ReqiredPaymentAmount),
                        x.ForAllManagedObjects.ToStr(),
                        x.ControlObject.ToStr()
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
                case 7:
                    return row.Cells[cell.Key].IsEmpty();
                case 8:
                    if (row.Cells[7] == "2")
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
                "Уникальный код",
                "Устав",
                "Статус",
                "Дата начала периода",
                "Дата окончания периода",
                "Размер обязательных платежей членов ТСЖ, кооператива",
                "Размер платы за содержание и ремонт помещений",
                "Для всех управляемых объектов",
                "Объект управления устава"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(UstavExportableEntity));
        }
    }
}