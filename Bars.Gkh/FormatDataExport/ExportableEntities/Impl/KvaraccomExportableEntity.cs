namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Impl;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// Связь помещений плательщиков с лицевыми счетами
    /// </summary>
    public class KvaraccomExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "KVARACCOM";

        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.Rso |
            FormatDataExportProviderFlags.RegOpCr |
            FormatDataExportProviderFlags.Rc;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<KvarProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(), // 1.	Уникальный код лицевого счета в системе отправителя
                        x.PremisesId.ToStr(), // 2. Уникальный идентификатор помещения
                        x.RoId.ToStr(), // 3. Уникальный код дома
                        x.RoomId.ToStr(), // 4. Уникальный идентификатор комнаты
                        this.GetDecimal(x.Share) // 5. Доля внесения платы, размер доли в %
                    }))
                .ToList();
        }

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код лицевого счета в системе отправителя",
                "Уникальный идентификатор помещения",
                "Уникальный код дома",
                "Уникальный идентификатор комнаты",
                "Доля внесения платы, размер доли в %"
            };
        }

        /// <inheritdoc />
        protected override Func<KeyValuePair<int, string>, ExportableRow, bool> EmptyFieldPredicate { get; } = (cell, row) =>
        {
            switch (cell.Key)
            {
                case 0:
                    return row.Cells[cell.Key].IsEmpty();
                case 1:
                case 2:
                case 3:
                    return row.Cells[1].IsEmpty() && row.Cells[2].IsEmpty() && row.Cells[3].IsEmpty();
            }
            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(PremisesExportableEntity));
        }
    }
}
