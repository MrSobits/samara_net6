namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
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
    /// Оплаты ЖКУ
    /// </summary>
    public class OplataExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "OPLATA";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags => FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.Rso |
            FormatDataExportProviderFlags.RegOpCr |
            FormatDataExportProviderFlags.Ogv |
            FormatDataExportProviderFlags.Rc;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<OplataProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.KvarId.ToStr(),
                        x.OperationType.ToStr(),
                        x.OplataPackNumber,
                        this.GetDate(x.PaymentDate),
                        this.GetDate(x.OperationDate),
                        x.DocumentNumber,
                        this.GetDecimal(x.Amount),
                        x.Source,
                        this.GetDate(x.Month),
                        x.OplataPackId.ToStr(),
                        x.ContragentId.ToStr(),
                        x.ContragentRschetId.ToStr(),
                        x.EpdId.ToStr(),
                        x.PayerIndId.ToStr(),
                        x.PayerLegalId.ToStr(),
                        x.PayerName,
                        x.Destination,
                        x.Remark,
                        x.State,
                        this.GetDate(x.TerminationDate),
                        x.TerminationReason
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
                case 7:
                case 12:
                case 19:
                    return row.Cells[cell.Key].IsEmpty();
                case 20:
                case 21:
                    return row.Cells[19] == "2" && row.Cells[cell.Key].IsEmpty();
            }
            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код оплаты",
                "Уникальный код лицевого счета в системе отправителя",
                "Тип операции",
                "Уникальный идентификатор распоряжения (уникальный номер платежа)",
                "Дата оплаты",
                "Дата учета",
                "Номер распоряжения",
                "Сумма оплаты",
                "Источник оплаты",
                "Месяц, за который произведена оплата",
                "Уникальный код пачки оплат",
                "Исполнитель",
                "Расчетный счет получателя платежа",
                "Платежный документ",
                "Уникальный идентификатор плательщика (физ. лицо)",
                "Уникальный идентификатор плательщика (юр. лицо)",
                "Наименование плательщика",
                "Назначение платежа",
                "Произвольный комментарий",
                "Статус оплаты",
                "Дата аннулирования",
                "Причина аннулирования"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(KvarExportableEntity),
                typeof(ContragentRschetExportableEntity),
                typeof(EpdExportableEntity),
                typeof(IndExportableEntity),
                typeof(OplataPackExportableEntity));
        }
    }
}