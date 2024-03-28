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
    /// Платежные документы (epd.csv)
    /// </summary>
    public class EpdExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "EPD";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.Rso |
            FormatDataExportProviderFlags.RegOpCr |
            FormatDataExportProviderFlags.Ogv |
            FormatDataExportProviderFlags.Rc;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<EpdProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.DocumentType,
                        this.GetDate(x.ReportPeriod),
                        x.DocNumber,
                        this.GetDate(x.Date),
                        x.AccountId.ToStr(),
                        x.ContragentRschetId.ToStr(),
                        x.ResidentCount.ToStr(),
                        this.GetDecimal(x.LivingArea),
                        this.GetDecimal(x.HeatedArea),
                        this.GetDecimal(x.TotalArea),
                        x.StateFlag,
                        this.GetDecimal(x.CalcPeriodDebt),
                        this.GetDecimal(x.PreviousPeriodDebtDebt),
                        this.GetDecimal(x.Overpayment),
                        this.GetDecimal(x.TotalCharge),
                        this.GetDate(x.PaymentsBeforeDate),
                        x.Param18,
                        this.GetDecimal(x.TotalPayment),
                        this.GetDecimal(x.TotalPenaltyPayment),
                        this.GetDecimal(x.AllTotalPayment),
                        this.GetDecimal(x.Paid),
                        this.GetDate(x.LastPaymentDate)
                    }))
                .ToList();
        }

        /// <inheritdoc />
        protected override IList<int> MandatoryFields => new List<int> { 0, 1, 2, 4, 5, 6, 11 };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код платежного документа",
                "Тип платежного документа",
                "Отчетный период",
                "Номер платежного документа",
                "Дата формирования платежного документа",
                "Лицевой счет",
                "Идентификатор расчетного счета получателя платежа",
                "Количество проживающих",
                "Жилая площадь",
                "Отапливаемая площадь",
                "Общая площадь для ЛС",
                "Статус платежного документа",
                "Сумма к оплате за расчетный период по услугам, руб. (по всем услугам за расчетный период)",
                "Задолженность за предыдущие периоды, руб.",
                "Аванс на начало расчетного периода, руб.",
                "Сумма к оплате с учетом рассрочки платежа и процентов за рассрочку, руб.",
                "В документе учтены оплаты, поступившие до:",
                "Дополнительная информация",
                "Итого к оплате за расчетный период с учетом задолженности/переплаты, руб. (по всему платежному документу)",
                "Итого к оплате по неустойкам и судебным издержкам, руб. (итог по всем неустойкам и судебным издержкам)",
                "Итого к оплате за расчетный период всего, руб. (по всему платежному документу)",
                "Оплачено денежных средств, руб.",
                "Дата последней поступившей оплаты"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(KvarExportableEntity),
                typeof(ContragentRschetExportableEntity));
        }
    }
}