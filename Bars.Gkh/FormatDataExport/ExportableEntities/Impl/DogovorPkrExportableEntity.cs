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
    /// Договоры на выполнение работ по капитальному ремонту
    /// </summary>
    public class DogovorPkrExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "DOGOVORPKR";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.RegOpCr;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<DogovorPkrProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.PkrId.ToStr(),
                        x.DocumentNumber.Cut(500),
                        this.GetDate(x.DocumentDate),
                        this.GetDate(x.StartDate),
                        this.GetDate(x.EndDate),
                        this.GetDecimal(x.Sum),
                        x.IsCustomer.ToStr(),
                        x.CustomerSurname,
                        x.CustomerName,
                        x.CustomerPatronymic,
                        x.CustomerContragentId.ToStr(),
                        x.ExecutantContragentId.ToStr(),
                        x.IsGuaranteePeriod.ToStr(),
                        x.GuaranteePeriod.ToStr(),
                        x.IsBudgetDocumentation.ToStr(),
                        x.IsLawProvided.ToStr(),
                        x.InfoUrl.Cut(100),
                        x.Status.ToStr(),
                        x.RevocationReason.ToStr(),
                        x.RevocationDocumentNumber.Cut(500),
                        this.GetDate(x.RevocationDate)
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
                case 7:
                case 12:
                case 13:
                case 15:
                case 16:
                case 18:
                    return row.Cells[cell.Key].IsEmpty();
                case 14:
                    return row.Cells[13] == "1" && row.Cells[cell.Key].IsEmpty();
                case 17:
                    return row.Cells[16] == "1" && row.Cells[cell.Key].IsEmpty();
                case 19:
                case 20:
                case 21:
                    return row.Cells[18] == "2" && row.Cells[cell.Key].IsEmpty();

            }
            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код",
                "Идентификатор ПКР",
                "Номер договора",
                "Дата договора",
                "Дата начала выполнения",
                "Дата окончания выполнения работ",
                "Сумма договора",
                "Заказчиком является",
                "Фамилия заказчика",
                "Имя заказчика",
                "Отчество заказчика",
                "Заказчик - Организация",
                "Исполнитель",
                "Гарантийный срок установлен",
                "Гарантийный срок (кол-во месяцев)",
                "Наличие сметной документации",
                "Проведение отбора предусмотрено законодательством",
                "Адрес сайта с информацией об отборе",
                "Статус договора",
                "Причина расторжения",
                "Номер документа расторжения",
                "Дата документа расторжения"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(PkrExportableEntity), typeof(ContragentExportableEntity));
        }
    }
}