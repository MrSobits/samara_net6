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
    /// Работы по дому  (pkrdomwork.csv)
    /// </summary>
    public class PkrDomWorkExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "PKRDOMWORK";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Ogv;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<PkrDomWorkProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.PkrDomId.ToStr(),
                        x.WorkKprTypeId.ToStr(),
                        this.GetDate(x.StartDate),
                        this.GetDate(x.EndDate),
                        this.GetDecimal(x.FundResourses),
                        this.GetDecimal(x.SubjectBudget),
                        this.GetDecimal(x.LocalBudget),
                        this.GetDecimal(x.OwnerBudget),
                        this.GetDecimal(x.UnitCost),
                        this.GetDecimal(x.MarginalCost),
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
                case 4:
                    return row.Cells[cell.Key].IsEmpty();
                case 5:
                case 6:
                case 7:
                case 8:
                    return row.Cells[3].IsEmpty() && row.Cells[cell.Key].IsEmpty();
            }
            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Код работы по дому",
                "Идентификатор дома в программе",
                "Вид работы капитального ремонта",
                "Начало выполнения работ",
                "Окончание выполнения работ",
                "Средства Фонда ЖКХ",
                "Бюджет субъекта РФ",
                "Местный бюджет",
                "Средства собственников",
                "Удельная стоимость работы",
                "Предельная стоимость работы"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(PkrDomExportableEntity), typeof(WorkKprTypeExportableEntity));
        }
    }
}