namespace Bars.GkhGji.FormatDataExport.ExportableEntities.Impl
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
    /// Планы проверок юр. лиц ГЖИ
    /// </summary>
    public class AuditPlanExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "AUDITPLAN";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Gji |
            FormatDataExportProviderFlags.Omjk;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<AuditPlanProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x,
                    new List<string>
                    {
                        x.Id.ToStr(), // 1. Уникальный код
                        x.IsPlanSigned.ToStr(), // 2. Признак подписания плана проверок для публикации в ГИС ЖКХ.
                        x.ContragentInspectorId.ToStr(), // 3. Проверяющая организация
                        x.PlanYear.ToStr(), // 4. Год плана проверок
                        this.GetDate(x.AcceptPlanDate), // 5. Дата утверждения плана проверок
                        x.AdditionalInfo, // 6. Дополнительная информация
                        x.State.ToStr(), // 7. Статус плана (Передавать 1)
                        x.IsNotRegistred.ToStr(), // 8. Не должен быть зарегистрирован в едином реестре проверок
                        x.RegistrationNumber.ToStr() // 9. Регистрационный номер плана в едином реестре проверок
                    }))
                .ToList();
        }

        /// <inheritdoc />
        protected override Func<KeyValuePair<int, string>, ExportableRow, bool> EmptyFieldPredicate { get; } = (cell, row) =>
        {
            switch (cell.Key)
            {
                case 0:
                case 2:
                case 3:
                case 4:
                case 6:
                case 7:
                    return row.Cells[cell.Key].IsEmpty();
                case 8:
                    return row.Cells[7] == "2" && row.Cells[cell.Key].IsEmpty();
            }

            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код",
                "Признак подписания плана проверок для публикации в ГИС ЖКХ.",
                "Проверяющая организация",
                "Год плана проверок",
                "Дата утверждения плана проверок",
                "Дополнительная информация",
                "Статус плана",
                "Не должен быть зарегистрирован в едином реестре проверок",
                "Регистрационный номер плана в едином реестре проверок"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(GjiExportableEntity));
        }
    }
}