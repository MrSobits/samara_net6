namespace Bars.GkhGji.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Impl;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Предписание проверки ГЖИ
    /// </summary>
    public class PreceptAuditExportableEntity : BaseExportableEntity
    {  
        /// <inheritdoc />
        public override string Code => "PRECEPTAUDIT";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Gji |
            FormatDataExportProviderFlags.Omjk;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<PreceptAuditProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(), // 1. Уникальный код
                        x.AuditId.ToStr(), // 2. Проверка
                        x.State.ToStr(), // 3. Статус протокола
                        x.DocumentNumber.Cut(255), // 4. Номер документа
                        this.GetDate(x.DocumentDate), // 5. Дата документа
                        this.GetDate(x.PlanExecutionDate), // 6. Срок исполнения требований
                        x.Info.Cut(2000), // 7. Краткая информация
                        x.ExecutionState.ToStr(), // 8. Статус исполнения протокола
                        this.GetDate(x.ExecutionDate), // 9. Фактическая дата исполнения
                        x.TerminationReason.ToStr(), // 10. Причина отмены документа
                        this.GetDate(x.TerminationDate), // 11. Дата отмены документа
                        x.TerminationContragent.ToStr(), // 12. Организация, принявшая решение об отмене
                        x.TerminationNumber, // 13. Номер решения об отмене
                        x.TerminationInfo, // 14. Дополнительная информация
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
                case 5:
                case 7:
                    return row.Cells[cell.Key].IsEmpty();
                case 9:
                case 10:
                case 11: // Если статус предписания = отменено
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
                "Статус предписания",
                "Номер документа",
                "Дата документа",
                "Срок исполнения требований",
                "Краткая информация",
                "Статус исполнения предписания",
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