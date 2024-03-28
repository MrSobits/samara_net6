namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Справочник услуг
    /// </summary>
    public class DictUslugaExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "DICTUSLUGA";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.Rso |
            FormatDataExportProviderFlags.Rc;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<DictUslugaProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(), // 1. Уникальный код услуги в системе отправителя
                        x.Name.Cut(150), // 2. Наименование услуги
                        x.ShortName.Cut(60), // 3. Краткое наименование услуги
                        x.Type.ToStr(), // 4. Тип услуги
                        x.ServiceType.ToStr(), // 5. Тип предоставления услуги
                        x.BaseCode.ToStr(), // 6. Код базовой услуги
                        x.CommunalServiceType.ToStr(), // 7. Вид коммунальной услуги
                        x.HousingServiceType.ToStr(), // 8. Вид жилищной услуги
                        x.ElectricMeteringType.ToStr(), // 9. Тип учета электроэнергии
                        x.OkeiCode.Cut(50), // 10. Код ОКЕИ
                        x.AnotherUnit.Cut(100), // 11. Другая единица измерения
                        x.CommunalResourceType.ToStr(), // 12. Вид коммунального ресурса
                        x.SortOrder.ToStr().Cut(3), // 13. Порядок сортировки
                        x.IsNotSortOrder.ToStr(), // 14. Порядок сортировки не задан
                    }))
                .ToList();
        }

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код услуги в системе отправителя",
                "Наименование услуги",
                "Краткое наименование услуги",
                "Тип услуги",
                "Тип предоставления услуги",
                "Код базовой услуги",
                "Вид коммунальной услуги",
                "Вид жилищной услуги",
                "Тип учета электроэнергии",
                "Код ОКЕИ",
                "Другая единица измерения",
                "Вид коммунального ресурса",
                "Порядок сортировки",
                "Порядок сортировки не задан"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "DICTMEASURE"
            };
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
                    return row.Cells[cell.Key].IsEmpty();
                case 6:
                case 11:
                    if (row.Cells[3] == "1")
                    {
                        return row.Cells[cell.Key].IsEmpty();
                    }
                    break;
                case 7:
                    if (row.Cells[3] == "2")
                    {
                        return row.Cells[cell.Key].IsEmpty();
                    }
                    break;
                case 8:
                    if (row.Cells[1].ToLower() == "электроснабжение")
                    {
                        return row.Cells[cell.Key].IsEmpty();
                    }
                    break;
                case 9:
                case 10:
                    return row.Cells[9].IsEmpty() && row.Cells[10].IsEmpty();
                case 12:
                case 13:
                    if (row.Cells[3] == "1")
                    {
                        return row.Cells[12].IsEmpty() && row.Cells[13].IsEmpty();
                    }
                    break;
            }
            return false;
        };
    }
}