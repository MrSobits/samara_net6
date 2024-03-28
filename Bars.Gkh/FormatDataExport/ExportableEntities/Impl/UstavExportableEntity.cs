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
    /// Уставы
    /// </summary>
    public class UstavExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "USTAV";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<UstavProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x,
                new List<string>
                {
                    x.Id.ToStr(),
                    x.ContragentId.ToStr(),
                    x.DocumentNumber.Cut(255),
                    x.IsTimingInfoExists.ToStr(),
                    x.IsInputMeteringDeviceValuesLastDay.ToStr(),
                    x.InputMeteringDeviceValuesBeginDay.ToStr(),
                    x.ThisMonthInputMeteringDeviceValuesBeginDate.ToStr(),
                    x.IsLastDayMeteringDeviceValuesBeginDate.ToStr(),
                    x.InputMeteringDeviceValuesEndDay.ToStr(),
                    x.ThisMonthInputMeteringDeviceValuesEndDate.ToStr(),
                    x.IsDrawingPaymentDocumentLastDay.ToStr(),
                    x.DrawingPaymentDocumentDay.ToStr(),
                    x.ThisMonthPaymentDocDate.ToStr(),
                    x.IsPaymentServicePeriodLastDay.ToStr(),
                    x.PaymentServicePeriodDay.ToStr(),
                    x.ThisMonthPaymentServiceDate.ToStr(),
                    x.Status.ToStr(),
                    x.TerminateReason.Cut(1000),
                    this.GetDate(x.TerminationDate),
                    x.ContractStopReason.Cut(255),
                    x.IsFormingApplications.ToStr(),
                    x.IsProtocolContainsDecision.ToStr()
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
                case 7:
                case 9:
                case 10:
                case 12:
                case 13:
                case 15:
                case 16:
                case 21:
                    return row.Cells[cell.Key].IsEmpty();
                case 5:
                case 8:
                case 11:
                case 14:
                    return row.Cells[cell.Key].IsEmpty() && row.Cells[cell.Key - 1] == "2";
                case 17:
                    return row.Cells[cell.Key].IsEmpty() && row.Cells[16] == "4"; // Аннулирован
                case 18:
                case 19:
                    return row.Cells[cell.Key].IsEmpty() && row.Cells[16] == "3"; // Расторгнут
            }
            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код ",
                "Уникальный идентификатор Управляющей организации/ТСЖ",
                "Номер документа",
                "Наличие сведений о сроках по уставу",
                "Ввод показаний по ПУ начинается в последний день месяца",
                "День месяца начала ввода показаний по ПУ",
                "Месяц начала ввода показаний",
                "Ввод показаний заканчивается в последний день месяца",
                "День месяца окончания ввода показаний по ПУ",
                "Месяц окончания ввода показаний",
                "Платежный документ выставляется в последний день месяца",
                "День месяца выставления платежных документов",
                "Месяц выставления платежных документов",
                "Оплата за ЖКУ должна быть внесена в последний день месяца",
                "День месяца внесения платы за ЖКУ",
                "Месяц внесения платы за ЖКУ",
                "Статус устава",
                "Причина аннулирования",
                "Дата расторжения, прекращения действия устава",
                "Причина прекращения действия устава (расторжения)",
                "Формировать заявки в реестр лицензий, если сведения о лицензии/управляемом объекте отсутствуют",
                "Протокол, содержащий решение об утверждении устава отсутствует"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(UoExportableEntity));
        }
    }
}
