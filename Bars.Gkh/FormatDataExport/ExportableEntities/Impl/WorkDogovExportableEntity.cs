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
    /// Работы договора на выполнение работ по капитальному ремонту
    /// </summary>
    public class WorkDogovExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "WORKDOGOV";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.RegOpCr;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<WorkDogovProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.DogovorPkrId.ToStr(),
                        x.IsHouseWork.ToStr(),
                        x.PkrDomWorkId.ToStr(),
                        x.HouseId.ToStr(),
                        x.TypeWorkId.ToStr(),
                        this.GetDate(x.WorkEndDate),
                        this.GetDate(x.StartDate),
                        this.GetDate(x.EndDate),
                        this.GetDecimal(x.ContractAmount),
                        this.GetDecimal(x.KprAmount),
                        this.GetDecimal(x.WorkVolume),
                        x.Okei,
                        x.AnotherUnit.Cut(50),
                        x.Description.Cut(500)
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
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                    return row.Cells[cell.Key].IsEmpty();
                case 3:
                    return row.Cells[2] == "1" && row.Cells[cell.Key].IsEmpty();
                case 4:
                case 5:
                case 6:
                    return row.Cells[2] == "2" && row.Cells[cell.Key].IsEmpty();
                case 12:
                case 13:
                    return row.Cells[12] == "2" && row.Cells[13].IsEmpty();
            }
            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код работы",
                "Идентификатор договорана выполнение работ по капитальному ремонту",
                "Работа по дому",
                "Идентификатор работы по дому",
                "Многоквартирный дом",
                "Вид работ капитального ремонта",
                "Месяц, год окончания работ в ПКР",
                "Дата начала выполнения работы",
                "Дата окончания выполнения работы",
                "Стоимость работы в договоре",
                "Стоимость работы в ПКР",
                "Объём работы",
                "Код ОКЕИ",
                "Другая единица измерения",
                "Дополнительная информация"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(DogovorPkrExportableEntity),
                typeof(HouseExportableEntity),
                typeof(PkrDomWorkExportableEntity));
        }
    }
}