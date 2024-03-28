namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Паспорт дома
    /// </summary>
    public class HouseDocExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "HOUSEDOC";

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<HouseDocProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.HouseId.ToStr(),
                        x.Code,
                        x.TextValue.Cut(255),
                        this.GetDecimal(x.DecimalValue),
                        this.GetDate(x.DateValue),
                        x.IntValue.ToStr(),
                        x.BoolValue == null ?
                            string.Empty
                            : (bool)x.BoolValue
                                ? this.Yes : this.No,
                        x.DictValue,
                        x.FileValue.ToStr()
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
                    return row.Cells[cell.Key].IsEmpty();
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                    return row.Cells[3].IsEmpty()
                        && row.Cells[4].IsEmpty()
                        && row.Cells[5].IsEmpty()
                        && row.Cells[6].IsEmpty()
                        && row.Cells[7].IsEmpty()
                        && row.Cells[8].IsEmpty()
                        && row.Cells[9].IsEmpty();
            }
            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код записи",
                "Уникальный код дома в системе отправителя",
                "Код параметра",
                "Значение Параметра (Текст)",
                "Значение Параметра (Число)",
                "Значение Параметра (Дата)",
                "Значение Параметра (Целое)",
                "Значение Параметра (Логическое Да / Нет)",
                "Значение Параметра (Справочное)",
                "Значение Параметра (Файл)"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(HouseExportableEntity));
            //todo секция файлы также должна быть выгружена
        }
    }
}