namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// Нормативные правовые акты
    /// </summary>
    public class NpaExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "NPA";

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.AddFilesToExport(this.ProxySelectorFactory
                        .GetSelector<NpaProxy>()
                        .ExtProxyListCache,
                    x => x.File)
                .Select(x => new ExportableRow(x,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.AuthLevel.ToStr(),
                        x.Name,
                        this.GetDate(x.DocumentDate),
                        x.Number,
                        x.AuthName,
                        x.InfoType.ToStr(),
                        x.ActType.ToStr(),
                        x.ActKind.ToStr(),
                        x.ContragentId.ToStr(),
                        this.GetDate(x.StartDate),
                        this.GetDate(x.EndDate),
                        x.Subject.ToStr(),
                        x.IsThroughoutTerritoryValid.ToStr(),
                        x.File?.Id.ToStr(),
                        x.Status.ToStr(),
                        x.TerminationReason
                    }))
                .ToList();
        }

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код",
                "Уровень органа власти",
                "Наименование",
                "Дата принятия документа",
                "Номер",
                "Наименование принявшего акт органа",
                "Тип информации в НПА",
                "Тип НПА",
                "Вид нормативного акта",
                "Орган принявший НПА",
                "Дата вступления в силу",
                "Дата утраты силы",
                "Субъект РФ",
                "Документ действует на всей территории субъекта РФ",
                "Файл вложение",
                "Статус записи",
                "Причина аннулирования"
            };
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
                case 6:
                case 7:
                case 8:
                case 14:
                case 15:
                    return row.Cells[cell.Key].IsEmpty();
                case 16:
                    return row.Cells[15] == "3" && row.Cells[cell.Key].IsEmpty();
            }
            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(ContragentExportableEntity));
        }
    }
}