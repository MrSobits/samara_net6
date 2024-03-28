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
    /// Результаты проверки
    /// </summary>
    public class AuditResultExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "AUDITRESULT";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Gji |
            FormatDataExportProviderFlags.Omjk;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<AuditResultProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x,
                    new List<string>
                    {
                        x.AuditId.ToStr(),
                        x.State.ToStr(),
                        x.DocumentKind.ToStr(),
                        x.DocumentNumber.ToStr(),
                        this.GetDate(x.DocumentDate),
                        x.Result.ToStr(),
                        x.Violations,
                        x.Param8,
                        x.ActViolations,
                        x.Param10,
                        x.Param11,
                        x.Param12,
                        x.Param13,
                        x.Param14,
                        x.Param15,
                        x.Param16,
                        x.Param17,
                        this.GetDate(x.StartDate),
                        this.GetDate(x.EndDate),
                        x.DayCount.ToStr(),
                        x.HourCount.ToStr(),
                        x.PlaceAddress,
                        x.Inspectors,
                        x.Respondents,
                        x.CreateDocPlaceAddress,
                        x.Param26,
                        x.ReviewState.ToStr(),
                        x.RefusedRespondents,
                        this.GetDate(x.ReiviewDate),
                        x.SignExists.ToStr(),
                        x.ConcentRespondents,
                        x.Param32,
                        x.Param33,
                        x.Param34,
                        x.Param35,
                        x.Param36
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
                case 5:
                case 17:
                case 18:
                case 21:
                case 22:
                case 23:
                case 24:
                case 26:
                case 27:
                case 28:
                case 30:
                case 31:
                case 32:
                case 34:
                    return row.Cells[cell.Key].IsEmpty();
                case 19:
                case 20:
                    return row.Cells[19].IsEmpty() && row.Cells[20].IsEmpty();
            }
            return false;
        };
       
        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Проверка",
                "Статус результата проверки",
                "Вид документа результата проверки",
                "Номер документа результата проверки",
                "Дата составления документа результата проверки",
                "Результат проверки",
                "Характер нарушения",
                "Несоответствие поданных сведений о начале осуществления предпринимательской деятельности",
                "Положение нарушенного правового акта",
                "Другие несоответствия поданных сведений",
                "Список лиц допустивших нарушение",
                "Орган, в который отправлены материалы о выявленных нарушениях",
                "Дата отправления материалов в ОГВ",
                "Перечень применённых мер обеспечения производства по делу об административном правонарушении",
                "Информация о привлечении проверяемых лиц к административной ответственности",
                "Информация об аннулировании или приостановлении документов, имеющих разрешительный характер",
                "Информация об обжаловании решений органа контроля",
                "Дата начала проведения проверки",
                "Дата окончания проведения проверки",
                "Продолжительность проведения проверки (дней)",
                "Продолжительность проведения проверки (часов)",
                "Место проведения проверки",
                "ФИО и должность лиц, проводивших проверку",
                "ФИО и должность представителей субъекта проверки",
                "Место составления документа результата проверки",
                "Дополнительная информация о результате проверки",
                "Статус ознакомления с результатами проверки",
                "ФИО должностного лица, отказавшегося от ознакомления с актом проверки",
                "Дата ознакомления",
                "Наличие подписи",
                "ФИО должностного лица, ознакомившегося с актом проверки",
                "Причина отмены результата проверки",
                "Дата отмены результата проверки",
                "Номер решения об отмене результата проверки",
                "Организация, принявшая решение об отмене результата проверки",
                "Дополнительная информация об отмене результата проверки"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(AuditPlanExportableEntity));
        }
    }
}