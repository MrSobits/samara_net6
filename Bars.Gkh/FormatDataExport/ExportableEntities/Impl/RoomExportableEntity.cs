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
    /// Комнаты
    /// </summary>
    [Obsolete("Не выгружаем")]
    public class RoomExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "ROOM";

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
            return null;
            return this.ProxySelectorFactory.GetSelector<RoomProxy>().ExtProxyListCache
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.RealityObjectId.ToStr(),
                        x.PremisesId.ToStr(), // 3. Уникальный идентификатор Помещения
                        x.IsCommunalRoom ? this.Yes : this.No, // 4. Признак комнаты коммунального заселения
                        x.CadastralHouseNumber.Cut(40), // 5. Кадастровый номер в ГКН
                        string.Empty, // 6. Условный номер ЕГРП
                        x.ChamberNum.Cut(255),
                        this.GetNotZeroValue(x.Area),
                        string.Empty, // 9. Наличие факта признания комнаты непригодной для проживания
                        string.Empty, // 10. Дата документа, содержащего решение о признании комнаты непригодной для проживания
                        string.Empty, // 11. Номер документа, содержащего решение о признании комнаты непригодной для проживания
                        string.Empty, // 12. Иные характеристики комнаты
                        string.Empty // 13. Дата прекращения существования объекта
                    }))
                .ToList();
        }

        /// <inheritdoc />
        protected override Func<KeyValuePair<int, string>, ExportableRow, bool> EmptyFieldPredicate { get; } = (cell, row) =>
        {
            switch (cell.Key)
            {
                case 0:
                case 3:
                case 6:
                case 7:
                    return row.Cells[cell.Key].IsEmpty();
                case 1:
                case 2:
                    return row.Cells[1].IsEmpty() && row.Cells[2].IsEmpty();
                case 4:
                case 5:
                    return row.Cells[4].IsEmpty() && row.Cells[5].IsEmpty();
            }
            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код комнаты в системе отправителя",
                "Уникальный идентификатор  Дома",
                "Уникальный идентификатор Помещения",
                "Признак комнаты коммунального заселения",
                "Кадастровый номер в ГКН",
                "Условный номер ЕГРП",
                "Номер комнаты",
                "Площадь комнаты",
                "Наличие факта признания комнаты непригодной для проживания",
                "Дата документа, содержащего решение о признании комнаты непригодной для проживания",
                "Номер документа, содержащего решение о признании комнаты непригодной для проживания",
                "Иные характеристики комнаты",
                "Дата прекращения существования объекта"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(HouseExportableEntity),
                typeof(PremisesExportableEntity));
        }
    }
}