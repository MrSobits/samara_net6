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

    /// <summary>
    /// Проверки
    /// </summary>
    public class AuditExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "AUDIT";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Gji |
            FormatDataExportProviderFlags.Omjk;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<AuditProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.ContragentFrguId.ToStr(),
                        x.FrguId.ToStr(),
                        x.CheckType.ToStr(),
                        x.CheckState.ToStr(),
                        x.AuditPlanId.ToStr(),
                        x.PlanNumber.ToStr(),
                        x.RegistrationNumber.ToStr(),
                        this.GetDate(x.RegistrationDate),
                        this.GetDate(x.LastAuditDate),
                        x.MustRegistered.ToStr(),
                        x.RegistrationReason.ToStr(),
                        x.AuditKind.ToStr(),
                        x.AuditForm.ToStr(),
                        x.DisposalNumber,
                        this.GetDate(x.DisposalDate),
                        x.Inspectors,
                        x.Param18,
                        x.Param19,
                        x.SubjectContragentId.ToStr(),
                        x.SubjectPhysicalId.ToStr(),
                        x.IsSubjectSmallBusiness.ToStr(),
                        x.FactActionPlace,
                        x.Param24,
                        x.NotificationState.ToStr(),
                        this.GetDate(x.NotificationDate),
                        x.NotificationMethod,
                        x.AuditReason.ToStr(),
                        x.Param29,
                        x.DependentAuditId.ToStr(),
                        x.AuditReasonPurpose,
                        x.Param32,
                        x.AuditTask,
                        this.GetDate(x.StartDate),
                        this.GetDate(x.EndDate),
                        x.WorkDaysCount.ToStr(),
                        x.WorkHoursCount.ToStr(),
                        x.AuditCompanion,
                        x.ProsecutorAgreed.ToStr(),
                        x.IsAgreed.ToStr(),
                        x.AgreedorderNumber,
                        this.GetDate(x.AgreedorderDate),
                        x.Param43,
                        x.Param44,
                        x.Param45,
                        x.Param46,
                        x.Param47,
                        x.Param48,
                        x.Param49,
                        x.Param50,
                        x.Param51,
                        x.Param52,
                        x.Param53,
                        x.Param54,
                        x.Param55,
                        x.Param56,
                        x.Param57,
                        x.Param58
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
                case 10:
                case 12:
                case 13:
                case 21:
                case 24:
                case 30:
                case 32:
                case 33:
                case 38:
                case 39:
                case 40:
                case 41:
                case 48:
                case 49:
                case 53:
                case 54:
                case 57:
                    return row.Cells[cell.Key].IsEmpty();
                case 5:
                case 8:
                    if (row.Cells[3] == "1") // если проверка плановая
                    {
                        return row.Cells[cell.Key].IsEmpty();
                    }
                    break;
                case 19:
                case 20:
                    return row.Cells[19].IsEmpty() && row.Cells[20].IsEmpty();
                case 22:
                    return row.Cells[20].IsNotEmpty() && row.Cells[22].IsEmpty();
                case 25:
                case 26:
                    if (row.Cells[24] == "3") // если уведомление отправлено
                    {
                        return row.Cells[cell.Key].IsEmpty();
                    }
                    break;
                case 35:
                case 36:
                    return row.Cells[35].IsEmpty() && row.Cells[36].IsEmpty();
            }
            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код проверки",
                "Контролирующий орган",
                "Уникальный идентификатор функции контролирующего органа ",
                "Вид проверки ",
                "Статус проверки",
                "План проверок",
                "Порядковый номер в плане",
                "Учётный номер проверки в едином реестре проверок",
                "Дата присвоения учётного номера проверки",
                "Дата окончания последней проверки",
                "Проверка должна быть зарегистрирована в едином реестре проверок",
                "Основание регистрации проверки в едином реестре проверок",
                "Вид осуществления контрольной деятельности",
                "Форма проведения проверки",
                "Номер распоряжения (приказа)",
                "Дата утверждения распоряжения (приказа)",
                "ФИО и должностных лиц, уполномоченных на проведение проверки",
                "ФИО и должность экспертов, привлекаемых к проведению проверки",
                "Тип внеплановой проверки",
                "Субъект проверки – ЮЛ,ИП",
                "Субъект проверки - Гражданин",
                "Субъект проверки является субъектом малого предпринимательства",
                "Место фактического осуществления деятельности",
                "Другие физ. лица, в отношении которых проводится проверка",

                // Информация об уведомлении
                "Статус уведомления",
                "Дата уведомления",
                "Способ уведомления о проведении проверки",

                // Сведения о проверке
                "Основание проведения проверки",
                "Предписание, на основании которого проводится проверка",
                "Связанная проверка",
                "Цель проведения проверки с реквизитами документов основания",
                "Дополнительная информация об основаниях проведения проверки",
                "Задачи проведения проверки",
                "Дата начала проведения проверки",
                "Дата окончания проведения проверки",
                "Срок проведения проверки-Рабочих дней",
                "Срок проведения проверки-Рабочих часов",
                "Орган государственного надзора (контроля) и/или орган муниципального контроля, с которым проверка проводится совместно",
                "Наличие информации о согласовании проверки с органами прокуратуры",

                // Информация о согласовании проведении проверки с органами прокуратуры
                "Проверка согласована",
                "Номер приказа о согласовании(отказе) в проведении проверки",
                "Дата приказа о согласовании(отказе) в проведении проверки",
                "Дата вынесения решения о согласовании (отказе) в проведении проверки",
                "Место вынесения решения о согласовании (отказе) в проведении проверки",
                "Должность подписанта",
                "ФИО подписанта",
                "Дополнительная информация о проверке",
                "Причины невозможности проведения проверки",

                // Информация об изменении проверки
                "Причина изменения проверки",
                "Дата изменения проверки",
                "Номер решения об изменении проверки",
                "Дополнительная информация об изменения проверки",
                "Организация, принявшая решение об изменении проверки",

                // Информация об отмене проверки
                "Причина отмены проверки",
                "Дата отмены проверки",
                "Номер решения об отмене проверки",
                "Дополнительная информация об отмене проверки",
                "Организация, принявшая решение об отмене проверки" 
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            var inheritedEntityCodeList = this.GetEntityCodeList(typeof(GjiExportableEntity),
                typeof(FrguFuncExportableEntity),
                typeof(AuditPlanExportableEntity),
                typeof(ContragentExportableEntity));

            inheritedEntityCodeList.Add("IND");

            return inheritedEntityCodeList;
        }
    }
}