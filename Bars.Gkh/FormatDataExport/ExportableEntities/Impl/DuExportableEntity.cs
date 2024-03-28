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
    /// Договоры управления многоквартирным домом
    /// </summary>
    public class DuExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "DU";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.Oms |
            FormatDataExportProviderFlags.RegOpWaste;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<DuProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.ContragentId.ToStr(),
                        x.DocumentNumber.Cut(255),
                        this.GetDate(x.DocumentDate),
                        this.GetDate(x.StartDate),
                        this.GetDate(x.PlannedEndDate),
                        x.ValidityMonths.ToStr(),
                        x.ValidityYear.ToStr(),
                        x.NoticeLink.Cut(1000),
                        x.ContragentOwnerType.ToStr(),
                        x.ContragentOwnerId.ToStr(),
                        x.ContractFoundation.ToStr(),
                        x.IsTimingInfoExists ? this.Yes : this.No,
                        x.IsInputMeteringDeviceValuesBeginLastDay.ToStr(),
                        x.InputMeteringDeviceValuesBeginDay.ToStr(),
                        x.InputMeteringDeviceValuesBeginMonth.ToStr(),
                        x.IsInputMeteringDeviceValuesEndLastDay.ToStr(),
                        x.InputMeteringDeviceValuesEndDay.ToStr(),
                        x.InputMeteringDeviceValuesEndMonth.ToStr(),
                        x.IsDrawingPaymentDocumentLastDay.ToStr(),
                        x.DrawingPaymentDocumentDay.ToStr(),
                        x.DrawingPaymentDocumentMonth.ToStr(),
                        x.IsPaymentTermLastDay.ToStr(),
                        x.PaymentTermDay.ToStr(),
                        x.PaymentTermMonth.ToStr(),
                        x.Status.ToStr(),
                        this.GetDate(x.TerminationDate),
                        x.TerminationReason.ToStr(),
                        x.CancellationReason.Cut(1000),
                        x.IsFormingApplications.ToStr(),
                        x.ProtocolTransmittingMethod.ToStr(),
                        x.NoticeNumber.Cut(20),
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
                case 9:
                case 11:
                case 12:
                case 13:
                case 15:
                case 16:
                case 18:
                case 19:
                case 21:
                case 22:
                case 24:
                case 25:
                    return row.Cells[cell.Key].IsEmpty();
                case 10:
                    return row.Cells[cell.Key].IsEmpty() && (row.Cells[9] == "2" || row.Cells[9] == "3" || row.Cells[9] == "4");
                case 14:
                case 17:
                case 20:
                case 23:
                    return row.Cells[cell.Key].IsEmpty() && row.Cells[cell.Key - 1] == "2";
                case 27:
                    return row.Cells[cell.Key].IsEmpty() && row.Cells[25] == "3"; // Расторгнут
                case 28:
                    return row.Cells[cell.Key].IsEmpty() && row.Cells[25] == "4"; // Аннулирован
            }
            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код",
                "Уникальный идентификатор Управляющей организации",
                "Номер документа",
                "Дата заключения",
                "Дата вступления в силу",
                "Планируемая дата окончания",
                "Срок действия (Месяц)",
                "Срок действия (Год/лет)",
                "Ссылка на извещение на официальном сайте в сети «Интернет» для размещения информации о проведении торгов",
                "Тип второй стороны договора",
                "Контрагент второй стороны договора",
                "Основание заключения договора",
                "Наличие сведений о сроках по договору управления",
                "Ввод показаний по ПУ начинается в последний день месяца",
                "День месяца начала ввода показаний по ПУ",
                "Месяц начала ввода показаний",
                "Ввод показаний заканчивается в последний день месяца",
                "День месяца окончания ввода показаний по ПУ",
                "Месяц окончания ввода показаний",
                "Платежный документ выставляется в последний день месяца",
                "День месяца выставления платежных документов",
                "Месяц выставления платежных документов",
                "Последним днем внесения платы за ЖКУ является последний день месяца",
                "День месяца внесения платы за ЖКУ",
                "Месяц внесения платы за ЖКУ",
                "Статус ДУ",
                "Дата расторжения, прекращения действия договора управления",
                "Причина расторжения договора",
                "Причина аннулирования",
                "Формировать заявки в реестр лицензий, если сведения о лицензии/управляемом объекте отсутствуют",
                "Способ передачи протокола голосования собрания собственников/открытого конкурса",
                "Номер извещения"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(UoExportableEntity),
                typeof(ContragentExportableEntity));
        }
    }
}