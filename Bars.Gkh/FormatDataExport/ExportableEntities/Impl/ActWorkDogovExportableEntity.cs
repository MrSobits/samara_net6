namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Акты выполненных работ по  договору на выполнение работ по капитальному ремонту
    /// </summary>
    public class ActWorkDogovExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "ACTWORKDOGOV";

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<ActWorkDogovProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.DogovorPkrId.ToStr(),
                        x.Status.ToStr(),
                        x.Name.Cut(500),
                        x.Number.Cut(500),
                        this.GetDate(x.Date),
                        this.GetDecimal(x.Sum),
                        this.GetDecimal(x.ExecutantPenaltySum),
                        this.GetDecimal(x.CustomerPenaltySum),
                        x.IsSigned.ToStr(),
                        x.AgentSurname,
                        x.AgentName,
                        x.AgentPatronymic,
                        x.IsInstallments.ToStr()
                    }))
                .ToList();
        }

        /// <inheritdoc />
        protected override Func<KeyValuePair<int, string>, ExportableRow, bool> EmptyFieldPredicate { get; } = (cell, row) =>
        {
            switch (cell.Key)
            {
                case 10:
                case 11:
                case 12:
                case 13:
                    return row.Cells[9] == "1";
                default:
                    return row.Cells[cell.Key].IsEmpty();
            }
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код записи",
                "Код договора на выполнение работ по капитальному ремонту",
                "Статус",
                "Наименование акта",
                "Номер акта",
                "Дата акта",
                "Сумма акта по составу принятых работ",
                "Сумма штрафных санкций Исполнителю",
                "Сумма штрафных санкций Заказчику",
                "Акт подписан представителем собственников",
                "Фамилия представителя собственников",
                "Имя представителя собственников",
                "Отчество представителя собственников",
                "Рассрочка по оплате выполненных работ"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(DogovorPkrExportableEntity),
                typeof(ActWorkExportableEntity),
                typeof(ActWorkDogovFilesExportableEntity));
        }
    }
}