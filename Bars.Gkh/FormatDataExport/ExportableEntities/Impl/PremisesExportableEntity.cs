namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// Помещения
    /// </summary>
    public class PremisesExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "PREMISES";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.Rso |
            FormatDataExportProviderFlags.RegOpCr |
            FormatDataExportProviderFlags.Oms |
            FormatDataExportProviderFlags.Rc;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<PremisesProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.HouseId.ToStr(),
                        x.Type.ToStr(),
                        x.HasSeparateEntrace.ToStr(),
                        x.EntraceId.ToStr(),
                        x.Number,
                        x.IsCommonProperty.ToStr(),
                        x.TypeHouse.ToStr(),
                        this.GetDecimal(x.Area),
                        x.HasLivingArea.ToStr(),
                        this.GetDecimal(x.LivingArea),
                        x.HasCadastralHouseNumber.ToStr(),
                        x.CadastralHouseNumber,
                        x.EigrpNumber,
                        x.Floor.ToStr(),
                        this.GetDate(x.TerminataionDate),
                        x.NonLivingPremisesOtherInfo,
                        x.LivingPremisesOtherInfo,
                        x.HasRecognizedUnfit.ToStr(),
                        x.RecognizedUnfitReason,
                        this.GetDate(x.RecognizedUnfitDocDate),
                        x.RecognizedUnfitDocNumber,
                        x.IsSupplierConfirmed.ToStr(),
                        x.IsDeviceNotInstalled.ToStr(),
                        x.DeviceNotInstalledReason.ToStr()
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
                case 5:
                case 8:
                case 11:
                case 23:
                    return row.Cells[cell.Key].IsEmpty();
                case 4:
                    return row.Cells[3] == "2" && row.Cells[cell.Key].IsEmpty();
                case 6:
                    return row.Cells[2] == "2" && row.Cells[cell.Key].IsEmpty();
                case 7:
                case 9:
                    return row.Cells[2] == "1" && row.Cells[cell.Key].IsEmpty();
                case 10:
                    return row.Cells[9].IsEmpty() && row.Cells[cell.Key].IsEmpty();
                case 12:
                case 13:
                    return "1" == row.Cells[11] && row.Cells[12].IsEmpty() && row.Cells[13].IsEmpty();
                case 24:
                    return row.Cells[23] == "1" && row.Cells[cell.Key].IsEmpty();
            }
            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код помещения в системе отправителя",
                "Уникальный идентификатор дома",
                "Тип помещения",
                "Помещение имеет отдельный вход",
                "Уникальный идентификатор подъезда",
                "Номер помещения",
                "Нежилое помещение является общим имуществом в МКД",
                "Характеристика жилого помещения",
                "Общая площадь помещения по паспорту помещения",
                "Наличие сведений о жилой площади помещения",
                "Жилая площадь помещения по паспорту помещения",
                "Наличие сведений о кадастровом номере или условном номере дома в ЕГРП/ГКН",
                "Кадастровый номер",
                "Условный номер ЕГРП",
                "Этаж",
                "Дата прекращения существования объекта",
                "Иные характеристики нежилого помещения",
                "Иные характеристики квартиры",
                "Наличие факта признания квартиры непригодной для проживания",
                "Основание признания квартиры непригодной для проживания",
                "Дата документа, содержащего решение о признании квартиры непригодной для проживания",
                "Номер документа, содержащего решение о признании квартиры непригодной для проживания",
                "Информация подтверждена поставщиком, ответственным за размещение сведений",
                "Отсутствует установленный ПУ (ИПУ или Общий (квартирный) ПУ)",
                "Причина отсутствия ПУ"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(HouseExportableEntity),
                typeof(EntranceExportableEntity));
        }
    }
}