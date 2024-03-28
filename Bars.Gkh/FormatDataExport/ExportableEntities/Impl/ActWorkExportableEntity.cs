namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// Работы акта выполненных работ по  договору на выполнение работ по капитальному ремонту(actwork.csv)
    /// </summary>
    public class ActWorkExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "ACTWORK";

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<ActWorkDogovProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.WorkDogovId.ToStr(),
                        this.GetDecimal(x.Cost),
                        this.GetDecimal(x.Volum),
                        x.ExploitationAccepted.ToStr(),
                        this.GetDate(x.WarrantyStartDate),
                        this.GetDate(x.WarrantyEndDate)
                    }))
                .ToList();
        }

        /// <inheritdoc />
        protected override Func<KeyValuePair<int, string>, ExportableRow, bool> EmptyFieldPredicate { get; } = (cell, row) =>
        {
            switch (cell.Key)
            {
                case 5:
                case 6:
                    return row.Cells[4] == "1";
                default:
                    return row.Cells[cell.Key].IsEmpty();
            }
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный идентификатор акта",
                "Уникальный идентификатор работы КПР",
                "Стоимость работ",
                "Объем работ",
                "Принята в эксплуатацию",
                "Дата начала гарантийного срока",
                "Дата окончания гарантийного срока"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(WorkDogovExportableEntity));
        }
    }
}