namespace Bars.GkhGji.Regions.Stavropol.FormatDataExport.ExportableEntities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Impl;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// Решения по капитальному ремонту (kapremdecisions.csv)
    /// </summary>
    public class KapremDecisionsExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "KAPREMDECISIONS";

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            var data = this.ProxySelectorFactory.GetSelector<KapremDecisionsProxy>()
                .ExtProxyListCache
                .GroupBy(x => x.HouseId)
                .ToDictionary(x => x.Key, y => y.OrderByDescending(x => x.StartDate).ToList());

            return data.SelectMany(x =>
                {
                    x.Value[0].State = 1;
                    return x.Value;
                })
                .Select(x => new ExportableRow(x,
                    new List<string>
                    {
                        this.GetStrId(x),
                        x.HouseId.ToStr(),
                        x.State.ToStr(),
                        x.BasisReason.ToStr(),
                        x.Type.ToStr(),
                        this.GetDate(x.StartDate),
                        x.FundType.ToStr(),
                        this.GetDecimal(x.CrPayment),
                        x.ProtocolId.ToStr(),
                        x.ProtocolNumber,
                        this.GetDate(x.ProtocolDate),
                        x.DocName,
                        x.DocKind,
                        x.DocNumber,
                        this.GetDate(x.DocDate),
                        string.Empty,
                        x.RegopSchetId.HasValue ? x.RegopSchetId.ToStr() : x.ContragentRschetId.ToStr()
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
                    return row.Cells[cell.Key].IsEmpty();
                case 6:
                    return row.Cells[4] == "1" && row.Cells[cell.Key].IsEmpty(); // Да, если Тип решения=1
                case 7:
                    return row.Cells[4] == "3" && row.Cells[cell.Key].IsEmpty(); // Да, если Тип решения=3
                case 8:
                case 9:
                case 10:
                    return row.Cells[3] == "1" && row.Cells[cell.Key].IsEmpty(); // Основание принятия решения=1-Решение собственников
                case 11:
                case 12:
                case 13:
                case 14:
                    return row.Cells[3] == "2" && row.Cells[cell.Key].IsEmpty(); // Основание принятия решения=2-Решение ОМС
                case 16:
                    return row.Cells[6] == "1" && row.Cells[cell.Key].IsEmpty(); // Способ формирования фонда КР на специальном счете
            }

            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код решения",
                "Адрес дома",
                "Статус",
                "Основание принятия решения",
                "Тип решения",
                "Дата вступления в силу",
                "Способ формирования фонда",
                "Размер превышения взноса на КР",
                "Протокол ОСС",
                "Номер протокола",
                "Дата протокола",
                "Наименование документа решения",
                "Вид документа",
                "Номер документа",
                "Дата документа",
                "НПА",
                "Расчетный счет"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(HouseExportableEntity),
                typeof(ProtocolossExportableEntity),
                typeof(ContragentRschetExportableEntity),
                typeof(RegopSchetExportableEntity));
        }
    }
}