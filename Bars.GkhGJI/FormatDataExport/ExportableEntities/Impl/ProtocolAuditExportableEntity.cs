namespace Bars.GkhGji.FormatDataExport.ExportableEntities.Impl
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
    using Bars.Gkh.Utils;

    /// <summary>
    /// Протоколы проверки
    /// </summary>
    public class ProtocolAuditExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "PROTOCOLAUDIT";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Gji |
            FormatDataExportProviderFlags.Omjk;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<ProtocolAuditProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(), // 1. Уникальный код
                        x.AuditId.ToStr(), // 2. Проверка
                        x.State.ToStr(), // 3. Статус протокола
                        x.DocumentNumber.Cut(255), // 4. Номер документа
                        this.GetDate(x.DocumentDate), // 5. Дата документа
                        x.Info.Cut(2000), // 6.  Краткая информация
                        x.ExecutionState.ToStr(), // 7. Статус исполнения протокола
                        this.GetDate(x.ExecutionDate), // 8. Фактическая дата исполнения
                        x.TerminationReason.ToStr(), // 9. Причина отмены документа
                        this.GetDate(x.TerminationDate), // 10. Дата отмены документа
                        x.TerminationContragent.ToStr(), // 11. Организация, принявшая решение об отмене
                        x.TerminationNumber, // 12. Номер решения об отмене
                        x.TerminationInfo, // 13. Дополнительная информация
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
                case 6:
                    return row.Cells[cell.Key].IsEmpty();
                case 8:
                case 9:
                case 10: // Если статус протокола = отменен
                    return row.Cells[2] == "2" && row.Cells[cell.Key].IsEmpty();
            }
            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код",
                "Проверка",
                "Статус протокола",
                "Номер протокола",
                "Дата документа",
                "Краткая информация",
                "Статус исполнения протокола",
                "Фактическая дата исполнения",
                "Причина отмены документа",
                "Дата отмены документа",
                "Организация, принявшая решение об отмене",
                "Номер решения об отмене",
                "Дополнительная информация"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(AuditExportableEntity), typeof(ContragentExportableEntity));
        }
    }
}