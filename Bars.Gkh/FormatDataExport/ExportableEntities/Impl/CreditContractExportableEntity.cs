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
    ///  Кредитные договоры/договоры займа (creditcontract.csv)
    /// </summary>
    public class CreditContractExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "CREDITCONTRACT";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.RegOpCr;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<CreditContractProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.Type.ToStr(),
                        x.Creditor.ToStr(),
                        x.Number,
                        this.GetDate(x.Date),
                        x.IsUnlimited.ToStr(),
                        x.CountMounthPeriod.ToStr(),
                        x.IsNoPercent.ToStr(),
                        this.GetDecimal(x.PercentRate),
                        x.NotationToPercentRate,
                        this.GetDecimal(x.Amount),
                        x.NotationToSize,
                        x.State.ToStr(),
                        x.Reason,
                        x.DocNumber,
                        this.GetDate(x.DocDate)
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
                case 3:
                case 4:
                case 5:
                case 7:
                case 10:
                case 12:
                    return row.Cells[cell.Key].IsEmpty();
                case 6:
                    return row.Cells[5] == "2" && row.Cells[cell.Key].IsEmpty();
                case 8:
                    return row.Cells[7] == "2" && row.Cells[cell.Key].IsEmpty();
                case 13:
                case 15:
                    return row.Cells[12] == "2" && row.Cells[cell.Key].IsEmpty();

            }
            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный идентификатор договора",
                "Тип договора",
                "Кредитор / Займодатель",
                "Номер договора",
                "Дата договора",
                "Договор бессрочный",
                "Срок договора в месяцах",
                "Договор беспроцентный",
                "Процентная ставка",
                "Примечание к процентной ставке",
                "Размер договора",
                "Примечание к размеру договора",
                "Статус договора",
                "Причина",
                "Номер документа",
                "Дата документа"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(ContragentExportableEntity), typeof(CreditContractFilesExportableEntity));
        }
    }
}