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
    /// Начисления по капитальному ремонту
    /// </summary>
    public class EpdCapitalExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "EPDCAPITAL";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags => FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.RegOpCr |
            FormatDataExportProviderFlags.Ogv |
            FormatDataExportProviderFlags.Rc;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<EpdProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x,
                    new List<string>
                    {
                        x.SnapshotIdCapital.ToStr(),
                        x.Id.ToStr(),
                        this.GetDecimal(x.Tariff),
                        this.GetDecimal(x.ChargeCapital),
                        this.GetDecimal(x.Correction),
                        this.GetDecimal(x.BenefitCapital),
                        this.GetDecimal(x.SaldoOutCapital),
                        x.ContragentId.ToStr(),
                        x.EpdCapitalParam9,
                        x.EpdCapitalState
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
                case 9:
                    return row.Cells[cell.Key].IsEmpty();
            }
            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код начисления",
                "Платежный документ",
                "Размер взноса на кв. метр (руб.)",
                "Всего начислено за расчётный период (руб.)",
                "Перерасчеты, корректировки (руб.)",
                "Льготы, субсидии, скидки (руб.)",
                "Итого к оплате за расчётный период (руб.)",
                "Поставщик Услуги",
                "Порядок расчетов",
                "Статус записи"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(EpdExportableEntity));
        }
    }
}